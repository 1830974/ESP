using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_CIUSSS_1830974.Migrations
{
    public partial class AddedRevenueByTypeforsummaries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RevenueByTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Week = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Hourly = table.Column<double>(type: "double", nullable: false),
                    HalfDay = table.Column<double>(type: "double", nullable: false),
                    FullDay = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevenueByTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RevenueByTypes");
        }
    }
}
