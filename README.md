# SecureMed Platform

[![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-21-DD0031?logo=angular)](https://angular.dev/)
[![dbt](https://img.shields.io/badge/dbt-Core-FF694B?logo=dbt)](https://www.getdbt.com/)
[![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?logo=docker)](https://www.docker.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

## What This Project Does

**SecureMed** is a personal learning lab for software en data engineering practices.
It simulates a healthcare data platform that solves the complex challenge of simultaneously supporting patient care operations while enabling analytics.

The platform combines operational systems (OLTP) for patient care with analytical systems (OLAP) for business intelligence, using modern architectural patterns, security-first design, and data engineering best practices.

## Credits
Sandbox of Tim Deschryver: https://github.com/timdeschryver/Sandbox/tree/main


## Why SecureMed?

- **Real-world complexity**: Demonstrates handling healthcare workflows with regulatory requirements
- **Full-stack development**: Complete implementation from frontend (Angular) to backend (.NET) to data pipeline (dbt)
- **Modern architecture**: Modular design, CQRS pattern, Domain-Driven Design, and event-driven architecture
- **Enterprise security**: Encryption, RBAC, audit logging, and compliance frameworks built-in
- **Data engineering**: Medallion architecture for analytics with automated pipelines and quality testing
- **Production-ready patterns**: CI/CD, observability, testing strategies, and containerization

## Architecture Overview

The platform separates operational and analytical concerns with dedicated databases:

```
┌─────────────────────────────────────────────────────────────────┐
│                  Frontend (Angular 21)                          │
└──────────────────────────┬──────────────────────────────────────┘
                           │
┌──────────────────────────▼──────────────────────────────────────┐
│         .NET 10 API + Gateway (Aspire)                          │
│       (Security, CQRS, Modular Design)                          │
└──────────────────────────┬──────────────────────────────────────┘
                           │
        ┌──────────────────┼──────────────────┐
        │                  │                  │
┌───────▼────────┐  ┌──────▼─────┐  ┌───────▼──────┐
│  PostgreSQL    │  │   Redis    │  │   Auth       │
│  (OLTP DB)     │  │   Cache    │  │   (OIDC)     │
│  Operations    │  │            │  │              │
└────────┬───────┘  └────────────┘  └──────────────┘
         │
         │  Extract & Load (EL)
         │
┌────────▼────────────────────────┐
│   PostgreSQL (OLAP Database)     │
│   Analytics Database Schema      │
│   (Bronze Layer - Raw Data)      │
└────────┬────────────────────────┘
         │
         │  Transform (T) - dbt
         │  Bronze → Silver → Gold
         │
┌────────▼────────────────────────┐
│      Data Warehouse              │
│   Medallion Architecture         │
│   - Silver: Cleaned Data         │
│   - Gold: Business Metrics       │
│   - Optimized for Queries        │
└──────────────────────────────────┘
```

**Operational System (Patient Care) - OLTP**
- PostgreSQL database optimized for transactional operations
- Real-time patient records and appointments
- Role-based access (Doctor, Nurse, Admin, Patient)
- Encrypted PII with audit trails

**Analytical System (Business Intelligence) - ELT Pipeline**
- **Extract & Load (EL)**: Data extracted from OLTP database and loaded to analytics database (Bronze layer)
- **Analytics Database (OLAP)**: Separate PostgreSQL database with three medallion layers
  - **Bronze**: Raw replicated data from source OLTP system
  - **Silver**: Cleaned, deduplicated, and validated data
  - **Gold**: Business-ready metrics, dimensions, and fact tables
- **Transform (T)**: dbt transforms Bronze → Silver → Gold entirely within analytics database
- **Data Warehouse**: Final optimized schema for Business Intelligence and reporting
- OLTP database remains unchanged and dedicated to live operations
- Data quality testing and monitoring
- Dimensional modeling for reporting

## Technology Stack

| Layer | Technologies |
|-------|--------------|
| **Frontend** | Angular 21, RxJS, TypeScript (strict mode) |
| **Backend** | .NET 10, Entity Framework Core, MediatR (CQRS) |
| **API** | RESTful with OpenAPI/Swagger, Scalar documentation |
| **Operational DB (OLTP)** | PostgreSQL (optimized for transactional operations) |
| **Analytics DB (OLAP)** | PostgreSQL or Data Warehouse (optimized for analytical queries) |
| **Caching** | Redis (sessions, caching, real-time features) |
| **Authentication** | OIDC/OAuth2 with RBAC |
| **Data Pipeline (ELT)** | Extract & Load (EL) + dbt Core (Transform), Medallion architecture in OLAP DB, data quality testing |
| **Containers** | Docker, Docker Compose, .NET Aspire |
| **Observability** | OpenTelemetry, structured logging with Serilog |
| **CI/CD** | GitHub Actions, SonarQube code quality |
| **Testing** | xUnit, Jest, Playwright (E2E) |

## Getting Started

### Prerequisites

- **.NET 10 SDK** - [Download](https://dotnet.microsoft.com/download)
- **Node.js 20+** - [Download](https://nodejs.org/)
- **Docker Desktop** - [Download](https://www.docker.com/products/docker-desktop)
- **PostgreSQL client** (optional, for direct DB access)
- **Git**

### Quick Start

1. **Clone the repository**
   ```bash
   git clone https://github.com/sieva-lab/secure-med-lab.git
   cd secure-med-lab
   ```

2. **Set up environment**
   ```bash
   # Copy and configure environment variables
   cp .env.example .env
   ```

3. **Start infrastructure (Database, Redis, etc.)**
   ```bash
   docker-compose up -d
   ```

4. **Run the application**
   ```bash
   # Using .NET Aspire (recommended for development)
   dotnet run --project src/SecureMed.AppHost
   ```

5. **Access the application**
   - 🌐 **Web Portal**: https://localhost:5001
   - 📚 **API Docs**: https://localhost:5000/swagger
   - 📊 **Aspire Dashboard**: https://localhost:18888
   - 🔐 **Keycloak Admin**: http://localhost:8080 (if configured)

### Development Setup

For local development without Docker:

```bash
# Build solution
dotnet build

# Run tests
dotnet test

# Start API service
dotnet run --project src/SecureMed.ApiService

# In separate terminal, start frontend
cd src/SecureMed.Portal
npm install
npm start
```

For detailed setup instructions, see [docs/SETUP.md](docs/SETUP.md) (if available).

> **ELT Data Flow Architecture**:
> 1. **OLTP Database**: PostgreSQL for real-time patient care (modified only by .NET API)
> 2. **Extract & Load**: Automated pipeline extracts data from OLTP and loads to analytics database Bronze layer
> 3. **Analytics Database (OLAP)**: Separate PostgreSQL with three medallion layers:
>    - **Bronze**: Raw replicated data from OLTP system
>    - **Silver**: Cleaned & validated data (dbt transformed)
>    - **Gold**: Business-ready metrics & dimensions (dbt transformed)
> 4. **Data Warehouse**: Final optimized schema for BI and reporting (Gold layer)
>
> The OLTP database is never modified by analytics operations, ensuring live patient care is never impacted by data transformation.


## Project Structure

```
secure-med-lab/
├── src/
├   ├── SecureMed.AngularWorkspace/     # Angular workspace
│   └──── projects
│   │       └── form-validation-lib     # Validation library
│   │       └── opentelemetry-lib       # Opentelemetry library
│   │       └── securemed-app           # Angular app
│   ├── SecureMed.AppHost/              # .NET Aspire orchestration
│   ├── SecureMed.ApiService/           # Main API service
│   ├── SecureMed.Gateway/              # API Gateway
│   ├── SecureMed.Modules.PatientCare/  # Patient care module (modular monolith)
│   ├── SecureMed.Modules.StaffManagement/  # Staff management module
│   ├── SecureMed.Migrations/           # Database migrations
│   ├── SecureMed.ServiceDefaults/      # Shared service configurations
│   └── SecureMed.SharedKernel/         # Shared domain & infrastructure
├── tests/
│   ├── SecureMed.API.Tests/            # API unit tests
│   ├── SecureMed.Domain.Tests/         # Domain tests
│   └── SecureMed.Integration.Tests/    # Integration tests
├── docs/
├── docker/
├── scripts/
│   └── setup scripts for local development
└── docker-compose.yml
```

## Key Features

### Patient Care System
- **Patient Management**: Complete patient records with encrypted PII, medical history, and audit trails
- **Appointment Scheduling**: Real-time appointment booking and management
- **Role-Based Access**: Secure access for Doctors, Nurses, Admin, and Patients
- **Real-time Features**: SignalR-based notifications and updates

### Data Analytics
- **Extract & Load (EL)**: Automated extraction of data from OLTP database into analytics database Bronze layer
- **Analytics Database (OLAP)**: Separate PostgreSQL with Medallion architecture layers
- **dbt Transform (T)**: Operates exclusively within analytics database, transforming Bronze → Silver → Gold
- **Read-Only from OLTP**: Extract process is read-only; OLTP data never modified
- **Data Warehouse**: Final Gold layer optimized for BI queries and reporting
- **Data Quality**: Comprehensive validation and monitoring at each medallion layer
- **Business Intelligence**: Executive dashboards and operational reports from Gold layer

### Security & Compliance
- **Encryption**: AES-256 for sensitive data at rest, TLS 1.3 for transit
- **Authentication**: OIDC/OAuth2 with multi-factor authentication support
- **Audit Logging**: Complete audit trail for compliance and security
- **GDPR/HIPAA**: Privacy-by-design with data retention policies

## Testing

Run tests locally:

```bash
# Unit tests
dotnet test src/

# Integration tests
dotnet test tests/SecureMed.Integration.Tests/

# All tests
dotnet test
```

Test coverage targets:
- Unit tests: >80%
- Critical integration paths: covered
- E2E workflows: validated through HTTP tests

For more details, check [tests/](tests/) directory.


## Performance

Expected performance at scale:
- API response time: <200ms (p95)
- Database queries: <100ms (p95) with proper indexing
- Frontend page load: <2 seconds
- dbt data refresh: <5 minutes (1M+ records)

## Deployment

### Development
```bash
docker-compose up -d
dotnet run --project src/SecureMed.AppHost
```

### Staging/Production
- Use Docker images built from [docker/](docker/) directory
- Deploy with infrastructure-as-code (Kubernetes, Azure Container Apps, etc.)
- Implement blue-green deployment strategy
- Automated database migrations on each deploy

## Monitoring & Observability

- **Metrics**: Application and infrastructure metrics via Prometheus
- **Logging**: Structured logs with Serilog (development), centralized (production)
- **Tracing**: Distributed tracing with OpenTelemetry
- **Dashboards**: Custom dashboards for application health and business metrics

## Roadmap

**Phase 1: Core Platform** (In Progress)
- ✅ Patient management and appointments
- ✅ Authentication and authorization
- ✅ Data pipeline foundation

**Phase 2: Advanced Features** (Next)
- 🚧 Real-time notifications
- 🚧 Advanced analytics and dashboards
- 🚧 Mobile app optimization

**Phase 3: Scale & Architecture** (Future)
- ⏳ Kubernetes deployment
- ⏳ Advanced microservices patterns
- ⏳ Multi-tenant architecture



## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Inspired by real-world healthcare systems and compliance requirements
- Built with modern open-source technologies (.NET, Angular, PostgreSQL, dbt)
- Design patterns influenced by domain-driven design and clean architecture principles
- Learning project from [sieva-lab](https://github.com/sieva-lab)


