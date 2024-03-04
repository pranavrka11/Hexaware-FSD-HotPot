using HotPot.Interfaces;
using HotPot.Models;
using HotPot.Services;
using Microsoft.Extensions.Logging;
using Moq;

[TestFixture]
public class DeliveryPartnerServicesTests
{
    private Mock<IRepository<int, string, Order>> _orderRepoMock;
    private Mock<IRepository<int, string, DeliveryPartner>> _deliveryPartnerRepoMock;
    private Mock<IRepository<int, string, User>> _userRepoMock;
    private Mock<ITokenServices> _tokenServicesMock;
    private Mock<ILogger<DeliveryPartnerServices>> _loggerMock;
    private DeliveryPartnerServices _deliveryPartnerServices;

    [SetUp]
    public void Setup()
    {
        _orderRepoMock = new Mock<IRepository<int, string, Order>>();
        _deliveryPartnerRepoMock = new Mock<IRepository<int, string, DeliveryPartner>>();
        _userRepoMock = new Mock<IRepository<int, string, User>>();
        _tokenServicesMock = new Mock<ITokenServices>();
        _loggerMock = new Mock<ILogger<DeliveryPartnerServices>>();

        _deliveryPartnerServices = new DeliveryPartnerServices(
            _orderRepoMock.Object,
            _deliveryPartnerRepoMock.Object,
            _userRepoMock.Object,
            _tokenServicesMock.Object,
            _loggerMock.Object);
    }

    [Test]
    public async Task ChangeOrderStatus_ValidOrderId_ReturnsUpdatedOrder()
    {
        // Arrange
        var orderId = 1;
        var order = new Order { OrderId = orderId, Status = "pending" };
        _orderRepoMock.Setup(r => r.GetAsync(orderId)).ReturnsAsync(order);
        _orderRepoMock.Setup(r => r.Update(order)).ReturnsAsync(order);

        // Act
        var result = await _deliveryPartnerServices.ChangeOrderStatus(orderId);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual("delivered", result.Status);
    }

    [Test]
    public async Task GetDeliveryPartnerDetails_ValidPartnerId_ReturnsDeliveryPartner()
    {
        // Arrange
        var partnerId = 1;
        var deliveryPartner = new DeliveryPartner { PartnerId = partnerId };
        _deliveryPartnerRepoMock.Setup(r => r.GetAsync(partnerId)).ReturnsAsync(deliveryPartner);

        // Act
        var result = await _deliveryPartnerServices.GetDeliveryPartnerDetails(partnerId);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(partnerId, result.PartnerId);
    }

    [Test]
    public async Task UpdateDeliveryPartnerDetails_ValidDeliveryPartner_ReturnsUpdatedDeliveryPartner()
    {
        // Arrange
        var deliveryPartner = new DeliveryPartner { PartnerId = 1 };
        _deliveryPartnerRepoMock.Setup(r => r.Update(deliveryPartner)).ReturnsAsync(deliveryPartner);

        // Act
        var result = await _deliveryPartnerServices.UpdateDeliveryPartnerDetails(deliveryPartner);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(1, result.PartnerId);
    }

    [Test]
    public async Task GetAllOrders_ValidPartnerId_ReturnsOrdersForPartner()
    {
        // Arrange
        var partnerId = 1;
        var orders = new List<Order>
        {
            new Order { OrderId = 1, PartnerId = partnerId },
            new Order { OrderId = 2, PartnerId = 2 }
        };
        _orderRepoMock.Setup(r => r.GetAsync()).ReturnsAsync(orders);

        // Act
        var result = await _deliveryPartnerServices.GetAllOrders(partnerId);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(partnerId, result[0].PartnerId);
    }
}
