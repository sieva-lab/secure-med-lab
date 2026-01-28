# 🛡️ SecureMed Lab - Enterprise Healthcare Platform

This is a personal lab where I'm practicing how to build a secure healthcare system by connecting a .NET 10 API and Angular frontend to a SQL database and dbt pipeline, all running in Docker.

**A Next-Generation Hospital Data & Management Solution | .NET 10 Aspire | Angular | dbt**

SecureMed is a high-performance, **Security-by-Design** reference architecture designed for hospitals and large-scale medical group practices. It addresses the critical challenge of balancing real-time patient care (Operational) with deep clinical insights (Analytical), all while maintaining strict GDPR and HIPAA compliance in a local-first, sovereign environment.

## 🏥 Clinical Value Proposition

In a modern medical environment, data silos and security breaches are the primary risks. SecureMed solves this by:

* **Zero-Trust Patient Records:** Ensuring that sensitive medical data is encrypted at the cellular level within the database.
* **Unified Provider Experience:** A seamless Angular-based portal where clinicians see real-time data and administrators see analytical trends.
* **Sovereign Data Control:** A fully containerized stack that runs on-premise, ensuring patient data never leaves the hospital's controlled infrastructure.

---

## 🛠️ The Medical Tech Stack

### Operational Core (The "Doctor's Office")

* **Backend:** .NET 10 Web API (High-concurrency medical record processing).
* **Frontend:** **Angular 19** Clinical Portal (Real-time vitals, patient history, and appointment scheduling).
* **Identity:** **Keycloak** (OIDC) with Multi-Factor Authentication (MFA) for medical staff login.
* **Database:** MS SQL Server 2022 (Developer Edition) featuring **AES-256 Column-Level Encryption** for PII (Patient Identifiable Information).

### Analytical Core (The "Research & Management Lab")

* **Data Transformation:** **dbt (data build tool)**. Automates the conversion of raw clinical logs into structured "Gold" tables for hospital management.
* **Orchestration:** **.NET Aspire**. Manages the entire hospital digital infrastructure as a single, observable unit.

---

## 🏗️ Hospital Platform Architecture

The ecosystem is divided into specialized zones managed by .NET Aspire:

1. **SecureMed.AppHost:** The "Digital Command Center" managing hospital databases and identity services.
2. **SecureMed.Portal (Angular):** The secure interface for doctors and nurses.
3. **SecureMed.ClinicalAPI:** The secure gateway for Electronic Health Records (EHR).
4. **SecureMed.Analytics (dbt):** The transformation engine that prepares data for clinical research and capacity planning.

---

## 🚀 Deployment (Local Hospital Environment)

### Prerequisites

* .NET 10 SDK & Docker Desktop
* Node.js (for the Clinical Portal)
* Python (for the dbt analytics engine)

### Setup

1. **Clone the Hospital Repository:**
```bash
git clone https://github.com/yourusername/securemed-platform.git
cd securemed-platform

```


2. **Initialize Analytics Engine:**
```bash
pip install dbt-sqlserver

```


3. **Launch the Platform:**
```bash
dotnet run --project SecureMed.AppHost

```



*The Aspire Dashboard will provide a real-time view of hospital service health and audit logs.*

---

## 🔒 Healthcare Compliance & Security

### 1. Data Privacy (GDPR/HIPAA)

By using EF Core Value Converters, fields such as **Patient Name, BSN, and Medical Diagnosis** are stored as encrypted blobs. This prevents unauthorized data exposure even during direct database access.

### 2. Clinical Analytics Pipeline

Using the **Medallion Architecture**, SecureMed processes data through:

* **Bronze (Raw):** Encrypted hospital transactions.
* **Silver (Cleaned):** De-duplicated clinical records for internal department use.
* **Gold (Insights):** Anonymized datasets for hospital-wide performance metrics (e.g., "Surgery Room Utilization Rates").

---

## 💻 Technical Deep-Dive

### 1. High-Grade Security Implementation

The system utilizes a **Zero-Trust** approach at the data layer.

* **Application-Level Encryption:** Unlike TDE (Transparent Data Encryption), data is encrypted *before* it leaves the API using a custom `ValueConverter` in EF Core. Even a compromised SQL backup remains unreadable.
* **OAuth2 Scopes:** Fine-grained access control ensuring the Angular frontend only requests the specific permissions (`medical.read`, `admin.analytics`) required for the session.

### 2. Analytics Engineering with dbt

Instead of traditional stored procedures, the platform uses **dbt** to manage the analytics lifecycle:

* **Modular SQL:** Transformation logic is version-controlled and testable.
* **Schema Isolation:** Operational data lives in `dbo`, while processed analytical insights are materialized into the `analytics` schema for reporting.

### 3. Service Orchestration (.NET Aspire)

The entire stack is defined in C# via the **AppHost**, eliminating the need for complex docker-compose files and manual connection string management:

* **Automatic Service Discovery:** The API and Angular apps resolve dependencies dynamically.
* **Persistent Volumes:** Configured SQL Server containers to retain medical records across development sessions.

---



## 🗺️ Clinical Roadmap

### Phase 1: Core EHR Infrastructure (Planned)

* [x] Secure .NET 10 API Foundation.
* [x] Local Identity Provider (Keycloak) for staff authentication.
* [x] Encrypted Patient Storage (SQL Server 2022).

### Phase 2: Clinical Portal & Analytics (Planned)

* [ ] **Angular Clinical Dashboard:** Implementing role-based views for Doctors vs. Administrators.
* [ ] **dbt Implementation:** Building the first set of clinical transformation models.
* [ ] **Audit Tracing:** Real-time monitoring of "Who accessed which patient record?" using OpenTelemetry.

### Phase 3: Advanced Medical Intelligence (Planned)

* [ ] **Predictive Analytics:** Using dbt models to predict patient discharge dates.
* [ ] **FHIR Integration:** Support for standard healthcare data exchange formats.

