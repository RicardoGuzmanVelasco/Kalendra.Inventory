using System;
using FluentAssertions;
using Kalendra.Inventory.Runtime.Domain;
using NSubstitute;
using NUnit.Framework;

namespace Kalendra.Inventory.Tests.Editor.Domain
{
    public class LimitedWeightedInventoryDecoratorTests
    {
        [Test]
        public void AddItem_WhenInventoryCantBear_DoesNotAddThatItem()
        {
            //Arrange
            var mockItem = Substitute.For<IInventoryItem>();
            mockItem.Weight.Returns(2);
            
            var docDecorated = new GeneralistInventory(1);
            var sut = new LimitedWeightedInventoryDecorator(docDecorated);
            
            //Act
            sut.AddItem(mockItem);
            
            //Assert
            sut.Items.Should().BeEmpty();
        }
    }
}