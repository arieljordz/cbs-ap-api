using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Users.Commands.CreateNewUser
{
    public record CreateUserCommand(
        CreateUserDTO userDTO,
        string CreatedBy)
        : ICommand<ResponseResult<string>>
    {
    }
}