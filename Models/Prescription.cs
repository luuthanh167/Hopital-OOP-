using System;

public class Prescription
{
    public string Id { get; }
    public Patient Patient { get; }
    public Doctor Doctor { get; }
    public string MedicationName { get; private set; }
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
        ValidationHelper.RequireNotEmpty(id, "Prescription ID");

        if (patient == null)
        {
            throw new ArgumentNullException(nameof(patient));
        }

        if (doctor == null)
        {
            throw new ArgumentNullException(nameof(doctor));
        }

        ValidationHelper.RequireNotEmpty(medicationName, "Medication name");
        ValidationHelper.RequireNotEmpty(dosage, "Dosage");
        ValidationHelper.RequireNotEmpty(instructions, "Instructions");

        Id = id;
        Patient = patient;
        Doctor = doctor;
        MedicationName = medicationName;
        Dosage = dosage;
        Instructions = instructions;
        PrescribedDate = DateTime.Now;
    }

    public void UpdateInstructions(string instructions)
    {
        ValidationHelper.RequireNotEmpty(
            instructions, "Instructions");

        Instructions = instructions;
    }

    public void UpdateDosage(string dosage)
    {
        ValidationHelper.RequireNotEmpty(dosage, "Dosage");

        Dosage = dosage;
    }

    public void DisplayPrescription()
    {
        Console.WriteLine($"Prescription ID: {Id}");
        Console.WriteLine($"Patient: {Patient.Name}");
        Console.WriteLine($"Doctor: {Doctor.Name}");
        Console.WriteLine($"Medication: {MedicationName}");
        Console.WriteLine($"Dosage: {Dosage}");
        Console.WriteLine($"Instructions: {Instructions}");
        Console.WriteLine($"Prescribed Date: {PrescribedDate}");
    }
}
