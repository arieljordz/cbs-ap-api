using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.AddComment
{
    public class InvCommentCommandHandler : ICommandHandler<InvCommentCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;

        public InvCommentCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<bool>> Handle(InvCommentCommand request, CancellationToken cancellationToken)
        {
            var invRepo = _unitofWork.GetRepository<Invoice>();

            var invoiceExist = await invRepo
                .Query()
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.InvoiceID == request.Dto.InvoiceID);

            if (invoiceExist == null)
                return ResponseResult<bool>.NotFound("Invoice is not found");

            var comment = new InvoiceComment
            {
                InvoiceID = request.Dto.InvoiceID,
                Comment = request.Dto.Comment,
            };

            comment.SetAuditFieldsOnCreate(request.CreatedBy);
            await _unitofWork.GetRepository<InvoiceComment>().AddAsync(comment);

            var isSaved = await _unitofWork.SaveChanges(cancellationToken);

            if (!isSaved)
                return ResponseResult<bool>.BadRequest("Error Adding New Invoice  Comments");
            return ResponseResult<bool>.Created(MessageConstants.Message(MessageOperationType.Create, "invoice comment"));
        }
    }
}