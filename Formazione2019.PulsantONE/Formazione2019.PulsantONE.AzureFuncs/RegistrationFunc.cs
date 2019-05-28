using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Formazione2019.PulsantONE.AzureFuncs
{
    public static class RegistrationFunc
    {
        private const string HubName = "Marzonzio";

        [FunctionName("RegistrationFunc")]
        public static async Task<bool> Run
        (
            [HttpTrigger(AuthorizationLevel.Anonymous)]object message,
            [SignalR(HubName = HubName)]IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a registration request.");
            try
            {
                //todo call signalr hub
                await signalRMessages.AddAsync(
                    new SignalRMessage
                    {
                        Target = "registration",
                        Arguments = new[] { message }
                    });
                return true;
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                log.LogError(e.StackTrace);
                return false;
            }
        }
    }
}
