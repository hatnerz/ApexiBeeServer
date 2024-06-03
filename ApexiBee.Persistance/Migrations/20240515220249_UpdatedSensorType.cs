using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApexiBee.Persistance.Migrations
{
    public partial class UpdatedSensorType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MeasureUnit",
                table: "SensorTypes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeasureUnit",
                table: "SensorTypes");
        }
    }
}
