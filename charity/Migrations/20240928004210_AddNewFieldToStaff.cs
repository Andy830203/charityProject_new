using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace charity.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldToStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AddColumn<string>(
            name: "imgName",
            table: "Staff",
            nullable: true);

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "imgName",
            table: "Staff");
        }
    }
}
