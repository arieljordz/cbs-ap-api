using CbsAp.API.Controllers.v1;
using CbsAp.Application.Features.Authentication.Common;
using CbsAp.Application.Features.Authentication.Queries;
using CbsAp.Application.Shared;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace CbsAp.UnitTesting.ApiControllerTest
{
    public class AuthenticationControllerTest
    {
        private readonly Mock<ISender> _mediator;
        private readonly Mock<IMapper> _mapper;
        private readonly AuthenticationController _authenticationController;

        public AuthenticationControllerTest()
        {
            _mediator = new Mock<ISender>();
            _mapper = new Mock<IMapper>();
            _authenticationController = new AuthenticationController(_mediator.Object, _mapper.Object);
        }

        [Fact]
        public async Task Login_Given_UserInputCorrectCredential()
        {
            var credential = new LoginRequest("JohnDoe", "qwerty");
            var expectedResult =
                new AuthenticationTokenResult("JohnDoe", "generateToken");
            Result<AuthenticationTokenResult>? response;

            var expectedObjectResult = Result<AuthenticationTokenResult>.Success(expectedResult);

            _mapper.Setup(x => x.Map<LoginQuery>(credential))
                .Returns(new LoginQuery(credential.Username, credential.Password))
                .Verifiable();
            _mediator.Setup(m => m.Send(It.IsAny<LoginQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedObjectResult)
                .Verifiable();

            _mapper.Setup(x => x.Map<Result<AuthenticationTokenResult>>(expectedResult))
                .Returns(response = expectedObjectResult
                )
                .Verifiable();

            var result = await _authenticationController.Login(credential);

            var okResult = result as OkObjectResult;

            Assert.Equal(response?.IsSuccess, expectedObjectResult.IsSuccess);
            Assert.Equal(response?.Data!.UserName, expectedObjectResult.Data!.UserName);
            Assert.Equal(response?.Data!.Token, expectedResult.Token);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult?
                .StatusCode);
        }

        [Fact]
        public async Task Login_Given_UserInputInCorrectCredential()
        {
            var credential = new LoginRequest("JohnDoe", "qwerty");
            Result<AuthenticationTokenResult>? response =
                 Result<AuthenticationTokenResult>.Failure("Error user credential");

            _mapper.Setup(x => x.Map<LoginQuery>(credential))
                .Returns(new LoginQuery(credential.Username, credential.Password))
                .Verifiable();
            _mediator.Setup(m => m.Send(It.IsAny<LoginQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response)
                .Verifiable();

            var result = await _authenticationController.Login(credential);

            var badRequestResult = result as BadRequestObjectResult;

            Assert.False(response.IsSuccess);
            Assert.Null(response?.Data);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult?
                .StatusCode);
        }
    }
}