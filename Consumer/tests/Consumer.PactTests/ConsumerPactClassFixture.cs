using System;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace Consumer.PactTests
{
    public class ConsumerPactClassFixture : IDisposable
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
        }

        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }

        public int MockServerPort { get { return 9222; } }
        public string MockProviderServiceBaseUri { get { return String.Format("http://localhost:{0}", MockServerPort); } }


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