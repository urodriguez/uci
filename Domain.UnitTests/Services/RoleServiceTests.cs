using System.Data;
using System.Threading.Tasks;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Predicates;
using Domain.Predicates.Factories;
using Domain.Services;
using Moq;
using NUnit.Framework;

namespace Domain.UnitTests.Services
{
    [TestFixture]
    public class RoleServiceTests
    {
        [Test]
        public void IsAdminAsync_UserNotExists_ThrowsObjectNotFoundException()
        {
            //Arrange
            var userPredicateFactory = new UserPredicateFactory();

            var unityOfWorkMock = new Mock<IUnitOfWork>();
            unityOfWorkMock.Setup(uow => uow.Users.GetFirstAsync(It.IsAny<IInventAppPredicate<User>>())).Returns(
                Task.FromResult<User>(null)
            );

            var roleService = new RoleService(userPredicateFactory, unityOfWorkMock.Object);

            //Act
            AsyncTestDelegate testDelegate = () => roleService.IsAdminAsync(It.IsAny<string>());

            //Assert
            Assert.ThrowsAsync<ObjectNotFoundException>(testDelegate);
        }

        [Test]
        public async Task IsAdminAsync_UserRoleIsAdmin_ReturnsTrue()
        {
            //Arrange
            var userPredicateFactory = new UserPredicateFactory();

            var userAdminMock = new Mock<User>();
            userAdminMock.Setup(u => u.IsAdmin()).Returns(true);

            var unityOfWorkMock = new Mock<IUnitOfWork>();
            unityOfWorkMock.Setup(uow => uow.Users.GetFirstAsync(It.IsAny<IInventAppPredicate<User>>())).Returns(
                Task.FromResult(userAdminMock.Object)
            );

            var roleService = new RoleService(userPredicateFactory, unityOfWorkMock.Object);

            //Act
            var isAdmin = await roleService.IsAdminAsync(It.IsAny<string>());

            //Assert
            Assert.IsTrue(isAdmin);
        }        
        
        [Test]
        public async Task IsAdminAsync_UserRoleIsNotAdmin_ReturnsFalse()
        {
            //Arrange
            var userPredicateFactory = new UserPredicateFactory();

            var userAdminMock = new Mock<User>();
            userAdminMock.Setup(u => u.IsAdmin()).Returns(false);

            var unityOfWorkMock = new Mock<IUnitOfWork>();
            unityOfWorkMock.Setup(uow => uow.Users.GetFirstAsync(It.IsAny<IInventAppPredicate<User>>())).Returns(
                Task.FromResult(userAdminMock.Object)
            );

            var roleService = new RoleService(userPredicateFactory, unityOfWorkMock.Object);

            //Act
            var isAdmin = await roleService.IsAdminAsync(It.IsAny<string>());

            //Assert
            Assert.IsFalse(isAdmin);
        }

    }
}