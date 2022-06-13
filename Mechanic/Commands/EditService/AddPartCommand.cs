using System.ComponentModel;

using Mechanic.ViewModels;


namespace Mechanic.Commands.EditService
{
    public class AddPartCommand : CommandBase
    {
        private EditServiceViewModel viewModel;


        public AddPartCommand(EditServiceViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.viewModel.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.IsReadOnly))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !viewModel.IsReadOnly;
        }

        public override void Execute(object? parameter)
        {
            viewModel.AddNewPart();
        }
    }
}
