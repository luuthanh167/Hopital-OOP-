using System;
using System.Collections.Generic;
using System.Linq;

public class AppointmentScheduler
{
    private readonly List<Appointment> appointments = new();

    public bool ScheduleAppointment(Appointment newAppointment)
    {
        ArgumentNullException.ThrowIfNull(newAppointment);
        ValidationHelper.RequireFutureDate(newAppointment.StartTime, "Appointment start time");

        if (appointments.Any(a => string.Equals(a.Id, newAppointment.Id, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"An appointment with ID '{newAppointment.Id}' already exists.");

        if (newAppointment.Status != "Scheduled")
            throw new InvalidOperationException("Only appointments with Scheduled status can be added.");

        EnsureNoConflicts(newAppointment, null);
        appointments.Add(newAppointment);
        return true;
    }

    public bool RescheduleAppointment(string appointmentId, DateTime newStartTime)
    {
        ValidationHelper.RequireNotEmpty(appointmentId, nameof(appointmentId));
        ValidationHelper.RequireFutureDate(newStartTime, "Appointment start time");

        Appointment? appointment = appointments.FirstOrDefault(a =>
            string.Equals(a.Id, appointmentId.Trim(), StringComparison.OrdinalIgnoreCase));

        if (appointment == null)
            return false;

        if (appointment.Status != "Scheduled")
            throw new InvalidOperationException("Only scheduled appointments can be rescheduled.");

        // Validate using a temporary appointment so a failed reschedule never mutates the original.
        Appointment proposed = new Appointment(
            appointment.Id, appointment.Patient, appointment.Doctor, newStartTime);
        EnsureNoConflicts(proposed, appointment);
        appointment.Reschedule(newStartTime);
        return true;
    }

    public bool UpdateAppointmentStatus(string appointmentId, string newStatus)
    {
        ValidationHelper.RequireNotEmpty(appointmentId, nameof(appointmentId));
        string normalizedStatus = ValidationHelper.NormalizeAppointmentStatus(newStatus);

        Appointment? appointment = appointments.FirstOrDefault(a =>
            string.Equals(a.Id, appointmentId.Trim(), StringComparison.OrdinalIgnoreCase));

        if (appointment == null)
            return false;

        if (!ValidationHelper.IsValidStatusTransition(appointment.Status, normalizedStatus))
            return false;

        appointment.UpdateStatus(normalizedStatus);
        return true;
    }

    private void EnsureNoConflicts(Appointment candidate, Appointment? ignoredAppointment)
    {
        bool conflicts = appointments.Any(existing =>
            !ReferenceEquals(existing, ignoredAppointment) &&
            existing.Status != "Cancelled" &&
            (string.Equals(existing.Doctor.Id, candidate.Doctor.Id, StringComparison.OrdinalIgnoreCase) ||
             string.Equals(existing.Patient.Id, candidate.Patient.Id, StringComparison.OrdinalIgnoreCase)) &&
            Overlaps(existing, candidate));

        if (conflicts)
            throw new InvalidOperationException(
                "The doctor or patient already has an appointment during this time.");
    }

    private static bool Overlaps(Appointment first, Appointment second) =>
        second.StartTime < first.EndTime && second.EndTime > first.StartTime;

    public IReadOnlyList<Appointment> GetAppointments() => appointments.AsReadOnly();
}
