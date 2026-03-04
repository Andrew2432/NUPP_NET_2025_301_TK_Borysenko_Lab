using System;
using University.Common;

namespace University.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Налаштування для коректного виводу українських літер
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Демонстрація ООП моделі 'Університет' ===\n");

            Student student1 = new Student("Олег", "Іванов", "КБ-21");
            Student student2 = new Student("Анна", "Петрова", "КБ-21");
            Teacher teacher1 = new Teacher("Марія", "Сидорова", "Програмування на C#");

            // Підписка на події
            student1.OnAction += ActionNotification;
            student2.OnAction += ActionNotification;
            teacher1.OnAction += ActionNotification;

            // Виклик методів (перевірка подій)
            student1.Study();
            teacher1.Teach();

            Console.WriteLine($"\nУсього студентів створено (статичне поле): {Student.TotalStudents}");

            string rawString = "основи ПРОГРАМУВАННЯ";
            Console.WriteLine($"Сирий рядок: {rawString}");
            Console.WriteLine($"Відформатований рядок (метод розширення): {rawString.ToTitleCase()}\n");

            Console.WriteLine("=== Демонстрація CRUD сервісу ===");

            ICrudService<Student> studentService = new CrudService<Student>();

            studentService.Create(student1);
            studentService.Create(student2);

            Console.WriteLine("\nПоточний список студентів у сервісі:");
            foreach (var s in studentService.ReadAll())
            {
                s.DisplayInfo();
            }

            Console.WriteLine("\nОновлення даних першого студента...");
            studentService.Update(0, new Student("Олег", "Смірнов", "КБ-21"));

            Console.WriteLine("\n=== Демонстрація збереження та завантаження (JSON) ===");
            string filePath = "students_data.json";

            studentService.Save(filePath);

            ICrudService<Student> newService = new CrudService<Student>();
            newService.Load(filePath);

            Console.WriteLine("\nСтуденти, завантажені з файлу:");
            foreach (var s in newService.ReadAll())
            {
                s.DisplayInfo();
            }

            Console.ReadLine();
        }

        static void ActionNotification(string message)
        {
            Console.WriteLine($"[СПОВІЩЕННЯ]: {message}");
        }
    }
}