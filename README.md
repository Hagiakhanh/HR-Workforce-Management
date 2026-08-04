# HR-Workforce-Management

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

    LOGIN --> L1["Login"]
    LOGIN --> L2["Logout"]
    LOGIN --> L3["Refresh Session"]
    LOGIN --> L4["Reset Password"]
    LOGIN --> L5["Verify Multi-Factor Authentication"]

    ACCOUNT --> AC1["Create User Account"]
    ACCOUNT --> AC2["Activate or Deactivate Account"]
    ACCOUNT --> AC3["Lock or Unlock Account"]
    ACCOUNT --> AC4["Link Account to Employee"]
    ACCOUNT --> AC5["View Account Status"]

    ACCESS --> AU1["Create Role"]
    ACCESS --> AU2["Assign Role"]
    ACCESS --> AU3["Configure Permissions"]
    ACCESS --> AU4["Define Access Scope"]
    ACCESS --> AU5["Review User Access"]

    class AUTH module;
    class LOGIN,ACCOUNT,ACCESS feature;
    class LOGIN,ACCESS important;
    class L1,L2,L3,L4,L5,AC1,AC2,AC3,AC4,AC5,AU1,AU2,AU3,AU4,AU5 usecase;
```

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

    STRUCTURE --> S1["Manage Company Information"]
    STRUCTURE --> S2["Create Department"]
    STRUCTURE --> S3["Create Team"]
    STRUCTURE --> S4["Define Job Position"]
    STRUCTURE --> S5["Update Organization Unit"]

    HIERARCHY --> H1["Assign Reporting Manager"]
    HIERARCHY --> H2["Assign Employee to Department or Team"]
    HIERARCHY --> H3["Transfer Employee"]
    HIERARCHY --> H4["View Organization Chart"]

    LOCATION --> W1["Create Work Location"]
    LOCATION --> W2["Assign Employee Location"]
    LOCATION --> W3["Configure Work Mode"]
    LOCATION --> W4["Manage Worksite"]

    class ORG module;
    class STRUCTURE,HIERARCHY,LOCATION feature;
    class STRUCTURE,HIERARCHY important;
    class S1,S2,S3,S4,S5,H1,H2,H3,H4,W1,W2,W3,W4 usecase;
```

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

    DIRECTORY --> D1["View Employee Directory"]
    DIRECTORY --> D2["Search Employees"]
    DIRECTORY --> D3["Filter Employees"]
    DIRECTORY --> D4["View Team Members"]

    PROFILE --> P1["Create Employee Profile"]
    PROFILE --> P2["View Employee Profile"]
    PROFILE --> P3["Update Personal Information"]
    PROFILE --> P4["Update Contact Information"]
    PROFILE --> P5["Manage Emergency Contacts"]

    RECORDS --> R1["Assign Department, Position and Manager"]
    RECORDS --> R2["Update Employment Status"]
    RECORDS --> R3["Promote or Transfer Employee"]
    RECORDS --> R4["View Employment History"]
    RECORDS --> R5["Terminate Employee Record"]

    DOCUMENTS --> DS1["Upload Employee Document"]
    DOCUMENTS --> DS2["View or Download Document"]
    DOCUMENTS --> DS3["Request Electronic Signature"]
    DOCUMENTS --> DS4["Acknowledge Company Policy"]
    DOCUMENTS --> DS5["Request Profile Update"]

    class EMP module;
    class DIRECTORY,PROFILE,RECORDS,DOCUMENTS feature;
    class PROFILE,RECORDS important;
    class D1,D2,D3,D4,P1,P2,P3,P4,P5,R1,R2,R3,R4,R5,DS1,DS2,DS3,DS4,DS5 usecase;
```

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

    JOB --> J1["Create Job Opening"]
    JOB --> J2["Publish Job Opening"]
    JOB --> J3["Assign Hiring Team"]
    JOB --> J4["Close Job Opening"]

    CANDIDATE --> C1["Register Candidate Application"]
    CANDIDATE --> C2["Screen Candidate"]
    CANDIDATE --> C3["Schedule Interview"]
    CANDIDATE --> C4["Record Interview Evaluation"]
    CANDIDATE --> C5["Send Job Offer"]
    CANDIDATE --> C6["Convert Candidate to Employee"]

    ONBOARDING --> O1["Create Onboarding Plan"]
    ONBOARDING --> O2["Assign Onboarding Tasks"]
    ONBOARDING --> O3["Collect Required Documents"]
    ONBOARDING --> O4["Provision User Account"]
    ONBOARDING --> O5["Assign Department and Manager"]
    ONBOARDING --> O6["Complete Onboarding"]

    OFFBOARDING --> F1["Create Offboarding Request"]
    OFFBOARDING --> F2["Assign Work Handover"]
    OFFBOARDING --> F3["Record Asset Return"]
    OFFBOARDING --> F4["Revoke System Access"]
    OFFBOARDING --> F5["Conduct Exit Interview"]
    OFFBOARDING --> F6["Complete Employee Exit"]

    class LIFE module;
    class JOB,CANDIDATE,ONBOARDING,OFFBOARDING feature;
    class CANDIDATE,ONBOARDING important;
    class J1,J2,J3,J4,C1,C2,C3,C4,C5,C6,O1,O2,O3,O4,O5,O6,F1,F2,F3,F4,F5,F6 usecase;
```

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

    TRACKING --> T1["Start Timer"]
    TRACKING --> T2["Stop Timer"]
    TRACKING --> T3["Select Project and Task"]
    TRACKING --> T4["Add Manual Time Entry"]
    TRACKING --> T5["Edit Time Entry"]
    TRACKING --> T6["Add Work Note"]

    ATTENDANCE --> A1["Check In"]
    ATTENDANCE --> A2["Check Out"]
    ATTENDANCE --> A3["Track Break"]
    ATTENDANCE --> A4["Record Late or Absence"]
    ATTENDANCE --> A5["Record Overtime"]
    ATTENDANCE --> A6["Request Attendance Correction"]

    TIMESHEET --> TS1["View Daily Timesheet"]
    TIMESHEET --> TS2["View Weekly Timesheet"]
    TIMESHEET --> TS3["Submit Timesheet"]
    TIMESHEET --> TS4["Approve or Reject Timesheet"]
    TIMESHEET --> TS5["Lock or Reopen Timesheet"]

    SCHEDULE --> S1["Create Work Shift"]
    SCHEDULE --> S2["Assign Shift"]
    SCHEDULE --> S3["Create Recurring Schedule"]
    SCHEDULE --> S4["Request Shift Change"]
    SCHEDULE --> S5["View Team Schedule"]

    LEAVE --> L1["Configure Leave Type and Policy"]
    LEAVE --> L2["Submit Leave Request"]
    LEAVE --> L3["Approve or Reject Leave"]
    LEAVE --> L4["View Leave Balance"]
    LEAVE --> L5["View Leave Calendar"]
    LEAVE --> L6["Cancel Leave Request"]

    class TIME module;
    class TRACKING,ATTENDANCE,TIMESHEET,SCHEDULE,LEAVE feature;
    class TRACKING,TIMESHEET,LEAVE important;
    class T1,T2,T3,T4,T5,T6,A1,A2,A3,A4,A5,A6,TS1,TS2,TS3,TS4,TS5,S1,S2,S3,S4,S5,L1,L2,L3,L4,L5,L6 usecase;
```

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

    PROJECT --> P1["Create Project"]
    PROJECT --> P2["Update Project Information"]
    PROJECT --> P3["Assign Project Manager"]
    PROJECT --> P4["Assign Project Members"]
    PROJECT --> P5["Update Project Status"]
    PROJECT --> P6["View Project Progress"]
    PROJECT --> P7["Archive Project"]

    TASK --> T1["Create Task"]
    TASK --> T2["Assign Task"]
    TASK --> T3["Set Task Priority"]
    TASK --> T4["Set Estimate and Deadline"]
    TASK --> T5["Update Task Status"]
    TASK --> T6["Track Time on Task"]
    TASK --> T7["View Task Progress"]

    RESOURCE --> R1["View Member Workload"]
    RESOURCE --> R2["Set Hour Budget"]
    RESOURCE --> R3["Set Cost Budget"]
    RESOURCE --> R4["Configure Hourly Rate"]
    RESOURCE --> R5["View Budget Usage"]
    RESOURCE --> R6["Receive Budget Alert"]

    class PROJECTS module;
    class PROJECT,TASK,RESOURCE feature;
    class PROJECT,TASK important;
    class P1,P2,P3,P4,P5,P6,P7,T1,T2,T3,T4,T5,T6,T7,R1,R2,R3,R4,R5,R6 usecase;
```

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

    ACTIVITY --> A1["Capture Activity Level"]
    ACTIVITY --> A2["Detect Idle Time"]
    ACTIVITY --> A3["View Active Time"]
    ACTIVITY --> A4["View Current Working Status"]
    ACTIVITY --> A5["Review Activity Timeline"]

    COMPUTER --> C1["Capture Screenshot"]
    COMPUTER --> C2["View Screenshot Timeline"]
    COMPUTER --> C3["Track Application Usage"]
    COMPUTER --> C4["Track Website Usage"]
    COMPUTER --> C5["Classify Productive and Unproductive Usage"]

    LOCATION --> L1["Track GPS Location"]
    LOCATION --> L2["Configure Geofence"]
    LOCATION --> L3["Record Field Check-In"]
    LOCATION --> L4["View Location History"]

    POLICY --> P1["Configure Screenshot Frequency"]
    POLICY --> P2["Enable Screenshot Blur"]
    POLICY --> P3["Configure Tracked Applications and Websites"]
    POLICY --> P4["Configure Tracking by Role or Team"]
    POLICY --> P5["Allow Employee to View Tracking Data"]

    class PRODUCTIVITY module;
    class ACTIVITY,COMPUTER,LOCATION,POLICY feature;
    class ACTIVITY,COMPUTER important;
    class A1,A2,A3,A4,A5,C1,C2,C3,C4,C5,L1,L2,L3,L4,P1,P2,P3,P4,P5 usecase;
```

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
    PAYROLL --> P2["Import Approved Work Hours"]
    PAYROLL --> P3["Calculate Overtime"]
    PAYROLL --> P4["Calculate Payroll"]
    PAYROLL --> P5["Review Payroll"]
    PAYROLL --> P6["Record Payment"]
    PAYROLL --> P7["View Payment History"]

    COMPENSATION --> C1["Manage Bonuses"]
    COMPENSATION --> C2["Manage Allowances"]
    COMPENSATION --> C3["Manage Salary Adjustments"]
    COMPENSATION --> C4["Manage Benefit Plans"]
    COMPENSATION --> C5["Enroll Employee in Benefits"]

    PERFORMANCE --> F1["Create Employee Goal"]
    PERFORMANCE --> F2["Update Goal Progress"]
    PERFORMANCE --> F3["Start Performance Review"]
    PERFORMANCE --> F4["Submit Self-Review"]
    PERFORMANCE --> F5["Submit Manager Review"]
    PERFORMANCE --> F6["Provide Continuous Feedback"]
    PERFORMANCE --> F7["Create Development Plan"]

    class PAYPERF module;
    class PAYROLL,COMPENSATION,PERFORMANCE feature;
    class PAYROLL,PERFORMANCE important;
    class P1,P2,P3,P4,P5,P6,P7,C1,C2,C3,C4,C5,F1,F2,F3,F4,F5,F6,F7 usecase;
```

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

    DASHBOARD --> D1["View Employee Overview"]
    DASHBOARD --> D2["View Workforce Status"]
    DASHBOARD --> D3["View Time and Attendance Summary"]
    DASHBOARD --> D4["View Project and Productivity Summary"]
    DASHBOARD --> D5["View Payroll Summary"]

    ANALYTICS --> A1["Generate HR Report"]
    ANALYTICS --> A2["Generate Time and Attendance Report"]
    ANALYTICS --> A3["Generate Project and Productivity Report"]
    ANALYTICS --> A4["Generate Payroll Report"]
    ANALYTICS --> A5["Filter Report"]
    ANALYTICS --> A6["Export Report"]

    WORKFLOW --> W1["Configure Approval Workflow"]
    WORKFLOW --> W2["Send Pending Approval Reminder"]
    WORKFLOW --> W3["Send Status Notification"]
    WORKFLOW --> W4["Send Schedule or Leave Reminder"]
    WORKFLOW --> W5["Escalate Overdue Approval"]

    ADMIN --> AD1["Configure System Settings"]
    ADMIN --> AD2["Configure Company Policies"]
    ADMIN --> AD3["Manage External Integrations"]
    ADMIN --> AD4["View Audit Logs"]
    ADMIN --> AD5["View Data Change History"]
    ADMIN --> AD6["Configure Data Retention"]

    class REPORTS module;
    class DASHBOARD,ANALYTICS,WORKFLOW,ADMIN feature;
    class DASHBOARD,ANALYTICS important;
    class D1,D2,D3,D4,D5,A1,A2,A3,A4,A5,A6,W1,W2,W3,W4,W5,AD1,AD2,AD3,AD4,AD5,AD6 usecase;
```

# System Actors and Roles

## 1. Overview

The **HR & Workforce Management Platform** includes eight main roles.

These roles cover recruitment, employee management, time tracking, project
management, productivity monitoring, payroll, and system administration.

| Role | Main Responsibility |
|---|---|
| Candidate | Participates in the recruitment process |
| Employee | Uses employee self-service and tracks daily work |
| Manager / Team Lead | Manages employees, approvals, and team performance |
| Project Manager | Manages projects, tasks, workloads, and budgets |
| HR Staff | Manages employee records and employee lifecycle |
| Recruiter | Manages job openings, candidates, interviews, and offers |
| Payroll Officer | Manages salaries, approved hours, and payroll |
| System Administrator | Manages accounts, permissions, policies, and system settings |

---

## 2. Candidate

A candidate is an external user who participates in the recruitment process
before becoming an employee.

### Main Functions

- View available job openings.
- Submit a job application.
- Create and update candidate information.
- Upload required application documents.
- View interview schedules.
- Participate in interviews.
- View and respond to a job offer.
- Submit onboarding documents after accepting an offer.

---

## 3. Employee

An employee is the primary internal user of the platform.

### Main Functions

- Log in and log out.
- Reset the account password.
- View and update personal information.
- Update contact and emergency contact information.
- View employee documents and company policies.
- Start and stop the work timer.
- Select a project and task before tracking time.
- Add or edit manual time entries.
- Check in and check out.
- Record breaks and overtime.
- Request an attendance correction.
- View daily and weekly timesheets.
- Submit a timesheet for approval.
- View assigned projects and tasks.
- Update task status.
- View work shifts and schedules.
- Submit or cancel a leave request.
- View leave balance and leave calendar.
- View personal productivity data.
- View performance goals and reviews.
- View salary and payment history.

---

## 4. Manager / Team Lead

A manager or team lead supervises employees within an assigned department,
team, or reporting structure.

A manager also has the normal functions of an employee.

### Main Functions

- View direct reports and team members.
- View employee profiles within the assigned access scope.
- Monitor team attendance and working status.
- View late arrivals, absences, breaks, and overtime.
- Approve or reject timesheets.
- Lock or reopen timesheets when authorized.
- Approve or reject leave requests.
- View the team leave calendar.
- Create and assign work shifts.
- View the team work schedule.
- Assign tasks to team members.
- Set task estimates and deadlines.
- Monitor tracked working hours.
- Monitor employee workload.
- View activity levels and idle time.
- View employee productivity information.
- Review employee goals.
- Submit manager performance reviews.
- Provide employee feedback.
- View team dashboards and reports.

---

## 5. Project Manager

A project manager is responsible for project execution, project members,
tasks, working hours, workloads, and budgets.

A project manager may also use the normal functions of an employee.

### Main Functions

- Create a project.
- Update project information.
- Assign project members.
- Remove members from a project.
- Update project status.
- Archive a completed project.
- Create and assign tasks.
- Set task priority.
- Set estimated hours.
- Set task deadlines.
- Monitor task status.
- Monitor tracked hours by project or task.
- View project progress.
- View member workload.
- Configure the project hour budget.
- Configure the project cost budget.
- View budget usage.
- Receive budget alerts.
- Generate project and productivity reports.

---

## 6. HR Staff

HR staff manage employee information, organization structure, employment
records, onboarding, offboarding, documents, and HR policies.

HR staff may also use the normal functions of an employee.

### Main Functions

- View, search, and filter the employee directory.
- Create an employee profile.
- Update employee information.
- Assign a department, team, position, manager, and work location.
- Update employee status.
- Promote or transfer an employee.
- View employment history.
- Terminate an employee record.
- Upload and manage employee documents.
- Request electronic signatures.
- Manage company information.
- Create and update departments.
- Create and update teams.
- Define job positions.
- Manage the organization hierarchy.
- View the organization chart.
- Manage branches, offices, and worksites.
- Configure remote or office work modes.
- Create an onboarding plan.
- Assign onboarding tasks.
- Collect required employee documents.
- Coordinate account creation.
- Complete employee onboarding.
- Create an offboarding request.
- Assign work handover activities.
- Record returned company assets.
- Coordinate access deactivation.
- Conduct an exit interview.
- Complete employee offboarding.
- Configure leave types and leave policies.
- Generate HR reports.

---

## 7. Recruiter

A recruiter is responsible for job openings and candidates throughout the
recruitment process.

The recruiter may be implemented as a specialized HR role.

### Main Functions

- Create a job opening.
- Update the job description.
- Publish a job opening.
- Close a job opening.
- Assign a hiring team.
- Register candidate applications.
- Create and update candidate profiles.
- Screen candidates.
- Update application status.
- Schedule interviews.
- Record interview evaluations.
- Send job offers.
- Track job offer status.
- Convert an accepted candidate into an employee.
- Start the onboarding process.

---

## 8. Payroll Officer

A payroll officer manages employee salaries, approved working hours,
compensation, benefits, payroll calculations, and payment records.

The payroll officer may also use the normal functions of an employee.

### Main Functions

- Configure employee salaries.
- Configure employee hourly rates.
- Retrieve approved working hours.
- Review regular working hours.
- Calculate overtime.
- Calculate payroll.
- Review payroll results.
- Record employee payments.
- View payment history.
- Manage bonuses.
- Manage allowances.
- Apply salary adjustments.
- Manage benefit plans.
- Enroll employees in benefits.
- Generate payroll reports.
- Export payroll data to an external payroll or payment system.

---

## 9. System Administrator

A system administrator manages platform accounts, roles, permissions,
monitoring policies, workflows, integrations, audit logs, and system
settings.

### Main Functions

- Create user accounts.
- Activate or deactivate accounts.
- Lock or unlock accounts.
- Link a user account to an employee profile.
- View account status.
- Create and update roles.
- Assign roles to users.
- Configure permissions.
- Define access scopes.
- Review user access.
- Configure authentication settings.
- Configure multi-factor authentication.
- Configure screenshot frequency.
- Enable screenshot blurring.
- Configure tracked applications and websites.
- Configure tracking by role or team.
- Configure approval workflows.
- Configure system settings.
- Configure company policies.
- Manage external integrations.
- View audit logs.
- View data change history.
- Configure data retention policies.

---

# Usecase

## Time, Attendance & Leave

![alt text](Usecase-Time-Attendance-Leave.jpg)