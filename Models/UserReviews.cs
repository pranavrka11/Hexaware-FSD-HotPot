using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotPot.Models
{
    public class UserReviews
    {
        [Key]
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public int Rating { get; set; }
        public string TextReview { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("RestaurantId")]
        public Restaurant? Restaurant { get; set; }
        public UserReviews()
        {
            
        }

        public UserReviews(int reviewId, int userId, int restaurantId, int rating, string textReview)
        {
            ReviewId = reviewId;
            UserId = userId;
            RestaurantId = restaurantId;
            Rating = rating;
            TextReview = textReview;
        }

        public UserReviews(int userId, int restaurantId, int rating, string textReview)
        {
            UserId = userId;
            RestaurantId = restaurantId;
            Rating = rating;
            TextReview = textReview;
        }
    }
}
