using HotPot.Contexts;
using HotPot.Exceptions;
using HotPot.Interfaces;
using HotPot.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPot.Repositories
{
    public class CityRepository : IRepository<int, String, City>
    {
        RequestTrackerContext _context;

        public CityRepository(RequestTrackerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Takes a City object and adds it to the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A City object which has been added to the database</returns>
        public async Task<City> Add(City item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        /// <summary>
        /// Takes a primary key <int> and deletes it's corresponding object from the database
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A City object which has been deleted from the database</returns>
        /// <exception cref="CityNotFoundException"></exception>
        public async Task<City> Delete(int key)
        {
            var city = await GetAsync(key);
            if(city!=null)
            {
                _context.Cities.Remove(city);
                _context.SaveChanges();
                return city;
            }
            throw new CityNotFoundException("No city found");
        }

        /// <summary>
        /// Takes <int> primary key and returns the object from the database that corresponds to it
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A City object with primary key which was passed as the parameter</returns>
        /// <exception cref="CityNotFoundException"></exception>
        public async Task<City> GetAsync(int key)
        {
            var cities = await GetAsync();
            var city = cities.FirstOrDefault(c => c.CityId == key);
            if(city!=null)
            {
                return city;
            }
            throw new CityNotFoundException("No city found");
        }

        /// <summary>
        /// Method to return a list of all the Cities in the City table of the database
        /// </summary>
        /// <returns>List<City> which contains all the City entities in the database</returns>
        /// <exception cref="CityNotFoundException"></exception>
        public async Task<List<City>> GetAsync()
        {
            var cities = _context.Cities.ToList();
            if (cities != null || cities.Count > 0)
                return cities;
            throw new CityNotFoundException("No cities available at the moment to display");
        }

        /// <summary>
        /// Takes a City object as parameter and updates it's reference in the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>An updated City object</returns>
        /// <exception cref="CityNotFoundException"></exception>
        public async Task<City> Update(City item)
        {
            var city=await GetAsync(item.CityId);
            if(city!=null )
            {
                _context.Entry<City>(item).State=EntityState.Modified;
                _context.SaveChanges();
                return city;
            }
            throw new CityNotFoundException("No city found");
        }

        public async Task<City> GetAsync(string name)
        {
            var cities = await GetAsync();
            var city = cities.FirstOrDefault(e => e.Name==name);
            if(city!=null)
                return city;
            throw new CityNotFoundException("No city found");
        }
    }
}
