using System;
using System.Threading.Tasks;
using Application.ApplicationResults;
using Application.Contracts;
using Application.Contracts.AggregateUpdaters;
using Application.Contracts.DuplicateValidators;
using Application.Contracts.Factories;
using Application.Dtos;
using Application.Services;
using Domain.Aggregates;
using Domain.Contracts.Infrastructure.Persistence;
using Domain.Contracts.Predicates;
using Domain.Contracts.Predicates.Factories;
using Domain.Contracts.Services;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Auditing;
using Infrastructure.Crosscutting.Authentication;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Mailing;
using Infrastructure.Crosscutting.Rendering;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        [Test]
        public async Task LoginAsync_UsingCorrectPassword_ReturnsOkApplicationResultWithLoginStatusSuccessSecurityToken()
        {
            //Arrange
            var userPredicateFactoryMock = new Mock<IUserPredicateFactory>();

            var userMock = new Mock<User>();
            userMock.SetupGet(u => u.Password).Returns("Pa$$word");
            userMock.SetupGet(u => u.EmailConfirmed).Returns(true);
            userMock.SetupGet(u => u.Active).Returns(true);
            userMock.Setup(u => u.IsLocked()).Returns(false);
            userMock.Setup(u => u.HasPassword(It.IsAny<string>())).Returns(true);
            userMock.SetupGet(u => u.IsUsingCustomPassword).Returns(true);
            userMock.Setup(u => u.ResetAccessFailedCount());

            var unityOfWorkMock = new Mock<IUnitOfWork>();
            unityOfWorkMock.Setup(uow => uow.Users.GetFirstAsync(It.IsAny<IInventAppPredicate<User>>())).Returns(
                Task.FromResult(userMock.Object)
            );

            var tokenExpirationDate = new DateTime(2000, 1, 1, 0, 0, 0);
            var tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(ts => ts.GenerateAsync(It.IsAny<TokenGenerateRequest>())).Returns(
                Task.FromResult(new TokenGenerateResponse
                {
                    Expires = tokenExpirationDate,
                    SecurityToken = "1234.SecurityToken.4567"
                })
            );

            var appSettingServiceMock = new Mock<IAppSettingsService>();
            appSettingServiceMock.Setup(ass => ass.DefaultTokenExpiresTime).Returns(It.IsAny<int>());

            var userService = new UserService(
                userPredicateFactoryMock.Object,
                unityOfWorkMock.Object,
                It.IsAny<IUserFactory>(),
                It.IsAny<IUserUpdater>(),
                It.IsAny<IAuditService>(),
                tokenServiceMock.Object,
                It.IsAny<IRoleService>(),
                It.IsAny<IUserDuplicateValidator>(),
                It.IsAny<ILogService>(),
                It.IsAny<ITemplateFactory>(),
                It.IsAny<ITemplateService>(),
                It.IsAny<IEmailFactory>(),
                It.IsAny<IEmailService>(),
                appSettingServiceMock.Object,
                It.IsAny<IInventAppContext>()
            );

            //Act
            var appResult = (ApplicationResult<LoginResultDto>) await userService.LoginAsync(new UserCredentialDto
            {
                Email = "test@test.com",
                Password = "Pa$$word"
            });

            //Assert
            Assert.IsTrue(appResult.Status == ApplicationResultStatus.Ok);
            Assert.IsTrue(appResult.Data.Status == LoginStatus.Success);
            Assert.IsTrue(appResult.Data.SecurityToken.Token == "1234.SecurityToken.4567");
            Assert.IsTrue(appResult.Data.SecurityToken.Expires == tokenExpirationDate);
        }
        
        [Test]
        public async Task LoginAsync_NotPassingUserCredential_ReturnsApplicationResultWithStatusUnauthenticatedAndLoginStatusInvalidEmailOrPassword()
        {
            //Arrange
            var unityOfWorkMock = new Mock<IUnitOfWork>();

            var logServiceMock = new Mock<ILogService>();

            var userService = new UserService(
                It.IsAny<IUserPredicateFactory>(),
                unityOfWorkMock.Object,
                It.IsAny<IUserFactory>(),
                It.IsAny<IUserUpdater>(),
                It.IsAny<IAuditService>(),
                It.IsAny<ITokenService>(),
                It.IsAny<IRoleService>(),
                It.IsAny<IUserDuplicateValidator>(),
                logServiceMock.Object,
                It.IsAny<ITemplateFactory>(),
                It.IsAny<ITemplateService>(),
                It.IsAny<IEmailFactory>(),
                It.IsAny<IEmailService>(),
                It.IsAny<IAppSettingsService>(),
                It.IsAny<IInventAppContext>()
            );

            //Act
            var appResult = (ApplicationResult<LoginResultDto>) await userService.LoginAsync(null);

            //Assert
            Assert.IsTrue(appResult.Status == ApplicationResultStatus.Unauthenticated);
            Assert.IsTrue(appResult.Data.Status == LoginStatus.InvalidEmailOrPassword);
        }

        [Test]
        public async Task LoginAsync_NotExistingUserEmail_ReturnsApplicationResultWithStatusUnauthenticatedAndLoginStatusInvalidEmailOrPassword()
        {
            //Arrange
            var userPredicateFactoryMock = new Mock<IUserPredicateFactory>();

            var unityOfWorkMock = new Mock<IUnitOfWork>();
            unityOfWorkMock.Setup(uow => uow.Users.GetFirstAsync(It.IsAny<IInventAppPredicate<User>>())).Returns(
                Task.FromResult<User>(null)
            );

            var logServiceMock = new Mock<ILogService>();

            var userService = new UserService(
                userPredicateFactoryMock.Object,
                unityOfWorkMock.Object,
                It.IsAny<IUserFactory>(),
                It.IsAny<IUserUpdater>(),
                It.IsAny<IAuditService>(),
                It.IsAny<ITokenService>(),
                It.IsAny<IRoleService>(),
                It.IsAny<IUserDuplicateValidator>(),
                logServiceMock.Object,
                It.IsAny<ITemplateFactory>(),
                It.IsAny<ITemplateService>(),
                It.IsAny<IEmailFactory>(),
                It.IsAny<IEmailService>(),
                It.IsAny<IAppSettingsService>(),
                It.IsAny<IInventAppContext>()
            );

            //Act
            var appResult = (ApplicationResult<LoginResultDto>) await userService.LoginAsync(new UserCredentialDto
            {
                Email = "notexists@test.com"
            });

            //Assert
            Assert.IsTrue(appResult.Status == ApplicationResultStatus.Unauthenticated);
            Assert.IsTrue(appResult.Data.Status == LoginStatus.InvalidEmailOrPassword);
        }

        [Test]
        public async Task LoginAsync_NotEmailConfirmedUser_ReturnsOkApplicationResultWithLoginStatusUnconfirmedEmail()
        {
            //Arrange
            var userPredicateFactoryMock = new Mock<IUserPredicateFactory>();

            var userMock = new Mock<User>();
            userMock.SetupGet(u => u.EmailConfirmed).Returns(false);

            var unityOfWorkMock = new Mock<IUnitOfWork>();
            unityOfWorkMock.Setup(uow => uow.Users.GetFirstAsync(It.IsAny<IInventAppPredicate<User>>())).Returns(
                Task.FromResult(userMock.Object)
            );

            var logServiceMock = new Mock<ILogService>();

            var userService = new UserService(
                userPredicateFactoryMock.Object,
                unityOfWorkMock.Object,
                It.IsAny<IUserFactory>(),
                It.IsAny<IUserUpdater>(),
                It.IsAny<IAuditService>(),
                It.IsAny<ITokenService>(),
                It.IsAny<IRoleService>(),
                It.IsAny<IUserDuplicateValidator>(),
                logServiceMock.Object,
                It.IsAny<ITemplateFactory>(),
                It.IsAny<ITemplateService>(),
                It.IsAny<IEmailFactory>(),
                It.IsAny<IEmailService>(),
                It.IsAny<IAppSettingsService>(),
                It.IsAny<IInventAppContext>()
            );

            //Act
            var appResult = (ApplicationResult<LoginResultDto>) await userService.LoginAsync(new UserCredentialDto());

            //Assert
            Assert.IsTrue(appResult.Status == ApplicationResultStatus.Ok);
            Assert.IsTrue(appResult.Data.Status == LoginStatus.UnconfirmedEmail);
        }

        [Test]
        public async Task LoginAsync_NotActiveUser_ReturnsOkApplicationResultWithLoginStatusInactive()
        {
            //Arrange
            var userPredicateFactoryMock = new Mock<IUserPredicateFactory>();

            var userMock = new Mock<User>();
            userMock.SetupGet(u => u.EmailConfirmed).Returns(true);
            userMock.SetupGet(u => u.Active).Returns(false);

            var unityOfWorkMock = new Mock<IUnitOfWork>();
            unityOfWorkMock.Setup(uow => uow.Users.GetFirstAsync(It.IsAny<IInventAppPredicate<User>>())).Returns(
                Task.FromResult(userMock.Object)
            );

            var logServiceMock = new Mock<ILogService>();

            var userService = new UserService(
                userPredicateFactoryMock.Object,
                unityOfWorkMock.Object,
                It.IsAny<IUserFactory>(),
                It.IsAny<IUserUpdater>(),
                It.IsAny<IAuditService>(),
                It.IsAny<ITokenService>(),
                It.IsAny<IRoleService>(),
                It.IsAny<IUserDuplicateValidator>(),
                logServiceMock.Object,
                It.IsAny<ITemplateFactory>(),
                It.IsAny<ITemplateService>(),
                It.IsAny<IEmailFactory>(),
                It.IsAny<IEmailService>(),
                It.IsAny<IAppSettingsService>(),
                It.IsAny<IInventAppContext>()
            );

            //Act
            var appResult = (ApplicationResult<LoginResultDto>) await userService.LoginAsync(new UserCredentialDto());

            //Assert
            Assert.IsTrue(appResult.Status == ApplicationResultStatus.Ok);
            Assert.IsTrue(appResult.Data.Status == LoginStatus.Inactive);
        }

        [Test]
        public async Task LoginAsync_LockedUser_ReturnsOkApplicationResultWithLoginStatusLocked()
        {
            //Arrange
            var userPredicateFactoryMock = new Mock<IUserPredicateFactory>();

            var userMock = new Mock<User>();
            userMock.SetupGet(u => u.EmailConfirmed).Returns(true);
            userMock.SetupGet(u => u.Active).Returns(true);
            userMock.Setup(u => u.IsLocked()).Returns(true);
            userMock.Setup(u => u.GenerateDefaultPassword());
            userMock.Setup(u => u.ResetAccessFailedCount());

            var unityOfWorkMock = new Mock<IUnitOfWork>();
            unityOfWorkMock.Setup(uow => uow.Users.GetFirstAsync(It.IsAny<IInventAppPredicate<User>>())).Returns(
                Task.FromResult(userMock.Object)
            );

            var logServiceMock = new Mock<ILogService>();

            var emailFactoryMock = new Mock<IEmailFactory>();

            var emailServiceMock = new Mock<IEmailService>();

            var userService = new UserService(
                userPredicateFactoryMock.Object,
                unityOfWorkMock.Object,
                It.IsAny<IUserFactory>(),
                It.IsAny<IUserUpdater>(),
                It.IsAny<IAuditService>(),
                It.IsAny<ITokenService>(),
                It.IsAny<IRoleService>(),
                It.IsAny<IUserDuplicateValidator>(),
                logServiceMock.Object,
                It.IsAny<ITemplateFactory>(),
                It.IsAny<ITemplateService>(),
                emailFactoryMock.Object,
                emailServiceMock.Object,
                It.IsAny<IAppSettingsService>(),
                It.IsAny<IInventAppContext>()
            );

            //Act
            var appResult = (ApplicationResult<LoginResultDto>) await userService.LoginAsync(new UserCredentialDto());

            //Assert
            Assert.IsTrue(appResult.Status == ApplicationResultStatus.Ok);
            Assert.IsTrue(appResult.Data.Status == LoginStatus.Locked);
        }
    }
}
