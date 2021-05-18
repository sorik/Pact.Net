using PactNet.Infrastructure.Outputters;
using Xunit.Abstractions;

namespace Provider.PactTests.XUnitHelpers
{
    public class XUnitOutput : IOutput
    {
        private readonly ITestOutputHelper _helper;

        public XUnitOutput(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        public void WriteLine(string line)
        {
            _helper.WriteLine(line);
        }
    }
}