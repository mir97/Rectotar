using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Rectotarat.Migrations
{
    public partial class v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Newss",
                columns: table => new
                {
                    Newsid = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Header = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    PublicDate = table.Column<DateTime>(nullable: false),
                    UnivercityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Newss", x => x.Newsid);
                    table.ForeignKey(
                        name: "FK_Newss_Universitys_UnivercityId",
                        column: x => x.UnivercityId,
                        principalTable: "Universitys",
                        principalColumn: "UniversityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Newss_UnivercityId",
                table: "Newss",
                column: "UnivercityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Newss");
        }
    }
}
