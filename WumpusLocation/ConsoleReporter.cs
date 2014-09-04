using System;

namespace IndustrialLogic.WumpusLocation
{
    public class ConsoleReporter : IGameReporter
    {
        public void Report(string message)
        {
            Console.WriteLine(message);
        }
    }
}