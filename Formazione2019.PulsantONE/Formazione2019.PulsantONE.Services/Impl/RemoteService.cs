using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Formazione2019.PulsantONE.Services.Impl
{
    public class RemoteService: IRemoteService
    {
        private const string FunctionBaseAddress = "";
        private const string SendMessageEndPoint = "movespaceship";
        private const string RegisterEndPoint = "register";
        private const string NegotiateEndPoint = "negotiate";
        
        public async Task<bool> SendMessage()
        {
            using (var httpClient = new HttpClient())
            {
                var baseAddress = new Uri(FunctionBaseAddress);
                httpClient.BaseAddress = baseAddress;
                var response = await httpClient.GetAsync(SendMessageEndPoint);
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<object> Negotiate()
        {
            using (var httpClient = new HttpClient())
            {
                var baseAddress = new Uri(FunctionBaseAddress);
                httpClient.BaseAddress = baseAddress;
                var response = await httpClient.GetAsync(NegotiateEndPoint);
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> Register()
        {
            using (var httpClient = new HttpClient())
            {
                var baseAddress = new Uri(FunctionBaseAddress);
                httpClient.BaseAddress = baseAddress;
                var response = await httpClient.GetAsync(RegisterEndPoint);
                return response.IsSuccessStatusCode;
            }
        }

    }
}