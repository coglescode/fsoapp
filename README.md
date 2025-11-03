# 🚀 FSOApp

**Last project for YH-Utbildning Cloud Developer 2023**


## Project Description

The goal of this project is to have your own application for family members activities planing through the week or the entire month.

This repository is based on my final capstone project for the “Cloud Developer 2023” program (YH-Utbildning). With the main focus being the deployment strategy using Terraform and Kubernetes. You will be able to implement the deployment on Azure Kubernetes Services (AKS).

For now the project is a simple monolithic .Net MVC application designed to use a local PostgreSQL database, this will be changed in the near future. 

## Prerequisites
Make sure to have this prerequisites in place:

  - Cloud Provider account (e.g., Azure, AWS, CGP) and its respective CLI also installed. 
  - Git
  - Terraform CLI  
  - kubectl
    
   

## Quickstart 

This guide goes through a kubernetes cluster set up on Azure Kubernetes Services (AKS). Assuming that you fullfiled the prerequisites. Make sure that you are logged in to your Azure account through the CLI. 

  Run the command below to log in.
  ```
   az login --tenant AZURE_TENNANT_ID_HERE
  ```
  

  ```
  cd terraform/az/provision-aks-cluster
  terraform init
  terraform validate
  terraform plan
  ```