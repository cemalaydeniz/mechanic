using System.Windows;

using Mechanic.Models;
using Mechanic.Views;
using Mechanic.ViewModels;
using Mechanic.Services;


namespace Mechanic.Commands.Services
{
    public class DeleteVehicleCommand : CommandBase
    {
        public override void Execute(object? parameter)
        {
            // Parameters check to see if they are correct
            if (parameter is not object[])
                return;

            object[] parameters = (object[])parameter;
            if (parameters.Length != 2)
                return;

            ServicesWindow servicesWindow = (ServicesWindow)parameters[0];
            Service service = (Service)parameters[1];
            if (servicesWindow == null || service == null)
                return;

            // Show a dialog message for confirmation
            DialogWindow dialog = new DialogWindow("Are you sure you want to delete the vehicle and the related data?");
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();

            if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
            {
                // Delete from the database
                ServiceSingleton.Instance.DeleteVehicle(service.Vehicle.Id);

                // Delete from the memory
                ServiceSingleton.Instance.AllServices?.RemoveAll(x => x.Vehicle.Id == service.Vehicle.Id);
                ServiceSingleton.Instance.LastSearchResult?.RemoveAll(x => x.Vehicle.Id == service.Vehicle.Id);

                service.Vehicle.Customer?.Vehicles.Remove(service.Vehicle);

                // After the deletion, the current page might not be available. So, try to give the same value again to clamp
                ServicesViewModel viewModel = (ServicesViewModel)servicesWindow.DataContext;
                viewModel.UpdatePage(viewModel.CurrentPage);
                viewModel.RefreshListView();
            }
        }
    }
}
