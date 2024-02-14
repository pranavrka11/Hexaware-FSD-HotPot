using HotPot.Exceptions;
using HotPot.Interfaces;
using HotPot.Models;
using HotPot.Models.DTO;
using HotPot.Repositories;
using System.Numerics;

namespace HotPot.Services
{
    public class CustomerUserService : ICustomerUserService
    {
        private readonly IRepository<int, string, CustomerAddress> _custAddressRepo;
        private readonly IRepository<int, string, CustomerReview> _custReviewRepo;
        private readonly IRepository<int, string, Menu> _menuRepo;
        private readonly IRepository<int, string, Restaurant> _restaurantRepo;
        private readonly IRepository<int, string, City> _cityRepo;
        private readonly IRepository<int, string, State> _stateRepo;

        //private readonly IRepository<int, string, Cart> _cartRepo;
        //private readonly IRepository<int, string, CartItem> _cartItemRepo;
        private readonly ILogger<CustomerUserService> _logger;

        public CustomerUserService(
                IRepository<int, string, CustomerAddress> custAddressRepo,
                IRepository<int, string, CustomerReview> custReviewRepo,
                IRepository<int, string, Menu> menuRepo,
                IRepository<int, string, Restaurant> restaurantRepo,
                IRepository<int, string, City> cityRepo,
                IRepository<int, string, State> stateRepo,
                //IRepository<int, string, Cart> cartRepo,
                //IRepository<int, string, CartItem> cartItemRepo,
                ILogger<CustomerUserService> logger)
        {
            _custAddressRepo = custAddressRepo;
            _custReviewRepo = custReviewRepo;
            _menuRepo = menuRepo; 
            _restaurantRepo = restaurantRepo;
            _cityRepo = cityRepo;
            _stateRepo = stateRepo;
            //_cartRepo = cartRepo;
            //_cartItemRepo = cartItemRepo;
            _logger = logger;
        }

        //Address Layer

        public async Task<CustomerAddress> AddCustomerAddress(CustomerAddress customerAddress)
        {
            customerAddress = await _custAddressRepo.Add(customerAddress);
            LogInformation($"Customer Address added successfully. AddressId: {customerAddress.AddressId}");
            return customerAddress;
            
        }


        public async Task<CustomerAddress> UpdateCustomerAddress(int addressId, CustomerAddressUpdateDTO addressUpdateDto)
        {
            var existingAddress = await _custAddressRepo.GetAsync(addressId);
            if (existingAddress == null)
            {
                throw new NoCustomerAddressFoundException($"Customer address with ID {addressId} not found.");
            }
            LogInformation("Existing Customer Address Details:");
            LogInformation($"House Number: {existingAddress.HouseNumber}");
            LogInformation($"Building Name: {existingAddress.BuildingName}");
            LogInformation($"Locality: {existingAddress.Locality}");
            LogInformation($"City ID: {existingAddress.CityId}");
            LogInformation($"Landmark: {existingAddress.LandMark}");

            if (!string.IsNullOrEmpty(addressUpdateDto.HouseNumber))
            {
                existingAddress.HouseNumber = addressUpdateDto.HouseNumber;
            }
            if (!string.IsNullOrEmpty(addressUpdateDto.BuildingName))
            {
                existingAddress.BuildingName = addressUpdateDto.BuildingName;
            }
            if (!string.IsNullOrEmpty(addressUpdateDto.Locality))
            {
                existingAddress.Locality = addressUpdateDto.Locality;
            }
            if (addressUpdateDto.CityId.HasValue)
            {
                existingAddress.CityId = addressUpdateDto.CityId.Value;
            }
            if (!string.IsNullOrEmpty(addressUpdateDto.LandMark))
            {
                existingAddress.LandMark = addressUpdateDto.LandMark;
            }

            var updatedAddress = await _custAddressRepo.Update(existingAddress);
            LogInformation($"Customer address updated successfully. AddressId: {updatedAddress.AddressId}");
            return updatedAddress;
        }

        public async Task<CustomerAddress> ViewCustomerAddressByCustomerId(int customerId)
        {
            var customerAddress = await _custAddressRepo.GetAsync(customerId);
            if (customerAddress == null)
            {
                throw new NoCustomerAddressFoundException($"No address found for customer with ID {customerId}");
            }
            return customerAddress;
        }


        //Review Layer

        public async Task<CustomerReview> AddCustomerReview(CustomerReview customerReview)
        {
            customerReview = await _custReviewRepo.Add(customerReview);
            LogInformation($"Customer Review added successfully. ReviewId: {customerReview.ReviewId}");
            return customerReview;
        }

        public async Task<CustomerReview> ViewCustomerReview(int customerReviewId)
        {
            var customerReview = await _custReviewRepo.GetAsync(customerReviewId);
            if (customerReview == null)
            {
                throw new NoCustomerReviewFoundException($"No review found with ID {customerReviewId}");
            }
            return customerReview;
        }

        public async Task<CustomerReview> UpdateCustomerReviewText(CustomerReviewUpdateDTO reviewUpdateDTO)
        {
            var existingReview = await _custReviewRepo.GetAsync(reviewUpdateDTO.ReviewId);
            if (existingReview == null)
            {
                throw new NoCustomerReviewFoundException($"Customer review with ID {reviewUpdateDTO.ReviewId} not found.");
            }
            LogInformation($"Existing Text Review: {existingReview.TextReview}");
            existingReview.TextReview = reviewUpdateDTO.TextReview;
            var updatedReview = await _custReviewRepo.Update(existingReview);
            LogInformation($"Text Review updated successfully. ReviewId: {updatedReview.ReviewId}");
            return updatedReview;
        }

        public async Task<CustomerReview> DeleteCustomerReview(int reviewId)
        {
            var existingReview = await _custReviewRepo.GetAsync(reviewId);
            if (existingReview == null)
            {
                throw new NoCustomerReviewFoundException($"Customer review with ID {reviewId} not found.");
            }
            existingReview.IsDeleted = true;
            var updatedReview = await _custReviewRepo.Update(existingReview);
            LogInformation($"Customer Review soft deleted successfully. ReviewId: {updatedReview.ReviewId}");
            return updatedReview;
        }



        //Menu Layer
        public async Task<List<Menu>> ViewMenu()
        {
            var menuItems = await _menuRepo.GetAsync();
            return menuItems;
        }

        public async Task<List<Menu>> SearchMenu(string query)
        {
            var allMenuItems = await _menuRepo.GetAsync();
            if(allMenuItems == null)
            {
                throw new NoMenuFoundException("No menu item found");
            }
            var matchingMenuItems = allMenuItems.Where(menu => menu.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            return matchingMenuItems;
        }

        public async Task<List<Menu>> FilterMenuByPriceRange(float minPrice, float maxPrice)
        {
            var allMenuItems = await _menuRepo.GetAsync();
            if (allMenuItems == null)
            {
                throw new NoMenuFoundException("No menu items found");
            }
            var filteredMenuItems = allMenuItems.Where(m => m.Price >= minPrice && m.Price <= maxPrice).ToList();
            return filteredMenuItems;
        }

        public async Task<List<Menu>> FilterMenuByType(string type)
        {
            var allMenuItems = await _menuRepo.GetAsync();
            if (allMenuItems == null)
            {
                throw new NoMenuFoundException("No menu items found");
            }
            var filteredMenuItems = allMenuItems.Where(m => string.Equals(m.Type, type, StringComparison.OrdinalIgnoreCase)).ToList();
            return filteredMenuItems;
        }

        public async Task<List<Menu>> FilterMenuByRestaurant(int restaurantId)
        {
            var restaurant = await _restaurantRepo.GetAsync(restaurantId);
            if (restaurant == null)
            {
                throw new RestaurantNotFoundException($"Restaurant with ID {restaurantId} not found.");
            }
            var allMenuItems = await _menuRepo.GetAsync();
            var filteredMenuItems = allMenuItems.Where(m => m.RestaurantId == restaurantId).ToList();
            return filteredMenuItems;
        }

        public async Task<List<Menu>> FilterMenuByCuisine(string cuisine)
        {
            var allMenuItems = await _menuRepo.GetAsync();
            if (allMenuItems == null)
            {
                throw new NoMenuFoundException("No menu with given cuisine");
            }
            var filteredMenuItems = allMenuItems.Where(m => string.Equals(m.Cuisine,cuisine, StringComparison.OrdinalIgnoreCase)).ToList();
            return filteredMenuItems;
        }


        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

    }
}
