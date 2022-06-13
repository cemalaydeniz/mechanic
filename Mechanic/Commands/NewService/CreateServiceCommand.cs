using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (parameter is not NewServiceWindow)
                return;

            EditServiceWindow editWindow = new EditServiceWindow();
            editWindow.Owner = Application.Current.MainWindow;
            editWindow.Show();

            ((NewServiceWindow)parameter).Close();
        }
    }
}
