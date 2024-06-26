using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MqttDashboard.Migrations
{
    /// <inheritdoc />
    public partial class inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MqttClients",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DeviceName = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SubscribedTopics = table.Column<string>(type: "TEXT", nullable: false),
                    LastAccessed = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<bool>(type: "INTEGER", nullable: false),
                    KeepAlive = table.Column<int>(type: "INTEGER", nullable: false),
                    IpAddress = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MqttClients", x => x.ClientId);
                });

            migrationBuilder.InsertData(
                table: "MqttClients",
                columns: new[] { "ClientId", "Created", "DeviceName", "IpAddress", "KeepAlive", "LastAccessed", "Status", "SubscribedTopics" },
                values: new object[,]
                {
                    { new Guid("84f18f36-9edc-4df5-bb8a-8ebe18adfb74"), new DateTime(2023, 7, 21, 9, 45, 0, 0, DateTimeKind.Unspecified), "Device Gamma", "192.168.1.3", 60, new DateTime(2024, 6, 22, 16, 30, 0, 0, DateTimeKind.Unspecified), true, "[\"topic5\",\"topic6\"]" },
                    { new Guid("d68b6951-3f69-439f-a859-199142adf975"), new DateTime(2023, 6, 11, 11, 20, 0, 0, DateTimeKind.Unspecified), "Device Beta", "192.168.1.2", 60, new DateTime(2024, 6, 23, 10, 15, 0, 0, DateTimeKind.Unspecified), false, "[\"topic3\",\"topic4\"]" },
                    { new Guid("ed0a80cf-8ef9-4ec4-8b65-d5abee91bf73"), new DateTime(2023, 5, 12, 14, 30, 0, 0, DateTimeKind.Unspecified), "Device Alpha", "192.168.1.1", 60, new DateTime(2024, 6, 24, 9, 45, 0, 0, DateTimeKind.Unspecified), true, "[\"topic1\",\"topic2\"]" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MqttClients");
        }
    }
}
