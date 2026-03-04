using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace University.Common
{
    // Реалізація CRUD сервісу
    public class CrudService<T> : ICrudService<T> where T : class
    {
        private List<T> _items = new List<T>();

        public void Create(T item)
        {
            _items.Add(item);
        }

        public IEnumerable<T> ReadAll()
        {
            return _items;
        }

        public void Update(int index, T item)
        {
            if (index >= 0 && index < _items.Count)
            {
                _items[index] = item;
            }
            else
            {
                Console.WriteLine("Помилка: Індекс виходить за межі.");
            }
        }

        public void Delete(int index)
        {
            if (index >= 0 && index < _items.Count)
            {
                _items.RemoveAt(index);
            }
            else
            {
                Console.WriteLine("Помилка: Індекс виходить за межі.");
            }
        }

        public void Save(string filePath)
        {
            string jsonString = JsonSerializer.Serialize(_items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonString);
            Console.WriteLine($"[Система]: Дані успішно збережено у файл {filePath}");
        }

        public void Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                _items = JsonSerializer.Deserialize<List<T>>(jsonString) ?? new List<T>();
                Console.WriteLine($"[Система]: Дані успішно завантажено з файлу {filePath}");
            }
            else
            {
                Console.WriteLine($"[Система]: Файл {filePath} не знайдено.");
            }
        }
    }
}