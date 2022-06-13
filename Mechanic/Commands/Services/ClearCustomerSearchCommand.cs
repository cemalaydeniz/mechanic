using System.ComponentModel;

using Mechanic.ViewModels;


namespace Mechanic.Commands.Services
{
    public class ClearCustomerSearchCommand : CommandBase
    {
        private ServicesViewModel viewModel;


        public ClearCustomerSearchCommand(ServicesViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.viewModel.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.CustomerName))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(viewModel.CustomerName);
        }

        public override void Execute(object? parameter)
        {
            viewModel.CustomerName = null;
        }
    }
}
