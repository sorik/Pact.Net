using System.Threading.Tasks;
using Consumer.Connectors.Membership;
using Consumer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Consumer.Controllers
{
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipConnector _connector;

        public MembershipController(IMembershipConnector connector)
        {
            _connector = connector;
        }

        [HttpGet("/users/{userId}/memberships/fellow")]
        public async Task<IActionResult> GetFellowMembershipAsync([FromRoute] string userId)
        {
            var isMembership = await _connector.IsUserAFellowMember(userId);

            var result = new Membership
            {
                Member = isMembership
            };
            
            return Ok(result);
        }   
    }
}