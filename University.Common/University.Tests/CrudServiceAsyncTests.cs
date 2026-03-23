using System;
using System.Linq;
using System.Threading.Tasks;
using University.Common;
using Xunit;

namespace University.Tests
{
    public class CrudServiceAsyncTests
    {
        // Допоміжний метод для створення чистого сервісу для кожного тесту
        private CrudServiceAsync<Bus> CreateService()
        {
            // Генеруємо унікальне ім'я файлу, щоб тести не перезаписували файли один одного
            string tempFile = $"test_buses_{Guid.NewGuid()}.json";
            return new CrudServiceAsync<Bus>(tempFile);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddElement()
        {
            // Arrange (Підготовка)
            var service = CreateService();
            var bus = Bus.CreateNew();

            // Act (Дія)
            var result = await service.CreateAsync(bus);
            var readBus = await service.ReadAsync(bus.Id);

            // Assert (Перевірка)
            Assert.True(result); // Перевіряємо, чи метод повернув true
            Assert.NotNull(readBus); // Перевіряємо, чи об'єкт дійсно зберігся
            Assert.Equal(bus.Id, readBus.Id);
        }

        [Fact]
        public async Task ReadAsync_ShouldReturnNull_WhenElementDoesNotExist()
        {
            var service = CreateService();

            // Шукаємо неіснуючий Guid
            var result = await service.ReadAsync(Guid.NewGuid());

            // Має повернути null
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyElement()
        {
            var service = CreateService();
            var bus = Bus.CreateNew();
            await service.CreateAsync(bus); // Спочатку додаємо

            // Змінюємо дані
            bus.Model = "Updated Bus Model";

            var updateResult = await service.UpdateAsync(bus);
            var updatedBus = await service.ReadAsync(bus.Id);

            Assert.True(updateResult);
            Assert.Equal("Updated Bus Model", updatedBus.Model); // Перевіряємо, чи змінилася модель
        }

        [Fact]
        public async Task RemoveAsync_ShouldDeleteElement()
        {
            var service = CreateService();
            var bus = Bus.CreateNew();
            await service.CreateAsync(bus); // Додаємо

            var removeResult = await service.RemoveAsync(bus); // Видаляємо
            var readBus = await service.ReadAsync(bus.Id); // Спробуємо знайти

            Assert.True(removeResult);
            Assert.Null(readBus); // Об'єкт має бути null, бо ми його видалили
        }

        [Fact]
        public async Task ReadAllAsync_Pagination_ShouldReturnCorrectAmount()
        {
            var service = CreateService();

            // Додаємо 10 автобусів
            for (int i = 0; i < 10; i++)
            {
                await service.CreateAsync(Bus.CreateNew());
            }

            // Отримуємо 2-гу сторінку, по 3 елементи на сторінці (має повернути 3 елементи)
            var pagedResult = await service.ReadAllAsync(page: 2, amount: 3);

            Assert.Equal(3, pagedResult.Count());
        }
    }
}