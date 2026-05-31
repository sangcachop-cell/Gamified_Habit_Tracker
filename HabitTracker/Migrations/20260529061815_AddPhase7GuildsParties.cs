using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddPhase7GuildsParties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Privacy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LeaderId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guilds_Users_LeaderId",
                        column: x => x.LeaderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Parties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LeaderId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parties_Users_LeaderId",
                        column: x => x.LeaderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GuildInvites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuildId = table.Column<int>(type: "int", nullable: false),
                    InviterId = table.Column<int>(type: "int", nullable: false),
                    InviteeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildInvites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildInvites_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildInvites_Users_InviteeId",
                        column: x => x.InviteeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GuildInvites_Users_InviterId",
                        column: x => x.InviterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GuildMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuildId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildMembers_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GuildMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuildId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildMessages_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildMessages_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartyInvites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartyId = table.Column<int>(type: "int", nullable: false),
                    InviterId = table.Column<int>(type: "int", nullable: false),
                    InviteeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyInvites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyInvites_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyInvites_Users_InviteeId",
                        column: x => x.InviteeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartyInvites_Users_InviterId",
                        column: x => x.InviterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartyMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartyId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyMembers_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartyMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartyId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyMessages_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyMessages_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GuildMessageLikes",
                columns: table => new
                {
                    GuildMessageId = table.Column<int>(type: "int", nullable: false),
                    LikerUserId = table.Column<int>(type: "int", nullable: false),
                    LikedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildMessageLikes", x => new { x.GuildMessageId, x.LikerUserId });
                    table.ForeignKey(
                        name: "FK_GuildMessageLikes_GuildMessages_GuildMessageId",
                        column: x => x.GuildMessageId,
                        principalTable: "GuildMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildMessageLikes_Users_LikerUserId",
                        column: x => x.LikerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartyMessageLikes",
                columns: table => new
                {
                    PartyMessageId = table.Column<int>(type: "int", nullable: false),
                    LikerUserId = table.Column<int>(type: "int", nullable: false),
                    LikedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyMessageLikes", x => new { x.PartyMessageId, x.LikerUserId });
                    table.ForeignKey(
                        name: "FK_PartyMessageLikes_PartyMessages_PartyMessageId",
                        column: x => x.PartyMessageId,
                        principalTable: "PartyMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyMessageLikes_Users_LikerUserId",
                        column: x => x.LikerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildInvites_GuildId_InviteeId",
                table: "GuildInvites",
                columns: new[] { "GuildId", "InviteeId" });

            migrationBuilder.CreateIndex(
                name: "IX_GuildInvites_InviteeId",
                table: "GuildInvites",
                column: "InviteeId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildInvites_InviterId",
                table: "GuildInvites",
                column: "InviterId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildMembers_GuildId_UserId",
                table: "GuildMembers",
                columns: new[] { "GuildId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuildMembers_UserId",
                table: "GuildMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildMessageLikes_LikerUserId",
                table: "GuildMessageLikes",
                column: "LikerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildMessages_AuthorId",
                table: "GuildMessages",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildMessages_GuildId_SentAt",
                table: "GuildMessages",
                columns: new[] { "GuildId", "SentAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_LeaderId",
                table: "Guilds",
                column: "LeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_Name",
                table: "Guilds",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parties_LeaderId",
                table: "Parties",
                column: "LeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyInvites_InviteeId",
                table: "PartyInvites",
                column: "InviteeId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyInvites_InviterId",
                table: "PartyInvites",
                column: "InviterId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyInvites_PartyId_InviteeId",
                table: "PartyInvites",
                columns: new[] { "PartyId", "InviteeId" });

            migrationBuilder.CreateIndex(
                name: "IX_PartyMembers_PartyId_UserId",
                table: "PartyMembers",
                columns: new[] { "PartyId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyMembers_UserId",
                table: "PartyMembers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyMessageLikes_LikerUserId",
                table: "PartyMessageLikes",
                column: "LikerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyMessages_AuthorId",
                table: "PartyMessages",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyMessages_PartyId_SentAt",
                table: "PartyMessages",
                columns: new[] { "PartyId", "SentAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuildInvites");

            migrationBuilder.DropTable(
                name: "GuildMembers");

            migrationBuilder.DropTable(
                name: "GuildMessageLikes");

            migrationBuilder.DropTable(
                name: "PartyInvites");

            migrationBuilder.DropTable(
                name: "PartyMembers");

            migrationBuilder.DropTable(
                name: "PartyMessageLikes");

            migrationBuilder.DropTable(
                name: "GuildMessages");

            migrationBuilder.DropTable(
                name: "PartyMessages");

            migrationBuilder.DropTable(
                name: "Guilds");

            migrationBuilder.DropTable(
                name: "Parties");
        }
    }
}
