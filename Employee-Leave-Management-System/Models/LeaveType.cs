using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagement.Models;

public class LeaveType
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string LeaveTypeName { get; set; } = string.Empty;

    [Range(1, 365)]
    public int MaximumDaysAllowed { get; set; }

    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
}