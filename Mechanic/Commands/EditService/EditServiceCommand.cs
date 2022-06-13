using Mechanic.ViewModels;


namespace Mechanic.Commands.EditService
{
    public class EditServiceCommand : CommandBase
    {
        private EditServiceViewModel viewModel;


        public EditServiceCommand(EditServiceViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            viewModel.IsReadOnly = false;
        }
    }
}
