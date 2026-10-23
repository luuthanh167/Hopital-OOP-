public class Patient
{
    private string _medicalRecord = string.Empty;

    public string Id { get; }
    public string Name { get; private set; }
    public string Phone { get; private set; }

    public Patient(string id, string name, string phone)
    {
        ValidationHelper.RequireNotEmpty(id, nameof(id));
        ValidationHelper.RequireNotEmpty(name, nameof(name));
        ValidationHelper.RequireNotEmpty(phone, nameof(phone));

        Id = id.Trim();
        Name = name.Trim();
        Phone = phone.Trim();
    }

    public void UpdateName(string name)
    {
        ValidationHelper.RequireNotEmpty(name, nameof(name));
        Name = name.Trim();
    }

    public void UpdatePhone(string phone)
    {
        ValidationHelper.RequireNotEmpty(phone, nameof(phone));
        Phone = phone.Trim();
    }

    public string GetMedicalRecord()
    {
        return _medicalRecord;
    }

    public void UpdateMedicalRecord(string record)
    {
        ValidationHelper.RequireNotEmpty(record, nameof(record));
        _medicalRecord = record.Trim();
    }
}