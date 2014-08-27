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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wumpus
{
    public class Wumpus
    {
// Inspired by Wumpus as designed by Gregory Yob in the 1970s. 
        private Random rand = new Random();
	    private const int MAX_ROOMS = 20;
        private const int MAX_EDGES = 3;
        private const int MAX_TARGETS = 5;
	    private const int STARTING_ARROWS = 5;
	
	    public enum Fate {Unknown, PlayerWins, PlayerLoses};

	    public enum Action {Shoot, Move};
	
	    int arrows;
	    int playerLocation;

	    Fate fate;
	
	    int[,] rooms = new int[MAX_ROOMS + 1, MAX_EDGES];   // 1-based, room 0 not used
	    int[] targets = new int[MAX_TARGETS];

        private static readonly int[] neighboringRooms = { 2, 5, 8, 1, 3, 10, 2, 4, 12, 3, 5, 14, 1, 4, 6, 5, 7, 15, 6, 8, 17, 1, 7, 9, 8,
            10, 18, 2, 9, 11, 10, 12, 19, 3, 11, 13, 12, 14, 20, 4, 13, 15, 6, 14, 16, 15, 17, 20,
            7, 16, 18, 9, 17, 19, 11, 18, 20, 13, 16, 19 };


        int[] locationOf = new int[6];
	    int[] savedActorLocations = new int[6];

	    const int player = 0;
	    const int wumpus = 1;
	    const int pit1 = 2;
	    const int pit2 = 3;
	    const int bats1 = 4;
	    const int bats2 = 5;

	
	    public void doit() {
		    print("Hunt the Wumpus");

		    print("instructions? [y/n]");
		    String wantsInstructions = input();
		    if (!(wantsInstructions.Equals("n")))
			    instructions();

		    loadMap();
		
		    setupNewWorld();
				
		    while (true) {
				    arrows = STARTING_ARROWS;
				    playerLocation = locationOf[player];
				    fate = Fate.Unknown;
				
				    while (fate == Fate.Unknown) {
					    situationalAwareness();
					
					    Action action = chooseOption();
	
					    if (action == Action.Shoot) {
						    shoot();
					    } else if (action == Action.Move) {
						    move();
					    }
				    }	

				    if (fate == Fate.PlayerWins) {
					    print("You win");
				    } else {
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

	private void loadMap() {
		int index = 0;
		for (int room = 1; room <= MAX_ROOMS; room++) {
			for (int edge = 0; edge < MAX_EDGES; edge++) {
				rooms[room,edge] = neighboringRooms[index++];
			}
		}
	}

	private void setupNewWorld() {
		bool someActorsHaveSameRoom = true;
		while (someActorsHaveSameRoom) {
			for (int actor = 0; actor < locationOf.ToArray().Length; actor++) {
				locationOf[actor] = random1toN(MAX_ROOMS);
				savedActorLocations[actor] = locationOf[actor];
			}
			
			someActorsHaveSameRoom = false;
			for (int actor1 = 0; actor1 < locationOf.ToArray().Length; actor1++) {
				for (int actor2 = actor1 + 1; actor2 < locationOf.ToArray().Length; actor2++) {
					if (locationOf[actor1] == locationOf[actor2])
						someActorsHaveSameRoom = true;
				}
			}
		}
	}

	private void restoreWorld() {
		for (int actor = 0; actor < locationOf.ToArray().Length; actor++) {
			locationOf[actor] = savedActorLocations[actor];
		}
	}

	private void situationalAwareness() {
		print("");
		for (int actor = wumpus; actor < locationOf.ToArray().Length; actor++) {
			for (int edge = 0; edge < 3; edge++) {
				if (rooms[locationOf[player],edge] != locationOf[actor])
					continue;
				if (actor  == wumpus) {
					print("I smell a wumpus!");
					continue;
				} else if (actor  == pit1 || actor == pit2) {
					print("I feel a draft");
					continue;
				} else if (actor == bats1 || actor == bats2) {
					print("Bats nearby!");
					continue;
				}
			}
		}

		print("You are in room " + locationOf[player]);
		print("Tunnels lead to " + rooms[playerLocation,0] + " " + rooms[playerLocation,1] + " "
				+ rooms[playerLocation,2]);
	}

	private void move() {
		fate = Fate.Unknown;
		print("Where to?");

		bool validRoom = false;
		do {
			do {
				playerLocation = input_number();
			} while (!(playerLocation >= 1 && playerLocation <= MAX_ROOMS));

			if (rooms[locationOf[player],0] == playerLocation || rooms[locationOf[player],1] == playerLocation
					|| rooms[locationOf[player],2] == playerLocation) {
				validRoom = true;
			} else if (playerLocation == locationOf[player]) {
				validRoom = true;
			} else {
				print("not possible");
			}
		} while (!validRoom);
		
		bool stillSettling = true;
		while (stillSettling) {
			stillSettling = false;
			
			locationOf[player] = playerLocation;
	
			if (playerLocation == locationOf[wumpus]) {
				print("bumped wumpus");
				moveWumpus();
				if (fate != Fate.Unknown) {
					return;
				}
			}
	
			if (playerLocation == locationOf[pit1] || playerLocation == locationOf[pit2]) {
				print("fell in pit");
				fate = Fate.PlayerLoses;
				return;
			} else if (playerLocation == locationOf[bats1] || playerLocation == locationOf[bats2]) {
				playerLocation = random1toN(MAX_ROOMS);
				stillSettling = true;
			} else {
				return;
			}
		}
	}

	private Action chooseOption() {
		String moveType;
		print("Shoot or move? [s/m]");
		do {
			moveType = input();
		} while (!(moveType.Equals("s") || moveType.Equals("m")));

		if (moveType.Equals("s")) {
			return Action.Shoot;
		} else if (moveType.Equals( "m")) {
			return Action.Move;
		}
		return Action.Move;	// can't happen
	}

	private void shoot() {
		int roomsToShoot;
		fate = Fate.Unknown;
		int arrowLocation = locationOf[player];
		print("# of rooms? [1-" + MAX_TARGETS + "]");
		do {
			roomsToShoot = input_number();
		} while (!(roomsToShoot >= 1 && roomsToShoot <= MAX_TARGETS));

		for (int target = 0; target < roomsToShoot; target++) {
			while (true) {
				print("room #");
				targets[target] = input_number();
				if (target <= 2 || targets[target] != targets[target - 2])
					break;
				print("Arrows aren't that crooked - try again");
			}
		}

		for (int target = 0; target < roomsToShoot; target++) {
			bool targetFound = false;
			for (int edge = 0; edge < 3; edge++) {
				if (rooms[arrowLocation,edge] == targets[target]) {
					targetFound = true;
					arrowLocation = targets[target];
					if (arrowLocation == locationOf[wumpus]) {
						print("You got the wumpus");
						fate = Fate.PlayerWins;
						return;
					}
				}
			}

			if (!targetFound) 
				arrowLocation = rooms[playerLocation,random0uptoN(3)];
			
			if (arrowLocation == locationOf[wumpus]) {
				print("You got the wumpus");
				fate = Fate.PlayerWins;
				return;
			} else if (arrowLocation == locationOf[player]) {
				print("Ouch - arrow got you");
				fate = Fate.PlayerLoses;
				return;
			}
		}

		print("missed");
		playerLocation = locationOf[player];
		moveWumpus();

		arrows = arrows - 1;
		if (arrows <= 0) {
			fate = Fate.PlayerLoses;
		}
	}

	private void instructions() {
		print("You're an explorer in a set of caves, trying to kill the deadly wumpus before it kills you.");
		print("There are 20 rooms, and each connects to 3 others as corners of a dodecahedron.");
		print("You get 5 arrows that can shoot across multiple rooms. If you shoot the wumpus, you win; if you run out of arrows, you die.");
		print("If you enter the room with the wumpus or shoot an arrow, the wumpus randomly moves to a new room or stays in the old one.");
		print("If you're in the same room as the wumpus after its move, it eats you (and you lose).");
		print("There are other hazards: (a) bottomless pits - kill you; (b) bats - carry you to a random room.");
		print("Good luck!");
	}

	public void moveWumpus() {
		int newWumpusLocation = random0uptoN(4);
		if (newWumpusLocation < 3) {
			locationOf[wumpus] = rooms[locationOf[wumpus],newWumpusLocation];
		}
		if (locationOf[wumpus] == playerLocation) {
			print("wumpus got you");
			fate = Fate.PlayerLoses;
		}
	}

	public void print(String s) {
        Console.Out.WriteLine(s);
	}

	public String input()
	{
	    return Console.In.ReadLine();
	}

	public int input_number() {
		String numberString = input();
		try
		{
		    return Convert.ToInt32(numberString);
		} catch (Exception e) {
			return 0;
		}
	}

	private int random1toN(int n)
	{
	    int random = rand.Next(1,n);
		return random;
	}

	private int random0uptoN(int n)
	{
	    int random = rand.Next(0, n - 1);
		return random;
	}
	

}

    }
