using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace coreAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "AccessType",
            //    columns: table => new
            //    {
            //        actID = table.Column<int>(type: "int", nullable: false),
            //        actName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AccessTypes", x => x.actID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Application",
            //    columns: table => new
            //    {
            //        appID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        appName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
            //        appFlags = table.Column<int>(type: "int", nullable: true),
            //        appActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
            //        appCreatedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        appCreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
            //        appModifiedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        appModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Applications", x => x.appID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AppUserPermission",
            //    columns: table => new
            //    {
            //        perID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        apuID = table.Column<int>(type: "int", nullable: true),
            //        apID = table.Column<int>(type: "int", nullable: false),
            //        actID = table.Column<int>(type: "int", nullable: true),
            //        perMetadata = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
            //        perActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
            //        perCreatedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        perCreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        perModifiedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        perModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AppUserPermissions", x => x.perID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ContentType",
            //    columns: table => new
            //    {
            //        cntID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        cntName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ContentType", x => x.cntID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "User",
            //    columns: table => new
            //    {
            //        usrID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        usrLogin = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        usrLastName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        usrFirstName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        usrClock = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
            //        usrEmail = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
            //        usrActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
            //        usrCreatedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //        usrCreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
            //        usrModifiedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        usrModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Users", x => x.usrID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AppDatabaseRole",
            //    columns: table => new
            //    {
            //        adbID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        appID = table.Column<int>(type: "int", nullable: true),
            //        adbName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
            //        adbRoleName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
            //        adbAccessKey = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
            //        adbActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
            //        adbCreatedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        adbCreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
            //        adbModifiedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        adbModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AppDatabaseRoles", x => x.adbID);
            //        table.ForeignKey(
            //            name: "FK_AppDatabaseRoles_Applications",
            //            column: x => x.appID,
            //            principalTable: "Application",
            //            principalColumn: "appID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AppPermission",
            //    columns: table => new
            //    {
            //        apID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        appID = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((1))"),
            //        permName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
            //        apActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
            //        apCreatedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        apCreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
            //        apModifiedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        apModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Permissions", x => x.apID);
            //        table.ForeignKey(
            //            name: "FK_Permission_Application",
            //            column: x => x.appID,
            //            principalTable: "Application",
            //            principalColumn: "appID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AppUser",
            //    columns: table => new
            //    {
            //        apuID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        appID = table.Column<int>(type: "int", nullable: false),
            //        usrID = table.Column<int>(type: "int", nullable: false),
            //        apuActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
            //        apuAccessKey = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
            //        apuCreatedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        apuCreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
            //        apuModifiedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        apuModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ApplicationUser", x => x.apuID);
            //        table.ForeignKey(
            //            name: "FK_ApplicationUser_Applications",
            //            column: x => x.appID,
            //            principalTable: "Application",
            //            principalColumn: "appID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_ApplicationUser_Users",
            //            column: x => x.usrID,
            //            principalTable: "User",
            //            principalColumn: "usrID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AppUserSetting",
            //    columns: table => new
            //    {
            //        setID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        apuID = table.Column<int>(type: "int", nullable: true),
            //        setName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
            //        setValue = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
            //        setContentTypeID = table.Column<int>(type: "int", nullable: true),
            //        setActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
            //        setCreatedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //        setCreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        setModifiedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //        setModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AppUserSettings", x => x.setID);
            //        table.ForeignKey(
            //            name: "FK_AppUserSettings_AppUsers",
            //            column: x => x.apuID,
            //            principalTable: "AppUser",
            //            principalColumn: "apuID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_AppUserSettings_ContentType",
            //            column: x => x.setContentTypeID,
            //            principalTable: "ContentType",
            //            principalColumn: "cntID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_AppDatabaseRole_appID",
            //    table: "AppDatabaseRole",
            //    column: "appID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AppPermission_appID",
            //    table: "AppPermission",
            //    column: "appID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AppUser_appID",
            //    table: "AppUser",
            //    column: "appID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AppUsers_Unique_usrID_appID",
            //    table: "AppUser",
            //    columns: new[] { "usrID", "appID" },
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_AppUserPermissions_Unique_apuID_apID",
            //    table: "AppUserPermission",
            //    columns: new[] { "apuID", "apID" },
            //    unique: true,
            //    filter: "[apuID] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AppUserSetting_apuID",
            //    table: "AppUserSetting",
            //    column: "apuID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AppUserSetting_setContentTypeID",
            //    table: "AppUserSetting",
            //    column: "setContentTypeID");

            //migrationBuilder.CreateIndex(
            //    name: "UX_UsrLogin",
            //    table: "User",
            //    column: "usrLogin",
            //    unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessType");

            migrationBuilder.DropTable(
                name: "AppDatabaseRole");

            migrationBuilder.DropTable(
                name: "AppPermission");

            migrationBuilder.DropTable(
                name: "AppUserPermission");

            migrationBuilder.DropTable(
                name: "AppUserSetting");

            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropTable(
                name: "ContentType");

            migrationBuilder.DropTable(
                name: "Application");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
