using System;
using Xunit;
using eShop.Catalog.API.Model;
using Pgvector;
using eShop.Catalog.API.Infrastructure.Exceptions;

namespace Catalog.UnitTests
{
    public class CatalogItemTests
    {
        [Fact]
        public void RemoveStock_RemovesCorrectAmount_WhenEnoughStock()
        {
            var item = new CatalogItem { Name = "Test", AvailableStock = 10 };
            int removed = item.RemoveStock(5);
            Assert.Equal(5, removed);
            Assert.Equal(5, item.AvailableStock);
        }

        [Fact]
        public void RemoveStock_RemovesAll_WhenNotEnoughStock()
        {
            var item = new CatalogItem { Name = "Test", AvailableStock = 3 };
            int removed = item.RemoveStock(5);
            Assert.Equal(3, removed);
            Assert.Equal(0, item.AvailableStock);
        }

        [Fact]
        public void RemoveStock_Throws_WhenStockIsZero()
        {
            var item = new CatalogItem { Name = "Test", AvailableStock = 0 };
            Assert.Throws<CatalogDomainException>(() => item.RemoveStock(1));
        }

        [Fact]
        public void RemoveStock_Throws_WhenQuantityIsNegative()
        {
            var item = new CatalogItem { Name = "Test", AvailableStock = 5 };
            Assert.Throws<CatalogDomainException>(() => item.RemoveStock(-1));
        }

        [Fact]
        public void AddStock_AddsCorrectly_WhenBelowMax()
        {
            var item = new CatalogItem { Name = "Test", AvailableStock = 5, MaxStockThreshold = 10 };
            int added = item.AddStock(3);
            Assert.Equal(3, added);
            Assert.Equal(8, item.AvailableStock);
            Assert.False(item.OnReorder);
        }

        [Fact]
        public void AddStock_CapsAtMax_WhenExceedingMax()
        {
            var item = new CatalogItem { Name = "Test", AvailableStock = 8, MaxStockThreshold = 10 };
            int added = item.AddStock(5);
            Assert.Equal(2, added);
            Assert.Equal(10, item.AvailableStock);
        }
    }
}
