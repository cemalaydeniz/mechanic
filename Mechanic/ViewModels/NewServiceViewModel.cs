using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Mechanic.Commands.NewService;


namespace Mechanic.ViewModels
{
    public class NewServiceViewModel : ViewModelBase
    {
        private string? licensePlate;
        public string? LicensePlate
        {
            get => licensePlate;
            set
            {
                licensePlate = value;
                OnPropertyChanged(nameof(LicensePlate));
            }
        }

        public ICommand CreateServiceCommand { get; }


        public NewServiceViewModel()
        {
            CreateServiceCommand = new CreateServiceCommand(this);
        }
    }
}
