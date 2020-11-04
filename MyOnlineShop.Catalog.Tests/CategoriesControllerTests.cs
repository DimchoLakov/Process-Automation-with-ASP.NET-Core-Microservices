using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Catalog.Controllers;
using MyOnlineShop.Catalog.Data.Models.Categories;
using System.Threading.Tasks;
using Xunit;

namespace MyOnlineShop.Catalog.Tests
{
    public class CategoriesControllerTests : BaseTesting
    {
        private readonly CategoriesController categoriesController;

        public CategoriesControllerTests()
        {
            this.categoriesController = new CategoriesController(this.CatalogDbContext);
        }

        [Fact]
        public async Task StatusChangeShouldChangeStatusOfACategory()
        {
            // Arrange
            int categoryId = 1;

            await this.CatalogDbContext
                .Categories
                .AddAsync(new Category
                {
                    Id = categoryId,
                    IsActive = true,
                    Name = "Test"
                });

            await this.CatalogDbContext
                .SaveChangesAsync();

            // Act
            var actionResult = await this.categoriesController.StatusChange(categoryId);

            // Assert
            var category = await this.CatalogDbContext
                .Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId);
            
            bool expectedStatus = false;
            bool actualStatus = category.IsActive;

            Assert.Equal(expectedStatus, actualStatus);
        }
    }
}
