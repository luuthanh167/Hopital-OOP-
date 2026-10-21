using System;

public class Appointment
{
    public string Id { get; }
    public Patient Patient { get; }
    public Doctor Doctor { get; }
    public DateTime StartTime { get; }
    public DateTime EndTime { get; }
    public string Status { get; private set; }
    public string? VisitOutcome { get; private set; }

    public Appointment(
        string id,
        Patient patient,
        Doctor doctor,
        DateTime startTime)
    {
        ValidationHelper.RequireNotEmpty(id, nameof(id));

        if (patient == null)
        {
            throw new ArgumentNullException(nameof(patient));
        }

        if (doctor == null)
        {
            throw new ArgumentNullException(nameof(doctor));
        }

        ValidationHelper.RequireFutureDate(startTime, "Appointment start time");

        Id = id.Trim();
        Patient = patient;
        Doctor = doctor;
        StartTime = startTime;
        EndTime = startTime.AddMinutes(30);
        Status = "Scheduled";
        VisitOutcome = null;
    }

    public void UpdateStatus(string newStatus)
    {
        ValidationHelper.RequireNotEmpty(newStatus, nameof(newStatus));

        if (!ValidationHelper.IsValidStatusTransition(Status, newStatus))
        {
            throw new InvalidOperationException(
                $"Cannot change appointment status from '{Status}' to '{newStatus}'.");
        }

        Status = newStatus;
    }

    public void RecordVisitOutcome(string outcome)
    {
        ValidationHelper.RequireNotEmpty(outcome, nameof(outcome));

        if (Status != "Completed")
        {
            throw new InvalidOperationException(
                "A visit outcome can only be recorded for a completed appointment.");
        }

        if (VisitOutcome != null)
        {
            throw new InvalidOperationException(
                "A visit outcome has already been recorded for this appointment.");
        }

        VisitOutcome = outcome.Trim();
    }
}