using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // ДОДАНО: підключаємо Identity
using University.Common;
using University.Infrastructure.Models; // Підключаємо наші моделі

namespace University.Infrastructure
{
    // ЗМІНЕНО: тепер наслідуємося від IdentityDbContext<AppUser> замість DbContext
    public class UniversityContext : IdentityDbContext<AppUser>
    {
        public UniversityContext()
        {
        }

        // Цей конструктор потрібен для ASP.NET Core, щоб передавати налаштування
        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        // Це таблиці (DbSet), які будуть створені в нашій базі даних
        public DbSet<VehicleModel> Vehicles { get; set; }
        public DbSet<BusModel> Buses { get; set; }
        public DbSet<DriverModel> Drivers { get; set; }
        public DbSet<RouteModel> Routes { get; set; }
        public DbSet<StationModel> Stations { get; set; }

        // Налаштування підключення до БД SQLite
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Файл university.db буде автоматично створено в папці з проєктом
            optionsBuilder.UseSqlite("Data Source=university.db");
        }

        // Налаштування зв'язків за допомогою пріоритетного Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Цей рядок обов'язково має бути першим, він генерує таблиці для логінів і паролів!
            base.OnModelCreating(modelBuilder);

            // === 1. Наслідування TPT (Table-per-Type) ===
            // Базовий клас йде в одну таблицю, а спадкоємець - в іншу, пов'язану по Id
            modelBuilder.Entity<VehicleModel>().ToTable("Vehicles");
            modelBuilder.Entity<BusModel>().ToTable("Buses");

            // === 2. Зв'язок 1-до-1 (Автобус - Водій) ===
            modelBuilder.Entity<BusModel>()
                .HasOne(b => b.Driver)
                .WithOne(d => d.Bus)
                .HasForeignKey<DriverModel>(d => d.BusModelId);

            // === 3. Зв'язок 1-до-багатьох (Автобус - Маршрути) ===
            modelBuilder.Entity<BusModel>()
                .HasMany(b => b.Routes)
                .WithOne(r => r.Bus)
                .HasForeignKey(r => r.BusModelId);

            // === 4. Зв'язок Багато-до-багатьох (Автобуси - Станції) ===
            modelBuilder.Entity<BusModel>()
                .HasMany(b => b.Stations)
                .WithMany(s => s.Buses)
                .UsingEntity(j => j.ToTable("BusStationLinks")); // Створюємо проміжну таблицю
        }
    }
}