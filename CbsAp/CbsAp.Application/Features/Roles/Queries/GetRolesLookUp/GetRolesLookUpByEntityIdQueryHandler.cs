using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.RoleManagement;
using Microsoft.EntityFrameworkCore;



namespace CbsAp.Application.Features.Roles.Queries.GetRolesLookUp
{
    public class GetRolesLookUpByEntityIdQueryHandler
    : IQueryHandler<GetRolesLookUpByEntityIdQuery, ResponseResult<IEnumerable<RoleDTO>>>
    {
        private readonly IUnitofWork _unitOfWork;



        public GetRolesLookUpByEntityIdQueryHandler(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public async Task<ResponseResult<IEnumerable<RoleDTO>>> Handle(GetRolesLookUpByEntityIdQuery request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Role>();



            var roleQuery = repository
            .Query()
            .AsNoTracking()
            .Include(r => r.RoleEntities)
            .Where(r =>
            r.IsActive &&
            r.CanBeAddedToInvoice &&
          // r.RoleEntities.Any(re => re.EntityProfileID == request.EntityID));
          (!r.RoleEntities.Any() || r.RoleEntities.Any(re => re.EntityProfileID == request.EntityID))

          );


            var roles = await roleQuery
            .Select(x => new RoleDTO
            {
                RoleID = x.RoleID,
                RoleName = x.RoleName ?? string.Empty
            })
            .ToListAsync(cancellationToken);



            return ResponseResult<IEnumerable<RoleDTO>>
            .SuccessRetrieveRecords(roles);
        }
    }
}