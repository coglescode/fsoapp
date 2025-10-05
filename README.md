# 🚀 FSOApp

**Last project for YH-Utbildning Cloud Developer 2023**


## Project Description

The goal of this project is to have my own interactive digitalized calendar for family planing using a touch screen.

This repository is based on my final capstone project for the “Cloud Developer 2023” program (YH-Utbildning). With the main focus being the deployment strategy using Terraform and Kubernetes. The solution is split into multiple subprojects/modules that together form a hybrid-based, full-stack application.  

For now the project is a simple monolithic .Net MVC application designed to use a local PostgreSQL database.

## Prerequisites
Before running this project, you will need the following installed:

  - .NET SDK (for the C# API)
  - Git
  - Terraform CLI
  - Cloud Provider account (e.g., Azure, AWS, CGP) and its respective CLI. 
  - kubectl 

## Quickstart 

Try deploying the application on Azure Kubernetes Services (AKS). Assuming that you fullfiled the prerequisites. Make sure that you are logged in through the CLI. 

  ```
  cd 
  az login --tenant <YOUR_ID_HERE>
  ```
