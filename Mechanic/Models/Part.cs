using System;
using System.Collections.Generic;

namespace Mechanic.Models
{
    public partial class Part
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int NumberUsed { get; set; }
        public decimal Price { get; set; }
        public int ServiceId { get; set; }

        public virtual Service Service { get; set; } = null!;
    }
}
