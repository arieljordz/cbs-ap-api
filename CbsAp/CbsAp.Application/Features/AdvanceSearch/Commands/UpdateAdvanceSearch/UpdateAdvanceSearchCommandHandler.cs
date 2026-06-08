using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.AdvanceSearch;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.AdvanceSearch;
using CbsAp.Domain.Enums;
using Mapster;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CbsAp.Application.Features.AdvanceSearch.Commands.UpdateAdvanceSearch
{
    public class UpdateAdvanceSearchCommandHandler : ICommandHandler<UpdateAdvanceSearchCommand, ResponseResult<long>>
    {
        private readonly IAdvanceSearchService _AdvanceSearchService;

        private readonly IUnitofWork _unitofWork;

        private readonly ILogger<UpdateAdvanceSearchCommandHandler> _logger;

        public UpdateAdvanceSearchCommandHandler(IAdvanceSearchService AdvanceSearchService, IUnitofWork unitofWork, ILogger<UpdateAdvanceSearchCommandHandler> logger)
        {
            _AdvanceSearchService = AdvanceSearchService;
            _unitofWork = unitofWork;
            _logger = logger;
        }

        public async Task<ResponseResult<long>> Handle(UpdateAdvanceSearchCommand request, CancellationToken cancellationToken)
        {
            var advanceSearch = await GetAdvanceSearch(request.AdvanceSearch.AdvanceSearchId);

            advanceSearch.UserId = request.updatedBy;
            advanceSearch.JsonFilter = JsonSerializer.Serialize(request.AdvanceSearch);
            advanceSearch.FormName = request.AdvanceSearch.FormName;

            if (!await _AdvanceSearchService.UpdateAdvanceSearch(advanceSearch, cancellationToken))
            {
                _logger.LogWarning("Error in updating AdvanceSearch  : {Name}", advanceSearch.AdvanceSearchId);
                return ResponseResult<long>.BadRequest("Error on updating AdvanceSearch");
            }

            return ResponseResult<long>.OK(advanceSearch.AdvanceSearchId,MessageConstants.Message(MessageOperationType.Update, "AdvanceSearch"));
        }

        private async Task<CbsAp.Domain.Entities.AdvanceSearch.AdvanceSearch> GetAdvanceSearch(long AdvanceSearchID)
        {
            return await _unitofWork.GetRepository<CbsAp.Domain.Entities.AdvanceSearch.AdvanceSearch>().GetByIdAsync(AdvanceSearchID);
        }
    }
}