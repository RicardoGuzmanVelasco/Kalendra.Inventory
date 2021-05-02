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
            IInventory sut = new GeneralistInventory();

            var result = sut.Items;

            result.Should().BeEmpty();
        }

        [Test]
        public void NewInventory_ReceivingItems_ThenIsNotEmpty()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            IInventory sut = new GeneralistInventory(new[] {mockItem});

            var result = sut.Items;

            result.Should().NotBeEmpty();
        }

        [Test]
        public void NewInventory_ReceivingItem_ThenHasThatItems()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            IInventory sut = new GeneralistInventory(new[] {mockItem});

            var result = sut.HasItem(mockItem);

            result.Should().BeTrue();
        }

        [Test]
        public void NewInventory_ReceivingPiles_ThenIsNotEmpty()
        {
            var mockItemPile = new ItemPile(Substitute.For<IInventoryItem>(), 10);
            IInventory sut = new GeneralistInventory(new[] {mockItemPile});

            var result = sut.Items;

            result.Should().NotBeEmpty();
        }

        [Test]
        public void NewInventory_ReceivingPiles_ThenHasSameItems()
        {
            var mockItemPile = new ItemPile(Substitute.For<IInventoryItem>(), 10);
            IInventory sut = new GeneralistInventory(new[]{mockItemPile});

            var result = sut.Items;

            result.Should().Contain(mockItemPile);
        }

        [Test]
        public void NewInventory_ReceivingPile_HasPileCount()
        {
            var mockItemPile = new ItemPile(Substitute.For<IInventoryItem>(), 10);
            IInventory sut = new GeneralistInventory(new[]{mockItemPile});

            var result = sut.HasItem(mockItemPile.item, 10);

            result.Should().BeTrue();
        }
        #endregion

        #region AddItem
        [Test]
        public void AddItem_OnEmptyInventory_ThenIsNotEmptyAnymore()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            IInventory sut = new GeneralistInventory();

            sut.AddItem(mockItem);

            sut.Items.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void AddItem_OnEmptyInventory_ThenHasThatItem()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            IInventory sut = new GeneralistInventory();

            sut.AddItem(mockItem);

            sut.HasItem(mockItem).Should().BeTrue();
        }

        [Test]
        public void AddItemNCount_OnEmptyInventory__ThenGetItemCount_IsN()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            IInventory sut = new GeneralistInventory();

            sut.AddItem(mockItem, 20);

            sut.HasItem(mockItem, 20).Should().BeTrue();
        }

        [Test]
        public void AddItem_NTimes_ThenGetItemCount_IsNTotalCount()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            IInventory sut = new GeneralistInventory();

            sut.AddItem(mockItem);
            sut.AddItem(mockItem, 19);

            sut.HasItem(mockItem, 20).Should().BeTrue();
        }

        [Test]
        public void AddItem_WhenCountIsZero_OnEmptyInventory_ThenIsStillEmpty()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            IInventory sut = new GeneralistInventory();

            sut.AddItem(mockItem, 0);

            sut.Items.Should().BeEmpty();
        }

        [Test]
        public void AddItem_WithNegativeCount_ThrowsException()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            IInventory sut = new GeneralistInventory();

            Action act = () => sut.AddItem(mockItem, -1);

            act.Should().ThrowExactly<InvalidOperationException>();
        }
        #endregion
        
        #region HasItem
        [Test]
        public void HasItem_OnEmptyBoard_IsFalse()
        {
            IInventory sut = new GeneralistInventory();

            var result = sut.HasItem(Substitute.For<IInventoryItem>());

            result.Should().BeFalse();
        }

        [Test]
        public void HasItem_WhenItemWasAdded_IsTrue()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            IInventory sut = new GeneralistInventory(new[] {mockItem});

            var result = sut.HasItem(mockItem);

            result.Should().BeTrue();
        }

        [Test]
        public void HasItem_WithMinCount_WhenItemWasAdded_WithLess_IsFalse()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            IInventory sut = new GeneralistInventory(new[] {mockItem});

            var result = sut.HasItem(mockItem, 2);

            result.Should().BeFalse();
        }
        
        [Test]
        public void HasItem_WithMinCount_WhenItemWasAdded_WithGreater_IsTrue()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            IInventory sut = new GeneralistInventory();
            sut.AddItem(mockItem, 11);

            var result = sut.HasItem(mockItem, 10);

            result.Should().BeTrue();
        }

        [Test]
        public void HasItem_WithMinCount_WhenItemWasAdded_WithEquals_IsTrue()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            IInventory sut = new GeneralistInventory();
            sut.AddItem(mockItem, 10);

            var result = sut.HasItem(mockItem, 10);

            result.Should().BeTrue();
        }
        #endregion

        #region RemoveItem
        [Test]
        public void RemoveItem_WithNegativeCount_ThrowsException()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            IInventory sut = new GeneralistInventory();

            Action act = () => sut.RemoveItem(mockItem, -1);

            act.Should().ThrowExactly<InvalidOperationException>();
        }

        [Test]
        public void RemoveItem_WhenCountIsZero_DoesNothing()
        {
            var mockItem = Substitute.For<IInventoryItem>();
            IInventory sut = new GeneralistInventory(new[] {mockItem});

            sut.RemoveItem(mockItem, 0);

            sut.HasItem(mockItem).Should().BeTrue();
        }

        [Test]
        public void RemoveItem_WithTotalCount_RemovesTheItem()
        {
            var mockItemPile = new ItemPile(Substitute.For<IInventoryItem>(), 10);
            IInventory sut = new GeneralistInventory(new[] {mockItemPile});

            sut.RemoveItem(mockItemPile.item, 10);

            sut.HasItem(mockItemPile.item).Should().BeFalse();
        }

        [Test]
        public void RemoveItem_WithCountGreaterThanTotalCount_ThrowsException()
        {
            var mockItemPile = new ItemPile(Substitute.For<IInventoryItem>(), 10);
            IInventory sut = new GeneralistInventory(new[] {mockItemPile});

            Action act = () => sut.RemoveItem(mockItemPile.item, 11);

            act.Should().ThrowExactly<InvalidOperationException>();
        }

        [Test]
        public void RemoveItem_WithCountGreater_WhenItemIsSplitInSomePiles_RemovesUntilCount()
        {
            //Arrange
            var mockItem = Substitute.For<IInventoryItem>();
            IInventory sut = new GeneralistInventory();
            
            sut.AddItem(mockItem, 2);
            sut.AddItem(mockItem, 3);

            //Act
            sut.RemoveItem(mockItem, 3);

            //Assert
            sut.HasItem(mockItem, 3).Should().BeFalse();
        }
        #endregion
    }
}