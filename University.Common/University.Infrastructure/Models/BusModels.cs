using System;
using System.Collections.Generic;

namespace University.Infrastructure.Models
{
    public abstract class VehicleModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Brand { get; set; } = string.Empty;
    }

    public class BusModel : VehicleModel
    {
        public string ModelName { get; set; } = string.Empty;
        public int Speed { get; set; }
        public int Capacity { get; set; }

        public DriverModel? Driver { get; set; }

        public List<RouteModel> Routes { get; set; } = new();

        public List<StationModel> Stations { get; set; } = new();
    }

    public class DriverModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;

        public Guid BusModelId { get; set; }
        public BusModel? Bus { get; set; }
    }

    public class RouteModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string RouteName { get; set; } = string.Empty;

        public Guid BusModelId { get; set; }
        public BusModel? Bus { get; set; }
    }

    public class StationModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string StationName { get; set; } = string.Empty;

        public List<BusModel> Buses { get; set; } = new();
    }
}