using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Rectotarat.Migrations
{
    public partial class v8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Raschets",
                columns: table => new
                {
                    Raschetid = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Position = table.Column<float>(nullable: false),
                    SumaValue = table.Column<float>(nullable: false),
                    UnivercityId = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raschets", x => x.Raschetid);
                    table.ForeignKey(
                        name: "FK_Raschets_Universitys_UnivercityId",
                        column: x => x.UnivercityId,
                        principalTable: "Universitys",
                        principalColumn: "UniversityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Raschets_UnivercityId",
                table: "Raschets",
                column: "UnivercityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Raschets");
        }
    }
}
