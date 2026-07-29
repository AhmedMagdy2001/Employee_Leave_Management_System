using Employee_Leave_Management_System.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagement.Models;

public class LeaveRequest
{
    public int Id { get; set; }

    [Required]
    public int EmployeeId { get; set; }

    public Employee? Employee { get; set; }

    [Required]
    public int LeaveTypeId { get; set; }

    public LeaveType? LeaveType { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateOnly StartDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateOnly EndDate { get; set; }

    [Required]
    [StringLength(500)]
    public string Reason { get; set; } = string.Empty;

    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
}