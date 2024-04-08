using do_an_ltweb.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace do_an.Migrations
{
    public partial class addUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            for (int i = 1; i < 150; i++)
            {
                migrationBuilder.InsertData(
                    "Users",
                    columns: new[] {
                        "Id",
                        "UserName",
                        "Email",
                        "SecurityStamp",
                        "EmailConfirmed",
                        "PhoneNumberConfirmed",
                        "TwoFactorEnabled",
                        "LockoutEnabled",
                        "AccessFailedCount",
                        "Address"


                     },
                    values: new object[] {
                        Guid.NewGuid().ToString(),
                        "User-"+i.ToString("D3"),
                        $"email{i.ToString("D3")}@example.com",
                        Guid.NewGuid().ToString(),
                        true,
                        false,
                        false,
                        false,
                        0,
                        "...@#%..."
                    }
                );
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

//SELECT TOP(1000) [Id]
//      ,[Address]
//      ,[BirthDate]
//      ,[UserName]
//      ,[NormalizedUserName]
//      ,[Email]
//      ,[NormalizedEmail]
//      ,[EmailConfirmed]
//      ,[PasswordHash]
//      ,[SecurityStamp]
//      ,[ConcurrencyStamp]
//      ,[PhoneNumber]
//      ,[PhoneNumberConfirmed]
//      ,[TwoFactorEnabled]
//      ,[LockoutEnd]
//      ,[LockoutEnabled]
//      ,[AccessFailedCount]
//FROM[do_an_ltweb].[dbo].[Users]