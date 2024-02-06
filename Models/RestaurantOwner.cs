using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotPot.Models
{
    public class RestaurantOwner
    {
        [Key]
        public int OwnerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        public Restaurant Restaurant { get; set; }

        public RestaurantOwner()
        {
            OwnerId = 0;
        }

        public RestaurantOwner(int ownerId, string name)
        {
            OwnerId = ownerId;
            Name = name;
        }


    }
}
