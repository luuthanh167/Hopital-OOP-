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
        ValidationHelper.RequireNotEmpty(id, nameof(id));
        ArgumentNullException.ThrowIfNull(patient);
        ArgumentNullException.ThrowIfNull(doctor);
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

    public string GetSummary() =>
        $"Prescription {Id}: {MedicationName}; Dosage: {Dosage}; Instructions: {Instructions}";

    public void DisplayPrescription()
    {
        Console.WriteLine($"Prescription ID: {Id}");
        Console.WriteLine($"Patient: {Patient.Name}");
        Console.WriteLine($"Doctor: {Doctor.Name}");
        Console.WriteLine($"Medication: {MedicationName}");
        Console.WriteLine($"Dosage: {Dosage}");
        Console.WriteLine($"Instructions: {Instructions}");
        Console.WriteLine($"Prescribed Date: {PrescribedDate:yyyy-MM-dd HH:mm}");
    }
}