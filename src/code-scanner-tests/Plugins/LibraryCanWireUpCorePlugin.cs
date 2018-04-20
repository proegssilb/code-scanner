using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.NUnit3;
using MicroResolver;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Reactive.Testing;
using NUnit.Framework;
using Proegssilb.CodeScanner.CorePlugin.Scanners;
using Proegssilb.CodeScanner.Lib;
using Proegssilb.CodeScanner.Lib.Data;
using Proegssilb.CodeScanner.Lib.Interfaces;
using Proegssilb.CodeScanner.Lib.Plugins;

namespace Proegssilb.CodeScanner.Tests.Plugins
{
    class LibraryCanWireUpCorePlugin : FeatureFixture
    {

        [Scenario]
        [ScenarioCategory("CorePlugin")]
        [ScenarioCategory("Rescanners")]
        [ScenarioCategory("ScanningDataflow")]
        public void LibraryCanConnectCorePluginClasses()
        {
            Runner.RunScenario(
                Given_a_config_which_specifies_a_solution,
                When_the_library_generates_a_scan_flow,
                Then_the_scan_flow_should_include_a_project_rescanner,
                Then_the_scan_flow_should_include_a_solution_rescanner
                );
        }

        // ----------------------------------------------------------------

        private AdhocWorkspace _workspace;
        private Solution _sln;
        private LibConfig _config;
        private PluginManager _pluginManager;
        private ScanFlow _sut;

        private void Given_a_config_which_specifies_a_solution()
        {
            _workspace = new AdhocWorkspace();
            var si = SolutionInfo.Create(SolutionId.CreateNewId(), VersionStamp.Default);
            _workspace.AddSolution(si);
            _sln = _workspace.CurrentSolution;

            _config = new LibConfig(new[] { _workspace }, "");

            var container = ObjectResolver.Create();
            container.Register<IRescanner<Project, ClassDeclarationSyntax>, ProjectRescanner>();
            container.Register<IRescanner<Solution, Project>, SolutionRescanner>();
            container.RegisterCollection<IRescanner>(typeof(ProjectRescanner), typeof(SolutionRescanner));
            _pluginManager = new PluginManager(container);
        }

        private void When_the_library_generates_a_scan_flow()
        {
            _sut = new ScanFlow(_config, _pluginManager);
            _sut.GenerateFlow();
        }

        private void Then_the_scan_flow_should_include_a_project_rescanner()
        {
            _sut.Rescanners.Count.Should().Equals(2);
            _sut.Rescanners.Any(r => r is ProjectRescanner);
        }

        private void Then_the_scan_flow_should_include_a_solution_rescanner()
        {
            _sut.Rescanners.Count.Should().Equals(2);
            _sut.Rescanners.Any(r => r is SolutionRescanner);
        }
    }
}
