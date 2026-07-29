using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Employee_Leave_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class AddLeaveTypeUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LeaveTypes_LeaveTypeName",
                table: "LeaveTypes",
                column: "LeaveTypeName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LeaveTypes_LeaveTypeName",
                table: "LeaveTypes");
        }
    }
}
