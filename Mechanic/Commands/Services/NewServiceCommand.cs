using System.Windows;

using Mechanic.Views;


namespace Mechanic.Commands.Services
{
    public class NewServiceCommand : CommandBase
    {
        public override void Execute(object? parameter)
        {
            NewServiceWindow window = new NewServiceWindow();
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }
    }
}
