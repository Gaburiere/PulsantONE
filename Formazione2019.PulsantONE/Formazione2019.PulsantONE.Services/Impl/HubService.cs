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
                new Action<GameState>(gameState => { OnGameStateReceived?.Invoke(this, gameState); }));

            Connection.On("registerResult",
                new Action<bool>(registered => { OnRegisterResult?.Invoke(this, registered); }));
            
            Connection.Closed += exception => Task.Run(() => OnConnectionLost?.Invoke(this,null));  //.OnClose(error => this.OnConnectionLost?.Invoke(this,null));

            await Connection.StartAsync();
        }

        public void Register()
        { 
            Connection.SendAsync("register","Bariere's RaspberryPI 2+","Space-X").ConfigureAwait(false);
        }

        public void SendMessage()
        {
            Connection.SendAsync("tap").ConfigureAwait(false);
        }

        public event EventHandler<GameState> OnGameStateReceived;
        public event EventHandler<bool> OnRegisterResult;
        public event EventHandler OnConnectionLost;

    }
}