using System;
using System.Collections.Generic;

public class MedicalHistory
{
    public string ID { get; }
    public Patient Patient { get; }

    private readonly List<Appointment> completedAppointments = new();

    public MedicalHistory(string id, Patient patient)
    {
        ValidationHelper.RequireNotEmpty(id, nameof(id));

        if (patient == null)
            throw new ArgumentNullException(nameof(patient), "Patient cannot be null.");

        ID = id.Trim();
        Patient = patient;
    }

    public void AddVisitRecord(Appointment appointment, string outcome)
    {
        if (appointment == null)
            throw new ArgumentNullException(nameof(appointment), "Appointment cannot be null.");

        if (!string.Equals(appointment.Patient.Id, Patient.Id, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Appointment does not belong to this patient.");

        ValidationHelper.RequireNotEmpty(outcome, nameof(outcome));
        string normalizedOutcome = outcome.Trim();

        // Appointment validates that the visit is completed and has no existing outcome.
        appointment.RecordVisitOutcome(normalizedOutcome);
        completedAppointments.Add(appointment);

        string recordEntry =
            $"[{appointment.StartTime:yyyy-MM-dd HH:mm}] Doctor: {appointment.Doctor.Name} | Outcome: {normalizedOutcome}";

        string existingRecord = Patient.GetMedicalRecord();
        string updatedRecord = string.IsNullOrWhiteSpace(existingRecord)
            ? recordEntry
            : $"{existingRecord}{Environment.NewLine}{recordEntry}";

        Patient.UpdateMedicalRecord(updatedRecord);
    }

    public IReadOnlyList<Appointment> GetCompletedAppointments()
    {
        return completedAppointments.AsReadOnly();
    }
}
