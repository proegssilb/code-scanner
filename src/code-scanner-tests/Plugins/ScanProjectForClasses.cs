using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.NUnit3;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Reactive.Testing;
using NUnit.Framework;
using Proegssilb.CodeScanner.CorePlugin.Scanners;

namespace Proegssilb.CodeScanner.Tests.Plugins
{
    [TestFixture]
    [FeatureDescription(
@"As a user,
I want classes to be indexed
So that I can search for a class.")]
    class ScanProjectForClasses : FeatureFixture
    {
        [Scenario]
        [ScenarioCategory("CorePlugin")]
        [ScenarioCategory("Rescanners")]
        public void ScanningProjectsWithClassesYieldsClasses()
        {
            Runner.RunScenario(
                Given_a_solution_with_projects_and_classes,
                Given_the_solution_has_projects,
                Given_the_projects_have_documents,
                Given_a_way_to_scan_projects,
                When_a_project_has_been_scanned,
                Then_classes_in_the_project_should_be_indexed
                );
        }

        // ----------------------------------------------------------------

        private AdhocWorkspace _workspace;
        private Solution _sln;
        private ProjectInfo[] _projs;
        private ProjectRescanner _projectScanner;
        private TestScheduler _scheduler;
        private ITestableObservable<Project> _inputObservable;
        private ITestableObserver<ClassDeclarationSyntax> _results;
        private IEnumerable<ClassDeclarationSyntax> _classes;

        private void Given_a_solution_with_projects_and_classes()
        {
            _workspace = new AdhocWorkspace();
            var si = SolutionInfo.Create(SolutionId.CreateNewId(), VersionStamp.Default);
            _workspace.AddSolution(si);
            _sln = _workspace.CurrentSolution;
        }

        private void Given_the_solution_has_projects()
        {
            // TODO: Generate random data.
            _projs = new[] {
                ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Default, "ProjectName", "ProjectName", "C#"),
                ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Default, "ProjectName2", "ProjectName2", "C#")
            };
            foreach (var proj in _projs)
            {
                _workspace.AddProject(proj);
            }
            _sln = _workspace.CurrentSolution;
        }

        private void Given_the_projects_have_documents()
        {
            var doc = _workspace.AddDocument(_projs[0].Id, "SampleDoc.cs", SourceText.From(@"namespace Test
{
    public class SampleClass
    {
    }
}"));
            var doc2 = _workspace.AddDocument(_projs[1].Id, "SampleDoc2.cs", SourceText.From(@"namespace Test
{
    public class SampleClass2
    {
    }
}"));
            _classes = doc.GetSyntaxRootAsync().Result.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .Concat(doc2.GetSyntaxRootAsync().Result.DescendantNodes().OfType<ClassDeclarationSyntax>())
                .ToArray();
            _sln = _workspace.CurrentSolution;
        }

        private void Given_a_way_to_scan_projects()
        {
            _projectScanner = new ProjectRescanner();
            _scheduler = new TestScheduler();
            _inputObservable = _scheduler.CreateColdObservable(_sln.Projects
                .Select((proj, i) => ReactiveTest.OnNext(i * 100 + 100, proj))
                .Append(ReactiveTest.OnCompleted<Project>(_sln.Projects.Count() * 100 + 300))
                .ToArray()
                );
        }

        private void When_a_project_has_been_scanned()
        {
            _results = _scheduler.Start(() => _projectScanner.Scan(_inputObservable), 50, 50, _sln.Projects.Count() * 100 + 700);
        }

        private void Then_classes_in_the_project_should_be_indexed()
        {
            var tbase = 150;
            var notifications = _classes.Select((cls, i) => ReactiveTest.OnNext(i*100+tbase, cls))
                .Append(ReactiveTest.OnCompleted<ClassDeclarationSyntax>(tbase+400))
                .ToArray();
            ReactiveAssert.AreElementsEqual(notifications, _results.Messages);
        }
    }
}
