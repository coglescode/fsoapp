terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=4.1.0"
    }
    kubernetes = {
      source  = "hashicorp/kubernetes"
      version = ">= 2.0.1"
    }
  }
}

data "terraform_remote_state" "aks" {
  backend = "local"

  config = {
    path = "../provision-aks-cluster/terraform.tfstate"
  }
}

# Retrieve AKS cluster information
provider "azurerm" {
  features {}
  subscription_id = "${var.subscription_id}"
}

data "azurerm_kubernetes_cluster" "cluster" {
  name                = data.terraform_remote_state.aks.outputs.kubernetes_cluster_name
  resource_group_name = data.terraform_remote_state.aks.outputs.resource_group_name  
}

provider "kubernetes" {
  host = data.azurerm_kubernetes_cluster.cluster.kube_config.0.host

  client_certificate     = base64decode(data.azurerm_kubernetes_cluster.cluster.kube_config.0.client_certificate)
  client_key             = base64decode(data.azurerm_kubernetes_cluster.cluster.kube_config.0.client_key)
  #cluster_ca_certificate = base64decode(data.azurerm_kubernetes_cluster.cluster.kube_config.0.cluster_ca_certificate)

  config_path    = "~/.kube/config"
  config_context = "fsoapp-aks"
}

# Container set of the application and PostgreSQL
# FSOAPP container
resource "kubernetes_deployment" "fsoapp" {
  metadata {
    name = "scalable-fso-app"
    labels = {
      App = "ScalableFSO_APP"
    }
  }

  spec {
    replicas = 2
    selector {
      match_labels = {
        App = "ScalableFSO_APP"
      }
    }
    template {
      metadata {
        labels = {
          App = "ScalableFSO_APP"
        }
      }
      spec {
        container {
          image = "coglescode/fso-app:dev"
          name  = "fsoapp"

          port {
            container_port = 8080
          }  
     
        }
      }
    }
  }
}

resource "kubernetes_service" "fsoappp_service" {
  metadata {
    name = "scalable-fso-app-service"
  }
  spec {
    selector = {
      App = kubernetes_deployment.fsoapp.spec.0.template.0.metadata[0].labels.App
    }
    port {
      port        = 80  # Node port. Is the external port, exposed to the internet.  
      target_port = 8080  # Same as the container_port
    }

    type = "LoadBalancer"
  }
}

output "lb_ip" {
  value = kubernetes_service.fsoappp_service.status.0.load_balancer.0.ingress.0.ip
}
