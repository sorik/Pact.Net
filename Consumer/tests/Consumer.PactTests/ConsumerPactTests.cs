using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Consumer;
using Consumer.PactTests;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace tests
{
    public class ConsumerPactTests : IClassFixture<ConsumerPactClassFixture<Startup>>
    {
        private IMockProviderService _mockProviderService;
        private string _mockServiceBaseUri;

        public ConsumerPactTests(ConsumerPactClassFixture<Startup> fixture)
        {
            _mockProviderService = fixture.MockProviderService;
            _mockServiceBaseUri = fixture.MockProviderServiceBaseUri;
            _mockProviderService.ClearInteractions();
            
            Client = fixture.Client;
        }
        
        public HttpClient Client { get; }

        [Fact]
        public async void GivenUserIsNotAMember_WhenGetMembershipInvoked_ThenReturnsFalse()
        {
            _mockProviderService.Given("The userId1 is not a member")
                .UponReceiving("A GET request with userId1")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/users/userId1/memberships"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 404
                });

            var request = new HttpRequestMessage(new HttpMethod("GET"), "/users/userId1/memberships/fellow");

            var response = await Client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("{\"member\":false}", content);
        }

        [Fact]
        public async void GivenUserIsAFellowMember_WhenGetMembershipInvoked_ThenReturnsTrue()
        {
            _mockProviderService.Given("The userId2 is a member")
                .UponReceiving("A GET request with userId2")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/users/userId2/memberships"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        type = "fellow"
                    }
                });

            var request = new HttpRequestMessage(new HttpMethod("GET"), "/users/userId2/memberships/fellow");

            var response = await Client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("{\"member\":true}", content);
        }
    }
}
