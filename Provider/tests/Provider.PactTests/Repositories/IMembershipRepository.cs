using System.Threading.Tasks;

namespace Provider.PactTests.Repositories
{
    public interface IMembershipRepository
    {
        public Task AddUser(string userId, string type);
        public Task DeleteAllUsers();
    }
}