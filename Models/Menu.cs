using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotPot.Models
{
    public class Menu : IEquatable<Menu>
    {
        [Key]
        public int MenuId { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public float Price { get; set; }
        public string? Description { get; set; } = string.Empty;
        public string? Cuisine { get; set; } = string.Empty;
        public TimeSpan CookingTime {  get; set; }
        public string? TasteInfo { get; set; } = string.Empty;
        public string? MenuImage {  get; set; } = string.Empty;
        public int NutritionId { get; set; }
        [ForeignKey("NutritionId")]
        public NutritionalInfo? NutritionalInfo { get; set; }   
        public int RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        public Restaurant? Restaurant { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }


        public Menu()
        {
            MenuId = 0;
        }

        public Menu(int menuid, string name, string type, float price, string? description, string? cuisine, TimeSpan cookingTime, string? tasteInfo, string? menuImage)
        {
            MenuId = menuid;
            Name = name;
            Type = type;
            Price = price;
            Description = description;
            Cuisine = cuisine;
            CookingTime = cookingTime;
            TasteInfo = tasteInfo;
            MenuImage = menuImage;
            
        }

        public Menu(string name, string type, float price, string? description, string? cuisine, TimeSpan cookingTime, string? tasteInfo,string? menuImage)
        {
            Name = name;
            Type = type;
            Price = price;
            Description = description;
            Cuisine = cuisine;
            CookingTime = cookingTime;
            TasteInfo = tasteInfo;
            MenuImage = menuImage;
        }

        public bool Equals(Menu? other)
        {
            var menu = other ?? new Menu();
            return this.MenuId.Equals(menu.MenuId);
        }
    }
}
