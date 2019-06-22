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
        private PinValue _lastGpioValue;

        public GpioManager()
        {
            _hubService = new HubService();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            _hubService.OnGameStateReceived += (sender, state) =>
            {
                switch (state)
                {
                    case GameState.Register:
                        Console.WriteLine("Hub has opened registrations!");
                        _hubService.Register();
                        break;
                    case GameState.InRun:
                        Console.WriteLine("Hub has started the game!");
                        _isInRun = true;
                        break;
                    case GameState.Closed:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Connection was closed!");
                        Console.ResetColor();
                        break;
                    case GameState.Finished:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Raspberry wins!");
                        Console.ResetColor();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(state), state, null);
                }
            };

            _hubService.OnRegisterResult += (sender, registered) =>
            {
                Console.WriteLine("Client has been registered!");
                if (registered)
                    _registered = true;
            };

            _hubService.OnConnectionLost += (sender, args) =>
            {
                Console.WriteLine("Hub connection has been closed");
                _isInRun = false;
                _registered = false;
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
                _lastGpioValue = PinValue.High;

                //Set pin 10 to be an input pin and set initial value to be pulled up (off)
                Console.WriteLine($"Setting pin {PushButtonGpioPin} to input pull up mode");
                gpioController.OpenPin(PushButtonGpioPin, PinMode.InputPullUp);

                if(!gpioController.IsPinOpen(PushButtonGpioPin))
                    throw new InvalidOperationException($"Can't open pin {PushButtonGpioPin} in mode {PinMode.InputPullUp}");
                
                //Console.WriteLine($"Listening pin {PushButton3dot3VoltPin} event type Rising");
//                gpioController.RegisterCallbackForPinValueChangedEvent(PushButtonGpioPin, PinEventTypes.Falling,  (sender, eventArgs) => OnButtonPushed());

                var initialState = gpioController.Read(PushButtonGpioPin);
                Console.WriteLine($"Gpio pin {PushButtonGpioPin} initial state: {initialState}");

                while (true)
                {
                    DebounceButton(gpioController);
                    Thread.Sleep(50);
                }
            }
        }

        private void DebounceButton(GpioController gpioController)
        {
            var gpioValue = gpioController.Read(PushButtonGpioPin);

            if (gpioValue == PinValue.High && _lastGpioValue == PinValue.High)
                return;
            if (gpioValue == PinValue.Low && _lastGpioValue == PinValue.High)
            {
                Console.WriteLine("Button pushed!");
                OnButtonPushed();
            }

            if (gpioValue == PinValue.Low && _lastGpioValue == PinValue.Low)
                return;
            if (gpioValue == PinValue.High && _lastGpioValue == PinValue.Low)
                Console.WriteLine("Button released!");
            _lastGpioValue = gpioValue;
        }

        private void OnButtonPushed()
        {
            if (!_registered)
            {
                Console.WriteLine("Client is not registered -_-");
                return;
            }

            if (_registered && !_isInRun)
            {
                Console.WriteLine("Game no started yet...");
                return;
            }
        
            Console.WriteLine("Try moving space ship!");

            _hubService.SendMessage();
            Console.WriteLine("Message hopefully sent!");

        }
      
    }
}