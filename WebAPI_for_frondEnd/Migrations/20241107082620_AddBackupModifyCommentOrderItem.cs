using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_for_frondEnd.Migrations
{
    /// <inheritdoc />
    public partial class AddBackupModifyCommentOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "backup1",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    col1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    col2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    col3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    col4 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    col5 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("backup1_id_pk", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "backup2",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    col1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    col2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    col3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    col4 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    col5 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("backup2_id_pk", x => x.id);
                }
            );
            migrationBuilder.CreateTable(
                name: "collections",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: true),
                    EventId = table.Column<int>(type: "int", nullable: true),
                    attendance = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("collection_id_pk", x => x.id);
                }
            );

            migrationBuilder.AddColumn<int>(
        name: "score",
        table: "order_item",
        type: "int",         // 設定資料型別為 int
        nullable: true);

            migrationBuilder.AddColumn<int>(
        name: "score",
        table: "comment",
        type: "int",         // 設定資料型別為 int
        nullable: true);
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 刪除新增的欄位
            migrationBuilder.DropColumn(
                name: "score",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "score",
                table: "Comment");

            // 刪除 collections 表格
            migrationBuilder.DropTable(
                name: "collections");

            // 刪除 backup2 表格
            migrationBuilder.DropTable(
                name: "backup2");

            // 刪除 backup1 表格
            migrationBuilder.DropTable(
                name: "backup1");
        }
    }
    
}
