using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Mechanic.Models;


namespace Mechanic.Services
{
    public class ServiceSingleton
    {
        private static ServiceSingleton instance = null!;
        public static ServiceSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServiceSingleton();
                }

                return instance;
            }
        }

        /// <summary>
        /// The last cached of the services
        /// </summary>
        public List<Service>? AllServices { get; private set; }

        /// <summary>
        /// The last search result executed by 'SearchService' method
        /// </summary>
        public List<Service>? LastSearchResult { get; set; }


        /// <summary>
        /// Get all the services asynchronously
        /// </summary>
        /// <returns>All services</returns>
        public async Task<List<Service>> GetAllServicesAsync()
        {
            using (var db = new mechanicContext())
            {
                AllServices = await db.Services.Include(x => x.Parts).Include(x => x.Vehicle).Include(x => x.Vehicle.Customer).OrderByDescending(x => x.ExitDate ?? DateOnly.FromDateTime(DateTime.MaxValue)).ThenByDescending(x => x.EnterDate).ToListAsync();
            }

            return AllServices;
        }

        /// <summary>
        /// Get all the services
        /// </summary>
        /// <returns>All services</returns>
        public List<Service> GetAllServices()
        {
            using (var db = new mechanicContext())
            {
                AllServices = db.Services.Include(x => x.Parts).Include(x => x.Vehicle).Include(x => x.Vehicle.Customer).OrderByDescending(x => x.ExitDate ?? DateOnly.FromDateTime(DateTime.MaxValue)).ThenByDescending(x => x.EnterDate).ToList();
            }

            return AllServices;
        }

        /// <summary>
        /// Search a service/services in 'AllServices'
        /// </summary>
        /// <param name="query">LINQ for the search</param>
        /// <returns>True if the query does not return all services. The result is saved in 'LastSearchResult' variable</returns>
        public bool SearchServices(Func<Service, bool> query)
        {
            if (query == null || AllServices == null)
                return false;

            LastSearchResult = AllServices.Where(query).ToList();

            return LastSearchResult.Count != AllServices.Count;
        }

        /// <summary>
        /// Adds a new service to the database
        /// </summary>
        /// <param name="data">Service data to add</param>
        public void AddService(Service data)
        {
            using (var db = new mechanicContext())
            {
                db.Services.Add(data);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Updates a service on the database
        /// </summary>
        /// <param name="data">New service data to update</param>
        public void UpdateService(Service data)
        {
            using (var db = new mechanicContext())
            {
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a service from the database
        /// </summary>
        /// <param name="id">ID of the service to delete</param>
        public void DeleteService(int id)
        {
            using (var db = new mechanicContext())
            {
                Service? service = db.Services.Include(x => x.Parts).FirstOrDefault(x => x.Id == id);
                if (service != null)
                {
                    db.Services.Remove(service);
                    db.SaveChanges();
                }
            }
        }



        /// <summary>
        /// Get the vehicle data. It uses 'AllServices', so that all the services must be saved first to call this method
        /// </summary>
        /// <param name="licensePlate">The license plate of the vehicle</param>
        /// <returns>Returns the related vehicle data if exists. If not, it returns null</returns>
        public Vehicle? GetVehicle(string licensePlate)
        {
            Service? service = AllServices?.FirstOrDefault(x => x.Vehicle.LicensePlate == licensePlate);
            if (service == null)
                return null;

            return service.Vehicle;
        }

        /// <summary>
        /// Adds a new vehicle to the database
        /// </summary>
        /// <param name="data">Vehicle data to add</param>
        public void AddVehicle(Vehicle data)
        {
            using (var db = new mechanicContext())
            {
                db.Vehicles.Add(data);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Updates a vehicle on the database
        /// </summary>
        /// <param name="data">New vehicle data to update</param>
        public void UpdatesVehicle(Vehicle data)
        {
            using (var db = new mechanicContext())
            {
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a vehicle and its related services and part data, and its customer data if the customer has no more vehicle
        /// </summary>
        /// <param name="id">The ID of the vehicle to delete</param>
        public void DeleteVehicle(int id)
        {
            using (var db = new mechanicContext())
            {
                Vehicle? vehicle = db.Vehicles.Include(x => x.Services).ThenInclude(x => x.Parts).Where(x => x.Id == id).FirstOrDefault();
                if (vehicle != null)
                {
                    db.Vehicles.Remove(vehicle);

                    if (vehicle.CustomerId != null)
                    {
                        Customer? customer = db.Customers.Include(x => x.Vehicles).Where(x => x.Id == vehicle.CustomerId).FirstOrDefault();
                        if (customer != null && customer.Vehicles.Count == 1)
                        {
                            db.Customers.Remove(customer);
                        }
                    }

                    db.SaveChanges();
                }
            }
        }



        /// <summary>
        /// Adds a new customer to the database
        /// </summary>
        /// <param name="data">Customer data to add</param>
        public void AddCustomer(Customer data)
        {
            using (var db = new mechanicContext())
            {
                db.Customers.Add(data);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Updates a customer on the database
        /// </summary>
        /// <param name="data">Customer data to update</param>
        public void UpdateCustomer(Customer data)
        {
            using (var db = new mechanicContext())
            {
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a vehicle from the database
        /// </summary>
        /// <param name="id">The ID of the customer to delete</param>
        public void DeleteCustomer(int id)
        {
            using (var db = new mechanicContext())
            {
                Customer? customer = db.Customers.Where(x => x.Id == id).FirstOrDefault();
                if (customer != null)
                {
                    db.Customers.Remove(customer);
                    db.SaveChanges();
                }
            }
        }
    }
}
