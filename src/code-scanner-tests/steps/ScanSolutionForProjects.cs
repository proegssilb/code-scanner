using Microsoft.CodeAnalysis;
using Proegssilb.CodeScanner.Lib.Interfaces;
using System;
using Proegssilb.CodeScanner.CorePlugin.Scanners;
using System.Collections.Generic;
using System.Text;
using Moq;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;

namespace Proegssilb.CodeScanner.Tests
{
    public partial class ScanSolutionForProjects
    {
        private Solution _sln;
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

            // The scanner will use this object to register a callable that handles items, so we need to extract that callable.
            _scheduler = new TestScheduler();

            _inputObservable = _scheduler.CreateColdObservable(ReactiveTest.OnNext(100, _sln), ReactiveTest.OnCompleted<Solution>(300));
        }

        private void When_the_solution_has_been_scanned()
        {
            _results = _scheduler.Start(() => _solutionScanner.Scan(_inputObservable),25,50,600);
        }

        private void Then_no_projects_should_be_found()
        {
            ReactiveAssert.AreElementsEqual(_results.Messages, new Recorded<Notification<Project>>[] { ReactiveTest.OnCompleted<Project>(350)});
            ReactiveAssert.AreElementsEqual(_inputObservable.Subscriptions, new Subscription[] { ReactiveTest.Subscribe(50,350)});
        }
    }
}
