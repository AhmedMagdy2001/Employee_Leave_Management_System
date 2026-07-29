using EmployeeLeaveManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagement.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<LeaveType> LeaveTypes => Set<LeaveType>();

    public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.EmployeeCode)
            .IsUnique();

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Email)
            .IsUnique();

        modelBuilder.Entity<LeaveType>()
            .HasIndex(l => l.LeaveTypeName)
            .IsUnique();

        modelBuilder.Entity<LeaveType>().HasData(
            new LeaveType
            {
                Id = 1,
                LeaveTypeName = "Annual Leave",
                MaximumDaysAllowed = 30
            },
            new LeaveType
            {
                Id = 2,
                LeaveTypeName = "Sick Leave",
                MaximumDaysAllowed = 15
            },
            new LeaveType
            {
                Id = 3,
                LeaveTypeName = "Casual Leave",
                MaximumDaysAllowed = 7
            });
            }
}