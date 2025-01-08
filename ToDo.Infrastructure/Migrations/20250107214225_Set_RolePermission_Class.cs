using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Set_RolePermission_Class : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_permissions_roles_RoleId",
                table: "permissions");

            migrationBuilder.DropIndex(
                name: "IX_permissions_RoleId",
                table: "permissions");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "permissions");

            migrationBuilder.CreateIndex(
                name: "IX_role_permissions_PermissionId",
                table: "role_permissions",
                column: "PermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_role_permissions_permissions_PermissionId",
                table: "role_permissions",
                column: "PermissionId",
                principalTable: "permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permissions_roles_RoleId",
                table: "role_permissions",
                column: "RoleId",
                principalTable: "roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permissions_permissions_PermissionId",
                table: "role_permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permissions_roles_RoleId",
                table: "role_permissions");

            migrationBuilder.DropIndex(
                name: "IX_role_permissions_PermissionId",
                table: "role_permissions");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "permissions",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "RoleId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_permissions_RoleId",
                table: "permissions",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_permissions_roles_RoleId",
                table: "permissions",
                column: "RoleId",
                principalTable: "roles",
                principalColumn: "Id");
        }
    }
}
