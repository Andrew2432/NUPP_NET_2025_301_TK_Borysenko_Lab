using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace University.Common
{
    // Оновлений інтерфейс згідно з Лабораторною №3
    public interface ICrudServiceAsync<T> where T : class
    {
        Task<bool> CreateAsync(T element);
        Task<T?> ReadAsync(Guid id); // Додали '?', бо БД може не знайти об'єкт і повернути null
        Task<IEnumerable<T>> ReadAllAsync();
        Task<IEnumerable<T>> ReadAllAsync(int page, int amount);
        Task<bool> UpdateAsync(T element);
        Task<bool> RemoveAsync(T element);
        Task<bool> SaveAsync();
    }
}