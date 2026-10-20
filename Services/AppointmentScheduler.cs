using System;
using System.Collections.Generic;
using System.Linq;

public class AppointmentScheduler
{
    private readonly List<Appointment> appointments = new List<Appointment>();

    public bool ScheduleAppointment(Appointment newAppointment)
    {
        if(newAppointment == null)
        {
            throw new ArgumentNullException(nameof(newAppointment),"The appointment cannot be left blank");
        }
        ValidationHelper.RequireNotEmpty(newAppointment.Id, "Appointment ID");
        ValidationHelper.RequireFutureDate(newAppointment.StartTime, "Appointment Time ");

        if (HasDoctorConflict(newAppointment))
        {
            throw new InvalidOperationException($"Doctor: {newAppointment.Doctor.Name} has been conflict");
        }

        appointments.Add(newAppointment);
        return true;
    }

    public bool UpdateAppointmentStatus(string appointmentId, string newStatus)
    {
        
        ValidationHelper.RequireNotEmpty(appointmentId, "Appointment ID");
        ValidationHelper.RequireNotEmpty(newStatus, "New Status");

        var appointment = appointments.FirstOrDefault(a => a.Id == appointmentId);
        if(appointment == null) return false;

        bool isValidTransition = ValidationHelper.IsValidStatusTransition(appointment.Status, newStatus);
        if (!isValidTransition)
        {
            return false;
        }

        appointment.Status = newStatus;
        return true;

    }

    private bool HasDoctorConflict(Appointment newApp)
    {
        return appointments.Any((existingApp =>
            existingApp.Doctor.Id == newApp.Doctor.Id &&
            existingApp.Status != "Cancelled" &&
            newApp.StartTime < existingApp.EndTime &&
            newApp.EndTime > existingApp.StartTime
        ));        

    }
    public IReadOnlyList<Appointment> GetAppointments()
    {
        return appointments.AsReadOnly();
    }
}
