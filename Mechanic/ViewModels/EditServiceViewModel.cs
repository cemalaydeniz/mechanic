using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Mechanic.Services;
using Mechanic.Models;
using Mechanic.Commands.EditService;
using Mechanic.ViewModels.Helpers;
using System.Collections.ObjectModel;

namespace Mechanic.ViewModels
{
    public class EditServiceViewModel : ViewModelBase
    {
        //~ Begin - Vehicle data
        private string licensePlate = null!;
        public string LicensePlate
        {
            get => licensePlate;
            set
            {
                licensePlate = value;
                OnPropertyChanged(nameof(LicensePlate));
            }
        }

        private string make = null!;
        public string Make
        {
            get => make;
            set
            {
                make = value;
                OnPropertyChanged(nameof(Make));
            }
        }

        private string model = null!;
        public string Model
        {
            get => model;
            set
            {
                model = value;
                OnPropertyChanged(nameof(Model));
            }
        }

        private int year;
        public int Year
        {
            get => year;
            set
            {
                year = value;
                OnPropertyChanged(nameof(Year));
            }
        }

        private string color = null!;
        public string Color
        {
            get => color;
            set
            {
                color = value;
                OnPropertyChanged(nameof(Color));
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

        private ObservableCollection<PartViewModel> parts = new ObservableCollection<PartViewModel>();
        public ObservableCollection<PartViewModel> Parts
        {
            get => parts;
            set
            {
                parts = value;
                OnPropertyChanged(nameof(Parts));
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

        private DateTime enterDate = DateTime.Now;
        public DateTime EnterDate
        {
            get => enterDate;
            set
            {
                enterDate = value;
                OnPropertyChanged(nameof(EnterDate));
            }
        }

        private DateTime? exitDate;
        public DateTime? ExitDate
        {
            get => exitDate;
            set
            {
                exitDate = value;
                OnPropertyChanged(nameof(ExitDate));
            }
        }

        private bool isFinished;
        public bool IsFinished
        {
            get => isFinished;
            set
            {
                isFinished = value;
                OnPropertyChanged(nameof(IsFinished));
            }
        }
        //~ End

        //~ Begin - Button bindings
        public ICommand AddPartCommand { get; }
        public ICommand RemovePartCommand { get; }

        public ICommand SaveServiceCommand { get; }
        //~ End

        // The next ID of a new part
        private int nextIDPart = 0;


        public EditServiceViewModel()
        {
            AddPartCommand = new AddPartCommand(this);
            RemovePartCommand = new RemovePartCommand(this);

            SaveServiceCommand = new SaveServiceCommand(this);
        }

        /// <summary>
        /// Fills the text fields if a vehicle with the license place exists
        /// </summary>
        /// <param name="licensePlate">The license plate of the vehicle</param>
        public void FillForm(string licensePlate)
        {
            Vehicle? vehicle = ServiceSingleton.Instance.GetVehicle(licensePlate);
            if (vehicle == null)
            {
                LicensePlate = licensePlate;
                return;
            }

            LicensePlate = vehicle.LicensePlate;
            Make = vehicle.Make;
            Model = vehicle.Model;
            Year = vehicle.Year;
            Color = vehicle.Color;

            CustomerName = vehicle.Customer?.Name;
            CustomerContact = vehicle.Customer?.Contact;
        }

        /// <summary>
        /// Adds new part to the service
        /// </summary>
        public void AddNewPart()
        {
            parts.Add(new PartViewModel(nextIDPart));
            nextIDPart++;
        }

        /// <summary>
        /// Removes the existing part in the service
        /// </summary>
        /// <param name="id">The ID of the part</param>
        public void RemovePart(int id)
        {
            PartViewModel? part = parts.Where(x => x.ID == id).FirstOrDefault();
            if (part == null)
                return;

            parts.Remove(part);
        }
    }
}
