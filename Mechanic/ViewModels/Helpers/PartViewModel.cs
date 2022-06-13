using Mechanic.Models;


namespace Mechanic.ViewModels.Helpers
{
    public class PartViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public int NumberUsed { get; set; } = 1;
        public string Price { get; set; } = null!;


        private PartViewModel() { }

        public PartViewModel(int id)
        {
            ID = id;
        }


        public static PartViewModel Converter(int id, Part part)
        {
            PartViewModel result = new PartViewModel()
            {
                ID = id,
                Name = part.Name,
                NumberUsed = part.NumberUsed,
                Price = part.Price.ToString("0.00")
            };

            return result;
        }
    }
}
