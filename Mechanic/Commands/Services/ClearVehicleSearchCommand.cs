using System.ComponentModel;

using Mechanic.ViewModels;


namespace Mechanic.Commands.Services
{
    public class ClearVehicleSearchCommand : CommandBase
    {
        private ServicesViewModel viewModel;


        public ClearVehicleSearchCommand(ServicesViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.viewModel.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.LicensePlate) ||
                e.PropertyName == nameof(viewModel.Make) ||
                e.PropertyName == nameof(viewModel.Model) ||
                e.PropertyName == nameof(viewModel.Year) ||
                e.PropertyName == nameof(viewModel.Color))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            // If there's any text in the 'Search by Vehicle' group box, the clear button will be actived
            return !string.IsNullOrEmpty(viewModel.LicensePlate) ||
                !string.IsNullOrEmpty(viewModel.Make) ||
                !string.IsNullOrEmpty(viewModel.Model) ||
                viewModel.Year.HasValue ||
                !string.IsNullOrEmpty(viewModel.Color);
        }

        public override void Execute(object? parameter)
        {
            viewModel.LicensePlate = null;
            viewModel.Make = null;
            viewModel.Model = null;
            viewModel.Year = null;
            viewModel.Color = null;
        }
    }
}
