using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Features.Users.Commands.Common;
using CbsAp.Application.Shared.Encryption;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.PermissionManagement;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Domain.Entities.UserManagement;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, ResponseResult<string>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IUserManagementRepository _userManagementRepository;
        private readonly ISender _mediator;
        private readonly IHasher _hasher;
        private readonly IMapper _mapper;

        private readonly ILogger<UpdateUserCommandHandler> _logger;

        public UpdateUserCommandHandler(
            IUnitofWork unitofWork,
            IUserManagementRepository userManagementRepository,
            ISender mediator,
            IHasher hasher,
            IMapper mapper,
            ILogger<UpdateUserCommandHandler> logger
            )
        {
            _unitofWork = unitofWork;
            _userManagementRepository = userManagementRepository;
            _mediator = mediator;
            _hasher = hasher;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseResult<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (await IsUserExistAsync(request.userDTO))
            {
                _logger.LogWarning("Email Address {@EmailAddress} not existed ", request.userDTO.EmailAddress);
                return ResponseResult<string>.BadRequest("Email address is existed.");
            }

            var userAccountDetail = await _userManagementRepository.GetUserAccountAsQueryable()
                .Include(u => u.UserLogInfo)
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.UserID == request.userDTO.UserID, cancellationToken);

            if (userAccountDetail == null)
            {
                _logger.LogWarning("User account {@UserID} not existed ", request.userDTO.UserID);
                return ResponseResult<string>.NotFound("User account not existed");
            }

            userAccountDetail.FirstName = request.userDTO.FirstName;
            userAccountDetail.LastName = request.userDTO.LastName;
            userAccountDetail.EmailAddress = request.userDTO.EmailAddress;
            userAccountDetail.IsActive = request.userDTO.IsActive;

            if (request.userDTO.PasswordMandatory && !string.IsNullOrWhiteSpace(request.userDTO.Password))
            {
                // If password is mandatory and provided, update the password
                var hashedPassword = _hasher.HashPasword(request.userDTO.Password, out var salt);
                userAccountDetail.UserLogInfo.PasswordHash = hashedPassword;
                userAccountDetail.UserLogInfo.PasswordSalt = Convert.ToBase64String(salt);
            }

            // ======= Update user roles =======
            var newUserRoleIds = request.userDTO.userRoles.ToHashSet() ?? [];

            var toRemoveUserRoles = userAccountDetail.UserRoles
                .Where(r => !newUserRoleIds.Contains(r.RoleID))
                .ToList();

            foreach (var item in toRemoveUserRoles)
            {
                userAccountDetail.UserRoles.Remove(item);
            }

            // Add new ones
            foreach (var roleId in newUserRoleIds)
            {
                if (!userAccountDetail.UserRoles.Any(r => r.RoleID == roleId))
                {
                    userAccountDetail.UserRoles.Add(new UserRole { RoleID = roleId });
                }
            }

            var isSuccess = await _unitofWork.SaveChanges(string.Empty,string.Empty,cancellationToken);

            if (!isSuccess)
            {
                _logger.LogError("Error on  updating user : {@UserID}", request.userDTO.UserID);
                return ResponseResult<string>.BadRequest("Error on updating user ");
            }
            return ResponseResult<string>.OK("user is successfully updated");
        }

        private async Task<bool> IsUserExistAsync(UpdateUserDTO dto)
        {
            return await _unitofWork.GetRepository<UserAccount>()
                .AnyAsync(u => u.UserAccountID != dto.UserAccountID
                    && u.EmailAddress == dto.EmailAddress);
        }
    }
}
