using EmployeeLeaveManagement.Data;
using EmployeeLeaveManagement.Models;
using EmployeeLeaveManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagement.Controllers;

public class LeaveRequestsController : Controller
{
    private readonly AppDbContext _context;

    public LeaveRequestsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var requests = await _context.LeaveRequests
            .Include(l => l.Employee)
            .Include(l => l.LeaveType)
            .OrderByDescending(l => l.StartDate)
            .ToListAsync();

        return View(requests);
    }

    public async Task<IActionResult> Create()
    {
        var model = new LeaveRequestViewModel
        {
            Employees = await _context.Employees
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.EmployeeName
                })
                .ToListAsync(),

            LeaveTypes = await _context.LeaveTypes
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.LeaveTypeName
                })
                .ToListAsync(),

            StartDate = DateOnly.FromDateTime(DateTime.Today),
            EndDate = DateOnly.FromDateTime(DateTime.Today)
        };

        return View(model);
    }

}