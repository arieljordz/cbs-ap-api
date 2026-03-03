using CbsAp.Application.Features.Locking.Command;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace CbsAp.API.SignalRHub
{
    public class LockHub:Hub
    {

        private readonly ISender _mediator;
        public LockHub(ISender mediator)
        {
            _mediator = mediator;
        }


        public async Task RequestLock(long recordId,string lockedBy)
        {
            var userName = Context.User?.Identity?.Name;
            await Clients.Users(lockedBy).SendAsync("requestLock", recordId, userName);
        }

        public async Task RequestLockResponse(long recordId,string requestedBy,bool accepted)
        {
            var userName = requestedBy ?? "";
            if (accepted)
            {
                var command = new AcquireLockCommand(userName, recordId, true);
                var result = await _mediator.Send(command);
            }

            await Clients.Users(userName).SendAsync("requestLockResponse", recordId,accepted);
        }

        public async Task ForceLockRecord(long recordId,string lockedBy)
        {
            var userName = Context.User?.Identity?.Name;
            var command = new AcquireLockCommand(userName ?? "", recordId, true);
            var result = await _mediator.Send(command);
            
            await Clients.Users(lockedBy).SendAsync("recordLocked", recordId, userName);                        
        }
        public async Task LockRecord(long recordId)
        {
            var userName = Context.User?.Identity?.Name;

            var command = new AcquireLockCommand(userName??"", recordId, false);
            var result = await _mediator.Send(command);

            if (!string.IsNullOrEmpty(result.ResponseData))
            {
                //currently locked by a user
                var lockedBy = result.ResponseData;
                await Clients.Caller.SendAsync("lockFailed", recordId, lockedBy);                
            }
           
        }

        public async Task UnlockRecord(long recordId)
        { 
            var userName = Context.User?.Identity?.Name;
            var command = new ReleaseLockCommand(userName ?? "", recordId);
            var result = await _mediator.Send(command);
            await Clients.Others.SendAsync("recordUnlocked", recordId);
        }
    }
}
