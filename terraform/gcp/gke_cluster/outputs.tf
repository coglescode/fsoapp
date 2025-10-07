# Copyright (c) HashiCorp, Inc.
# SPDX-License-Identifier: MPL-2.0

output "zone" {
  value       = var.zone
  description = "GCloud Region"
}

output "project_id" {
  value       = var.project_id
  description = "GCloud Project ID"
}

output "kubernetes_cluster_name" {
  value       = google_container_cluster.cluster-1.name
  description = "GKE Cluster Name"
}

output "kubernetes_cluster_host" {
  value       = google_container_cluster.cluster-1.endpoint
  description = "GKE Cluster Host"
}

output "container_version" {
  value = data.google_container_engine_versions.gke_version.release_channel_default_version
}
 