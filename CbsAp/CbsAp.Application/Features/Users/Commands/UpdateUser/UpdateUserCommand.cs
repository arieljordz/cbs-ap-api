using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Users.Commands.UpdateUser
{
    public record UpdateUserCommand(
          UpdateUserDTO userDTO,
          string UpdatedBy
        ) : ICommand<ResponseResult<string>>
    { }
}