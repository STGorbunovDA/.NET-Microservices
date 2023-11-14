using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        /*
            * Метод PrepPopulation используется для подготовки данных популяции (наполнения) базы данных приложения.
            * Принимает параметры: app - экземпляр IApplicationBuilder для доступа к сервисам приложения,
            * isProd - флаг, указывающий, находится ли приложение в продакшн-окружении.
        */
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            // Создание области использования сервисов приложения
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                // Получение экземпляра AppDbContext из сервисов приложения (DI)
                var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();

                // Вызов метода SeedData для заполнения данных в базе данных
                // Передаем экземпляр dbContext и флаг isProd в метод SeedData
                SeedData(dbContext, isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }

            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data...");

                context.Platforms.AddRange(
                    new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}