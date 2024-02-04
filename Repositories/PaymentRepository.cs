using HotPot.Context;
using HotPot.Interfaces;
using HotPot.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPot.Repositories
{
    public class PaymentRepository : IRepository<int, Payment>
    {
        private readonly HotpotContext _context;
        private readonly ILogger<PaymentRepository> _logger;
        public PaymentRepository(HotpotContext context, ILogger<PaymentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<Payment> Add(Payment item)
        {
            _context.Payments.Add(item);
            await _context.SaveChangesAsync();

            LogInformation($"Payment Added: {item.PaymentId}");

            return item;
        }
        public async Task<Payment> Delete(int key)
        {
            var payment = await GetAsync(key);

            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();

                LogInformation($"Payment Deleted: {payment.PaymentId}");
            }

            return payment;
        }
        public async Task<Payment> GetAsync(int key)
        {
            var payments = await GetAsync();
            var payment = payments.FirstOrDefault(p => p.PaymentId == key);

            if (payment != null)
            {
                return payment;
            }

            throw new NoPaymentFoundException();
        }

        public async Task<List<Payment>> GetAsync()
        {
            var payments = await _context.Payments
                .Include(p => p.Order)
                .ToListAsync();

            return payments;
        }

        public async Task<Payment> Update(Payment item)
        {
            var payment = await GetAsync(item.PaymentId);

            if (payment != null)
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                LogInformation($"Payment Updated: {item.PaymentId}");
            }

            return payment;
        }


        public void LogInformation(string message)
        {
            _logger.LogInformation(message);   
        }
    }
}
