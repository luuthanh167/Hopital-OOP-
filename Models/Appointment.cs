public class Appointment
{
    public Patient Patient { get; }
    public Doctor Doctor { get; }
    public DateTime StartTime { get; }
    public string? VisitOutcome { get; private set; }

    public Appointment(
        Patient patient,
        Doctor doctor,
        DateTime startTime)
    {
        if (patient == null){
            throw new ArgumentNullException(nameof(patient));
        }
        if (doctor == null){
            throw new ArgumentNullException(nameof(doctor));
        }
        ValidationHelper.RequireFutureDate(startTime);

        Patient = patient;
        Doctor = doctor;
        StartTime = startTime;
        VisitOutcome = null;
    }

    public void RecordVisitOutcome(string outcome)
    {
        ValidationHelper.RequireNotEmpty(outcome, "Visit outcome");

        VisitOutcome = outcome;
    }
}