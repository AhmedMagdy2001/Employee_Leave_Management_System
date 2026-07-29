using EmployeeLeaveManagement.Models;

namespace EmployeeLeaveManagement.Helpers;

public static class StatusExtensions
{
    public static string BadgeClass(this LeaveStatus status)
    {
        return status switch
        {
            LeaveStatus.Pending => "bg-warning text-dark",
            LeaveStatus.Approved => "bg-success",
            LeaveStatus.Rejected => "bg-danger",
            _ => "bg-secondary"
        };
    }
}