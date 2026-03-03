using CbsAp.Application.Abstractions.Services.Authentication;
using CbsAp.Application.Features.Authentication.Queries;
using CbsAp.Application.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Moq;

namespace CbsAp.UnitTesting.ApplicationTest.Authentication.Queries
{
    public class LoginQueryHandlerTest
    {
        private readonly Mock<ILogger<LoginQueryHandler>> _logger;
        private readonly Mock<IAuthenticationJwtService> _authenticationJwtService;
        private readonly LoginQueryHandler _loginQueryHandler;

        public LoginQueryHandlerTest()
        {
            _logger = new Mock<ILogger<LoginQueryHandler>>();
            _authenticationJwtService = new Mock<IAuthenticationJwtService>(MockBehavior.Strict);
            _loginQueryHandler = new LoginQueryHandler(_authenticationJwtService.Object, _logger.Object);
        }

        [Fact]
        public async Task Given_UserIsAuthenticated_When_ValidUserCredential_then_GenerateToken()
        {
            var loginQuery = new LoginQuery("cbsapTestUser@canon.com.au", "Qwerty$123456");
            var expectedToken = "generatedToken";

            _authenticationJwtService
                .Setup(x => x.GenerateUserJwtToken(loginQuery.Username))
                .Returns(expectedToken)
                .Verifiable();

            var tokenResponse = await _loginQueryHandler.Handle(loginQuery, new CancellationToken());

            Assert.Equal(expectedToken, tokenResponse.Data?.Token);
            Assert.Equal(loginQuery.Username, tokenResponse.Data?.UserName);
        }

        [Fact]
        public async Task Given_InValidUserCredential_when_GenerateToken_then_ThrowException()
        {
            var loginQuery = new LoginQuery("JohnDoe", "12345");
            var expectedFailResult =
                Result<AuthenticationResult>.Failure("Authentication Error: username and password unauthorized");
            var isValidUserCredential = false;

            _authenticationJwtService
                .Setup(x => x.GenerateUserJwtToken(loginQuery.Username))
                .Verifiable();

            var tokenResponse = await _loginQueryHandler.Handle(loginQuery, new CancellationToken());

            Assert.Equal(tokenResponse.IsSuccess, isValidUserCredential);
            Assert.Equal(tokenResponse.Messages, expectedFailResult.Messages);
        }
    }
}