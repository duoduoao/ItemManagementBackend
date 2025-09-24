using AutoMapper;
using ItemManagement.Application.Common.DTO;
using ItemManagement.Application.UseCaseInterfaces;
using ItemManagement.Application.UseCasesInterfaces;
using ItemManagement.Application.UserCases;
using ItemManagement.Domain.Entities;
using ItemManagement.Domain.Repositories;
using ItemManagement.Infrastructure.Data;
using ItemManagement.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ItemManagement.Application.Tests.UseCases
{
    public class TransactionRepositoryTests
    { 
    private ApplicationDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unique DB per test
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public void Get_ReturnsAllTransactions()
        {
            using var context = CreateInMemoryDbContext();
            context.Transactions.Add(new Transaction { TransactionId = 1, CashierName = "John" });
            context.Transactions.Add(new Transaction { TransactionId = 2, CashierName = "Jane" });
            context.SaveChanges();

            var repo = new TransactionRepository(context);
            var result = repo.Get("anyname").ToList();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetByDay_FiltersByCashierNameAndDate()
        {
            using var context = CreateInMemoryDbContext();
            var date = DateTime.Today;
            context.Transactions.Add(new Transaction { TransactionId = 1, CashierName = "John", TimeStamp = date });
            context.Transactions.Add(new Transaction { TransactionId = 2, CashierName = "Jane", TimeStamp = date });
            context.Transactions.Add(new Transaction { TransactionId = 3, CashierName = "John", TimeStamp = date.AddDays(-1) });
            context.SaveChanges();

            var repo = new TransactionRepository(context);
            var results = repo.GetByDay("John", date).ToList();

            Assert.Single(results);
            Assert.All(results, t => Assert.Equal("John", t.CashierName));
            Assert.All(results, t => Assert.Equal(date.Date, t.TimeStamp.Date));
        }

        [Fact]
        public void Save_AddsTransaction()
        {
            using var context = CreateInMemoryDbContext();
            var repo = new TransactionRepository(context);

            repo.Save("John", 1, "Item1", 10.0, 5, 2);

            var saved = context.Transactions.FirstOrDefault();
            Assert.NotNull(saved);
            Assert.Equal("John", saved.CashierName);
            Assert.Equal(1, saved.ItemId);
            Assert.Equal("Item1", saved.ItemName);
            Assert.Equal(10.0, saved.Price);
            Assert.Equal(5, saved.BeforeQty);
            Assert.Equal(2, saved.SoldQty);
        }

        [Fact]
        public void Search_FiltersByCashierNameAndDateRange()
        {
            using var context = CreateInMemoryDbContext();
            var startDate = DateTime.Today.AddDays(-2);
            var endDate = DateTime.Today;
            context.Transactions.Add(new Transaction { TransactionId = 1, CashierName = "John", TimeStamp = DateTime.Today.AddDays(-1) });
            context.Transactions.Add(new Transaction { TransactionId = 2, CashierName = "Jane", TimeStamp = DateTime.Today.AddDays(-1) });
            context.Transactions.Add(new Transaction { TransactionId = 3, CashierName = "John", TimeStamp = DateTime.Today.AddDays(-3) });
            context.SaveChanges();

            var repo = new TransactionRepository(context);
            var results = repo.Search("John", startDate, endDate).ToList();

            Assert.Single(results);
            Assert.All(results, t => Assert.Equal("John", t.CashierName));

            Assert.All(results, t =>
            {
                Assert.True(t.TimeStamp >= startDate.Date && t.TimeStamp <= endDate.Date.AddDays(1).Date);
            });
        }

        [Fact]
        public void GetAll_ReturnsAllTransactions()
        {
            using var context = CreateInMemoryDbContext();
            context.Transactions.Add(new Transaction { TransactionId = 1 });
            context.Transactions.Add(new Transaction { TransactionId = 2 });
            context.SaveChanges();

            var repo = new TransactionRepository(context);
            var result = repo.GetAll().ToList();

            Assert.Equal(2, result.Count);
        }
    }
}
