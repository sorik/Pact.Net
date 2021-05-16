using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Provider.Exceptions;
using Provider.Models;

namespace Provider.Repositories
{
    [DynamoDBTable("PactMembership")]
    public record PactMembership
    {
        [DynamoDBHashKey("UserId")] public string UserId { get; set; }
        public string Type { get; set; }
    }
    
    public class MembershipRepository : IMembershipRepository
    {
        private readonly DynamoDBContext _dbContext;
        
        public MembershipRepository(IAmazonDynamoDB dbClient)
        {
            _dbContext = new DynamoDBContext(dbClient);
        }

        public async Task<Membership> GetMembership(string userId)
        {
            var membership = await _dbContext.LoadAsync<PactMembership>(userId);

            if (membership is null)
            {
                throw new MemberNotFoundException();
            }
            
            return new Membership
            {
                Type = membership.Type
            };
        }
    }
}