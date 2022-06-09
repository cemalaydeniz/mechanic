using System;
using System.Collections.Generic;

namespace Mechanic.Models
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            Services = new HashSet<Service>();
        }

        public int Id { get; set; }
        public string LicensePlate { get; set; } = null!;
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int Year { get; set; }
        public string Color { get; set; } = null!;
        public int? CustomerId { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
