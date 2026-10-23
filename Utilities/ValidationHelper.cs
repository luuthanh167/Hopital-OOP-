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

    public static string NormalizeAppointmentStatus(string status)
    {
        RequireNotEmpty(status, nameof(status));

        string normalizedStatus = status.Trim();

        if (string.Equals(normalizedStatus, "Scheduled", StringComparison.OrdinalIgnoreCase))
            return "Scheduled";

        if (string.Equals(normalizedStatus, "Completed", StringComparison.OrdinalIgnoreCase))
            return "Completed";

        if (string.Equals(normalizedStatus, "Cancelled", StringComparison.OrdinalIgnoreCase))
            return "Cancelled";

        throw new ArgumentException($"Unknown appointment status '{status}'.", nameof(status));
    }

    public static bool IsValidStatusTransition(string currentStatus, string newStatus)
    {
        if (string.IsNullOrWhiteSpace(currentStatus) || string.IsNullOrWhiteSpace(newStatus))
            return false;

        string current = currentStatus.Trim();
        string next = newStatus.Trim();

        return string.Equals(current, "Scheduled", StringComparison.OrdinalIgnoreCase)
            && (string.Equals(next, "Completed", StringComparison.OrdinalIgnoreCase)
                || string.Equals(next, "Cancelled", StringComparison.OrdinalIgnoreCase));
    }
}
