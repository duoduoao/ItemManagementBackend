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
        public void Item_PropertyAssignment_WorksCorrectly()
        {
            var item = new Item
            {
                ItemId = 1,
                CategoryId = 2,
                Name = "Test Item",
                Quantity = 5,
                Price = 10.99,
                ImageUrl = "image-url"
            };

            Assert.Equal(1, item.ItemId);
            Assert.Equal(2, item.CategoryId);
            Assert.Equal("Test Item", item.Name);
            Assert.Equal(5, item.Quantity);
            Assert.Equal(10.99, item.Price);
            Assert.Equal("image-url", item.ImageUrl);
            Assert.Null(item.Category); // No Category assigned

        }
    }
}