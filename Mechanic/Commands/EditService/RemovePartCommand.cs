using Mechanic.ViewModels;


namespace Mechanic.Commands.EditService
{
    public class RemovePartCommand : CommandBase
    {
        private EditServiceViewModel viewModel;


        public RemovePartCommand(EditServiceViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is not int)
                return;

            viewModel.RemovePart((int)parameter);
        }
    }
}
