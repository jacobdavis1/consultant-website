using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using consultant_data.Models;
using consultant_data.RepositoryInterfaces;
using consultant_logic.Repositories;
using consultant_server.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace consultant_server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository _user;
        private IAuthProvider _auth;

        public UserController(IUserRepository user, IAuthProvider auth)
        {
            _user = user;
            _auth = auth;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<ActionResult<User>> Get()
        {
            try
            {
                string userId = _auth.GetUserIdFromToken(HttpContext);
                if (userId == "")
                    return Forbid();

                User user = await _user.GetUserByIdAsync(userId);

                if (user == null)
                {
                    user = new User
                    {
                        UserId = userId,
                        Role = Role.Client
                    };

                    user = await _user.AddUserAsync(user);
                }

                return user;
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
