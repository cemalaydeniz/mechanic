using System.ComponentModel;
using System.Windows;

using Mechanic.ViewModels;
using Mechanic.Views;


namespace Mechanic.Commands.NewService
{
    public class CreateServiceCommand : CommandBase
    {
        private NewServiceViewModel viewModel;


        public CreateServiceCommand(NewServiceViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.viewModel.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.LicensePlate))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(viewModel.LicensePlate);
        }

        public override void Execute(object? parameter)
        {
            // Parameters check to see if they are correct
            if (parameter is not object[])
                return;

            object[] parameters = (object[])parameter;
            if (parameters.Length != 2)
                return;

            NewServiceWindow newServiceWindow = (NewServiceWindow)parameters[0];
            string licensePlate = (string)parameters[1];
            if (newServiceWindow == null || licensePlate == null)
                return;

            // Create a Edit Service Window and fill the form if the vehicle already exists
            EditServiceWindow editWindow = new EditServiceWindow(false);
            editWindow.Owner = Application.Current.MainWindow;
            ((EditServiceViewModel)editWindow.DataContext).FillForm(licensePlate);
            editWindow.Show();

            newServiceWindow.Close();
        }
    }
}
