# HR-Workforce-Management

# Entity Relationship Diagram (ERD) - Employee Management

This section describes the ERD for the **Employee Management** module, derived directly from 36 UI screens located in `docs/assets/UI`.

## 1. High-Level ERD (Overview - Compact Layout)

Overview diagram using standard `erDiagram` notation (`||--o{`, `||--o|`, `}o--||`), organized in top-down layers for a clean, balanced layout:

```mermaid
erDiagram
    %% Layer 1: Organization & System Accounts
    ROLES ||--o{ USER_ROLES : "assigned"
    USERS ||--o{ USER_ROLES : "has_roles"
    COMPANIES ||--o{ DEPARTMENTS : "contains"
    DEPARTMENTS ||--o{ POSITIONS : "defines"
    DEPARTMENTS ||--o{ TEAMS : "contains"

    %% Layer 2: Employees - Core Link
    USERS ||--o| EMPLOYEES : "account"
    DEPARTMENTS ||--o{ EMPLOYEES : "belongs_to"
    POSITIONS ||--o{ EMPLOYEES : "position"

    %% Layer 3: Employment Records & History
    EMPLOYEES ||--o{ EMPLOYMENT_RECORDS : "records"
    EMPLOYEES ||--o{ REPORTING_LINES : "reports"
    EMPLOYEES ||--o{ EMPLOYMENT_HISTORIES : "history"

    %% Layer 4: Contracts & Documents
    EMPLOYEES ||--o{ CONTRACTS : "contracts"
    CONTRACT_TYPES ||--o{ CONTRACTS : "type"
    EMPLOYEES ||--o{ EMPLOYEE_DOCUMENTS : "documents"
    DOCUMENT_TYPES ||--o{ EMPLOYEE_DOCUMENTS : "type"

    %% Layer 5: Qualifications & Approval Requests
    EMPLOYEES ||--o{ EDUCATIONS : "education"
    EMPLOYEES ||--o{ CERTIFICATIONS : "certs"
    EMPLOYEES ||--o{ REQUESTS : "submits"
    REQUEST_TYPES ||--o{ REQUESTS : "type"
    REQUESTS ||--o{ APPROVAL_STEPS : "steps"
```

---

## 2. Detailed ERD by UI Screen Group (Tables, Fields & Keys)

All UI screens are grouped into **7 functional groups**:

### Group 1: Authentication & Access Control
*Corresponding screens:* `01. Login`, `02. Reset Password`, `03. Forgot Password`, `Pop-up_ Compensation & Access` (System Access section).

```mermaid
erDiagram
    users {
        uuid id PK
        string email UK
        string password_hash
        string status
        boolean is_locked
        datetime email_verified_at
        datetime last_login_at
        datetime created_at
    }

    employees {
        uuid id PK
        string employee_code UK
        string first_name
        string last_name
        string preferred_name
        string photo_url
        string work_email UK
        string personal_email
        string work_phone
        string personal_phone
        string gender
        string marital_status
        string nationality
        date date_of_birth
        string status
        date hire_date
        uuid user_id FK
    }

    roles {
        uuid id PK
        string code UK
        string name
        string description
    }

    permissions {
        uuid id PK
        string code UK
        string name
        string module
    }

    user_roles {
        uuid user_id PK, FK
        uuid role_id PK, FK
        datetime assigned_at
    }

    password_reset_tokens {
        uuid id PK
        uuid user_id FK
        string token UK
        datetime expires_at
        boolean is_used
    }

    users ||--o{ user_roles : "has"
    roles ||--o{ user_roles : "assigned_to"
    users ||--o{ password_reset_tokens : "requests"
    users ||--o| employees : "identity_of"
```

---

### Group 2: Organization Structure & Reporting Lines (Organization Management)
*Corresponding screens:* `Department`, `Department Detail`, `Create/Edit Department form`, `Position`, `Position Detail`, `Create/Edit Position form`, `Team`, `Team Detail`, `Create/Edit Team form`, `Assign manager popup`, `Assign member`, `Organization chart`, `Reporting lines`.

```mermaid
erDiagram
    companies {
        uuid id PK
        string name
        string registration_number
        string country
        string status 
    }

    work_locations {
        uuid id PK
        string code UK 
        string name
        string city
        string country
        string status 
    }

    departments {
        uuid id PK
        uuid company_id FK
        string dept_code UK 
        string dept_name
        uuid parent_dept_id FK 
        uuid manager_id FK 
        string cost_center 
        string location
        date effective_date
        text description
        string status 
    }

    positions {
        uuid id PK
        uuid department_id FK
        string position_code UK
        string position_name
        uuid default_team_id FK
        uuid reports_to_position_id FK 
        string job_level 
        integer approved_headcount
        string status 
        text description
    }

    teams {
        uuid id PK
        uuid department_id FK
        string team_code UK 
        string team_name 
        uuid team_lead_id FK 
        string location 
        integer target_headcount
        string status 
        date effective_date
        text description
    }

    team_members {
        uuid id PK
        uuid team_id FK
        uuid employee_id FK
        string role_in_team 
        datetime joined_at
    }

    reporting_lines {
        uuid id PK
        uuid employee_id FK
        uuid manager_id FK
        string reporting_type
        date effective_date
    }

    employees {
        uuid id PK
        string employee_code UK
        string first_name
        string last_name
        string preferred_name
        string photo_url
        string work_email UK
        string personal_email
        string work_phone
        string personal_phone
        string gender
        string marital_status
        string nationality
        date date_of_birth
        string status
        date hire_date
        uuid user_id FK
    }

    companies ||--o{ departments : "contains"
    departments ||--o{ departments : "parent_dept (Hierarchy)"
    departments ||--o{ positions : "defines"
    departments ||--o{ teams : "contains"
    positions ||--o{ positions : "reports_to (Hierarchy)"
    positions }o--|| teams : "default_team"
    teams ||--o{ team_members : "includes"
    employees ||--o{ team_members : "member_of"
    departments }o--|| employees : "managed_by"
    teams }o--|| employees : "led_by"
    work_locations ||--o{ departments : "located_at"
    reporting_lines }o--|| employees : "employee"
    reporting_lines }o--|| employees : "manager"
```

---

### Group 3: Employee Directory & Add Employee Wizard (Directory & Onboarding Wizard)
*Corresponding screens:* `Pop-up_ add employee modal` (Step 1), `Pop-up_ Employment details` (Step 2), `Pop-up_ Compensation & Access` (Step 3), `Employee Directory - tab directory`, `Employee directory - tab status`, `Export Employee Data`, `04. HR Dashboard`, `People Management Overview`.

```mermaid
erDiagram
    employees {
        uuid id PK
        string employee_code UK
        string first_name 
        string last_name 
        string preferred_name
        string photo_url
        string work_email UK
        string personal_email
        string work_phone
        string personal_phone
        string gender
        string marital_status
        string nationality
        date date_of_birth
        string status
        date hire_date
        uuid user_id FK
    }

    addresses {
        uuid id PK
        uuid employee_id FK
        string address_type 
        string street_address
        string city
        string country
    }

    emergency_contacts {
        uuid id PK
        uuid employee_id FK
        string full_name
        string relationship
        string primary_phone
        string secondary_phone
        string email
        string preferred_contact_method
        string address
        text notes
    }

    employment_records {
        uuid id PK
        uuid employee_id FK
        uuid department_id FK
        uuid position_id FK
        uuid team_id FK
        uuid manager_id FK
        uuid work_location_id FK
        string employment_type 
        string worker_type 
        string job_level 
        string work_mode 
        integer weekly_hours
        string time_zone
        string pay_frequency
        string currency
        decimal salary_amount
    }

    export_logs {
        uuid id PK
        uuid user_id FK
        string export_format
        jsonb selected_fields
        datetime created_at
    }

    employees ||--o{ addresses : "lives_at"
    employees ||--o{ emergency_contacts : "has"
    employees ||--o{ employment_records : "current_and_past"
    export_logs }o--|| employees : "exported_by"
```

---

### Group 4: Employee Profile & Career History
*Corresponding screens:* `Employee Profile - Updated Overview`, `Employee Profile - Employment Tab`, `Employee Directory - tab history`, `Employee Profile - History Tab`, `Employee Profile - History Tab-1`.

```mermaid
erDiagram
    employees {
        uuid id PK
        string employee_code UK
        string first_name
        string last_name
        string preferred_name
        string photo_url
        string work_email UK
        string personal_email
        string work_phone
        string personal_phone
        string gender
        string marital_status
        string nationality
        date date_of_birth
        string status
        date hire_date
        uuid user_id FK
    }

    employment_histories {
        uuid id PK
        uuid employee_id FK
        uuid request_id FK
        string category 
        string event_type
        string title
        text description
        jsonb old_values
        jsonb new_values
        date effective_date
        uuid performed_by_user_id FK
        datetime created_at
    }

    salary_histories {
        uuid id PK
        uuid employee_id FK
        decimal old_salary
        decimal new_salary
        string change_reason
        date effective_date
        uuid approved_by_user_id FK
    }

    employees ||--o{ employment_histories : "logs"
    employees ||--o{ salary_histories : "tracks"
```

---

### Group 5: Employment Contracts Management
*Corresponding screens:* `Employee directory - tab contract`, `Add New Contract Modal`, *Current Contract* section in `Employee Profile - Overview`.

```mermaid
erDiagram
    employees {
        uuid id PK
        string employee_code UK
        string first_name
        string last_name
        string preferred_name
        string photo_url
        string work_email UK
        string personal_email
        string work_phone
        string personal_phone
        string gender
        string marital_status
        string nationality
        date date_of_birth
        string status
        date hire_date
        uuid user_id FK
    }

    contract_types {
        uuid id PK
        string code UK
        string name
    }

    contract_templates {
        uuid id PK
        string template_name
        string template_type 
        string file_url
        boolean is_active
    }

    contracts {
        uuid id PK
        uuid employee_id FK
        string contract_number UK
        uuid contract_type_id FK
        uuid template_id FK
        string document_title
        date effective_date
        date expiry_date
        string status
        decimal base_salary
        string document_url
    }

    contract_allowances {
        uuid id PK
        uuid contract_id FK
        string allowance_name
        decimal amount
    }

    contract_types ||--o{ contracts : "defines"
    contract_templates ||--o{ contracts : "used_by"
    contracts ||--o{ contract_allowances : "includes"
    employees ||--o{ contracts : "signs"
```

---

### Group 6: Education, Certifications & Document Vault
*Corresponding screens:* `Employee Profile - Documents Tab`, `Employee Profile - Education & Certifications`.

```mermaid
erDiagram
    employees {
        uuid id PK
        string employee_code UK
        string first_name
        string last_name
        string preferred_name
        string photo_url
        string work_email UK
        string personal_email
        string work_phone
        string personal_phone
        string gender
        string marital_status
        string nationality
        date date_of_birth
        string status
        date hire_date
        uuid user_id FK
    }

    educations {
        uuid id PK
        uuid employee_id FK
        string degree_type
        string major
        string institution_name
        string location
        string start_year
        string end_year
        string status
    }

    certifications {
        uuid id PK
        uuid employee_id FK
        string certification_name
        string credential_id
        string issuing_organization
        date issue_date
        date expiry_date
        string status
    }

    training_records {
        uuid id PK
        uuid employee_id FK
        string program_name
        string provider
        date start_date
        integer duration_hours
        string result
        string status
    }

    document_types {
        uuid id PK
        string code UK
        string name
        boolean is_required
    }

    employee_documents {
        uuid id PK
        uuid employee_id FK
        uuid document_type_id FK
        string category 
        string document_name 
        string file_url
        datetime uploaded_at
        uuid uploaded_by_user_id FK
        date expiration_date
        string status
        datetime verified_at
    }

    employees ||--o{ educations : "has"
    employees ||--o{ certifications : "holds"
    employees ||--o{ training_records : "attends"
    employees ||--o{ employee_documents : "uploads"
    document_types ||--o{ employee_documents : "classifies"
```

---

### Group 7: Employee Requests & Approval Workflow Tracking
*Corresponding screens:* `RequestManagement.html.html.../Frame.png`, `TrackingRequest.html.html.../Frame.png`, `Frame.png`.

```mermaid
erDiagram
    employees {
        uuid id PK
        string employee_code UK
        string first_name
        string last_name
        string preferred_name
        string photo_url
        string work_email UK
        string personal_email
        string work_phone
        string personal_phone
        string gender
        string marital_status
        string nationality
        date date_of_birth
        string status
        date hire_date
        uuid user_id FK
    }

    request_types {
        uuid id PK
        string code UK
        string name
        integer default_sla_hours
    }

    requests {
        uuid id PK
        string request_code UK
        uuid requester_id FK
        uuid request_type_id FK
        uuid handover_assignee_id FK
        string title
        text reason_notes
        string contact_phone
        date start_date
        date end_date
        integer calculated_days
        string priority
        string current_stage
        string status
        datetime sla_deadline_at
        datetime submitted_at
    }

    request_attachments {
        uuid id PK
        uuid request_id FK
        string file_name
        string file_url
        string file_type
        datetime uploaded_at
    }

    approval_steps {
        uuid id PK
        uuid request_id FK
        integer step_number
        string step_name
        uuid assigned_approver_id FK
        string status
        datetime action_date
    }

    request_activity_logs {
        uuid id PK
        uuid request_id FK
        uuid actor_id FK
        string action
        text comment
        datetime created_at
    }

    leave_quotas {
        uuid id PK
        uuid employee_id FK
        string leave_type
        integer year
        integer total_days
        integer used_days
    }

    request_types ||--o{ requests : "defines"
    employees ||--o{ requests : "creates"
    employees ||--o{ requests : "handover_assignee"
    requests ||--o{ request_attachments : "includes"
    requests ||--o{ approval_steps : "workflow"
    requests ||--o{ request_activity_logs : "timeline"
    employees ||--o{ approval_steps : "assigned_approver"
    employees ||--o{ request_activity_logs : "actor"
    employees ||--o{ leave_quotas : "has_quota"
```

---

## UI/X Design

https://www.figma.com/design/lVYyY2rxIvDSMv5dHhUPAn/HR-Platform?node-id=0-1&t=s2k7yUaYU5qM7xkB-1

## Information Architecture

https://www.relume.ai/app/project/P3510455_YXELzSJDc8T2OjU7C3qlEKHothe_wKvotzCyXKyHarU#mode=sitemap

![alt text](docs/assets/IA/sitemap.png)

### People Management

![alt text](docs/assets/IA/Process-1.png)

![alt text](docs/assets/IA/Process-2.png)

![alt text](docs/assets/IA/Process-3.png)

![alt text](docs/assets/IA/IA-1.png)

![alt text](docs/assets/IA/IA-2.png)

![alt text](docs/assets/IA/IA-3.png)

![alt text](docs/assets/IA/IA-4.png)

## 0. Main Mindmap

```mermaid
flowchart LR
    classDef root fill:#dbeafe,stroke:#2563eb,stroke-width:4px,color:#111,font-weight:bold;
    classDef leftModule fill:#bfdbfe,stroke:#1d4ed8,stroke-width:2px,color:#111,font-weight:bold;
    classDef rightModule fill:#a7f3d0,stroke:#15803d,stroke-width:2px,color:#111,font-weight:bold;
    classDef importantModule fill:#fef08a,stroke:#dc2626,stroke-width:3px,color:#7f1d1d,font-weight:bold;
    classDef feature fill:#ffffff,stroke:#f59e0b,stroke-width:2px,color:#111,font-weight:bold;

    P(("HR & Workforce<br/>Management Platform"))

    M1["1. Authentication &<br/>Access Control"] --- P
    M2["2. Organization<br/>Management"] --- P
    M3["3. Employee<br/>Management"] --- P
    M4["4. Recruitment &<br/>Employee Lifecycle"] --- P

    A11["Authentication"] --- M1
    A12["Account Management"] --- M1
    A13["Authorization"] --- M1

    A21["Company Structure"] --- M2
    A22["Organization Hierarchy"] --- M2
    A23["Work Locations"] --- M2

    A31["Employee Directory"] --- M3
    A32["Employee Profile"] --- M3
    A33["Employment Records"] --- M3
    A34["Documents & Self-Service"] --- M3

    A41["Job Management"] --- M4
    A42["Candidate Management"] --- M4
    A43["Onboarding"] --- M4
    A44["Offboarding"] --- M4

    P --- M5["5. Time, Attendance<br/>& Leave"]
    P --- M6["6. Project & Task<br/>Management"]
    P --- M7["7. Productivity<br/>Monitoring"]
    P --- M8["8. Payroll &<br/>Performance"]
    P --- M9["9. Reports & System<br/>Administration"]

    M5 --- A51["Time Tracking"]
    M5 --- A52["Attendance"]
    M5 --- A53["Timesheets"]
    M5 --- A54["Work Scheduling"]
    M5 --- A55["Leave Management"]

    M6 --- A61["Project Management"]
    M6 --- A62["Task Management"]
    M6 --- A63["Resources & Budget"]

    M7 --- A71["Activity Tracking"]
    M7 --- A72["Computer Monitoring"]
    M7 --- A73["Location Tracking"]
    M7 --- A74["Monitoring Policies"]

    M8 --- A81["Payroll"]
    M8 --- A82["Compensation & Benefits"]
    M8 --- A83["Performance Management"]

    M9 --- A91["Dashboard"]
    M9 --- A92["Reports & Analytics"]
    M9 --- A93["Notifications & Workflows"]
    M9 --- A94["Administration & Audit"]

    class P root;
    class M1,M2,M4 leftModule;
    class M8,M9 rightModule;
    class M3,M5,M6,M7 importantModule;
    class A11,A12,A13,A21,A22,A23,A31,A32,A33,A34,A41,A42,A43,A44 feature;
    class A51,A52,A53,A54,A55,A61,A62,A63,A71,A72,A73,A74,A81,A82,A83,A91,A92,A93,A94 feature;
```

---

## 1. Authentication & Access Control

```mermaid
flowchart LR
    classDef module fill:#fca5a5,stroke:#ef4444,stroke-width:3px,color:#111,font-weight:bold;
    classDef feature fill:#ffffff,stroke:#f59e0b,stroke-width:2px,color:#111,font-weight:bold;
    classDef important fill:#fef08a,stroke:#dc2626,stroke-width:3px,color:#7f1d1d,font-weight:bold;
    classDef usecase fill:#f8fafc,stroke:#cbd5e1,color:#111;

    AUTH["Authentication &<br/>Access Control"]

    AUTH --> LOGIN["Authentication"]
    AUTH --> ACCOUNT["Account Management"]
    AUTH --> ACCESS["Authorization"]

    LOGIN --> L1["Login / Logout"]
    LOGIN --> L2["Reset Password"]
    LOGIN --> L3["Refresh Session"]

    ACCOUNT --> AC1["Create and Link User Account"]
    ACCOUNT --> AC2["Activate, Deactivate or Lock Account"]

    ACCESS --> AU1["Assign Roles"]
    ACCESS --> AU2["Configure Permissions and Access Scope"]

    class AUTH module;
    class LOGIN,ACCOUNT,ACCESS feature;
    class LOGIN,ACCESS important;
    class L1,L2,L3,AC1,AC2,AU1,AU2 usecase;
```

---

## 2. Organization Management

```mermaid
flowchart LR
    classDef module fill:#fca5a5,stroke:#ef4444,stroke-width:3px,color:#111,font-weight:bold;
    classDef feature fill:#ffffff,stroke:#f59e0b,stroke-width:2px,color:#111,font-weight:bold;
    classDef important fill:#fef08a,stroke:#dc2626,stroke-width:3px,color:#7f1d1d,font-weight:bold;
    classDef usecase fill:#f8fafc,stroke:#cbd5e1,color:#111;

    ORG["Organization Management"]

    ORG --> STRUCTURE["Company Structure"]
    ORG --> HIERARCHY["Organization Hierarchy"]
    ORG --> LOCATION["Work Locations"]

    STRUCTURE --> S1["Manage Company, Departments and Teams"]
    STRUCTURE --> S2["Define Job Positions"]

    HIERARCHY --> H1["Assign Reporting Manager"]
    HIERARCHY --> H2["Assign Employee to Organization Unit"]
    HIERARCHY --> H3["View Organization Chart"]

    LOCATION --> W1["Manage Work Locations and Work Modes"]

    class ORG module;
    class STRUCTURE,HIERARCHY,LOCATION feature;
    class STRUCTURE,HIERARCHY important;
    class S1,S2,H1,H2,H3,W1 usecase;
```

---

## 3. Employee Management

```mermaid
flowchart LR
    classDef module fill:#fca5a5,stroke:#ef4444,stroke-width:3px,color:#111,font-weight:bold;
    classDef feature fill:#ffffff,stroke:#f59e0b,stroke-width:2px,color:#111,font-weight:bold;
    classDef important fill:#fef08a,stroke:#dc2626,stroke-width:3px,color:#7f1d1d,font-weight:bold;
    classDef usecase fill:#f8fafc,stroke:#cbd5e1,color:#111;

    EMP["Employee Management"]

    EMP --> DIRECTORY["Employee Directory"]
    EMP --> PROFILE["Employee Profile"]
    EMP --> RECORDS["Employment Records"]
    EMP --> DOCUMENTS["Documents & Self-Service"]

    DIRECTORY --> D1["View and Search Employee Directory"]

    PROFILE --> P1["Create Employee Profile"]
    PROFILE --> P2["View or Update Personal and Contact Information"]
    PROFILE --> P3["Manage Emergency Contacts"]

    RECORDS --> R1["Assign Department, Position and Manager"]
    RECORDS --> R2["Update Employment Status"]
    RECORDS --> R3["Promote or Transfer Employee"]
    RECORDS --> R4["View Employment History"]

    DOCUMENTS --> DS1["Manage Employee Documents"]
    DOCUMENTS --> DS2["Request Electronic Signature or Policy Acknowledgement"]

    class EMP module;
    class DIRECTORY,PROFILE,RECORDS,DOCUMENTS feature;
    class PROFILE,RECORDS important;
    class D1,P1,P2,P3,R1,R2,R3,R4,DS1,DS2 usecase;
```

---

## 4. Recruitment & Employee Lifecycle

```mermaid
flowchart LR
    classDef module fill:#fca5a5,stroke:#ef4444,stroke-width:3px,color:#111,font-weight:bold;
    classDef feature fill:#ffffff,stroke:#f59e0b,stroke-width:2px,color:#111,font-weight:bold;
    classDef important fill:#fef08a,stroke:#dc2626,stroke-width:3px,color:#7f1d1d,font-weight:bold;
    classDef usecase fill:#f8fafc,stroke:#cbd5e1,color:#111;

    LIFE["Recruitment &<br/>Employee Lifecycle"]

    LIFE --> JOB["Job Management"]
    LIFE --> CANDIDATE["Candidate Management"]
    LIFE --> ONBOARDING["Onboarding"]
    LIFE --> OFFBOARDING["Offboarding"]

    JOB --> J1["Create and Publish Job Opening"]
    JOB --> J2["Close Job Opening"]

    CANDIDATE --> C1["Screen Candidate Application"]
    CANDIDATE --> C2["Schedule Interview and Record Evaluation"]
    CANDIDATE --> C3["Send Job Offer"]
    CANDIDATE --> C4["Convert Candidate to Employee"]

    ONBOARDING --> O1["Complete Onboarding Checklist and Documents"]
    ONBOARDING --> O2["Provision Account and Organization Assignment"]

    OFFBOARDING --> F1["Complete Handover, Asset Return and Access Revocation"]

    class LIFE module;
    class JOB,CANDIDATE,ONBOARDING,OFFBOARDING feature;
    class CANDIDATE,ONBOARDING important;
    class J1,J2,C1,C2,C3,C4,O1,O2,F1 usecase;
```

---

## 5. Time, Attendance & Leave

```mermaid
flowchart LR
    classDef module fill:#fca5a5,stroke:#ef4444,stroke-width:3px,color:#111,font-weight:bold;
    classDef feature fill:#ffffff,stroke:#f59e0b,stroke-width:2px,color:#111,font-weight:bold;
    classDef important fill:#fef08a,stroke:#dc2626,stroke-width:3px,color:#7f1d1d,font-weight:bold;
    classDef usecase fill:#f8fafc,stroke:#cbd5e1,color:#111;

    TIME["Time, Attendance & Leave"]

    TIME --> TRACKING["Time Tracking"]
    TIME --> ATTENDANCE["Attendance"]
    TIME --> TIMESHEET["Timesheets"]
    TIME --> SCHEDULE["Work Scheduling"]
    TIME --> LEAVE["Leave Management"]

    TRACKING --> T1["Start or Stop Timer"]
    TRACKING --> T2["Select Project and Task"]
    TRACKING --> T3["Add or Edit Manual Time Entry"]

    ATTENDANCE --> A1["Check In or Check Out"]
    ATTENDANCE --> A2["Track Break and Overtime"]
    ATTENDANCE --> A3["Request Attendance Correction"]

    TIMESHEET --> TS1["View and Submit Timesheet"]
    TIMESHEET --> TS2["Approve or Reject Timesheet"]
    TIMESHEET --> TS3["Lock or Reopen Timesheet"]

    SCHEDULE --> S1["Create and Assign Work Shift"]
    SCHEDULE --> S2["View Team Schedule"]

    LEAVE --> L1["Submit Leave Request"]
    LEAVE --> L2["Approve or Reject Leave and View Balance"]

    class TIME module;
    class TRACKING,ATTENDANCE,TIMESHEET,SCHEDULE,LEAVE feature;
    class TRACKING,TIMESHEET,LEAVE important;
    class T1,T2,T3,A1,A2,A3,TS1,TS2,TS3,S1,S2,L1,L2 usecase;
```

---

## 6. Project & Task Management

```mermaid
flowchart LR
    classDef module fill:#fca5a5,stroke:#ef4444,stroke-width:3px,color:#111,font-weight:bold;
    classDef feature fill:#ffffff,stroke:#f59e0b,stroke-width:2px,color:#111,font-weight:bold;
    classDef important fill:#fef08a,stroke:#dc2626,stroke-width:3px,color:#7f1d1d,font-weight:bold;
    classDef usecase fill:#f8fafc,stroke:#cbd5e1,color:#111;

    PROJECTS["Project & Task Management"]

    PROJECTS --> PROJECT["Project Management"]
    PROJECTS --> TASK["Task Management"]
    PROJECTS --> RESOURCE["Resources & Budget"]

    PROJECT --> P1["Create or Update Project"]
    PROJECT --> P2["Assign Project Manager and Members"]
    PROJECT --> P3["View Project Status and Progress"]

    TASK --> T1["Create and Assign Task"]
    TASK --> T2["Set Priority, Estimate and Deadline"]
    TASK --> T3["Update Task Status and Track Time"]

    RESOURCE --> R1["View Member Workload"]
    RESOURCE --> R2["Set and Monitor Budget with Alerts"]

    class PROJECTS module;
    class PROJECT,TASK,RESOURCE feature;
    class PROJECT,TASK important;
    class P1,P2,P3,T1,T2,T3,R1,R2 usecase;
```

---

## 7. Productivity Monitoring

```mermaid
flowchart LR
    classDef module fill:#fca5a5,stroke:#ef4444,stroke-width:3px,color:#111,font-weight:bold;
    classDef feature fill:#ffffff,stroke:#f59e0b,stroke-width:2px,color:#111,font-weight:bold;
    classDef important fill:#fef08a,stroke:#dc2626,stroke-width:3px,color:#7f1d1d,font-weight:bold;
    classDef usecase fill:#f8fafc,stroke:#cbd5e1,color:#111;

    PRODUCTIVITY["Productivity Monitoring"]

    PRODUCTIVITY --> ACTIVITY["Activity Tracking"]
    PRODUCTIVITY --> COMPUTER["Computer Monitoring"]
    PRODUCTIVITY --> LOCATION["Location Tracking"]
    PRODUCTIVITY --> POLICY["Monitoring Policies"]

    ACTIVITY --> A1["View Activity Level and Active Time"]
    ACTIVITY --> A2["Detect Idle Time and Working Status"]

    COMPUTER --> C1["View Screenshot Timeline"]
    COMPUTER --> C2["Track Application and Website Usage"]

    LOCATION --> L1["Track GPS and Geofence"]
    LOCATION --> L2["View Location History"]

    POLICY --> P1["Configure Monitoring Policies"]

    class PRODUCTIVITY module;
    class ACTIVITY,COMPUTER,LOCATION,POLICY feature;
    class ACTIVITY,COMPUTER important;
    class A1,A2,C1,C2,L1,L2,P1 usecase;
```

---

## 8. Payroll & Performance

```mermaid
flowchart LR
    classDef module fill:#fca5a5,stroke:#ef4444,stroke-width:3px,color:#111,font-weight:bold;
    classDef feature fill:#ffffff,stroke:#f59e0b,stroke-width:2px,color:#111,font-weight:bold;
    classDef important fill:#fef08a,stroke:#dc2626,stroke-width:3px,color:#7f1d1d,font-weight:bold;
    classDef usecase fill:#f8fafc,stroke:#cbd5e1,color:#111;

    PAYPERF["Payroll & Performance"]

    PAYPERF --> PAYROLL["Payroll"]
    PAYPERF --> COMPENSATION["Compensation & Benefits"]
    PAYPERF --> PERFORMANCE["Performance Management"]

    PAYROLL --> P1["Configure Salary or Hourly Rate"]
    PAYROLL --> P2["Import Approved Hours and Calculate Overtime"]
    PAYROLL --> P3["Calculate and Review Payroll"]
    PAYROLL --> P4["Record Payment and View History"]

    COMPENSATION --> C1["Manage Compensation"]
    COMPENSATION --> C2["Manage Benefits"]

    PERFORMANCE --> F1["Manage Employee Goals"]
    PERFORMANCE --> F2["Conduct Performance Reviews and Feedback"]

    class PAYPERF module;
    class PAYROLL,COMPENSATION,PERFORMANCE feature;
    class PAYROLL,PERFORMANCE important;
    class P1,P2,P3,P4,C1,C2,F1,F2 usecase;
```

---

## 9. Reports & System Administration

```mermaid
flowchart LR
    classDef module fill:#fca5a5,stroke:#ef4444,stroke-width:3px,color:#111,font-weight:bold;
    classDef feature fill:#ffffff,stroke:#f59e0b,stroke-width:2px,color:#111,font-weight:bold;
    classDef important fill:#fef08a,stroke:#dc2626,stroke-width:3px,color:#7f1d1d,font-weight:bold;
    classDef usecase fill:#f8fafc,stroke:#cbd5e1,color:#111;

    REPORTS["Reports &<br/>System Administration"]

    REPORTS --> DASHBOARD["Dashboard"]
    REPORTS --> ANALYTICS["Reports & Analytics"]
    REPORTS --> WORKFLOW["Notifications & Workflows"]
    REPORTS --> ADMIN["Administration & Audit"]

    DASHBOARD --> D1["View HR and Workforce Dashboard"]
    DASHBOARD --> D2["View Project, Productivity and Payroll Summary"]

    ANALYTICS --> A1["Generate and Export Reports"]

    WORKFLOW --> W1["Configure Approval Workflows"]
    WORKFLOW --> W2["Send Reminders and Status Notifications"]

    ADMIN --> AD1["Configure System Settings and Integrations"]
    ADMIN --> AD2["View Audit Logs and Change History"]

    class REPORTS module;
    class DASHBOARD,ANALYTICS,WORKFLOW,ADMIN feature;
    class DASHBOARD,ANALYTICS important;
    class D1,D2,A1,W1,W2,AD1,AD2 usecase;
```

---

# System Actors and Roles

## 1. Overview

The platform includes seven main roles. `Recruiter` is merged into `HR Staff` because recruitment is treated as a specialized HR responsibility.

| Role | Main Responsibility |
|---|---|
| Candidate | Participates in recruitment before becoming an employee |
| Employee | Uses self-service and performs daily workforce activities |
| Team Lead | Manages employees, attendance, approvals and team performance |
| Project Manager | Manages projects, tasks, resources and budgets |
| HR Staff | Manages organization data, employees, recruitment and employee lifecycle |
| Accountant | Manages payroll, compensation and benefits |
| System Administrator | Manages accounts, permissions, policies, integrations and audits |

---

## 2. Candidate

- View job openings and submit an application.
- Update candidate information and application documents.
- View interview schedules and job-offer status.
- Accept an offer and submit onboarding information.

---

## 3. Employee

- View and update personal information.
- View employee documents and company policies.
- Track time by project and task.
- Check in, check out and submit attendance corrections.
- View and submit timesheets.
- View schedules and request leave.
- Update assigned task status.
- View personal productivity, goals and payment history.

---

## 4. Team Lead

A team lead also has normal employee permissions within the permitted access scope.

- View direct reports and team workforce status.
- Approve timesheets, leave and attendance corrections.
- Create shifts and view the team schedule.
- Assign tasks and monitor workload.
- Review working hours, attendance and productivity.
- Review employee goals and provide performance feedback.
- View team dashboards and reports.

---

## 5. Project Manager

A project manager may also have normal employee permissions.

- Create and update projects.
- Assign project managers, members and tasks.
- Set task priorities, estimates and deadlines.
- Monitor task status and tracked hours.
- View project progress and member workload.
- Configure and monitor hour or cost budgets.
- Receive budget alerts and generate project reports.

---

## 6. HR Staff

HR Staff includes recruitment responsibilities instead of using a separate Recruiter role.

- Manage company structure, departments, teams, positions and locations.
- Create and maintain employee profiles and employment records.
- Manage employee documents and policy acknowledgements.
- Create and publish job openings.
- Screen candidates, schedule interviews and send offers.
- Convert accepted candidates into employees.
- Coordinate onboarding and offboarding.
- Configure leave policies and generate HR reports.

---

## 7. Accountant

- Configure salary and hourly rates.
- Retrieve approved working hours and overtime.
- Calculate and review payroll.
- Record payments and view payment history.
- Manage compensation, allowances and benefits.
- Generate or export payroll reports.

---

## 8. System Administrator

- Create, activate, deactivate or lock user accounts.
- Assign roles and configure permissions or access scopes.
- Configure authentication and session settings.
- Configure productivity-monitoring policies.
- Configure approval workflows and system settings.
- Manage external integrations.
- Review audit logs and data-change history.

---

# HR & Workforce Management Platform — Use Case Catalogue

> Actor assignments are mapped from the role responsibilities defined in the system mindmap.  
> The catalogue contains **75 high-level use cases** across **9 modules**.

---

# P0 — Critical: Required for the MVP

These use cases establish the core flow of the system:

**Account → Organization → Employee → Project and Task → Time Tracking → Timesheet Approval**

| Priority Order | Use Case ID | Module | High-Level Use Case | Main Actors |
|---:|---|---|---|---|
| 1 | `UC-AUTH-01` | Authentication & Access Control | Login / Logout | All platform users |
| 2 | `UC-AUTH-04` | Authentication & Access Control | Create and Link User Account | HR Staff; System Administrator |
| 3 | `UC-AUTH-06` | Authentication & Access Control | Assign Roles | System Administrator |
| 4 | `UC-AUTH-07` | Authentication & Access Control | Configure Permissions and Access Scope | System Administrator |
| 5 | `UC-AUTH-05` | Authentication & Access Control | Activate, Deactivate or Lock Account | HR Staff; System Administrator |
| 6 | `UC-AUTH-02` | Authentication & Access Control | Reset Password | All platform users |
| 7 | `UC-AUTH-03` | Authentication & Access Control | Refresh Session | All platform users |
| 8 | `UC-ORG-01` | Organization Management | Manage Company, Departments and Teams | HR Staff; System Administrator |
| 9 | `UC-ORG-02` | Organization Management | Define Job Positions | HR Staff |
| 10 | `UC-EMP-02` | Employee Management | Create Employee Profile | HR Staff |
| 11 | `UC-EMP-05` | Employee Management | Assign Department, Position and Manager | HR Staff |
| 12 | `UC-ORG-04` | Organization Management | Assign Employee to Organization Unit | HR Staff |
| 13 | `UC-ORG-03` | Organization Management | Assign Reporting Manager | HR Staff |
| 14 | `UC-EMP-06` | Employee Management | Update Employment Status | HR Staff |
| 15 | `UC-EMP-01` | Employee Management | View and Search Employee Directory | Employee; Team Lead; HR Staff |
| 16 | `UC-PROJ-01` | Project & Task Management | Create or Update Project | Team Lead; Project Manager |
| 17 | `UC-PROJ-02` | Project & Task Management | Assign Project Manager and Members | Team Lead; Project Manager |
| 18 | `UC-PROJ-04` | Project & Task Management | Create and Assign Task | Team Lead; Project Manager |
| 19 | `UC-TIME-02` | Time, Attendance & Leave | Select Project and Task | Employee |
| 20 | `UC-TIME-01` | Time, Attendance & Leave | Start or Stop Timer | Employee |
| 21 | `UC-TIME-03` | Time, Attendance & Leave | Add or Edit Manual Time Entry | Employee |
| 22 | `UC-PROJ-06` | Project & Task Management | Update Task Status and Track Time | Employee; Team Lead; Project Manager |
| 23 | `UC-TIME-04` | Time, Attendance & Leave | Check In or Check Out | Employee |
| 24 | `UC-TIME-07` | Time, Attendance & Leave | View and Submit Timesheet | Employee |
| 25 | `UC-TIME-08` | Time, Attendance & Leave | Approve or Reject Timesheet | Team Lead; HR Staff |

---

# P1 — High: Important Immediately After the MVP

These use cases complete major operational workflows, including attendance correction, leave, scheduling, payroll, recruitment, and basic productivity monitoring.

| Priority Order | Use Case ID | Module | High-Level Use Case | Main Actors |
|---:|---|---|---|---|
| 26 | `UC-TIME-09` | Time, Attendance & Leave | Lock or Reopen Timesheet | Team Lead; HR Staff |
| 27 | `UC-TIME-06` | Time, Attendance & Leave | Request Attendance Correction | Employee; Team Lead; HR Staff |
| 28 | `UC-TIME-05` | Time, Attendance & Leave | Track Break and Overtime | Employee; Team Lead |
| 29 | `UC-TIME-12` | Time, Attendance & Leave | Submit Leave Request | Employee |
| 30 | `UC-TIME-13` | Time, Attendance & Leave | Approve or Reject Leave and View Balance | Employee; Team Lead; HR Staff |
| 31 | `UC-PROJ-05` | Project & Task Management | Set Priority, Estimate and Deadline | Team Lead; Project Manager |
| 32 | `UC-PROJ-03` | Project & Task Management | View Project Status and Progress | Employee; Team Lead; Project Manager |
| 33 | `UC-PROJ-07` | Project & Task Management | View Member Workload | Team Lead; Project Manager |
| 34 | `UC-TIME-10` | Time, Attendance & Leave | Create and Assign Work Shift | Team Lead; HR Staff |
| 35 | `UC-TIME-11` | Time, Attendance & Leave | View Team Schedule | Employee; Team Lead; HR Staff |
| 36 | `UC-PROD-07` | Productivity Monitoring | Configure Monitoring Policies | HR Staff; System Administrator |
| 37 | `UC-PROD-01` | Productivity Monitoring | View Activity Level and Active Time | Employee; Team Lead |
| 38 | `UC-PROD-02` | Productivity Monitoring | Detect Idle Time and Working Status | Employee; Team Lead |
| 39 | `UC-ADMIN-07` | Reports & System Administration | View Audit Logs and Change History | HR Staff; System Administrator |
| 40 | `UC-ADMIN-06` | Reports & System Administration | Configure System Settings and Integrations | System Administrator |
| 41 | `UC-PAY-01` | Payroll & Performance | Configure Salary or Hourly Rate | HR Staff; Accountant |
| 42 | `UC-PAY-02` | Payroll & Performance | Import Approved Hours and Calculate Overtime | Accountant |
| 43 | `UC-PAY-03` | Payroll & Performance | Calculate and Review Payroll | Accountant |
| 44 | `UC-PAY-04` | Payroll & Performance | Record Payment and View History | Employee; Accountant |
| 45 | `UC-REC-01` | Recruitment & Employee Lifecycle | Create and Publish Job Opening | HR Staff |
| 46 | `UC-REC-03` | Recruitment & Employee Lifecycle | Screen Candidate Application | HR Staff |
| 47 | `UC-REC-04` | Recruitment & Employee Lifecycle | Schedule Interview and Record Evaluation | Candidate; Team Lead; HR Staff |
| 48 | `UC-REC-05` | Recruitment & Employee Lifecycle | Send Job Offer | Candidate; HR Staff |
| 49 | `UC-REC-06` | Recruitment & Employee Lifecycle | Convert Candidate to Employee | HR Staff |
| 50 | `UC-REC-08` | Recruitment & Employee Lifecycle | Provision Account and Organization Assignment | HR Staff; System Administrator |

---

# P2 — Medium: Completes HR and Workforce Operations

These use cases provide a more complete employee lifecycle, workforce administration, benefits, performance management, and detailed monitoring capabilities.

| Priority Order | Use Case ID | Module | High-Level Use Case | Main Actors |
|---:|---|---|---|---|
| 51 | `UC-REC-07` | Recruitment & Employee Lifecycle | Complete Onboarding Checklist and Documents | Candidate; Employee; Team Lead; HR Staff |
| 52 | `UC-REC-09` | Recruitment & Employee Lifecycle | Complete Handover, Asset Return and Access Revocation | Employee; Team Lead; HR Staff; System Administrator |
| 53 | `UC-REC-02` | Recruitment & Employee Lifecycle | Close Job Opening | HR Staff |
| 54 | `UC-EMP-03` | Employee Management | View or Update Personal and Contact Information | Employee; HR Staff |
| 55 | `UC-EMP-09` | Employee Management | Manage Employee Documents | Employee; HR Staff |
| 56 | `UC-EMP-10` | Employee Management | Request Electronic Signature or Policy Acknowledgement | Employee; HR Staff |
| 57 | `UC-EMP-07` | Employee Management | Promote or Transfer Employee | Team Lead; HR Staff |
| 58 | `UC-EMP-08` | Employee Management | View Employment History | Employee; Team Lead; HR Staff |
| 59 | `UC-EMP-04` | Employee Management | Manage Emergency Contacts | Employee; HR Staff |
| 60 | `UC-ORG-06` | Organization Management | Manage Work Locations and Work Modes | HR Staff; System Administrator |
| 61 | `UC-ORG-05` | Organization Management | View Organization Chart | Employee; Team Lead; Project Manager; HR Staff |
| 62 | `UC-PAY-05` | Payroll & Performance | Manage Compensation | HR Staff; Accountant |
| 63 | `UC-PAY-06` | Payroll & Performance | Manage Benefits | Employee; HR Staff; Accountant |
| 64 | `UC-PAY-07` | Payroll & Performance | Manage Employee Goals | Employee; Team Lead; HR Staff |
| 65 | `UC-PAY-08` | Payroll & Performance | Conduct Performance Reviews and Feedback | Employee; Team Lead; HR Staff |
| 66 | `UC-PROD-03` | Productivity Monitoring | View Screenshot Timeline | Employee; Team Lead |
| 67 | `UC-PROD-04` | Productivity Monitoring | Track Application and Website Usage | Employee; Team Lead |
| 68 | `UC-PROD-05` | Productivity Monitoring | Track GPS and Geofence | Employee; Team Lead; System Administrator |
| 69 | `UC-PROD-06` | Productivity Monitoring | View Location History | Employee; Team Lead; HR Staff |

---

# P3 — Low: Advanced and Supporting Capabilities

These use cases should be implemented after core operational data and workflows are stable.

| Priority Order | Use Case ID | Module | High-Level Use Case | Main Actors |
|---:|---|---|---|---|
| 70 | `UC-ADMIN-05` | Reports & System Administration | Send Reminders and Status Notifications | HR Staff; System Administrator |
| 71 | `UC-ADMIN-04` | Reports & System Administration | Configure Approval Workflows | HR Staff; System Administrator |
| 72 | `UC-ADMIN-01` | Reports & System Administration | View HR and Workforce Dashboard | Team Lead; HR Staff; System Administrator |
| 73 | `UC-ADMIN-02` | Reports & System Administration | View Project, Productivity and Payroll Summary | Team Lead; Project Manager; HR Staff; Accountant; System Administrator |
| 74 | `UC-ADMIN-03` | Reports & System Administration | Generate and Export Reports | Team Lead; Project Manager; HR Staff; Accountant; System Administrator |
| 75 | `UC-PROJ-08` | Project & Task Management | Set and Monitor Budget with Alerts | Team Lead; Project Manager |

---

## Usecase Diagram

### Authentication & Access Control

![alt text](docs/assets/usecase/HR-WorkFoce-Usecase-Authentication-Access-Control-v2.drawio.png)

### Organization Management

![alt text](docs/assets/usecase/HR-WorkFoce-Usecase-Organization-Management-v2.drawio.png)

### Employee Management

![alt text](docs/assets/usecase/HR-WorkFoce-Usecase-Employee-Management-v2.drawio.png)

### Time & Attendance Management

![alt text](docs/assets/usecase/HR-WorkFoce-Usecase-Time-Attendance-Leave-v2.drawio.png)

### Project & Task Management

![alt text](docs/assets/usecase/HR-WorkFoce-Usecase-Project-Task-Management-v2.drawio.png)

### Payroll & Performance

![alt text](docs/assets/usecase/HR-WorkFoce-Usecase-Payroll-Performance-v2.drawio.png)

### Recruitment & Employee Lifecycle

![alt text](docs/assets/usecase/HR-WorkFoce-Usecase-Recruitment-Employee-Lifecycle-v2.drawio.png)

### Productivity Monitoring

![alt text](docs/assets/usecase/HR-WorkFoce-Usecase-Productivity-Monitoring-v2.drawio.png)

### Reports & System Administration

![alt text](docs/assets/usecase/HR-WorkFoce-Usecase-Reports-System-Administration-v2.drawio.png)

# Process User Interact

## Promote or Transfer Employee

![alt text](docs/assets/swimlands/promotion.png)

## Electronic Signature or Policy Acknowledgement

![alt text](docs/assets/swimlands/Electronic-Signature-Policy-Acknowledgement.png)

## Schedule Interview and Record Evaluation

![alt text](docs/assets/swimlands/Schedule-Interview-Record-Evaluation.png)

## Send and Negotiate Job Offer

![alt text](docs/assets/swimlands/Send-Negotiate-Job-Offer.png)

## Onboarding and Account Provisioning

![alt text](docs/assets/swimlands/Onboarding-Account-Provisioning.png)

## Offboarding, Handover, Asset Return and Access Revocation

![alt text](docs/assets/swimlands/Offboarding-Handover-Asset-Return-Access-Revocation.png)

## Attendance Correction

![alt text](docs/assets/swimlands/Attendance-Correction.png)

## Timesheet Submission, Approval, Reopen and Lock

![alt text](docs/assets/swimlands/Timesheet-Submission-Approval-Reopen-Lock.png)

## Leave Request and Approval

![alt text](docs/assets/swimlands/Leave-Request-Approval.png)

## Create and Assign Work Shift

![alt text](docs/assets/swimlands/Create-Assign-Work-Shift.png)

## Task Assignment, Progress and Time Tracking

![alt text](docs/assets/swimlands/Task-Assignment-Progress-Time-Tracking.png)

## Compensation Change and Approval

![alt text](docs/assets/swimlands/Compensation-Change-Approval.png)

## Benefit Enrollment

![alt text](docs/assets/swimlands/Benefit-Enrollment.png)

## Employee Goals and Performance Review

![alt text](docs/assets/swimlands/Employee-Goals-Performance-Review.png)

---

---

# Entity Relationship Diagram (ERD) - Project Management

This section describes the ERD for the **Project Management** module, derived from 22 UI screens located in `docs/assets/UI-Project-Management`.

> **Nguyên tắc thiết kế:**
> - **Không lưu** các trường có thể **tính toán** từ dữ liệu khác: `total_hours`, `remaining_budget`, `team_size`, `avg_allocation_%`, `available_capacity_%`, `active_time`, `idle_time`, `total_labor_cost`, `sprint_progress_%`, `weekly_total`, `diff_hours`.
> - **Không lưu** trạng thái phái sinh: `capacity_status` (Available / Fully Allocated / Over Capacity) — tính real-time từ tổng `allocation_%`.
> - **Lưu** các giá trị gốc mà người dùng nhập hoặc hệ thống ghi nhận trực tiếp: giờ làm thực tế, tỷ lệ allocation được chỉ định, budget gốc, threshold cảnh báo.

---

## 1. High-Level ERD (Overview)

```mermaid
erDiagram
    %% Core
    employees ||--o{ project_members       : "assigned_to"
    projects  ||--o{ project_members       : "has"
    employees ||--o| employees             : "manages (manager_id)"

    %% Time Tracking
    employees  ||--o{ timesheets           : "submits"
    timesheets ||--o{ time_entries         : "contains"
    projects   ||--o{ time_entries         : "logged_against"

    %% Budget
    projects ||--o{ budget_adjustments     : "adjusted_by"

    %% Productivity Monitoring
    employees ||--o{ productivity_sessions : "tracked"
    productivity_sessions ||--o{ productivity_app_logs     : "contains"
    productivity_sessions ||--o{ productivity_website_logs : "contains"
    productivity_sessions ||--o{ productivity_screenshots  : "captures"
```

---

## 2. Detailed ERD by Feature Group

### Group 1: Projects (Quản lý dự án)
*Screens:* `PM_ Projects Dashboard`, `PM_ Create Project Form`, `PM_ Project Detail View`

```mermaid
erDiagram
    projects {
        uuid        id                      PK
        string      code                    UK "e.g. PRJ-2024-001"
        string      name
        text        description
        string      status                     "Planning | Active | On Hold | Completed | Cancelled"
        uuid        manager_id              FK "→ employees.id"
        date        start_date
        date        end_date
        decimal     total_budget               "Tổng ngân sách phân bổ ($)"
        decimal     labor_budget               "Ngân sách nhân công ($)"
        integer     budget_warning_threshold   "% cảnh báo, default 85"
        integer     expected_team_size_fte     "FTE kế hoạch"
        integer     default_weekly_capacity_h  "Giờ/tuần mặc định, e.g. 40"
        string      client                     "Internal R&D | tên client"
        string      priority                   "Low | Medium | High | Critical"
        string      tech_stack                 "e.g. React, Node.js, Python"
        datetime    created_at
        datetime    updated_at
    }

    employees {
        uuid   id          PK
        string first_name
        string last_name
        string employee_code
    }

    projects }o--|| employees : "managed_by"
```

---

### Group 2: Project Members & Resource Allocation (Thành viên & phân bổ nguồn lực)
*Screens:* `PM_ Project Members List`, `PM_ Add Project Member Drawer`, `PM_ Resource Allocation`, `PM_ Project Capacity View`, `Employee_ My Project`, `HR_ Resource Overview`, `HR_ Capacity Overview`

```mermaid
erDiagram
    project_members {
        uuid    id              PK
        uuid    project_id      FK "→ projects.id"
        uuid    employee_id     FK "→ employees.id"
        string  project_role       "e.g. Backend Developer, UX Designer"
        integer allocation_pct     "% phân bổ cho dự án này (do PM nhập)"
        date    start_date
        date    end_date           "NULL = không xác định"
        datetime joined_at
        datetime updated_at
    }

    %% Ghi chú:
    %% - overall_allocation_%  = SUM(allocation_pct) across all active assignments → tính toán
    %% - available_capacity_%  = 100 - overall_allocation_%  → tính toán
    %% - capacity_status       = Available / Fully Allocated / Over Capacity → tính toán
    %% - weekly_hours          = allocation_pct * employee.default_weekly_hours → tính toán

    projects  ||--o{ project_members : "has"
    employees ||--o{ project_members : "assigned_in"
```

---

### Group 3: Time Tracking & Timesheets (Chấm công)
*Screens:* `PM_ Time Tracking`, `PM_ Timesheet Review`, `Employee_ My Time`

```mermaid
erDiagram
    timesheets {
        uuid     id              PK
        uuid     employee_id     FK "→ employees.id"
        date     week_start_date    "Luôn là thứ Hai"
        string   status             "Draft | Submitted | Pending Approval | Approved | Rejected"
        datetime submitted_at
        uuid     reviewed_by     FK "→ employees.id (PM/HR)"
        datetime reviewed_at
        text     reviewer_notes
    }

    time_entries {
        uuid     id              PK
        uuid     timesheet_id    FK "→ timesheets.id"
        uuid     project_id      FK "→ projects.id"
        date     work_date
        string   time_type          "Regular | Training | Overtime"
        time     start_time
        time     end_time
        text     notes
    }

    %% Ghi chú:
    %% - hours_logged   = EXTRACT(EPOCH FROM end_time - start_time)/3600  → tính toán
    %% - total_regular  = SUM(hours) WHERE time_type = 'Regular'          → tính toán
    %% - total_training = SUM(hours) WHERE time_type = 'Training'         → tính toán
    %% - total_overtime = SUM(hours) WHERE time_type = 'Overtime'         → tính toán
    %% - weekly_total   = SUM(all hours in timesheet)                     → tính toán
    %% - diff_hours     = weekly_total - expected_hours                   → tính toán

    employees  ||--o{ timesheets   : "submits"
    timesheets ||--o{ time_entries : "contains"
    projects   ||--o{ time_entries : "logged_against"
```

---

### Group 4: Budget (Ngân sách dự án)
*Screens:* `PM_ Project Budget`

```mermaid
erDiagram
    budget_adjustments {
        uuid     id              PK
        uuid     project_id      FK "→ projects.id"
        string   adjustment_type    "Increase | Decrease | Reallocation"
        decimal  amount             "Số tiền thay đổi ($)"
        string   budget_category    "Total | Labor"
        text     reason
        uuid     approved_by     FK "→ employees.id"
        datetime created_at
    }

    %% Ghi chú:
    %% - current_total_budget = projects.total_budget + SUM(adjustments for Total) → tính toán
    %% - budget_consumed      = SUM(hourly_rate * hours) from time_entries          → tính toán
    %% - budget_remaining     = current_total_budget - budget_consumed              → tính toán
    %% - budget_used_%        = budget_consumed / current_total_budget * 100        → tính toán
    %% - labor_cost_per_entry = employee.hourly_rate * hours_logged                 → tính toán

    projects ||--o{ budget_adjustments : "adjusted_by"
```

> **Lưu ý:** `hourly_rate` của nhân viên được lấy từ bảng lương (module Compensation), không lưu lại trong Project Management.

---

### Group 5: Productivity Monitoring (Theo dõi năng suất)
*Screens:* `PM_ Team Productivity`, `PM_ Employee Productivity Detail`, `Employee_ My Productivity`

```mermaid
erDiagram
    productivity_sessions {
        uuid     id              PK
        uuid     employee_id     FK "→ employees.id"
        uuid     project_id      FK "→ projects.id (nullable)"
        date     session_date
        time     session_start
        time     session_end
        integer  activity_pct       "% hoạt động thực tế (keyboard/mouse)"
        string   session_type       "Active | Idle | Offline"
        datetime created_at
    }

    productivity_app_logs {
        uuid     id              PK
        uuid     session_id      FK "→ productivity_sessions.id"
        string   app_name           "e.g. VS Code, MS Teams"
        integer  duration_mins
        string   category           "Development | Communication | Design | Other"
    }

    productivity_website_logs {
        uuid     id              PK
        uuid     session_id      FK "→ productivity_sessions.id"
        string   url
        integer  duration_mins
        string   category           "Development | Social | Other"
    }

    productivity_screenshots {
        uuid     id              PK
        uuid     session_id      FK "→ productivity_sessions.id"
        datetime captured_at
        string   file_url           "Path đến screenshot"
        integer  activity_pct       "Activity % tại thời điểm chụp"
    }

    %% Ghi chú:
    %% - tracked_hours   = SUM(session_end - session_start) per day → tính toán
    %% - active_time     = SUM(duration WHERE session_type='Active') → tính toán
    %% - idle_time       = SUM(duration WHERE session_type='Idle')   → tính toán
    %% - avg_activity_%  = AVG(activity_pct) across sessions         → tính toán
    %% - data_points     = COUNT(app_logs) + COUNT(web_logs) + COUNT(screenshots) → tính toán

    employees             ||--o{ productivity_sessions      : "tracked"
    productivity_sessions ||--o{ productivity_app_logs      : "logs_app"
    productivity_sessions ||--o{ productivity_website_logs  : "logs_website"
    productivity_sessions ||--o{ productivity_screenshots   : "captures"
```

---

## 3. Tổng hợp - Tất cả bảng & quan hệ

| Bảng | Mô tả | Các trường KHÔNG lưu (tính toán) |
|---|---|---|
| `projects` | Thông tin dự án | `team_size`, `avg_allocation_%`, `budget_remaining`, `budget_used_%` |
| `project_members` | Phân bổ thành viên vào dự án | `overall_allocation_%`, `available_capacity_%`, `capacity_status`, `weekly_hours` |
| `timesheets` | Bảng chấm công tuần của nhân viên | `total_regular_h`, `total_training_h`, `total_overtime_h`, `weekly_total`, `diff_hours` |
| `time_entries` | Từng dòng giờ làm trong ngày | `hours_logged` (= end_time - start_time) |
| `budget_adjustments` | Điều chỉnh ngân sách dự án | `current_budget`, `budget_consumed`, `budget_used_%` |
| `productivity_sessions` | Phiên làm việc của nhân viên | `tracked_hours`, `active_time`, `idle_time` |
| `productivity_app_logs` | Log ứng dụng trong phiên | — |
| `productivity_website_logs` | Log website trong phiên | — |
| `productivity_screenshots` | Screenshot theo dõi hoạt động | — |

> **Jira Integration** (`jira_integrations`, `jira_sprint_snapshots`) được dự kiến cho **v2** — sau khi các tính năng core ổn định.

