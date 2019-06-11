using System;
using System.Threading.Tasks;
using Formazione2019.PulsantONE.Services.Classes;
using Microsoft.AspNetCore.SignalR.Client;

namespace Formazione2019.PulsantONE.Services
{
    public interface IHubService
    {
        /// <summary>
        /// Hub
        /// </summary>
        HubConnection Connection { get; }

        Task Connect();
        Task Register();

        Task SendMessage();    
        

        event EventHandler<GameState> OnGameStateReceived;
        event EventHandler<bool> OnRegisterResult;
        event EventHandler OnConnectionLost;
    }
}