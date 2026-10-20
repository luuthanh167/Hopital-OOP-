using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class MedicalHistory
{
    public string ID {get; }
    public Patient Patient {get; }

    private readonly List<Appointment> completedAppointments = new List<Appointment>();

    public MedicalHistory(string id, Patient patient)
    {
        ValidationHelper.RequireNotEmpty(id, "MedicalHistory ID");

        if(patient == null)
        {
            throw new ArgumentNullException(nameof(patient), "Patient cannot be left blank.");
        }

        ID = id;
        Patient = patient;
    }

    public void AddVisitRecord(Appointment appointment, string outcome)
    {
        if (appointment == null)
        {
            throw new ArgumentNullException(nameof(appointment), "Appointment cannot be left blank.");
        }

        if (appointment.Patient.Id != Patient.Id)
        {
            throw new InvalidOperationException("Appointment does not belong to this patient.");
        }

        ValidationHelper.RequireNotEmpty(outcome, "Visit outcome");

        appointment.RecordVisitOutcome(outcome);
        completedAppointments.Add(appointment);

        string recordEntry = $"[{appointment.StartTime:yyyy-MM-dd HH:mm}] Doctor: {appointment.Doctor.Name} | Outcome: {outcome}";
        
        string updatedRecord = string.IsNullOrEmpty(Patient.GetMedicalRecord())
            ? recordEntry: $"{Patient.GetMedicalRecord()}\n{recordEntry}";

        Patient.UpdateMedicalRecord(updatedRecord);
    }   

    public IReadOnlyList<Appointment> GetCompletedAppointments()
    {
        return completedAppointments.AsReadOnly();
    }
}