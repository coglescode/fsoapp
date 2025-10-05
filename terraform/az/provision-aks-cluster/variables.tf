# Copyright (c) HashiCorp, Inc.
# SPDX-License-Identifier: MPL-2.0

variable "appId" {
  description = "Azure Kubernetes Service Cluster service principal"
}

variable "password" {
  description = "Azure Kubernetes Service Cluster password"
}

variable "location" {
  description = "Default region for current subscription"
}

variable "rg" {
  description = "Name of my resource group"
}

variable "cluster-name" {
  description = "Name of the cluster"
}

variable "subscription_id" {
  description = "Subscription id value"
}

variable "tenant_id" {
  description = "Tennat id value"
}