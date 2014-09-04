using System;

namespace IndustrialLogic.WumpusLocation
{
    public class ConsoleReporter : IGameReporter
    {
        public void report(string message)
        {
            Console.WriteLine(message);
        }
    }
}