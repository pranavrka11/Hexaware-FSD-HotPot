namespace HotPot.Models.DTO
{
    public class OrderMenuDTO
    {
        public int orderId { get; set; }
        public int customerId { get; set; }
        public int restaurantId { get; set; }
        public List<MenuNameDTO> menuName { get; set; }
        public float Price { get; set; }
        public string Status { get; set; }
    }
}
