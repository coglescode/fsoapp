terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = ">= 4.48.0"
    }

    kubernetes = {
      source  = "hashicorp/kubernetes"
      version = ">= 2.16.1"
    }
  }
}

data "terraform_remote_state" "eks" {
  backend = "local"

  config = {
    path = "../terraform-provision-eks-cluster/terraform.tfstate"
  }
}

# Retrieve EKS cluster information
provider "aws" {
  region = data.terraform_remote_state.eks.outputs.region
}

data "aws_eks_cluster" "cluster" {
  name = data.terraform_remote_state.eks.outputs.cluster_name
}

#data "aws_eks_node_group" "fso_nodes" {
#  cluster_name        = data.aws_eks_cluster.cluster.name
  #node_group_name    = "<NODE_GROUP_NAME>"
#  
#}

provider "kubernetes" {
  #host                   = data.aws_eks_cluster.cluster.endpoint
  cluster_ca_certificate = base64decode(data.aws_eks_cluster.cluster.certificate_authority.0.data)
  config_path     = "~/.kube/config"
  config_context  = "<YOUR_CONTEXT_HERE>"


  #exec {
  #  api_version = "client.authentication.k8s.io/v1beta1"
  #  command     = "aws"
  #  args = [
  #    "eks",
  #    "get-token",
  #    "--cluster-name",
  #    data.aws_eks_cluster.cluster.name
  #  ]
  #}
}

#data "aws_secretsmanager_secret_version" "secret-version" {
#  secret_id = data.aws_secretsmanager_secret.CONNECTION_STRING.value
#}


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

          env {
            name  = "CONNECTION_STRING"
            value = "<CONNECTION_STRING>"
          }

          port {
            container_port = 8080
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
            value = "http://${kubernetes_service.api_service.status[0].load_balancer[0].ingress[0].hostname}:5046/api/members"
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

output "cluster_name" {
  value = data.aws_eks_cluster.cluster.name
}

output "cluster_endpoint" {
  value = data.aws_eks_cluster.cluster.endpoint
}

output "api_ip" {
 value = kubernetes_service.api_service.status[0].load_balancer[0].ingress[0].hostname
}
