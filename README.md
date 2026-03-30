# SecureMed Platform

[![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-21-DD0031?logo=angular)](https://angular.dev/)
[![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?logo=docker)](https://www.docker.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

## What This Project Does

**SecureMed** is a personal learning lab for experimenting with modern software engineering practices. It simulates a healthcare data platform.


## Why SecureMed?

- **Real-world complexity**: Demonstrates handling healthcare workflows with regulatory requirements
- **Full-stack development**: Complete implementation from frontend (Angular) to backend (.NET) to data pipeline (dbt)
- **Modern architecture**: Modular design, CQRS pattern, Domain-Driven Design, and event-driven architecture
- **Enterprise security**: Encryption, RBAC, audit logging, and compliance frameworks built-in
- **Data engineering**: Medallion architecture for analytics with automated pipelines and quality testing

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
└────────────────┘  └────────────┘  └──────────────┘
```

**Operational System (Patient Care) - OLTP**
- PostgreSQL database optimized for transactional operations
- Real-time patient records and appointments
- Role-based access (Doctor, Nurse, Admin, Patient)
- Encrypted PII with audit trails

## Technology Stack

| Layer | Technologies |
|-------|--------------|
| **Frontend** | Angular 21, RxJS, TypeScript (strict mode) |
| **Backend** | .NET 10, Entity Framework Core, MediatR (CQRS) |
| **API** | RESTful with OpenAPI/Swagger, Scalar documentation |
| **Operational DB (OLTP)** | PostgreSQL (optimized for transactional operations) |
| **Caching** | Redis (sessions, caching, real-time features) |
| **Authentication** | OIDC/OAuth2 with RBAC |
| **Containers** | Docker, Docker Compose, .NET Aspire |
| **Observability** | OpenTelemetry, structured logging with Serilog |
| **Testing** | xUnit, Jest, Playwright (E2E) |

## Getting Started


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
└── docs
```

## Key Features

### Patient Care System
- **Patient Management**: Complete patient records with encrypted PII, medical history, and audit trails
- **Appointment Scheduling**: Real-time appointment booking and management
- **Role-Based Access**: Secure access for Doctors, Nurses, Admin, and Patients
- **Real-time Features**: SignalR-based notifications and updates

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


## Credits
Based on the sandbox of Tim Deschryver: https://github.com/timdeschryver/Sandbox/tree/main

