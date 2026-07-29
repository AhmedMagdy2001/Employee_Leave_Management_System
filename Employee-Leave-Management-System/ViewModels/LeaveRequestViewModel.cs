using System.ComponentModel.DataAnnotations;
using EmployeeLeaveManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeLeaveManagement.ViewModels;

public class LeaveRequestViewModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Employee")]
    public int EmployeeId { get; set; }

    [Required]
    [Display(Name = "Leave Type")]
    public int LeaveTypeId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateOnly StartDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateOnly EndDate { get; set; }

    [Required]
    [StringLength(500)]
    public string Reason { get; set; } = string.Empty;

    public LeaveStatus Status { get; set; }

    public IEnumerable<SelectListItem> Employees { get; set; }
        = Enumerable.Empty<SelectListItem>();

    public IEnumerable<SelectListItem> LeaveTypes { get; set; }
        = Enumerable.Empty<SelectListItem>();
}