using System.Linq;

namespace IndustrialLogic.WumpusLocation
{
    public class Player
    {
        private const int ActorId = 0;

        private readonly WumpusWorld _inWorld;
        private readonly IGameReporter _reporter;

        public Player(WumpusWorld inWorld, IGameReporter reporter)
        {
            _inWorld = inWorld;
            _reporter = reporter;
        }

        public int Location {
            get
            {
                return _inWorld.LocationOf(ActorId);
            }
            set
            {
                _inWorld.PutActorInRoom(ActorId, value);
            }
        }


        public int[] Neighbors
        {
            get { return _inWorld.NeighborsOf(Location); }
        }

        public PlayerFate Fate { get; set; }

        public bool DoesWin()
        {
            return Fate == PlayerFate.Wins;
        }

        public bool IsNeighborRoom(int room)
        {
            return Neighbors.Any(neighbor => neighbor == room);
        }

        public bool ShootIntoRooms(params int[] targets)
        {
            int arrowLocation = Location;
            foreach (int target in targets)
            {
                bool targetFound = false;
                for (int edge = 0; edge < WumpusWorld.MaxEdges; edge++)
                {
                    if (_inWorld.RoomAt(arrowLocation, edge) == target)
                    {
                        targetFound = true;
                        arrowLocation = target;
                        if (arrowLocation == _inWorld.WumpusLocation)
                        {
                            _reporter.Report("You got the wumpus");
                            Fate = PlayerFate.Wins;
                            return true;
                        }
                    }
                }

                if (!targetFound)
                {
                    arrowLocation = _inWorld.AnyPlayerNeighbor();
                }

                if (arrowLocation == _inWorld.WumpusLocation)
                {
                    _reporter.Report("You got the wumpus");
                    Fate = PlayerFate.Wins;
                    return true;
                }
                if (arrowLocation == Location)
                {
                    _reporter.Report("Ouch - arrow got you");
                    Fate = PlayerFate.Loses;
                    return true;
                }
            }

            return false;
        }
    }
}