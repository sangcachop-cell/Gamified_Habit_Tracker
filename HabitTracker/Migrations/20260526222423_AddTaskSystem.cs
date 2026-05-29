using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastCronDate",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HabitTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Up = table.Column<bool>(type: "bit", nullable: true),
                    Down = table.Column<bool>(type: "bit", nullable: true),
                    CounterUp = table.Column<int>(type: "int", nullable: false),
                    CounterDown = table.Column<int>(type: "int", nullable: false),
                    HabitFrequency = table.Column<int>(type: "int", nullable: true),
                    DailyFrequency = table.Column<int>(type: "int", nullable: true),
                    EveryX = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RepeatMonday = table.Column<bool>(type: "bit", nullable: false),
                    RepeatTuesday = table.Column<bool>(type: "bit", nullable: false),
                    RepeatWednesday = table.Column<bool>(type: "bit", nullable: false),
                    RepeatThursday = table.Column<bool>(type: "bit", nullable: false),
                    RepeatFriday = table.Column<bool>(type: "bit", nullable: false),
                    RepeatSaturday = table.Column<bool>(type: "bit", nullable: false),
                    RepeatSunday = table.Column<bool>(type: "bit", nullable: false),
                    Streak = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GoldCost = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HabitTasks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTaskTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTaskTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTaskTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskChecklistItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HabitTaskId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskChecklistItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskChecklistItems_HabitTasks_HabitTaskId",
                        column: x => x.HabitTaskId,
                        principalTable: "HabitTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskTagAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HabitTaskId = table.Column<int>(type: "int", nullable: false),
                    UserTaskTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTagAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskTagAssignments_HabitTasks_HabitTaskId",
                        column: x => x.HabitTaskId,
                        principalTable: "HabitTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskTagAssignments_UserTaskTags_UserTaskTagId",
                        column: x => x.UserTaskTagId,
                        principalTable: "UserTaskTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HabitTasks_UserId_Type",
                table: "HabitTasks",
                columns: new[] { "UserId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskChecklistItems_HabitTaskId",
                table: "TaskChecklistItems",
                column: "HabitTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTagAssignments_HabitTaskId_UserTaskTagId",
                table: "TaskTagAssignments",
                columns: new[] { "HabitTaskId", "UserTaskTagId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskTagAssignments_UserTaskTagId",
                table: "TaskTagAssignments",
                column: "UserTaskTagId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTaskTags_UserId",
                table: "UserTaskTags",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskChecklistItems");

            migrationBuilder.DropTable(
                name: "TaskTagAssignments");

            migrationBuilder.DropTable(
                name: "HabitTasks");

            migrationBuilder.DropTable(
                name: "UserTaskTags");

            migrationBuilder.DropColumn(
                name: "LastCronDate",
                table: "Users");
        }
    }
}
