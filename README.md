# Hospital / Clinic Patient Management System

## 1. Project Overview & Team Division

- **Course code:** 261OOPR230279E_02FIE
- **Instructor:** Huỳnh Xuân Phụng

**Team members:**
- Lưu Nhật Thành - Student ID: 25110056 - GitHub: [luuthanh167](https://github.com/luuthanh167)
- Trần Nguyễn Hoàng Huy - Student ID: 25110024 - GitHub: [Huy090107](https://github.com/Huy090107)

### Project Overview

The Hospital / Clinic Patient Management System is a C# object-oriented application designed to demonstrate basic outpatient clinic operations. It models patients, medical staff, appointments, prescriptions, and medical histories.

The project applies OOP principles and demonstrates input validation, appointment scheduling, conflict detection, and medical record management.

### Detailed Division of Labor

| Member | Responsibilities | Contribution |
| :---: | --- | :---: |
| Thành | `Appointment`, `Patient`, `Prescription`, and `Staff` classes | 100% of assigned tasks |
| Huy | `AppointmentScheduler`, `MedicalHistory`, `Program`, and `ValidationHelper` | 100% of assigned tasks |

## 2. Features and Requirements

### Features

- **Patient Management:** Stores patient identifiers, names, contact details, and medical records.
- **Staff Management:** Models medical staff and specialized roles such as doctors and nurses.
- **Appointment Scheduling:** Creates appointments and checks for scheduling conflicts.
- **Appointment Status Management:** Supports valid status transitions such as Scheduled to Completed or Cancelled.
- **Prescription Management:** Represents medication, dosage, and usage instructions for a patient.
- **Medical History Tracking:** Records visit outcomes and associates them with a patient.
- **Validation and Error Handling:** Rejects missing required values and appointment dates that are not in the future.

### OOP Requirements

- **Encapsulation:** Uses properties and controlled setters to protect object state.
- **Inheritance:** Uses `Staff` as a base class for roles such as `Doctor` and `Nurse`.
- **Polymorphism:** Uses the abstract `PerformDuty()` method and overrides it in derived staff classes.
- **Abstraction:** Separates domain models from service classes that manage scheduling and medical history.

## 3. Project Structure

```text
HospitalOOP/
├── Models/
│   ├── Appointment.cs
│   ├── Patient.cs
│   ├── Prescription.cs
│   └── Staff.cs
├── Services/
│   ├── AppointmentScheduler.cs
│   └── MedicalHistory.cs
├── Utilities/
│   └── ValidationHelper.cs
├── Test/
│   └── Program.cs
├── Report/
│   └── Report.docx
└── README.md
```

## 4. System Design

### 4.1. Architecture

The application follows a simple object-oriented, layered design:

- **Models:** Represent the main domain objects, including `Patient`, `Staff`, `Doctor`, `Nurse`, `Appointment`, and `Prescription`.
- **Services:** `AppointmentScheduler` manages appointment scheduling and status changes. `MedicalHistory` records visit outcomes and maintains a patient's treatment history.
- **Utilities:** `ValidationHelper` centralizes common validation rules, including required text, future appointment dates, and valid appointment status transitions.
- **Test / Program:** The console entry point creates sample objects and exercises the main workflow.

### 4.2. Main Class Relationships

- A `Doctor` and a `Nurse` inherit common identity and contact information from the abstract `Staff` class.
- An `Appointment` associates a patient with a doctor and a scheduled time.
- `AppointmentScheduler` stores appointments and checks whether a doctor has an overlapping active appointment.
- `MedicalHistory` belongs to a patient and records appointments and their visit outcomes.
- `Prescription` represents medication information associated with a patient and a prescribing doctor.

### 4.3. Typical Workflow

1. Create a doctor and a patient.
2. Create an appointment with a future date and time.
3. Ask `AppointmentScheduler` to schedule the appointment and check for conflicts.
4. Update the appointment status when the visit is completed.
5. Record the visit outcome through `MedicalHistory`.
6. Display the patient's medical record to verify that the outcome was saved.

## 5. OOP Principles and Self-Explanation

The project demonstrates four fundamental Object-Oriented Programming principles:

**Encapsulation**
- Implementation: Classes expose data through properties and use private or protected setters where appropriate.
- Design rationale: Limits uncontrolled modification of important data such as patient information and appointment details.

**Inheritance**
- Implementation: `Doctor` and `Nurse` derive from the abstract `Staff` class.
- Design rationale: Shares common staff fields and reduces duplicated code.

**Abstraction**
- Implementation: `Staff` defines the abstract `PerformDuty()` method, while service classes provide higher-level operations for scheduling and medical history.
- Design rationale: Hides implementation details and gives other parts of the program a clearer interface.

**Polymorphism**
- Implementation: `Doctor` and `Nurse` provide their own overrides of `PerformDuty()`.
- Design rationale: The same method call can produce behavior appropriate to each staff role.

## 6. Validation and Error Handling

The application uses the static `ValidationHelper` class to centralize common validation rules.

### 6.1. Validation Rules

**Required string validation (`RequireNotEmpty`)**
- Checks that required text is not null, empty, or whitespace.
- Throws an `ArgumentException` when the value is invalid.

**Future appointment validation (`RequireFutureDate`)**
- Checks that the appointment time is later than the current time.
- Throws an `ArgumentException` if the appointment is in the past or at the current time.

**Appointment status transitions (`IsValidStatusTransition`)**
- Allows a `Scheduled` appointment to transition to `Completed` or `Cancelled`.
- Rejects transitions from terminal states such as `Completed` and `Cancelled`.

### 6.2. Error Handling

Validation exceptions make invalid input fail explicitly instead of silently creating invalid objects. The calling code should handle expected user-facing errors where appropriate and display a clear message.

## 7. Test Cases and Test Results

The console test program demonstrates the main patient and appointment workflow. The following cases describe the expected behavior to verify when running the application.

| Test ID | Scenario | Input / Action | Expected result |
| --- | --- | --- | --- |
| TC-01 | Create a doctor | Provide a valid ID, name, specialty, and phone number | A `Doctor` object is created with the supplied details |
| TC-02 | Create a patient | Provide a valid ID, name, and phone number | A `Patient` object is created and its medical record starts empty |
| TC-03 | Schedule an appointment | Create an appointment at a future time and pass it to the scheduler | The appointment is added successfully |
| TC-04 | Reject a past appointment | Try to create or schedule an appointment whose time is in the past | Validation throws an `ArgumentException` |
| TC-05 | Detect a doctor conflict | Schedule another appointment for the same doctor during an overlapping time | The scheduler rejects the conflicting appointment |
| TC-06 | Update appointment status | Change a `Scheduled` appointment to `Completed` | The status is updated successfully |
| TC-07 | Reject an invalid status transition | Try to change an already completed or cancelled appointment | The transition is rejected |
| TC-08 | Record a visit outcome | Add a non-empty outcome to the patient's medical history | The outcome is recorded and the patient's medical record is updated |
| TC-09 | Reject empty required text | Pass an empty or whitespace-only required string to validation | An `ArgumentException` is thrown |

**Test result reporting:** Record the actual outcome after running each test. Do not mark all cases as passed unless they have been executed and verified. The current console demonstration prints the created doctor and patient, schedules a future appointment, updates its status, records a visit outcome, and displays the medical record.

## 8. Data Privacy Considerations

This project is an educational demonstration and does not implement production-grade authentication, authorization, encryption, or persistent secure storage.

- **Encapsulation:** Patient contact details and medical information are kept within model objects and accessed through their public interfaces.
- **Data separation:** Patient identity information is modeled separately from appointment and medical-history behavior.
- **Input validation:** Required fields are validated to reduce invalid data entering the application.
- **Safe demonstration data:** Use fictional patient information when presenting or testing the project. Do not enter real patient records into this demo.

## 9. Build and Run Instructions

### 9.1. Prerequisites

- Install the .NET SDK compatible with the project's target framework.
- Install an IDE or editor such as Visual Studio or Visual Studio Code (optional).
- Clone the repository and open a terminal in the project root directory.

### 9.2. Build the Project

Run:

```bash
dotnet build
```

A successful build should finish without compilation errors.

### 9.3. Run the Console Demonstration

Run:

```bash
dotnet run
```

If the project file is not automatically detected, specify it explicitly:

```bash
dotnet run --project HospitalOOP.csproj
```

The console program creates sample doctor and patient objects, schedules an appointment for a future time, updates its status, records a visit outcome, and displays the patient's medical record.

### 9.4. Troubleshooting

- If the terminal reports that `dotnet` is not recognized, install the .NET SDK and reopen the terminal.
- If compilation fails, read the first compiler error and verify that the model and service classes expose the properties and methods used by `Test/Program.cs`.
- Appointment tests use future dates; use `DateTime.Now.AddDays(1)` or another future time in test code.
- This console demonstration stores data in memory only. Data is not retained after the program exits.

## 10. AI Usage Log

As recorded in the team's AI-use form, **Gemini 3.6 Flash** was used as an assistant when developing the console menu for the Hospital Management System.

| Activity | AI tool and assistance | Team review and contribution |
| --- | --- | --- |
| Console menu | Gemini 3.6 Flash helped suggest an initial menu-driven console structure, including numbered options and a flow for receiving and handling the user's selection. | The team reviewed the suggestion and adapted the menu to the existing project classes and required hospital workflow. |
| Understanding and integration | Gemini 3.6 Flash was used to help explain the generated structure and how menu options could connect to the existing Model and Service classes. | The team checked the implementation against the project code and remained responsible for integrating and understanding the final result. |

The team ran the project's test runner separately to verify behavior. The recorded test run reported **18 passed and 0 failed**. This result describes that specific run and is not presented as an AI-generated test result.

AI output was treated as a suggestion that required review. The team is responsible for understanding the submitted code and ensuring this log matches the tool and assistance actually used.
