using System;
using System.Collections.Generic;

namespace Mechanic.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Vehicles = new HashSet<Vehicle>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Contact { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
