using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BilgeShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "IsDeleted", "LastName", "ModifiedDate", "Password", "UserType" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@bilgeshop.com", "Bilge", false, "Adam", null, "CfDJ8PW3WN_1evBGkHBECERk5VYLxhy9RfuNpqJUKVM8mpRqNzj1JBz8oj9agFsmKgUBEwf1MoATugId9mFd3zkWAQ_uQ9e2kKsk1RM2EsOiPHGNXlvC4KdvqLS_SRTxkz8D-g", 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
