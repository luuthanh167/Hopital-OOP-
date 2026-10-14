public abstract class Staff
{
    public string Id { get; }
    public string Name { get; private set; }
    public string PhoneNumber {get; protected set;}

    protected Staff(string id, string name, string phonenumber)
    {
        ValidationHelper.RequireNotEmpty(id, "Staff ID");
        ValidationHelper.RequireNotEmpty(name, "Staff name");
        ValidationHelper.RequireNotEmpty(phonenumber, "Phone number");

        Id = id;
        Name = name;
        PhoneNumber = phonenumber;
    }

    public abstract void PerformDuty();
}
public class Doctor : Staff
{
    public string Specialty { get; }

    public Doctor(string id, string name, string specialty, string phonenumber)
        : base(id, name, phonenumber)
    {
        ValidationHelper.RequireNotEmpty(specialty, "Specialty");

        Specialty = specialty;
    }

    public override void PerformDuty()
    {
        Console.WriteLine($"Doctor {Name} examines patients.");
    }
}

public class Nurse : Staff
{
    public string Department { get; }

    public Nurse(string id, string name, string department, string phonenumber)
        : base(id, name, phonenumber)
    {
        ValidationHelper.RequireNotEmpty(department, "Department");

        Department = department;
    }

    public override void PerformDuty()
    {
        Console.WriteLine($"Nurse {Name} assists patients.");
    }
}