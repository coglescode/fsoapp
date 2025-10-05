# Copyright (c) HashiCorp, Inc.
# SPDX-License-Identifier: MPL-2.0

resource "random_pet" "prefix" {}

provider "azurerm" {
  #resource_provider_registrations = "none" # This is only required when the User, Service Principal, or Identity running Terraform lacks the permissions to register Azure Resource Providers.
   features {}
   subscription_id = "${var.subscription_id}"
   tenant_id       = "${var.tenant_id}"
}

resource "azurerm_resource_group" "default" {
  name     = var.rg
  location = var.location

  tags = {
    environment = "Demo"
  }
}

resource "azurerm_kubernetes_cluster" "default" {
  name                = "${var.cluster-name}-aks"
  location            = azurerm_resource_group.default.location
  resource_group_name = azurerm_resource_group.default.name
  dns_prefix          = "${random_pet.prefix.id}-k8s"
  kubernetes_version  = "1.33.2"

  default_node_pool {
    name            = "default"
    node_count      = 2
    vm_size         = "Standard_D2s_v3"
    os_disk_size_gb = 30 
  }

  service_principal {
    client_id     = var.appId
    client_secret = var.password
  }

  role_based_access_control_enabled = true

  tags = {
    environment = "Demo"
  }
}
