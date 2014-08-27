﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18444
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace IndustrialLogic.WumpusLocation
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Wumpus World")]
    public partial class WumpusWorldFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "WumpusWorldScenarios.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Wumpus World", "In order to enjoy playing the wumpus game\r\nAs a player\r\nI want to be able to inte" +
                    "ract with the Wumpus World according to the standard rules", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Describe the initial layout of the Wumpus World")]
        [NUnit.Framework.TestCaseAttribute("1", "2", "5", "8", null)]
        [NUnit.Framework.TestCaseAttribute("2", "1", "3", "10", null)]
        [NUnit.Framework.TestCaseAttribute("3", "2", "4", "12", null)]
        [NUnit.Framework.TestCaseAttribute("4", "3", "5", "14", null)]
        [NUnit.Framework.TestCaseAttribute("5", "1", "4", "6", null)]
        [NUnit.Framework.TestCaseAttribute("6", "5", "7", "15", null)]
        [NUnit.Framework.TestCaseAttribute("7", "6", "8", "17", null)]
        [NUnit.Framework.TestCaseAttribute("8", "1", "7", "9", null)]
        [NUnit.Framework.TestCaseAttribute("9", "8", "10", "18", null)]
        [NUnit.Framework.TestCaseAttribute("10", "2", "9", "11", null)]
        [NUnit.Framework.TestCaseAttribute("11", "10", "12", "19", null)]
        [NUnit.Framework.TestCaseAttribute("12", "3", "11", "13", null)]
        [NUnit.Framework.TestCaseAttribute("13", "12", "14", "20", null)]
        [NUnit.Framework.TestCaseAttribute("14", "4", "13", "15", null)]
        [NUnit.Framework.TestCaseAttribute("15", "6", "14", "16", null)]
        [NUnit.Framework.TestCaseAttribute("16", "15", "17", "20", null)]
        [NUnit.Framework.TestCaseAttribute("17", "7", "16", "18", null)]
        [NUnit.Framework.TestCaseAttribute("18", "9", "17", "19", null)]
        [NUnit.Framework.TestCaseAttribute("19", "11", "18", "20", null)]
        [NUnit.Framework.TestCaseAttribute("20", "13", "16", "19", null)]
        public virtual void DescribeTheInitialLayoutOfTheWumpusWorld(string room, string a, string b, string c, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Describe the initial layout of the Wumpus World", exampleTags);
#line 6
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("a Wumpus World", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.When(string.Format("I am in a {0}", room), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 10
 testRunner.Then(string.Format("my neighbors are {0}, {1}, and {2}", a, b, c), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion