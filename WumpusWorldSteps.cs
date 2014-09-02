using System;
using TechTalk.SpecFlow;

namespace IndustrialLogic.WumpusLocation...
{
    [Binding]
    public class WumpusWorldSteps
    {
        [Given(@"the player is in room (.*)")]
public void GivenThePlayerIsInRoom(int p0)
{
    ScenarioContext.Current.Pending();
}

        [Given(@"the wumpus is in room (.*)")]
public void GivenTheWumpusIsInRoom(int p0)
{
    ScenarioContext.Current.Pending();
}

        [When(@"the player shoots into room (.*)")]
public void WhenThePlayerShootsIntoRoom(int p0)
{
    ScenarioContext.Current.Pending();
}

        [Then(@"the wumpus is dead")]
public void ThenTheWumpusIsDead()
{
    ScenarioContext.Current.Pending();
}

        [Then(@"the player wins")]
public void ThenThePlayerWins()
{
    ScenarioContext.Current.Pending();
}
    }
}
