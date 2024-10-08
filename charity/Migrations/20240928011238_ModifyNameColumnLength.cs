using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace charity.Migrations
{
    /// <inheritdoc />
    public partial class ModifyNameColumnLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
    name: "imgName",
    table: "Staff",
    type: "nvarchar(100)",
    maxLength: 100,
    nullable: true,
    oldClrType: typeof(string),
    oldType: "nvarchar(max)",
    oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
       name: "Name",
       table: "Staff",
       type: "nvarchar(max)",
       nullable: true,
       oldClrType: typeof(string),
       oldType: "nvarchar(100)",
       oldMaxLength: 100,
       oldNullable: true);
        }
    }
}
