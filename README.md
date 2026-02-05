# SecureMed Platform
IN PROGRESS
A personal lab to learn and deepen architecture, data engineering, and full-stack development capabilities

[![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-19-DD0031?logo=angular)](https://angular.dev/)
[![dbt](https://img.shields.io/badge/dbt-Core-FF694B?logo=dbt)](https://www.getdbt.com/)
[![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?logo=docker)](https://www.docker.com/)

## Overview

SecureMed is a healthcare data platform built to leearn enterprise-level software development practices. It addresses the critical challenge of balancing real-time patient care operations with deep clinical analytics, while maintaining strict GDPR and HIPAA compliance requirements.

The platform uses modern architectural patterns, security-by-design principles, and data engineering best practices in a complex, regulated domain.
It was inspired by and built upon the architectural sandbox project of [Tim Deschryver](https://github.com/timdeschryver).

## Architecture

### High-Level Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                     PRESENTATION LAYER                          │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │   Angular    │  │    Admin     │  │   Mobile     │         │
│  │   Portal     │  │  Dashboard   │  │     PWA      │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    API & AUTH LAYER                             │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │   .NET 10    │  │   Keycloak   │  │     API      │         │
│  │   Web API    │  │     OIDC     │  │   Gateway    │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                   BUSINESS LOGIC LAYER                          │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │     EHR      │  │   Patient    │  │  Appointment │         │
│  │   Service    │  │   Service    │  │   Service    │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    DATA LAYER                                   │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │  PostgresDB  │  │    Redis     │  │   MongoDB    │         │
│  │   (OLTP)     │  │    Cache     │  │   Documents  │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                ANALYTICS & DATA PIPELINE                        │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │   dbt Core   │  │   Airflow    │  │     Data     │         │
│  │  Bronze→Gold │  │ Orchestration│  │  Warehouse   │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
└─────────────────────────────────────────────────────────────────┘
```

### Technology Stack

**Frontend:**
- Angular 19 with standalone components
- RxJS for reactive programming
- NgRx for state management
- Angular Material for UI components
- TypeScript (strict mode)

**Backend:**
- .NET 10 Web API
- Entity Framework Core 10
- MediatR for CQRS pattern
- FluentValidation
- AutoMapper

**Authentication & Security:**
- Keycloak (OAuth2/OIDC)
- JWT Bearer authentication
- Role-Based Access Control (RBAC)
- AES-256 encryption for PII

**Data & Analytics:**
- PostgresDB (Operational database)
- dbt Core (Data transformation)
- Apache Airflow (Orchestration)
- Medallion Architecture (Bronze → Silver → Gold)

**Infrastructure:**
- .NET Aspire (Service orchestration)
- Docker & Docker Compose
- Redis (Caching & sessions)
- OpenTelemetry (Observability)

**DevOps:**
- GitHub Actions (CI/CD)
- SonarQube (Code quality)
- xUnit / Jest (Testing)
- Playwright (E2E testing)

## Key Features

### Operational System (OLTP)

**Patient Management**
- Comprehensive patient records with encrypted PII
- Medical history tracking
- Appointment scheduling and management
- Real-time notifications via SignalR

**Electronic Health Records (EHR)**
- Secure medical record storage
- Document management for medical images
- Audit trail for all data access
- FHIR-compatible data structures

**Role-Based Access**
- Doctor portal (patient care focus)
- Nurse interface (workflow optimization)
- Administrator dashboard (system management)
- Patient portal (self-service)

### Analytics System (OLAP)

**Data Pipeline**
- Automated ETL using dbt
- Incremental model refresh
- Data quality testing
- Lineage tracking

**Business Intelligence**
- Patient flow analysis
- Department utilization metrics
- Staff productivity reports
- Predictive analytics for capacity planning

**Reporting**
- Executive dashboards
- Operational reports
- Clinical outcome analysis
- Regulatory compliance reports

## Security & Compliance

### Data Protection

**Encryption**
- Data at rest: AES-256 encryption for sensitive fields
- Data in transit: TLS 1.3 for all communications
- Key management: Secure key rotation policies

**Access Control**
- Multi-Factor Authentication (MFA)
- Principle of Least Privilege (PoLP)
- Session management with timeout
- IP allowlisting (configurable)

**Audit & Compliance**
- Comprehensive audit logging
- Data access tracking
- Retention policies
- GDPR right-to-erasure support
- HIPAA compliance framework

### 🛡️ Security & Compliance

Het SecureMed platform is ontworpen volgens de **OWASP Top 10 (2025)** standaarden en implementeert Defense-in-Depth op elk niveau van de stack.

- OWASP Top 10 protection
https://owasp.org/Top10/2025/
A01:2025 - Broken Access Control
A02:2025 - Security Misconfiguration
A03:2025 - Software Supply Chain Failures
A04:2025 - Cryptographic Failures
A05:2025 - Injection
A06:2025 - Insecure Design
A07:2025 - Authentication Failures
A08:2025 - Software or Data Integrity Failures
A09:2025 - Security Logging and Alerting Failures
A10:2025 - Mishandling of Exceptional Conditions


#### **1. Identity & Access Management (IAM)**

*Focus op: A01 (Broken Access Control) & A07 (Authentication Failures)*

* **Secure Authentication Flows:** Volledige OIDC/OAuth2 implementatie.
* **MFA Integration:** Ondersteuning voor Multi-Factor Authentication via providers zoals Auth0 of Azure AD.
* **Role-Based Access Control (RBAC):** Granulaire permissies per gebruikerstype (Doctor, Nurse, Admin).
* **OAuth Scopes:** Beveiliging van API-resources op basis van scopes en claims.

#### **2. Data Protection & Cryptography**

*Focus op: A04 (Cryptographic Failures) & A08 (Software or Data Integrity Failures)*

* **At-Rest Encryption:** AES-256 encryptie voor PII (zoals INSZ en patiëntnamen) via EF Core Converters.
* **Searchable Hashing:** Implementatie van blind indexes voor performante, veilige lookups zonder decryptie.
* **In-Transit Security:** Verplichte TLS 1.3 verbindingen voor alle service-to-service communicatie.

#### **3. API & Infrastructure Security**

*Focus op: A02 (Security Misconfiguration) & A05 (Injection)*

* **SQL Injection Prevention:** Gebruik van Entity Framework Core en geparametriseerde queries.
* **Secured API Endpoints:** * **Rate Limiting:** Bescherming tegen brute-force en DoS-aanvallen.
* **Input Validation:** Strikte schema-validatie via FluentValidation.
* **Output Encoding:** Voorkomen van data-lekken door alleen noodzakelijke DTO's (Thin Results) te exposen.


* **API Keys & Secrets:** Veilig beheer via .NET User Secrets en Key Vaults.

#### **4. Resilience & Proactive Defense**

*Focus op: A03 (Supply Chain), A06 (Insecure Design), A09 (Logging) & A10 (Mishandling Conditions)*

* **XSS & CSRF Protection:** Implementatie van Content Security Policy (CSP) en Anti-forgery tokens.
* **Security Logging & Alerting:** Gestructureerde logging via Serilog met automatische redactie van gevoelige velden.
* **Exceptional Condition Handling:** Centrale foutafhandeling die geen stacktraces lekt naar de client.
* **Software Supply Chain:** Automatische scans op kwetsbaarheden in NuGet-pakketten via GitHub Actions.

---


| Maatregel | OWASP 2025 Categorie |
| --- | --- |
| **SQL Injection Prevention** | A05:2025 - Injection |
| **MFA & Secure Flows** | A07:2025 - Authentication Failures |
| **INSZ Encryptie** | A04:2025 - Cryptographic Failures |
| **Rate Limiting** | A02:2025 - Security Misconfiguration (DoS prevention) |
| **Input Validation** | A05:2025 - Injection & A06:2025 - Insecure Design |
| **Error Handling (geen lekken)** | A10:2025 - Mishandling of Exceptional Conditions |
| **Serilog / OpenTelemetry** | A09:2025 - Security Logging and Alerting Failures |


## Data Engineering Highlights

### Medallion Architecture

**Bronze Layer (Raw)**
- Source-aligned data ingestion
- Full historical data retention
- Minimal transformation
- Schema-on-read approach

**Silver Layer (Cleaned)**
- Data quality validation
- De-duplication
- Type casting and standardization
- Business rule application

**Gold Layer (Curated)**
- Star schema dimensional model
- Pre-aggregated metrics
- Optimized for BI tools
- Role-specific data marts

### dbt Implementation

```sql
-- Example: Patient admission metrics
WITH patient_admissions AS (
    SELECT
        patient_id,
        admission_date,
        discharge_date,
        department,
        diagnosis_code,
        DATEDIFF(day, admission_date, discharge_date) AS length_of_stay
    FROM {{ ref('stg_admissions') }}
    WHERE admission_date >= DATEADD(month, -12, GETDATE())
),

metrics AS (
    SELECT
        department,
        DATE_TRUNC('month', admission_date) AS month,
        COUNT(*) AS total_admissions,
        AVG(length_of_stay) AS avg_los,
        PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY length_of_stay) AS median_los,
        COUNT(CASE WHEN length_of_stay > 7 THEN 1 END) AS extended_stays
    FROM patient_admissions
    GROUP BY department, DATE_TRUNC('month', admission_date)
)

SELECT * FROM metrics
ORDER BY month DESC, department
```

### Data Quality

**dbt Tests**
- Schema validation
- Referential integrity checks
- Uniqueness constraints
- Null value monitoring
- Accepted value ranges

**Great Expectations**
- Statistical profiling
- Anomaly detection
- Data drift monitoring
- Automated validation suites

## DevOps & CI/CD

### Continuous Integration

```yaml
# GitHub Actions workflow
- Build .NET API
- Run unit tests
- Run integration tests
- Code coverage analysis
- SonarQube scan
- Build Docker images
- Security vulnerability scan
```

### Continuous Deployment

```yaml
# Deployment pipeline
- Deploy to development environment
- Run smoke tests
- Deploy to staging environment
- Run E2E tests
- Manual approval gate
- Deploy to production
- Post-deployment validation
```

### Monitoring & Observability

**Metrics**
- Application performance (response time, throughput)
- Database performance (query duration, connection pool)
- Business metrics (user activity, appointment bookings)
- Infrastructure metrics (CPU, memory, disk I/O)

**Logging**
- Structured logging with Serilog
- Centralized log aggregation
- Log correlation across services
- Sensitive data redaction

**Tracing**
- Distributed tracing with OpenTelemetry
- Request correlation IDs
- Database query tracing
- External API call tracking


## Getting Started

### Prerequisites

- .NET 10 SDK
- Node.js 20+
- Docker Desktop
- Python 3.11+
- Git

### Installation

1. Clone the repository:
```bash
git clone https://github.com/yourusername/securemed-platform.git
cd securemed-platform
```

2. Setup environment variables:
```bash
cp .env.example .env
# Edit .env with your configuration
```

3. Start infrastructure services:
```bash
docker-compose up -d
```

4. Initialize database:
```bash
cd src/SecureMed.API
dotnet ef database update
```

5. Setup dbt:
```bash
cd src/SecureMed.Analytics
pip install -r requirements.txt
dbt deps
dbt run
```

6. Start the application:
```bash
dotnet run --project src/SecureMed.AppHost
```

7. Access the application:
- Web Portal: https://localhost:5001
- API Swagger: https://localhost:5000/swagger
- Aspire Dashboard: https://localhost:18888
- Keycloak Admin: http://localhost:8080

### Quick Start with Docker Compose

```bash
docker-compose up --build
```

## Project Structure

```
SecureMed/
├── src/
│   ├── SecureMed.AppHost/              # .NET Aspire orchestration
│   ├── SecureMed.ServiceDefaults/      # Shared configurations
│   ├── SecureMed.API/                  # Web API project
│   ├── SecureMed.Domain/               # Domain models & business logic
│   ├── SecureMed.Infrastructure/       # Data access & external services
│   ├── SecureMed.Portal/               # Angular frontend
│   └── SecureMed.Analytics/            # dbt project
│       ├── models/
│       │   ├── staging/                # Bronze layer
│       │   ├── intermediate/           # Silver layer
│       │   └── marts/                  # Gold layer
│       ├── tests/
│       ├── macros/
│       └── snapshots/
├── tests/
│   ├── SecureMed.API.Tests/           # Unit tests
│   ├── SecureMed.Domain.Tests/        # Domain tests
│   ├── SecureMed.Integration.Tests/   # Integration tests
│   └── SecureMed.E2E.Tests/           # End-to-end tests
├── docs/
│   ├── architecture/                   # Architecture documentation
│   ├── api/                           # API documentation
│   └── adr/                           # Architecture Decision Records
├── scripts/
│   ├── setup.sh                       # Setup script
│   └── seed-data.sql                  # Sample data
├── .github/
│   └── workflows/                     # CI/CD pipelines
├── docker/
│   ├── api.Dockerfile
│   ├── portal.Dockerfile
│   └── analytics.Dockerfile
├── docker-compose.yml
├── .gitignore
├── README.md
└── LICENSE
```

## Testing

### Running Tests

```bash
# Unit tests
dotnet test

# Integration tests
dotnet test --filter Category=Integration

# E2E tests
cd tests/SecureMed.E2E.Tests
npx playwright test

# dbt tests
cd src/SecureMed.Analytics
dbt test
```

### Test Coverage

- Unit tests: >80% coverage target
- Integration tests: Critical paths covered
- E2E tests: User workflows validated
- dbt tests: All models tested

## Performance

### Benchmarks

- API response time: <200ms (p95)
- Database query time: <100ms (p95)
- Frontend load time: <2 seconds
- dbt full refresh: <5 minutes (1M records)

### Optimization Strategies

- Database indexing
- Redis caching (70%+ hit rate)
- Query optimization
- Connection pooling
- Response compression
- CDN for static assets

## Deployment

### Environments

- **Development**: Local Docker Compose
- **Staging**: Kubernetes cluster
- **Production**: Cloud provider (Azure/AWS)

### Deployment Strategy

- Blue-Green deployment
- Database migrations automated
- Zero-downtime releases
- Automated rollback capability

## Monitoring

### Dashboards

- Application health (Grafana)
- Business metrics (Power BI / Metabase)
- Infrastructure metrics (Prometheus)
- dbt pipeline status (Airflow UI)

### Alerting

- API error rate threshold
- Database connection issues
- Failed dbt model runs
- Security events


## Roadmap

### Phase 1: Core Platform (In Progress)
- ✅ Basic CRUD operations
- ✅ Authentication & authorization
- ✅ Data pipeline foundation

### Phase 2: Advanced Features (Planned)
- 🚧 Real-time notifications
- 🚧 Advanced analytics
- 🚧 Mobile optimization

### Phase 3: Scale & Performance (Planned)
- ⏳ Kubernetes deployment
- ⏳ Microservices architecture
- ⏳ Multi-tenant support

### Phase 4: Intelligence (Planned)
- ⏳ ML-based predictions
- ⏳ Natural language processing
- ⏳ Automated insights

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Inspired by real-world healthcare systems
- Built with open-source technologies
- Community-driven best practices

## Contact

**Project Repository**: [github.com/yourusername/securemed-platform](https://github.com/yourusername/securemed-platform)

**Demo Video**: [Link to demo]

**Documentation**: [Link to full documentation]

---

## Technical Highlights

### Full-Stack Development
- Modern frontend with Angular 19
- RESTful API with .NET 10
- Real-time features with SignalR
- Progressive Web App capabilities

### Data Engineering
- Medallion architecture implementation
- dbt for data transformation
- Automated data quality testing
- Dimensional modeling (Kimball methodology)

### Analytics Engineering
- Complex SQL with CTEs and window functions
- Incremental model development
- Data documentation and lineage
- Business metric definitions

### DevOps & Infrastructure
- Containerized with Docker
- CI/CD with GitHub Actions
- Infrastructure as Code
- Observability with OpenTelemetry

### Security & Compliance
- Zero-trust architecture
- Encryption at rest and in transit
- GDPR and HIPAA considerations
- Comprehensive audit logging

### Software Engineering
- Clean Architecture principles
- CQRS and Event Sourcing patterns
- Domain-Driven Design
- Test-Driven Development
- SOLID principles


