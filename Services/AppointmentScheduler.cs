using System;
using System.Collections.Generic;
using System.Linq;

public class AppointmentScheduler
{
    private readonly List<Appointment> appointments = new();

    public bool ScheduleAppointment(Appointment newAppointment)
    {
        if (newAppointment == null)
        {
            throw new ArgumentNullException(
                nameof(newAppointment),
                "Appointment cannot be null.");
        }

        ValidationHelper.RequireNotEmpty(newAppointment.Id, "Appointment ID");
        ValidationHelper.RequireFutureDate(newAppointment.StartTime, "Appointment start time");

        if (appointments.Any(a =>
            string.Equals(a.Id, newAppointment.Id, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException(
                $"An appointment with ID '{newAppointment.Id}' already exists.");
        }

        if (newAppointment.Status != "Scheduled")
        {
            throw new InvalidOperationException(
                "Only appointments with Scheduled status can be added.");
        }

        if (HasDoctorConflict(newAppointment))
        {
            throw new InvalidOperationException(
                $"Doctor '{newAppointment.Doctor.Name}' already has an appointment during this time.");
        }

        if (HasPatientConflict(newAppointment))
        {
            throw new InvalidOperationException(
                $"Patient '{newAppointment.Patient.Name}' already has an appointment during this time.");
        }

        appointments.Add(newAppointment);
        return true;
    }

    public bool UpdateAppointmentStatus(string appointmentId, string newStatus)
    {
        ValidationHelper.RequireNotEmpty(appointmentId, "Appointment ID");
        ValidationHelper.RequireNotEmpty(newStatus, "New status");

        var appointment = appointments.FirstOrDefault(a =>
            string.Equals(a.Id, appointmentId.Trim(), StringComparison.OrdinalIgnoreCase));

        if (appointment == null)
        {
            return false;
        }

        if (!ValidationHelper.IsValidStatusTransition(appointment.Status, newStatus))
        {
            return false;
        }

        appointment.UpdateStatus(newStatus);
        return true;
    }

    private bool HasDoctorConflict(Appointment newAppointment)
    {
        return appointments.Any(existing =>
            existing.Status != "Cancelled" &&
            existing.Doctor.Id == newAppointment.Doctor.Id &&
            Overlaps(existing, newAppointment));
    }

    private bool HasPatientConflict(Appointment newAppointment)
    {
        return appointments.Any(existing =>
            existing.Status != "Cancelled" &&
            existing.Patient.Id == newAppointment.Patient.Id &&
            Overlaps(existing, newAppointment));
    }

    private static bool Overlaps(Appointment first, Appointment second)
    {
        return second.StartTime < first.EndTime &&
               second.EndTime > first.StartTime;
    }

    public IReadOnlyList<Appointment> GetAppointments()
    {
        return appointments.AsReadOnly();
    }
}
