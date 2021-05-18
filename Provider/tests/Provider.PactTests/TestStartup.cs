using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Provider.PactTests.Middleware;
using Provider.PactTests.Repositories;

namespace Provider.PactTests
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddSingleton<IAmazonDynamoDB>(s => 
                new AmazonDynamoDBClient(
                    new AmazonDynamoDBConfig
                    {
                        ServiceURL = Configuration.GetSection("AWS")["ServiceUrl"],
                        AuthenticationRegion = Configuration.GetSection("AWS")["Region"]
                    }));
            services.AddScoped<IMembershipRepository, MembershipLocalRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMvc();
            app.UseMiddleware<ProviderStateMiddleware>();
        }
    }
}