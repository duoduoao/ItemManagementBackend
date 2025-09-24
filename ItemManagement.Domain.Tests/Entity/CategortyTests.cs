using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;
using ItemManagement.Domain.Entities;
namespace ItemManagement.Domain.Tests.Entity
{
    public class CategoryTests
    {
        [Fact]
        public void Category_CanBeCreatedWithProperties_AssignsCorrectValues()
        {
            // Arrange & Act
            var category = new Category
            {
                CategoryId = 1,
                Name = "Electronics",
                Description = "Electronic devices and gadgets"
            };

            // Assert
            Assert.Equal(1, category.CategoryId);
            Assert.Equal("Electronics", category.Name);
            Assert.Equal("Electronic devices and gadgets", category.Description);
        }

        [Fact]
        public void Category_Items_IsInitializedToEmptyList()
        {
            // Arrange & Act
            var category = new Category();

            // Assert
            Assert.NotNull(category.Items);
            Assert.IsType<List<Item>>(category.Items);
            Assert.Empty(category.Items);
        }

        [Fact]
        public void Category_CanAddItem_ToItemsList()
        {
            // Arrange
            var category = new Category();
            var item = new Item { ItemId = 1, Name = "TestItem" };

            // Act
            category.Items.Add(item);

            // Assert
            Assert.Single(category.Items);
            Assert.Contains(item, category.Items);
        }
    }
}