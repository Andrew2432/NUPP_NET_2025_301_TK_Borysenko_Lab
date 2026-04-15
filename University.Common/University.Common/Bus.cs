using System;

namespace University.Common
{
    // Клас реалізує IEntity, щоб гарантовано мати Guid Id для нашого сервісу
    public class Bus
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Model { get; set; }

        // Цифрові значення для перевірки LINQ (Min, Max, Avg)
        public int Speed { get; set; }
        public int Capacity { get; set; }

        // Об'єкт для генерації випадкових чисел
        private static readonly Random _random = new Random();

        // Статичний метод для створення об'єкта зі згенерованими даними (Пункт 2 завдання)
        public static Bus CreateNew()
        {
            return new Bus
            {
                Model = $"Bus-{_random.Next(1, 1000)}", // Генеруємо назву, наприклад Bus-45
                Speed = _random.Next(40, 120),          // Випадкова швидкість від 40 до 120 км/год
                Capacity = _random.Next(15, 60)         // Випадкова місткість від 15 до 60 пасажирів
            };
        }
    }
}