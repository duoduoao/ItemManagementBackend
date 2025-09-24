using Xunit;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using ItemManagement.Domain.Entities;
namespace ItemManagement.Domain.Tests.Entity
{
    public class ItemTests
    {
        [Fact]
        public void Item_CanBeCreatedWithAllProperties_AssignsCorrectValues()
        {
            // Arrange
            var category = new Category { CategoryId = 1, Name = "Electronics" };

            // Act
            var item = new Item
            {
                ItemId = 100,
                CategoryId = 1,
                Name = "Smartphone",
                Quantity = 50,
                Price = 299.99,
                Category = category,
                ImageUrl = "http://example.com/image.jpg"
            };

            // Assert
            Assert.Equal(100, item.ItemId);
            Assert.Equal(1, item.CategoryId);
            Assert.Equal("Smartphone", item.Name);
            Assert.Equal(50, item.Quantity);
            Assert.Equal(299.99, item.Price);
            Assert.Equal(category, item.Category);
            Assert.Equal("http://example.com/image.jpg", item.ImageUrl);
        }

        [Fact]
        public void Item_NameCanBeNull_AllowsNullValue()
        {
            // Arrange & Act
            var item = new Item { Name = null };

            // Assert
            Assert.Null(item.Name);
        }

        [Fact]
        public void Item_CategoryProperty_IsNotNullByDefault()
        {
            // Arrange & Act
            var item = new Item();

            // Assert
            Assert.NotNull(item.Category);
        }

        [Fact]
        public void Item_QuantityCanBeNull_AllowsNullValue()
        {
            // Arrange & Act
            var item = new Item { Quantity = null };

            // Assert
            Assert.Null(item.Quantity);
        }

        [Fact]
        public void Item_PriceCanBeNull_AllowsNullValue()
        {
            // Arrange & Act
            var item = new Item { Price = null };

            // Assert
            Assert.Null(item.Price);
        }
    }
}