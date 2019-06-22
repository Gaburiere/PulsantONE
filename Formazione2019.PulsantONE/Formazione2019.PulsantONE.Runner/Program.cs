using System;

namespace Formazione2019.PulsantONE.Runner
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine($"Attempt of {DateTime.Now}");
                var gpioManager = new GpioManager();
                gpioManager.InitHub();
                gpioManager.Run();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.ResetColor();
            }
        }
    }
}