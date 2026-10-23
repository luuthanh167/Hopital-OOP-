using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("HOSPITAL MANAGEMENT SYSTEM\n");

        Doctor doctor1 = new Doctor("DOC01", "Dr. John Smith", "Cardiology", "0901234567");
        Patient patient1 = new Patient("PAT01", "Jane Doe", "0987654321");
        Patient patient2 = new Patient("PAT02", "Alex Brown", "0912345678");

        Console.WriteLine($"[Created] Doctor: {doctor1.Name} - Specialty: {doctor1.Specialty}");
        Console.WriteLine($"[Created] Patient: {patient1.Name} - Phone: {patient1.Phone}\n");

        AppointmentScheduler scheduler = new AppointmentScheduler();
        DateTime futureTime = DateTime.Now.AddDays(1);

        Appointment appointment1 = new Appointment("APP01", patient1, doctor1, futureTime);
        scheduler.ScheduleAppointment(appointment1);
        Console.WriteLine($"[Success] Scheduled appointment {appointment1.Id} for {appointment1.StartTime:yyyy-MM-dd HH:mm}");

        // A second appointment overlapping the same doctor's schedule should be rejected.
        Appointment overlappingAppointment = new Appointment(
            "APP02", patient2, doctor1, futureTime.AddMinutes(15));
        ExpectException<InvalidOperationException>(
            () => scheduler.ScheduleAppointment(overlappingAppointment),
            "Reject overlapping appointments");

        // Status matching is case-insensitive; the stored status remains canonical.
        bool completed = scheduler.UpdateAppointmentStatus("APP01", "completed");
        Console.WriteLine(completed
            ? $"[Updated] Appointment {appointment1.Id} Status: {appointment1.Status}\n"
            : "[Error] Could not update appointment status.");

        // A completed appointment cannot transition to another status.
        bool invalidTransitionAccepted = scheduler.UpdateAppointmentStatus("APP01", "Cancelled");
        Console.WriteLine(!invalidTransitionAccepted
            ? "[Passed] Rejected invalid status transition."
            : "[Failed] Invalid status transition was accepted.");

        MedicalHistory medicalHistory = new MedicalHistory("MH01", patient1);
        string visitOutcome = "Patient blood pressure is stable. Prescribed medication for 7 days.";
        medicalHistory.AddVisitRecord(appointment1, visitOutcome);
        Console.WriteLine("[Success] Visit outcome recorded to medical history.\n");

        // A visit outcome must not be recorded twice for the same appointment.
        ExpectException<InvalidOperationException>(
            () => medicalHistory.AddVisitRecord(appointment1, visitOutcome),
            "Reject duplicate visit record");

        Console.WriteLine("=== PATIENT MEDICAL RECORD HISTORY ===");
        Console.WriteLine(patient1.GetMedicalRecord());
    }

    private static void ExpectException<TException>(Action action, string testName)
        where TException : Exception
    {
        try
        {
            action();
            Console.WriteLine($"[Failed] {testName}: expected {typeof(TException).Name}.");
        }
        catch (TException)
        {
            Console.WriteLine($"[Passed] {testName}.");
        }
    }
}
