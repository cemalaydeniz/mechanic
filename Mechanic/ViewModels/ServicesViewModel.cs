using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Mechanic.Models;
using Mechanic.Commands.ServicesWindow;

using Microsoft.EntityFrameworkCore;


namespace Mechanic.ViewModels
{
    public class ServicesViewModel : ViewModelBase
    {
        private static readonly int NumofEntryToShow = 50;


        //~ Begin - Search by Vehicle bindings
        private string? licensePlate;
        public string? LicensePlate
        {
            get => licensePlate;
            set
            {
                licensePlate = value;
                OnPropertyChanged(nameof(LicensePlate));
            }
        }

        private string? make;
        public string? Make
        {
            get => make;
            set
            {
                make = value;
                OnPropertyChanged(nameof(Make));
            }
        }

        private string? model;
        public string? Model
        {
            get => model;
            set
            {
                model = value;
                OnPropertyChanged(nameof(Model));
            }
        }

        private int? year;
        public int? Year
        {
            get => year;
            set
            {
                year = value;
                OnPropertyChanged(nameof(Year));
            }
        }

        private string? color;
        public string? Color
        {
            get => color;
            set
            {
                color = value;
                OnPropertyChanged(nameof(Color));
            }
        }
        //~ End

        // Search by Customer binding
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

        // List View binding - TODO: Just to test the view, ObservableList has been converted to List
        private List<Service>? services;
        public List<Service>? Services
        {
            get => services;
            set
            {
                services = value;
                OnPropertyChanged(nameof(Services));
            }
        }

        // ~ Begin - Paging binding
        private int? currentPage;
        public int? CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        private int? numofPages;
        public int? NumofPages
        {
            get => numofPages;
            set
            {
                numofPages = value;
                OnPropertyChanged(nameof(NumofPages));
            }
        }
        //~ End

        //~ Begin - Button commands
        public ICommand ClearVehicleSearchCommand { get; }
        public ICommand ClearCustomerSearchCommand { get; }

        public ICommand ShowServiceCommand { get; }

        public ICommand NewServiceCommand { get; }
        public ICommand EditServiceCommand { get; }
        public ICommand DeleteServiceCommand { get; }

        public ICommand DeleteVehicleCommand { get; }

        public ICommand FirstPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand LastPageCommand { get; }
        //~ End


        public ServicesViewModel()
        {
            ClearVehicleSearchCommand = new ClearVehicleSearchCommand(this);
            ClearCustomerSearchCommand = new ClearCustomerSearchCommand(this);

            ShowServiceCommand = new ShowServiceCommand();

            NewServiceCommand = new NewServiceCommand();
            EditServiceCommand = new EditServiceCommand();
            DeleteServiceCommand = new DeleteServiceCommand();

            DeleteVehicleCommand = new DeleteVehicleCommand();

            FirstPageCommand = new ChangePageCommand(this, ChangePageCommand.Direction.Left);
            PreviousPageCommand = new ChangePageCommand(this, ChangePageCommand.Direction.Left);
            NextPageCommand = new ChangePageCommand(this, ChangePageCommand.Direction.Right);
            LastPageCommand = new ChangePageCommand(this, ChangePageCommand.Direction.Right);

            // TODO: Just to test the view. It will be removed
            using (var db = new mechanicContext())
            {
                allServices = db.Services.Include(x => x.Vehicle).Include(x => x.Parts).Include(x => x.Vehicle.Customer).OrderBy(x => x.ExitDate).ToList();
            }

            CurrentPage = 1;
            NumofPages = (int)Math.Ceiling((decimal)allServices.Count / NumofEntryToShow);

            UpdateListView();
        }

        // TODO: Just to test the view. It will be removed
        private List<Service> allServices;
        public void UpdateListView()
        {
            List<Service> result = allServices.Skip((CurrentPage.Value - 1) * NumofEntryToShow).Take(NumofEntryToShow).ToList();
            Services = result;
        }
    }
}
