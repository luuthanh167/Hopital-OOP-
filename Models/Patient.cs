public class Patient
{
    private string _medicalRecord;

    public string Id { get; }
    public string Name { get; private set; }
    public string Phone { get; private set; }

    public Patient(string id, string name, string phone)
    {
        ValidationHelper.RequireNotEmpty(id, "Patient ID");
        ValidationHelper.RequireNotEmpty(name, "Patient name");
        ValidationHelper.RequireNotEmpty(phone, "Phone");

        Id = id;
        Name = name;
        Phone = phone;
        _medicalRecord = "";
    }

    

    public string GetMedicalRecord()
    {
        return _medicalRecord;
    }

    public void UpdateMedicalRecord(string record)
    {
        ValidationHelper.RequireNotEmpty(record, "Medical record");
        _medicalRecord = record;
    }
}