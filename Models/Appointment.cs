using System;

public class Appointment
{
    public string Id { get; }
    public Patient Patient { get; }
    public Doctor Doctor { get; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public string Status { get; internal set; }
    public string? VisitOutcome { get; private set; }

    public Appointment(string id, Patient patient, Doctor doctor, DateTime startTime)
    {
        ValidationHelper.RequireNotEmpty(id, nameof(id));
        ArgumentNullException.ThrowIfNull(patient);
        ArgumentNullException.ThrowIfNull(doctor);
        ValidationHelper.RequireFutureDate(startTime, "Appointment start time");

        Id = id.Trim();
        Patient = patient;
        Doctor = doctor;
        StartTime = startTime;
        EndTime = startTime.AddMinutes(30);
        Status = "Scheduled";
    }

    public void UpdateStatus(string newStatus)
    {
        string normalizedStatus = ValidationHelper.NormalizeAppointmentStatus(newStatus);

        if (!ValidationHelper.IsValidStatusTransition(Status, normalizedStatus))
        {
            throw new InvalidOperationException(
                $"Cannot change appointment status from '{Status}' to '{normalizedStatus}'.");
        }

        Status = normalizedStatus;
    }

    // Only the scheduler may change appointment times, after checking conflicts.
    internal void Reschedule(DateTime newStartTime)
    {
        ValidationHelper.RequireFutureDate(newStartTime, "Appointment start time");

        if (Status != "Scheduled")
            throw new InvalidOperationException("Only scheduled appointments can be rescheduled.");

        StartTime = newStartTime;
        EndTime = newStartTime.AddMinutes(30);
    }

    public void RecordVisitOutcome(string outcome)
    {
        ValidationHelper.RequireNotEmpty(outcome, nameof(outcome));

        if (Status != "Completed")
            throw new InvalidOperationException(
                "A visit outcome can only be recorded for a completed appointment.");

        if (VisitOutcome != null)
            throw new InvalidOperationException(
                "A visit outcome has already been recorded for this appointment.");

        VisitOutcome = outcome.Trim();
    }
}
