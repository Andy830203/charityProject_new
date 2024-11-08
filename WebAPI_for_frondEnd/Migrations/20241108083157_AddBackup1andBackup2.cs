using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_for_frondEnd.Migrations
{
    /// <inheritdoc />
    public partial class AddBackup1andBackup2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "backUp1s",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    col1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    col2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    col3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    col4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    col5 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_backUp1s", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "backUp2s",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    col1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    col2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    col3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    col4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    col5 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_backUp2s", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "backUp1s");

            migrationBuilder.DropTable(
                name: "backUp2s");
        }
    }
}
