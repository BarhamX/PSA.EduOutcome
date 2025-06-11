# PSA.EduOutcome

## About this solution

This is a layered startup solution based on [Domain Driven Design (DDD)](https://docs.abp.io/en/abp/latest/Domain-Driven-Design) practises. All the fundamental ABP modules are already installed. Check the [Application Startup Template](https://abp.io/docs/latest/startup-templates/application/index) documentation for more info.

### Pre-requirements

* [.NET9.0+ SDK](https://dotnet.microsoft.com/download/dotnet)
* [Node v18 or 20](https://nodejs.org/en)

### Configurations

The solution comes with a default configuration that works out of the box. However, you may consider to change the following configuration before running your solution:

* Check the `ConnectionStrings` in `appsettings.json` files under the `PSA.EduOutcome.HttpApi.Host` and `PSA.EduOutcome.DbMigrator` projects and change it if you need.

### Before running the application

* Run `abp install-libs` command on your solution folder to install client-side package dependencies. This step is automatically done when you create a new solution, if you didn't especially disabled it. However, you should run it yourself if you have first cloned this solution from your source control, or added a new client-side package dependency to your solution.
* Run `PSA.EduOutcome.DbMigrator` to create the initial database. This step is also automatically done when you create a new solution, if you didn't especially disabled it. This should be done in the first run. It is also needed if a new database migration is added to the solution later.

#### Generating a Signing Certificate

In the production environment, you need to use a production signing certificate. ABP Framework sets up signing and encryption certificates in your application and expects an `openiddict.pfx` file in your application.

To generate a signing certificate, you can use the following command:

```bash
dotnet dev-certs https -v -ep openiddict.pfx -p 337089d1-7916-48b0-8d7d-958c5f311068
```

> `337089d1-7916-48b0-8d7d-958c5f311068` is the password of the certificate, you can change it to any password you want.

It is recommended to use **two** RSA certificates, distinct from the certificate(s) used for HTTPS: one for encryption, one for signing.

For more information, please refer to: [https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html#registering-a-certificate-recommended-for-production-ready-scenarios](https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html#registering-a-certificate-recommended-for-production-ready-scenarios)

> Also, see the [Configuring OpenIddict](https://docs.abp.io/en/abp/latest/Deployment/Configuring-OpenIddict#production-environment) documentation for more information.

### Solution structure

This is a layered monolith application that consists of the following applications:

* `PSA.EduOutcome.DbMigrator`: A console application which applies the migrations and also seeds the initial data. It is useful on development as well as on production environment.
* `PSA.EduOutcome.HttpApi.Host`: ASP.NET Core API application that is used to expose the APIs to the clients.
* `angular`: Angular application.


## Deploying the application

Deploying an ABP application is not different than deploying any .NET or ASP.NET Core application. However, there are some topics that you should care about when you are deploying your applications. You can check ABP's [Deployment documentation](https://docs.abp.io/en/abp/latest/Deployment/Index) and ABP Commercial's [Deployment documentation](https://abp.io/docs/latest/startup-templates/application/deployment?UI=MVC&DB=EF&Tiered=No) before deploying your application.

### Additional resources

#### Internal Resources

You can find detailed setup and configuration guide(s) for your solution below:

* [Angular](./angular/README.md)

#### External Resources
You can see the following resources to learn more about your solution and the ABP Framework:

* [Web Application Development Tutorial](https://abp.io/docs/latest/tutorials/book-store/part-1)
* [Application Startup Template](https://abp.io/docs/latest/startup-templates/application/index)


# Learning Outcome Evaluation System

## Document Control

| **Document Title**       | Learning Outcome Evaluation System BRD |
|--------------------------|----------------------------------------|
| **Version**              | 1.0                                    |
| **Date**                 | June 3, 2025                           |
| **Status**               | Draft                                  |
| **Prepared By**          | Manus AI                               |
| **Prepared For**         | University Faculty                     |

## Table of Contents

- [Executive Summary](#executive-summary)  
- [Project Overview](#project-overview)  
  - [Background](#background)  
  - [Purpose](#purpose)  
  - [Scope](#scope)  
  - [Objectives](#objectives)  
- [Stakeholders](#stakeholders)  
- [Business Requirements](#business-requirements)  
  - [Functional Requirements](#functional-requirements)  
    - [Learning Outcome Management](#learning-outcome-management)  
    - [Assessment Management](#assessment-management)  
    - [Grade Calculation](#grade-calculation)  
    - [Conflict Detection and Resolution](#conflict-detection-and-resolution)  
    - [Reporting and Analytics](#reporting-and-analytics)  
    - [User and Access Management](#user-and-access-management)  
    - [Integration](#integration)  
  - [Non-Functional Requirements](#non-functional-requirements)  
    - [Performance](#performance)  
    - [Scalability](#scalability)  
    - [Availability](#availability)  
    - [Security](#security)  
    - [Usability](#usability)  
    - [Maintainability](#maintainability)  
    - [Compliance](#compliance)  
- [System Architecture](#system-architecture)  
- [Technical Specifications](#technical-specifications)  
- [Implementation Plan](#implementation-plan)  
- [Testing Strategy](#testing-strategy)  
- [Appendices](#appendices)  

---

## Executive Summary

This BRD outlines requirements for a **Learning Outcome Evaluation System** built on ABP Boilerplate with Angular. It will allow instructors to map assessment questions to learning outcomes, compute grades, detect conflicts, and produce multi-level reports (student, course, program, faculty, university). The platform enhances measurement of educational effectiveness, supports accreditation, and drives continuous improvement.

---

## Project Overview

### Background

Universities must demonstrate outcomes-based education to accreditation bodies. Currently, many lack systematic tools to map assessments to learning outcomes and track achievement across organizational levels.

### Purpose

To deliver a comprehensive solution enabling:
- Definition and management of learning outcomes  
- Mapping of questions to outcomes  
- Automated grade calculations  
- Conflict detection/resolution  
- Multi-level statistical reporting  

### Scope

**In-scope**  
- Outcome definition & categories  
- Assessment & question management  
- Mapping questions to outcomes  
- Grade calculation & achievement tracking  
- Conflict detection/resolution  
- Reporting (student → university)  
- Role-based access  
- Integration APIs  

**Out-of-scope**  
- Content delivery/LMS functions  
- Enrollment/registration  
- Financial billing  
- Curriculum authoring  

### Objectives

1. Build a scalable, DDD-based system  
2. Support precise question-to-outcome mapping  
3. Automate grade and achievement calculations  
4. Implement conflict detection tools  
5. Provide multi-level reports & analytics  
6. Enforce role-based access  
7. Ensure educational standards compliance  
8. Deliver a user-friendly Angular UI  
9. Offer integration endpoints  
10. Leverage ABP + EF Core best practices  

---

## Stakeholders

| Stakeholder Group        | Role                                    | Key Interests                                            |
|--------------------------|-----------------------------------------|----------------------------------------------------------|
| **Instructors**          | Define outcomes & assessments           | Flexibility, ease of use, clear reporting                |
| **Students**             | Receive feedback                       | Transparency, meaningful feedback                        |
| **Program Coordinators** | Oversee program-level outcomes          | Program stats, accreditation support                     |
| **Department Heads**     | Oversee faculty-level outcomes          | Faculty stats, resource allocation                       |
| **Administrators**       | University-wide oversight               | Strategic planning, accreditation reporting              |
| **Accreditation Bodies** | External reviewers                      | Comprehensive, standards-aligned data                    |
| **IT Department**        | System maintenance                      | Performance, security, integrations                      |

---

## Business Requirements

### Functional Requirements

#### Learning Outcome Management

1. **Define Learning Outcomes**  
   - CRUD operations; code, description, category, weight.  
   - Assignable to courses/programs.  

2. **Categorize Outcomes**  
   - Hierarchical categories.  
   - Multiple assignments per category.  

3. **Map Outcomes to Org Levels**  
   - Link outcomes to program, faculty, university objectives.  

#### Assessment Management

1. **Create Assessments**  
   - CRUD exams, quizzes, assignments; name, type, marks, weight.  

2. **Create Questions**  
   - CRUD questions; text, max marks; mapable to outcomes.  

3. **Map Questions → Outcomes**  
   - Many-to-many mapping; assign marks per question; enforce total marks consistency.  

#### Grade Calculation

1. **Automated Grade Computation**  
   - Aggregate scores per outcome; support weights.  
2. **Achievement Tracking**  
   - Track completion; calculate achievement percentages.  

#### Conflict Detection and Resolution

1. **Detect Conflicts**  
   - Overlapping mappings, inconsistent marks, missing assessments, imbalance.  
2. **Resolve Conflicts**  
   - Validation tools; recommendations; ensure total marks alignment.  

#### Reporting and Analytics

1. **Multi-Level Reports**  
   - Student, course, program, faculty, university.  
2. **Trend Analysis**  
   - Semester/year comparisons; cohort analyses.  
3. **Export Options**  
   - PDF, Excel, CSV; customizable templates.  

#### User and Access Management

1. **Roles**  
   - Instructors, Coordinators, Heads, Admins, Students.  
2. **Permissions**  
   - Role-based feature/data access; audit logs.  

#### Integration

1. **University Systems**  
   - Data import/export (SIS, LMS); SSO; APIs for external tools.  

---

### Non-Functional Requirements

#### Performance

- ≥500 concurrent users; <2s UI response; 1,000 tx/min; standard reports <5s.

#### Scalability

- Horizontal scaling; optimized for large datasets.

#### Availability

- ≥99.5% uptime; off-peak maintenance; backup/recovery.

#### Security

- Authenticated access; encryption in transit/at rest; audit logs; compliance.

#### Usability

- Intuitive UI; desktop/mobile; WCAG 2.1 AA; contextual help.

#### Maintainability

- Modular code; full documentation; automated tests; configurable without redeploy.

#### Compliance

- FERPA; accreditation standards; institutional policies.

---

## System Architecture

1. **Presentation**: Angular SPA, lazy-loaded feature modules, NgRx state.  
2. **API**: ASP .NET Core REST controllers, ABP dynamic APIs, middleware.  
3. **Application**: Services, DTOs, validation, MediatR for CQRS.  
4. **Domain**: Entities, value objects, domain services, events.  
5. **Infrastructure**: EF Core, repositories, caching, external integrations.  

**Multi-Tenancy** via ABP: tenant data isolation, shared code, cross-tenant reporting.

**Deployment**: Dockerized ASP .NET Core, Angular; SQL Server/PostgreSQL; Redis (optional).

---

## Technical Specifications

- **Backend**: .NET 7+, ABP, EF Core, AutoMapper, FluentValidation  
- **Frontend**: Angular 15+, NgRx, Angular Material, Chart.js, PrimeNG  
- **Database**: SQL Server/PostgreSQL; Redis cache  
- **DevOps**: Docker, Azure DevOps/GitHub Actions, Kubernetes (optional)  

Refer to the System Architecture & Design document for schema diagrams and API specs.

---

## Implementation Plan

### Approach

- Modular, DDD-based  
- Test-Driven Development (unit/integration/E2E)  
- CI/CD pipelines  
- Agile iterations with stakeholder feedback  

### Phases

1. **Foundation** (M1–M2): Env setup, auth, core modules  
2. **Core Features** (M3–M4): Outcomes, assessments, mappings, basic reports  
3. **Advanced** (M5–M6): Grades, conflicts, analytics, visualizations  
4. **Integration** (M7–M8): SIS/LMS, performance tuning, UI polish  
5. **Deployment** (M9–M10): Prod rollout, training, docs, support  

### Resources

- **Team**: PM, BA, Architects, Devs, DBAs, QA, DevOps, Writer  
- **Infra**: Dev/Test/Staging/Prod, CI/CD, repo, issue tracker  
- **Tools**: IDEs, test frameworks, monitoring, documentation  

---

## Testing Strategy

- **Unit Tests**: Domain & application logic  
- **Integration Tests**: API & data interactions  
- **System Tests**: End-to-end scenarios, performance, security, accessibility  
- **UAT**: Stakeholder validation  

Environments: Dev → QA → Staging → Prod-like for performance

Deliverables: Plans, cases, scripts, reports, defect logs, summary

---

## Appendices

### Glossary

| Term                    | Definition                                                         |
|-------------------------|--------------------------------------------------------------------|
| Learning Outcome        | What students should be able to do post-course/program             |
| Assessment              | Exam, assignment, quiz used to measure outcomes                    |
| Outcome Mapping         | Linking questions to outcomes                                      |
| Conflict                | Misalignment between assessments and outcomes                      |
| Multi-tenancy           | Single instance serving multiple organizational units (tenants)    |

### References

1. Requirements Analysis Document  
2. System Architecture & Design Document  
3. AAHE “Nine Principles of Good Practice for Assessing Student Learning”  
4. AAC&U “Elements of Good Assessment Practice”  
5. ASP.NET Boilerplate Docs: https://aspnetboilerplate.com/pages/documents  
6. ABP Framework Best Practices: https://abp.io/docs/latest/Best-Practices  

### Approval

| Role                     | Name/Signature | Date        |
|--------------------------|----------------|-------------|
| Project Sponsor          |                |             |
| Department Head          |                |             |
| IT Director              |                |             |
| Faculty Representative   |                |             |
| Student Representative   |                |             |
