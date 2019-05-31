using System;
using System.Net.Http;
using System.Threading.Tasks;
using Formazione2019.PulsantONE.Services.Classes;
using Microsoft.AspNetCore.SignalR.Client;

namespace Formazione2019.PulsantONE.Services.Impl
{
    public class HubService : IHubService
    {
        public HubConnection Connection { get; private set; }

        public void Connect()
        {
            this.Connection = new HubConnectionBuilder().WithUrl("https://ad-rome-admin.azurewebsites.net/play")
                .Build();
            
            this.Connection.On("gameStateMode",
                new Action<GameState>((gameState) => { this.OnGameStateReceived?.Invoke(this, gameState); }));

            this.Connection.On("registerResult",
                new Action<bool>((registered) => { this.OnRegisterResult?.Invoke(this, registered); }));
        }

        public void Register()
        { 
            this.Connection.SendAsync("register","Bariere's RaspberryPI 2+","Space-X").ConfigureAwait(false);
        }

        public void SendMessage()
        {
            this.Connection.SendAsync("tap").ConfigureAwait(false);
        }

        public event EventHandler<GameState> OnGameStateReceived;
        public event EventHandler<bool> OnRegisterResult;
    }
}