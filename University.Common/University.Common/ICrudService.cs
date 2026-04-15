using System.Collections.Generic;

namespace University.Common
{
    // Узагальнений (generic) інтерфейс CRUD
    public interface ICrudService<T> where T : class
    {
        void Create(T item);
        IEnumerable<T> ReadAll();
        void Update(int index, T item);
        void Delete(int index);

        // Додаткове завдання
        void Save(string filePath);
        void Load(string filePath);
    }
}