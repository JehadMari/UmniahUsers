using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using UmniahUsers_BAL.Service;
using UmniahUsers_DAL.Models;

namespace UmniahUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ILogger<UserController> _logger;


        public UserController(UserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        //Add User
        [HttpPost("AddUser")]
        [Route("api/[controller]")]
        public async Task<object> AddUser([FromBody] User user)
        {
            try
            {
                await _userService.AddUser(user);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Update User
        [HttpPut("UpdateUser")]
        [Route("api/[controller]")]
        public bool UpdateUser([FromBody] User user)
        {
            try
            {
                _userService.UpdateUser(user);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //GET All Users
        [HttpGet("GetAllUsers")]
        [Route("api/[controller]")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        //GET User Details
        [HttpGet("GetUserById/{id}")]
        [Route("api/[controller]")]

        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound("User Not Found");
        }

        //GET User Details
        [HttpGet("GetUserByUserName/{username}")]
        [Route("api/[controller]")]

        public IActionResult GetUserByUserName(string username)
        {
            var user = _userService.GetUserByUserName(username);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound("User Not Found");
        }
    }
}