using Bulimia.MessengerServer.BLL.Services;
using Bulimia.MessengerServer.Domain.Core;
using Microsoft.AspNetCore.Mvc;

namespace Bulimia.MessengerServer.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;

        public UsersController(UsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<List<UserRecord>> GetUsers()
        {
            return await _usersService.GetUsers();
        }

        [HttpPost(Name = "GetUsernameById")]
        public async Task<string?> GetUsernameById(int id)
        {
            return await _usersService.GetUsernameById(id);
        }
    }
}