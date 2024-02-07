using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NTR.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeekStartDate",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "WorkDayEnd",
                table: "Schedule");

            migrationBuilder.RenameColumn(
                name: "WorkDayStart",
                table: "Schedule",
                newName: "Date");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Schedule",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatientId",
                table: "Schedule",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Schedule");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Schedule",
                newName: "WorkDayStart");

            migrationBuilder.AddColumn<DateTime>(
                name: "WeekStartDate",
                table: "Schedule",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkDayEnd",
                table: "Schedule",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
