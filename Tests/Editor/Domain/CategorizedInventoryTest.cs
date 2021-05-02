using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Kalendra.Inventory.Tests.Editor.Domain
{
    public class CategorizedInventoryTest
    {
        #region GetItems
        [Test]
        public void GetItems_OnEmptyBoard_ReturnsEmpty()
        {
            var sut = new GeneralistInventory();

            var result = sut.GetItems(null);

            result.Should().BeEmpty();
        }

        [Test]
        public void GetItems_WhenInventoryHasOnlyOneCategory_ReturnsAllItems()
        {
            //Arrange
            var mockCategory = Substitute.For<IInventoryItemCategory>();
            
            var mockItem = Substitute.For<IInventoryItem>();
            mockItem.Category.Returns(mockCategory);
            
            var sut = new GeneralistInventory(new[] {mockItem});
            
            //Act
            var result = sut.GetItems(mockCategory);

            //Assert
            result.Should().BeEquivalentTo(sut.Items);
        }

        [Test]
        public void GetItems_WhenInventoryHasManyCategories_DoesNotReturnAllItems()
        {
            //Arrange
            var mockCategory = Substitute.For<IInventoryItemCategory>();
            
            var mockItemCategory1 = Substitute.For<IInventoryItem>();
            mockItemCategory1.Category.Returns(mockCategory);
            
            var mockItemCategory2 = Substitute.For<IInventoryItem>();
            mockItemCategory2.Category.Returns(Substitute.For<IInventoryItemCategory>());
            
            var sut = new GeneralistInventory(new[] {mockItemCategory1, mockItemCategory2});
            
            //Act
            var result = sut.GetItems(mockCategory);

            //Assert
            result.Should().NotBeEquivalentTo(sut.Items);
        }
        #endregion
        
        #region HasCategory
        [Test]
        public void HasCategory_OnEmptyBoard_IsFalse()
        {
            var sut = new GeneralistInventory();

            var result = sut.HasCategory(null);

            result.Should().BeFalse();
        }

        [Test]
        public void HasCategory_OnBoardWithoutItemWithThatCategory_IsFalse()
        {
            //Arrange
            var mockCategory = Substitute.For<IInventoryItemCategory>();
            
            var mockItem = Substitute.For<IInventoryItem>();
            mockItem.Category.Returns(mockCategory);
            
            var sut = new GeneralistInventory(new[] {mockItem});
            
            //Act
            var result = sut.HasCategory(null);

            //Assert
            result.Should().BeFalse();
        }
        
        [Test]
        public void HasCategory_WithNull_OnBoardWithItemWithNullCategory_IsTrue()
        {
            //Arrange
            var mockItem = Substitute.For<IInventoryItem>();
            mockItem.Category.Returns((IInventoryItemCategory) null);
            
            var sut = new GeneralistInventory(new[] {mockItem});
            
            //Act
            var result = sut.HasCategory(null);

            //Assert
            result.Should().BeTrue();
        }
        
        [Test]
        public void HasCategory_OnBoardWithItemWithThatCategory_IsTrue()
        {
            //Arrange
            var mockCategory = Substitute.For<IInventoryItemCategory>();
            
            var mockItem = Substitute.For<IInventoryItem>();
            mockItem.Category.Returns(mockCategory);
            
            var sut = new GeneralistInventory(new[] {mockItem});
            
            //Act
            var result = sut.HasCategory(mockCategory);

            //Assert
            result.Should().BeTrue();
        }
        #endregion
    }
}