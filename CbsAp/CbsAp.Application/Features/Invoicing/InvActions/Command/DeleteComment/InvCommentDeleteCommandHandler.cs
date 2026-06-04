using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;



namespace CbsAp.Application.Features.Invoicing.InvActions.Command.DeleteComment
{
    public class InvCommentDeleteCommandHandler : ICommandHandler<InvCommentDeleteCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;



        public InvCommentDeleteCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }



        public async Task<ResponseResult<bool>> Handle(InvCommentDeleteCommand request, CancellationToken cancellationToken)
        {
            var invRepo = _unitofWork.GetRepository<InvoiceComment>();



            await _unitofWork.GetRepository<InvoiceComment>().DeleteAsync(new InvoiceComment
            {
                InvoiceCommentID = request.Dto.InvoiceCommentID,
                Comment = request.Dto.Comment
            });


              

                var isSaved = await _unitofWork.SaveChangesAsync(cancellationToken);



                if (!isSaved)
                    return ResponseResult<bool>.BadRequest("Error Deleting Comments");
            
            
            

            return ResponseResult<bool>.Created(MessageConstants.Message(MessageOperationType.Delete, "invoice comment"));
        }
    }
}