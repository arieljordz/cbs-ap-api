using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.AdvanceSearch;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.AdvanceSearch;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.AdvanceSearch;
using CbsAp.Domain.Enums;
using Mapster;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CbsAp.Application.Features.AdvanceSearch.Commands.CreateAdvanceSearch
{
    public class CreateAdvanceSearchCommandHandler : ICommandHandler<CreateAdvanceSearchCommand, ResponseResult<long>>
    {
        private readonly IAdvanceSearchService _AdvanceSearchService;
        private readonly ILogger<CreateAdvanceSearchCommandHandler> _logger;

        public CreateAdvanceSearchCommandHandler(
           IAdvanceSearchService AdvanceSearchService,
           ILogger<CreateAdvanceSearchCommandHandler> logger
            )
        {
            _AdvanceSearchService = AdvanceSearchService;
           _logger = logger;
        }

        public async Task<ResponseResult<long>> Handle(CreateAdvanceSearchCommand request, CancellationToken cancellationToken)
        {

            var advanceSearch = new CbsAp.Domain.Entities.AdvanceSearch.AdvanceSearch();

            advanceSearch.SetAuditFieldsOnCreate(request.CreatedBy);

            advanceSearch.UserId = request.CreatedBy;
            advanceSearch.JsonFilter = JsonSerializer.Serialize(request.advanceSearch);
            advanceSearch.FormName = request.advanceSearch.FormName;
       
            

            if (!await _AdvanceSearchService.CreateAdvanceSearch(advanceSearch, cancellationToken))
            {
                _logger.LogError("Error in adding new AdvanceSearch : {Name}", advanceSearch.FormName);
                return ResponseResult<long>.BadRequest("Error adding new AdvanceSearch");
            }

            return ResponseResult<long>.Created(advanceSearch.AdvanceSearchId,MessageConstants.Message(MessageOperationType.Create, "AdvanceSearch"));
        }
    }
}
