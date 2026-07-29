using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagement.Models;

public class LeaveType
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "Leave Type")]
    public string LeaveTypeName { get; set; } = string.Empty;

    [Range(1, 365)]
    [Display(Name = "Maximum Days")]
    public int MaximumDaysAllowed { get; set; }

    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
}