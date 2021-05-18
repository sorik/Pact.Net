using System;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PactNet;
using PactNet.Infrastructure.Outputters;
using Provider.PactTests.XUnitHelpers;
using Xunit;
using Xunit.Abstractions;

namespace Provider.PactTests
{
    public class ProviderApiTests : IDisposable
    {
        private string _providerUri { get; }
        private string _pactServiceUri { get; }
        private IWebHost _webHost { get;  }
        private ITestOutputHelper _outputHelper { get; }

        public ProviderApiTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _providerUri = "http://run-standalone-app:5001";
            _pactServiceUri = "http://localhost:9001";
            _webHost = WebHost.CreateDefaultBuilder()
                .UseUrls(_pactServiceUri)
                .UseStartup<TestStartup>()
                .Build();
            _webHost.Start();
        }

        [Fact]
        public void EnsureProviderApiHonoursPactWithConsumer()
        {
            var config = new PactVerifierConfig
            {
                Outputters = new List<IOutput>
                {
                    new XUnitOutput(_outputHelper)
                },
                Verbose = true
            };

            IPactVerifier pactVerifier = new PactVerifier(config);
            pactVerifier.ProviderState($"{_pactServiceUri}/pact-states")
                .ServiceProvider("Provider", _providerUri)
                .HonoursPactWith("Consumer")
                .PactUri("../../../../../pacts/consumer-provider.json")
                .Verify();
        }

        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _webHost.StopAsync().GetAwaiter().GetResult();
                    _webHost.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}