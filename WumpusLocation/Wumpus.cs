// ***************************************************************************
// Copyright (c) 2013, Industrial Logic, Inc., All Rights Reserved.
//
// This code is the exclusive property of Industrial Logic, Inc. It may ONLY be
// used by students during Industrial Logic's workshops or by individuals
// who are being coached by Industrial Logic on a project.
//
// This code may NOT be copied or used for any other purpose without the prior
// written consent of Industrial Logic, Inc.
// ****************************************************************************

using System;
using IndustrialLogic.WumpusLocation;

namespace Wumpus
{
    public class Wumpus
    {
// Inspired by Wumpus as designed by Gregory Yob in the 1970s. 

        private const int MAX_TARGETS = 5;

        private const int STARTING_ARROWS = 5;

        public enum Action
        {
            Shoot,
            Move
        };

        private int arrows;

        private WumpusWorld _world;
        private RandomNumber _randomNumber = new RandomNumber();

        public Wumpus()
        {
            _world = new WumpusWorld(new ConsoleReporter());
        }

        public void doit()
        {
            _world.Reporter.Report("Hunt the Wumpus");

            _world.Reporter.Report("instructions? [y/n]");
            String wantsInstructions = input();
            if (!(wantsInstructions.Equals("n")))
                instructions();

            _world.LoadMap();
            _world.SetupNew();

            while (true)
            {
                arrows = STARTING_ARROWS;
                _world.Player.Fate = PlayerFate.Unknown;

                while (_world.Player.Fate == PlayerFate.Unknown)
                {
                    _world.SituationalAwareness();

                    Action action = chooseOption();

                    if (action == Action.Shoot)
                    {
                        shoot();
                    }
                    else if (action == Action.Move)
                    {
                        move();
                    }
                }

                if (_world.Player.Fate == PlayerFate.Wins)
                {
                    _world.Reporter.Report("You win");
                }
                else
                {
                    _world.Reporter.Report("You lose");
                }

                _world.Reporter.Report("Same setup? [y/n]");
                String wantsSameSetup = input();

                if (wantsSameSetup.Equals("y"))
                    _world.Restore();
                else
                    _world.SetupNew();
            }
        }

        private void move()
        {
            _world.Player.Fate = PlayerFate.Unknown;
            _world.Reporter.Report("Where to?");

            bool validRoom = false;
            int playerMoveToRoom = -1;
            do
            {
                do
                {
                    playerMoveToRoom = input_number();
                } while (!_world.IsRoomInWorld(playerMoveToRoom));

                if (_world.Player.IsNeighborRoom(playerMoveToRoom))
                {
                    validRoom = true;
                }
                else if (_world.Player.Location == playerMoveToRoom)
                {
                    validRoom = true;
                }
                else
                {
                    _world.Reporter.Report("not possible");
                }
            } while (!validRoom);

            bool stillSettling = true;
            while (stillSettling)
            {
                stillSettling = false;

                _world.PutPlayerIn(playerMoveToRoom);

                if (_world.Player.Location == _world.WumpusLocation)
                {
                    _world.Reporter.Report("bumped wumpus");
                    moveWumpus();
                    if (_world.Player.Fate != PlayerFate.Unknown)
                    {
                        return;
                    }
                }

                if (_world.Player.Location == _world.Pit1Location || _world.Player.Location == _world.Pit2Location)
                {
                    _world.Reporter.Report("fell in pit");
                    _world.Player.Fate = PlayerFate.Loses;
                    return;
                }
                else if (_world.Player.Location == _world.Bat1Location || _world.Player.Location == _world.Bat2Location)
                {
                    _world.PutPlayerIn(_world.AnyRoomInWorld());
                    stillSettling = true;
                }
                else
                {
                    return;
                }
            }
        }

        private Action chooseOption()
        {
            String moveType;
            _world.Reporter.Report("Shoot or move? [s/m]");
            do
            {
                moveType = input();
            } while (!(moveType.Equals("s") || moveType.Equals("m")));

            if (moveType.Equals("s"))
            {
                return Action.Shoot;
            }
            else if (moveType.Equals("m"))
            {
                return Action.Move;
            }
            return Action.Move; // can't happen
        }

        private void shoot()
        {
            int roomsToShoot;
            _world.Player.Fate = PlayerFate.Unknown;
            _world.Reporter.Report("# of rooms? [1-" + MAX_TARGETS + "]");
            do
            {
                roomsToShoot = input_number();
            } while (!_world.IsRoomInWorld(roomsToShoot));

            int[] targets = new int[roomsToShoot];
            for (int target = 0; target < targets.Length; target++)
            {
                while (true)
                {
                    _world.Reporter.Report("room #");
                    targets[target] = input_number();
                    if (target <= 2 || targets[target] != targets[target - 2])
                        break;
                    _world.Reporter.Report("Arrows aren't that crooked - try again");
                }
            }

            bool hitted = _world.Player.ShootIntoRooms(targets);
            if (hitted)
            {
                return;
            }

            _world.Reporter.Report("missed");
            moveWumpus();

            arrows = arrows - 1;
            if (arrows <= 0)
            {
                _world.Player.Fate = PlayerFate.Loses;
            }
        }

        private void instructions()
        {
            _world.Reporter.Report("You're an explorer in a set of caves, trying to kill the deadly wumpus before it kills you.");
            _world.Reporter.Report("There are 20 rooms, and each connects to 3 others as corners of a dodecahedron.");
            _world.Reporter.Report("You get 5 arrows that can shoot across multiple rooms. If you shoot the wumpus, you win; if you run out of arrows, you die.");
            _world.Reporter.Report("If you enter the room with the wumpus or shoot an arrow, the wumpus randomly moves to a new room or stays in the old one.");
            _world.Reporter.Report("If you're in the same room as the wumpus after its move, it eats you (and you lose).");
            _world.Reporter.Report("There are other hazards: (a) bottomless pits - kill you; (b) bats - carry you to a random room.");
            _world.Reporter.Report("Good luck!");
        }

        public void moveWumpus()
        {
            int newWumpusLocation = _randomNumber.Random0uptoN(4);
            if (newWumpusLocation < 3)
            {
                _world.WumpusLocation = _world.RoomAt(_world.WumpusLocation, newWumpusLocation);
            }
            if (_world.WumpusLocation == _world.Player.Location)
            {
                _world.Reporter.Report("wumpus got you");
                _world.Player.Fate = PlayerFate.Loses;
            }
        }

        public String input()
        {
            return Console.In.ReadLine();
        }

        public int input_number()
        {
            String numberString = input();
            try
            {
                return Convert.ToInt32(numberString);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}
