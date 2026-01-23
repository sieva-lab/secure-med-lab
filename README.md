# 🛡️ SecureMed API - .NET 10 Healthcare Reference Architecture

[![.NET 10](https://img.shields.io/badge/.NET-10.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/10.0)
[![Aspire Orchestration](https://img.shields.io/badge/Orchestration-Aspire-green.svg)](https://learn.microsoft.com/en-us/dotnet/aspire/)
[![Security](https://img.shields.io/badge/Security-OWASP%20Top%2010-red.svg)](#-security-implementation-details)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

**SecureMed API** is a self-study lab focused on Advanced .NET Security.
It is a high-grade reference implementation for a secured API within the healthcare sector. 
This project demonstrates how sensitive patient data can be safely stored and accessed using the latest .NET 10 features, with **Security by Design** as the core principle.

---

## 🎯 Project Objectives
This project showcases expertise in modern cloud-native development and security architecture:
* **Identity Management**: Enterprise-grade flows via Keycloak (OIDC/OAuth2) as a local-first alternative to Azure AD.
* **Data Protection**: Implementation of Column-level encryption in MS SQL Server via EF Core.
* **Resilience & Protection**: Advanced Rate Limiting and brute-force prevention at the API level.
* **Observability**: Deep insights into security events, metrics, and distributed tracing via the .NET Aspire Dashboard.

---

## 🛠️ Tools & Technologies

### **Core Stack**
* **Framework**: .NET 10 (Preview/Latest)
* **Orchestration**: .NET Aspire (Service discovery & container management)
* **Database**: Microsoft SQL Server 2022 (Developer Edition)
* **ORM**: Entity Framework Core 10 with Encrypted Column Mapping

### **Security Tooling**
* **Identity Provider**: Keycloak (Dockerized) - Implementing OIDC/OAuth2 protocols.
* **Authentication**: JWT Bearer Tokens with full MFA (Multi-Factor Authentication) support.
* **Authorization**: Fine-grained access control using OAuth Scopes (`medical.read`, `medical.write`).
* **API Defense**: .NET `RateLimiter` middleware (Fixed & Sliding Windows).
* **Data Security**: AES-256 Encryption for sensitive fields and SHA-256 for data integrity.

---

## 🏗️ Architecture

The project is orchestrated by **.NET Aspire**, ensuring seamless integration between components:



1.  **SecureMed.AppHost**: The central orchestrator managing SQL Server and Keycloak containers.
2.  **SecureMed.ApiService**: The core API housing healthcare business logic and security policies.
3.  **SecureMed.ServiceDefaults**: Centralized configuration for OpenTelemetry (logging, metrics, tracing) and health checks.

---

## 🚀 Getting Started

### **Prerequisites**
* [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/)
* [Visual Studio 2022 (17.12+)](https://visualstudio.microsoft.com/vs/) or JetBrains Rider

### **Installation & Usage**
1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/yourusername/securemed-api.git](https://github.com/yourusername/securemed-api.git)
    cd securemed-api
    ```

2.  **Install the Aspire Workload:**
    ```bash
    dotnet workload install aspire
    ```

3.  **Run the project:**
    Launch the `AppHost` from your IDE or use the terminal:
    ```bash
    dotnet run --project SecureMed.AppHost
    ```

4.  **Dashboard Access:**
    Once started, follow the link to the **Aspire Dashboard** in your terminal to monitor the database, identity provider, and API logs in real-time.

---

## 🔒 Security Implementation Details

### **1. Column-Level Encryption**
Sensitive fields, such as *Social Security Numbers (SSN)*, are encrypted using EF Core Value Converters before they ever reach the database storage. 
> **Impact:** This mitigates damage in the event of a data breach; the stored data remains unreadable without the encryption keys managed in secure environment settings.

### **2. Rate Limiting & Brute Force Defense**
The API leverages built-in .NET 10 Rate Limiting middleware.
* **Auth Endpoints**: Strictly limited to slow down brute-force attempts.
* **Data Endpoints**: Utilizing sliding windows to prevent bulk data scraping.

### **3. Audit Logging & Compliance**
Every access request to sensitive medical data is logged via OpenTelemetry. This ensures a robust audit trail for **GDPR/HIPAA** compliance, allowing for transparent "who, when, and what" reporting.

---

## 📜 Roadmap
- [x] .NET 10 & Aspire Base Architecture
- [x] Keycloak OIDC Integration (Local-first Cloud-native)
- [ ] Implementation of EF Core Encryption Converters
- [ ] Automated API Documentation via OpenAPI/Swagger with OAuth2 flows
- [ ] Unit & Integration testing for Security Middleware

---
