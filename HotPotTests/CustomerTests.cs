//using Castle.Core.Configuration;
//using HotPot.Contexts;
//using HotPot.Interfaces;
//using HotPot.Models;
//using HotPot.Models.DTO;
//using HotPot.Repositories;
//using HotPot.Services;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace HotPotTests
//{
//    internal class CustomerTests
//    {
//        RequestTrackerContext context;

//        [SetUp]
//        public void SetUp()
//        {
//            var options = new DbContextOptionsBuilder<RequestTrackerContext>().UseInMemoryDatabase("TestDb").Options;
//            context = new RequestTrackerContext(options);
//        }

//        [Order(1)]
//        [Test]
//        public async Task RegisterCustomerTest()
//        {
//            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
//            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
//            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
//            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
//            var mockAddressRepoLogger = new Mock<ILogger<CustomerAddressRepository>>();
//            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();
//            var mockPartnerRepoLogger = new Mock<ILogger<DeliveryPartnerRepository>>();
//            var mockTokenService=new Mock<TokenServices>();

//            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
//            IRepository<int, string, User> userRepo = new UserRepository(context);
//            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
//            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
//            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
//            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
//            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
//            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
//            IRepository<int, string, City> cityRepo = new CityRepository(context);
//            IRepository<int, string, CustomerReview> custReviewRepo = new CustomerReviewRepository(context);
//            IRepository<int, string, CustomerAddress> custAddressRepo = new CustomerAddressRepository(context, mockAddressRepoLogger.Object);
//            IRepository<int, string, DeliveryPartner> deliveryPartnerRepo = new DeliveryPartnerRepository(context, mockPartnerRepoLogger.Object);

//            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, custReviewRepo, custAddressRepo, deliveryPartnerRepo, mockCustomerServiceLogger.Object);

//            var customer = await services.RegisterCustomer(new RegisterCustomerDTO
//            {
//                Name = "Pranav Karlekar",
//                Email = "pranav@abc.com",
//                Phone = "5584201236",
//                UserName = "pranav",
//                Password = "pranav123",
//                Role = "customer"
//            });

//            Assert.AreEqual("pranav", customer.UserName);
//            Assert.AreEqual("Customer", customer.Role);
//        }

//        [Order(2)]
//        [Test]
//        public async Task AddTest()
//        {
//            var mockServiceLogger = new Mock<ILogger<RestaurantUserServices>>();
//            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
//            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
//            var mockPaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
//            var mockRestOwnerRepoLogger = new Mock<ILogger<RestaurantOwnerRepository>>();

//            IRepository<int, String, City> cityRepo = new CityRepository(context);
//            IRepository<int, String, Restaurant> restRepo = new RestaurantRepository(context);
//            IRepository<int, String, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
//            IRepository<int, String, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
//            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaymentRepoLogger.Object);
//            IRepository<int, string, User> userRepo = new UserRepository(context);
//            IRepository<int, string, RestaurantOwner> restOwnerRepo = new RestaurantOwnerRepository(context, mockRestOwnerRepoLogger.Object);

//            IRestaurantUserServices services = new RestaurantUserServices(restRepo, cityRepo, menuRepo, paymentRepo, orderRepo, userRepo, restOwnerRepo, mockServiceLogger.Object);

//            var res1 = await services.AddRestaurant(
//                new Restaurant
//                {
//                    RestaurantName = "Cafe Express",
//                    Phone = "7785456985",
//                    Email = "abc@def.com",
//                    RestaurantImage = "",
//                    CityId = 1,
//                    City = new City
//                    {
//                        CityId = 1,
//                        Name = "Pune",
//                        StateId = 1,
//                        State = new State
//                        {
//                            StateId = 1,
//                            Name = "Maharashtra"
//                        }
//                    }
//                });

//            Assert.AreEqual(1, res1.RestaurantId);
//        }

//        [Order(3)]
//        [Test]
//        public async Task AddMenuTest()
//        {
//            var mockServiceLogger = new Mock<ILogger<RestaurantUserServices>>();
//            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
//            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
//            var mockPaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
//            var mockRestOwnerRepoLogger = new Mock<ILogger<RestaurantOwnerRepository>>();
//            ;
//            IRepository<int, String, City> cityRepo = new CityRepository(context);
//            IRepository<int, String, Restaurant> restRepo = new RestaurantRepository(context);
//            IRepository<int, String, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
//            IRepository<int, String, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
//            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaymentRepoLogger.Object);
//            IRepository<int, string, User> userRepo = new UserRepository(context);
//            IRepository<int, string, RestaurantOwner> restOwnerRepo = new RestaurantOwnerRepository(context, mockRestOwnerRepoLogger.Object);

//            IRestaurantUserServices services = new RestaurantUserServices(restRepo, cityRepo, menuRepo, paymentRepo, orderRepo, userRepo, restOwnerRepo, mockServiceLogger.Object);

//            var menu = await services.AddMenuItem(new Menu
//            {
//                Name = "TestItem1",
//                Type = "TestType",
//                Price = 20,
//                Description = "This is description",
//                Cuisine = "Local",
//                CookingTime = new TimeSpan(0, 30, 0),
//                TasteInfo = "Taste",
//                ItemImage = "Sample.jpg",
//                NutritionId = 1,
//                NutritionalInfo = new NutritionalInfo
//                {
//                    NutritionId = 1,
//                    Calories = 1000,
//                    Fats = 1000,
//                    Proteins = 1000,
//                    Carbohydrates = 1000
//                },
//                RestaurantId = 1
//            });

//            Assert.AreEqual(1, menu.MenuId);
//        }

//        [Order(4)]
//        [Test]
//        //[TestCase(1, 1)]
//        public async Task AddToCartTest()
//        {
//            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
//            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
//            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
//            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
//            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

//            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
//            IRepository<int, string, User> userRepo = new UserRepository(context);
//            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
//            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
//            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
//            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
//            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
//            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
//            IRepository<int, string, City> cityRepo = new CityRepository(context);
//            //ITokenServices tokenServices = new TokenServices(IConfiguration);

//            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

//            var cartMenu = await services.AddToCart(1, 1);

//            Assert.AreEqual(1, cartMenu.MenuItemId);
//            Assert.AreEqual(1, cartMenu.CustomerId);
//            Assert.AreEqual(1, cartMenu.Quantity);
//        }

//        [Test]
//        public async Task LoginUserTest()
//        {
//            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
//            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
//            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
//            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
//            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

//            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
//            IRepository<int, string, User> userRepo = new UserRepository(context);
//            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
//            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
//            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
//            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
//            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
//            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
//            IRepository<int, string, City> cityRepo = new CityRepository(context);

//            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

//            var loginUser = await services.LogIn(
//                new LoginUserDTO
//                {
//                    UserName = "pranav",
//                    Password = "pranav123"
//                });

//            Assert.AreEqual("pranav", loginUser.UserName);
//            Assert.AreEqual("Customer", loginUser.Role);
//        }

//        [Test]
//        [TestCase("Pune")]
//        public async Task GetByCityTest(string city)
//        {
//            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
//            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
//            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
//            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
//            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

//            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
//            IRepository<int, string, User> userRepo = new UserRepository(context);
//            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
//            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
//            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
//            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
//            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
//            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
//            IRepository<int, string, City> cityRepo = new CityRepository(context);

//            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

//            var restaurants = await services.GetRestaurantsByCity(city);

//            Assert.IsNotNull(restaurants);
//        }

//        [Test]
//        [TestCase(1)]
//        public async Task GetMenuByRestaurantTest(int RestaurantId)
//        {
//            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
//            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
//            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
//            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
//            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

//            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
//            IRepository<int, string, User> userRepo = new UserRepository(context);
//            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
//            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
//            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
//            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
//            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
//            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
//            IRepository<int, string, City> cityRepo = new CityRepository(context);

//            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

//            var result = await services.GetMenuByRestaurant(RestaurantId);

//            Assert.IsNotNull(result);
//        }

//        [Test]
//        [TestCase("Cafe Express")]
//        public async Task GetRestaurantByName(string name)
//        {
//            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
//            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
//            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
//            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
//            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

//            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
//            IRepository<int, string, User> userRepo = new UserRepository(context);
//            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
//            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
//            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
//            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
//            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
//            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
//            IRepository<int, string, City> cityRepo = new CityRepository(context);

//            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

//            var result = await services.GetRestaurantByName(name);

//            Assert.AreEqual(name, result.RestaurantName);
//        }

//        [Test]
//        [TestCase(1)]
//        public async Task GetCartsTest(int userId)
//        {
//            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
//            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
//            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
//            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
//            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

//            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
//            IRepository<int, string, User> userRepo = new UserRepository(context);
//            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
//            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
//            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
//            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
//            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
//            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
//            IRepository<int, string, City> cityRepo = new CityRepository(context);

//            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

//            var cart = await services.AddToCart(1, 1);
//            var carts = await services.GetCarts(userId);

//            Assert.IsNotNull(carts);
//        }

//        [Test]
//        [TestCase(1)]
//        public async Task DeleteCartItemTest(int cartItemId)
//        {
//            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
//            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
//            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
//            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
//            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

//            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
//            IRepository<int, string, User> userRepo = new UserRepository(context);
//            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
//            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
//            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
//            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
//            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
//            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
//            IRepository<int, string, City> cityRepo = new CityRepository(context);

//            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

//            //var cart = await services.AddToCart(1, 1);
//            //var cart2 = await services.AddToCart(1, 1);
//            await services.DeleteCartItem(1);
//            var carts = await services.GetCarts(1);

//            Assert.IsNull(carts);
//        }

//        [Test]
//        [TestCase(1)]
//        public async Task EmptyCartTest(int customerId)
//        {
//            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
//            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
//            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
//            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
//            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

//            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
//            IRepository<int, string, User> userRepo = new UserRepository(context);
//            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
//            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
//            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
//            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
//            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
//            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
//            IRepository<int, string, City> cityRepo = new CityRepository(context);

//            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

//            var cart = await services.AddToCart(1, 1);
//            await services.EmptyCart(customerId);
//            var carts = await services.GetCarts(customerId);

//            Assert.IsNull(carts);
//        }

//        [Test]
//        [TestCase(1)]
//        public async Task IncreaseCartItemQuantityTest(int cartId)
//        {
//            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
//            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
//            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
//            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
//            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

//            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
//            IRepository<int, string, User> userRepo = new UserRepository(context);
//            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
//            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
//            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
//            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
//            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
//            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
//            IRepository<int, string, City> cityRepo = new CityRepository(context);

//            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

//            var cart = await services.IncreaseCartItemQuantity(cartId);

//            Assert.AreEqual(2, cart.Quantity);
//        }

//        [Test]
//        [TestCase(1)]
//        public async Task DecreaseCartItemQuantityTest(int cartId)
//        {
//            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
//            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
//            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
//            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
//            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

//            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
//            IRepository<int, string, User> userRepo = new UserRepository(context);
//            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
//            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
//            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
//            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
//            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
//            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
//            IRepository<int, string, City> cityRepo = new CityRepository(context);

//            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

//            var cart = await services.DecreaseCartItemQuantity(cartId);

//            Assert.IsNull(cart);
//        }

//        [Test]
//        [TestCase(1, "online")]
//        public async Task PlaceOrderForOne(int cartId, string paymentMode)
//        {
//            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
//            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
//            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
//            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
//            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

//            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
//            IRepository<int, string, User> userRepo = new UserRepository(context);
//            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
//            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
//            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
//            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
//            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
//            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
//            IRepository<int, string, City> cityRepo = new CityRepository(context);

//            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

//            var orderMenu = await services.PlaceOrderForOne(cartId, paymentMode);
//            Assert.IsNotNull(orderMenu);
//            Assert.AreEqual("TestItem1", orderMenu.menuName[0].ManuItemName);
//        }

//        [Test]
//        [TestCase(1, "online")]
//        public async Task PlaceOrderForAll(int customerId, string paymentMode)
//        {
//            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
//            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
//            var mockOrderItemRepoLogger = new Mock<ILogger<OrderItemRepository>>();
//            var mockPaaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
//            var mockCustomerServiceLogger = new Mock<ILogger<CustomerServices>>();

//            IRepository<int, string, Customer> custRepo = new CustomerRepository(context);
//            IRepository<int, string, User> userRepo = new UserRepository(context);
//            IRepository<int, string, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
//            IRepository<int, string, Cart> cartRepo = new CartRepository(context);
//            IRepository<int, string, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
//            IRepository<int, string, OrderItem> orderItemRepo = new OrderItemRepository(context, mockOrderItemRepoLogger.Object);
//            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaaymentRepoLogger.Object);
//            IRepository<int, string, Restaurant> restaurantRepo = new RestaurantRepository(context);
//            IRepository<int, string, City> cityRepo = new CityRepository(context);

//            ICustomerServices services = new CustomerServices(custRepo, userRepo, menuRepo, cartRepo, orderRepo, orderItemRepo, paymentRepo, restaurantRepo, cityRepo, mockCustomerServiceLogger.Object);

//            var menuOrder = await services.PlaceOrder(customerId, paymentMode);
//            Assert.IsNotNull(menuOrder);
//            Assert.AreEqual("TestItem1", menuOrder.menuName[0].ManuItemName);
//        }
//    }
//}


using HotPot.Exceptions;
using HotPot.Interfaces;
using HotPot.Models;
using HotPot.Models.DTO;
using HotPot.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotPot.Tests.Services
{
    [TestFixture]
    public class CustomerServicesTests
    {
        private CustomerServices _customerServices;
        private Mock<IRepository<int, string, Customer>> _custRepoMock;
        private Mock<IRepository<int, string, User>> _userRepoMock;
        private Mock<IRepository<int, string, Menu>> _menuRepoMock;
        private Mock<IRepository<int, string, Cart>> _cartRepoMock;
        private Mock<IRepository<int, string, Order>> _orderRepoMock;
        private Mock<IRepository<int, string, OrderItem>> _orderItemRepoMock;
        private Mock<IRepository<int, string, Payment>> _paymentRepoMock;
        private Mock<IRepository<int, string, Restaurant>> _restaurantRepoMock;
        private Mock<IRepository<int, string, City>> _cityRepoMock;
        private Mock<IRepository<int, string, CustomerAddress>> _custAddressRepoMock;
        private Mock<IRepository<int, string, CustomerReview>> _custReviewRepoMock;
        private Mock<IRepository<int, string, DeliveryPartner>> _deliveryPartnerRepoMock;
        private Mock<ITokenServices> _tokenServicesMock;
        private Mock<ILogger<CustomerServices>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _custRepoMock = new Mock<IRepository<int, string, Customer>>();
            _userRepoMock = new Mock<IRepository<int, string, User>>();
            _menuRepoMock = new Mock<IRepository<int, string, Menu>>();
            _cartRepoMock = new Mock<IRepository<int, string, Cart>>();
            _orderRepoMock = new Mock<IRepository<int, string, Order>>();
            _orderItemRepoMock = new Mock<IRepository<int, string, OrderItem>>();
            _paymentRepoMock = new Mock<IRepository<int, string, Payment>>();
            _restaurantRepoMock = new Mock<IRepository<int, string, Restaurant>>();
            _cityRepoMock = new Mock<IRepository<int, string, City>>();
            _custAddressRepoMock = new Mock<IRepository<int, string, CustomerAddress>>();
            _custReviewRepoMock = new Mock<IRepository<int, string, CustomerReview>>();
            _deliveryPartnerRepoMock = new Mock<IRepository<int, string, DeliveryPartner>>();
            _tokenServicesMock = new Mock<ITokenServices>();
            _loggerMock = new Mock<ILogger<CustomerServices>>();

            _customerServices = new CustomerServices(
                _custRepoMock.Object,
                _userRepoMock.Object,
                _menuRepoMock.Object,
                _cartRepoMock.Object,
                _orderRepoMock.Object,
                _orderItemRepoMock.Object,
                _paymentRepoMock.Object,
                _restaurantRepoMock.Object,
                _cityRepoMock.Object,
                _custAddressRepoMock.Object,
                _custReviewRepoMock.Object,
                _deliveryPartnerRepoMock.Object,
                _tokenServicesMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public async Task GetMenuByRestaurant_ValidRestaurantId_ReturnsMenuList()
        {
            // Arrange
            var restaurantId = 1;
            var menus = new List<Menu>
            {
                new Menu { RestaurantId = restaurantId },
                new Menu { RestaurantId = 2 },
                new Menu { RestaurantId = restaurantId }
            };
            _menuRepoMock.Setup(repo => repo.GetAsync()).ReturnsAsync(menus);

            // Act
            var result = await _customerServices.GetMenuByRestaurant(restaurantId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(menu => menu.RestaurantId == restaurantId));
        }

        [Test]
        public async Task GetMenuByRestaurant_NoMenuAvailable_ThrowsNoMenuAvailableException()
        {
            // Arrange
            var restaurantId = 1;
            _menuRepoMock.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<Menu>());

            // Act & Assert
            Assert.ThrowsAsync<NoMenuAvailableException>(async () => await _customerServices.GetMenuByRestaurant(restaurantId));
        }

        [Test]
        public async Task GetRestaurantByName_ValidName_ReturnsRestaurant()
        {
            // Arrange
            var restaurantName = "Test Restaurant";
            var restaurant = new Restaurant { RestaurantName = restaurantName };
            _restaurantRepoMock.Setup(repo => repo.GetAsync(restaurantName)).ReturnsAsync(restaurant);

            // Act
            var result = await _customerServices.GetRestaurantByName(restaurantName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(restaurantName, result.RestaurantName);
        }

        [Test]
        public async Task GetRestaurantByName_RestaurantNotFound_ThrowsRestaurantNotFoundException()
        {
            // Arrange
            var restaurantName = "Non-existent Restaurant";
            _restaurantRepoMock.Setup(repo => repo.GetAsync(restaurantName)).ReturnsAsync((Restaurant)null);

            // Act & Assert
            Assert.ThrowsAsync<RestaurantNotFoundException>(async () => await _customerServices.GetRestaurantByName(restaurantName));
        }

        [Test]
        public async Task GetRestaurantsByCity_ValidCity_ReturnsRestaurantList()
        {
            // Arrange
            var cityName = "Test City";
            var city = new City { Name = cityName };
            var restaurants = new List<Restaurant>
            {
                new Restaurant { CityId = city.CityId },
                new Restaurant { CityId = 2 },
                new Restaurant { CityId = city.CityId }
            };
            _cityRepoMock.Setup(repo => repo.GetAsync(cityName)).ReturnsAsync(city);
            _restaurantRepoMock.Setup(repo => repo.GetAsync()).ReturnsAsync(restaurants);

            // Act
            var result = await _customerServices.GetRestaurantsByCity(cityName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(restaurant => restaurant.CityId == city.CityId));
        }

        [Test]
        public async Task GetRestaurantsByCity_CityNotFound_ThrowsCityNotFoundException()
        {
            // Arrange
            var cityName = "Non-existent City";
            _cityRepoMock.Setup(repo => repo.GetAsync(cityName)).ReturnsAsync((City)null);

            // Act & Assert
            Assert.ThrowsAsync<CityNotFoundException>(async () => await _customerServices.GetRestaurantsByCity(cityName));
        }

        [Test]
        public async Task GetRestaurantsByCity_NoRestaurantsFound_ThrowsRestaurantNotFoundException()
        {
            // Arrange
            var cityName = "Test City";
            var city = new City { Name = cityName };
            _cityRepoMock.Setup(repo => repo.GetAsync(cityName)).ReturnsAsync(city);
            _restaurantRepoMock.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<Restaurant>());

            // Act & Assert
            Assert.ThrowsAsync<RestaurantNotFoundException>(async () => await _customerServices.GetRestaurantsByCity(cityName));
        }

        [Test]
        public async Task PlaceOrder_ValidCustomerIdAndPaymentMode_ReturnsOrderMenuDTO()
        {
            // Arrange
            var customerId = 1;
            var paymentMode = "online";
            var carts = new List<Cart>
    {
        new Cart { CustomerId = customerId, Status = "added", MenuItemId = 1, Quantity = 2 },
        new Cart { CustomerId = 2, Status = "added", MenuItemId = 2, Quantity = 1 },
        new Cart { CustomerId = customerId, Status = "added", MenuItemId = 3, Quantity = 3 }
    };
            _cartRepoMock.Setup(r => r.GetAsync()).ReturnsAsync(carts);
            var menuItems = new List<Menu>
    {
        new Menu { MenuId = 1, Price = 10 },
        new Menu { MenuId = 2, Price = 15 },
        new Menu { MenuId = 3, Price = 20 }
    };
            _menuRepoMock.Setup(r => r.GetAsync(1)).ReturnsAsync(menuItems[0]);
            _menuRepoMock.Setup(r => r.GetAsync(3)).ReturnsAsync(menuItems[2]);
            var restaurantId = 1;
            var restaurant = new Restaurant { RestaurantId = restaurantId, CityId = 1 };
            _restaurantRepoMock.Setup(r => r.GetAsync(restaurantId)).ReturnsAsync(restaurant);
            var deliveryPartners = new List<DeliveryPartner>
    {
        new DeliveryPartner { PartnerId = 1, CityId = 1, Name = "Partner1" },
        new DeliveryPartner { PartnerId = 2, CityId = 1, Name = "Partner2" }
    };
            _deliveryPartnerRepoMock.Setup(r => r.GetAsync()).ReturnsAsync(deliveryPartners);
            _deliveryPartnerRepoMock.Setup(r => r.GetAsync(1)).ReturnsAsync(deliveryPartners[0]);
            var newOrder = new Order { OrderId = 1, Amount = 80, CustomerId = customerId, RestaurantId = restaurantId };
            _orderRepoMock.Setup(r => r.Add(newOrder)).ReturnsAsync(newOrder);
            var newOrderItem = new OrderItem { OrderId = newOrder.OrderId, MenuId = 1, Quantity = 2, SubTotalPrice = 20 };
            _orderItemRepoMock.Setup(r => r.Add(newOrderItem)).ReturnsAsync(newOrderItem);
            var payment = new Payment { PaymentId = 1, Status = "successful" };
            _paymentRepoMock.Setup(r => r.Update(payment)).ReturnsAsync(payment);

            // Act
            var result = await _customerServices.PlaceOrder(customerId, paymentMode);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(newOrder.OrderId, result.orderId);
            Assert.AreEqual(customerId, result.customerId);
            Assert.AreEqual(restaurantId, result.restaurantId);
            Assert.AreEqual(80, result.Price);
            Assert.AreEqual("placed", result.Status);
            Assert.NotNull(result.partnerId);
            Assert.NotNull(result.PartnerName);
            Assert.IsNotEmpty(result.menuName);
            Assert.AreEqual(2, result.menuName.Count);
        }

        [Test]
        public async Task RecordPayment_ValidOrder_ReturnsPayment()
        {
            // Arrange
            var order = new Order { OrderId = 1, Amount = 100 };

            // Act
            var result = await _customerServices.RecordPayment(order);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("online", result.PaymentMode);
            Assert.AreEqual(100, result.Amount);
            Assert.AreEqual("successful", result.Status);
        }

        [Test]
        public async Task AddToCart_ValidUserIdAndMenuItemId_ReturnsCartMenuDTO()
        {
            // Arrange
            var userId = 1;
            var menuItemId = 1;
            var menuItem = new Menu { MenuId = menuItemId, RestaurantId = 1, Price = 10 };
            _menuRepoMock.Setup(r => r.GetAsync(menuItemId)).ReturnsAsync(menuItem);
            var cartItems = new List<Cart>
    {
        new Cart { Id = 1, CustomerId = userId, MenuItemId = 2, Quantity = 1, Status = "added" },
        new Cart { Id = 2, CustomerId = userId, MenuItemId = 3, Quantity = 2, Status = "added" }
    };
            _cartRepoMock.Setup(r => r.GetAsync()).ReturnsAsync(cartItems);
            _cartRepoMock.Setup(r => r.Add(It.IsAny<Cart>())).ReturnsAsync((Cart c) => c);

            // Act
            var result = await _customerServices.AddToCart(userId, menuItemId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.CartId);
            Assert.AreEqual(userId, result.CustomerId);
            Assert.AreEqual(menuItem.RestaurantId, result.RestaurantId);
            Assert.AreEqual(menuItemId, result.MenuItemId);
            Assert.AreEqual(menuItem.Name, result.MenuTitle);
            Assert.AreEqual(1, result.Quantity);
            Assert.AreEqual(10, result.Price);
        }

    //    [Test]
    //    public async Task GetCarts_ValidCustomerId_ReturnsListCartMenuDTO()
    //    {
    //        // Arrange
    //        var customerId = 1;
    //        var cartItems = new List<Cart>
    //{
    //    new Cart { Id = 1, CustomerId = customerId, MenuItemId = 1, Quantity = 1, Status = "added" },
    //    new Cart { Id = 2, CustomerId = customerId, MenuItemId = 2, Quantity = 2, Status = "added" }
    //};
    //        _cartRepoMock.Setup(r => r.GetAsync()).ReturnsAsync(cartItems);
    //        _restaurantRepoMock.Setup(r => r.GetAsync(1)).ReturnsAsync(new Restaurant { RestaurantName = "Restaurant1", CityId = 1 });
    //        _menuRepoMock.Setup(r => r.GetAsync(1)).ReturnsAsync(new Menu { MenuId = 1, Name = "Menu1", Price = 10, ItemImage = "Image1" });

    //        // Act
    //        var result = await _customerServices.GetCarts(customerId);

    //        // Assert
    //        Assert.NotNull(result);
    //        //Assert.AreEqual(2, result.Count);
    //        //Assert.AreEqual(1, result[0].CartId);
    //        //Assert.AreEqual(customerId, result[0].CustomerId);
    //        //Assert.AreEqual(1, result[0].RestaurantId);
    //        //Assert.AreEqual(1, result[0].MenuItemId);
    //        //Assert.AreEqual("Menu1", result[0].MenuTitle);
    //        //Assert.AreEqual(1, result[0].Quantity);
    //        //Assert.AreEqual(10, result[0].Price);
    //        //Assert.AreEqual("Image1", result[0].MenuImage);
    //        //Assert.AreEqual("Restaurant1", result[0].RestaurantName);
    //        //Assert.AreEqual(1, result[0].RestaurantCityId);
    //    }

        [Test]
        public async Task DeleteCartItem_ValidCartItemId_CallsCartRepoUpdate()
        {
            // Arrange
            var cartItemId = 1;
            var cartItem = new Cart { Id = cartItemId, Status = "added" };
            _cartRepoMock.Setup(r => r.GetAsync(cartItemId)).ReturnsAsync(cartItem);

            // Act
            await _customerServices.DeleteCartItem(cartItemId);

            // Assert
            _cartRepoMock.Verify(r => r.Update(cartItem), Times.Once);
        }

        [Test]
        public async Task EmptyCart_ValidCustomerId_CallsCartRepoUpdateForEachCartItem()
        {
            // Arrange
            var customerId = 1;
            var cartItems = new List<Cart>
    {
        new Cart { Id = 1, CustomerId = customerId, Status = "added" },
        new Cart { Id = 2, CustomerId = customerId, Status = "added" }
    };
            _cartRepoMock.Setup(r => r.GetAsync()).ReturnsAsync(cartItems);

            // Act
            await _customerServices.EmptyCart(customerId);

            // Assert
            _cartRepoMock.Verify(r => r.Update(It.IsAny<Cart>()), Times.Exactly(2));
        }

        [Test]
        public async Task IncreaseCartItemQuantity_ValidCartId_CallsCartRepoUpdate()
        {
            // Arrange
            var cartId = 1;
            var cartItem = new Cart { Id = cartId, Quantity = 1 };
            _cartRepoMock.Setup(r => r.GetAsync(cartId)).ReturnsAsync(cartItem);

            // Act
            await _customerServices.IncreaseCartItemQuantity(cartId);

            // Assert
            _cartRepoMock.Verify(r => r.Update(cartItem), Times.Once);
        }

        [Test]
        public async Task DecreaseCartItemQuantity_QuantityGreaterThan1_CallsCartRepoUpdate()
        {
            // Arrange
            var cartId = 1;
            var cartItem = new Cart { Id = cartId, Quantity = 2 };
            _cartRepoMock.Setup(r => r.GetAsync(cartId)).ReturnsAsync(cartItem);

            // Act
            await _customerServices.DecreaseCartItemQuantity(cartId);

            // Assert
            _cartRepoMock.Verify(r => r.Update(cartItem), Times.Once);
        }

        [Test]
        public async Task DecreaseCartItemQuantity_QuantityEquals1_CallsDeleteCartItem()
        {
            // Arrange
            var cartId = 1;
            var cartItem = new Cart { Id = cartId, Quantity = 1 };
            _cartRepoMock.Setup(r => r.GetAsync(cartId)).ReturnsAsync(cartItem);

            // Act
            await _customerServices.DecreaseCartItemQuantity(cartId);

            // Assert
            _cartRepoMock.Verify(r => r.Update(cartItem), Times.Once);
        }

        [Test]
        public async Task ViewOrderHistory_ValidCustomerId_ReturnsListOrderMenuDTO()
        {
            // Arrange
            var customerId = 1;
            var orders = new List<Order>
    {
        new Order { OrderId = 1, CustomerId = customerId, RestaurantId = 1, Status = "placed", OrderDate = DateTime.Now.AddDays(-1) },
        new Order { OrderId = 2, CustomerId = customerId, RestaurantId = 2, Status = "placed", OrderDate = DateTime.Now }
    };
            _orderRepoMock.Setup(r => r.GetAsync()).ReturnsAsync(orders);
            var orderItems = new List<OrderItem>
    {
        new OrderItem { OrderId = 1, MenuId = 1, Quantity = 2, SubTotalPrice = 20 },
        new OrderItem { OrderId = 2, MenuId = 2, Quantity = 1, SubTotalPrice = 15 }
    };
            _orderItemRepoMock.Setup(r => r.GetAsync()).ReturnsAsync(orderItems);
            _restaurantRepoMock.Setup(r => r.GetAsync(1)).ReturnsAsync(new Restaurant { RestaurantName = "Restaurant1", RestaurantImage = "Image1" });
            _restaurantRepoMock.Setup(r => r.GetAsync(2)).ReturnsAsync(new Restaurant { RestaurantName = "Restaurant2", RestaurantImage = "Image2" });
            _menuRepoMock.Setup(r => r.GetAsync(1)).ReturnsAsync(new Menu { Name = "Menu1" });
            _menuRepoMock.Setup(r => r.GetAsync(2)).ReturnsAsync(new Menu { Name = "Menu2" });

            // Act
            var result = await _customerServices.ViewOrderHistory(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].orderId);
            Assert.AreEqual(customerId, result[0].customerId);
            Assert.AreEqual(1, result[0].restaurantId);
            Assert.AreEqual(20, result[0].Price);
            Assert.AreEqual("placed", result[0].Status);
            Assert.AreEqual("Restaurant1", result[0].RestaurantName);
            Assert.AreEqual("Image1", result[0].RestaurantImage);
            Assert.AreEqual(DateTime.Now.AddDays(-1).Date, result[0].OrderDate.Date);
            Assert.IsNotEmpty(result[0].menuName);
            Assert.AreEqual(1, result[1].orderId);
            Assert.AreEqual(customerId, result[1].customerId);
            Assert.AreEqual(2, result[1].restaurantId);
            Assert.AreEqual(15, result[1].Price);
            Assert.AreEqual("placed", result[1].Status);
            Assert.AreEqual("Restaurant2", result[1].RestaurantName);
            Assert.AreEqual("Image2", result[1].RestaurantImage);
            Assert.AreEqual(DateTime.Now.Date, result[1].OrderDate.Date);
            Assert.IsNotEmpty(result[1].menuName);
        }

        [Test]
        public async Task GetCustomerDetails_ValidCustomerId_ReturnsCustomer()
        {
            // Arrange
            var customerId = 1;
            var customer = new Customer { Id = customerId, Name = "John Doe" };
            _custRepoMock.Setup(r => r.GetAsync(customerId)).ReturnsAsync(customer);

            // Act
            var result = await _customerServices.GetCustomerDetails(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(customerId, result.Id);
            Assert.AreEqual("John Doe", result.Name);
        }

        [Test]
        public async Task UpdateCustomerDetails_ValidCustomer_ReturnsCustomer()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Jane Doe" };
            _custRepoMock.Setup(r => r.GetAsync(1)).ReturnsAsync(customer);
            _custRepoMock.Setup(r => r.Update(customer)).ReturnsAsync(customer);

            // Act
            var result = await _customerServices.UpdateCustomerDetails(customer);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Jane Doe", result.Name);
        }

        [Test]
        public async Task GetAllCities_ReturnsListOfCities()
        {
            // Arrange
            var cities = new List<City>
    {
        new City { CityId = 1, Name = "City1" },
        new City { CityId = 2, Name = "City2" }
    };
            _cityRepoMock.Setup(r => r.GetAsync()).ReturnsAsync(cities);

            // Act
            var result = await _customerServices.GetAllCities();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].CityId);
            Assert.AreEqual("City1", result[0].Name);
            Assert.AreEqual(2, result[1].CityId);
            Assert.AreEqual("City2", result[1].Name);
        }

        [Test]
        public async Task CancelOrderFromCustomer_ValidOrderId_ReturnsCancelledOrder()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { OrderId = orderId, Status = "placed" };
            _orderRepoMock.Setup(r => r.GetAsync(orderId)).ReturnsAsync(order);
            _orderRepoMock.Setup(r => r.Update(order)).ReturnsAsync(order);

            // Act
            var result = await _customerServices.CancelOrderFromCustomer(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(orderId, result.OrderId);
            Assert.AreEqual("cancelled", result.Status);
        }

        [Test]
        public async Task AddCustomerAddress_ValidCustomerAddress_ReturnsAddedCustomerAddress()
        {
            // Arrange
            var customerAddress = new CustomerAddress { AddressId = 1 };
            _custAddressRepoMock.Setup(r => r.Add(customerAddress)).ReturnsAsync(customerAddress);

            // Act
            var result = await _customerServices.AddCustomerAddress(customerAddress);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.AddressId);
        }

        [Test]
        public async Task UpdateCustomerAddress_ValidAddressIdAndAddressUpdateDto_ReturnsUpdatedCustomerAddress()
        {
            // Arrange
            var addressId = 1;
            var addressUpdateDto = new CustomerAddressUpdateDTO { HouseNumber = "123", BuildingName = "Building", Locality = "Locality", CityId = 1, LandMark = "Landmark" };
            var existingAddress = new CustomerAddress { AddressId = addressId };
            _custAddressRepoMock.Setup(r => r.GetAsync(addressId)).ReturnsAsync(existingAddress);
            _custAddressRepoMock.Setup(r => r.Update(existingAddress)).ReturnsAsync(existingAddress);

            // Act
            var result = await _customerServices.UpdateCustomerAddress(addressId, addressUpdateDto);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.AddressId);
            Assert.AreEqual("123", result.HouseNumber);
            Assert.AreEqual("Building", result.BuildingName);
            Assert.AreEqual("Locality", result.Locality);
            Assert.AreEqual(1, result.CityId);
            Assert.AreEqual("Landmark", result.LandMark);
        }

        [Test]
        public async Task ViewCustomerAddressByCustomerId_ValidCustomerId_ReturnsCustomerAddress()
        {
            // Arrange
            var customerId = 1;
            var customerAddress = new CustomerAddress { CustomerId = customerId };
            _custAddressRepoMock.Setup(r => r.GetAsync(customerId)).ReturnsAsync(customerAddress);

            // Act
            var result = await _customerServices.ViewCustomerAddressByCustomerId(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(customerId, result.CustomerId);
        }

        [Test]
        public async Task AddCustomerReview_ValidCustomerReview_ReturnsAddedCustomerReview()
        {
            // Arrange
            var customerReview = new CustomerReview { ReviewId = 1 };
            _custReviewRepoMock.Setup(r => r.Add(customerReview)).ReturnsAsync(customerReview);

            // Act
            var result = await _customerServices.AddCustomerReview(customerReview);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.ReviewId);
        }
    }
}

