using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;

namespace Formazione2019.PulsantONE.AzureFuncs
{
    public static class SendMessageFunc
    {
        private const string HubName = "Marzonzio";
        
        [FunctionName("SendMessage")]
        public static async Task<bool> SendMessage(
            [HttpTrigger(AuthorizationLevel.Anonymous)]object message, 
            [SignalR(HubName = HubName)]IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a send message request.");

            try
            {
               //todo call signalr hub
               await signalRMessages.AddAsync(
                   new SignalRMessage 
                   {
                       Target = "sendMessage", 
                       Arguments = new [] { message } 
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
