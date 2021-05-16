using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Consumer.Dto;
using Consumer.Settings;
using Microsoft.Extensions.Options;

namespace Consumer.Connectors.Membership
{
    public class MembershipConnector : IMembershipConnector
    {
        private readonly HttpClient _httpClient;
        private readonly MembershipApiSettings _config;
        
        public MembershipConnector(HttpClient httpClient, IOptions<MembershipApiSettings> config)
        {
            _httpClient = httpClient ?? throw new ArgumentException(nameof(httpClient));
            _config = config?.Value ?? throw new ArgumentException(nameof(config));
        }

        public async Task<bool> IsUserAFellowMember(string userId)
        {
            var baseUri = _config.BaseUri;
            var uri = new Uri($"{baseUri}/users/{userId}/memberships");
            
            using var request = new HttpRequestMessage(HttpMethod.Get, uri);
            
            using HttpResponseMessage response = await _httpClient.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var membership = JsonSerializer.Deserialize<MembershipDto>(responseString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return membership?.Type.ToLower() == "fellow";
            }
            
            return false;
        }
    }
}