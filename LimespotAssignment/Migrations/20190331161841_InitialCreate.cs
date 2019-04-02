using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LimespotAssignment.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpecialNames",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransformerName = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialNames", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Transformers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Allegiance = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Strength = table.Column<int>(nullable: false),
                    Intelligence = table.Column<int>(nullable: false),
                    Speed = table.Column<int>(nullable: false),
                    Endurance = table.Column<int>(nullable: false),
                    Rank = table.Column<int>(nullable: false),
                    Courage = table.Column<int>(nullable: false),
                    Firepower = table.Column<int>(nullable: false),
                    Skill = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsSpecial = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ID", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "SpecialNames",
                columns: new[] { "ID", "IsActive", "TransformerName" },
                values: new object[] { 1, true, "Predaking" });

            migrationBuilder.InsertData(
                table: "SpecialNames",
                columns: new[] { "ID", "IsActive", "TransformerName" },
                values: new object[] { 2, true, "Optimus" });

            migrationBuilder.CreateIndex(
                name: "IX_Name",
                table: "Transformers",
                column: "Name")
                .Annotation("SqlServer:Clustered", false);

            var storedProc = @" CREATE PROCEDURE [dbo].[GetOverallScore]
                                @TransformerID INT
                                AS
                                BEGIN
                                    DECLARE @OVERALLSCORE INT;
                                    SET @OVERALLSCORE = -1;

                                    SELECT @OVERALLSCORE = STRENGTH + INTELLIGENCE + SPEED + ENDURANCE + RANK + COURAGE + FIREPOWER + SKILL 
                                           FROM Transformers WHERE ID = @TransformerID AND IsActive = 1;
                                    
                                    RETURN @OVERALLSCORE;
                                END";
            migrationBuilder.Sql(storedProc);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecialNames");

            migrationBuilder.DropTable(
                name: "Transformers");
        }
    }
}
