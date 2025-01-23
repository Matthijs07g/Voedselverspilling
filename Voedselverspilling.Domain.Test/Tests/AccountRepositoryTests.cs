using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using Voedselverspilling.Domain.Models;
using Voedselverspilling.Infrastructure.Repositories;
using Xunit;

namespace Voedselverspilling.Domain.Test.Tests
{
    public class AccountRepositoryTests
    {
        private readonly Mock<UserManager<AppIdentity>> _userManagerMock;
        private readonly Mock<SignInManager<AppIdentity>> _signInManagerMock;
        private readonly AccountRepository _accountRepository;

        public AccountRepositoryTests()
        {
            // Setup mocks
            _userManagerMock = new Mock<UserManager<AppIdentity>>(
                Mock.Of<IUserStore<AppIdentity>>(), null, null, null, null, null, null, null, null);

            _signInManagerMock = new Mock<SignInManager<AppIdentity>>(
                _userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<AppIdentity>>(),
                null, null, null, null);

            _accountRepository = new AccountRepository(_userManagerMock.Object, _signInManagerMock.Object);
        }

        [Fact]
        public async Task LoginAsync_UserNotFound_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "nonexistent@example.com", Password = "password" };

            _userManagerMock.Setup(x => x.Users)
                .Returns(new List<AppIdentity>().AsQueryable()); // No users found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _accountRepository.LoginAsync(loginRequest));

            Assert.Equal("Invalid login attempt", exception.Message);
        }

        [Fact]
        public async Task LoginAsync_InvalidPassword_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var user = new AppIdentity { Email = "test@example.com", Rol = "student" };
            var loginRequest = new LoginRequest { Email = user.Email, Password = "wrongpassword" };

            _userManagerMock.Setup(x => x.Users)
                .Returns(new List<AppIdentity> { user }.AsQueryable());

            _signInManagerMock.Setup(x => x.PasswordSignInAsync(user, loginRequest.Password, true, false))
                .ReturnsAsync(SignInResult.Failed);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _accountRepository.LoginAsync(loginRequest));

            Assert.Equal("Invalid credentials", exception.Message);
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ShouldReturnUser()
        {
            // Arrange
            var user = new AppIdentity { Email = "test@example.com", Rol = "student" };
            var loginRequest = new LoginRequest { Email = user.Email, Password = "correctpassword" };

            _userManagerMock.Setup(x => x.Users)
                .Returns(new List<AppIdentity> { user }.AsQueryable());

            _signInManagerMock.Setup(x => x.PasswordSignInAsync(user, loginRequest.Password, true, false))
                .ReturnsAsync(SignInResult.Success);

            _signInManagerMock.Setup(x => x.SignInAsync(user, false, null))
                .Returns(Task.CompletedTask);  // Corrected signature with `authenticationProperties`

            // Act
            var result = await _accountRepository.LoginAsync(loginRequest);

            // Assert
            Assert.Equal(user, result);
        }
    }
}
