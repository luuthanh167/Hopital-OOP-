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

| Member | Responsibilities                                                            |      Contribution      |
| :----: | --------------------------------------------------------------------------- | :--------------------: |
| Thành  | `Appointment`, `Patient`, `Prescription`, and `Staff` classes               | 100% of assigned tasks |
|  Huy   | `AppointmentScheduler`, `MedicalHistory`, `Program`, and `ValidationHelper` | 100% of assigned tasks |

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
├── Test/
│   ├── Program.cs
│   └── TestRunner.cs
├── Utilities/
│   └── ValidationHelper.cs
├── Report/
│   └── Report.docx
├── Hopital class.png
├── HospitalOOP.csproj
└── README.mdort.docx
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

Below are the representative test cases executed directly through the Interactive Console Interface (`Program.cs`). Each test case demonstrates step-by-step user input, system validation handling, real console response messages, and the final status.

---

### 7.1. Step-by-Step Interactive Test Cases

#### 🔹 TC01: Happy Path Scenario (End-to-End Patient Flow)

- **Menu Options:** Option `1` (Register Patient) -> Option `2` (Add Doctor) -> Option `3` (Schedule Appointment) -> Option `4` (Complete Appointment).
- **User Inputs:**
  - **Menu 1:** Patient ID: `PAT01`, Full Name: `Nguyen Van A`, Phone: `0901234567`.
  - **Menu 2:** Doctor ID: `DOC01`, Full Name: `Dr. Smith`, Specialty: `Cardiology`, Phone: `0987654321`.
  - **Menu 3:** Patient ID: `PAT01`, Doctor ID: `DOC01`, Appointment ID: `APP01`, Time: `2026-11-10 09:00`.
  - **Menu 4:** Appointment ID: `APP01`.
- **System Console Output:**
  - `Đã tiếp nhận bệnh nhân Nguyen Van A (mã PAT01).`
  - `Đã thêm bác sĩ Dr. Smith - Cardiology.`
  - `Đặt lịch thành công: APP01 | Nguyen Van A | BS. Dr. Smith | 10/11/2026 09:00`
  - `Lịch khám APP01 đã chuyển sang trạng thái Completed.`
- **Final Status:** **PASSED**

---

#### 🔹 TC02: Validation & Error Handling (Overlapping Appointment Conflict)

- **Menu Option:** Option `3` (Schedule Appointment).
- **Validation Target:** Verify conflict detection mechanism in `AppointmentScheduler` when attempting to book an overlapping time slot for the same doctor.
- **User Inputs:**
  - Attempt to book an overlapping slot for `DOC01` with `APP01` (already booked at 09:00).
  - Patient ID: `PAT01`, Doctor ID: `DOC01`, Appointment ID: `APP02`, Time: `2026-11-10 09:15`.
- **Exception & System Response:**
  - Method `ScheduleAppointment()` blocks execution and throws `InvalidOperationException`.
  - Console displays: `Không thể thực hiện: Bác sĩ DOC01 đã có lịch khám trong khoảng thời gian này.`
- **Final Status:** **PASSED**

---

#### 🔹 TC03: Validation & Boundary Check (Past Date Constraint)

- **Menu Option:** Option `3` (Schedule Appointment).
- **Validation Target:** Verify `ReadFutureDateTime()` constraint requiring appointment times to be strictly in the future.
- **User Inputs:**
  - Patient ID: `PAT01`, Doctor ID: `DOC01`, Appointment ID: `APP03`.
  - Appointment Time (Past): `2020-01-01 08:00`.
- **Exception & System Response:**
  - `ReadFutureDateTime()` loop rejects input and prompts user to re-enter.
  - Console displays: `Thời gian không hợp lệ/đã qua. Nhập theo mẫu yyyy-MM-dd HH:mm và chọn thời gian tương lai.`
- **Final Status:** **PASSED**

---

#### 🔹 TC04: Business Logic & Patient Safety (Record Result to Uncompleted Appointment)

- **Menu Option:** Option `5` (Record Medical History).
- **Validation Target:** Enforce workflow constraint preventing diagnosis records from being appended to appointments that are not in `Completed` status.
- **User Inputs:**
  - Enter Appointment ID currently in `Scheduled` status: `APP03`.
- **Exception & System Response:**
  - Method `RecordMedicalHistory()` catches error and throws `InvalidOperationException`.
  - Console displays: `Không thể thực hiện: Lịch khám chưa hoàn tất hoặc bị hủy.`
- **Final Status:** **PASSED**

---

#### 🔹 TC05: Object-Oriented Design (Polymorphism & Role Display)

- **Menu Option:** Option `7` (View Patient Record).
- **Validation Target:** Demonstrate runtime polymorphism by storing Doctor and Nurse objects in a List<Staff> and calling the overridden PerformDuty() method through a base-class reference
- **User Inputs:**
- Query Patient ID: `PAT01`.
- **System Console Output:**

  ```text
  ==================================================
                   HỒ SƠ BỆNH NHÂN
  ==================================================
  Mã bệnh nhân : PAT01
  Họ và tên    : Nguyen Van A
  Số điện thoại: 0901234567
  --------------------------------------------------
  LỊCH SỬ LỊCH KHÁM:
    APP01 | 10/11/2026 09:00 | BS. Dr. Smith | Completed

  BỆNH ÁN / KẾT QUẢ KHÁM:
    Chưa có ghi nhận.

  ĐƠN THUỐC:
    Chưa có đơn thuốc.
  ==================================================
  ```

  ### 7.2. Executed Test Suite Summary Log

  #### 7.2.1. Automated Unit Test Suite (TestRunner.cs)

- File TestRunner.cs executed 18 automated test cases across 4 main modules (Appointment & Status: 10, Prescription: 4, MedicalHistory: 3, Validation: 1) to verify core business logic and exception handling.

```text
AUTOMATED TEST RESULTS

[PASS] Schedule a valid appointment
[PASS] Reject overlapping doctor appointments
[PASS] Allow back-to-back appointments
[PASS] Reject reschedule into a conflict
[PASS] Failed reschedule leaves original time unchanged
[PASS] Reschedule to a free time
[PASS] Unknown appointment returns false
[PASS] Case-insensitive status update
[PASS] Reject invalid status transition
[PASS] Reject unknown status
[PASS] Reject visit record for another patient
[PASS] Record completed visit
[PASS] Reject duplicate visit outcome
[PASS] Prescription trims input
[PASS] Prescription updates and summary
[PASS] Reject empty prescription dosage
[PASS] Reject missing prescription dosage
[PASS] Reject appointment in the past
```

Total Test Cases: 18
Passed: 18
Failed: 0
Overall Result: ALL TESTS PASSED

#### 7.2.2. Interactive Console Scenarios (TC01 – TC05)

#         HỆ THỐNG QUẢN LÝ BỆNH ÁN BỆNH VIỆN

1. Tiếp nhận bệnh nhân
2. Thêm bác sĩ
3. Đặt lịch khám
4. Hoàn tất lịch khám
5. Ghi bệnh án / kết quả khám
6. Kê đơn thuốc
7. Xem hồ sơ bệnh nhân
8. Xem danh sách lịch khám
9. Xem danh sách đơn thuốc
10. Thoát

---

Bệnh nhân: 1 | Bác sĩ: 1 | Lịch khám: 1

```text
[EXECUTION TEST RESULTS SUMMARY]<br>
TC01 (Happy Path)               : SUCCESS - Registered & Completed APP01 [PASSED]<br>
TC02 (Validation Overlap)       : CAUGHT - InvalidOperationException [PASSED]<br>
TC03 (Validation Past Date)     : CAUGHT - Console Retry Prompt Prompted [PASSED]<br>
TC04 (Invalid Status Transition): CAUGHT - InvalidOperationException [PASSED]<br>
TC05 (Polymorphism / History)   : SUCCESS - Medical History Compiled [PASSED]<br>

Status: 5/5 Scenarios Verified & Passed.
```

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


AI tools may be used as learning and development assistants during this project. Any AI-assisted output should be reviewed by the team, checked against the actual source code, and understood by the students before submission.

| Week       | AI                                 | Prompt                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         | AI Assistance / Suggestions                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         | Processing & Improvement                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   |
| :--------- | :--------------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Week 1** | **ChatGPT**<br>_(ChatGPT-6 Luna)_  | 1. Based on my project, please summarize the tasks I need to complete.<br>2. The group consists of two people; please divide the tasks.<br>3. Explain how to build the ValidationHelper, Patient, Staff, and Appointment classes in the OOP Hospital project. Present the meaning, attributes, constructor, and methods of each class in an easy-to-understand manner.                                                                                                                                                                                                                                                                                                                                         | 1. Main tasks to complete:<br>- Write files: Patient, Staff, Appointment, Prescription, MedicalHistory, and Validation.<br>- Systematize the essential OOP principles: Encapsulation, Inheritance, Abstraction, and the products that need to be completed in the report.<br>2. Divide the work between two people:<br>- Person 1: (Data and Models): Programming the Patient, Staff (Doctor, Nurse), and Prescription classes; testing data management; explaining Encapsulation & Inheritance/Polymorphism.<br>- Person 2: Programming Appointment, AppointmentScheduler, MedicalHistory, ValidationHelper, Program; testing scheduling logic; explaining abstraction.<br>- Working together: Agreeing on the interface, integrating the system, taking screenshots to demonstrate testing, finalizing the UML, and preparing the report.<br>3.<br>- Patient class: Outlines the basic attributes and methods for managing MedicalRecord.<br>- Appointment Layer: Patient, Doctor, StartTime attribute framework and basic VisitOutcome assignment mechanism.<br>- The ValidationHelper class outlines functions for validating input data (empty strings, date formats, and appointment status). | - Tightly integrate data validation methods from ValidationHelper (such as RequireNotEmpty, RequireFutureDate) into the constructors and methods of the Patient, Doctor, Nurse, and Appointment classes to ensure data security.<br>- Fine-tune the access permissions of attributes (e.g., use private sets for names and phone numbers, assign read-only permissions to IDs) to ensure proper OOP encapsulation.<br>- Complete the logic for processing patient examination results in the RecordVisitOutcome method within the Appointment class.                                                                                                                                                                                                                                                                                                                                                       |
| **Week 1** | **Gemini**<br>_(Gemini 3.6 Flash)_ | 1. Please suggest the Staff and Patient classes for me to write code for.<br>2. Here is the Staff and Patient code; please check it to see if there are any errors that need fixing.<br>3. Please check if my ValidationHelper and Appointment code is correct.                                                                                                                                                                                                                                                                                                                                                                                                                                                | 1.<br>- Suggested structure: Create an abstract class called Staff (with attributes StaffID, FullName, PhoneNumber, Role) to act as the parent class for Doctor and Nurse.<br>- Suggested structure for the Patient class to store patient information (PatientID, FullName, PhoneNumber, DateOfBirth, Gender, Address).<br>2.<br>- A compilation error CS0534 was detected in the Staff class due to subclasses lacking the `override` keyword in the `PerformDuty()` method.<br>- Additional validation reminder: Check for null/empty strings for the Specialty (Doctor) and Department (Nurse) properties.<br>- Warning: The Appointment class has not checked past time in its constructor (startTime).<br>- Your Patient class is perfectly syntax-correct in C# and meets the Object-Oriented Design (OOP) standard.                                                                                                                                                                                                                                                                                                                                                                         | - Fixes for the Staff class: Add the `override` keyword to the `PerformDuty()` method; integrate `ValidationHelper.RequireNotEmpty` to check for Specialty and Department.<br>- Fixing the Appointment class bug: Add the ValidationHelper.RequireFutureDate(startTime) method to the constructor to block past time.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      |
| **Week 2** | **Gemini**<br>_(Gemini 3.6 Flash)_ | 1. Double-check all classes to ensure they work together correctly. If there are any errors, suboptimal elements, or redundancies, please point them out so I can fix them.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    | - Error in Appointment: Compilation error in Appointment.cs: Appointment.cs does not declare Id, EndTime, Status, and the constructor does not match the call in Program.cs. A single Appointment model needs to be standardized before modifying other services.<br>- At the same time, the constructor must receive all the necessary data and check EndTime > StartTime. Simply adding attributes to fix compilation errors should not be done while ignoring business logic.<br>- Additionally, to ensure the status after the update is standardized, I've made a few adjustments.<br>- Error in the AppointmentScheduler section.<br>- In the ValidationHelper class, to normalize the state, case sensitivity is not a factor during validation.<br>- MedicalHistory.cs is used to standardize IDs, process medical record content, and eliminate unnecessary usage.<br>- Add test cases in Program.cs.                                                                                                                                                                                                                                                                                      | - Appointment Module: Declare additional attributes `Id`, `EndTime`, `Status`, `VisitOutcome` and update the constructor to ensure the business rule `EndTime > StartTime`, Normalize appointment status after updating and synchronizing the data model with `Program.cs`.<br>- ValidationHelper Module: Add case-insensitive checking and normalize data input.<br>- AppointmentScheduler Module: Fixed logic to block duplicate `Appointment ID`, check for doctor/patient time conflicts, skip canceled appointments, synchronize calls to `UpdateStatus()`, and support case-insensitive ID search.<br>- MedicalHistory Module: Standardize appointment IDs, optimize patient record content processing, and clean up redundant libraries (`using`).<br>- Program.cs (Main UI & Testing): Match the new constructor call and add realistic test case scenarios to the Console interface.              |
| **Week 3** | **Gemini**<br>_(Gemini 3.6 Flash)_ | 1. Based on the OOP course topic I'm working on, please suggest a README file structure for me to complete my report.<br>2. I would like to add a list of functions including: Add new patient, Add new doctor/staff, Schedule appointment, Complete appointment, Record medical history, Create prescription, View patient profiles, View appointment list, View prescription list, Exit program<br>- The requirement is: The program needs to display a numbered menu, allowing the user to select and perform the corresponding function. After completion, the program should return to the main menu until the user selects "exit". Please provide support and suggestions to help me complete this idea. | 1. The README.md for the topic Hospital/Clinic Patient Management System consists of 10 parts: Project Overview, Features and Requirements, Project Structure, System Design, OOP Principles and Self-Explanation, Validation and Error Handling, Test Cases and Test Results, Data Privacy Considerations, Build and Run Instructions, AI Usage Log.<br>2.AI assisted in building a hospital management interface on the Console using the C# language. The generated code includes:<br>- The menu displays hospital management functions in numerical order.<br>- A loop allows the program to continue running until the user chooses to exit.<br>- A switch condition structure is used to handle user selections.<br>- Calls are made to the corresponding Model and Service classes to perform the functionality.<br>- Basic handling procedures when a user enters an invalid selection.<br>- Separate the menu interface and the test execution into separate files.                                                                                                                                                                                                                        | - 1. Complete the structure and content of the README file according to the suggestions provided by the AI. 2. The tasks I performed include:<br>- Menu integration: Connect menu options with the Model and Service classes already built into the project.<br>- Program organization: Separate the testing section from the interactive menu section and place the testing logic in the file Test/TestRunner.cs.<br>- Testing the operational process: Run the program to check the display of the menu, inputting selections, and navigating to the corresponding function.<br>- Error handling test: Examine how the program handles situations where the user enters an invalid selection and how it returns to the main menu.<br>- Understanding the code: Read and understand how the program receives data from the user, uses conditional structures, and calls methods to perform its functions. |
| **Week 4** | **ChatGPT**<br>_(ChatGPT-6)_       | 1. Review and evaluate the HospitalOOP project based on the README file and source code.<br>2. OOP Review: Explain and evaluate the four principles of OOP                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     | 1. Evaluate the implementation of the four principles of OOP: Encapsulation, Inheritance, Polymorphism, and Abstraction.<br>2. Provide a list of additional sections to check, such as Validation, Appointment Scheduling, Medical History, and Test Cases.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         | - Examine the OOP properties: encapsulation, inheritance, polymorphism, and abstraction.<br>- Implement and test polymorphism by calling the `TestPolymorphism` functions within`main`.<br>- Evaluating Validation & Time Constraints, Appointment Conflict & Rescheduling Management, and Appointment Status Workflow.<br>- The TestRunner.cs file has successfully executed 18 test cases across 4 main categories (Appointments & Status: 10, Prescriptions: 4, Medical History: 3, Validation: 1), ensuring that the system correctly handles appointment scheduling/rescheduling, medical records, and prescriptions, as well as exception handling for invalid data.                                                                                                                                                                                                                                 |
During the development of the Hospital Management System, the team used AI tools to support implementation, debugging, code review, testing, and documentation. **Gemini 3.6 Flash** was declared in the AI-use form. ChatGPT was also used as an assistant for explaining code, troubleshooting errors, and reviewing the project.

## 10. Conclusion

The Hospital Management System successfully demonstrates the application of fundamental Object-Oriented Programming principles, including Encapsulation, Inheritance, Abstraction, and Polymorphism.

The five test scenarios (TC01–TC05) verify the main system functionalities, including patient registration, appointment scheduling, input validation, appointment status transitions, and medical history management. All five scenarios passed successfully.

The system also ensures that newly created appointments start with the Scheduled status and can transition to Completed or Cancelled only when the corresponding business rules are satisfied.

Overall, this project provides a practical demonstration of how OOP concepts can be applied to build a structured and maintainable hospital management system. Although it is an educational console application with in-memory data storage, it establishes a foundation for future improvements, such as database integration, authentication, and a graphical user interface.


