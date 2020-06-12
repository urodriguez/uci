using Domain.Aggregates;
using Domain.Exceptions;
using NUnit.Framework;

namespace Domain.UnitTests.Aggregates
{
    [TestFixture]
    public class InventionTests
    {

        [Test]
        public void SetName_UsingValidName_SetsNameToInvention()
        {
            //Arrange
            var invention = new Invention();

            //Act
            invention.SetName("test");

            //Assert
            Assert.IsTrue(invention.Name == "testFail");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("test012345678901234567890123456789")]
        public void SetName_UsingInvalidName_ThrowsBusinessRuleException(string name)
        {
            //Arrange
            var invention = new Invention();

            //Act
            TestDelegate testDelegate = () => invention.SetName(name);

            //Assert
            Assert.Throws<BusinessRuleException>(testDelegate);
        }        
        
        [Test]
        public void SetCode_UsingValidCode_SetsCodeToInvention()
        {
            //Arrange
            var invention = new Invention();

            //Act
            invention.SetCode("test0000");

            //Assert
            Assert.IsTrue(invention.Code == "test0000");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("test")]
        [TestCase("test0123456789")]
        public void SetCode_UsingInvalidCode_ThrowsBusinessRuleException(string code)
        {
            //Arrange
            var invention = new Invention();

            //Act
            TestDelegate testDelegate = () => invention.SetCode(code);

            //Assert
            Assert.Throws<BusinessRuleException>(testDelegate);
        }        
        
        [Test]
        public void SetPrice_UsingValidPrice_SetsPriceToInvention()
        {
            //Arrange
            var invention = new Invention();

            //Act
            invention.SetPrice(100);

            //Assert
            Assert.IsTrue(invention.Price == 100);
        }

        [TestCase(0)]
        [TestCase(-100)]
        public void SetPrice_UsingInvalidPrice_ThrowsBusinessRuleException(int price)
        {
            //Arrange
            var invention = new Invention();

            //Act
            TestDelegate testDelegate = () => invention.SetPrice(price);

            //Assert
            Assert.Throws<BusinessRuleException>(testDelegate);
        }
    }
}