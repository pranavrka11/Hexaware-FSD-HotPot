using Castle.Core.Configuration;
using HotPot.Contexts;
using HotPot.Interfaces;
using HotPot.Models;
using HotPot.Models.DTO;
using HotPot.Repositories;
using HotPot.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotPotTests
{
    internal class CustomerTests
    {
        RequestTrackerContext context;

        [SetUp]
        public void SetUp() 
        {
            var options = new DbContextOptionsBuilder<RequestTrackerContext>().UseInMemoryDatabase("TestDb").Options;
            context = new RequestTrackerContext(options);
        }

        [Order(1)]
        [Test]
        public async Task RegisterCustomerTest()
        {
            var mockMenuRepoLogger=new Mock<ILogger<MenuRepository>>();
            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

            IRepository<int, string, Customer> custRepo=new CustomerRepository(context);
            IRepository<int, string, User> userRepo = new UserRepository(context);
            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
            IRepository<int, string, City> cityRepo = new CityRepository(context);

            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

            var customer = await services.RegisterCustomer(new RegisterCustomerDTO
            {
                Name = "Pranav Karlekar",
                Email = "pranav@abc.com",
                Phone = "5584201236",
                UserName = "pranav",
                Password = "pranav123",
                Role = "customer"
            });

            Assert.AreEqual("pranav", customer.UserName);
            Assert.AreEqual("Customer", customer.Role);
        }

        [Order(2)]
        [Test]
        public async Task AddTest()
        {
            var mockServiceLogger = new Mock<ILogger<RestaurantUserServices>>();
            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
            var mockPaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
            var mockRestOwnerRepoLogger = new Mock<ILogger<RestaurantOwnerRepository>>();

            IRepository<int, String, City> cityRepo = new CityRepository(context);
            IRepository<int, String, Restaurant> restRepo = new RestaurantRepository(context);
            IRepository<int, String, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
            IRepository<int, String, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaymentRepoLogger.Object);
            IRepository<int, string, User> userRepo = new UserRepository(context);
            IRepository<int, string, RestaurantOwner> restOwnerRepo = new RestaurantOwnerRepository(context, mockRestOwnerRepoLogger.Object);

            IRestaurantUserServices services = new RestaurantUserServices(restRepo, cityRepo, menuRepo, paymentRepo, orderRepo, userRepo, restOwnerRepo, mockServiceLogger.Object);

            var res1 = await services.AddRestaurant(
                new Restaurant
                {
                    RestaurantName = "Cafe Express",
                    Phone = "7785456985",
                    Email = "abc@def.com",
                    RestaurantImage = "",
                    CityId = 1,
                    City = new City
                    {
                        CityId = 1,
                        Name = "Pune",
                        StateId = 1,
                        State = new State
                        {
                            StateId = 1,
                            Name = "Maharashtra"
                        }
                    }
                });

            Assert.AreEqual(1, res1.RestaurantId);
        }

        [Order(3)]
        [Test]
        public async Task AddMenuTest()
        {
            var mockServiceLogger = new Mock<ILogger<RestaurantUserServices>>();
            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
            var mockPaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
            var mockRestOwnerRepoLogger = new Mock<ILogger<RestaurantOwnerRepository>>();
            ;
            IRepository<int, String, City> cityRepo = new CityRepository(context);
            IRepository<int, String, Restaurant> restRepo = new RestaurantRepository(context);
            IRepository<int, String, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
            IRepository<int, String, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaymentRepoLogger.Object);
            IRepository<int, string, User> userRepo = new UserRepository(context);
            IRepository<int, string, RestaurantOwner> restOwnerRepo = new RestaurantOwnerRepository(context, mockRestOwnerRepoLogger.Object);

            IRestaurantUserServices services = new RestaurantUserServices(restRepo, cityRepo, menuRepo, paymentRepo, orderRepo, userRepo, restOwnerRepo, mockServiceLogger.Object);

            var menu = await services.AddMenuItem(new Menu
            {
                Name = "TestItem1",
                Type = "TestType",
                Price = 20,
                Description = "This is description",
                Cuisine = "Local",
                CookingTime = new TimeSpan(0, 30, 0),
                TasteInfo = "Taste",
                ItemImage = "Sample.jpg",
                NutritionId = 1,
                NutritionalInfo = new NutritionalInfo
                {
                    NutritionId = 1,
                    Calories = 1000,
                    Fats = 1000,
                    Proteins = 1000,
                    Carbohydrates = 1000
                },
                RestaurantId = 1
            });

            Assert.AreEqual(1, menu.MenuId);
        }

        [Order(4)]
        [Test]
        //[TestCase(1, 1)]
        public async Task AddToCartTest()
        {
            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
            IRepository<int, string, User> userRepo = new UserRepository(context);
            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
            IRepository<int, string, City> cityRepo = new CityRepository(context);
            //ITokenServices tokenServices = new TokenServices(IConfiguration);

            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

            var cartMenu = await services.AddToCart(1,1);

            Assert.AreEqual(1, cartMenu.MenuItemId);
            Assert.AreEqual(1, cartMenu.CustomerId);
            Assert.AreEqual(1, cartMenu.Quantity);
        }

        [Test]
        public async Task LoginUserTest()
        {
            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
            IRepository<int, string, User> userRepo = new UserRepository(context);
            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
            IRepository<int, string, City> cityRepo = new CityRepository(context);

            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

            var loginUser = await services.LogIn(
                new LoginUserDTO
                {
                    UserName = "pranav",
                    Password = "pranav123"
                });

            Assert.AreEqual("pranav", loginUser.UserName);
            Assert.AreEqual("Customer", loginUser.Role);
        }

        [Test]
        [TestCase("Pune")]
        public async Task GetByCityTest(string city)
        {
            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
            IRepository<int, string, User> userRepo = new UserRepository(context);
            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
            IRepository<int, string, City> cityRepo = new CityRepository(context);

            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

            var restaurants = await services.GetRestaurantsByCity(city);

            Assert.IsNotNull(restaurants);
        }

        [Test]
        [TestCase(1)]
        public async Task GetMenuByRestaurantTest(int RestaurantId)
        {
            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
            IRepository<int, string, User> userRepo = new UserRepository(context);
            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
            IRepository<int, string, City> cityRepo = new CityRepository(context);

            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

            var result = await services.GetMenuByRestaurant(RestaurantId);

            Assert.IsNotNull(result);
        }

        [Test]
        [TestCase("Cafe Express")]
        public async Task GetRestaurantByName(string name)
        {
            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
            IRepository<int, string, User> userRepo = new UserRepository(context);
            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
            IRepository<int, string, City> cityRepo = new CityRepository(context);

            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

            var result = await services.GetRestaurantByName(name);

            Assert.AreEqual(name, result.RestaurantName);
        }

        [Test]
        [TestCase(1)]
        public async Task GetCartsTest(int userId)
        {
            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
            IRepository<int, string, User> userRepo = new UserRepository(context);
            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
            IRepository<int, string, City> cityRepo = new CityRepository(context);

            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

            var cart = await services.AddToCart(1, 1);
            var carts = await services.GetCarts(userId);

            Assert.IsNotNull(carts);
        }

        [Test]
        [TestCase(1)]
        public async Task DeleteCartItemTest(int cartItemId)
        {
            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
            IRepository<int, string, User> userRepo = new UserRepository(context);
            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
            IRepository<int, string, City> cityRepo = new CityRepository(context);

            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

            //var cart = await services.AddToCart(1, 1);
            //var cart2 = await services.AddToCart(1, 1);
            await services.DeleteCartItem(1);
            var carts = await services.GetCarts(1);

            Assert.IsNull(carts);
        }

        [Test]
        [TestCase(1)]
        public async Task EmptyCartTest(int customerId)
        {
            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
            IRepository<int, string, User> userRepo = new UserRepository(context);
            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
            IRepository<int, string, City> cityRepo = new CityRepository(context);

            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

            var cart = await services.AddToCart(1, 1);
            await services.EmptyCart(customerId);
            var carts = await services.GetCarts(customerId);

            Assert.IsNull(carts);
        }

        [Test]
        [TestCase(1)]
        public async Task IncreaseCartItemQuantityTest(int cartId)
        {
            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
            IRepository<int, string, User> userRepo = new UserRepository(context);
            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
            IRepository<int, string, City> cityRepo = new CityRepository(context);

            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

            var cart = await services.IncreaseCartItemQuantity(cartId);

            Assert.AreEqual(2, cart.Quantity);
        }

        [Test]
        [TestCase(1)]
        public async Task DecreaseCartItemQuantityTest(int cartId)
        {
            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
            IRepository<int, string, User> userRepo = new UserRepository(context);
            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
            IRepository<int, string, City> cityRepo = new CityRepository(context);

            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

            var cart = await services.DecreaseCartItemQuantity(cartId);

            Assert.IsNull(cart);
        }

        [Test]
        [TestCase(1, "online")]
        public async Task PlaceOrderForOne(int cartId, string paymentMode)
        {
            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
            IRepository<int, string, User> userRepo = new UserRepository(context);
            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
            IRepository<int, string, City> cityRepo = new CityRepository(context);

            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

            var orderMenu = await services.PlaceOrderForOne(cartId, paymentMode);
            Assert.IsNotNull(orderMenu);
            Assert.AreEqual("TestItem1", orderMenu.menuName[0].ManuItemName);
        }

        [Test]
        [TestCase(1, "online")]
        public async Task PlaceOrderForAll(int customerId, string paymentMode)
        {
            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
            IRepository<int, string, User> userRepo = new UserRepository(context);
            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
            IRepository<int, string, City> cityRepo = new CityRepository(context);

            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

            var menuOrder = await services.PlaceOrder(customerId, paymentMode);
            Assert.IsNotNull(menuOrder);
            Assert.AreEqual("TestItem1", menuOrder.menuName[0].ManuItemName);
        }
    }
}
