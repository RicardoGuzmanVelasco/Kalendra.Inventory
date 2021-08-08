using System;
using FluentAssertions;
using JetBrains.Annotations;
using Kalendra.Commons.Tests.TestDataBuilders.StaticShortcuts;
using Kalendra.Inventory.Runtime.Domain;
using NSubstitute;
using NUnit.Framework;

namespace Kalendra.Inventory.Tests.Editor.Domain
{
    public class WeightedInventoryTests
    {
        #region Constructors
        [Test, TestCase(-1), TestCase(0)]
        public void NewWeightedInventory_WithNonPositiveMaxWeight_ThrowsException(int maxWeight)
        {
            GeneralistInventory sut;
            
            Action act = () => sut = new GeneralistInventory(maxWeight);

            act.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }
        #endregion
        
        #region CurrentWeight
        [Test]
        public void CurrentWeight_IsZero_ByDefault()
        {
            var sut = new GeneralistInventory();

            var result = sut.CurrentWeight;

            result.Should().Be(0);
        }

        [Test]
        public void CurrentWeight_IsZero_AddingItemsWithoutWeight()
        {
            //Arrange
            var mockItem = Substitute.For<IInventoryItem>();
            mockItem.Weight.Returns(0);
            
            var sut = new GeneralistInventory(20);
            
            //Act
            sut.AddItem(mockItem);

            //Assert
            sut.CurrentWeight.Should().Be(0);
        }

        [Test]
        public void CurrentWeight_IsNotZero_AfterAddOneItemWithWeight()
        {
            //Arrange
            var mockItem = Substitute.For<IInventoryItem>();
            mockItem.Weight.Returns(10);
            
            var sut = new GeneralistInventory(20);
            
            //Act
            sut.AddItem(mockItem);

            //Assert
            sut.CurrentWeight.Should().Be(10);
        }
        
        [Test]
        public void CurrentWeight_IsSum_AfterAddSomeItems()
        {
            //Arrange
            var mockItem = Substitute.For<IInventoryItem>();
            mockItem.Weight.Returns(10);
            
            var sut = new GeneralistInventory(20);
            
            //Act
            sut.AddItem(mockItem);
            sut.AddItem(mockItem);

            //Assert
            sut.CurrentWeight.Should().Be(20);
        }
        #endregion

        #region CanBearItem
        [Test]
        [TestCase(4, 5, true)]
        [TestCase(5, 5, true)]
        [TestCase(6, 5, false)]
        public void CanBearItem_OnEmptyInventory_DependsOnItemAndMaxWeight(int itemWeight, int maxWeight, bool expected)
        {
            //Arrange
            var mockItem = Substitute.For<IInventoryItem>();
            mockItem.Weight.Returns(itemWeight);
            
            var sut = new GeneralistInventory(maxWeight);
            
            //Act
            var result = sut.CanBearItem(mockItem);

            //Assert
            result.Should().Be(expected);
        }
        
        [Test]
        public void CanBearItem_DependsOnCurrentWeight()
        {
            //Arrange
            var mockItem = Substitute.For<IInventoryItem>();
            mockItem.Weight.Returns(15);
            
            var sut = new GeneralistInventory(35);
            sut.AddItem(mockItem);
            sut.AddItem(mockItem);
            
            //Act
            var result = sut.CanBearItem(mockItem);

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void CanBearItem_ConsidersItemCount()
        {
            //Arrange
            var mockItem = Substitute.For<IInventoryItem>();
            mockItem.Weight.Returns(15);
            
            var sut = new GeneralistInventory(35);
            sut.AddItem(mockItem);
            
            //Act
            var result = sut.CanBearItem(mockItem, 2);

            //Assert
            result.Should().BeFalse();
        }
        #endregion

        #region Overweight
        [Test]
        public void AddItem_WhenInventoryCantBear_RaisesOnOverweight()
        {
            //Arrange
            var mockItem = Substitute.For<IInventoryItem>();
            mockItem.Weight.Returns(2);

            var mockListener = Fake.MockListener();
            var sut = new GeneralistInventory(1);
            sut.OnOverweight += mockListener.Call;
            
            //Act
            sut.AddItem(mockItem);

            //Assert
            mockListener.Received(1).Call();
        }
        
        [Test]
        public void AddItem_WhenInventoryHasOverweight_DoesNotRaiseOnOverweight()
        {
            //Arrange
            var mockItem = Substitute.For<IInventoryItem>();
            mockItem.Weight.Returns(2);

            var mockListener = Fake.MockListener();
            var sut = new GeneralistInventory(1);
            sut.OnOverweight += mockListener.Call;
            sut.AddItem(mockItem);
            
            //Act
            mockListener.ClearReceivedCalls();
            sut.AddItem(mockItem);

            //Assert
            mockListener.DidNotReceive().Call();
        }
        
        [Test]
        public void AddItem_AfterDecreaseOverweight_RaisesOnOverweightAgain()
        {
            //Arrange
            var mockItem = Substitute.For<IInventoryItem>();
            mockItem.Weight.Returns(2);

            var mockListener = Fake.MockListener();
            var sut = new GeneralistInventory(1);
            sut.OnOverweight += mockListener.Call;
            
            //Act
            sut.AddItem(mockItem);
            sut.RemoveItem(mockItem);
            sut.AddItem(mockItem);

            //Assert
            mockListener.Received(2).Call();
        }
        #endregion

        #region IncreaseMaxWeight
        [Test]
        public void DecreasingMaxWeight_TilNonPositiveWeight_ThrowsException()
        {
            var sut = new GeneralistInventory(1);
            
            Action act = () => sut.IncreaseMaxWeight(-1);

            act.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }
        #endregion
    }
}