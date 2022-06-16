using System.ComponentModel;

using Mechanic.Models;


namespace Mechanic.ViewModels.Helpers
{
    public class PartViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        private int id;
        public int ID
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        private string name = null!;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private int numberUsed = 1;
        public int NumberUsed
        {
            get => numberUsed;
            set
            {
                numberUsed = value;
                OnPropertyChanged(nameof(NumberUsed));
            }
        }

        private string price = null!;
        public string Price
        {
            get => price;
            set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }


        private PartViewModel() { }

        public PartViewModel(int id)
        {
            ID = id;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
