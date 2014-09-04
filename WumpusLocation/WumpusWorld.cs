using System.Linq;

namespace IndustrialLogic.WumpusLocation
{
    public class WumpusWorld
    {
        public const int MAX_ROOMS = 20;
        public const int MAX_EDGES = 3;

        private const int PLAYER = 0;
        private const int WUMPUS = 1;
        private const int PIT1   = 2;
        private const int PIT2   = 3;
        private const int BAT1  = 4;
        private const int BAT2  = 5;

        private static readonly int[] neighboringRooms = { 2, 5, 8, 1, 3, 10, 2, 4, 12, 3, 5, 14, 1, 4, 6, 5, 7, 15, 6, 8, 17, 1, 7, 9, 8,
            10, 18, 2, 9, 11, 10, 12, 19, 3, 11, 13, 12, 14, 20, 4, 13, 15, 6, 14, 16, 15, 17, 20,
            7, 16, 18, 9, 17, 19, 11, 18, 20, 13, 16, 19 };

        private int[,] _rooms = new int[MAX_ROOMS + 1, MAX_EDGES];   // 1-based, room 0 not used
        private RandomNumber _randomNumber = new RandomNumber();
        private int _playerLocation;

        public int[] locationOf = new int[6];
        public int[] savedActorLocations = new int[6];

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

        public int PlayerLocation
        {
            get { return _playerLocation; }
        }

        public int WumpusLocation
        {
            get { return locationOf[WUMPUS]; }
            set { locationOf[WUMPUS] = value; }
        }

        public int Pit1Location
        {
            get { return locationOf[PIT1]; }
            set { locationOf[PIT1] = value; }
        }
        
        public int Pit2Location
        {
            get { return locationOf[PIT2]; }
            set { locationOf[PIT2] = value; }
        }

        public int Bat1Location
        {
            get { return locationOf[BAT1]; }
            set { locationOf[BAT2] = value; }
        }

        public int Bat2Location
        {
            get { return locationOf[BAT2]; }
            set { locationOf[BAT2] = value; }
        }

        public int[] Neighbors {
            get { return new[] { Rooms[PlayerLocation, 0], Rooms[PlayerLocation, 1], Rooms[PlayerLocation, 2] }; }
        }

        public int RoomAt(int room, int edge)
        {
            return Rooms[room, edge];
        }

        public void PutPlayerIn(int room)
        {
            _playerLocation = room;
        }

        public void PlayerShootsInto(int room)
        {
            
        }

        public bool WumpusIsDead()
        {
            return false;
        }

        public bool DoesPlayWin()
        {
            return false;
        }

        public void SetupNew()
        {
            bool someActorsHaveSameRoom = true;
            while (someActorsHaveSameRoom)
            {
                for (int actor = 0; actor < locationOf.ToArray().Length; actor++)
                {
                    locationOf[actor] = _randomNumber.Random1toN(MAX_ROOMS);
                    savedActorLocations[actor] = locationOf[actor];
                }

                someActorsHaveSameRoom = false;
                for (int actor1 = 0; actor1 < locationOf.ToArray().Length; actor1++)
                {
                    for (int actor2 = actor1 + 1; actor2 < locationOf.ToArray().Length; actor2++)
                    {
                        if (locationOf[actor1] == locationOf[actor2])
                            someActorsHaveSameRoom = true;
                    }
                }
            }
        }

        public void Restore()
        {
            for (int actor = 0; actor < locationOf.ToArray().Length; actor++)
            {
                locationOf[actor] = savedActorLocations[actor];
            }
        }
    }
}