using System.Windows;


namespace Mechanic.Commands.Dialog
{
    public class YesCommand : CommandBase
    {
        public override void Execute(object? parameter)
        {
            if (parameter is not Window)
                return;

            Window window = (Window)parameter;
            window.DialogResult = true;
            window.Close();
        }
    }
}
