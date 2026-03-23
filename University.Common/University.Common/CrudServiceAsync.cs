using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace University.Common
{
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : IEntity
    {
        private readonly ConcurrentDictionary<Guid, T> _collection = new();
        private readonly string _filePath;

        private readonly SemaphoreSlim _fileSemaphore = new SemaphoreSlim(1, 1);

        public CrudServiceAsync(string filePath)
        {
            _filePath = filePath;
        }

        public Task<bool> CreateAsync(T element)
        {
            bool added = _collection.TryAdd(element.Id, element);
            return Task.FromResult(added);
        }

        public Task<T> ReadAsync(Guid id)
        {
            _collection.TryGetValue(id, out var element);
            return Task.FromResult(element); 
        }

        public Task<IEnumerable<T>> ReadAllAsync()
        {
            return Task.FromResult(_collection.Values.AsEnumerable());
        }

        public Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            var pagedData = _collection.Values
                .Skip((page - 1) * amount)
                .Take(amount);

            return Task.FromResult(pagedData);
        }

        public Task<bool> UpdateAsync(T element)
        {
            if (_collection.ContainsKey(element.Id))
            {
                _collection[element.Id] = element;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> RemoveAsync(T element)
        {
            bool removed = _collection.TryRemove(element.Id, out _);
            return Task.FromResult(removed);
        }

        public async Task<bool> SaveAsync()
        {
            // Чекаємо, поки файл не звільниться від інших потоків
            await _fileSemaphore.WaitAsync();
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(_collection.Values, options);

                // Асинхронний запис у файл
                await File.WriteAllTextAsync(_filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка збереження у файл: {ex.Message}");
                return false;
            }
            finally
            {
                // Обов'язково звільняємо семафор, щоб інші потоки могли працювати
                _fileSemaphore.Release();
            }
        }

        // Реалізація інтерфейсу IEnumerable<T>
        public IEnumerator<T> GetEnumerator()
        {
            return _collection.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}