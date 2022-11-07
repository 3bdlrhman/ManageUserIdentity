using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManageIdentity.Data.Migrations
{
    public partial class addAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "INSERT INTO [security].[Users] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [ProfilePic]) " +
                "VALUES (N'4955bff4-3281-4c40-86cf-43ffcf1bc912', N'Admin', N'ADMIN', N'Admin@test.com', N'ADMIN@TEST.COM', 0, N'AQAAAAEAACcQAAAAEL4KfDxiAqMTy7fB5RM5pq53pIzxhC3E0LFwsnZOQ0fjmcu4DsakdBSpvZtnWfQiUw==', N'AF33EOX3IBPQMMBR3BXSGEOLQ6GYG2CE', N'c7ee53d5-420b-4f59-833f-71df1e49d121', N'01153244894', 0, NULL, 1, 0, N'Abdo Ahmed', NULL, NULL)"
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.Sql("DELETE FROM [security].[Users] WHERE Id = '4955bff4-3281-4c40-86cf-43ffcf1bc912'");
            migrationBuilder.DeleteData("Users", "Id", new String[] { "4955bff4-3281-4c40-86cf-43ffcf1bc912" }, "security");
        }
    }
}
