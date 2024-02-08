using Castle.Core.Logging;
using HotPot.Contexts;
using Microsoft.EntityFrameworkCore;
using Moq;
using HotPot.Repositories;
using Microsoft.Extensions.Logging;
using HotPot.Services;
using HotPot.Interfaces;
using HotPot.Models;

namespace HotPotTests
{
    public class RestaurantUserTests
    {
        RequestTrackerContext context;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<RequestTrackerContext>().UseInMemoryDatabase("TestDb").Options;
            context = new RequestTrackerContext(options);
        }

        [Order(1)]
        [Test]
        public async Task AddTest()
        {
            var mockServiceLogger = new Mock<ILogger<RestaurantUserServices>>();
            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
            var mockPaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
            ;
            IRepository<int, String, City> cityRepo = new CityRepository(context);
            IRepository<int, String, Restaurant> restRepo = new RestaurantRepository(context);
            IRepository<int, String, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
            IRepository<int, String, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaymentRepoLogger.Object);

            IRestaurantUserServices services = new RestaurantUserServices(restRepo, cityRepo, menuRepo, paymentRepo, orderRepo, mockServiceLogger.Object);

            var res1 = await services.AddRestaurant(
                new Restaurant
                {
                    RestaurantName="Cafe Express",
                    Phone="7785456985",
                    Email="abc@def.com",
                    RestaurantImage="",
                    CityId=1,
                    City=new City
                    {
                        CityId=1,
                        Name="Pune",
                        StateId=1,
                        State=new State
                        {
                            StateId=1,
                            Name="Maharashtra"
                        }
                    }
                });

            Assert.AreEqual(1, res1.RestaurantId);
        }

        [Test]
        [TestCase("Pune")]
        public async Task GetByCityTest(string city)
        {
            var mockServiceLogger = new Mock<ILogger<RestaurantUserServices>>();
            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
            var mockPaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
            ;
            IRepository<int, String, City> cityRepo = new CityRepository(context);
            IRepository<int, String, Restaurant> restRepo = new RestaurantRepository(context);
            IRepository<int, String, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
            IRepository<int, String, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaymentRepoLogger.Object);

            IRestaurantUserServices services = new RestaurantUserServices(restRepo, cityRepo, menuRepo, paymentRepo, orderRepo, mockServiceLogger.Object);

            var restaurants = await services.GetRestaurantsByCity(city);

            Assert.IsNotNull(restaurants);
        }

        [Test]
        public async Task AddMenuTest()
        {
            var mockServiceLogger = new Mock<ILogger<RestaurantUserServices>>();
            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
            var mockPaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
            ;
            IRepository<int, String, City> cityRepo = new CityRepository(context);
            IRepository<int, String, Restaurant> restRepo = new RestaurantRepository(context);
            IRepository<int, String, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
            IRepository<int, String, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
            IRepository<int, string, Payment> paymentRepo=new PaymentRepository(context, mockPaymentRepoLogger.Object);

            IRestaurantUserServices services = new RestaurantUserServices(restRepo, cityRepo, menuRepo, paymentRepo, orderRepo, mockServiceLogger.Object);

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

        [Test]
        [TestCase(1)]
        public async Task GetPaymentsForRestaurantTest(int id)
        {
            var mockServiceLogger = new Mock<ILogger<RestaurantUserServices>>();
            var mockMenuRepoLogger = new Mock<ILogger<MenuRepository>>();
            var mockOrderRepoLogger = new Mock<ILogger<OrderRepository>>();
            var mockPaymentRepoLogger = new Mock<ILogger<PaymentRepository>>();
            ;
            IRepository<int, String, City> cityRepo = new CityRepository(context);
            IRepository<int, String, Restaurant> restRepo = new RestaurantRepository(context);
            IRepository<int, String, Menu> menuRepo = new MenuRepository(context, mockMenuRepoLogger.Object);
            IRepository<int, String, Order> orderRepo = new OrderRepository(context, mockOrderRepoLogger.Object);
            IRepository<int, string, Payment> paymentRepo = new PaymentRepository(context, mockPaymentRepoLogger.Object);

            IRestaurantUserServices services = new RestaurantUserServices(restRepo, cityRepo, menuRepo, paymentRepo, orderRepo, mockServiceLogger.Object);

            var payments = await services.GetAllPayments(id);

            Assert.IsNotNull(payments);
        }
    }
}