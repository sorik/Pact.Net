using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace Provider.PactTests.Repositories
{
    [DynamoDBTable("PactMembership")]
    public record PactMembership
    {
        [DynamoDBHashKey("UserId")] public string UserId { get; set; }
        public string Type { get; set; }
    }
    
    public class MembershipLocalRepository : IMembershipRepository
    {
        private readonly DynamoDBContext _dbContext;

        public MembershipLocalRepository(IAmazonDynamoDB dbClient)
        {
            _dbContext = new DynamoDBContext(dbClient);
        }

        public async Task AddUser(string userId, string type)
        {
            var member = new PactMembership
            {
                UserId = userId,
                Type = type
            };

            await _dbContext.SaveAsync(member);
        }

        public async Task DeleteAllUsers()
        {
            var conditions = new List<ScanCondition>();
            var users = await _dbContext.ScanAsync<PactMembership>(conditions).GetRemainingAsync();
            foreach (var user in users)
            {
                await _dbContext.DeleteAsync<PactMembership>(user.UserId);
            }
        }
    }
}