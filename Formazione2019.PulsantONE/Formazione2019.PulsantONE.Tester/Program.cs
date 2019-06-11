using System;
using System.Threading;
using System.Threading.Tasks;
using Formazione2019.PulsantONE.Services;
using Formazione2019.PulsantONE.Services.Classes;
using Formazione2019.PulsantONE.Services.Impl;

namespace Formazione2019.PulsantONE.Tester
{
    class Program
    {
        private static bool _isInRun;
        private static bool _registered;
        private static bool _closed;

        static async Task Main(string[] args)
        {
            
            var hubService = new HubService();
            RegisterEvents(hubService);
            
            await hubService.Connect();

            while (!_closed)
            {
                Thread.Sleep(5000);

                if (_isInRun)
                {
                    Console.WriteLine("SPARO");
                    await hubService.SendMessage();
                    continue;
                }
                Console.WriteLine("sto aspettando...");
            }
        }

        private static void RegisterEvents(IHubService hubService)
        {
            hubService.OnGameStateReceived += async (sender, state) =>
            {
                switch (state)
                {
                    case GameState.Register:
                        Console.WriteLine("Hub has opened registrations!");
                        await hubService.Register();
                        Console.WriteLine("Registered!");
                        break;
                    case GameState.InRun:
                        Console.WriteLine("Hub has started the game!");
                        _isInRun = true;
                        break;
                    case GameState.Closed:
                        _closed = true;
                        break;
                    case GameState.Finished:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(state), state, null);
                }
            };
            
            hubService.OnRegisterResult += (sender, registered) =>
            {
                Console.WriteLine("Client has been registered!");
                if (registered)
                    _registered = true;
            };

            hubService.OnConnectionLost += (sender, args) =>
            {
                Console.WriteLine("Hub connection has been closed");
                _isInRun = false;
                _registered = false;
            };
        }
    }
}