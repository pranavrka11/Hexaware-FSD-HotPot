using HotPot.Exceptions;
using HotPot.Interfaces;
using HotPot.Mappers;
using HotPot.Migrations;
using HotPot.Models;
using HotPot.Models.DTO;
using System.Security.Cryptography;
using System.Text;

namespace HotPot.Services
{
    public class CustomerServices : ICustomerServices
    {
        private readonly IRepository<int, string, Customer> _custRepo;
        private readonly IRepository<int, string, User> _userRepo;
        private readonly IRepository<int, string, Menu> _menuRepo;
        private readonly IRepository<int, string, Cart> _cartRepo;
        private readonly IRepository<int, string, Order> _orderRepo;
        private readonly IRepository<int, string, OrderItem> _orderItemRepo;
        private readonly IRepository<int, string, Payment> _paymentRepo;
        private readonly IRepository<int, string, Restaurant> _restaurantRepo;
        private readonly IRepository<int, string, City> _cityRepo;
        private readonly ITokenServices _tokenServices;
        private readonly ILogger<CustomerServices> _logger;

        public CustomerServices(IRepository<int, string, Customer> custRepo,
                                IRepository<int, string, User> userRepo,
                                IRepository<int, string, Menu> menuRepo,
                                IRepository<int, string, Cart> cartRepo,
                                IRepository<int, string, Order> orderRepo,
                                IRepository<int, string, OrderItem> orderItemRepo,
                                IRepository<int, string, Payment> paymentRepo,
                                IRepository<int, String, Restaurant> restaurantRepo,
                                IRepository<int, String, City> cityRepo,
                                ITokenServices tokenServices,
                                ILogger<CustomerServices> logger)
        {
            _custRepo = custRepo;
            _userRepo = userRepo;
            _menuRepo = menuRepo;
            _cartRepo = cartRepo;
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _paymentRepo = paymentRepo;
            _restaurantRepo = restaurantRepo;
            _cityRepo = cityRepo;
            _tokenServices = tokenServices;
            _logger = logger;
        }

        public async Task<LoginUserDTO> LogIn(LoginUserDTO loginCustomer)
        {
            var user = await _userRepo.GetAsync(loginCustomer.UserName);
            if (user == null)
                throw new InvalidUserException();
            var password = getEncryptedPassword(loginCustomer.Password, user.Key);
            bool matchPassword = passwordMatch(password, user.Password);
            if (matchPassword)
            {
                loginCustomer.Password = "";
                loginCustomer.Role = user.Role;
                loginCustomer.Role = await _tokenServices.GenerateToken(loginCustomer);
                return loginCustomer;
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

        public async Task<LoginUserDTO> RegisterCustomer(RegisterCustomerDTO registerCustomer)
        {
            registerCustomer.Role = "Customer";
            User myUser = new RegisterToUser(registerCustomer).getUser();
            myUser = await _userRepo.Add(myUser);
            Customer myCustomer = new RegisterToCustomer(registerCustomer).GetCustomer();
            myCustomer=await _custRepo.Add(myCustomer);
            LoginUserDTO result = new LoginUserDTO
            {
                UserName=myUser.UserName,
                Role=myUser.Role
            };
            return result;
        }

        public async Task<List<Menu>> GetMenuByRestaurant(int RestaurantId)
        {
            var menusItems = await _menuRepo.GetAsync();
            var menuForRestaurant = menusItems.Where(m => m.RestaurantId == RestaurantId).ToList();
            if (menuForRestaurant == null || menuForRestaurant.Count == 0)
                throw new NoMenuAvailableException();
            return menuForRestaurant;     
        }

        public async Task<Restaurant> GetRestaurantByName(string name)
        {
            var restaurant = await _restaurantRepo.GetAsync(name);
            if (restaurant == null)
                throw new RestaurantNotFoundException();
            return restaurant;
        }

        public async Task<List<Restaurant>> GetRestaurantsByCity(string city)
        {
            var myCity = await _cityRepo.GetAsync(city);
            if (myCity == null)
            {
                throw new CityNotFoundException();
            }          
            var restaurants = await _restaurantRepo.GetAsync();
            var myCityRestaurants = restaurants.Where(r => r.CityId == myCity.CityId).ToList();
            if (myCityRestaurants == null || myCityRestaurants.Count == 0)
                throw new RestaurantNotFoundException();
            return myCityRestaurants;
        }

        public async Task<OrderMenuDTO> PlaceOrder(int customerId, string paymentMode)
        {
            var carts = await _cartRepo.GetAsync();
            var cartItems = carts.Where(c => c.CustomerId == customerId).Where(c => c.Status == "added").ToList();
            float totalAmount = 0;
            foreach(var cart in cartItems)
            {
                var menuItem = await _menuRepo.GetAsync(cart.MenuItemId);
                totalAmount += menuItem.Price * cart.Quantity;
            }
            Order newOrder = new Order
            {
                OrderDate = DateTime.Now,
                Amount = totalAmount,
                Status = "created",
                CustomerId = cartItems[0].CustomerId,
                RestaurantId = cartItems[0].RestaurantId
            };
            var payment = await RecordPayment(newOrder);
            if(payment.Status=="successful")
            {
                newOrder.Status = "placed";
                newOrder = await _orderRepo.Add(newOrder);
                List<MenuNameDTO> names = new List<MenuNameDTO>();
                foreach (var cart in cartItems)
                {
                    var menuItem = await _menuRepo.GetAsync(cart.MenuItemId);
                    OrderItem newOrderItem = new OrderItem
                    {
                        OrderId = newOrder.OrderId,
                        MenuId = cart.MenuItemId,
                        SubTotalPrice = menuItem.Price * cart.Quantity,
                        Quantity = cart.Quantity
                    };
                    await _orderItemRepo.Add(newOrderItem);

                    cart.Status = "purchased";
                    await _cartRepo.Update(cart);

                    MenuNameDTO menuNameDTO = new MenuNameDTO
                    {
                        ManuItemName = menuItem.Name,
                        Quantity = cart.Quantity
                    };
                    names.Add(menuNameDTO);
                }
                payment.OrderId= newOrder.OrderId;
                payment = await _paymentRepo.Update(payment);
                OrderMenuDTO orderItems = new OrderMenuDTO
                {
                    orderId = newOrder.OrderId,
                    customerId = newOrder.CustomerId,
                    restaurantId = newOrder.RestaurantId,
                    menuName = names,
                    Price = totalAmount,
                    Status = newOrder.Status
                };
                return orderItems;
            }
            throw new PaymentFailedException();
        }

        public async Task<OrderMenuDTO> PlaceOrderForOne(int cartItemId, string paymentMode)
        {
            Cart cartItem = await _cartRepo.GetAsync(cartItemId);
            Menu menuItem = await _menuRepo.GetAsync(cartItem.MenuItemId);
            float amount = menuItem.Price * cartItem.Quantity;
            Order newOrder = new Order
            {
                OrderDate = DateTime.Now,
                Amount = amount,
                Status = "created",
                CustomerId = cartItem.CustomerId,
                RestaurantId = cartItem.RestaurantId,
            };
            OrderItem newOrderItem = new OrderItem
            {
                MenuId = menuItem.MenuId,
                Quantity = cartItem.Quantity,
                SubTotalPrice = amount
            };
            var payment = await RecordPayment(newOrder);
            if(payment.Status=="successful")
            {
                newOrder.Status = "placed";
                newOrder = await _orderRepo.Add(newOrder);
                newOrderItem.OrderId = newOrder.OrderId;
                newOrderItem=await _orderItemRepo.Add(newOrderItem);
                payment.OrderId = newOrderItem.OrderId;
                payment = await _paymentRepo.Update(payment);
                cartItem.Status = "purchased";
                cartItem = await _cartRepo.Update(cartItem);
                List<MenuNameDTO> names = new List<MenuNameDTO>();
                MenuNameDTO menuNameDTO = new MenuNameDTO
                {
                    ManuItemName = menuItem.Name,
                    Quantity = cartItem.Quantity
                };
                names.Add(menuNameDTO);
                OrderMenuDTO orderItems = new OrderMenuDTO
                {
                    orderId = newOrder.OrderId,
                    customerId = newOrder.CustomerId,
                    restaurantId = newOrder.RestaurantId,
                    menuName = names,
                    Price = amount,
                };
                return orderItems;
            }
            throw new PaymentFailedException();
        }

        public async Task<Payment> RecordPayment(Order order)
        {
            Payment payment = new Payment
            {
                PaymentMode = "online",
                Amount = order.Amount,
                Status = "successful",
                Date = DateTime.Now
            };
            payment = await _paymentRepo.Add(payment);
            return payment;
        }

        public async Task<CartMenuDTO> AddToCart(int userId, int menuItemId)
        {
            var menuItem = await _menuRepo.GetAsync(menuItemId);
            if(menuItem!=null)
            {
                var cartItems = await _cartRepo.GetAsync();
                var customerCart = cartItems.Where(c => c.CustomerId == userId).Where(c => c.Status == "added").ToList();
                var checkMenuInCart=customerCart.FirstOrDefault(c=>c.MenuItemId==menuItemId);
                if (checkMenuInCart == null)
                {
                    Cart cartItem = new Cart
                    {
                        CustomerId = userId,
                        RestaurantId = menuItem.RestaurantId,
                        MenuItemId = menuItemId,
                        Quantity = 1,
                        Status = "added"
                    };
                    cartItem = await _cartRepo.Add(cartItem);

                    CartMenuDTO cartMenu = new CartMenuDTO
                    {
                        CartId = cartItem.Id,
                        CustomerId = userId,
                        RestaurantId = menuItem.RestaurantId,
                        MenuItemId = menuItemId,
                        MenuTitle = menuItem.Name,
                        Quantity = cartItem.Quantity,
                        Price = menuItem.Price * cartItem.Quantity
                    };
                    return cartMenu;
                }
                await IncreaseCartItemQuantity(checkMenuInCart.Id);
                return null;
            }
            throw new NoMenuAvailableException();
        }

        public async Task<List<CartMenuDTO>> GetCarts(int customerId)
        {
            var cartItems = await _cartRepo.GetAsync();
            var cartForCustomer = cartItems.Where(c => c.CustomerId == customerId).Where(c => c.Status == "added").ToList();
            List<CartMenuDTO> cartMenus = new List<CartMenuDTO>();
            if (cartForCustomer != null || cartForCustomer.Count > 0)
            {
                foreach(var cartItem in cartForCustomer)
                {
                    var menuItem = await _menuRepo.GetAsync(cartItem.MenuItemId);
                    CartMenuDTO cartMenu = new CartMenuDTO
                    {
                        CartId = cartItem.Id,
                        CustomerId = cartItem.CustomerId,
                        RestaurantId = menuItem.RestaurantId,
                        MenuItemId = menuItem.MenuId,
                        MenuTitle = menuItem.Name,
                        Quantity = cartItem.Quantity,
                        Price = menuItem.Price * cartItem.Quantity
                    };
                    cartMenus.Add(cartMenu);
                }
                if (cartMenus == null || cartMenus.Count == 0)
                    return null;
                else
                    return cartMenus;
            }
            throw new EmptyCartException();
        }

        public async Task DeleteCartItem(int cartItemId)
        {
            var cartItem=await _cartRepo.GetAsync(cartItemId);
            cartItem.Status = "deleted";
            cartItem = await _cartRepo.Update(cartItem);
        }

        public async Task EmptyCart(int customerId)
        {
            var cartItems = await _cartRepo.GetAsync();
            var cartForCustomer = cartItems.Where(c => c.CustomerId == customerId).Where(c => c.Status == "added").ToList();
            foreach (var cartItem in cartItems)
            {
                cartItem.Status = "deleted";
                await _cartRepo.Update(cartItem);
            }
        }

        public async Task<Cart> IncreaseCartItemQuantity(int cartId)
        {
            var cartItem = await _cartRepo.GetAsync(cartId);
            cartItem.Quantity++;
            cartItem = await _cartRepo.Update(cartItem);
            return cartItem;
        }

        public async Task<Cart> DecreaseCartItemQuantity(int cartId)
        {
            var cartItem = await _cartRepo.GetAsync(cartId);
            if(cartItem.Quantity>1)
            {
                cartItem.Quantity--;
                cartItem = await _cartRepo.Update(cartItem);
                return cartItem;
            }
            else
            {
                await DeleteCartItem(cartId);
                return null;
            }
        }

        public async Task<OrderMenuDTO> ViewOrderStatus(int orderId)
        {
            var order = await _orderRepo.GetAsync(orderId);
            if (order == null)
                throw new OrdersNotFoundException();

            var orderitems = await _orderItemRepo.GetAsync();
            var orderItemsForCustomer = orderitems.Where(oi => oi.OrderId == orderId).ToList();

            List<MenuNameDTO> menuList = new List<MenuNameDTO>();
            float totalPrice = 0;

            foreach(var orderItem in orderItemsForCustomer)
            {
                var menu = await _menuRepo.GetAsync(orderItem.MenuId);
                MenuNameDTO menuNameDTO = new MenuNameDTO
                {
                    ManuItemName = menu.Name,
                    Quantity = orderItem.Quantity
                };
                menuList.Add(menuNameDTO);
                totalPrice += orderItem.SubTotalPrice;
            }

            OrderMenuDTO orderMenuDTO = new OrderMenuDTO
            {
                orderId = orderId,
                customerId = order.CustomerId,
                restaurantId = order.RestaurantId,
                menuName = menuList,
                Price = totalPrice,
                Status = order.Status
            };

            return orderMenuDTO;
        }
    }
}
