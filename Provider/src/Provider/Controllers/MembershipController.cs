using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Provider.Exceptions;
using Provider.Models;
using Provider.Repositories;

namespace Provider.Controllers
{
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipRepository _repository;
        
        public MembershipController(IMembershipRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("/users/{userId}/memberships")]
        public async Task<IActionResult> GetUserMembershipAsync([FromRoute] string userId)
        {
            try
            {
                var membership = await _repository.GetMembership(userId);
                return Ok(membership);
            }
            catch (MemberNotFoundException e)
            {
                return new NotFoundResult();
            }
        }
    }
    

}