using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University.Common
{
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : class
    {
        private readonly IRepository<T> _repository;

        // Впроваджуємо залежність через конструктор (Dependency Injection)
        public CrudServiceAsync(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<bool> CreateAsync(T element)
        {
            try
            {
                await _repository.AddAsync(element);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<T?> ReadAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            var allData = await _repository.GetAllAsync();
            return allData.Skip((page - 1) * amount).Take(amount);
        }

        public async Task<bool> UpdateAsync(T element)
        {
            try
            {
                await _repository.Update(element);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveAsync(T element)
        {
            try
            {
                await _repository.Delete(element);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // В контексті EF Core SaveAsync часто не потрібен окремо, 
        // бо SaveChangesAsync викликається всередині репозиторію. 
        // Але ми залишаємо його для виконання інтерфейсу.
        public Task<bool> SaveAsync()
        {
            return Task.FromResult(true);
        }
    }
}