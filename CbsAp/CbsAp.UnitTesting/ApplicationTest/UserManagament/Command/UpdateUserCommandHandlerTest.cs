using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs;
using CbsAp.Application.Features.Users.Commands.Common;
using CbsAp.Application.Features.Users.Commands.UpdateUser;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Domain.Entities.UserManagement;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;

namespace CbsAp.UnitTesting.ApplicationTest.UserManagament.Command
{
    public class UpdateUserCommandHandlerTest
    {
        private readonly Mock<IUnitofWork> _unitWork;
        private readonly Mock<ISender> _mediator;
        private readonly Mock<IMapper> _mapper;
        private readonly UpdateUserCommandHandler _updateUserCommandHandler;

        public UpdateUserCommandHandlerTest()
        {
            _unitWork = new Mock<IUnitofWork>();
            _mapper = new Mock<IMapper>();
            _mediator = new Mock<ISender>();
            _updateUserCommandHandler =
                new UpdateUserCommandHandler(_unitWork.Object, _mediator.Object, _mapper.Object);
        }

        [Fact]
        public async Task Given_UserExists_then_UpdateUser()
        {
            //Arrange
            var expectedUserPasswordDTO = ExpectedUserLogInfo();
            var resultUserInfoPassword =
                Result<UpdatePasswordDTO>.Success(expectedUserPasswordDTO);

            var updateUserCommand = UpdateCreateUserCommand();

            var expectedResult = Result<string>
                .SuccessAsync(updateUserCommand.UserId, "Successfully updated");

            var isUserExist = true;
            long userLogInfoID = 1;
            long userAccountID = 1;

            var user = new UserAccount()
            {
                UserAccountID = userAccountID,
                CreatedBy = "admin",
                CreatedDate = DateTime.UtcNow
            };

            List<UserAccount> userAccounts = new List<UserAccount>() {
                new UserAccount() {
                 UserAccountID = userAccountID,
                }
            };
            List<UserLogInfo> userlogInfo = new List<UserLogInfo>() {
                new UserLogInfo() {
                 UserLogInfoID = userLogInfoID
                }
            };
            IQueryable<UserAccount> userAccountQuery = userAccounts.AsQueryable();
            IQueryable<UserLogInfo> userLogInfoQuery = userlogInfo.AsQueryable();

            var userRoleAdd = updateUserCommand
                .Roles
                .Select(roleID =>
                    new UserRole
                    {
                        UserAccountID = userAccountID,
                        RoleID = roleID
                    }
                ).SetAuditFieldsOnCreate(updateUserCommand.UpdatedBy).AsQueryable();

            _mediator.Setup(m => m.Send(It.IsAny<GenerateUserPasswordCommand>()
                , It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultUserInfoPassword).Verifiable();

            _mapper.Setup(m =>
                m.Map<UserLogInfo>(It.IsAny<UpdatePasswordDTO>()
                )).Returns(ExpectedUserLogInfoPass)
                .Verifiable();

            _unitWork.Setup(userAccount => userAccount.GetRepository<UserAccount>()
                .AnyAsync(It.IsAny<Expression<Func<UserAccount, bool>>>()).Result)
                .Returns(isUserExist);

            _unitWork.Setup(userAccountRole => userAccountRole.GetRepository<UserAccount>()
                      .ApplyPredicateAsync(useraccount => useraccount.UserID == updateUserCommand.UserId).Result)
                      .Returns(userAccountQuery)
                      .Verifiable();

            _unitWork.SetupGet(userLogInfo => userLogInfo.GetRepository<UserLogInfo>()
                 .ApplyPredicateAsync(x => x.UserID == updateUserCommand.UserId).Result)
                 .Returns(userLogInfoQuery)
                 .Verifiable();

            _unitWork.Setup(userAccount => userAccount.GetRepository<UserAccount>()
                .UpdateAsync(userAccountID, It.IsAny<UserAccount>()))
                .Verifiable();

            _unitWork.Setup(userLogInfo => userLogInfo.GetRepository<UserLogInfo>()
               .UpdateAsync(userLogInfoID, It.IsAny<UserLogInfo>()))
                .Verifiable();

            _unitWork.Setup(userLogInfo => userLogInfo.GetRepository<UserRole>()
              .GetByIdAsync(userAccountID).Result).Returns(It.IsAny<UserRole>())
               .Verifiable();

            _unitWork.Setup(userAccountRole =>
                              userAccountRole.GetRepository<UserRole>()
                              .ApplyPredicateAsync(useraccount =>
                                      useraccount.UserAccountID == updateUserCommand.UserAccountID).Result)
                                     .Returns(userRoleAdd).Verifiable();

        

            _unitWork.Setup(userRole => userRole.GetRepository<UserRole>()
               .AddRangeAsync(userRoleAdd))
               .Returns(Task.CompletedTask)
               .Verifiable();

            _unitWork.Setup(uow => uow.SaveChanges(It.IsAny<CancellationToken>()).Result)
                .Returns(true);

            //act

            var result = await _updateUserCommandHandler.Handle(updateUserCommand, It.IsAny<CancellationToken>());

            //Assert
            Assert.True(isUserExist); // return value if user not existed add user
            Assert.True(result.IsSuccess);
            Assert.Contains(expectedResult.Result.Messages, "Successfully updated");
        }

        [Fact]
        public async Task Given_ExistingUser_when_NotUserNotExisting_then_RecordShouldNotUpdated()
        {
            //Arrange
            var updateUserCommand = UpdateCreateUserCommand();
            var expectedResult = Result<string>.FailureAsync("User Id not found");
            var isUserExist = false;

            _unitWork.Setup(userAccount => userAccount.GetRepository<UserAccount>()
                .AnyAsync(It.IsAny<Expression<Func<UserAccount, bool>>>()).Result)
                .Returns(isUserExist);

            _unitWork.Setup(uow => uow.SaveChanges(It.IsAny<CancellationToken>()).Result)
                .Returns(false);

            //act

            var result = await _updateUserCommandHandler
                            .Handle(updateUserCommand, It.IsAny<CancellationToken>());

            //Assert
            Assert.False(isUserExist);
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedResult.Result.Messages, "User Id not found");
        }

        private static UpdateUserCommand UpdateCreateUserCommand()
        {
            return new UpdateUserCommand(
                  1,
                               "JohnDoe@canon.com.au",
                               "John",
                               "Doe",
                               "JohnDoe@canon.com.au",
                               "qwerty123",
                               true,
                               "systemAdministrator",
                                new List<int>() { 1, 2, 3 },
                                false
                                );
        }

        private static UpdatePasswordDTO ExpectedUserLogInfo()
        {
            return new UpdatePasswordDTO()
            {
                PasswordHash = "12121212WEWEWDFSG54",
                PasswordSalt = "WEWEW232321112121",
                LastUpdatedBy = "systemAdmin"
            };
        }

        private static UserLogInfo ExpectedUserLogInfoPass()
        {
            return new UserLogInfo()
            {
                PasswordHash = "12121212WEWEWDFSG54",
                PasswordSalt = "WEWEW232321112121",
                CreatedBy = "systemAdmin"
            };
        }
    }
}