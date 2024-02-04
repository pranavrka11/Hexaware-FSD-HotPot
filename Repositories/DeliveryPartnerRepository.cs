using HotPot.Context;
using HotPot.Interfaces;
using HotPot.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPot.Repositories
{
    public class DeliveryPartnerRepository : IRepository<int, DeliveryPartner>
    {
        private readonly HotpotContext _context;
        private readonly ILogger<DeliveryPartnerRepository> _logger;
        public DeliveryPartnerRepository(HotpotContext context, ILogger<DeliveryPartnerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<DeliveryPartner> Add(DeliveryPartner item)
        {
            _context.DeliveryPartners.Add(item);
            await _context.SaveChangesAsync();

            LogInformation($"Delivery Partner Added: {item.PartnerId}");

            return item;
        }
        public async Task<DeliveryPartner> Delete(int key)
        {
            var partner = await GetAsync(key);

            if (partner != null)
            {
                _context.DeliveryPartners.Remove(partner);
                await _context.SaveChangesAsync();

                LogInformation($"Delivery Partner Deleted: {partner.PartnerId}");
            }

            return partner;
        }
        public async Task<DeliveryPartner> GetAsync(int key)
        {
            var partners = await GetAsync();
            var partner = partners.FirstOrDefault(p => p.PartnerId == key);

            if (partner != null)
            {
                return partner;
            }

            throw new NoDeliveryPartnerFoundException();
        }

        public async Task<List<DeliveryPartner>> GetAsync()
        {
            var partners = await _context.DeliveryPartners.ToListAsync();
            return partners;
        }


        public async Task<DeliveryPartner> Update(DeliveryPartner item)
        {
            var partner = await GetAsync(item.PartnerId);

            if (partner != null)
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                LogInformation($"Delivery Partner Updated: {item.PartnerId}");
            }

            return partner;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
