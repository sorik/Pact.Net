using System;
using System.Net.Http;
using Consumer.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace Consumer.PactTests
{
    public class ConsumerPactClassFixture<TStartup> : IDisposable where TStartup : class
    {
        public ConsumerPactClassFixture()
        {
            var pactConfig = new PactConfig
            {
                SpecificationVersion = "2.0.0",
                PactDir = @"..\..\..\..\..\pacts",
                LogDir = @".\pact_logs"
            };
        
            PactBuilder = new PactBuilder(pactConfig);
            
            PactBuilder.ServiceConsumer("Consumer")
                .HasPactWith("Provider");
            
            MockProviderService = PactBuilder.MockService(MockServerPort);
            
            var builder = new WebHostBuilder()
                .ConfigureServices(s =>
                    s.Configure<MembershipApiSettings>(settings => settings.BaseUri = MockProviderServiceBaseUri ))
                .UseStartup<TStartup>();
            _server = new TestServer(builder);
            Client = _server.CreateClient();
            Client.BaseAddress = new Uri("http://localhost:5000");
        }

        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }

        public int MockServerPort { get { return 9222; } }
        public string MockProviderServiceBaseUri { get { return String.Format("http://localhost:{0}", MockServerPort); } }

        private readonly TestServer _server;
        public HttpClient Client { get; }

        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    PactBuilder.Build();
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