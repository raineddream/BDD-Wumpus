using System.Linq;

namespace IndustrialLogic.WumpusLocation
{
    public class WumpusWorld
    {
        public const int MaxEdges  = 3;
        private const int MaxRooms = 20;

        private const int Wumpus = 1;
        private const int Pit1   = 2;
        private const int Pit2   = 3;
        private const int Bat1   = 4;
        private const int Bat2   = 5;

        private readonly int[] _neighboringRooms = { 2, 5, 8, 1, 3, 10, 2, 4, 12, 3, 5, 14, 1, 4, 6, 5, 7, 15, 6, 8, 17, 1, 7, 9, 8,
            10, 18, 2, 9, 11, 10, 12, 19, 3, 11, 13, 12, 14, 20, 4, 13, 15, 6, 14, 16, 15, 17, 20,
            7, 16, 18, 9, 17, 19, 11, 18, 20, 13, 16, 19 };

        private readonly int[,] _rooms = new int[MaxRooms + 1, MaxEdges];   // 1-based, room 0 not used
        private readonly int[] _locationOf = new int[6];
        private readonly int[] _savedActorLocations = new int[6];
        private readonly RandomNumber _randomNumber = new RandomNumber();

        public WumpusWorld(IGameReporter reporter)
        {
            Player = new Player(this, reporter) { Fate = PlayerFate.Unknown };
        }

        public void LoadMap()
        {
            int index = 0;
            for (int room = 1; room <= MaxRooms; room++)
            {
                for (int edge = 0; edge < MaxEdges; edge++)
                {
                    _rooms[room, edge] = _neighboringRooms[index++];
                }
            }
        }

        public Player Player { get; private set; }

        public int WumpusLocation
        {
            get { return _locationOf[Wumpus]; }
            set { _locationOf[Wumpus] = value; }
        }

        public int Pit1Location
        {
            get { return _locationOf[Pit1]; }
        }

        public int Pit2Location
        {
            get { return _locationOf[Pit2]; }
        }

        public int Bat1Location
        {
            get { return _locationOf[Bat1]; }
        }

        public int Bat2Location
        {
            get { return _locationOf[Bat2]; }
        }

        public int[] NeighborsOf(int location)
        {
            return new[] { RoomAt(location, 0), RoomAt(location, 1), RoomAt(location, 2) };
        }

        public void PutPlayerIn(int room)
        {
            Player.Location = room;
        }

        public int RoomAt(int room, int edge)
        {
            return _rooms[room, edge];
        }

        public bool WumpusIsDead()
        {
            return Player.DoesWin();
        }

        public void SetupNew()
        {
            bool someActorsHaveSameRoom = true;
            while (someActorsHaveSameRoom)
            {
                for (int actor = 0; actor < _locationOf.ToArray().Length; actor++)
                {
                    _locationOf[actor] = _randomNumber.Random1toN(MaxRooms);
                    _savedActorLocations[actor] = _locationOf[actor];
                }

                someActorsHaveSameRoom = false;
                for (int actor1 = 0; actor1 < _locationOf.ToArray().Length; actor1++)
                {
                    for (int actor2 = actor1 + 1; actor2 < _locationOf.ToArray().Length; actor2++)
                    {
                        if (_locationOf[actor1] == _locationOf[actor2])
                            someActorsHaveSameRoom = true;
                    }
                }
            }
        }

        public void Restore()
        {
            for (int actor = 0; actor < _locationOf.ToArray().Length; actor++)
            {
                _locationOf[actor] = _savedActorLocations[actor];
            }
        }

        public int AnyRoomInWorld()
        {
            return _randomNumber.Random1toN(MaxRooms);
        }

        public bool IsRoomInWorld(int room)
        {
            return room >= 1 && room <= MaxRooms;
        }

        public int LocationOf(int actor)
        {
            return _locationOf[actor];
        }

        public void PutActorInRoom(int actor, int room)
        {
            _locationOf[actor] = room;
        }

        public int AnyPlayerNeighbor()
        {
            return Player.Neighbors[_randomNumber.Random0uptoN(3)];
        }
    }
}