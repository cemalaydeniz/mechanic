using System.ComponentModel;

using Mechanic.ViewModels;


namespace Mechanic.Commands.EditService
{
    public class RemovePartCommand : CommandBase
    {
        private EditServiceViewModel viewModel;


        public RemovePartCommand(EditServiceViewModel viewModel)
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
            if (parameter is not int)
                return;

            viewModel.RemovePart((int)parameter);
        }
    }
}
