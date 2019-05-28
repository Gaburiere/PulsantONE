using System;
using System.Device.Gpio;
using System.Threading.Tasks;
using Formazione2019.PulsantONE.Services;
using Formazione2019.PulsantONE.Services.Impl;

namespace Formazione2019.PulsantONE.Runner
{
    public class GpioManager
    {
        private readonly IRemoteService _remoteService;
        private const int PushButton33VoltPin = 10;
        private const int PushButtonGpioPin = 3;
        private int _httpCount;

        public GpioManager()
        {
            _remoteService = new RemoteService();
            _httpCount = 0;
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
                gpioController.RegisterCallbackForPinValueChangedEvent(10, PinEventTypes.Rising, async (sender, eventArgs) => await OnButtonPushed());

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

        private async Task OnButtonPushed()
        {
            Console.WriteLine("Try moving space ship");
            var (statusCode, reasonPhrase) = await _remoteService.MoveSpaceShip();
            _httpCount++;
            Console.WriteLine($"Status code: {statusCode} | Reason phrase: {reasonPhrase}");
        }
    }
}