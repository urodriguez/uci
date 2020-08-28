using System.Linq;
using Domain.Aggregates;
using Domain.Enums;
using Domain.Exceptions;
using Moq;
using NUnit.Framework;

namespace Domain.UnitTests.Aggregates
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void SetEmail_UsingValidEmail_SetsEmailToUser()
        {
            //Arrange
            var user = new User();

            //Act
            user.SetEmail("test@test.com");

            //Assert
            Assert.IsTrue(user.Email == "test@test.com");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("test")]
        [TestCase("@test.com")]
        [TestCase("test@test,com")]
        [TestCase("test@@test.com")]
        [TestCase("test.test.com")]
        public void SetEmail_UsingInvalidEmail_ThrowsBusinessRuleException(string email)
        {
            //Arrange
            var user = new User();

            //Act
            TestDelegate testDelegate = () => user.SetEmail(email);

            //Assert
            Assert.Throws<BusinessRuleException>(testDelegate);
        }

        [Test]
        public void SetFullName_UsingValidFullName_SetsFullNameToUser()
        {
            //Arrange
            var user = new User();

            //Act
            user.SetFirstName("firstTest");
            user.SetLastName("lastTest");

            //Assert
            Assert.IsTrue(user.FirstName == "firstTest");
            Assert.IsTrue(user.LastName == "lastTest");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("test012345678901234567890123456789;test012345678901234567890123456789")]
        public void SetFullName_UsingInvalidFullName_ThrowsBusinessRuleException(string fullName)
        {
            //Arrange
            var user = new User();
            var fn = !string.IsNullOrEmpty(fullName) ? fullName.Split('.').First() : fullName;
            var ln = !string.IsNullOrEmpty(fullName) ? fullName.Split('.').Last() : fullName;
            //Act
            TestDelegate testDelegateFn = () => user.SetFirstName(fn);
            TestDelegate testDelegateLn = () => user.SetLastName(ln);

            //Assert
            Assert.Throws<BusinessRuleException>(testDelegateFn);
            Assert.Throws<BusinessRuleException>(testDelegateLn);
        }

        [Test]
        public void SetRole_UsingValidRole_SetsRoleToUser()
        {
            //Arrange
            var user = new User();

            //Act
            user.SetRole(UserRole.Admin);

            //Assert
            Assert.IsTrue(user.Role == UserRole.Admin);
        }

        [TestCase(999999)]
        public void SetRole_UsingInvalidRole_ThrowsBusinessRuleException(UserRole userRole)
        {
            //Arrange
            var user = new User();

            //Act
            TestDelegate testDelegate = () => user.SetRole(userRole);

            //Assert
            Assert.Throws<BusinessRuleException>(testDelegate);
        }

        [Test]
        public void IsLocked_AccessFailedCountIs3_ReturnsTrue()
        {
            //Arrange
            var user = new User { AccessFailedCount = 3 };

            //Act
            var isLocked = user.IsLocked();

            //Assert
            Assert.IsTrue(isLocked);
        }

        [Test]
        public void HasPassword_PasswordProvidedMatchWithUserPassword_ReturnsTrue()
        {
            //Arrange
            var userMock = new Mock<User> { CallBase = true };
            userMock.SetupGet(u => u.Password).Returns("admin-9999");

            //Act
            var hasPassword = userMock.Object.HasPassword("admin-9999");

            //Assert
            Assert.IsTrue(hasPassword);
        }              
        
        [TestCase("admin-9998")]
        [TestCase("admin-8999")]
        [TestCase("Admin-9999")]
        [TestCase("adm1n-9999")]
        public void HasPassword_PasswordProvidedNotMatchWithUserPassword_ReturnsFalse(string password)
        {
            //Arrange
            var userMock = new Mock<User>();
            userMock.SetupGet(u => u.Password).Returns("admin-9999");

            //Act
            var passwordIsValid = userMock.Object.HasPassword(password);

            //Assert
            Assert.IsFalse(passwordIsValid);
        }        
        
        [Test]
        public void IsAdmin_UsingAdminRole_ReturnsTrue()
        {
            //Arrange
            var userMock = new Mock<User> { CallBase = true };
            userMock.SetupGet(u => u.Role).Returns(UserRole.Admin);

            //Act
            var isAdmin = userMock.Object.IsAdmin();

            //Assert
            Assert.IsTrue(isAdmin);
        }

        [Test]
        public void GenerateDefaultPassword_NewValidDefaultPasswordIsRequested_GeneratesDefaultPassword()
        {
            //Arrange
            var user = new User();

            //Act
            user.GenerateDefaultPassword();

            //Assert
            Assert.IsTrue(!string.IsNullOrEmpty(user.Password));
        }        
        
        [Test]
        public void SetPassword_UsingPasswordThatSatisfyComplexity_SetsPasswordOk()
        {
            //Arrange
            var user = new User();

            //Act
            user.SetPassword("Pa$$w0rd");

            //Assert
            Assert.IsTrue(user.Password == "Pa$$w0rd");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("admin")]
        [TestCase("9999")]
        [TestCase("test0123456789")]
        public void SetPassword_UsingPasswordThatNotSatisfyComplexity_ThrowsBusinessRuleException(string password)
        {
            //Arrange
            var user = new User();

            //Act
            TestDelegate testDelegate = () => user.SetPassword(password);

            //Assert
            Assert.Throws<BusinessRuleException>(testDelegate);
        }

        [Test]
        public void ResetAccessFailedCount_ResetAccessFailedCountIsRequested_SetsAccessFailedCountInZero()
        {
            //Arrange
            var user = new User { AccessFailedCount = 2 };

            //Act
            user.ResetAccessFailedCount();

            //Assert
            Assert.IsTrue(user.AccessFailedCount == 0);
        }

    }
}