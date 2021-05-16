using System.Threading.Tasks;

namespace Consumer.Connectors.Membership
{
    public interface IMembershipConnector
    {
        Task<bool> IsUserAFellowMember(string userId);
    }
}