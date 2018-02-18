using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.NUnit3;
using NUnit.Framework;

namespace Proegssilb.CodeScanner.Tests
{
    [TestFixture]
    [FeatureDescription(
@"As a user,
I want projects indexed when solutions are indexed
So that I can find how classes map to solutions.")]
    public partial class ScanSolutionForProjects : FeatureFixture
    {
        [Scenario]
        [ScenarioCategory("CorePlugin")]
        [ScenarioCategory("Rescanners")]
        public void ScanningSolutionWithNoProjectsYieldsNoProjects()
        {
            Runner.RunScenario(
                Given_a_solution_with_no_projects,
                When_the_solution_has_been_scanned,
                Then_no_projects_should_be_found
                );
        }
    }
}
