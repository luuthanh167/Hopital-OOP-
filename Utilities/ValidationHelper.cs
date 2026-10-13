using System;

public static class ValidationHelper
{
    public static void RequireNotEmpty(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"Value for {fieldName} cannot be null or whitespace.");
        }
    }

    public static void RequireFutureDate(DateTime appointmentDate, string fieldName = "Appointment date")
    {
        if (appointmentDate <= DateTime.Now)
        {
            throw new ArgumentException($"{fieldName} must be a future date.");
        }
    }
    

    public static bool IsValidStatusTransition(string currentStatus, string newStatus)
    {
        if(currentStatus == "Scheduled")
        {
            if(newStatus == "Completed" || newStatus == "Cancelled")
            {
                return true;
            }
        }
        else if(currentStatus == "Completed" || currentStatus == "Cancelled")
        {
            return false;
        }
        return false;
    }
}
