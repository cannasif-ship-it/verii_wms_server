using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data
{
    public static class SeedData
    {
        public static void Initialize(WmsDbContext context)
        {
            // Veritabanının oluşturulduğundan emin ol
            context.Database.EnsureCreated();

            try
            {
                context.Database.ExecuteSqlRaw(@"
                    IF NOT EXISTS (
                        SELECT 1
                        FROM sys.default_constraints dc
                        INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
                        WHERE dc.parent_object_id = OBJECT_ID('[dbo].[RII_USER_SESSION]') AND c.name = 'IsDeleted'
                    )
                    BEGIN
                        ALTER TABLE [dbo].[RII_USER_SESSION]
                        ADD CONSTRAINT [DF_RII_USER_SESSION_IsDeleted] DEFAULT(0) FOR [IsDeleted];
                    END

                    UPDATE [dbo].[RII_USER_SESSION]
                    SET [IsDeleted] = 0
                    WHERE [IsDeleted] IS NULL;


                    ");
            }
            catch { }

            // Admin kullanıcısı zaten var mı kontrol et
            if (context.Users.Any(u => u.Email == "admin@v3rii.com"))
            {
                return; // Zaten var, seed data'ya gerek yok
            }

            // Admin kullanıcısını oluştur
            var adminUser = new User
            {
                Username = "admin@v3rii.com",
                Email = "admin@v3rii.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Veriipass123!"),
                FirstName = "Admin",
                LastName = "User",
                RoleId = 3, // Admin role ID (UserAuthorityConfiguration'dan)
                IsEmailConfirmed = true,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            context.Users.Add(adminUser);
            context.SaveChanges();
        }
    }
}