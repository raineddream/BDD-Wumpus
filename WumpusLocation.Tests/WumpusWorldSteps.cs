using System;
using System.ComponentModel;
using Moq;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace IndustrialLogic.WumpusLocation
{
    [Binding]
    public class WumpusWorldSteps
    {
        private WumpusWorld _world;
        private Mock<IGameReporter> _reporterMock;

        [BeforeScenario]
        public void SetupScenario()
        {
            _reporterMock = new Mock<IGameReporter>();
        }

        [Given(@"a Wumpus World")]
        public void Given_a_wumpus_world()
        {
            _world = new WumpusWorld(_reporterMock.Object);
            _world.LoadMap();
        }

        [Then(@"his neighbors are (.*), (.*), and (.*)")]
        public void Then_his_neighbors_are_and(int neighbour1, int neighbour2, int neighbour3)
        {
            Assert.That(_world.Player.Neighbors.Length, Is.EqualTo(3));
            Assert.That(_world.Player.Neighbors, Contains.Item(neighbour1));
            Assert.That(_world.Player.Neighbors, Contains.Item(neighbour2));
            Assert.That(_world.Player.Neighbors, Contains.Item(neighbour3));
        }

        [Given(@"the player is in room (.*)")]
        [When( @"the player is in room (.*)")]
        public void Given_the_player_is_in_room(int room)
        {
            _world.PutPlayerIn(room);
        }

        [Given(@"the wumpus is in room (.*)")]
        public void Given_the_wumpus_is_in_room(int room)
        {
            _world.WumpusLocation = room;
        }

        [When(@"the player shoots into room (.*)")]
        public void When_the_player_shoots_into_room(int room)
        {
            _world.Player.ShootIntoRooms(room);
        }

        [Then(@"the wumpus is dead")]
        public void Then_the_wumpus_is_dead()
        {
            Assert.That(_world.WumpusIsDead(), Is.True);
        }

        [Then(@"the player wins")]
        public void Then_the_player_wins()
        {
            Assert.That(_world.Player.DoesWin(), Is.True);
        }

        [Then(@"game prompts ""(.*)""")]
        public void Then_game_prompts(string message)
        {
            _reporterMock.Verify(x => x.Report(message));
        }

        [Given(@"a room with an adjacent (.*)")]
        public void Given_a_room_with_an_adjacent(string hazard)
        {
            int location = GetLocationOfActor(ParseToActor(hazard));
            int[] neighbors = _world.NeighborsOf(location);

            _world.PutPlayerIn(neighbors[0]);
            _world.SituationalAwareness();
        }

        [Then(@"you get the corresponding (.*)")]
        public void Then_you_get_the_corresponding(string message)
        {
            _reporterMock.Verify(x => x.Report(message));
        }

        private int GetLocationOfActor(Actor actor)
        {
            switch (actor)
            {
                case Actor.Wumpus:
                    return _world.WumpusLocation;
                case Actor.Bat1:
                    return _world.Bat1Location;
                case Actor.Pit1:
                    return _world.Pit1Location;
            }
            throw new ArgumentException();
        }

        private Actor ParseToActor(string actorName)
        {
            switch (actorName)
            {
                case "wumpus":
                    return Actor.Wumpus;
                case "bats":
                    return Actor.Bat1;
                case "bottomless pit":
                    return Actor.Pit1;
            }
            throw new ArgumentException();
        }
    }
}