using HotPot.Context;
using HotPot.Interfaces;
using HotPot.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPot.Repositories
{
    public class NutritionalInfoRepository : IRepository<int, NutritionalInfo>
    {
        private readonly HotpotContext _context;
        private readonly ILogger<NutritionalInfoRepository> _logger;
        public NutritionalInfoRepository(HotpotContext context, ILogger<NutritionalInfoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<NutritionalInfo> Add(NutritionalInfo item)
        {
            _context.NutritionalInfos.Add(item);
            await _context.SaveChangesAsync();

            LogInformation($"Nutritional Info Added: {item.NutritionId}");

            return item;
        }

        public async Task<NutritionalInfo> Delete(int key)
        {
            var nutritionalInfo = await GetAsync(key);

            if (nutritionalInfo != null)
            {
                _context.NutritionalInfos.Remove(nutritionalInfo);
                await _context.SaveChangesAsync();

                LogInformation($"Nutritional Info Deleted: {nutritionalInfo.NutritionId}");
            }

            return nutritionalInfo;
        }
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

        public async Task<List<NutritionalInfo>> GetAsync()
        {
            var nutritionalInfos = await _context.NutritionalInfos.ToListAsync();
            return nutritionalInfos;
        }

        public async Task<NutritionalInfo> Update(NutritionalInfo item)
        {
            var nutritionalInfo = await GetAsync(item.NutritionId);

            if (nutritionalInfo != null)
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                LogInformation($"Nutritional Info Updated: {item.NutritionId}");
            }

            return nutritionalInfo;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
