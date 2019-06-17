using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace mvcCookieAuthSample.Migrations
{
    public partial class SetIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "AspNetRoles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "AspNetRoles");
        }
    }
}
