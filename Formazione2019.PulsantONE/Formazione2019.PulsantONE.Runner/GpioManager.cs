using System;
using System.Threading;
using System.Device.Gpio;
using Formazione2019.PulsantONE.Services;
using Formazione2019.PulsantONE.Services.Classes;
using Formazione2019.PulsantONE.Services.Impl;

namespace Formazione2019.PulsantONE.Runner
{
    public class GpioManager
    {
        private readonly IHubService _hubService;
        private const int GroundPin = 3;
        private const int PushButtonGpioPin = 18;
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
            if(_hubService.Connection == null)
                throw new InvalidOperationException($"Can't connect to hub -_-");

            Console.WriteLine("Connection succeeded.");
        }

        public void Run()
        {
            using (var gpioController = new GpioController())
            { 

                //Set pin 10 to be an input pin and set initial value to be pulled low (off)
                Console.WriteLine($"Setting pin {PushButtonGpioPin} to input pull up mode");
                gpioController.OpenPin(PushButtonGpioPin, PinMode.InputPullUp);

                if(!gpioController.IsPinOpen(PushButtonGpioPin))
                    throw new InvalidOperationException($"Can't open pin {PushButtonGpioPin} in mode {PinMode.InputPullUp}");
                
                //Console.WriteLine($"Setting pin {PushButton3dot3VoltPin} initial value to Low");
                //gpioController.Write(PushButton3dot3VoltPin, PinValue.Low);

                //Console.WriteLine($"Listening pin {PushButton3dot3VoltPin} event type Rising");
                //gpioController.RegisterCallbackForPinValueChangedEvent(10, PinEventTypes.Rising,  (sender, eventArgs) => OnButtonPushed());

                var initialState = gpioController.Read(PushButtonGpioPin);
                Console.WriteLine($"Gpio pin {PushButtonGpioPin} initial state: {initialState}");

                while (true)
                {
                    var gpioValue = gpioController.Read(PushButtonGpioPin);

                    if(gpioValue == PinValue.High)
                        Console.WriteLine("Button pin value high!");
                    else
                        Console.WriteLine("Button pin value low!");

                    Thread.Sleep(50);
                }

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
            if (!_registered) 
            {
                Console.WriteLine($"Client is not registered -_-");
                return;
            }
            
            Console.WriteLine("Try moving space ship!");

            _hubService.SendMessage();

            Console.WriteLine("Space ship has not moved...");
        }
    }
}