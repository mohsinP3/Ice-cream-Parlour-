using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ice_Cream_Parlour_Eproject.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryTableNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 30, 16, 14, 3, 257, DateTimeKind.Local).AddTicks(3562));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 30, 16, 14, 3, 257, DateTimeKind.Local).AddTicks(3573));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 30, 16, 14, 3, 257, DateTimeKind.Local).AddTicks(3575));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 30, 16, 14, 3, 257, DateTimeKind.Local).AddTicks(3577));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 30, 16, 14, 3, 257, DateTimeKind.Local).AddTicks(3578));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 30, 16, 11, 16, 565, DateTimeKind.Local).AddTicks(8144));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 30, 16, 11, 16, 565, DateTimeKind.Local).AddTicks(8156));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 30, 16, 11, 16, 565, DateTimeKind.Local).AddTicks(8158));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 30, 16, 11, 16, 565, DateTimeKind.Local).AddTicks(8160));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 30, 16, 11, 16, 565, DateTimeKind.Local).AddTicks(8162));
        }
    }
}
