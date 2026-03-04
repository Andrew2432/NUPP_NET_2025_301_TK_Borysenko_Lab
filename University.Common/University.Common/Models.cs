using System;

namespace University.Common
{
    // 1. Делегат
    public delegate void PersonActionHandler(string message);

    // 2. Базовий абстрактний клас
    public abstract class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // 3. Подія
        public event PersonActionHandler OnAction;

        // 4. Конструктор
        public Person(string firstName, string lastName)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
        }

        // 5. Віртуальний метод
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Особа: {FirstName} {LastName}");
        }

        protected void TriggerAction(string action)
        {
            OnAction?.Invoke($"{FirstName} {LastName} {action}");
        }
    }

    // Наслідування 1
    public class Student : Person
    {
        // 6. Статичне поле
        public static int TotalStudents = 0;

        public string StudentIdNumber { get; set; }
        public double AverageGrade { get; set; }

        // 7. Статичний конструктор
        static Student()
        {
            TotalStudents = 0;
        }

        public Student(string firstName, string lastName, string studentId)
            : base(firstName, lastName)
        {
            StudentIdNumber = studentId;
            AverageGrade = 0.0;
            TotalStudents++;
        }

        public void Study()
        {
            TriggerAction("навчається.");
        }

        // Перевизначення методу
        public override void DisplayInfo()
        {
            Console.WriteLine($"Студент: {FirstName} {LastName}, Група: {StudentIdNumber}");
        }
    }

    // Наслідування 2
    public class Teacher : Person
    {
        public string Subject { get; set; }
        public int ExperienceYears { get; set; }

        public Teacher(string firstName, string lastName, string subject)
            : base(firstName, lastName)
        {
            Subject = subject;
        }

        public void Teach()
        {
            TriggerAction($"викладає предмет: {Subject}.");
        }
    }

    // Клас без наслідування (для кількості)
    public class Course
    {
        public Guid Id { get; set; }
        public string CourseName { get; set; }
        public int Credits { get; set; }

        public Course(string courseName, int credits)
        {
            Id = Guid.NewGuid();
            CourseName = courseName;
            Credits = credits;
        }
    }

    // 8. Метод розширення
    public static class StringExtensions
    {
        public static string ToTitleCase(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }
    }
}