using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC.Newsreel.Data.Migrations
{
    /// <inheritdoc />
    public partial class ContextMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                 name: "AccessFailedCount",
                 table: "AspNetUsers",
                 type: "int",
                 nullable: false,
                 defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
