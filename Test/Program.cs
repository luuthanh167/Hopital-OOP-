using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

internal static class Program
{
    private static readonly List<Patient> patients = new();
    private static readonly List<Doctor> doctors = new();
    private static readonly List<Prescription> prescriptions = new();
    private static readonly Dictionary<string, MedicalHistory> medicalHistories = new(StringComparer.OrdinalIgnoreCase);
    private static readonly AppointmentScheduler scheduler = new();

    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        bool running = true;
        while (running)
        {
            ShowMenu();
            Console.Write("Chọn chức năng: ");
            string choice = (Console.ReadLine() ?? "").Trim();
            try
            {
                switch (choice)
                {
                    case "1": AddPatient(); break;
                    case "2": AddDoctor(); break;
                    case "3": ScheduleAppointment(); break;
                    case "4": CompleteAppointment(); break;
                    case "5": RecordMedicalHistory(); break;
                    case "6": CreatePrescription(); break;
                    case "7": ViewPatientRecord(); break;
                    case "8": ViewAppointments(); break;
                    case "9": ViewPrescriptions(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Lựa chọn không hợp lệ."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nKhông thể thực hiện: {ex.Message}");
            }
            if (running)
            {
                Console.WriteLine("\nNhấn Enter để tiếp tục...");
                Console.ReadLine();
            }
        }
        Console.WriteLine("Đã thoát hệ thống quản lý bệnh án.");
    }

    private static void ShowMenu()
    {
        Console.Clear();
        Console.WriteLine("==================================================");
        Console.WriteLine("        HỆ THỐNG QUẢN LÝ BỆNH ÁN BỆNH VIỆN");
        Console.WriteLine("==================================================");
        Console.WriteLine("1. Tiếp nhận bệnh nhân");
        Console.WriteLine("2. Thêm bác sĩ");
        Console.WriteLine("3. Đặt lịch khám");
        Console.WriteLine("4. Hoàn tất lịch khám");
        Console.WriteLine("5. Ghi bệnh án / kết quả khám");
        Console.WriteLine("6. Kê đơn thuốc");
        Console.WriteLine("7. Xem hồ sơ bệnh nhân");
        Console.WriteLine("8. Xem danh sách lịch khám");
        Console.WriteLine("9. Xem danh sách đơn thuốc");
        Console.WriteLine("0. Thoát");
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine($"Bệnh nhân: {patients.Count} | Bác sĩ: {doctors.Count} | Lịch khám: {scheduler.GetAppointments().Count}");
        Console.WriteLine("==================================================");
    }

    private static void AddPatient()
    {
        Console.WriteLine("\n--- TIẾP NHẬN BỆNH NHÂN ---");
        string id = ReadRequired("Mã bệnh nhân: ");
        if (patients.Any(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("Mã bệnh nhân đã tồn tại.");
        string name = ReadRequired("Họ và tên: ");
        string phone = ReadRequired("Số điện thoại: ");
        Patient patient = new(id, name, phone);
        patients.Add(patient);
        medicalHistories[patient.Id] = new MedicalHistory($"MH-{patient.Id}", patient);
        Console.WriteLine($"Đã tiếp nhận bệnh nhân {patient.Name} (mã {patient.Id}).");
    }

    private static void AddDoctor()
    {
        Console.WriteLine("\n--- THÊM BÁC SĨ ---");
        string id = ReadRequired("Mã bác sĩ: ");
        if (doctors.Any(d => d.Id.Equals(id, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("Mã bác sĩ đã tồn tại.");
        string name = ReadRequired("Họ và tên bác sĩ: ");
        string specialty = ReadRequired("Chuyên khoa: ");
        string phone = ReadRequired("Số điện thoại: ");
        Doctor doctor = new(id, name, specialty, phone);
        doctors.Add(doctor);
        Console.WriteLine($"Đã thêm bác sĩ {doctor.Name} - {doctor.Specialty}.");
    }

    private static void ScheduleAppointment()
    {
        Console.WriteLine("\n--- ĐẶT LỊCH KHÁM ---");
        if (patients.Count == 0 || doctors.Count == 0)
        {
            Console.WriteLine("Cần tiếp nhận ít nhất một bệnh nhân và thêm một bác sĩ trước.");
            return;
        }
        ShowPatients();
        Patient patient = FindPatient(ReadRequired("Mã bệnh nhân: "));
        ShowDoctors();
        Doctor doctor = FindDoctor(ReadRequired("Mã bác sĩ: "));
        string id = ReadRequired("Mã lịch khám: ");
        DateTime start = ReadFutureDateTime("Thời gian khám (yyyy-MM-dd HH:mm): ");
        Appointment appointment = new(id, patient, doctor, start);
        scheduler.ScheduleAppointment(appointment);
        Console.WriteLine($"Đặt lịch thành công: {appointment.Id} | {patient.Name} | BS. {doctor.Name} | {start:dd/MM/yyyy HH:mm}");
    }

    private static void CompleteAppointment()
    {
        Console.WriteLine("\n--- HOÀN TẤT LỊCH KHÁM ---");
        ViewAppointments();
        string id = ReadRequired("\nMã lịch khám cần hoàn tất: ");
        Appointment? appointment = scheduler.GetAppointments().FirstOrDefault(a => a.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        if (appointment == null)
            throw new InvalidOperationException("Không tìm thấy lịch khám.");
        if (!scheduler.UpdateAppointmentStatus(id, "Completed"))
            throw new InvalidOperationException($"Không thể hoàn tất lịch khám có trạng thái '{appointment.Status}'.");
        Console.WriteLine($"Lịch khám {appointment.Id} đã chuyển sang trạng thái Completed.");
    }

    private static void RecordMedicalHistory()
    {
        Console.WriteLine("\n--- GHI KẾT QUẢ KHÁM VÀO BỆNH ÁN ---");
        ViewAppointments();
        string id = ReadRequired("\nMã lịch khám đã hoàn tất: ");
        Appointment? appointment = scheduler.GetAppointments().FirstOrDefault(a => a.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        if (appointment == null)
            throw new InvalidOperationException("Không tìm thấy lịch khám.");
        if (!medicalHistories.TryGetValue(appointment.Patient.Id, out MedicalHistory? history))
            throw new InvalidOperationException("Chưa có hồ sơ bệnh án cho bệnh nhân này.");
        Console.WriteLine($"Bệnh nhân: {appointment.Patient.Name} | Bác sĩ: {appointment.Doctor.Name}");
        string outcome = ReadRequired("Kết quả khám / chẩn đoán: ");
        history.AddVisitRecord(appointment, outcome);
        Console.WriteLine("Đã lưu kết quả khám vào hồ sơ bệnh án.");
    }

    private static void CreatePrescription()
    {
        Console.WriteLine("\n--- KÊ ĐƠN THUỐC ---");
        if (patients.Count == 0 || doctors.Count == 0)
        {
            Console.WriteLine("Cần có bệnh nhân và bác sĩ trước khi kê đơn.");
            return;
        }
        ShowPatients();
        Patient patient = FindPatient(ReadRequired("Mã bệnh nhân: "));
        ShowDoctors();
        Doctor doctor = FindDoctor(ReadRequired("Mã bác sĩ: "));
        string id = ReadRequired("Mã đơn thuốc: ");
        if (prescriptions.Any(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("Mã đơn thuốc đã tồn tại.");
        string medication = ReadRequired("Tên thuốc: ");
        string dosage = ReadRequired("Liều dùng: ");
        string instructions = ReadRequired("Hướng dẫn sử dụng: ");
        Prescription prescription = new(id, patient, doctor, medication, dosage, instructions);
        prescriptions.Add(prescription);
        Console.WriteLine("\nĐã tạo đơn thuốc:");
        prescription.DisplayPrescription();
    }

    private static void ViewPatientRecord()
    {
        Console.WriteLine("\n--- TRA CỨU HỒ SƠ BỆNH NHÂN ---");
        if (patients.Count == 0)
        {
            Console.WriteLine("Chưa có bệnh nhân nào.");
            return;
        }
        ShowPatients();
        Patient patient = FindPatient(ReadRequired("Nhập mã bệnh nhân cần tra cứu: "));
        Console.WriteLine("\n==================================================");
        Console.WriteLine("                 HỒ SƠ BỆNH NHÂN");
        Console.WriteLine("==================================================");
        Console.WriteLine($"Mã bệnh nhân : {patient.Id}");
        Console.WriteLine($"Họ và tên    : {patient.Name}");
        Console.WriteLine($"Số điện thoại: {patient.Phone}");
        Console.WriteLine("--------------------------------------------------");
        Appointment[] visits = scheduler.GetAppointments()
            .Where(a => a.Patient.Id.Equals(patient.Id, StringComparison.OrdinalIgnoreCase))
            .OrderBy(a => a.StartTime).ToArray();
        Console.WriteLine("LỊCH SỬ LỊCH KHÁM:");
        if (visits.Length == 0) Console.WriteLine("  Chưa có lịch khám.");
        foreach (Appointment a in visits)
            Console.WriteLine($"  {a.Id} | {a.StartTime:dd/MM/yyyy HH:mm} | BS. {a.Doctor.Name} | {a.Status}");
        Console.WriteLine("\nBỆNH ÁN / KẾT QUẢ KHÁM:");
        string record = patient.GetMedicalRecord();
        Console.WriteLine(string.IsNullOrWhiteSpace(record) ? "  Chưa có ghi nhận." : record);
        Console.WriteLine("\nĐƠN THUỐC:");
        Prescription[] patientPrescriptions = prescriptions.Where(p => p.Patient.Id.Equals(patient.Id, StringComparison.OrdinalIgnoreCase)).ToArray();
        if (patientPrescriptions.Length == 0) Console.WriteLine("  Chưa có đơn thuốc.");
        foreach (Prescription p in patientPrescriptions)
            Console.WriteLine($"  {p.GetSummary()} | Bác sĩ: {p.Doctor.Name} | Ngày kê: {p.PrescribedDate:dd/MM/yyyy HH:mm}");
        Console.WriteLine("==================================================");
    }

    private static void ViewAppointments()
    {
        Console.WriteLine("\n--- DANH SÁCH LỊCH KHÁM ---");
        IReadOnlyList<Appointment> appointments = scheduler.GetAppointments();
        if (appointments.Count == 0)
        {
            Console.WriteLine("Chưa có lịch khám.");
            return;
        }
        foreach (Appointment a in appointments.OrderBy(a => a.StartTime))
            Console.WriteLine($"{a.Id} | {a.StartTime:dd/MM/yyyy HH:mm}-{a.EndTime:HH:mm} | BN: {a.Patient.Name} ({a.Patient.Id}) | BS: {a.Doctor.Name} ({a.Doctor.Id}) | {a.Status}");
    }

    private static void ViewPrescriptions()
    {
        Console.WriteLine("\n--- DANH SÁCH ĐƠN THUỐC ---");
        if (prescriptions.Count == 0)
        {
            Console.WriteLine("Chưa có đơn thuốc.");
            return;
        }
        foreach (Prescription prescription in prescriptions)
        {
            Console.WriteLine("------------------------------------------");
            prescription.DisplayPrescription();
        }
    }

    private static void ShowPatients()
    {
        Console.WriteLine("\nDanh sách bệnh nhân:");
        foreach (Patient patient in patients)
            Console.WriteLine($"  {patient.Id} - {patient.Name} - {patient.Phone}");
    }

    private static void ShowDoctors()
    {
        Console.WriteLine("\nDanh sách bác sĩ:");
        foreach (Doctor doctor in doctors)
            Console.WriteLine($"  {doctor.Id} - {doctor.Name} - {doctor.Specialty}");
    }

    private static Patient FindPatient(string id) =>
        patients.FirstOrDefault(p => p.Id.Equals(id.Trim(), StringComparison.OrdinalIgnoreCase))
        ?? throw new InvalidOperationException("Không tìm thấy bệnh nhân với mã này.");

    private static Doctor FindDoctor(string id) =>
        doctors.FirstOrDefault(d => d.Id.Equals(id.Trim(), StringComparison.OrdinalIgnoreCase))
        ?? throw new InvalidOperationException("Không tìm thấy bác sĩ với mã này.");

    private static string ReadRequired(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string value = (Console.ReadLine() ?? "").Trim();
            if (!string.IsNullOrWhiteSpace(value)) return value;
            Console.WriteLine("Thông tin không được để trống, vui lòng nhập lại.");
        }
    }

    private static DateTime ReadFutureDateTime(string prompt)
    {
        while (true)
        {
            string input = ReadRequired(prompt);
            if (DateTime.TryParseExact(input, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime value)
                && value > DateTime.Now)
                return value;
            Console.WriteLine("Thời gian không hợp lệ/đã qua. Nhập theo mẫu yyyy-MM-dd HH:mm và chọn thời gian tương lai.");
        }
    }
}
