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

    private async Task PopulateDropdowns(LeaveRequestViewModel model)
    {
        model.Employees = await _context.Employees
            .Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.EmployeeName
            })
            .ToListAsync();

        model.LeaveTypes = await _context.LeaveTypes
            .Select(l => new SelectListItem
            {
                Value = l.Id.ToString(),
                Text = l.LeaveTypeName
            })
            .ToListAsync();
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
            StartDate = DateOnly.FromDateTime(DateTime.Today),
            EndDate = DateOnly.FromDateTime(DateTime.Today)
        };

        await PopulateDropdowns(model);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, LeaveRequestViewModel model)
    {
        if (id != model.Id)
            return NotFound();

        if (!ModelState.IsValid)
        {
            await PopulateDropdowns(model);
            return View(model);
        }

        var leaveRequest = await _context.LeaveRequests.FindAsync(id);

        if (leaveRequest == null)
            return NotFound();

        if (leaveRequest.Status != LeaveStatus.Pending)
        {
            TempData["Error"] = "Only pending requests can be edited.";
            return RedirectToAction(nameof(Index));
        }

        if (!await ValidateLeaveRequest(model))
        {
            await PopulateDropdowns(model);
            return View(model);
        }

        leaveRequest.EmployeeId = model.EmployeeId;
        leaveRequest.LeaveTypeId = model.LeaveTypeId;
        leaveRequest.StartDate = model.StartDate;
        leaveRequest.EndDate = model.EndDate;
        leaveRequest.Reason = model.Reason;

        await _context.SaveChangesAsync();

        TempData["Success"] = "Leave request updated successfully.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LeaveRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdowns(model);
            return View(model);
        }

        if (!await ValidateLeaveRequest(model))
        {
            await PopulateDropdowns(model);
            return View(model);
        }

        var leaveRequest = new LeaveRequest
        {
            EmployeeId = model.EmployeeId,
            LeaveTypeId = model.LeaveTypeId,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Reason = model.Reason,
            Status = LeaveStatus.Pending
        };

        _context.LeaveRequests.Add(leaveRequest);

        await _context.SaveChangesAsync();

        TempData["Success"] = "Leave request submitted successfully.";

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var leaveRequest = await _context.LeaveRequests
            .Include(l => l.Employee)
            .Include(l => l.LeaveType)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (leaveRequest == null)
        {
            return NotFound();
        }

        return View(leaveRequest);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var leaveRequest = await _context.LeaveRequests.FindAsync(id);

        if (leaveRequest == null)
            return NotFound();

        if (leaveRequest.Status != LeaveStatus.Pending)
        {
            TempData["Error"] = "Only pending requests can be edited.";
            return RedirectToAction(nameof(Index));
        }

        var model = new LeaveRequestViewModel
        {
            Id = leaveRequest.Id,
            EmployeeId = leaveRequest.EmployeeId,
            LeaveTypeId = leaveRequest.LeaveTypeId,
            StartDate = leaveRequest.StartDate,
            EndDate = leaveRequest.EndDate,
            Reason = leaveRequest.Reason,
            Status = leaveRequest.Status
        };

        await PopulateDropdowns(model);

        return View(model);
    }

    private async Task<bool> ValidateLeaveRequest(LeaveRequestViewModel model)
    {
        if (model.StartDate < DateOnly.FromDateTime(DateTime.Today))
        {
            ModelState.AddModelError(nameof(model.StartDate),
                "Start date cannot be earlier than today.");
        }

        if (model.EndDate < model.StartDate)
        {
            ModelState.AddModelError(nameof(model.EndDate),
                "End date must be after or equal to start date.");
        }

        var leaveType = await _context.LeaveTypes
            .FirstOrDefaultAsync(l => l.Id == model.LeaveTypeId);

        if (leaveType != null)
        {
            int requestedDays =
                model.EndDate.DayNumber -
                model.StartDate.DayNumber + 1;

            if (requestedDays > leaveType.MaximumDaysAllowed)
            {
                ModelState.AddModelError("",
                    $"Maximum allowed days for {leaveType.LeaveTypeName} is {leaveType.MaximumDaysAllowed}.");
            }
        }

        bool overlap = await _context.LeaveRequests.AnyAsync(l =>
            l.Id != model.Id &&
            l.EmployeeId == model.EmployeeId &&
            l.Status != LeaveStatus.Rejected &&
            model.StartDate <= l.EndDate &&
            model.EndDate >= l.StartDate);

        if (overlap)
        {
            ModelState.AddModelError("",
                "The selected leave dates overlap with another leave request.");
        }

        return ModelState.IsValid;
    }

    public async Task<IActionResult> Approve(int id)
    {
        var leaveRequest = await _context.LeaveRequests.FindAsync(id);

        if (leaveRequest == null)
        {
            return NotFound();
        }

        if (leaveRequest.Status != LeaveStatus.Pending)
        {
            TempData["Error"] = "Only pending requests can be approved.";
            return RedirectToAction(nameof(Index));
        }

        leaveRequest.Status = LeaveStatus.Approved;

        await _context.SaveChangesAsync();

        TempData["Success"] = "Leave request approved successfully.";

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Reject(int id)
    {
        var leaveRequest = await _context.LeaveRequests.FindAsync(id);

        if (leaveRequest == null)
        {
            return NotFound();
        }

        if (leaveRequest.Status != LeaveStatus.Pending)
        {
            TempData["Error"] = "Only pending requests can be rejected.";
            return RedirectToAction(nameof(Index));
        }

        leaveRequest.Status = LeaveStatus.Rejected;

        await _context.SaveChangesAsync();

        TempData["Success"] = "Leave request rejected successfully.";

        return RedirectToAction(nameof(Index));
    }

}