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
using System.Linq;
using IndustrialLogic.WumpusLocation;

namespace Wumpus
{
    public class Wumpus
    {
// Inspired by Wumpus as designed by Gregory Yob in the 1970s. 
        private Random rand = new Random();
        private const int MAX_TARGETS = 5;
        private const int STARTING_ARROWS = 5;

        public enum Fate
        {
            Unknown,
            PlayerWins,
            PlayerLoses
        };

        public enum Action
        {
            Shoot,
            Move
        };

        private int arrows;
        private int playerLocation;

        private Fate fate;

        private int[] targets = new int[MAX_TARGETS];


        private int[] locationOf = new int[6];
        private int[] savedActorLocations = new int[6];
        private WumpusWorld _world;

        private const int player = 0;
        private const int wumpus = 1;
        private const int pit1 = 2;
        private const int pit2 = 3;
        private const int bats1 = 4;
        private const int bats2 = 5;

        public Wumpus()
        {
            _world = new WumpusWorld();
        }

        public void doit()
        {
            print("Hunt the Wumpus");

            print("instructions? [y/n]");
            String wantsInstructions = input();
            if (!(wantsInstructions.Equals("n")))
                instructions();

            _world.LoadMap();

            setupNewWorld();

            while (true)
            {
                arrows = STARTING_ARROWS;
                playerLocation = locationOf[player];
                fate = Fate.Unknown;

                while (fate == Fate.Unknown)
                {
                    situationalAwareness();

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

                if (fate == Fate.PlayerWins)
                {
                    print("You win");
                }
                else
                {
                    print("You lose");
                }

                print("Same setup? [y/n]");
                String wantsSameSetup = input();

                if (wantsSameSetup.Equals("y"))
                    restoreWorld();
                else
                    setupNewWorld();
            }
        }

        private void setupNewWorld()
        {
            bool someActorsHaveSameRoom = true;
            while (someActorsHaveSameRoom)
            {
                for (int actor = 0; actor < locationOf.ToArray().Length; actor++)
                {
                    locationOf[actor] = random1toN(WumpusWorld.MAX_ROOMS);
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

        private void restoreWorld()
        {
            for (int actor = 0; actor < locationOf.ToArray().Length; actor++)
            {
                locationOf[actor] = savedActorLocations[actor];
            }
        }

        private void situationalAwareness()
        {
            print("");
            for (int actor = wumpus; actor < locationOf.ToArray().Length; actor++)
            {
                for (int edge = 0; edge < 3; edge++)
                {
                    if (_world.Rooms[locationOf[player], edge] != locationOf[actor])
                        continue;
                    if (actor == wumpus)
                    {
                        print("I smell a wumpus!");
                        continue;
                    }
                    else if (actor == pit1 || actor == pit2)
                    {
                        print("I feel a draft");
                        continue;
                    }
                    else if (actor == bats1 || actor == bats2)
                    {
                        print("Bats nearby!");
                        continue;
                    }
                }
            }

            print("You are in room " + locationOf[player]);
            print("Tunnels lead to " + _world.Rooms[playerLocation, 0] + " " + _world.Rooms[playerLocation, 1] + " "
                  + _world.Rooms[playerLocation, 2]);
        }

        private void move()
        {
            fate = Fate.Unknown;
            print("Where to?");

            bool validRoom = false;
            do
            {
                do
                {
                    playerLocation = input_number();
                } while (!(playerLocation >= 1 && playerLocation <= WumpusWorld.MAX_ROOMS));

                if (_world.Rooms[locationOf[player], 0] == playerLocation ||
                    _world.Rooms[locationOf[player], 1] == playerLocation
                    || _world.Rooms[locationOf[player], 2] == playerLocation)
                {
                    validRoom = true;
                }
                else if (playerLocation == locationOf[player])
                {
                    validRoom = true;
                }
                else
                {
                    print("not possible");
                }
            } while (!validRoom);

            bool stillSettling = true;
            while (stillSettling)
            {
                stillSettling = false;

                locationOf[player] = playerLocation;

                if (playerLocation == locationOf[wumpus])
                {
                    print("bumped wumpus");
                    moveWumpus();
                    if (fate != Fate.Unknown)
                    {
                        return;
                    }
                }

                if (playerLocation == locationOf[pit1] || playerLocation == locationOf[pit2])
                {
                    print("fell in pit");
                    fate = Fate.PlayerLoses;
                    return;
                }
                else if (playerLocation == locationOf[bats1] || playerLocation == locationOf[bats2])
                {
                    playerLocation = random1toN(WumpusWorld.MAX_ROOMS);
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
            print("Shoot or move? [s/m]");
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
            fate = Fate.Unknown;
            int arrowLocation = locationOf[player];
            print("# of rooms? [1-" + MAX_TARGETS + "]");
            do
            {
                roomsToShoot = input_number();
            } while (!(roomsToShoot >= 1 && roomsToShoot <= MAX_TARGETS));

            for (int target = 0; target < roomsToShoot; target++)
            {
                while (true)
                {
                    print("room #");
                    targets[target] = input_number();
                    if (target <= 2 || targets[target] != targets[target - 2])
                        break;
                    print("Arrows aren't that crooked - try again");
                }
            }

            for (int target = 0; target < roomsToShoot; target++)
            {
                bool targetFound = false;
                for (int edge = 0; edge < 3; edge++)
                {
                    if (_world.Rooms[arrowLocation, edge] == targets[target])
                    {
                        targetFound = true;
                        arrowLocation = targets[target];
                        if (arrowLocation == locationOf[wumpus])
                        {
                            print("You got the wumpus");
                            fate = Fate.PlayerWins;
                            return;
                        }
                    }
                }

                if (!targetFound)
                    arrowLocation = _world.Rooms[playerLocation, random0uptoN(3)];

                if (arrowLocation == locationOf[wumpus])
                {
                    print("You got the wumpus");
                    fate = Fate.PlayerWins;
                    return;
                }
                else if (arrowLocation == locationOf[player])
                {
                    print("Ouch - arrow got you");
                    fate = Fate.PlayerLoses;
                    return;
                }
            }

            print("missed");
            playerLocation = locationOf[player];
            moveWumpus();

            arrows = arrows - 1;
            if (arrows <= 0)
            {
                fate = Fate.PlayerLoses;
            }
        }

        private void instructions()
        {
            print("You're an explorer in a set of caves, trying to kill the deadly wumpus before it kills you.");
            print("There are 20 rooms, and each connects to 3 others as corners of a dodecahedron.");
            print(
                "You get 5 arrows that can shoot across multiple rooms. If you shoot the wumpus, you win; if you run out of arrows, you die.");
            print(
                "If you enter the room with the wumpus or shoot an arrow, the wumpus randomly moves to a new room or stays in the old one.");
            print("If you're in the same room as the wumpus after its move, it eats you (and you lose).");
            print("There are other hazards: (a) bottomless pits - kill you; (b) bats - carry you to a random room.");
            print("Good luck!");
        }

        public void moveWumpus()
        {
            int newWumpusLocation = random0uptoN(4);
            if (newWumpusLocation < 3)
            {
                locationOf[wumpus] = _world.Rooms[locationOf[wumpus], newWumpusLocation];
            }
            if (locationOf[wumpus] == playerLocation)
            {
                print("wumpus got you");
                fate = Fate.PlayerLoses;
            }
        }

        public void print(String s)
        {
            Console.Out.WriteLine(s);
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

        private int random1toN(int n)
        {
            int random = rand.Next(1, n);
            return random;
        }

        private int random0uptoN(int n)
        {
            int random = rand.Next(0, n - 1);
            return random;
        }
    }
}
