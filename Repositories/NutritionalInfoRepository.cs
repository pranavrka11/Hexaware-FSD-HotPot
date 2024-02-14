using HotPot.Context;
using HotPot.Exceptions;
using HotPot.Interfaces;
using HotPot.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPot.Repositories
{
    public class NutritionalInfoRepository : IRepository<int, string, NutritionalInfo>
    {
        private readonly HotpotContext _context;
        private readonly ILogger<NutritionalInfoRepository> _logger;
        public NutritionalInfoRepository(HotpotContext context, ILogger<NutritionalInfoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new nutritional information item to the database.
        /// </summary>
        /// <param name="item">The nutritional information item to add.</param>
        /// <returns>The added nutritional information item.</returns>
        public async Task<NutritionalInfo> Add(NutritionalInfo item)
        {
            _context.Add(item);
            _context.SaveChanges();

            LogInformation($"Nutritional Info Added: {item.NutritionId}");

            return item;
        }

        /// <summary>
        /// Deletes an existing nutritional information item from the database.
        /// </summary>
        /// <param name="key">The ID of the nutritional information item to delete.</param>
        /// <returns>The deleted nutritional information item, if found.</returns>
        public async Task<NutritionalInfo> Delete(int key)
        {
            var nutritionalInfo = await GetAsync(key);

            if (nutritionalInfo != null)
            {
                _context.NutritionalInfos.Remove(nutritionalInfo);
                 _context.SaveChanges();

                LogInformation($"Nutritional Info Deleted: {nutritionalInfo.NutritionId}");
                return nutritionalInfo;
            }
            throw new Exception();
            
        }

        /// <summary>
        /// Retrieves a nutritional information item by its ID.
        /// </summary>
        /// <param name="key">The ID of the nutritional information item to retrieve.</param>
        /// <returns>The retrieved nutritional information item, if found.</returns>
        public async Task<NutritionalInfo> GetAsync(int key)
        {
            var nutritionalInfos = await GetAsync();
            var nutritionalInfo = nutritionalInfos.FirstOrDefault(n => n.NutritionId == key);

            if (nutritionalInfo != null)
            {
                return nutritionalInfo;
            }

            throw new NoNutritionalInfoFoundException();
        }

        /// <summary>
        /// Retrieves all nutritional information items from the database.
        /// </summary>
        /// <returns>A list of all nutritional information items.</returns>
        public async Task<List<NutritionalInfo>> GetAsync()
        {
            var nutritionalInfos = _context.NutritionalInfos.ToList();
            return nutritionalInfos;
        }

        /// <summary>
        /// Updates an existing nutritional information item in the database.
        /// </summary>
        /// <param name="item">The nutritional information item to update.</param>
        /// <returns>The updated nutritional information item.</returns>
        public async Task<NutritionalInfo> Update(NutritionalInfo item)
        {
            var nutritionalInfo = await GetAsync(item.NutritionId);

            if (nutritionalInfo != null)
            {
                _context.Entry<NutritionalInfo>(item).State = EntityState.Modified;
                _context.SaveChanges();

                LogInformation($"Nutritional Info Updated: {item.NutritionId}");
                return nutritionalInfo;
            }
            throw new Exception();
            
        }

        /// <summary>
        /// Logs an informational message using the provided logger.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public Task<NutritionalInfo> GetAsync(string key)
        {
            throw new NotImplementedException();
        }
    }
}
