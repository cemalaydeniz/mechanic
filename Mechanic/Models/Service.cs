using System;
using System.Collections.Generic;

namespace Mechanic.Models
{
    public partial class Service
    {
        public Service()
        {
            Parts = new HashSet<Part>();
        }

        public int Id { get; set; }
        public string? Details { get; set; }
        public decimal Fee { get; set; }
        public DateOnly EnterDate { get; set; }
        public DateOnly? ExitDate { get; set; }
        public int VehicleId { get; set; }

        public virtual Vehicle Vehicle { get; set; } = null!;
        public virtual ICollection<Part> Parts { get; set; }
    }
}
