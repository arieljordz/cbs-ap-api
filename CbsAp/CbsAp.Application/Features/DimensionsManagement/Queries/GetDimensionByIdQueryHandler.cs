using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.DimensionsManagement;
using CbsAp.Application.Features.DimensionsManagement.Queries.Common;
using CbsAp.Application.Shared.ResultPatten;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.DimensionsManagement.Queries
{
    public class GetDimensionByIdQueryHandler : IQueryHandler<GetDimensionByIdQuery, ResponseResult<DimensionDTO>>
    {
        private readonly IDimensionRepository _dimensionRepository;

        public GetDimensionByIdQueryHandler(IDimensionRepository dimensionRepository)
        {
            _dimensionRepository = dimensionRepository;
        }

        public async Task<ResponseResult<DimensionDTO>> Handle(GetDimensionByIdQuery request, CancellationToken cancellationToken)
        {
            var dimension = await _dimensionRepository.GetDimensionsAsQueryable()
                .Include(d => d.EntityProfile)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DimensionID == request.DimensionID, cancellationToken);

            if (dimension == null)
            {
                return ResponseResult<DimensionDTO>.NotFound("Dimension not found");
            }

            var dto = new DimensionDTO
            {
                DimensionID = dimension.DimensionID,
                Entity = dimension.EntityProfile != null ? dimension.EntityProfile.EntityName : string.Empty,
                Dimension = dimension.DimensionCode,
                DimensionName = dimension.Name,
                Active = dimension.IsActive,
            };

            return ResponseResult<DimensionDTO>.OK(dto);
        }
    }
}
