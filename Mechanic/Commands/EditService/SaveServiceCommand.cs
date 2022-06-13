using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Mechanic.ViewModels;


namespace Mechanic.Commands.EditService
{
    public class SaveServiceCommand : CommandBase
    {
        private EditServiceViewModel viewModel;

        private Regex moneyRegex = new Regex("^\\d+[,]\\d\\d$");


        public SaveServiceCommand(EditServiceViewModel viewModel)
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
                e.PropertyName == nameof(viewModel.Color) ||
                e.PropertyName == nameof(viewModel.ServiceFee) ||
                e.PropertyName == nameof(viewModel.IsReadOnly))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(viewModel.LicensePlate) &&
                !string.IsNullOrEmpty(viewModel.Make) &&
                !string.IsNullOrEmpty(viewModel.Model) &&
                viewModel.Year > 0 &&
                !string.IsNullOrEmpty(viewModel.Color) &&
                !string.IsNullOrEmpty(viewModel.ServiceFee) && moneyRegex.IsMatch(viewModel.ServiceFee) &&
                !viewModel.IsReadOnly;
        }

        public override void Execute(object? parameter)
        {
            // TODO: Save db to related data
        }
    }
}
