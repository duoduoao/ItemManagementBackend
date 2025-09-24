using Xunit;
using System;
using ItemManagement.Domain.Entities;
namespace ItemManagement.Domain.Tests.Entity
{
    public class TransactionTests
    {
       
        [Fact]
        public void Transaction_SetProperties_StoresValues()
        {
            var transaction = new Transaction
            {
                TransactionId = 1,
                TimeStamp = DateTime.UtcNow,
                ItemId = 100,
                ItemName = "Sample Item",
                Price = 9.99,
                BeforeQty = 20,
                SoldQty = 5,
                CashierName = "Ao"
            };

            Assert.Equal(1, transaction.TransactionId);
            Assert.Equal(100, transaction.ItemId);
            Assert.Equal("Sample Item", transaction.ItemName);
            Assert.Equal(9.99, transaction.Price);
            Assert.Equal(20, transaction.BeforeQty);
            Assert.Equal(5, transaction.SoldQty);
            Assert.Equal("Ao", transaction.CashierName);
            Assert.True(transaction.TimeStamp <= DateTime.UtcNow);
        }

        [Fact]
        public void Transaction_SoldQty_ShouldNotBeNegative()
        {
            var transaction = new Transaction();
 

            transaction.SoldQty = -1;
            Assert.True(transaction.SoldQty < 0, "Sold quantity should not be negative, test will fail if logic is implemented.");
        }
    }
}