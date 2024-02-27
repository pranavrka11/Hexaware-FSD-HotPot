using HotPot.Models;
using HotPot.Models.DTO;

namespace HotPot.Interfaces
{
    public interface ICustomerUserService
    {
        //Address Layer
        public Task<CustomerAddress> AddCustomerAddress(CustomerAddress customerAddress);
        public Task<CustomerAddress> UpdateCustomerAddress(int addressId, CustomerAddressUpdateDTO addressUpdateDto);
        public Task<CustomerAddress> ViewCustomerAddressByCustomerId(int customerId);

        //Review Layer
        public Task<CustomerReview> AddCustomerReview(CustomerReview customerReview);
        public Task<CustomerReview> ViewCustomerReview(int customerReviewId);
        public Task<CustomerReview> UpdateCustomerReviewText(CustomerReviewUpdateDTO reviewUpdateDTO);
        public Task<CustomerReview> DeleteCustomerReview(int customerReviewId);

        //Menu Layer 
        public Task<List<Menu>> ViewMenu();
        public Task<List<Menu>> SearchMenu(string query);
        public Task<List<Menu>> FilterMenuByPriceRange(float minPrice, float maxPrice);
        public Task<List<Menu>> FilterMenuByType(string type);
        public Task<List<Menu>> FilterMenuByRestaurant(int restaurantId);
        public Task<List<Menu>> FilterMenuByCuisine(string cuisine);

        //Restaurant layer
        public Task<List<Restaurant>> ViewRestaurants();

        //Order Layer
        public Task<List<Order>> ViewOrders();
        public Task<List<Payment>> ViewPayments();
        public Task<Order> UpdateOrderStatus(int orderId, string newStatus);
    }
}
