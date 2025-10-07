terraform {
  required_providers {
    google = {
      source  = "hashicorp/google"
      version = "6.8.0"
    }
    kubernetes = {
      source  = "hashicorp/kubernetes"
      version = ">= 2.0.1"
    }
  }
}

data "terraform_remote_state" "gke" {
  backend = "local"

  config = {
    path = "../gke_cluster/terraform.tfstate"
  }
}

# Retrieve GKE cluster information
provider "google" {
  project = data.terraform_remote_state.gke.outputs.project_id
  region  = data.terraform_remote_state.gke.outputs.zone
}

# Configure kubernetes provider with Oauth2 access token.
# https://registry.terraform.io/providers/hashicorp/google/latest/docs/data-sources/client_config
# This fetches a new token, which will expire in 1 hour.
data "google_client_config" "default" {}

data "google_container_cluster" "my_cluster" {
  name     = data.terraform_remote_state.gke.outputs.kubernetes_cluster_name
  location = data.terraform_remote_state.gke.outputs.zone
}

provider "kubernetes" {
  #host = data.terraform_remote_state.gke.outputs.kubernetes_cluster_host
  #token                  = data.google_client_config.default.access_token
  cluster_ca_certificate = base64decode(data.google_container_cluster.my_cluster.master_auth[0].cluster_ca_certificate)
  config_path     = "~/.kube/config"
  config_context  = "<YOUR_CONTEXT_HERE>"
#  cluster_ca_certificate = google_container_cluster.primary.master_auth.0.cluster_ca_certificate
}

# FSO_API container
resource "kubernetes_deployment" "fso-api" {
  metadata {
    name = "scalable-fso-api"
    labels = {
      App = "ScalableFSO_API"
    }
  }

  spec {
    replicas = 2
    selector {
      match_labels = {
        App = "ScalableFSO_API"
      }
    }
    template {
      metadata {
        labels = {
          App = "ScalableFSO_API"
        }
      }
      spec {
        container {
          image = "coglescode/fsoapi"
          name  = "fsoapi"

          port {
            container_port = 8080
          }  

          env {
            name  = "CONNECTION_STRING"
            value = "<YOUR_CONNECTION_STRING>" 
          }
        }
      }
    }
  }
}

resource "kubernetes_service" "api_service" {
  metadata {
    name = "scalable-fso-api"
  }
  spec {
    selector = {
      App = kubernetes_deployment.fso-api.spec.0.template.0.metadata[0].labels.App
    }
    port {
      port        = 5046  # Node port. Is the external port, exposed to the internet.  
      target_port = 8080  # Same as the container_port
    }

    type = "LoadBalancer"
  }
}

# FSO-Client

resource "kubernetes_deployment" "fso_client" {
  metadata {
    name = "scalable-fso-client"
    labels = {
      App = "ScalableFSO_Client"
    }
  }

  spec {
    replicas = 2
    selector {
      match_labels = {
        App = "ScalableFSO_Client"
      }
    }
    template {
      metadata {
        labels = {
          App = "ScalableFSO_Client"
        }
      }
      spec {
        container {
          image = "coglescode/fsoclient"
          name  = "fsoclient"

          port {
            container_port = 8080
          }   

          env {
            name  = "MembersEndpointUrl"
            value = "http://${kubernetes_service.api_service.status.0.load_balancer.0.ingress.0.ip}:5046/api/members"
          }
              
        }
      }
    }
  }
}

resource "kubernetes_service" "fso_client_service" {
  metadata {
    name = "scalable-fso-client"
  }
  spec {
    selector = {
      App = kubernetes_deployment.fso_client.spec.0.template.0.metadata[0].labels.App
    }
    port {
      port        = 80
      target_port = 8080
    }    

    type = "LoadBalancer"
  }
}

output "lb_ip" {
  value = kubernetes_service.fso_client_service.status.0.load_balancer.0.ingress.0.ip
}



#resource "kubernetes_env" "apiendpoint" {
#  container = "fso-client"
#  metadata {
#    name  = "fso-api-deployment"
#  }
#
#  api_version = "app/v1"
#  kind        = "Deployment"
#
#  env {
#    name  = "ApiEndpointUrl"
#    value = "${kubernetes_service.api_service.status.0.load_balancer.0.ingress.0.ip}"
#  }
#}

output "api_ip" {
  value = kubernetes_service.api_service.status.0.load_balancer.0.ingress.0.ip
}
