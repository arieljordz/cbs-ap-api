using CbsAp.API.Controllers.v1;
using CbsAp.Application.Features.Users.Commands.Common;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CbsAp.UnitTesting.ApiControllerTest
{
    public class UserManagementControllerTest
    {
        private readonly Mock<ISender> _mediator;
        private readonly Mock<IMapper> _mapper;
        private readonly UserManagementController _userManagementController;

        public UserManagementControllerTest()
        {
            _mediator = new Mock<ISender>();
            _mapper = new Mock<IMapper>();
            _userManagementController = new UserManagementController(_mediator.Object, _mapper.Object);
        }

        [Fact]
        public async Task Given_CreateNewUser_Then_ShouldCreateNewRecord()
        {
            //Arrange
            var createUserCommand = GetCreateUserCommand();
            var expectedMediatorResult = ResponseResult<string>.Success("Success Creation");

            _mediator.Setup(m =>
            m.Send(createUserCommand, It.IsAny<CancellationToken>()).Result)
                .Returns(expectedMediatorResult);

            //Act
            var result = await _userManagementController
                .CreateUserAccount(createUserCommand);
            var objectResult = result as ObjectResult;

            //Assert
            var successResultData = Assert.IsType<ResponseResult<string>>(objectResult?.Value);
            var createdUserResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(expectedMediatorResult.Messages.FirstOrDefault(), successResultData.Messages.FirstOrDefault());
            Assert.Equal(StatusCodes.Status201Created, createdUserResult.StatusCode);
        }

        [Fact]
        public async Task Given_CreateNewUserDuplicate_Then_ShouldNotCreated()
        {
            //Arrange
            var createUserCommand = GetCreateUserCommand();
            var expectedMediatorResult = ResponseResult<string>.Failure(400, "Fail Creation");

            _mediator.Setup(m =>
            m.Send(createUserCommand, It.IsAny<CancellationToken>()).Result)
                .Returns(expectedMediatorResult);

            //Act
            var result = await _userManagementController
                .CreateUserAccount(createUserCommand);
            var badRequestObjectResult = result as ObjectResult;

            //Assert
            var badRequestResultData = Assert.IsType<ObjectResult>(badRequestObjectResult);
            Assert.Equal(expectedMediatorResult.Messages, badRequestObjectResult?.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestObjectResult?.StatusCode);
        }

        private static CreateUserCommand GetCreateUserCommand()
        {
            return new CreateUserCommand(
                               "JohnDoe@canon.com.au",
                               "John",
                               "Doe",
                               "JohnDoe@canon.com.au",
                               "qwerty123",
                               true,
                               "systemAdministrator",
                               new List<int>() { 1, 2, 3 }

                                );
        }
    }
}