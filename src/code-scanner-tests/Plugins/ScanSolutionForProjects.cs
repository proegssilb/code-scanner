using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.NUnit3;
using Microsoft.CodeAnalysis;
using Microsoft.Reactive.Testing;
using NUnit.Framework;
using Proegssilb.CodeScanner.CorePlugin.Scanners;
using Proegssilb.CodeScanner.Lib.Interfaces;
using System.Linq;
using System.Reactive;

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
                Given_a_way_to_scan_solutions,
                When_the_solution_has_been_scanned,
                Then_no_projects_should_be_found
                );
        }

        [Scenario]
        [ScenarioCategory("CorePlugin")]
        [ScenarioCategory("Rescanners")]
        public void ScanningSolutionWithSomeProjectsYieldsSomeProjects()
        {
            Runner.RunScenario(
                Given_a_solution_with_n_projects,
                Given_a_way_to_scan_solutions,
                When_the_solution_has_been_scanned,
                Then_n_projects_should_be_found
                );
        }

        // ----------------------------------------------------------------

        private Solution _sln;
        private ProjectInfo[] _projs;
        private AdhocWorkspace _workspace;
        private IRescanner<Solution, Project> _solutionScanner;
        private TestScheduler _scheduler;
        private ITestableObservable<Solution> _inputObservable;
        private ITestableObserver<Project> _results;

        private void Given_a_solution_with_no_projects()
        {
            _workspace = new AdhocWorkspace();
            var si = SolutionInfo.Create(SolutionId.CreateNewId(), VersionStamp.Default);
            _workspace.AddSolution(si);
            _sln = _workspace.CurrentSolution;
        }

        private void Given_a_way_to_scan_solutions()
        {
            _solutionScanner = new SolutionRescanner();
            _scheduler = new TestScheduler();
            _inputObservable = _scheduler.CreateColdObservable(ReactiveTest.OnNext(100, _sln), ReactiveTest.OnCompleted<Solution>(300));
        }

        private void When_the_solution_has_been_scanned()
        {
            _results = _scheduler.Start(() => _solutionScanner.Scan(_inputObservable), 25, 50, 400);
        }

        private void Then_no_projects_should_be_found()
        {
            ReactiveAssert.AreElementsEqual(new Recorded<Notification<Project>>[] { ReactiveTest.OnCompleted<Project>(350) }, _results.Messages);
            ReactiveAssert.AreElementsEqual(new Subscription[] { ReactiveTest.Subscribe(50, 350) }, _inputObservable.Subscriptions);
        }

        private void Given_a_solution_with_n_projects()
        {
            _workspace = new AdhocWorkspace();
            var si = SolutionInfo.Create(SolutionId.CreateNewId(), VersionStamp.Default);
            _workspace.AddSolution(si);
            _sln = _workspace.CurrentSolution;
            // TODO: Generate random data.
            _projs = new[] {
                ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Default, "ProjectName", "ProjectName", "C#"),
                ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Default, "ProjectName2", "ProjectName2", "C#")
            };
            foreach (var proj in _projs)
            {
                _workspace.AddProject(proj);
                _sln = _sln.AddProject(proj);
            }
            _workspace.TryApplyChanges(_sln);
        }

        private void Then_n_projects_should_be_found()
        {
            var notifications = _sln.Projects.Select(p => ReactiveTest.OnNext(150, p)).ToList();
            notifications.Add(ReactiveTest.OnCompleted<Project>(350));
            ReactiveAssert.AreElementsEqual(notifications, _results.Messages);
            ReactiveAssert.AreElementsEqual(new Subscription[] { ReactiveTest.Subscribe(50, 350) }, _inputObservable.Subscriptions);
        }
    }
}
