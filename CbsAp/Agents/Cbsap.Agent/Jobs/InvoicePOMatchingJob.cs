using CbsAp.Application.Features.AutoMatching;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;


namespace Cbsap.Agent.Jobs
{
    public class InvoicePOMatchingJob (ILogger<InvoicePOMatchingJob> logger,ISender mediator) : IJob
    {  
        public async Task Execute(IJobExecutionContext context)
        {
            logger.LogInformation($"[Job Executed] {DateTime.Now}");
            var command = new MatchInvoicePOCommand();
            await mediator.Send(command);
            logger.LogInformation($"[Job Finished] {DateTime.Now}");
        }
    }
}
