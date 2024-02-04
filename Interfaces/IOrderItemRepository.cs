using HotPot.Models;

namespace HotPot.Interfaces
{
    public interface IOrderItemRepository : IRepository<(int OrderId, int MenuId), OrderItem>
    {
        Task<List<OrderItem>> GetAsync();
    }
}
