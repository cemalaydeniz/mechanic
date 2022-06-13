namespace Mechanic.ViewModels.Helpers
{
    public class PartViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public int NumberUsed { get; set; } = 1;
        public string Price { get; set; } = null!;


        public PartViewModel(int id)
        {
            ID = id;
        }
    }
}
