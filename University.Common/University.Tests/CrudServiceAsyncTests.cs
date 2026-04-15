//using system;
//using system.linq;
//using system.threading.tasks;
//using university.common;
//using xunit;

//namespace university.tests
//{
//    public class crudserviceasynctests
//    {
//        // допоміжний метод для створення чистого сервісу для кожного тесту
//        private crudserviceasync<bus> createservice()
//        {
//            // генеруємо унікальне ім'я файлу, щоб тести не перезаписували файли один одного
//            string tempfile = $"test_buses_{guid.newguid()}.json";
//            return new crudserviceasync<bus>(tempfile);
//        }

//        [fact]
//        public async task createasync_shouldaddelement()
//        {
//            // arrange (підготовка)
//            var service = createservice();
//            var bus = bus.createnew();

//            // act (дія)
//            var result = await service.createasync(bus);
//            var readbus = await service.readasync(bus.id);

//            // assert (перевірка)
//            assert.true(result); // перевіряємо, чи метод повернув true
//            assert.notnull(readbus); // перевіряємо, чи об'єкт дійсно зберігся
//            assert.equal(bus.id, readbus.id);
//        }

//        [fact]
//        public async task readasync_shouldreturnnull_whenelementdoesnotexist()
//        {
//            var service = createservice();

//            // шукаємо неіснуючий guid
//            var result = await service.readasync(guid.newguid());

//            // має повернути null
//            assert.null(result);
//        }

//        [fact]
//        public async task updateasync_shouldmodifyelement()
//        {
//            var service = createservice();
//            var bus = bus.createnew();
//            await service.createasync(bus); // спочатку додаємо

//            // змінюємо дані
//            bus.model = "updated bus model";

//            var updateresult = await service.updateasync(bus);
//            var updatedbus = await service.readasync(bus.id);

//            assert.true(updateresult);
//            assert.equal("updated bus model", updatedbus.model); // перевіряємо, чи змінилася модель
//        }

//        [fact]
//        public async task removeasync_shoulddeleteelement()
//        {
//            var service = createservice();
//            var bus = bus.createnew();
//            await service.createasync(bus); // додаємо

//            var removeresult = await service.removeasync(bus); // видаляємо
//            var readbus = await service.readasync(bus.id); // спробуємо знайти

//            assert.true(removeresult);
//            assert.null(readbus); // об'єкт має бути null, бо ми його видалили
//        }

//        [fact]
//        public async task readallasync_pagination_shouldreturncorrectamount()
//        {
//            var service = createservice();

//            // додаємо 10 автобусів
//            for (int i = 0; i < 10; i++)
//            {
//                await service.createasync(bus.createnew());
//            }

//            // отримуємо 2-гу сторінку, по 3 елементи на сторінці (має повернути 3 елементи)
//            var pagedresult = await service.readallasync(page: 2, amount: 3);

//            assert.equal(3, pagedresult.count());
//        }
//    }
//}