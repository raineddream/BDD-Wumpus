﻿namespace IndustrialLogic.WumpusLocation
{
    public class WumpusWorld
    {
        public const int MAX_ROOMS = 20;
        public const int MAX_EDGES = 3;

        private static readonly int[] neighboringRooms = { 2, 5, 8, 1, 3, 10, 2, 4, 12, 3, 5, 14, 1, 4, 6, 5, 7, 15, 6, 8, 17, 1, 7, 9, 8,
            10, 18, 2, 9, 11, 10, 12, 19, 3, 11, 13, 12, 14, 20, 4, 13, 15, 6, 14, 16, 15, 17, 20,
            7, 16, 18, 9, 17, 19, 11, 18, 20, 13, 16, 19 };

        private int[,] _rooms = new int[MAX_ROOMS + 1, MAX_EDGES];   // 1-based, room 0 not used

        public void LoadMap()
        {
            int index = 0;
            for (int room = 1; room <= MAX_ROOMS; room++)
            {
                for (int edge = 0; edge < MAX_EDGES; edge++)
                {
                    _rooms[room, edge] = neighboringRooms[index++];
                }
            }
        }

        public int[,] Rooms
        {
            get { return _rooms; }
        }
    }
}