using Mechanic.ViewModels;


namespace Mechanic.Commands.EditService
{
    public class AddPartCommand : CommandBase
    {
        private EditServiceViewModel viewModel;


        public AddPartCommand(EditServiceViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            viewModel.AddNewPart();
        }
    }
}
