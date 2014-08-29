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
            ScenarioContext.Current.Pending();
        }

        [Then(@"my neighbors are (.*), (.*), and (.*)")]
        public void Then_my_neighbors_are_and(int neighbour1, int neighbour2, int neighbour3)
        {
            ScenarioContext.Current.Pending();
        }
    }
}