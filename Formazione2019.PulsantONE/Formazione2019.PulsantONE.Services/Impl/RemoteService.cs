using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Formazione2019.PulsantONE.Services.Impl
{
    public class RemoteService: IRemoteService
    {
        private const string FunctionBaseAddress = "";
        private const string FunctionEndPoint = "movespaceship";
        
        public async Task<Tuple<string, string>> MoveSpaceShip()
        {
            using (var httpClient = new HttpClient())
            {
                var baseAddress = new Uri(FunctionBaseAddress);
                httpClient.BaseAddress = baseAddress;
                var response = await httpClient.GetAsync(FunctionEndPoint);
                return Tuple.Create(response.StatusCode.ToString(), response.ReasonPhrase);
            }
        }
    }
}