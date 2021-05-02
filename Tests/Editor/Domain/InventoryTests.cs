using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Kalendra.Inventory.Tests.Editor.Domain
{
    public class InventoryTests
    {
        #region Constructors
        [Test]
        public void NewInventory_IsEmpty_ByDefault()
        {
            var sut = new GeneralistInventory();

            var result = sut.Items;

            result.Should().BeEmpty();
        }

        [Test]
        public void NewInventory_ReceivingItems_ThenIsNotEmpty()
        {
            var items = new[] {Substitute.For<IInventoryItem>()};
            var sut = new GeneralistInventory(items);

            var result = sut.Items;

            result.Should().NotBeEmpty();
        }

        [Test]
        public void NewInventory_ReceivingItem_ThenHasThatItems()
        {
            var items = new[] {Substitute.For<IInventoryItem>()};
            var sut = new GeneralistInventory(items);

            var result = sut.HasItem(items[0]);

            result.Should().BeTrue();
        }

        [Test]
        public void NewInventory_ReceivingPiles_ThenIsNotEmpty()
        {
            var piles = new[] {new ItemPile(Substitute.For<IInventoryItem>(), 10)};
            var sut = new GeneralistInventory(piles);

            var result = sut.Items;

            result.Should().NotBeEmpty();
        }

        [Test]
        public void NewInventory_ReceivingPiles_ThenHasSamePile()
        {
            var piles = new[] {new ItemPile(Substitute.For<IInventoryItem>(), 10)};
            var sut = new GeneralistInventory(piles);

            var result = sut.Items;

            result.Should().Contain(piles[0]);
        }
        #endregion

        #region AddItem
        [Test]
        public void AddItem_OnEmptyInventory_ThenIsNotEmptyAnymore()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            var sut = new GeneralistInventory();

            sut.AddItem(mockItem);

            sut.Items.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void AddItem_OnEmptyInventory_ThenHasThatItem()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            var sut = new GeneralistInventory();

            sut.AddItem(mockItem);

            sut.HasItem(mockItem).Should().BeTrue();
        }

        [Test]
        public void AddItemNCount_OnEmptyInventory__ThenGetItemCount_IsN()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            var sut = new GeneralistInventory();

            sut.AddItem(mockItem, 20);

            sut.GetItemCount(mockItem).Should().Be(20);
        }

        [Test]
        public void AddItem_NTimes_ThenGetItemCount_IsNTotalCount()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            var sut = new GeneralistInventory();

            sut.AddItem(mockItem);
            sut.AddItem(mockItem, 19);

            sut.GetItemCount(mockItem).Should().Be(20);
        }

        [Test]
        public void AddItem_WhenCountIsZero_OnEmptyInventory_ThenIsStillEmpty()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            var sut = new GeneralistInventory();

            sut.AddItem(mockItem, 0);

            sut.Items.Should().BeEmpty();
        }

        [Test]
        public void AddItem_WithNegativeCount_ThrowsException()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            var sut = new GeneralistInventory();

            Action act = () => sut.AddItem(mockItem, -1);

            act.Should().ThrowExactly<InvalidOperationException>();
        }
        #endregion

        #region RemoveItem
        [Test]
        public void RemoveItem_WithNegativeCount_ThrowsException()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            var sut = new GeneralistInventory();

            Action act = () => sut.RemoveItem(mockItem, -1);

            act.Should().ThrowExactly<InvalidOperationException>();
        }

        [Test]
        public void RemoveItem_WhenCountIsZero_DoesNothing()
        {
            var mockItems = new[] {Substitute.For<IInventoryItem>()};
            var sut = new GeneralistInventory(mockItems);

            sut.RemoveItem(mockItems[0], 0);

            sut.HasItem(mockItems[0]).Should().BeTrue();
        }
        
        [Test]
        public void RemoveItem_WithTotalCount_RemovesTheItem()
        {
            var mockItems = new[] {new ItemPile(Substitute.For<IInventoryItem>(), 10)};
            var sut = new GeneralistInventory(mockItems);

            sut.RemoveItem(mockItems[0].item, 10);

            sut.HasItem(mockItems[0].item).Should().BeFalse();
        }
        #endregion
    }
}