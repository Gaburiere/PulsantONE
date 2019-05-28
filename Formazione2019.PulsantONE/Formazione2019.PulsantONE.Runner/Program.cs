namespace Formazione2019.PulsantONE.Runner
{
    public class Program
    {
        static void Main(string[] args)
        {
            var gpioManager = new GpioManager();
            gpioManager.Run();
        }
    }
}