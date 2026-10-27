using System;

internal static class Program
{
    private static int passed;
    private static int failed;

    private static void Main()
    {
        Console.WriteLine("HOSPITAL MANAGEMENT SYSTEM - TEST RUN\n");

        Doctor doctor = new("DOC01", "Dr. John Smith", "Cardiology", "0901234567");
        Patient patient1 = new("PAT01", "Jane Doe", "0987654321");
        Patient patient2 = new("PAT02", "Alex Brown", "0912345678");
        AppointmentScheduler scheduler = new();
        DateTime firstTime = DateTime.Now.AddDays(1);

        Appointment appointment = new("APP01", patient1, doctor, firstTime);
        Check("Schedule a valid appointment", () => scheduler.ScheduleAppointment(appointment));
        CheckThrows<InvalidOperationException>(
            () => scheduler.ScheduleAppointment(new Appointment("APP02", patient2, doctor, firstTime.AddMinutes(15))),
            "Reject overlapping doctor appointments");
        Check("Allow back-to-back appointments", () =>
            scheduler.ScheduleAppointment(new Appointment("APP03", patient2, doctor, firstTime.AddMinutes(30))));

        DateTime originalStart = appointment.StartTime;
        CheckThrows<InvalidOperationException>(
            () => scheduler.RescheduleAppointment("APP01", firstTime.AddMinutes(15)),
            "Reject reschedule into a conflict");
        Check("Failed reschedule leaves original time unchanged", () => appointment.StartTime == originalStart);
        Check("Reschedule to a free time", () =>
            scheduler.RescheduleAppointment("APP01", firstTime.AddHours(2)));
        Check("Unknown appointment returns false", () => !scheduler.RescheduleAppointment("UNKNOWN", firstTime.AddHours(3)));

        Check("Case-insensitive status update", () => scheduler.UpdateAppointmentStatus("APP01", "completed")
            && appointment.Status == "Completed");
        Check("Reject invalid status transition", () => !scheduler.UpdateAppointmentStatus("APP01", "Cancelled"));
        CheckThrows<ArgumentException>(
            () => scheduler.UpdateAppointmentStatus("APP03", "Waiting"),
            "Reject unknown status");

        MedicalHistory history = new("MH01", patient1);
        string outcome = "Blood pressure is stable.";
        CheckThrows<InvalidOperationException>(
            () => history.AddVisitRecord(scheduler.GetAppointments()[1], "Wrong patient"),
            "Reject visit record for another patient");
        Check("Record completed visit", () =>
        {
            history.AddVisitRecord(appointment, outcome);
            return history.GetCompletedAppointments().Count == 1 &&
                   patient1.GetMedicalRecord().Contains(outcome, StringComparison.Ordinal);
        });
        CheckThrows<InvalidOperationException>(
            () => history.AddVisitRecord(appointment, outcome),
            "Reject duplicate visit outcome");

        Prescription prescription = new(
            " RX01 ", patient1, doctor, "  Example medicine  ", " 1 tablet daily ", " Take after meals ");
        Check("Prescription trims input", () =>
            prescription.Id == "RX01" &&
            prescription.MedicationName == "Example medicine" &&
            prescription.Dosage == "1 tablet daily" &&
            prescription.Instructions == "Take after meals");
        prescription.UpdateDosage(" 2 tablets daily ");
        prescription.UpdateInstructions(" Take with water ");
        Check("Prescription updates and summary", () =>
            prescription.Dosage == "2 tablets daily" &&
            prescription.Instructions == "Take with water" &&
            prescription.GetSummary().Contains("Example medicine", StringComparison.Ordinal));
        CheckThrows<ArgumentException>(
            () => prescription.UpdateDosage("   "),
            "Reject empty prescription dosage");
        CheckThrows<ArgumentException>(
            () => new Prescription("RX02", patient1, doctor, "Medicine", "", "Instructions"),
            "Reject missing prescription dosage");
        CheckThrows<ArgumentException>(
            () => new Appointment("OLD", patient1, doctor, DateTime.Now.AddMinutes(-1)),
            "Reject appointment in the past");

        Console.WriteLine($"\nRESULT: {passed} passed, {failed} failed.");
        if (failed > 0)
            Environment.ExitCode = 1;
    }

    private static void Check(string name, Func<bool> test)
    {
        try
        {
            if (test())
            {
                passed++;
                Console.WriteLine($"[PASS] {name}");
            }
            else
            {
                failed++;
                Console.WriteLine($"[FAIL] {name}");
            }
        }
        catch (Exception ex)
        {
            failed++;
            Console.WriteLine($"[FAIL] {name}: unexpected {ex.GetType().Name} - {ex.Message}");
        }
    }

    private static void CheckThrows<TException>(Action action, string name) where TException : Exception
    {
        try
        {
            action();
            failed++;
            Console.WriteLine($"[FAIL] {name}: expected {typeof(TException).Name}");
        }
        catch (TException)
        {
            passed++;
            Console.WriteLine($"[PASS] {name}");
        }
        catch (Exception ex)
        {
            failed++;
            Console.WriteLine($"[FAIL] {name}: expected {typeof(TException).Name}, got {ex.GetType().Name}");
        }
    }
}
