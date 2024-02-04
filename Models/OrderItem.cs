using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotPot.Models
{
    public class OrderItem : IEquatable<OrderItem>
    {
        
        public int Quantity { get; set; }
        public float SubTotalPrice { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }
        public int MenuId {  get; set; }
        [ForeignKey("MenuId")]
        public Menu? Menu { get; set; }

        public OrderItem()
        { 

        }


        public OrderItem(int quantity, float subTotalPrice)
        {
            Quantity = quantity;
            SubTotalPrice = subTotalPrice;
        }

        public bool Equals(OrderItem? other)
        {
            var orderItem = other ?? new OrderItem();
            return this.OrderId.Equals(orderItem.OrderId);
        }
    }
}
