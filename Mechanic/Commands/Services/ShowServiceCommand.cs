using System.Windows;
using System.Linq;

using Mechanic.Services;
using Mechanic.Models;
using Mechanic.Views;
using Mechanic.ViewModels;


namespace Mechanic.Commands.Services
{
    public class ShowServiceCommand : CommandBase
    {
        public override void Execute(object? parameter)
        {
            if (parameter is not int)
                return;

            Service? service = ServiceSingleton.Instance.AllServices?.FirstOrDefault(x => x.Id == (int)parameter);
            if (service == null)
                return;

            // Create a Edit Service Window and fill the form if the service exists
            EditServiceWindow editWindow = new EditServiceWindow(true);
            editWindow.Owner = Application.Current.MainWindow;
            ((EditServiceViewModel)editWindow.DataContext).FillForm(service);
            editWindow.Show();
        }
    }
}
