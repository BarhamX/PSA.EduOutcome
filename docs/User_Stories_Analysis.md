# Learning Outcome Evaluation System - User Stories Analysis

## Extracted User Stories from Business Documents

Based on the Business Requirements Document (BRD), Functional Requirements Document (FRD), System Requirements Document (SRD), and Software Requirements Specification (SRS), here are the categorized user stories:

## 1. User Management Functions

### Authentication Functions

**User Management/Authentication Functions/F-UM-001: User Login**
- As a user, I want to login with my username/email and password so that I can access the system
- As a user, I want the system to remember me so that I don't have to login repeatedly
- As a system admin, I want accounts to be automatically locked after 5 failed attempts to prevent brute force attacks

**User Management/Authentication Functions/F-UM-002: Multi-Factor Authentication**
- As a user, I want to use multi-factor authentication (OTP) so that my account is more secure
- As a user, I want to receive OTP via SMS, email, or authenticator app for flexibility
- As a system admin, I want OTP to expire after 5 minutes for security

### Authorization Functions

**User Management/Authorization Functions/F-UM-003: Role-Based Access Control**
- As a system admin, I want to assign roles to users so that they have appropriate access levels
- As a user, I want to only see and access features relevant to my role
- As a system admin, I want to ensure users can only access data within their tenant for multi-tenancy

### Profile Management Functions

**User Management/Profile Management Functions/F-UM-004: Update User Profile**
- As a user, I want to update my profile information (name, email, phone) so that my details are current
- As a user, I want to receive confirmation when I change my email address for security
- As a system admin, I want all profile changes to be logged for audit purposes

## 2. Learning Outcome Management Functions

### CRUD Operations

**Learning Outcome Management/CRUD Operations/F-LO-001: Create Learning Outcome**
- As an instructor, I want to create learning outcomes for my course with a unique code so that I can define what students should achieve
- As an instructor, I want to set a maximum mark for each learning outcome so that grading is consistent
- As an instructor, I want to categorize learning outcomes (Knowledge/Skills/Competence) so that they align with educational standards
- As an instructor, I want the system to prevent me from creating learning outcomes that exceed 100 total marks per course

**Learning Outcome Management/CRUD Operations/F-LO-002: Update Learning Outcome**
- As an instructor, I want to modify learning outcome details so that I can improve course structure
- As an instructor, I want the system to prevent changes that would break existing question mappings
- As an instructor, I want all changes to be tracked for audit purposes

**Learning Outcome Management/CRUD Operations/F-LO-003: Delete Learning Outcome**
- As an instructor, I want to remove learning outcomes that are no longer relevant
- As an instructor, I want the system to prevent deletion if questions are mapped to the learning outcome
- As a system admin, I want deleted items to be soft-deleted to maintain audit trail

### Validation Functions

**Learning Outcome Management/Validation Functions/F-LO-004: Validate Course Learning Outcomes**
- As an instructor, I want the system to validate that my course learning outcomes total exactly 100 marks
- As a quality assurance officer, I want to run validation reports to ensure course compliance
- As a program coordinator, I want to see which courses have incomplete learning outcome definitions

## 3. Assessment Management Functions

### Assessment Lifecycle

**Assessment Management/Assessment Lifecycle/F-AS-001: Create Assessment**
- As an instructor, I want to create assessments with title, type, marks, and due date so that I can evaluate students
- As an instructor, I want to set assessment weight as percentage of final grade for proper course structure
- As an instructor, I want to configure late submission policies so that students understand consequences

**Assessment Management/Assessment Lifecycle/F-AS-002: Add Question to Assessment**
- As an instructor, I want to add questions to my assessment so that I can build comprehensive evaluations
- As an instructor, I want the system to prevent me from exceeding the total assessment marks
- As an instructor, I want to specify question types (MCQ, Essay, etc.) for proper grading

**Assessment Management/Assessment Lifecycle/F-AS-003: Publish Assessment**
- As an instructor, I want to publish assessments so that students can access them
- As an instructor, I want the system to validate completeness before publishing to avoid errors
- As a student, I want to receive notifications when assessments are published

### Assessment Configuration

**Assessment Management/Assessment Configuration/F-AS-004: Configure Late Submission Policy**
- As an instructor, I want to set late submission penalties so that students submit work on time
- As an instructor, I want to configure grace periods for fairness
- As a student, I want to see late submission policies clearly displayed

## 4. Question Management Functions

### Question Operations

**Question Management/Question Operations/F-QU-001: Create Question**
- As an instructor, I want to create questions with text, marks, and type so that I can assess specific learning outcomes
- As an instructor, I want to assign unique question numbers within an assessment for organization
- As an instructor, I want to specify question types to ensure appropriate assessment methods

**Question Management/Question Operations/F-QU-002: Update Question**
- As an instructor, I want to modify question details so that I can improve assessment quality
- As an instructor, I want the system to prevent changes that would break learning outcome mappings
- As an instructor, I want to update questions only before assessment publication to maintain integrity

### Question Types and Validation

**Question Management/Question Types/F-QU-003: Validate Question by Type**
- As an instructor, I want the system to validate question-specific requirements (e.g., MCQ options)
- As an instructor, I want to ensure essay questions have proper rubrics for consistent grading
- As a student, I want questions to be properly formatted based on their type for clarity

## 5. Mapping and Validation Functions

### Question-Learning Outcome Mapping

**Mapping and Validation/Question-LO Mapping/F-MV-001: Create Question-Learning Outcome Mapping**
- As an instructor, I want to map questions to learning outcomes with mark allocation so that I can track student achievement
- As an instructor, I want to assign percentages to show how much each question contributes to learning outcome
- As an instructor, I want the system to prevent duplicate mappings and validate totals

**Mapping and Validation/Question-LO Mapping/F-MV-002: Validate Question Mappings**
- As an instructor, I want to validate that question mappings are complete and accurate
- As a quality assurance officer, I want to run mapping validation reports across courses
- As a program coordinator, I want to ensure all questions properly map to learning outcomes

### Assessment Validation

**Mapping and Validation/Assessment Validation/F-MV-003: Validate Assessment Completeness**
- As an instructor, I want comprehensive validation before publishing assessments
- As a system admin, I want to ensure data integrity across the system
- As a quality assurance officer, I want validation reports for compliance

### Course Validation

**Mapping and Validation/Course Validation/F-MV-004: Validate Course Assessment Coverage**
- As an instructor, I want to ensure my assessments adequately cover all learning outcomes
- As a program coordinator, I want coverage analysis reports for program evaluation
- As a department head, I want to identify courses with inadequate assessment coverage

## 6. Grade Calculation Functions

### Individual Grade Calculations

**Grade Calculation/Individual Calculations/F-GC-001: Calculate Learning Outcome Achievement**
- As a student, I want to see my achievement level for each learning outcome
- As an instructor, I want automated calculation of student achievements to save time
- As an instructor, I want to see detailed calculation breakdowns for transparency

**Grade Calculation/Individual Calculations/F-GC-002: Calculate Assessment Grade**
- As a student, I want to see my grade for each assessment with late penalties applied if applicable
- As an instructor, I want automated grade calculation to reduce errors
- As an instructor, I want to see how grades are calculated for verification

### Aggregate Calculations

**Grade Calculation/Aggregate Calculations/F-GC-003: Calculate Course Final Grade**
- As a student, I want to see my final course grade with clear breakdown
- As an instructor, I want weighted averages calculated automatically
- As an instructor, I want to see comprehensive grade reports for each student

**Grade Calculation/Program Analysis/F-GC-004: Calculate Program Learning Outcome Statistics**
- As a program coordinator, I want statistical analysis of learning outcome achievements
- As a department head, I want trend analysis to identify improvement areas
- As an accreditation officer, I want statistical reports for compliance

## 7. Reporting Functions

### Student Reports

**Reporting/Student Reports/F-RP-001: Generate Individual Student Report**
- As a student, I want comprehensive reports showing my performance and progress
- As a parent/guardian, I want to access my child's academic progress reports
- As an instructor, I want detailed student reports to provide better guidance

**Reporting/Student Progress/F-RP-002: Generate Student Learning Outcome Progress**
- As a student, I want to track my progress on specific learning outcomes over time
- As an instructor, I want to identify students who need additional support
- As an advisor, I want progress tracking for student counseling

### Course Reports

**Reporting/Course Reports/F-RP-003: Generate Course Performance Report**
- As an instructor, I want comprehensive course performance analysis
- As a department head, I want to compare course performance across semesters
- As a program coordinator, I want course-level statistics for program improvement

**Reporting/Assessment Analysis/F-RP-004: Generate Assessment Analysis Report**
- As an instructor, I want detailed analysis of assessment performance to improve future assessments
- As a curriculum designer, I want item analysis to identify problematic questions
- As a quality assurance officer, I want assessment reliability metrics

### Program and Institutional Reports

**Reporting/Program Reports/F-RP-005: Generate Program Assessment Report**
- As a program coordinator, I want comprehensive program assessment reports for accreditation
- As a department head, I want program-level outcome achievements for strategic planning
- As an accreditation officer, I want standardized reports for compliance submissions

### Export and Scheduling Functions

**Reporting/Automation/F-RP-006: Schedule Automatic Report Generation**
- As an administrator, I want to schedule regular report generation to reduce manual work
- As a department head, I want automated monthly/semester reports delivered via email
- As a system admin, I want to configure report recipients and formats

## 8. Administrative Functions

### System Configuration

**Administrative/System Configuration/F-AD-001: Manage System Settings**
- As a system admin, I want to configure system-wide settings for consistent operation
- As a system admin, I want configuration changes to be logged for security
- As a system admin, I want environment-specific configurations for dev/test/prod

**Administrative/User Management/F-AD-002: Manage User Roles and Permissions**
- As a system admin, I want to assign and modify user roles efficiently
- As a system admin, I want temporal role assignments for temporary access
- As a system admin, I want role changes to be immediately effective

### Data Management

**Administrative/Data Management/F-AD-003: Bulk Data Import**
- As a system admin, I want to import large datasets from external systems
- As a data manager, I want data validation during import to prevent errors
- As a system admin, I want import error reports to fix data issues

**Administrative/Data Management/F-AD-004: Data Archival and Cleanup**
- As a system admin, I want to archive old data to maintain system performance
- As a compliance officer, I want to ensure legal retention requirements are met
- As a system admin, I want rollback capabilities for failed operations

## 9. Integration Functions

### Student Information System (SIS) Integration

**Integration/SIS Integration/F-IN-001: Synchronize Student Enrollment Data**
- As a registrar, I want automatic synchronization of student enrollment from SIS
- As an instructor, I want current enrollment data without manual updates
- As a system admin, I want synchronization reports to monitor data integrity

**Integration/SIS Integration/F-IN-002: Grade Passback to SIS**
- As an instructor, I want grades automatically sent back to SIS to avoid double entry
- As a registrar, I want grade integration for transcript generation
- As a system admin, I want grade export audit trails for verification

### Learning Management System (LMS) Integration

**Integration/LMS Integration/F-IN-003: Import Assessment Content**
- As an instructor, I want to import assessments from my LMS to save time
- As a curriculum designer, I want content mapping suggestions for learning outcomes
- As a system admin, I want import validation to ensure data quality

### External Reporting Integration

**Integration/External Reporting/F-IN-004: Export Accreditation Reports**
- As an accreditation officer, I want standardized reports for accreditation bodies
- As a compliance manager, I want automated report generation for deadlines
- As a quality assurance officer, I want validation of report completeness

## 10. Enhanced User Interface Functions (from BRD Enhancement Proposals)

### Dashboard Development

**UI Enhancement/Dashboards/FR-UI-001: Dashboard Development**
- As a user, I want role-specific dashboards showing relevant information at a glance
- As an instructor, I want interactive charts showing course performance metrics
- As a student, I want a personal dashboard showing my progress and upcoming assessments

### Advanced Data Visualization

**UI Enhancement/Data Visualization/FR-UI-002: Advanced Data Visualization**
- As an instructor, I want heatmaps showing student performance across learning outcomes
- As a program coordinator, I want trend analysis graphs for program evaluation
- As a department head, I want comparative charts for cross-course analysis

### Enhanced User Experience

**UI Enhancement/User Experience/FR-UI-003: Enhanced User Experience**
- As an instructor, I want drag-and-drop functionality for easy question mapping
- As a user, I want inline editing capabilities to quickly update information
- As an instructor, I want bulk operations to efficiently manage multiple items

## 11. Advanced Analytics Functions (from BRD Enhancement Proposals)

### Predictive Analytics

**Analytics/Predictive Analytics/FR-AN-001: Predictive Analytics**
- As an instructor, I want to identify at-risk students early for intervention
- As an advisor, I want performance predictions to guide student counseling
- As a program coordinator, I want achievement forecasting for program planning

### Comparative Analysis

**Analytics/Comparative Analysis/FR-AN-002: Comparative Analysis**
- As a department head, I want semester-over-semester performance comparisons
- As a program coordinator, I want cohort analysis for program evaluation
- As an instructor, I want benchmarking against similar courses

### Statistical Reporting

**Analytics/Statistical Reporting/FR-AN-003: Statistical Reporting**
- As a researcher, I want advanced statistical measures for academic research
- As a quality assurance officer, I want correlation analysis between assessments and outcomes
- As a program coordinator, I want confidence intervals for reliable reporting