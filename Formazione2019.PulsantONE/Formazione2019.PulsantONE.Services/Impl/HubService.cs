using System;
using System.Threading.Tasks;
using Formazione2019.PulsantONE.Services.Classes;
using Microsoft.AspNetCore.SignalR.Client;

namespace Formazione2019.PulsantONE.Services.Impl
{
    public class HubService : IHubService
    {
        public HubConnection Connection { get; private set; }

        public async Task Connect()
        {
            Connection = new HubConnectionBuilder().WithUrl("https://ad-rome-admin.azurewebsites.net/play")
                .Build();
            
            Connection.On("gameStateMode",
                new Action<GameState>(gameState =>
                {
                    Console.WriteLine("Invoking OnGameStateReceived");
                    OnGameStateReceived?.Invoke(this, gameState);
                }));

            Connection.On("registerResult",
                new Action<bool>(registered =>
                {
                    Console.WriteLine("Invoking OnRegisterResult");
                    OnRegisterResult?.Invoke(this, registered);
                }));
            
            Connection.Closed += exception => Task.Run(() => OnConnectionLost?.Invoke(this,null));  //.OnClose(error => this.OnConnectionLost?.Invoke(this,null));

            Console.WriteLine("Trying starting connection...");
            await Connection.StartAsync();
        }

        public async Task Register()
        { 
            await Connection.SendAsync("register","Bariere's RaspberryPI 2+",Guid.Parse("0D2C37F7-49FE-48D9-A1D3-1A90E7948BCC"));
        }

        public async Task SendMessage()
        {
            await Connection.SendAsync("tap").ConfigureAwait(false);
        }

        public event EventHandler<GameState> OnGameStateReceived;
        public event EventHandler<bool> OnRegisterResult;
        public event EventHandler OnConnectionLost;

    }
}