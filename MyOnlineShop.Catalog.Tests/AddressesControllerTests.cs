using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MyOnlineShop.Catalog.Controllers;
using MyOnlineShop.Catalog.Data.Models.Customers;
using MyOnlineShop.Catalog.Profiles;
using MyOnlineShop.Common.ViewModels.Addresses;
using MyOnlineShop.Ordering.Data;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MyOnlineShop.Catalog.Tests
{
    public class AddressesControllerTests : BaseTesting
    {
        private readonly AddressesController addressesController;

        public AddressesControllerTests()
        {
            this.addressesController = new AddressesController(this.CatalogDbContext, this.Mapper);
        }

        [Fact]
        public async Task GetAddressShouldReturnCorrectAddressWhenValidUserIdIsPassed()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();
            string country = "UK";
            int customerId = 1;

            await this.CatalogDbContext
                .Customers
                .AddAsync(new Customer
                {
                    UserId = userId,
                    Id = customerId
                });

            await this.CatalogDbContext
                .Addresses
                .AddAsync(new Address
                {
                    CustomerId = customerId,
                    IsDeliveryAddress = true,
                    Country = country
                });

            await this.CatalogDbContext
                .SaveChangesAsync();

            // Act

            var actionResult = await this.addressesController.GetAddress(userId);

            // Assert
            string expectedCountry = country;
            int expectedCustomerId = customerId;

            var okObjectResult = actionResult.Result as OkObjectResult;
            var addressViewModelResult = okObjectResult.Value as AddressViewModel;

            string actualCountry = addressViewModelResult.Country;
            int actualCustomerId = addressViewModelResult.CustomerId;

            Assert.Equal(expectedCountry, actualCountry);
            Assert.Equal(expectedCustomerId, actualCustomerId);
        }

        [Fact]
        public async Task GetAddressShouldReturnEmptyViewModelWhenInvalidUserIdIsPassed()
        {
            // Arrange
            await this.CatalogDbContext
                .Customers
                .AddAsync(new Customer
                {
                    Id = 1,
                    UserId = "Test user id"
                });

            await this.CatalogDbContext
                .Addresses
                .AddAsync(new Address
                {
                    CustomerId = 1,
                    IsDeliveryAddress = true
                });

            // Act
            var actionResult = await this.addressesController.GetAddress("Invalid User");

            // Assert
            string expectedCountry = default;
            int expectedAddressId = default;

            var okObjectResult = actionResult.Result as OkObjectResult;
            var addressViewModel = okObjectResult.Value as AddressViewModel;

            string actualCountry = addressViewModel.Country;
            int actualAddressId = addressViewModel.Id;

            Assert.Equal(expectedCountry, actualCountry);
            Assert.Equal(expectedAddressId, actualAddressId);
        }
    }
}
