using NUnit.Framework;
using TechTalk.SpecFlow;

namespace IndustrialLogic.WumpusLocation
{
    [Binding]
    public class WumpusWorldSteps
    {
        private WumpusWorld _world;

        [Given(@"a Wumpus World")]
        public void Given_aa_wumpus_world()
        {
            _world = new WumpusWorld();
            _world.LoadMap();
        }

        [When(@"I am in a (.*)")]
        public void When_I_am_in_a(int room)
        {
            _world.PutPlayerIn(room);
        }

        [Then(@"my neighbors are (.*), (.*), and (.*)")]
        public void Then_my_neighbors_are_and(int neighbour1, int neighbour2, int neighbour3)
        {
            Assert.That(_world.Neighbors.Length, Is.EqualTo(3));
            Assert.That(_world.Neighbors, Contains.Item(neighbour1));
            Assert.That(_world.Neighbors, Contains.Item(neighbour2));
            Assert.That(_world.Neighbors, Contains.Item(neighbour3));
        }
    }
}