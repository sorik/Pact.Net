using System.Threading.Tasks;
using Provider.Models;

namespace Provider.Repositories
{
    public interface IMembershipRepository
    {
        public Task<Membership> GetMembership(string userId);
    }
}