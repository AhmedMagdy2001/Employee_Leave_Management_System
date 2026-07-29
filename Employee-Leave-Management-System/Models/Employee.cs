
using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagement.Models;

public class Employee
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string EmployeeName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Department { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    public string? PhoneNumber { get; set; }

    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
}
