using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IntialCreateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CBSAP");

            migrationBuilder.CreateTable(
                name: "Menu",
                schema: "CBSAP",
                columns: table => new
                {
                    MenuID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RouterLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.MenuID);
                });

            migrationBuilder.CreateTable(
                name: "Operation",
                schema: "CBSAP",
                columns: table => new
                {
                    OperationID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Panel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApplyOperationIn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operation", x => x.OperationID);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "CBSAP",
                columns: table => new
                {
                    PermissionID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.PermissionID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "CBSAP",
                columns: table => new
                {
                    RoleID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AuthorisationLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LineManager1 = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: true),
                    LineManager2 = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: true),
                    CanBeAddedToInvoice = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "UserAccount",
                schema: "CBSAP",
                columns: table => new
                {
                    UserAccountID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    LastName = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    EmailAddress = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsUserPartialDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccount", x => x.UserAccountID);
                    table.UniqueConstraint("AK_UserAccount_UserID", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "MenuItem",
                schema: "CBSAP",
                columns: table => new
                {
                    MenuItemID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuID = table.Column<long>(type: "bigint", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RouterLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItem", x => x.MenuItemID);
                    table.ForeignKey(
                        name: "FK_MenuItem_Menu_MenuID",
                        column: x => x.MenuID,
                        principalSchema: "CBSAP",
                        principalTable: "Menu",
                        principalColumn: "MenuID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ControlElement",
                schema: "CBSAP",
                columns: table => new
                {
                    ControlID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ActionType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OperationID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlElement", x => x.ControlID);
                    table.ForeignKey(
                        name: "FK_ControlElement_Operation_OperationID",
                        column: x => x.OperationID,
                        principalSchema: "CBSAP",
                        principalTable: "Operation",
                        principalColumn: "OperationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionGroup",
                schema: "CBSAP",
                columns: table => new
                {
                    PermissionGroupID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionID = table.Column<long>(type: "bigint", nullable: false),
                    OperationID = table.Column<long>(type: "bigint", nullable: false),
                    Access = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroup", x => x.PermissionGroupID);
                    table.UniqueConstraint("AK_PermissionGroup_PermissionID_OperationID", x => new { x.PermissionID, x.OperationID });
                    table.ForeignKey(
                        name: "FK_PermissionGroup_Operation_OperationID",
                        column: x => x.OperationID,
                        principalSchema: "CBSAP",
                        principalTable: "Operation",
                        principalColumn: "OperationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionGroup_Permission_PermissionID",
                        column: x => x.PermissionID,
                        principalSchema: "CBSAP",
                        principalTable: "Permission",
                        principalColumn: "PermissionID");
                });

            migrationBuilder.CreateTable(
                name: "RoleReminderNotification",
                schema: "CBSAP",
                columns: table => new
                {
                    RoleReminderNotificationID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsNewInvoiceReceiveNotification = table.Column<bool>(type: "bit", nullable: false),
                    InvoiceDueDateNotification = table.Column<int>(type: "int", nullable: true),
                    InvoiceEscalateToLevel1ManagerNotification = table.Column<int>(type: "int", nullable: true),
                    ForwardToLevel1Manager = table.Column<int>(type: "int", nullable: true),
                    ForwardToLevel2Manager = table.Column<int>(type: "int", nullable: true),
                    RoleID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleReminderNotification", x => x.RoleReminderNotificationID);
                    table.UniqueConstraint("AK_RoleReminderNotification_RoleID", x => x.RoleID);
                    table.ForeignKey(
                        name: "FK_RoleReminderNotification_Role_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "CBSAP",
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogInfo",
                schema: "CBSAP",
                columns: table => new
                {
                    UserLogInfoID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PasswordHash = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: true),
                    PasswordSalt = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: true),
                    ConfirmationToken = table.Column<string>(type: "NVARCHAR(1000)", maxLength: 1000, nullable: true),
                    TokenGenerationTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PasswordrecoveryToken = table.Column<string>(type: "NVARCHAR(1000)", maxLength: 1000, nullable: true),
                    RecoveryTokenTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    MaximumLogInAttemp = table.Column<int>(type: "int", nullable: true),
                    MinimumPasswordAge = table.Column<int>(type: "int", nullable: true),
                    MaximuPasswordAge = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserAccountID = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogInfo", x => x.UserLogInfoID);
                    table.UniqueConstraint("AK_UserLogInfo_UserID", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_UserLogInfo_UserAccount_UserAccountID",
                        column: x => x.UserAccountID,
                        principalSchema: "CBSAP",
                        principalTable: "UserAccount",
                        principalColumn: "UserAccountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "CBSAP",
                columns: table => new
                {
                    UserRoleID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAccountID = table.Column<long>(type: "bigint", nullable: false),
                    RoleID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.UserRoleID);
                    table.UniqueConstraint("AK_UserRole_UserAccountID_RoleID", x => new { x.UserAccountID, x.RoleID });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "CBSAP",
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_UserAccount_UserAccountID",
                        column: x => x.UserAccountID,
                        principalSchema: "CBSAP",
                        principalTable: "UserAccount",
                        principalColumn: "UserAccountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissionGroup",
                schema: "CBSAP",
                columns: table => new
                {
                    RolePermissionGroupID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionID = table.Column<long>(type: "bigint", nullable: false),
                    RoleID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissionGroup", x => x.RolePermissionGroupID);
                    table.UniqueConstraint("AK_RolePermissionGroup_RoleID_PermissionID", x => new { x.RoleID, x.PermissionID });
                    table.ForeignKey(
                        name: "FK_RolePermissionGroup_PermissionGroup_PermissionID",
                        column: x => x.PermissionID,
                        principalSchema: "CBSAP",
                        principalTable: "PermissionGroup",
                        principalColumn: "PermissionGroupID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissionGroup_Role_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "CBSAP",
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ControlElement_OperationID",
                schema: "CBSAP",
                table: "ControlElement",
                column: "OperationID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_MenuID",
                schema: "CBSAP",
                table: "MenuItem",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroup_OperationID",
                schema: "CBSAP",
                table: "PermissionGroup",
                column: "OperationID");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionGroup_PermissionID",
                schema: "CBSAP",
                table: "RolePermissionGroup",
                column: "PermissionID");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogInfo_UserAccountID",
                schema: "CBSAP",
                table: "UserLogInfo",
                column: "UserAccountID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleID",
                schema: "CBSAP",
                table: "UserRole",
                column: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ControlElement",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "MenuItem",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "RolePermissionGroup",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "RoleReminderNotification",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "UserLogInfo",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "Menu",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "PermissionGroup",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "UserAccount",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "Operation",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "Permission",
                schema: "CBSAP");
        }
    }
}
