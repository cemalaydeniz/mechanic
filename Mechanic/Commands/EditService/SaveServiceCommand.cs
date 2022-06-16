using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Mechanic.ViewModels;
using Mechanic.Services;
using Mechanic.Models;
using Mechanic.Views;


namespace Mechanic.Commands.EditService
{
    public class SaveServiceCommand : CommandBase
    {
        private EditServiceViewModel viewModel;

        private Regex moneyRegex = new Regex("^\\d+[,]\\d\\d$");


        public SaveServiceCommand(EditServiceViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.viewModel.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.VehicleLicensePlate) ||
                e.PropertyName == nameof(viewModel.VehicleMake) ||
                e.PropertyName == nameof(viewModel.VehicleModel) ||
                e.PropertyName == nameof(viewModel.VehicleYear) ||
                e.PropertyName == nameof(viewModel.VehicleColor) ||
                e.PropertyName == nameof(viewModel.ServiceFee) ||
                e.PropertyName == nameof(viewModel.ServiceParts) ||
                e.PropertyName == nameof(viewModel.IsReadOnly))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            // Check if the part data are entered correctly
            bool arePartDateOkay = true;
            foreach (var part in viewModel.ServiceParts)
            {
                if (string.IsNullOrEmpty(part.Name) || part.NumberUsed <= 0 || string.IsNullOrEmpty(part.Price) || !moneyRegex.IsMatch(part.Price))
                {
                    arePartDateOkay = false;
                    break;
                }
            }

            return !string.IsNullOrEmpty(viewModel.VehicleLicensePlate) &&
                !string.IsNullOrEmpty(viewModel.VehicleMake) &&
                !string.IsNullOrEmpty(viewModel.VehicleModel) &&
                viewModel.VehicleYear > 0 &&
                !string.IsNullOrEmpty(viewModel.VehicleColor) &&
                !string.IsNullOrEmpty(viewModel.ServiceFee) && moneyRegex.IsMatch(viewModel.ServiceFee) &&
                arePartDateOkay &&
                !viewModel.IsReadOnly;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is not EditServiceWindow)
                return;

            Customer? customerData;
            Vehicle? vehicleData;
            Service? serviceData;

            // Get the latest data from the database. There can be more than one new/edit service windows and they can be saved at different times
            List<Service>? allServices = ServiceSingleton.Instance.GetAllServices();

            if (viewModel.ServiceID == null)
            {
                // Fill the service data
                serviceData = new Service()
                {
                    Details = viewModel.ServiceDetails,
                    EnterDate = DateOnly.FromDateTime(viewModel.ServiceEnterDate),
                    ExitDate = viewModel.IsFinished && viewModel.ServiceExitDate != null ? DateOnly.FromDateTime(viewModel.ServiceExitDate.Value) : null,
                    Fee = decimal.Parse(viewModel.ServiceFee)
                };

                foreach (var part in viewModel.ServiceParts)
                {
                    serviceData.Parts.Add(new Part()
                    {
                        Name = part.Name,
                        NumberUsed = part.NumberUsed,
                        Price = decimal.Parse(part.Price)
                    });
                }

                // Check if the vehicle is already exist. If not, create a new vehicle on the database
                vehicleData = allServices?.Where(x => x.Vehicle.LicensePlate == viewModel.VehicleLicensePlate).FirstOrDefault()?.Vehicle;
                if (vehicleData == null)
                {
                    vehicleData = new Vehicle()
                    {
                        LicensePlate = viewModel.VehicleLicensePlate,
                        Make = viewModel.VehicleMake,
                        Model = viewModel.VehicleModel,
                        Year = viewModel.VehicleYear,
                        Color = viewModel.VehicleColor,
                    };

                    // Check if the customer data is entered. If not, add the vehicle to the database directly with its related service data
                    if (string.IsNullOrEmpty(viewModel.CustomerName))
                    {
                        vehicleData.Services.Add(serviceData);
                        ServiceSingleton.Instance.AddVehicle(vehicleData);
                    }
                    else
                    {
                        // Check if the customer is already exist. If not, create a new customer and add the customer to the database with its related service data
                        customerData = allServices?.Where(x => x.Vehicle.Customer != null && x.Vehicle.Customer.Name == viewModel.CustomerName).FirstOrDefault()?.Vehicle.Customer;
                        if (customerData == null)
                        {
                            customerData = new Customer()
                            {
                                Name = viewModel.CustomerName,
                                Contact = viewModel.CustomerContact
                            };

                            customerData.Vehicles.Add(vehicleData);
                            vehicleData.Services.Add(serviceData);
                            ServiceSingleton.Instance.AddCustomer(customerData);
                        }
                        else
                        {
                            // Add the customer and the service data to the vehicle and save on the database
                            vehicleData.CustomerId = customerData.Id;
                            vehicleData.Services.Add(serviceData);
                            ServiceSingleton.Instance.AddVehicle(vehicleData);

                            // In case the customer data is updated, update the customer data on the database
                            customerData.Name = viewModel.CustomerName;
                            customerData.Contact = viewModel.CustomerContact;
                            ServiceSingleton.Instance.UpdateCustomer(customerData);
                        }
                    }
                }
                else
                {
                    // In case the vehicle data is updated, update the vehicle data
                    vehicleData.LicensePlate = viewModel.VehicleLicensePlate;
                    vehicleData.Make = viewModel.VehicleMake;
                    vehicleData.Model = viewModel.VehicleModel;
                    vehicleData.Year = viewModel.VehicleYear;
                    vehicleData.Color = viewModel.VehicleColor;

                    // Check if the customer data is entered. If not, update the vehicle on the database and add a new service data
                    if (string.IsNullOrEmpty(viewModel.CustomerName))
                    {
                        serviceData.VehicleId = vehicleData.Id;
                        ServiceSingleton.Instance.AddService(serviceData);
                        ServiceSingleton.Instance.UpdateVehicle(vehicleData);
                    }
                    else
                    {
                        // Check if the customer is already exist. If not, create a new customer and add the customer to the database with its related service data
                        customerData = allServices?.Where(x => x.Vehicle.Customer != null && x.Vehicle.Customer.Name == viewModel.CustomerName).FirstOrDefault()?.Vehicle.Customer;
                        if (customerData == null)
                        {
                            customerData = new Customer()
                            {
                                Name = viewModel.CustomerName,
                                Contact = viewModel.CustomerContact
                            };

                            int customerId = ServiceSingleton.Instance.AddCustomer(customerData);

                            vehicleData.CustomerId = customerId;
                            serviceData.VehicleId = vehicleData.Id;

                            ServiceSingleton.Instance.UpdateVehicle(vehicleData);
                            ServiceSingleton.Instance.AddService(serviceData);
                        }
                        else
                        {
                            // In case the customer data is updated, update the customer data on the database
                            customerData.Name = viewModel.CustomerName;
                            customerData.Contact = viewModel.CustomerContact;

                            // Add the customer and the service data to the vehicle and save on the database
                            vehicleData.CustomerId = customerData.Id;
                            serviceData.VehicleId = vehicleData.Id;

                            ServiceSingleton.Instance.UpdateCustomer(customerData);
                            ServiceSingleton.Instance.UpdateVehicle(vehicleData);
                            ServiceSingleton.Instance.AddService(serviceData);
                        }
                    }
                }
            }
            else
            {
                // Fill the service data
                serviceData = allServices?.Where(x => x.Id == viewModel.ServiceID).FirstOrDefault();
                if (serviceData != null)
                {
                    serviceData.Details = viewModel.ServiceDetails;
                    serviceData.EnterDate = DateOnly.FromDateTime(viewModel.ServiceEnterDate);
                    serviceData.ExitDate = viewModel.IsFinished && viewModel.ServiceExitDate != null ? DateOnly.FromDateTime(viewModel.ServiceExitDate.Value) : null;
                    serviceData.Fee = decimal.Parse(viewModel.ServiceFee);

                    // Remove all the parts to update the current service's parts
                    serviceData.Parts.Clear();
                    ServiceSingleton.Instance.DeleteParts(viewModel.ServiceID.Value);

                    // Add the new parts to the service
                    List<Part> parts = new List<Part>();
                    foreach (var part in viewModel.ServiceParts)
                    {
                        parts.Add(new Part()
                        {
                            Name = part.Name,
                            NumberUsed = part.NumberUsed,
                            Price = decimal.Parse(part.Price)
                        });
                    }

                    ServiceSingleton.Instance.AddParts(viewModel.ServiceID.Value, parts);

                    // Check if the vehicle is already exist. If not, create a new vehicle on the database
                    vehicleData = allServices?.Where(x => x.Vehicle.LicensePlate == viewModel.VehicleLicensePlate).FirstOrDefault()?.Vehicle;
                    if (vehicleData == null)
                    {
                        vehicleData = new Vehicle()
                        {
                            LicensePlate = viewModel.VehicleLicensePlate,
                            Make = viewModel.VehicleMake,
                            Model = viewModel.VehicleModel,
                            Year = viewModel.VehicleYear,
                            Color = viewModel.VehicleColor,
                        };

                        // Check if the customer data is entered. If not, add the vehicle to the database directly and update the service data
                        if (string.IsNullOrEmpty(viewModel.CustomerName))
                        {
                            int vehicleId = ServiceSingleton.Instance.AddVehicle(vehicleData);

                            serviceData.VehicleId = vehicleId;
                            ServiceSingleton.Instance.UpdateService(serviceData);
                        }
                        else
                        {
                            // Check if the customer is already exist. If not, create a new customer and add the customer to the database and update the service data
                            customerData = allServices?.Where(x => x.Vehicle.Customer != null && x.Vehicle.Customer.Name == viewModel.CustomerName).FirstOrDefault()?.Vehicle.Customer;
                            if (customerData == null)
                            {
                                customerData = new Customer()
                                {
                                    Name = viewModel.CustomerName,
                                    Contact = viewModel.CustomerContact
                                };

                                int customerId = ServiceSingleton.Instance.AddCustomer(customerData);

                                vehicleData.CustomerId = customerId;
                                int vehicleId = ServiceSingleton.Instance.AddVehicle(vehicleData);

                                serviceData.VehicleId = vehicleId;
                                ServiceSingleton.Instance.UpdateService(serviceData);
                            }
                            else
                            {
                                // Add the customer data to the vehicle and save on the database
                                vehicleData.CustomerId = customerData.Id;
                                int vehicleId = ServiceSingleton.Instance.AddVehicle(vehicleData);

                                // Update the service data with the new vehicle ID
                                serviceData.VehicleId = vehicleId;
                                ServiceSingleton.Instance.UpdateService(serviceData);

                                // In case the customer data is updated, update the customer data on the database
                                customerData.Name = viewModel.CustomerName;
                                customerData.Contact = viewModel.CustomerContact;
                                ServiceSingleton.Instance.UpdateCustomer(customerData);
                            }
                        }
                    }
                    else
                    {
                        // In case the vehicle data is updated, update the vehicle data
                        vehicleData.LicensePlate = viewModel.VehicleLicensePlate;
                        vehicleData.Make = viewModel.VehicleMake;
                        vehicleData.Model = viewModel.VehicleModel;
                        vehicleData.Year = viewModel.VehicleYear;
                        vehicleData.Color = viewModel.VehicleColor;

                        // Check if the customer data is entered. If not, update the vehicle and the service data on the database
                        if (string.IsNullOrEmpty(viewModel.CustomerName))
                        {
                            serviceData.Vehicle = null!;
                            serviceData.VehicleId = vehicleData.Id;
                            ServiceSingleton.Instance.UpdateService(serviceData);
                            ServiceSingleton.Instance.UpdateVehicle(vehicleData);
                        }
                        else
                        {
                            // Check if the customer is already exist. If not, create a new customer and add the customer to the database update the service data
                            customerData = allServices?.Where(x => x.Vehicle.Customer != null && x.Vehicle.Customer.Name == viewModel.CustomerName).FirstOrDefault()?.Vehicle.Customer;
                            if (customerData == null)
                            {
                                customerData = new Customer()
                                {
                                    Name = viewModel.CustomerName,
                                    Contact = viewModel.CustomerContact
                                };

                                int customerId = ServiceSingleton.Instance.AddCustomer(customerData);

                                vehicleData.CustomerId = customerId;
                                ServiceSingleton.Instance.UpdateVehicle(vehicleData);

                                serviceData.VehicleId = vehicleData.Id;
                                ServiceSingleton.Instance.UpdateService(serviceData);
                            }
                            else
                            {
                                // In case the customer data is updated, update the customer data on the database
                                customerData.Name = viewModel.CustomerName;
                                customerData.Contact = viewModel.CustomerContact;

                                // Add the customer and the service data to the vehicle and save on the database
                                vehicleData.CustomerId = customerData.Id;
                                serviceData.VehicleId = vehicleData.Id;

                                ServiceSingleton.Instance.UpdateCustomer(customerData);
                                ServiceSingleton.Instance.UpdateVehicle(vehicleData);
                                ServiceSingleton.Instance.UpdateService(serviceData);
                            }
                        }
                    }
                }
            }

            EditServiceWindow editWindow = (EditServiceWindow)parameter;
            ((ServicesViewModel)editWindow.Owner.DataContext).RefreshListView();
            editWindow.Close();
        }
    }
}
