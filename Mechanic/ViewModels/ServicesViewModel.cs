using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Mechanic.Models;
using Mechanic.Commands.Services;
using Mechanic.Services;


namespace Mechanic.ViewModels
{
    public class ServicesViewModel : ViewModelBase
    {
        private static readonly int PageSize = 50;


        //~ Begin - Search by Vehicle bindings
        private string? licensePlate;
        public string? LicensePlate
        {
            get => licensePlate;
            set
            {
                licensePlate = value;
                OnPropertyChanged(nameof(LicensePlate));
                OnSearchServices();
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
                OnSearchServices();
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
                OnSearchServices();
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
                OnSearchServices();
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
                OnSearchServices();
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
                OnSearchServices();
            }
        }

        // List View binding
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

        // List View's loading text binding
        private bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        // ~ Begin - Paging binding
        private int currentPage;
        public int CurrentPage
        {
            get => currentPage;
            private set
            {
                if (NumofPages == 0)
                {
                    currentPage = 0;
                    return;
                }

                currentPage = Math.Clamp(value, 1, NumofPages);
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        private int numofPages;
        public int NumofPages
        {
            get => numofPages;
            private set
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

        // In order to check if a different list should be used for paging, search result or all services
        private bool isSearching = false;


        public ServicesViewModel()
        {
            InitializeServices();

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
        }

        private async void InitializeServices()
        {
            IsLoading = true;
            await ServiceSingleton.Instance.GetAllServicesAsync();
            IsLoading = false;

            UpdatePage(1);

            Services = ServiceSingleton.Instance.AllServices?.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        }

        private void OnSearchServices()
        {
            isSearching = ServiceSingleton.Instance.SearchServices(x => (string.IsNullOrEmpty(LicensePlate) || x.Vehicle.LicensePlate.Contains(LicensePlate, StringComparison.InvariantCultureIgnoreCase)) &&
                                                                        (string.IsNullOrEmpty(Make) || x.Vehicle.Make.StartsWith(Make, StringComparison.InvariantCultureIgnoreCase)) &&
                                                                        (string.IsNullOrEmpty(Model) || x.Vehicle.Model.StartsWith(Model, StringComparison.InvariantCultureIgnoreCase)) &&
                                                                        (!Year.HasValue || x.Vehicle.Year == Year) &&
                                                                        (string.IsNullOrEmpty(Color) || x.Vehicle.Color.StartsWith(Color, StringComparison.InvariantCultureIgnoreCase)) &&
                                                                        (string.IsNullOrEmpty(CustomerName) || (x.Vehicle.Customer != null && x.Vehicle.Customer.Name.Contains(CustomerName, StringComparison.InvariantCultureIgnoreCase))));

            UpdatePage(1);
            RefreshListView();
        }

        /// <summary>
        /// Refreshes the list view. Call it when the current page changes
        /// </summary>
        public void RefreshListView()
        {
            if (isSearching)
            {
                if (ServiceSingleton.Instance.LastSearchResult == null)
                    return;

                Services = ServiceSingleton.Instance.LastSearchResult.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            }
            else
            {
                Services = ServiceSingleton.Instance.GetAllServices().Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            }
        }

        /// <summary>
        /// Updates the current page number
        /// </summary>
        /// <param name="pageNumber">The new page</param>
        public void UpdatePage(int pageNumber)
        {
            if (isSearching)
            {
                if (ServiceSingleton.Instance.LastSearchResult == null)
                {
                    NumofPages = 0;
                    CurrentPage = 0;
                }
                else
                {
                    NumofPages = (int)Math.Ceiling((decimal)ServiceSingleton.Instance.LastSearchResult.Count / PageSize);
                    CurrentPage = pageNumber;
                }
            }
            else
            {
                Services = ServiceSingleton.Instance.GetAllServices();
                if (Services == null)
                {
                    NumofPages = 0;
                    CurrentPage = 0;
                }
                else
                {
                    NumofPages = (int)Math.Ceiling((decimal)Services.Count / PageSize);
                    CurrentPage = pageNumber;

                    Services = Services.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                }
            }
        }
    }
}
