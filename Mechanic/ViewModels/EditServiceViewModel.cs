using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Collections.Specialized;
using System.ComponentModel;

using Mechanic.Services;
using Mechanic.Models;
using Mechanic.Commands.EditService;
using Mechanic.ViewModels.Helpers;


namespace Mechanic.ViewModels
{
    public class EditServiceViewModel : ViewModelBase
    {
        //~ Begin - Vehicle data
        private string vehicleLicensePlate = null!;
        public string VehicleLicensePlate
        {
            get => vehicleLicensePlate;
            set
            {
                vehicleLicensePlate = value;
                OnPropertyChanged(nameof(VehicleLicensePlate));
            }
        }

        private string vehicleMake = null!;
        public string VehicleMake
        {
            get => vehicleMake;
            set
            {
                vehicleMake = value;
                OnPropertyChanged(nameof(VehicleMake));
            }
        }

        private string vehicleModel = null!;
        public string VehicleModel
        {
            get => vehicleModel;
            set
            {
                vehicleModel = value;
                OnPropertyChanged(nameof(VehicleModel));
            }
        }

        private int vehicleYear;
        public int VehicleYear
        {
            get => vehicleYear;
            set
            {
                vehicleYear = value;
                OnPropertyChanged(nameof(VehicleYear));
            }
        }

        private string vehicleColor = null!;
        public string VehicleColor
        {
            get => vehicleColor;
            set
            {
                vehicleColor = value;
                OnPropertyChanged(nameof(VehicleColor));
            }
        }
        //~ End

        //~ Begin - Customer data
        private string? customerName;
        public string? CustomerName
        {
            get => customerName;
            set
            {
                customerName = value;
                OnPropertyChanged(nameof(CustomerName));
            }
        }

        private string? customerContact;
        public string? CustomerContact
        {
            get => customerContact;
            set
            {
                customerContact = value;
                OnPropertyChanged(nameof(CustomerContact));
            }
        }
        //~ End

        //~ Begin - Service data
        private string? serviceDetails;
        public string? ServiceDetails
        {
            get => serviceDetails;
            set
            {
                serviceDetails = value;
                OnPropertyChanged(nameof(ServiceDetails));
            }
        }

        private ObservableCollection<PartViewModel> serviceParts = new ObservableCollection<PartViewModel>();
        public ObservableCollection<PartViewModel> ServiceParts
        {
            get => serviceParts;
            private set
            {
                serviceParts = value;
                OnPropertyChanged(nameof(ServiceParts));
            }
        }

        private string serviceFee = null!;
        public string ServiceFee
        {
            get => serviceFee;
            set
            {
                serviceFee = value;
                OnPropertyChanged(nameof(ServiceFee));
            }
        }

        private DateTime serviceEnterDate;
        public DateTime ServiceEnterDate
        {
            get => serviceEnterDate;
            set
            {
                serviceEnterDate = value;
                OnPropertyChanged(nameof(ServiceEnterDate));
            }
        }

        private DateTime? serviceExitDate;
        public DateTime? ServiceExitDate
        {
            get => serviceExitDate;
            set
            {
                serviceExitDate = value;
                OnPropertyChanged(nameof(ServiceExitDate));
            }
        }

        private bool isFinished;
        public bool IsFinished
        {
            get => isFinished;
            set
            {
                if (!isFinished && value)
                {
                    ServiceExitDate = DateTime.Now;
                }
                else if (isFinished && !value)
                {
                    ServiceExitDate = null;
                }

                isFinished = value;
                OnPropertyChanged(nameof(IsFinished));
            }
        }
        //~ End

        private bool isReadOnly;
        public bool IsReadOnly
        {
            get => isReadOnly;
            set
            {
                isReadOnly = value;
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }

        //~ Begin - Button bindings
        public ICommand AddPartCommand { get; }
        public ICommand RemovePartCommand { get; }

        public ICommand EditServiceCommand { get; }
        public ICommand SaveServiceCommand { get; }
        //~ End

        /** Null if the service is new. Not null if the service exists already
         * Is is used to detect if the window is in edit mode */
        public int? ServiceID { get; private set; }

        // The next ID of a new part
        private int nextIDPart = 0;


        public EditServiceViewModel()
        {
            AddPartCommand = new AddPartCommand(this);
            RemovePartCommand = new RemovePartCommand(this);

            EditServiceCommand = new EditServiceCommand(this);
            SaveServiceCommand = new SaveServiceCommand(this);

            serviceParts.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (object item in e.NewItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += ItemPropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (object item in e.OldItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= ItemPropertyChanged;
                }
            }
        }

        private void ItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ServiceParts));
        }

        /// <summary>
        /// Fills the text fields if a vehicle with the license place exists. This doesn't fill any service data
        /// Use this if the service is new
        /// </summary>
        /// <param name="licensePlate">The license plate of the vehicle</param>
        public void FillForm(string licensePlate)
        {
            ServiceEnterDate = DateTime.Now;

            Vehicle? vehicle = ServiceSingleton.Instance.GetVehicle(licensePlate);
            if (vehicle == null)
            {
                VehicleLicensePlate = licensePlate;
                return;
            }

            VehicleLicensePlate = vehicle.LicensePlate;
            VehicleMake = vehicle.Make;
            VehicleModel = vehicle.Model;
            VehicleYear = vehicle.Year;
            VehicleColor = vehicle.Color;

            CustomerName = vehicle.Customer?.Name;
            CustomerContact = vehicle.Customer?.Contact;

            ServiceID = null;
        }

        /// <summary>
        /// Fills the text fields including the service data
        /// Use this if the service will be edited or shown
        /// </summary>
        /// <param name="service">The service data to fill the form</param>
        public void FillForm(Service service)
        {
            if (service == null)
                return;

            VehicleLicensePlate = service.Vehicle.LicensePlate;
            VehicleMake = service.Vehicle.Make;
            VehicleModel = service.Vehicle.Model;
            VehicleYear = service.Vehicle.Year;
            VehicleColor = service.Vehicle.Color;

            CustomerName = service.Vehicle.Customer?.Name;
            CustomerContact = service.Vehicle.Customer?.Contact;

            ServiceDetails = service.Details;
            ServiceFee = service.Fee.ToString("0.00");
            ServiceEnterDate = service.EnterDate.ToDateTime(TimeOnly.MinValue);
            
            if (service.ExitDate == null)
            {
                ServiceExitDate = null;
                IsFinished = false;
            }
            else
            {
                ServiceExitDate = service.ExitDate.Value.ToDateTime(TimeOnly.MinValue);
                IsFinished = true;
            }

            foreach (var part in service.Parts)
            {
                ServiceParts.Add(PartViewModel.Converter(nextIDPart, part));
                nextIDPart++;
            }

            ServiceID = service.Id;
        }

        /// <summary>
        /// Adds new part to the service
        /// </summary>
        public void AddNewPart()
        {
            ServiceParts.Add(new PartViewModel(nextIDPart));
            nextIDPart++;

            OnPropertyChanged(nameof(ServiceParts));
        }

        /// <summary>
        /// Removes the existing part in the service
        /// </summary>
        /// <param name="id">The ID of the part</param>
        public void RemovePart(int id)
        {
            PartViewModel? part = ServiceParts.Where(x => x.ID == id).FirstOrDefault();
            if (part == null)
                return;

            ServiceParts.Remove(part);

            OnPropertyChanged(nameof(ServiceParts));
        }
    }
}
