using HotPot.Exceptions;
using HotPot.Interfaces;
using HotPot.Mappers;
using HotPot.Models;
using HotPot.Models.DTO;
using System.Security.Cryptography;
using System.Text;

namespace HotPot.Services
{
    public class RestaurantUserServices : IRestaurantUserServices
    {
        private readonly IRepository<int, String, Restaurant> _restaurantRepo;
        private readonly IRepository<int, String, City> _cityRepo;
        private readonly IRepository<int, string, Menu> _menuRepo;
        private readonly IRepository<int, string, Payment> _paymentRepo;
        private readonly IRepository<int, string, Order> _orderRepo;
        private readonly IRepository<int, string, User> _userRepo;
        private readonly IRepository<int, string, RestaurantOwner> _restOwnerRepo;
        private ILogger<RestaurantUserServices> _logger;

        public RestaurantUserServices(IRepository<int, String, Restaurant> restaurantRepo,
                                      IRepository<int, String, City> cityRepo,
                                      IRepository<int, String, Menu> menuRepo,
                                      IRepository<int, string, Payment> paymentRepo,
                                      IRepository<int, string, Order> orderRepo,
                                      IRepository<int, string, User> userRepo,
                                      IRepository<int, string, RestaurantOwner> restOwnerRepo,
                                      ILogger<RestaurantUserServices> logger)
        {
            _restaurantRepo = restaurantRepo;
            _cityRepo = cityRepo;
            _menuRepo = menuRepo;
            _paymentRepo = paymentRepo;
            _orderRepo = orderRepo;
            _userRepo = userRepo;
            _restOwnerRepo = restOwnerRepo;
            _logger = logger;
        }

        public async Task<Menu> AddMenuItem(Menu menu)
        {
            var restaurant=await _restaurantRepo.GetAsync(menu.RestaurantId);
            if(restaurant != null)
            {
                var newItem=await _menuRepo.Add(menu);
                return newItem;
            }
            throw new RestaurantNotFoundException();
        }

        public async Task<Restaurant> AddRestaurant(Restaurant restaurant)
        {
            restaurant=await _restaurantRepo.Add(restaurant);
            return restaurant;
        }

        public async Task<Order> ChangeOrderStatus(int orderId, string newStatus)
        {
            var order=await _orderRepo.GetAsync(orderId);
            if(order!=null)
            {
                order.Status=newStatus.ToLower();
                order = await _orderRepo.Update(order);
                return order;
            }
            throw new OrdersNotFoundException();
        }

        public async Task<List<Order>> GetAllOrders(int RestaurantId)
        {
            var orders = await _orderRepo.GetAsync();
            var ordersForRestaurant = orders.Where(o => o.RestaurantId == RestaurantId).ToList();
            if (ordersForRestaurant != null || ordersForRestaurant.Count > 0)
                return ordersForRestaurant;
            throw new OrdersNotFoundException();
        }

        public async Task<List<Order>> GetAllOrders()
        {
            var orders = await _orderRepo.GetAsync();
            if (orders != null || orders.Count > 0)
                return orders;
            throw new OrdersNotFoundException();
        }

        public async Task<List<Payment>> GetAllPayments(int RestaurantId)
        {
            var payments = await _paymentRepo.GetAsync();
            var orders = await _orderRepo.GetAsync();
            List<Payment> paymentsForRestaurant = (from payment in payments
                                                   join order in orders on payment.OrderId equals order.OrderId
                                                   where order.RestaurantId == RestaurantId
                                                   select payment).ToList();
            if (paymentsForRestaurant != null || paymentsForRestaurant.Count > 0)
                return paymentsForRestaurant;
            throw new PaymentsNotFoundException();
        }

        public async Task<List<Payment>> GetAllPayments()
        {
            var payments = await _paymentRepo.GetAsync();
            if (payments != null || payments.Count > 0)
                return payments;
            throw new PaymentsNotFoundException();
        }

        public async Task<LoginUserDTO> LogInRestaurant(LoginUserDTO loginRestaurant)
        {
            var user = await _userRepo.GetAsync(loginRestaurant.UserName);
            if (user == null)
                throw new InvalidUserException();
            var password = getEncryptedPassword(loginRestaurant.Password, user.Key);
            bool matchPassword = passwordMatch(password, user.Password);
            if (matchPassword)
            {
                loginRestaurant.Password = "";
                loginRestaurant.Role = user.Role;
                return loginRestaurant;
            }
            throw new InvalidUserException();
        }

        private bool passwordMatch(byte[] password, byte[] userPassword)
        {
            for (int i = 0; i < password.Length; i++)
            {
                if (password[i] != userPassword[i])
                    return false;
            }
            return true;
        }

        private byte[] getEncryptedPassword(string password, byte[] key)
        {
            HMACSHA512 hmac = new HMACSHA512(key);
            var userPwd = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return userPwd;
        }

        public async Task<LoginUserDTO> RegisterRestaurant(RegisterRestaurantDTO registerRestaurant)
        {
            registerRestaurant.Role = "RestaurantOwner";
            User newUser = new RegisterToRestaurantUser(registerRestaurant).getUser();
            newUser = await _userRepo.Add(newUser);
            var newRestaurantOwner = new RegisterToRestaurant(registerRestaurant).GetRestaurantOwner();
            newRestaurantOwner = await _restOwnerRepo.Add(newRestaurantOwner);
            LoginUserDTO result = new LoginUserDTO
            {
                UserName = newUser.UserName,
                Role = newUser.Role
            };
            return result;
        }
    }
}
