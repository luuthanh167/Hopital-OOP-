using System;

public abstract class Staff
{
    public string Id { get; }
    public string Name { get; private set; }
    public string PhoneNumber { get; private set; }

    protected Staff(string id, string name, string phoneNumber)
    {
        ValidationHelper.RequireNotEmpty(id, nameof(id));
        ValidationHelper.RequireNotEmpty(name, nameof(name));
        ValidationHelper.RequireNotEmpty(phoneNumber, nameof(phoneNumber));

        Id = id.Trim();
        Name = name.Trim();
        PhoneNumber = phoneNumber.Trim();
    }

    public void UpdateName(string name)
    {
        ValidationHelper.RequireNotEmpty(name, nameof(name));
        Name = name.Trim();
    }

    public void UpdatePhoneNumber(string phoneNumber)
    {
        ValidationHelper.RequireNotEmpty(phoneNumber, nameof(phoneNumber));
        PhoneNumber = phoneNumber.Trim();
    }

    public abstract void PerformDuty();
}

public class Doctor : Staff
{
    public string Specialty { get; }

    public Doctor(string id, string name, string specialty, string phoneNumber)
        : base(id, name, phoneNumber)
    {
        ValidationHelper.RequireNotEmpty(specialty, nameof(specialty));
        Specialty = specialty.Trim();
    }

    public override void PerformDuty()
    {
        Console.WriteLine($"Doctor {Name} examines patients.");
    }
}

public class Nurse : Staff
{
    public string Department { get; }

    public Nurse(string id, string name, string department, string phoneNumber)
        : base(id, name, phoneNumber)
    {
        ValidationHelper.RequireNotEmpty(department, nameof(department));
        Department = department.Trim();
    }

    public override void PerformDuty()
    {
        Console.WriteLine($"Nurse {Name} assists patients.");
    }
}