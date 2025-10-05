# Copyright (c) HashiCorp, Inc.
# SPDX-License-Identifier: MPL-2.0

variable "project_id" {
  description = "project id"
}

variable "region" {
  description = "zone"
}

provider "google" {
  project = var.project_id
  zone    = var.zone
}

# VPC Creation
resource "google_compute_network" "vpc" {
  name                    = "${var.project_id}-vpc"
  auto_create_subnetworks = "false"
}

# Subnet
resource "google_compute_subnetwork" "subnet" {
  name          = "${var.project_id}-subnet"
  region        = var.region
  network       = google_compute_network.vpc.name
  ip_cidr_range = "10.10.0.0/24"
}


#
# Default VPC network & subnetwork on GCP
#
#data "google_compute_network" default_network{
#  name    = "default"
#  project = "${var.project_id}"
#}


# Subnet
#data "google_compute_subnetwork" "default_subnetwork" { 
#  name          = "${var.project_id}-subnet"
#  name          = "default"
#  region        = var.region
#  network       = data.google_compute_network.default_network.name
#  ip_cidr_range = "10.10.0.0/24"
#  ip_cidr_range = "172.18.0.0/24"
#}
