using System;

namespace Formazione2019.PulsantONE.Runner
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var gpioManager = new GpioManager();

                gpioManager.InitHub();
                gpioManager.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}