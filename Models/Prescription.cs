using System;

public class Prescription
{
    public string Id { get; }
    public Patient Patient { get; }
    public Doctor Doctor { get; }
    public string MedicationName { get; }
    public string Dosage { get; private set; }
    public string Instructions { get; private set; }
    public DateTime PrescribedDate { get; }

    public Prescription(
        string id,
        Patient patient,
        Doctor doctor,
        string medicationName,
        string dosage,
        string instructions)
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

        ValidationHelper.RequireNotEmpty(medicationName, nameof(medicationName));
        ValidationHelper.RequireNotEmpty(dosage, nameof(dosage));
        ValidationHelper.RequireNotEmpty(instructions, nameof(instructions));

        Id = id.Trim();
        Patient = patient;
        Doctor = doctor;
        MedicationName = medicationName.Trim();
        Dosage = dosage.Trim();
        Instructions = instructions.Trim();
        PrescribedDate = DateTime.Now;
    }

    public void UpdateInstructions(string instructions)
    {
        ValidationHelper.RequireNotEmpty(instructions, nameof(instructions));
        Instructions = instructions.Trim();
    }

    public void UpdateDosage(string dosage)
    {
        ValidationHelper.RequireNotEmpty(dosage, nameof(dosage));
        Dosage = dosage.Trim();
    }

    public string GetSummary()
    {
        return $"Prescription ID: {Id}{Environment.NewLine}" +
               $"Patient: {Patient.Name}{Environment.NewLine}" +
               $"Doctor: {Doctor.Name}{Environment.NewLine}" +
               $"Medication: {MedicationName}{Environment.NewLine}" +
               $"Dosage: {Dosage}{Environment.NewLine}" +
               $"Instructions: {Instructions}{Environment.NewLine}" +
               $"Prescribed Date: {PrescribedDate:yyyy-MM-dd HH:mm}";
    }

    public void DisplayPrescription()
    {
        Console.WriteLine(GetSummary());
    }
}