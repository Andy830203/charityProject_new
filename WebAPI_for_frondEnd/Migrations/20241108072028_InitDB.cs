using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_for_frondEnd.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Collection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: true),
                    eventId = table.Column<int>(type: "int", nullable: true),
                    attendance = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "discount",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    rate = table.Column<decimal>(type: "decimal(2,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("discount_id_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "event_category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ec_c_id_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "level_exp",
                columns: table => new
                {
                    level = table.Column<int>(type: "int", nullable: false),
                    exp = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("le_level_pk", x => x.level);
                });

            migrationBuilder.CreateTable(
                name: "location",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    longitude = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    latitude = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    plus_code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    capacity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("location_id_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "member_access",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ma_id_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "member_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ms_id_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("os_id_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pc_c_id_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "staff_access",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sa_id_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "staff_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ss_id_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "location_img",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    l_id = table.Column<int>(type: "int", nullable: true),
                    img_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("location_imgs_li_id_pk", x => x.id);
                    table.ForeignKey(
                        name: "location_l_id_fk",
                        column: x => x.l_id,
                        principalTable: "location",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "member",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    account = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    nickName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    realName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    gender = table.Column<bool>(type: "bit", nullable: true),
                    birthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    phone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    points = table.Column<int>(type: "int", nullable: true),
                    checkin = table.Column<int>(type: "int", nullable: true),
                    exp = table.Column<int>(type: "int", nullable: true),
                    img_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    access = table.Column<int>(type: "int", nullable: true),
                    face_rec = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("member_id_pk", x => x.id);
                    table.ForeignKey(
                        name: "member_access_fk",
                        column: x => x.access,
                        principalTable: "member_access",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "member_status_fk",
                        column: x => x.status,
                        principalTable: "member_status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "staff",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    account = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    realName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    gender = table.Column<bool>(type: "bit", nullable: true),
                    birthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    phone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    arrivalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    resignDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    access = table.Column<int>(type: "int", nullable: true),
                    imgName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("staff_id_pk", x => x.id);
                    table.ForeignKey(
                        name: "staff_access_fk",
                        column: x => x.access,
                        principalTable: "staff_access",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "staff_status_fk",
                        column: x => x.status,
                        principalTable: "staff_status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "event",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    organizer_id = table.Column<int>(type: "int", nullable: true),
                    fee = table.Column<int>(type: "int", nullable: true),
                    capacity = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    priority = table.Column<int>(type: "int", nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("events_id_pk", x => x.id);
                    table.ForeignKey(
                        name: "events_category_id_fk",
                        column: x => x.category_id,
                        principalTable: "event_category",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "events_organizer_id_fk",
                        column: x => x.organizer_id,
                        principalTable: "member",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    buyer = table.Column<int>(type: "int", nullable: true),
                    total_price = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    order_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    discount_code = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("o_id_pk", x => x.id);
                    table.ForeignKey(
                        name: "o_buyer_fk",
                        column: x => x.buyer,
                        principalTable: "member",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "o_status_fk",
                        column: x => x.status,
                        principalTable: "order_status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    seller = table.Column<int>(type: "int", nullable: true),
                    category = table.Column<int>(type: "int", nullable: true),
                    price = table.Column<int>(type: "int", nullable: true),
                    on_shelf = table.Column<bool>(type: "bit", nullable: true),
                    on_shelf_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    instock = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_id_pk", x => x.id);
                    table.ForeignKey(
                        name: "p_category_fk",
                        column: x => x.category,
                        principalTable: "product_category",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "p_seller_fk",
                        column: x => x.seller,
                        principalTable: "member",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    m_id = table.Column<int>(type: "int", nullable: true),
                    e_id = table.Column<int>(type: "int", nullable: true),
                    content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    score = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("comment_id_pk", x => x.id);
                    table.ForeignKey(
                        name: "comment_e_id_fk",
                        column: x => x.e_id,
                        principalTable: "event",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "comment_m_id_fk",
                        column: x => x.m_id,
                        principalTable: "member",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "event_img",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    e_id = table.Column<int>(type: "int", nullable: true),
                    img_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ei_id_pk", x => x.id);
                    table.ForeignKey(
                        name: "ei_e_id_fk",
                        column: x => x.e_id,
                        principalTable: "event",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "event_location",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    e_id = table.Column<int>(type: "int", nullable: true),
                    l_id = table.Column<int>(type: "int", nullable: true),
                    order_in_event = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("el_id_pk", x => x.id);
                    table.ForeignKey(
                        name: "el_e_id_fk",
                        column: x => x.e_id,
                        principalTable: "event",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "el_l_id_fk",
                        column: x => x.l_id,
                        principalTable: "location",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "event_period",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    e_id = table.Column<int>(type: "int", nullable: true),
                    start_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    end_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ep_id_pk", x => x.id);
                    table.ForeignKey(
                        name: "ep_e_id_fk",
                        column: x => x.e_id,
                        principalTable: "event",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "cart_item",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    buyer = table.Column<int>(type: "int", nullable: true),
                    p_id = table.Column<int>(type: "int", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("cart_item_id_pk", x => x.id);
                    table.ForeignKey(
                        name: "cart_item_buyer_fk",
                        column: x => x.buyer,
                        principalTable: "member",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "cart_item_p_id_fk",
                        column: x => x.p_id,
                        principalTable: "product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order_item",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    o_id = table.Column<int>(type: "int", nullable: true),
                    p_id = table.Column<int>(type: "int", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    score = table.Column<int>(type: "int", nullable: true),
                    shipped_time = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("oi_id_pk", x => x.id);
                    table.ForeignKey(
                        name: "oi_o_id_fk",
                        column: x => x.o_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "oi_p_id_fk",
                        column: x => x.p_id,
                        principalTable: "product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "product_img",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    p_id = table.Column<int>(type: "int", nullable: true),
                    img_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pi_id_pk", x => x.id);
                    table.ForeignKey(
                        name: "pi_p_id_fk",
                        column: x => x.p_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comment_img",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    c_id = table.Column<int>(type: "int", nullable: true),
                    img_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ci_id_pk", x => x.id);
                    table.ForeignKey(
                        name: "ci_c_id_fk",
                        column: x => x.c_id,
                        principalTable: "comment",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "sign_up",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ep_id = table.Column<int>(type: "int", nullable: true),
                    applicant = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("su_id_pk", x => x.id);
                    table.ForeignKey(
                        name: "su_applicant",
                        column: x => x.applicant,
                        principalTable: "member",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "su_ep_id_fk",
                        column: x => x.ep_id,
                        principalTable: "event_period",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_cart_item_buyer",
                table: "cart_item",
                column: "buyer");

            migrationBuilder.CreateIndex(
                name: "IX_cart_item_p_id",
                table: "cart_item",
                column: "p_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_e_id",
                table: "comment",
                column: "e_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_m_id",
                table: "comment",
                column: "m_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_img_c_id",
                table: "comment_img",
                column: "c_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_category_id",
                table: "event",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_organizer_id",
                table: "event",
                column: "organizer_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_img_e_id",
                table: "event_img",
                column: "e_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_location_e_id",
                table: "event_location",
                column: "e_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_location_l_id",
                table: "event_location",
                column: "l_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_period_e_id",
                table: "event_period",
                column: "e_id");

            migrationBuilder.CreateIndex(
                name: "IX_location_img_l_id",
                table: "location_img",
                column: "l_id");

            migrationBuilder.CreateIndex(
                name: "IX_member_access",
                table: "member",
                column: "access");

            migrationBuilder.CreateIndex(
                name: "IX_member_status",
                table: "member",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_o_id",
                table: "order_item",
                column: "o_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_p_id",
                table: "order_item",
                column: "p_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_buyer",
                table: "orders",
                column: "buyer");

            migrationBuilder.CreateIndex(
                name: "IX_orders_status",
                table: "orders",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_product_category",
                table: "product",
                column: "category");

            migrationBuilder.CreateIndex(
                name: "IX_product_seller",
                table: "product",
                column: "seller");

            migrationBuilder.CreateIndex(
                name: "IX_product_img_p_id",
                table: "product_img",
                column: "p_id");

            migrationBuilder.CreateIndex(
                name: "IX_sign_up_applicant",
                table: "sign_up",
                column: "applicant");

            migrationBuilder.CreateIndex(
                name: "IX_sign_up_ep_id",
                table: "sign_up",
                column: "ep_id");

            migrationBuilder.CreateIndex(
                name: "IX_staff_access",
                table: "staff",
                column: "access");

            migrationBuilder.CreateIndex(
                name: "IX_staff_status",
                table: "staff",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cart_item");

            migrationBuilder.DropTable(
                name: "Collection");

            migrationBuilder.DropTable(
                name: "comment_img");

            migrationBuilder.DropTable(
                name: "discount");

            migrationBuilder.DropTable(
                name: "event_img");

            migrationBuilder.DropTable(
                name: "event_location");

            migrationBuilder.DropTable(
                name: "level_exp");

            migrationBuilder.DropTable(
                name: "location_img");

            migrationBuilder.DropTable(
                name: "order_item");

            migrationBuilder.DropTable(
                name: "product_img");

            migrationBuilder.DropTable(
                name: "sign_up");

            migrationBuilder.DropTable(
                name: "staff");

            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "location");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "event_period");

            migrationBuilder.DropTable(
                name: "staff_access");

            migrationBuilder.DropTable(
                name: "staff_status");

            migrationBuilder.DropTable(
                name: "order_status");

            migrationBuilder.DropTable(
                name: "product_category");

            migrationBuilder.DropTable(
                name: "event");

            migrationBuilder.DropTable(
                name: "event_category");

            migrationBuilder.DropTable(
                name: "member");

            migrationBuilder.DropTable(
                name: "member_access");

            migrationBuilder.DropTable(
                name: "member_status");
        }
    }
}
