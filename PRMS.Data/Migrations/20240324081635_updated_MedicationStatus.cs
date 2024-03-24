using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updated_MedicationStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MedicationStatus",
                table: "Medications",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MedicationStatus",
                table: "Medications");
        }
    }
}
