using HotPot.Context;
using HotPot.Interfaces;
using HotPot.Models;
using HotPot.Models.DTO;
using HotPot.Repositories;
using HotPot.Services;
using HotPot.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

namespace HotpotTest
{
    [TestFixture]
    public class CustomerUserServiceTests
    {
        private CustomerUserService _customerUserService;
        private Mock<IRepository<int, string, CustomerAddress>> _mockCustomerAddressRepo;
        private Mock<IRepository<int, string, CustomerReview>> _mockCustomerReviewRepo;
        private Mock<IRepository<int, string, Menu>> _mockMenuRepo;
        private Mock<IRepository<int, string, Restaurant>> _mockRestaurantRepo;
        private Mock<IRepository<int, string, City>> _mockCityRepo;
        private Mock<IRepository<int, string, State>> _mockStateRepo;
        private Mock<ILogger<CustomerUserService>> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            _mockCustomerAddressRepo = new Mock<IRepository<int, string, CustomerAddress>>();
            _mockCustomerReviewRepo = new Mock<IRepository<int, string, CustomerReview>>();
            _mockMenuRepo = new Mock<IRepository<int, string, Menu>>();
            _mockRestaurantRepo = new Mock<IRepository<int, string, Restaurant>>();
            _mockCityRepo = new Mock<IRepository<int, string, City>>();
            _mockStateRepo = new Mock<IRepository<int, string, State>>();
            _mockLogger = new Mock<ILogger<CustomerUserService>>();

            _customerUserService = new CustomerUserService(
                _mockCustomerAddressRepo.Object,
                _mockCustomerReviewRepo.Object,
                _mockMenuRepo.Object,
                _mockRestaurantRepo.Object,
                _mockCityRepo.Object,
                _mockStateRepo.Object,
                _mockLogger.Object
            );
        }

        [Test]
        public async Task AddCustomerAddress_ValidAddress_ReturnsAddress()
        {
            // Arrange
            var customerAddress = new CustomerAddress();
            _mockCustomerAddressRepo.Setup(repo => repo.Add(customerAddress))
                .ReturnsAsync(customerAddress);

            // Act
            var result = await _customerUserService.AddCustomerAddress(customerAddress);

            // Assert
            Assert.AreEqual(customerAddress, result);
        }

        [Test]
        public void UpdateCustomerAddress_AddressNotFound_ThrowsException()
        {
            // Arrange
            _mockCustomerAddressRepo.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync((CustomerAddress)null);

            // Act & Assert
            Assert.ThrowsAsync<NoCustomerAddressFoundException>(async () =>
            {
                await _customerUserService.UpdateCustomerAddress(1, new CustomerAddressUpdateDTO());
            });
        }

        [Test]
        public async Task UpdateCustomerAddress_ValidAddress_ReturnsUpdatedAddress()
        {
            // Arrange
            var existingAddress = new CustomerAddress();
            _mockCustomerAddressRepo.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(existingAddress);
            var updatedAddress = new CustomerAddress();
            _mockCustomerAddressRepo.Setup(repo => repo.Update(existingAddress))
                .ReturnsAsync(updatedAddress);
            var addressUpdateDto = new CustomerAddressUpdateDTO();

            // Act
            var result = await _customerUserService.UpdateCustomerAddress(1, addressUpdateDto);

            // Assert
            Assert.AreEqual(updatedAddress, result);
        }

        [Test]
        public void ViewCustomerAddressByCustomerId_AddressNotFound_ThrowsException()
        {
            // Arrange
            _mockCustomerAddressRepo.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync((CustomerAddress)null);

            // Act & Assert
            Assert.ThrowsAsync<NoCustomerAddressFoundException>(async () =>
            {
                await _customerUserService.ViewCustomerAddressByCustomerId(1);
            });
        }

        [Test]
        public async Task ViewCustomerAddressByCustomerId_AddressFound_ReturnsAddress()
        {
            // Arrange
            var customerAddress = new CustomerAddress();
            _mockCustomerAddressRepo.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(customerAddress);

            // Act
            var result = await _customerUserService.ViewCustomerAddressByCustomerId(1);

            // Assert
            Assert.AreEqual(customerAddress, result);
        }

        [Test]
        public async Task AddCustomerReview_ValidReview_ReturnsReview()
        {
            // Arrange
            var customerReview = new CustomerReview();
            _mockCustomerReviewRepo.Setup(repo => repo.Add(customerReview))
                .ReturnsAsync(customerReview);

            // Act
            var result = await _customerUserService.AddCustomerReview(customerReview);

            // Assert
            Assert.AreEqual(customerReview, result);
        }

        [Test]
        public void ViewCustomerReview_ReviewNotFound_ThrowsException()
        {
            // Arrange
            _mockCustomerReviewRepo.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync((CustomerReview)null);

            // Act & Assert
            Assert.ThrowsAsync<NoCustomerReviewFoundException>(async () =>
            {
                await _customerUserService.ViewCustomerReview(1);
            });
        }

        [Test]
        public async Task ViewCustomerReview_ReviewFound_ReturnsReview()
        {
            // Arrange
            var customerReview = new CustomerReview();
            _mockCustomerReviewRepo.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(customerReview);

            // Act
            var result = await _customerUserService.ViewCustomerReview(1);

            // Assert
            Assert.AreEqual(customerReview, result);
        }

        [Test]
        public async Task UpdateCustomerReviewText_ValidReview_ReturnsUpdatedReview()
        {
            // Arrange
            var existingReview = new CustomerReview();
            _mockCustomerReviewRepo.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(existingReview);
            var updatedReview = new CustomerReview();
            _mockCustomerReviewRepo.Setup(repo => repo.Update(existingReview))
                .ReturnsAsync(updatedReview);
            var reviewUpdateDTO = new CustomerReviewUpdateDTO();

            // Act
            var result = await _customerUserService.UpdateCustomerReviewText(reviewUpdateDTO);

            // Assert
            Assert.AreEqual(updatedReview, result);
        }

        [Test]
        public async Task DeleteCustomerReview_ValidReview_ReturnsDeletedReview()
        {
            // Arrange
            var existingReview = new CustomerReview();
            _mockCustomerReviewRepo.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(existingReview);

            // Assuming the existing review is soft-deleted and its IsDeleted property is set to true
            existingReview.IsDeleted = true;

            var updatedReview = new CustomerReview { IsDeleted = true }; // Assuming IsDeleted is set to true after deletion
            _mockCustomerReviewRepo.Setup(repo => repo.Update(existingReview))
                .ReturnsAsync(updatedReview);

            // Act
            var result = await _customerUserService.DeleteCustomerReview(1);

            // Assert
            Assert.IsTrue(result.IsDeleted); // Check if the IsDeleted property is set to true
            Assert.AreEqual(existingReview, result); // Ensure the same instance is returned
        }


    }
}



    