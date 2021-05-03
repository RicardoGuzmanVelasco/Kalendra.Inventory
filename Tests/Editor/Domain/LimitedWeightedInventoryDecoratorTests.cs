using System;
using FluentAssertions;
using Kalendra.Commons.Tests.TestDataBuilders.StaticShortcuts;
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
        
        [Test]
        public void AddItem_WhenInventoryCantBear_DoesNotRaiseOnOverweight()
        {
            //Arrange
            var mockItem = Substitute.For<IInventoryItem>();
            mockItem.Weight.Returns(2);
            
            var mockListener = Fake.MockListener();
            var sut = new LimitedWeightedInventoryDecorator(new GeneralistInventory(1));
            sut.OnOverweight += mockListener.Call;
            
            //Act
            sut.AddItem(mockItem);
            
            //Assert
            mockListener.DidNotReceive().Call();
        }
        
        [Test]
        public void AddItem_WhenInventoryCantBear_ConsidersCount()
        {
            //Arrange
            var mockItem = Substitute.For<IInventoryItem>();
            mockItem.Weight.Returns(1);
            
            var docDecorated = new GeneralistInventory(1);
            var sut = new LimitedWeightedInventoryDecorator(docDecorated);
            
            //Act
            sut.AddItem(mockItem, 2);
            
            //Assert
            sut.Items.Should().BeEmpty();
        }
    }
}