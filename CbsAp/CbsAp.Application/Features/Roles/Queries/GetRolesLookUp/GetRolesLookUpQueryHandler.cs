using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.RoleManagement;
using Mapster;

namespace CbsAp.Application.Features.Roles.Queries.GetRolesLookUp
{
    public class GetRolesLookUpQueryHandler : IQueryHandler<GetRolesLookUpQuery, ResponseResult<IEnumerable<RoleDTO>>>
    {
        private readonly IUnitofWork _unitofWork;

        public GetRolesLookUpQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<IEnumerable<RoleDTO>>> Handle(GetRolesLookUpQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitofWork.GetRepository<Role>().ApplyPredicateAsync(r => r.IsActive && r.CanBeAddedToInvoice);

            var mapDTO = result.AsEnumerable().Adapt<IEnumerable<RoleDTO>>() ?? Enumerable.Empty<RoleDTO>();

            return ResponseResult<IEnumerable<RoleDTO>>.OK(mapDTO, string.Empty);
        }
    }
}