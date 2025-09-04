# Learning Outcome Evaluation System - Software Requirements Specification (SRS)

## Document Control

| **Document Title**       | Learning Outcome Evaluation System SRS |
|--------------------------|----------------------------------------|
| **Version**              | 1.0                                    |
| **Date**                 | September 3, 2025                     |
| **Status**               | Final                                  |
| **Prepared By**          | Claude Code Analysis                   |
| **Prepared For**         | Development Team                       |
| **Based on Analysis of** | PSA.EduOutcome Codebase               |

## Table of Contents

- [1. Introduction](#1-introduction)
- [2. Overall Description](#2-overall-description)
- [3. System Features](#3-system-features)
- [4. External Interface Requirements](#4-external-interface-requirements)
- [5. Non-Functional Requirements](#5-non-functional-requirements)
- [6. System Architecture](#6-system-architecture)
- [7. Data Requirements](#7-data-requirements)
- [8. Security Requirements](#8-security-requirements)

---

## 1. Introduction

### 1.1 Purpose
This Software Requirements Specification (SRS) defines the software requirements for the Learning Outcome Evaluation System based on analysis of the existing PSA.EduOutcome codebase and proposed enhancements.

### 1.2 Document Scope
This document covers functional and non-functional requirements for the complete system including existing implementation and proposed enhancements.

### 1.3 References
- PSA.EduOutcome Codebase Analysis
- Enhanced Business Requirements Document (BRD)
- ABP Framework Documentation
- Angular Framework Documentation

---

## 2. Overall Description

### 2.1 Product Perspective
The Learning Outcome Evaluation System is a web-based platform built on:
- **Backend**: .NET 9+ with ABP Framework
- **Frontend**: Angular 15+ with modern UI components  
- **Database**: SQL Server/PostgreSQL with Entity Framework Core
- **Architecture**: Domain-Driven Design (DDD) with CQRS patterns

### 2.2 Product Functions
- Learning outcome definition and management
- Assessment and question creation
- Question-to-outcome mapping with validation
- Student response tracking and grading
- Multi-level reporting and analytics
- User management with role-based access control

### 2.3 User Classes and Characteristics

| User Class | Characteristics | Technical Expertise |
|------------|----------------|-------------------|
| System Administrator | Full system access, technical background | High |
| Department Head | Faculty oversight, basic technical skills | Medium |
| Program Coordinator | Program-level management | Medium |
| Instructor | Course management, assessment creation | Low-Medium |
| Student | View results and feedback | Low |

---

## 3. System Features

### 3.1 Learning Outcome Management

#### 3.1.1 Description
Complete lifecycle management of learning outcomes with categorization and validation.

#### 3.1.2 Functional Requirements

**FR-LO-001: Create Learning Outcome**
- **Input**: Code, Description, Max Mark, Course ID, Category, Display Order
- **Processing**: Validate business rules, check code uniqueness, ensure mark constraints
- **Output**: Created learning outcome with unique ID
- **Validation Rules**:
  - Code: Required, max 20 characters, unique per course
  - Description: Required, max 1000 characters
  - Max Mark: 0 < mark ≤ 100
  - Category: Must be Knowledge, Skills, or Competence

**FR-LO-002: Update Learning Outcome**
- **Precondition**: Learning outcome exists and user has edit permission
- **Processing**: Apply business validation, maintain referential integrity
- **Postcondition**: Learning outcome updated with audit trail

**FR-LO-003: Delete Learning Outcome**
- **Precondition**: No active question mappings exist
- **Processing**: Soft delete with audit information
- **Postcondition**: Learning outcome marked as deleted

### 3.2 Assessment Management

#### 3.2.1 Description
Complete assessment lifecycle including creation, validation, and publishing.

#### 3.2.2 Functional Requirements

**FR-AS-001: Create Assessment**
- **Input**: Title, Type, Total Marks, Weight, Course ID, Due Date, Description
- **Processing**: Validate assessment type, verify weight constraints, set initial status
- **Output**: Created assessment with unpublished status
- **Business Rules**:
  - Title: Required, max 200 characters
  - Type: Must be valid assessment type (Exam, Quiz, Assignment, Project, Lab, Presentation, Other)
  - Total Marks: Must be > 0
  - Weight: 0 < weight ≤ 100
  - Due Date: Must be future date

**FR-AS-002: Publish Assessment**
- **Precondition**: Assessment validation passes
- **Processing**: 
  - Verify all questions have learning outcome mappings
  - Confirm total question marks equal assessment total marks
  - Set published status
- **Postcondition**: Assessment available for student responses

### 3.3 Question Management

#### 3.3.1 Description
Question creation, editing, and learning outcome mapping functionality.

#### 3.3.2 Functional Requirements

**FR-QU-001: Create Question**
- **Input**: Text, Question Number, Max Mark, Assessment ID, Question Type, Description
- **Processing**: Validate constraints, ensure question number uniqueness within assessment
- **Output**: Created question ready for learning outcome mapping
- **Validation Rules**:
  - Text: Required, max 2000 characters
  - Question Number: Must be > 0, unique within assessment
  - Max Mark: Must be > 0
  - Question Type: Must be valid type (MultipleChoice, Essay, ShortAnswer, TrueFalse, Numerical, Matching, FillInTheBlank)

**FR-QU-002: Map Question to Learning Outcomes**
- **Input**: Learning Outcome ID, Allocated Mark, Percentage
- **Processing**: 
  - Validate no duplicate mappings
  - Ensure total allocated marks ≤ question max mark
  - Verify total percentage = 100%
- **Output**: Question-Learning Outcome mapping created
- **Business Rules**:
  - Sum of allocated marks across all mappings ≤ question max mark
  - Sum of percentages across all mappings = 100%
  - No duplicate learning outcome mappings per question

### 3.4 Grade Calculation System

#### 3.4.1 Description
Automated calculation of student achievement based on learning outcome mappings.

#### 3.4.2 Functional Requirements

**FR-GC-001: Calculate Learning Outcome Achievement**
- **Input**: Student responses for all questions mapped to learning outcome
- **Processing**: 
  - For each question mapping: (student_mark / question_max_mark) × percentage × allocated_mark
  - Sum all question contributions
  - Calculate as percentage of learning outcome max mark
- **Output**: Student achievement percentage for learning outcome

**FR-GC-002: Calculate Final Grade**
- **Input**: All learning outcome achievements for student
- **Processing**: Weighted average based on learning outcome max marks
- **Output**: Final course grade

### 3.5 Reporting System

#### 3.5.1 Description
Multi-level reporting with statistical analysis and export capabilities.

#### 3.5.2 Functional Requirements

**FR-RP-001: Generate Student Report**
- **Content**: Individual learning outcome achievements, overall grade, areas for improvement
- **Format**: PDF, HTML view
- **Access**: Student and instructors

**FR-RP-002: Generate Course Report**
- **Content**: Course-level statistics, learning outcome distribution, assessment performance
- **Format**: PDF, Excel, CSV
- **Access**: Instructors, coordinators, department heads

**FR-RP-003: Generate Program Report**
- **Content**: Program-level outcomes, trend analysis, comparative statistics
- **Format**: PDF, Excel, PowerPoint
- **Access**: Coordinators, department heads, administrators

---

## 4. External Interface Requirements

### 4.1 User Interfaces

#### 4.1.1 Web Interface Requirements
- **Responsive Design**: Support desktop, tablet, and mobile devices
- **Browser Compatibility**: Chrome 90+, Firefox 88+, Safari 14+, Edge 90+
- **Accessibility**: WCAG 2.1 AA compliance
- **Language Support**: Multi-language with localization

#### 4.1.2 Dashboard Requirements
- **Role-based Dashboards**: Customized views for each user type
- **Real-time Updates**: Live data refresh without page reload
- **Interactive Elements**: Drill-down capabilities, filtering, sorting
- **Visualizations**: Charts, graphs, heatmaps, trend lines

### 4.2 Hardware Interfaces
- **Server Requirements**: Windows Server 2019+ or Linux distribution
- **Database**: SQL Server 2019+ or PostgreSQL 12+
- **Storage**: Minimum 100GB for initial deployment
- **Network**: Minimum 1Gbps connection for production

### 4.3 Software Interfaces

#### 4.3.1 Student Information System (SIS) Integration
- **Protocol**: REST API with OAuth 2.0 authentication
- **Data Exchange**: Student enrollment, course registration, grade passback
- **Frequency**: Real-time for critical operations, batch for bulk updates
- **Error Handling**: Retry mechanism with exponential backoff

#### 4.3.2 Learning Management System (LMS) Integration
- **Standards**: LTI 1.3, QTI 3.0 for assessment interoperability
- **Data Exchange**: Assessment content, student responses, gradebook integration
- **Authentication**: Single Sign-On (SSO) integration

#### 4.3.3 External Reporting Systems
- **Format**: Standard export formats (CSV, Excel, PDF, XML)
- **Scheduling**: Automated report generation and delivery
- **Compliance**: Support for accreditation body requirements

---

## 5. Non-Functional Requirements

### 5.1 Performance Requirements

| Metric | Requirement | Measurement Method |
|--------|-------------|-------------------|
| Response Time | <2 seconds for 95% of requests | Load testing |
| Throughput | 1000 concurrent users | Stress testing |
| Database Queries | <500ms for complex reports | Query profiling |
| File Upload | 100MB max file size | Functional testing |

### 5.2 Scalability Requirements
- **Horizontal Scaling**: Support for load balancer with multiple app servers
- **Database Scaling**: Read replicas for reporting queries
- **Caching**: Redis/In-memory caching for frequently accessed data
- **CDN**: Content delivery network for static assets

### 5.3 Availability Requirements
- **Uptime**: 99.5% availability (approximately 22 hours downtime per year)
- **Maintenance Window**: Planned maintenance during off-peak hours
- **Recovery Time**: <4 hours for system restoration
- **Backup**: Daily incremental, weekly full backup

### 5.4 Security Requirements
- **Authentication**: Multi-factor authentication (MFA) support
- **Authorization**: Role-based access control (RBAC)
- **Data Encryption**: TLS 1.3 for data in transit, AES-256 for data at rest
- **Audit Logging**: Comprehensive activity logging with tamper protection
- **Privacy Compliance**: FERPA, GDPR compliance

### 5.5 Usability Requirements
- **Learning Curve**: New users productive within 2 hours of training
- **Error Prevention**: Input validation with clear error messages
- **Help System**: Context-sensitive help and user documentation
- **Keyboard Navigation**: Full keyboard accessibility support

---

## 6. System Architecture

### 6.1 Architecture Overview
```
┌─────────────────────────────────────────────┐
│                Frontend                      │
│            Angular SPA                      │
│   ┌─────────────┬─────────────┬───────────┐ │
│   │ Components  │ Services    │ Routing   │ │
│   │             │             │           │ │
│   └─────────────┴─────────────┴───────────┘ │
└─────────────────┬───────────────────────────┘
                  │ HTTP/HTTPS
┌─────────────────▼───────────────────────────┐
│              API Gateway                    │
│            ABP Framework                    │
│   ┌─────────────┬─────────────┬───────────┐ │
│   │ Controllers │ Services    │ Auth      │ │
│   │             │             │           │ │
│   └─────────────┴─────────────┴───────────┘ │
└─────────────────┬───────────────────────────┘
                  │
┌─────────────────▼───────────────────────────┐
│           Application Layer                 │
│   ┌─────────────┬─────────────┬───────────┐ │
│   │ App Services│ DTOs        │ Validators│ │
│   │             │             │           │ │
│   └─────────────┴─────────────┴───────────┘ │
└─────────────────┬───────────────────────────┘
                  │
┌─────────────────▼───────────────────────────┐
│             Domain Layer                    │
│   ┌─────────────┬─────────────┬───────────┐ │
│   │ Entities    │ Services    │ Events    │ │
│   │             │             │           │ │
│   └─────────────┴─────────────┴───────────┘ │
└─────────────────┬───────────────────────────┘
                  │
┌─────────────────▼───────────────────────────┐
│         Infrastructure Layer                │
│   ┌─────────────┬─────────────┬───────────┐ │
│   │ EF Core     │ Repositories│ External  │ │
│   │ DbContext   │             │ APIs      │ │
│   └─────────────┴─────────────┴───────────┘ │
└─────────────────┬───────────────────────────┘
                  │
┌─────────────────▼───────────────────────────┐
│               Database                      │
│          SQL Server/PostgreSQL              │
└─────────────────────────────────────────────┘
```

### 6.2 Data Flow Architecture
1. **Request Flow**: Angular → API Gateway → Application Service → Domain Service → Repository → Database
2. **Response Flow**: Database → Repository → Domain Service → Application Service → API Gateway → Angular
3. **Event Flow**: Domain Events → Event Handlers → External System Notifications

---

## 7. Data Requirements

### 7.1 Database Schema Overview

#### 7.1.1 Core Entities
```sql
-- Universities
Universities (Id, Name, Code, Address, ContactInfo)

-- Faculties  
Faculties (Id, Name, Code, UniversityId)

-- Programs
Programs (Id, Name, Code, FacultyId, DegreeType)

-- Courses
Courses (Id, Name, Code, Credits, Semester, Year, ProgramId, InstructorId)

-- Learning Outcomes
LearningOutcomes (Id, Code, Description, MaxMark, CourseId, Category, DisplayOrder)

-- Assessments
Assessments (Id, Title, Type, TotalMarks, Weight, CourseId, DueDate, IsPublished)

-- Questions  
Questions (Id, Text, QuestionNumber, MaxMark, AssessmentId, QuestionType)

-- Question-Learning Outcome Mappings
QuestionLearningOutcomes (Id, QuestionId, LearningOutcomeId, AllocatedMark, Percentage)

-- Students
Students (Id, StudentNumber, FirstName, LastName, Email, DateOfBirth)

-- Student Responses
StudentResponses (Id, StudentId, QuestionId, AchievedMark, SubmissionDate)

-- Enrollments
Enrollments (Id, StudentId, CourseId, EnrollmentDate, Status)
```

#### 7.1.2 Data Relationships
- Universities → Faculties (1:N)
- Faculties → Programs (1:N)  
- Programs → Courses (1:N)
- Courses → LearningOutcomes (1:N)
- Courses → Assessments (1:N)
- Assessments → Questions (1:N)
- Questions → QuestionLearningOutcomes (1:N)
- LearningOutcomes → QuestionLearningOutcomes (1:N)
- Students → StudentResponses (1:N)
- Questions → StudentResponses (1:N)

### 7.2 Data Integrity Rules
- **Referential Integrity**: Foreign key constraints on all relationships
- **Business Rules**: Check constraints for mark validations, percentage validations
- **Audit Trail**: CreatedBy, CreatedTime, LastModifiedBy, LastModifiedTime on all entities
- **Soft Delete**: IsDeleted flag with DeletedBy, DeletedTime for core entities

---

## 8. Security Requirements

### 8.1 Authentication
- **Identity Provider**: ABP Identity with custom extensions
- **MFA Support**: Time-based OTP (TOTP), SMS, email verification
- **SSO Integration**: SAML 2.0, OAuth 2.0, OpenID Connect
- **Password Policy**: Minimum 8 characters, complexity requirements, expiration

### 8.2 Authorization  
- **Role-Based Access Control (RBAC)**: Predefined roles with granular permissions
- **Resource-Based Authorization**: Course-level, assessment-level access control
- **Tenant Isolation**: Multi-tenant data segregation
- **API Security**: JWT tokens with refresh mechanism

### 8.3 Data Protection
- **Encryption at Rest**: Database encryption, file storage encryption
- **Encryption in Transit**: TLS 1.3 for all communications
- **PII Protection**: Encrypted storage of personal identifiable information
- **Data Retention**: Configurable retention policies with automated cleanup

### 8.4 Compliance
- **FERPA**: Educational records protection
- **GDPR**: Right to deletion, data portability, consent management  
- **SOC 2**: Security controls framework compliance
- **Audit Requirements**: Comprehensive logging, tamper-proof audit trails

---

## Appendices

### A. Glossary
- **Learning Outcome**: Measurable statement of what students should achieve
- **Assessment**: Method of evaluating student performance
- **Mapping**: Relationship between questions and learning outcomes
- **Achievement**: Student performance level on learning outcomes

### B. Assumptions and Dependencies
- **Assumptions**: Stable internet connectivity, modern browser support
- **Dependencies**: ABP Framework, Angular framework, database system
- **External Dependencies**: Email service, file storage service

### C. Future Enhancements
- Machine learning integration for predictive analytics
- Mobile native applications
- Advanced data visualization components
- Real-time collaboration features