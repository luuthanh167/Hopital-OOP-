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
        ArgumentNullException.ThrowIfNull(patient);
        ID = id.Trim();
        Patient = patient;
    }

    public void AddVisitRecord(Appointment appointment, string outcome)
    {
        ArgumentNullException.ThrowIfNull(appointment);

        if (!string.Equals(appointment.Patient.Id, Patient.Id, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Appointment does not belong to this patient.");

        ValidationHelper.RequireNotEmpty(outcome, nameof(outcome));
        string normalizedOutcome = outcome.Trim();

        // RecordVisitOutcome validates status and duplicate outcomes before the history is changed.
        appointment.RecordVisitOutcome(normalizedOutcome);
        string recordEntry =
            $"[{appointment.StartTime:yyyy-MM-dd HH:mm}] Doctor: {appointment.Doctor.Name} | Outcome: {normalizedOutcome}";

        string existingRecord = Patient.GetMedicalRecord();
        string updatedRecord = string.IsNullOrWhiteSpace(existingRecord)
            ? recordEntry
            : $"{existingRecord}{Environment.NewLine}{recordEntry}";

        Patient.UpdateMedicalRecord(updatedRecord);
        completedAppointments.Add(appointment);
    }

    public IReadOnlyList<Appointment> GetCompletedAppointments() => completedAppointments.AsReadOnly();
}
