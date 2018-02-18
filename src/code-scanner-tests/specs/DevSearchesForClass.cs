using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.NUnit3;
using NUnit.Framework;

namespace Proegssilb.CodeScanner.Tests
{
    [TestFixture]
    [FeatureDescription(
@"As a user,
I want to be able to type in a class name
So that I can search for a class.")]
    public partial class DevSearchesForClass : FeatureFixture
    {
        [Scenario]
        [ScenarioCategory("Console")]
        [ScenarioCategory("Search")]
        public void EnterTextInConsole()
        {
            Runner.RunScenario(
                Given_a_console_session,
                When_the_dev_enters_a_search,
                Then_the_code_search_should_return_results,
                Then_the_code_searching_backend_should_have_been_called
                );
        }
    }
}
