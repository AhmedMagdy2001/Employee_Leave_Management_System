using EmployeeLeaveManagement.Data;
using EmployeeLeaveManagement.Models;
using EmployeeLeaveManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagement.Controllers;

public class DashboardController : Controller
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var model = new DashboardViewModel
        {
            TotalEmployees = await _context.Employees.CountAsync(),
            TotalLeaveRequests = await _context.LeaveRequests.CountAsync(),
            PendingRequests = await _context.LeaveRequests.CountAsync(l => l.Status == LeaveStatus.Pending),
            ApprovedRequests = await _context.LeaveRequests.CountAsync(l => l.Status == LeaveStatus.Approved),
            RejectedRequests = await _context.LeaveRequests.CountAsync(l => l.Status == LeaveStatus.Rejected)
        };

        return View(model);
    }
}