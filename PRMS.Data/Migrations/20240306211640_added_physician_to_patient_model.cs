using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class added_physician_to_patient_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhysicianId",
                table: "Patients",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PhysicianId",
                table: "Patients",
                column: "PhysicianId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Physicians_PhysicianId",
                table: "Patients",
                column: "PhysicianId",
                principalTable: "Physicians",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Physicians_PhysicianId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_PhysicianId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PhysicianId",
                table: "Patients");
        }
    }
}
