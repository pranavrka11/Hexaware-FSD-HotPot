using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotPot.Models
{
    public class Order : IEquatable<Order>
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public float Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        public int RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        public Restaurant? Restaurant { get; set; }

        public int PartnerId {  get; set; }
        [ForeignKey("PartnerId")]
        public DeliveryPartner? DeliveryPartner { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; } 


        public Order()
        {
            OrderId = 0;
        }

        public Order(int orderid, DateTime orderDate, float amount, string status)
        {
            OrderId = orderid;
            OrderDate = orderDate;
            Amount = amount;
            Status = status;
        }

        public Order(DateTime orderDate, float amount, string status)
        {
            OrderDate = orderDate;
            Amount = amount;
            Status = status;
        }

        public bool Equals(Order? other)
        {
            var order = other ?? new Order();
            return this.OrderId.Equals(order.OrderId);
        }
    }

}
