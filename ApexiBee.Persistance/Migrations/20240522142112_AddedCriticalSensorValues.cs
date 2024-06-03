using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApexiBee.Persistance.Migrations
{
    public partial class AddedCriticalSensorValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CriticalHumidityHigh",
                table: "HubStations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CriticalHumidityLow",
                table: "HubStations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CriticalTemperaruteHigh",
                table: "HubStations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CriticalTemperatureLow",
                table: "HubStations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CriticalHumidityHigh",
                table: "HubStations");

            migrationBuilder.DropColumn(
                name: "CriticalHumidityLow",
                table: "HubStations");

            migrationBuilder.DropColumn(
                name: "CriticalTemperaruteHigh",
                table: "HubStations");

            migrationBuilder.DropColumn(
                name: "CriticalTemperatureLow",
                table: "HubStations");
        }
    }
}
