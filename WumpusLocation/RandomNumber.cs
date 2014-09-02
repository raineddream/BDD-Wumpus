using System;

namespace IndustrialLogic.WumpusLocation
{
    public class RandomNumber
    {
        private static Random _rand = new Random();

        public int Random1toN(int n)
        {
            int random = _rand.Next(1, n);
            return random;
        }

        public int Random0uptoN(int n)
        {
            int random = _rand.Next(0, n - 1);
            return random;
        }
    }
}
