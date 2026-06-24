using CbsAp.Application.Features.AutoMatching;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbsap.Agent.Jobs
{
    public class POGrLineMatchingJob(ILogger<POGrLineMatchingJob> logger, ISender mediator) : IJob
    {  
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                logger.LogInformation($"[Job Executed] {DateTime.Now}");
                var command = new MatchInvoicePOCommand();
                await mediator.Send(command);
                logger.LogInformation($"[Job Finished] {DateTime.Now}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}
