# Copyright (c) HashiCorp, Inc.
# SPDX-License-Identifier: MPL-2.0

variable "zone" {
  description = "zone"
}

variable "gke_username" {
  default     = ""
  description = "gke username"
}


variable "gke_password" {
  default     = ""
  description = "gke password"
}

variable "gke_num_nodes" {
  default     = 1
  description = "number of gke nodes"
}

# GKE cluster
data "google_container_engine_versions" "gke_version" {
  location       = var.zone
  version_prefix = "1.27."
}

resource "google_container_cluster" "cluster-1" {
  name     = "${var.project_id}-gke"
  location = var.zone

  # We can't create a cluster with no node pool defined, but we want to only use
  # separately managed node pools. So we create the smallest possible default
  # node pool and immediately delete it.

  # remove_default_node_pool  = true
  initial_node_count  = 1
  deletion_protection = false

  network    = google_compute_network.vpc.name
  subnetwork = google_compute_subnetwork.subnet.name
}

# # Kubernetes provider
# # The Terraform Kubernetes Provider configuration below is used as a learning reference only. 
# # It references the variables and resources provisioned in this file. 
# # We recommend you put this in another file -- so you can have a more modular configuration.
# # https://learn.hashicorp.com/terraform/kubernetes/provision-gke-cluster#optional-configure-terraform-kubernetes-provider
# # To learn how to schedule deployments and services using the provider, go here: https://learn.hashicorp.com/tutorials/terraform/kubernetes-provider.

provider "kubernetes" {
  config_path     = "~/.kube/config"
  config_context  = "gke_server01-413108_europe-north1-a_server01-413108-gke"
  cluster_ca_certificate = google_container_cluster.cluster-1.master_auth.0.cluster_ca_certificate
#  cluster_ca_certificate = google_container_cluster.primary.master_auth.0.cluster_ca_certificate

}

#provider "kubernetes" {
#  load_config_file = "false"
#
#  host     = google_container_cluster.cluster-1.endpoint
#  username = var.gke_username
#  password = var.gke_password
#
#  client_certificate     = google_container_cluster.primary.master_auth.0.client_certificate
#  client_key             = google_container_cluster.primary.master_auth.0.client_key
#  cluster_ca_certificate = google_container_cluster.primary.master_auth.0.cluster_ca_certificate
#}

#gcloud container clusters get-credentials server01-413108-gke --zone europe-north1-a --project server01-413108