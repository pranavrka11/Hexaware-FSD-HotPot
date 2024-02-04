using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotPot.Models
{
    public class UserAddress
    {
        [Key]
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public string? HouseNumber { get; set; }
        public string? BuildingName { get; set; }
        public string? Locality { get; set; }
        public int CityId { get; set; }

        [ForeignKey("CityId")]
        public City? City { get; set; }
        public string? LandMark { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public UserAddress()
        {
            
        }

        public UserAddress(int userId, string? houseNumber, string? buildingName, string? locality, int cityId, string? landMark)
        {
            UserId = userId;
            HouseNumber = houseNumber;
            BuildingName = buildingName;
            Locality = locality;
            CityId = cityId;
            LandMark = landMark;
        }

        public UserAddress(int addressId, int userId, string? houseNumber, string? buildingName, string? locality, int cityId, string? landMark)
        {
            AddressId = addressId;
            UserId = userId;
            HouseNumber = houseNumber;
            BuildingName = buildingName;
            Locality = locality;
            CityId = cityId;
            LandMark = landMark;
        }
    }
}
