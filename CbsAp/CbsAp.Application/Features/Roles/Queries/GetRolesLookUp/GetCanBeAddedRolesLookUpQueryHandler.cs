using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.RoleManagement;
using Mapster;

namespace CbsAp.Application.Features.Roles.Queries.GetRolesLookUp
{
    public class GetCanBeAddedRolesLookUpQueryHandler : IQueryHandler<GetCanBeAddedRolesLookUpQuery, ResponseResult<IEnumerable<RoleDTO>>>
    {
        private readonly IUnitofWork _unitofWork;

        public GetCanBeAddedRolesLookUpQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<IEnumerable<RoleDTO>>> Handle(GetCanBeAddedRolesLookUpQuery request, CancellationToken cancellationToken)
        {
            var invoiceRepository = _unitofWork.GetRepository<Invoice>();
            var roleRepository = _unitofWork.GetRepository<Role>();
            var roleEntityRepository = _unitofWork.GetRepository<RoleEntity>();

            var invoice = await invoiceRepository.GetByIdAsync(request.InvoiceID);

            if (invoice is null)
            {
                return ResponseResult<IEnumerable<RoleDTO>>.NotFound("Invoice not found.");
            }

            var entityProfileId = invoice.EntityProfileID;

            var roleEntities = await roleEntityRepository.ApplyPredicateAsync(
                x => x.EntityProfileID == entityProfileId
            );

            IEnumerable<Role> roles;

            if (roleEntities.Any())
            {
                var roleIds = roleEntities
                    .Select(x => x.RoleID)
                    .Distinct()
                    .ToList();

                roles = await roleRepository.ApplyPredicateAsync(
                    r => roleIds.Contains(r.RoleID)
                         && r.IsActive
                         && r.CanBeAddedToInvoice
                );
            }
            else
            {
                roles = await roleRepository.ApplyPredicateAsync(
                    r => r.IsActive
                         && r.CanBeAddedToInvoice
                );
            }

            var mapDTO = roles.Adapt<IEnumerable<RoleDTO>>();

            return ResponseResult<IEnumerable<RoleDTO>>
                .OK(mapDTO, string.Empty);
        }
    }
}