using CbsAp.Application.Abstractions.NotificationStrategy;
using CbsAp.Application.Abstractions.NotificationStrategy.NotificationContext;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations;
using CbsAp.Application.DTOs;
using CbsAp.Application.Features.Users.Commands.Common;
using CbsAp.Application.Features.Users.Commands.CreateNewUser;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Domain.Entities.UserManagement;
using CbsAp.Infrastracture.Contexts.Notification;
using CbsAp.Infrastracture.Persistence.EntityConfiguration;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System.Linq.Expressions;
using System.Text;

namespace CbsAp.UnitTesting.ApplicationTest.UserManagament.Command
{
    public class CreateUserCommandHandlerTest
    {
        private readonly Mock<IUnitofWork> _unitWork;
        private readonly Mock<ISender> _mediator;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<INotificationContext> _notificationContext;
        private readonly Mock<IOptions<AppSettings>> _option;
        private readonly CreateUserCommandHandler _createUserCommandHandler;

        public CreateUserCommandHandlerTest()
        {
            _unitWork = new Mock<IUnitofWork>();
            _mapper = new Mock<IMapper>();
            _mediator = new Mock<ISender>();
            _notificationContext = new Mock<INotificationContext>();
            _option = new Mock<IOptions<AppSettings>>();
            _createUserCommandHandler =
                new CreateUserCommandHandler(
                    _unitWork.Object,
                    _mediator.Object,
                    _mapper.Object,
                    _notificationContext.Object,
                    _option.Object
                    );
        }
       

        //[Fact]
        //public async Task Given_NewUser_then_CreateNew()
        //{
        //    //Arrange
        //    var expectedUserPasswordDTO = ExpectedUserLogInfo();

        //    var resultUserInfoPassword = Result<UpdatePasswordDTO>.Success(expectedUserPasswordDTO);
        //    var createUserCommand = GetCreateUserCommand();
        //    var isUserExist = false;
        //    var expectedResult = Result<string>
        //        .SuccessAsync(createUserCommand.UserId, "New User created successfully");

        //    long userAccountID = 1;

        //    List<UserAccount> userAccounts = new List<UserAccount>() {
        //        new UserAccount() {
        //         UserAccountID = userAccountID
        //        }
        //    };

        //   var userRoleAdd =  createUserCommand.Roles.Select(roleID =>
        //                  new UserRole
        //                  {
        //                      UserAccountID = userAccountID,
        //                      RoleID = roleID
        //                  }
        //              ).SetAuditFieldsOnCreate(createUserCommand.CreatedBy);
            
        //    var emailBindData = new Dictionary<string, string> {
        //                { "{firstName}" , "test name" },
        //                { "{customerName}", "Test Customer" },
        //                { "{tempPassword}", "test password" }
        //            };


        //    IQueryable<UserAccount> userAccountQuery = userAccounts.AsQueryable();

        //    _mediator.Setup(m => m.Send(It.IsAny<GenerateUserPasswordCommand>()
        //        , It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(resultUserInfoPassword).Verifiable();

        //    _mapper.Setup(m =>
        //        m.Map<UserLogInfo>(It.IsAny<UpdatePasswordDTO>()
        //        )).Returns(ExpectedUserLogInfoPass)
        //        .Verifiable();

        //    _unitWork.Setup(userAccount => userAccount.GetRepository<UserAccount>()
        //        .AnyAsync(It.IsAny<Expression<Func<UserAccount, bool>>>()).Result)
        //        .Returns(isUserExist);

        //    _unitWork.SetupGet(userAccount => userAccount.GetRepository<UserAccount>()
        //        .AddAsync(It.IsAny<UserAccount>()).Result)
        //        .Returns(It.IsAny<UserAccount>())
        //        .Verifiable();


        //    _unitWork.SetupGet(userAccountRole => userAccountRole.GetRepository<UserAccount>()
        //      .ApplyPredicateAsync(useraccount => useraccount.UserID == createUserCommand.UserId).Result)
        //     .Returns(userAccountQuery).Verifiable();

        //    _unitWork.Setup(userRole => userRole.GetRepository<UserRole>()
        //       .AddRangeAsync(userRoleAdd))
        //       .Returns(Task.CompletedTask)
        //       .Verifiable();


        //    _option.Setup(o=> o.Value).Returns(new AppSettings { WebUrl ="TEST"}).Verifiable();


        //    _notificationContext.Setup(notif =>
        //    notif.GetNotificationTypeStrategy(NotificationType.NewUserNotification)
        //    .SendNotificationAsync("xx@yahoo.com", emailBindData))
        //        .ReturnsAsync(true).Verifiable();

           
        //    _unitWork.Setup(uow => uow.SaveChanges(It.IsAny<CancellationToken>()).Result)
        //        .Returns(true);


        //    //act

        //    var result = await _createUserCommandHandler.Handle(createUserCommand, It.IsAny<CancellationToken>());

        //    //Assert
        //    Assert.False(isUserExist); // return value if user not existed add user
        //    Assert.True(result.IsSuccess);
        //    Assert.Contains(expectedResult.Result.Messages, "New User created successfully");
        //}

        [Fact]
        public async Task Given_NewUser_when_UserExist_then_RecordShouldNotCreated()
        {
            //Arrange
            var userAccountRepositoryMock =
                new Mock<IRepository<UserAccount>>();
            var userInfoPasswordRepositoryMock =
                new Mock<IRepository<UserLogInfo>>();
            var expectedUserPasswordDTO = ExpectedUserLogInfo();

            var resultUserInfoPassword = Result<UpdatePasswordDTO>.Success(expectedUserPasswordDTO);
            var createUserCommand = GetCreateUserCommand();
            var isUserExist = true;
            var expectedResult = Result<string>
                .SuccessAsync(createUserCommand.UserId, "New User created successfully");
           

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

            _unitWork.SetupGet(userAccount => userAccount.GetRepository<UserAccount>()
                .AddAsync(It.IsAny<UserAccount>()).Result)
                .Returns(It.IsAny<UserAccount>())
                .Verifiable();

            _unitWork.SetupGet(userLogInfo => userLogInfo.GetRepository<UserLogInfo>()
                .AddAsync(It.IsAny<UserLogInfo>()).Result)
                .Returns(It.IsAny<UserLogInfo>())
                .Verifiable();

        

            _unitWork.Setup(uow => uow.SaveChanges(It.IsAny<CancellationToken>()).Result)
                .Returns(false);

            //act

            var result = await _createUserCommandHandler
                            .Handle(createUserCommand, It.IsAny<CancellationToken>());

            //Assert
            Assert.True(isUserExist); // return value if user not existed add user
            Assert.True(!result.IsSuccess);
            Assert.Contains(expectedResult.Result.Messages, "New User created successfully");
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
                               new List<int>() { 1, 2, 2 }

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
                PasswordSalt ="WEWEW232321112121",
                CreatedBy = "systemAdmin"
            };
        }

        private  AppSettings ppSettingValue ()  {

            return new AppSettings()
            {
                WebUrl = "xxx"
            };
        }
    }
}