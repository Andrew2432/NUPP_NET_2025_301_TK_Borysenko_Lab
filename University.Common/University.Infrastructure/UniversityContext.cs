using Microsoft.EntityFrameworkCore;
using University.Infrastructure.Models; // Підключаємо наші моделі

namespace University.Infrastructure
{
    public class UniversityContext : DbContext
    {
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