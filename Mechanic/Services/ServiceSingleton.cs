using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// All the services that are cached in the memory
        /// </summary>
        public List<Service>? AllServices { get; private set; }

        /// <summary>
        /// The last search result executed by 'SearchService' method
        /// </summary>
        public List<Service>? LastSearchResult { get; set; }


        /// <summary>
        /// Get all the services. In the first call, the method gets all the services from DB and saves the result
        /// in 'AllServices' variable. The next calls will not connect to DB, but return the cached result
        /// </summary>
        /// <returns>All services</returns>
        public async Task<List<Service>> GetAllServices()
        {
            if (AllServices != null)
                return AllServices;

            using (var db = new mechanicContext())
            {
                AllServices = await db.Services.Include(x => x.Parts).Include(x => x.Vehicle).Include(x => x.Vehicle.Customer).ToListAsync();
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
    }
}
