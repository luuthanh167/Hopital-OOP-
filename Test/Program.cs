using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("HOSPITAL MANAGEMENT SYSTEM\n");

        Doctor doctor1 = new Doctor("DOC01", "Dr. John Smith", "Cardiology", "0901234567");
        Patient patient1 = new Patient("PAT01", "Jane Doe", "0987654321");

        Console.WriteLine($"[Created] Doctor: {doctor1.Name} - Specialty: {doctor1.Specialty}");
        Console.WriteLine($"[Created] Patient: {patient1.Name} - Phone: {patient1.Phone}\n");

        AppointmentScheduler scheduler = new AppointmentScheduler();

        DateTime futureTime = DateTime.Now.AddDays(1); 
        Appointment appointment1 = new Appointment("APP01", patient1, doctor1, futureTime);

        scheduler.ScheduleAppointment(appointment1);
        Console.WriteLine($"[Success] Scheduled appointment {appointment1.Id} for {appointment1.StartTime:yyyy-MM-dd HH:mm}");


        scheduler.UpdateAppointmentStatus("APP01", "Completed");
        Console.WriteLine($"[Updated] Appointment {appointment1.Id} Status: {appointment1.Status}\n");

        MedicalHistory medicalHistory = new MedicalHistory("MH01", patient1);

        string visitOutcome = "Patient blood pressure is stable. Prescribed medication for 7 days.";
        medicalHistory.AddVisitRecord(appointment1, visitOutcome);

        Console.WriteLine("[Success] Visit outcome recorded to medical history.\n");

        Console.WriteLine("=== PATIENT MEDICAL RECORD HISTORY ===");
        Console.WriteLine(patient1.GetMedicalRecord());
    }
}