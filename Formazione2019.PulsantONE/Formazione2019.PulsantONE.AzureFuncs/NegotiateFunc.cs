using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;

namespace Formazione2019.PulsantONE.AzureFuncs
{
    public static class NegotiateFunc
    {
        private const string HubName = "Marzonzio";

        [FunctionName("Negotiate")]
        public static SignalRConnectionInfo Negotiate
        (
            [HttpTrigger(AuthorizationLevel.Anonymous)]HttpRequest req,
            [SignalRConnectionInfo(HubName = HubName)] SignalRConnectionInfo connectionInfo,
            ILogger log
        )
        {
            log.LogInformation("C# HTTP trigger function processed a negotiate request.");
            return connectionInfo;
        }
    }
}
