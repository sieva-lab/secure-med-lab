# 🛡️ SecureMedLab API - .NET 10 Healthcare Reference Architecture

[![.NET 10](https://img.shields.io/badge/.NET-10.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/10.0)
[![Aspire Orchestration](https://img.shields.io/badge/Orchestration-Aspire-green.svg)](https://learn.microsoft.com/en-us/dotnet/aspire/)
[![Security](https://img.shields.io/badge/Security-OWASP%20Top%2010-red.svg)](#-security-implementatie-details)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

**SecureMed API** is een hoogwaardige referentie-implementatie voor een beveiligde API in de medische sector. Dit project demonstreert hoe gevoelige patiëntgegevens veilig opgeslagen en ontsloten kunnen worden met de nieuwste .NET technieken, waarbij **Security by Design** het uitgangspunt is.

---

## 🎯 Project Doelstellingen
Dit project bewijst expertise op het gebied van moderne cloud-native ontwikkeling en security-architectuur:
* **Identity Management**: Enterprise-grade flows via Keycloak (OIDC/OAuth2).
* **Data Protection**: Implementatie van Column-level encryption in MS SQL Server via EF Core.
* **Resilience & Protection**: Rate limiting en brute-force preventie op API-niveau.
* **Observability**: Diepgaand inzicht in security events en tracing via het .NET Aspire Dashboard.

---

## 🛠️ Tools & Technologieën

### **Core Stack**
* **Framework**: .NET 10 (Preview/Latest)
* **Orchestratie**: .NET Aspire (Service discovery & container management)
* **Database**: Microsoft SQL Server 2022 (Developer Edition)
* **ORM**: Entity Framework Core 10 met Encrypted Column Mapping

### **Security Tooling**
* **Identity Provider**: Keycloak (Dockerized) - Vervanger voor Azure AD/Entra ID.
* **Authentication**: JWT Bearer Tokens met volledige MFA-ondersteuning.
* **Authorization**: Fine-grained access control middels OAuth Scopes (`medical.read`, `medical.write`).
* **API Defense**: .NET `RateLimiter` middleware (Fixed & Sliding Windows).
* **Data Security**: AES-256 Encryption voor gevoelige velden en SHA-256 voor data-integriteit.

---

## 🏗️ Architectuur

Het project wordt georkestreerd door **.NET Aspire**, wat zorgt voor een naadloze samenwerking tussen de verschillende componenten:



1.  **SecureMed.AppHost**: De centrale orchestrator die de SQL Server en Keycloak containers beheert.
2.  **SecureMed.ApiService**: De kern-API waarin de medische business logica en security-policies leven.
3.  **SecureMed.ServiceDefaults**: Gecentraliseerde configuratie voor OpenTelemetry (logging, metrics, tracing) en health checks.

---

## 🚀 Getting Started

### **Prerequisites**
* [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/)
* [Visual Studio 2022 (17.12+)](https://visualstudio.microsoft.com/vs/) of JetBrains Rider

### **Installatie & Gebruik**
1.  **Clone de repository:**
    ```bash
    git clone [https://github.com/jouwgebruikersnaam/securemed-api.git](https://github.com/jouwgebruikersnaam/securemed-api.git)
    cd securemed-api
    ```

2.  **Installeer de Aspire Workload:**
    ```bash
    dotnet workload install aspire
    ```

3.  **Start het project:**
    Draai de `AppHost` vanuit je IDE of gebruik de terminal:
    ```bash
    dotnet run --project SecureMed.AppHost
    ```

4.  **Dashboard toegang:**
    Na het opstarten verschijnt er een link naar het **Aspire Dashboard**. Hier kun je de status van de database, de identity provider en de API-logs in real-time volgen.

---

## 🔒 Security Implementatie Details

### **1. Column-Level Encryption**
Gevoelige velden zoals het *Burgerservicenummer (BSN)* worden middels EF Core Value Converters versleuteld voordat ze de database bereiken. 
> **Impact:** Dit minimaliseert de schade bij een eventueel datalek; de data is onbruikbaar zonder de sleutels uit de beveiligde environment settings.

### **2. Rate Limiting & Brute Force Defense**
De API maakt gebruik van de ingebouwde .NET 10 Rate Limiting middleware.
* **Auth Endpoints**: Strikt gelimiteerd om brute-force aanvallen te vertragen.
* **Data Endpoints**: Gebruiken sliding windows om misbruik van patiëntgegevens te voorkomen.

### **3. Audit Logging**
Elke toegang tot gevoelige medische data wordt gelogd via OpenTelemetry. Dit zorgt voor een sluitende bewijslast conform de **GDPR/AVG** wetgeving.

---

## 📜 Roadmap
- [x] .NET 10 & Aspire Basis Architectuur
- [x] Keycloak OIDC Integratie (Local-first Cloud-native)
- [ ] Implementatie van EF Core Encryption Converters
- [ ] Automatische API-documentatie via OpenAPI/Swagger met OAuth2 flow
- [ ] Unit- & Integration tests voor Security Middleware

---

**Project onderhouden door [Jouw Naam]** *Dit project is ontwikkeld als onderdeel van een professioneel zelfstudietraject gericht op Advanced .NET Security.*
