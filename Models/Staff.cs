public abstract class Staff
{
    public string Id { get; }
    public string Name { get; private set; }

    protected Staff(string id, string name)
    {
        ValidationHelper.RequireNotEmpty(id, "Staff ID");
        ValidationHelper.RequireNotEmpty(name, "Staff name");

        Id = id;
        Name = name;
    }

    public abstract void PerformDuty();
}
public class Doctor : Staff
{
    public string Specialty { get; }

    public Doctor(string id, string name, string specialty)
        : base(id, name)
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

    public Nurse(string id, string name, string department)
        : base(id, name)
    {
        ValidationHelper.RequireNotEmpty(department, "Department");

        Department = department;
    }

    public override void PerformDuty()
    {
        Console.WriteLine($"Nurse {Name} assists patients.");
    }
}