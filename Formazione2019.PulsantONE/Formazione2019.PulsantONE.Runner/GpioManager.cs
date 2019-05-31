using System;
using System.Device.Gpio;
using Formazione2019.PulsantONE.Services;
using Formazione2019.PulsantONE.Services.Classes;
using Formazione2019.PulsantONE.Services.Impl;

namespace Formazione2019.PulsantONE.Runner
{
    public class GpioManager
    {
        private readonly IHubService _hubService;
        private const int PushButton33VoltPin = 10;
        private const int PushButtonGpioPin = 3;
        private bool _registered;
        private bool _isInRun;

        public GpioManager()
        {
            _hubService = new HubService();
            
            _hubService.OnGameStateReceived += (sender, state) =>
            {
                switch (state)
                {
                    case GameState.Register:
                        _hubService.Register();
                        break;
                    case GameState.InRun:
                        _isInRun = true;
                        break;
                    case GameState.Closed:
                        break;
                    case GameState.Finished:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(state), state, null);
                }
            };

            _hubService.OnRegisterResult += (sender, registered) =>
            {
                if (registered)
                    _registered = true;
            };
        }

        public void InitHub()
        {
            Console.WriteLine("Connecting to hub...");
            _hubService.Connect();
            Console.WriteLine("Connection succeeded.");
        }

        public void Run()
        {
            using (var gpioController = new GpioController())
            { 
                //Set pin 10 to be an input pin and set initial value to be pulled low (off)
                Console.WriteLine($"Setting pin {PushButton33VoltPin} to input");
                gpioController.SetPinMode(10, PinMode.Input);
                
                Console.WriteLine($"Setting pin {PushButton33VoltPin} initial value to Low");
                gpioController.Write(10, PinValue.Low);

                Console.WriteLine($"Listening pin {PushButton33VoltPin} event type Rising");
                gpioController.RegisterCallbackForPinValueChangedEvent(10, PinEventTypes.Rising,  (sender, eventArgs) => OnButtonPushed());

                Console.WriteLine("Push enter to quit.");

                var enterPushed = false;
                while (!enterPushed)
                {
                    var read = Console.ReadKey(true);
                    if (read.Key == ConsoleKey.Enter)
                        enterPushed = true;
                }
            }
        }

        private void OnButtonPushed()
        {
            if (!_registered || !_isInRun) return;
            
            Console.WriteLine("Try moving space ship!");

            _hubService.SendMessage();

            Console.WriteLine("Space ship has not moved...");

        }
    }
}