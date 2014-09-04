using NUnit.Framework;
using TechTalk.SpecFlow;

namespace IndustrialLogic.WumpusLocation
{
    [Binding]
    public class WumpusWorldSteps
    {
        private WumpusWorld _world;

        [BeforeScenario]
        public void SetupScenario()
        {
            _world = new WumpusWorld();
            _world.LoadMap();
        }

        [Given(@"a Wumpus World")]
        public void Given_a_wumpus_world()
        {
        }

        [When(@"I am in a (.*)")]
        public void When_I_am_in_a(int room)
        {
            _world.PlayerLocation = room;
        }

        [Then(@"my neighbors are (.*), (.*), and (.*)")]
        public void Then_my_neighbors_are_and(int neighbour1, int neighbour2, int neighbour3)
        {
            Assert.That(_world.PlayNeighbors.Length, Is.EqualTo(3));
            Assert.That(_world.PlayNeighbors, Contains.Item(neighbour1));
            Assert.That(_world.PlayNeighbors, Contains.Item(neighbour2));
            Assert.That(_world.PlayNeighbors, Contains.Item(neighbour3));
        }

        [Given(@"the player is in room (.*)")]
        public void Given_the_player_is_in_room(int room)
        {
            _world.PlayerLocation = room;
        }

        [Given(@"the wumpus is in room (.*)")]
        public void Given_the_wumpus_is_in_room(int room)
        {
            _world.WumpusLocation = room;
        }

        [When(@"the player shoots into room (.*)")]
        public void When_the_player_shoots_into_room(int room)
        {
            _world.PlayerShootsInto(room);
        }

        [Then(@"the wumpus is dead")]
        public void Then_the_wumpus_is_dead()
        {
            Assert.That(_world.WumpusIsDead(), Is.True);
        }

        [Then(@"the player wins")]
        public void Then_the_player_wins()
        {
            Assert.That(_world.DoesPlayWin(), Is.True);
        }
    }
}