using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using University.Common;
using University.Infrastructure;
using University.Infrastructure.Models;

namespace University.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Лабораторна робота №3: Entity Framework Core ===");

            using var context = new UniversityContext();

            // Автоматичне створення бази та таблиць
            await context.Database.MigrateAsync();

            var busRepository = new UniversityRepository<BusModel>(context);

            // ОСЬ ЦЕЙ РЯДОК ЗАГУБИВСЯ: Створюємо сервіс!
            var busService = new CrudServiceAsync<BusModel>(busRepository);

            Console.WriteLine("Генеруємо 5 автобусів для бази даних...");

            // Створюємо 5 автобусів у циклі
            for (int i = 1; i <= 5; i++)
            {
                var bus = new BusModel
                {
                    Brand = $"Автобус-Бренд {i}",
                    ModelName = $"Модель {i}00",
                    Speed = 60 + (i * 10),
                    Capacity = 20 + (i * 5),
                    Driver = new DriverModel { Name = $"Водій {i}" }
                };

                // Додаємо маршрути
                bus.Routes.Add(new RouteModel { RouteName = $"Маршрут {i}-A" });
                bus.Routes.Add(new RouteModel { RouteName = $"Маршрут {i}-B" });

                // Зберігаємо в базу
                await busService.CreateAsync(bus);
            }

            Console.WriteLine("Усі 5 автобусів з водіями та маршрутами успішно додано до БД!");

            // Читаємо з бази, щоб перевірити
            var allBuses = await busService.ReadAllAsync();
            Console.WriteLine($"\nЗагальна кількість автобусів у БД: {allBuses.Count()}");

            foreach (var b in allBuses)
            {
                Console.WriteLine($"- {b.Brand} {b.ModelName} (Швидкість: {b.Speed})");
            }

            Console.WriteLine("\nРобота програми успішно завершена. Натисніть будь-яку клавішу...");
            Console.ReadKey();
        }
    }
}