using System.Windows;

using Mechanic.Models;
using Mechanic.Views;
using Mechanic.ViewModels;


namespace Mechanic.Commands.Services
{
    public class EditServiceCommand : CommandBase
    {
        public override void Execute(object? parameter)
        {
            if (parameter is not Service)
                return;

            // Create a Edit Service Window and fill the form if the service exists
            EditServiceWindow editWindow = new EditServiceWindow("Edit Service", false);
            editWindow.Owner = Application.Current.MainWindow;
            ((EditServiceViewModel)editWindow.DataContext).FillForm((Service)parameter);
            editWindow.Show();
        }
    }
}
