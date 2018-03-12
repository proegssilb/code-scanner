using FluentAssertions;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.NUnit3;
using Moq;
using NUnit.Framework;
using Proegssilb.CodeScanner.Lib;
using System.IO;

namespace Proegssilb.CodeScanner.Tests.UI
{
    [TestFixture]
    [FeatureDescription(
@"As a user,
I want to be able to type in a class name
So that I can search for a class.")]
    public class DevSearchesForClass : FeatureFixture
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

        // ----------------------------------------------------------------

        private ICodeSearchConsoleSession _session;
        private StringWriter _stdoutMock;
        private Mock<ICodeSearcher> _mockSearcher;

        private const string _search = "SampleClass";
        private const string _resp = "Assembly.Namespace.SampleClass";

        private void Given_a_console_session()
        {
            var stdin = new StringReader(_search + "\n");
            _stdoutMock = new StringWriter();
            _mockSearcher = new Mock<ICodeSearcher>();
            _mockSearcher.Setup(m => m.Search(_search)).Returns(new[] { _resp }).Verifiable();
            _session = new CodeSearchConsoleSession(stdin, _stdoutMock, _mockSearcher.Object);
        }

        private void When_the_dev_enters_a_search()
        {
            _session.Run();
        }

        private void Then_the_code_search_should_return_results()
        {
            _stdoutMock.ToString().Should().Contain(_resp);
        }

        private void Then_the_code_searching_backend_should_have_been_called()
        {
            _mockSearcher.VerifyAll();
        }
    }
}