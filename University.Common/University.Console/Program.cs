using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using University.Common;

namespace University.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            string filePath = "buses_data.json";
            var service = new CrudServiceAsync<Bus>(filePath);

            Console.WriteLine("=== Завдання 3: Паралельне створення об'єктів ===");
            Console.WriteLine("Починаємо паралельне створення 1000 автобусів...");

            Parallel.For(0, 1000, i =>
            {
                var newBus = Bus.CreateNew();
                service.CreateAsync(newBus).Wait();
            });

            var allBuses = await service.ReadAllAsync();
            Console.WriteLine($"Успішно створено та додано {allBuses.Count()} об'єктів у колекцію.\n");

            Console.WriteLine("=== LINQ: Статистика для цифрових значень ===");

            var minSpeed = allBuses.Min(b => b.Speed);
            var maxSpeed = allBuses.Max(b => b.Speed);
            var avgSpeed = allBuses.Average(b => b.Speed);
            Console.WriteLine($"Швидкість (км/год) -> Мін: {minSpeed}, Макс: {maxSpeed}, Середня: {avgSpeed:F2}");

            var minCapacity = allBuses.Min(b => b.Capacity);
            var maxCapacity = allBuses.Max(b => b.Capacity);
            var avgCapacity = allBuses.Average(b => b.Capacity);
            Console.WriteLine($"Місткість (пасажирів) -> Мін: {minCapacity}, Макс: {maxCapacity}, Середня: {avgCapacity:F2}\n");

            Console.WriteLine("=== Збереження у файл ===");
            await service.SaveAsync();
            Console.WriteLine($"Згенеровану колекцію успішно збережено у файл: {filePath}\n");

            Console.WriteLine("=== Завдання 4: Примітиви синхронізації ===");
            DemoSyncPrimitives();

            Console.WriteLine("\nРобота програми успішно завершена. Натисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void DemoSyncPrimitives()
        {
            Console.WriteLine("\n--- Демонстрація Lock ---");
            object lockObj = new object();
            int sharedCounter = 0;

            Parallel.For(0, 5, i =>
            {
                lock (lockObj)
                {
                    sharedCounter++;
                    Console.WriteLine($"Потік {Task.CurrentId} збільшив лічильник до {sharedCounter}");
                }
            });

            Console.WriteLine("\n--- Демонстрація AutoResetEvent ---");
            AutoResetEvent autoEvent = new AutoResetEvent(false);

            Task.Run(() =>
            {
                Console.WriteLine("Побічний потік 1 чекає на дозвіл (сигнал)...");
                autoEvent.WaitOne();
                Console.WriteLine("Побічний потік 1 отримав сигнал і продовжив роботу!");
            });

            Thread.Sleep(1000);
            Console.WriteLine("Головний потік надсилає сигнал для AutoResetEvent...");
            autoEvent.Set();
            Thread.Sleep(500);
        }
    }
}