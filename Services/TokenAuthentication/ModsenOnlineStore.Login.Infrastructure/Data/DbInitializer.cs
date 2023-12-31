﻿using ModsenOnlineStore.Login.Domain.Entities;

namespace ModsenOnlineStore.Login.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(DataContext context)
        {
            if (!context.Users.Any())
            {
                var adminUser = new User()
                {
                    Email = "admin@gmail.com",
                    Password = "rJaJ4ickJwheNbnT4+i+2IyzQ0gotDuG/AWWytTG4nA=",
                    Name = "Evgeny",
                    Role = Role.Admin,
                };

                var Ivan = new User()
                {
                    Email = "ivan@gmail.com",
                    Password = "SMRJR4jgSEAQxcJ0Ojkwq5TxNXx4UoPTUo28Hb4+jF8=",
                    Name = "Ivan",
                    Money = 100,
                    Role = Role.User,
                };

                var Max = new User()
                {
                    Email = "max@gmail.com",
                    Password = "MEmjge84+knLBnRR2GrnPD12pZ8niQSijLStq4PJrTY=",
                    Name = "Max",
                    Money = 100,
                    Role = Role.User,
                };

                context.Users.AddRange(new User[] { adminUser, Ivan, Max });
                await context.SaveChangesAsync();
            }
        }
    }
}
