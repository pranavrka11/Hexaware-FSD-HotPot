using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotPot.Models
{
    public class RestaurantSpeciality
    {
        [Key]
        public int CategoryId { get; set; }
        public int RestaurantId { get; set; }
        public string CategoryName { get; set; }

        [ForeignKey("RestaurantId")]
        public Restaurant? Restaurant { get; set; }
        public string? CategoryImage { get; set; }

        public RestaurantSpeciality()
        {
            
        }

        public RestaurantSpeciality(int categoryId, int restaurantId, string categoryName)
        {
            CategoryId = categoryId;
            RestaurantId = restaurantId;
            CategoryName = categoryName;
        }

        public RestaurantSpeciality(int restaurantId, string categoryName)
        {
            RestaurantId = restaurantId;
            CategoryName = categoryName;
        }
    }
}
