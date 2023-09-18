using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriverAPI.Migrations
{
    /// <inheritdoc />
    public partial class changeTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_drivers",
                table: "drivers");

            migrationBuilder.RenameTable(
                name: "drivers",
                newName: "driver");

            migrationBuilder.AddPrimaryKey(
                name: "PK_driver",
                table: "driver",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_driver",
                table: "driver");

            migrationBuilder.RenameTable(
                name: "driver",
                newName: "drivers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_drivers",
                table: "drivers",
                column: "Id");
        }
    }
}
