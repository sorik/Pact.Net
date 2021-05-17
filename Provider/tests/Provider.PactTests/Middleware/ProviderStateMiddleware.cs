using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Provider.PactTests.Middleware
{
    public class ProviderStateMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ConsumerName = "Consumer";
        private readonly IDictionary<string, Action> _providerState;

        public ProviderStateMiddleware(RequestDelegate next)
        {
            _next = next;
            _providerState = new Dictionary<string, Action>();
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value == "/pact-states")
            {
                HandleProviderStateRequest(context);
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                await context.Response.WriteAsync(String.Empty);
            }
            else
            {
                await _next(context);
            }
        }

        private async void HandleProviderStateRequest(HttpContext context)
        {
            if (context.Request.Method.ToUpper() == HttpMethods.Post.ToString().ToUpper() &&
                context.Request.Body != null)
            {
                string jsonRequestBody = String.Empty;
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    jsonRequestBody = await reader.ReadToEndAsync();
                }

                var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);
                if (providerState != null && providerState.Consumer == ConsumerName &&
                    !String.IsNullOrEmpty(providerState.State))
                {
                    _providerState[providerState.State].Invoke();
                }
            }
        }
    }
}