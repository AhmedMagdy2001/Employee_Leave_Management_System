namespace EmployeeLeaveManagement.ViewModels;

public class DashboardViewModel
{
    public int TotalEmployees { get; set; }

    public int TotalLeaveRequests { get; set; }

    public int PendingRequests { get; set; }

    public int ApprovedRequests { get; set; }

    public int RejectedRequests { get; set; }
}