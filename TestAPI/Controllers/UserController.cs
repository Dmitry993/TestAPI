using Microsoft.AspNetCore.Mvc;
using TestAPI.DTOs;
using TestAPI.Interfaces;
using TestAPI.Shared.Parameters;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPage(RequestParameters parameters)
        {
            return Ok(await _repository.GetUserPage(parameters));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            return Ok(await _repository.GetUserById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDTO userDto)
        {
            return Ok(await _repository.CreateUser(userDto));
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _repository.DeleteUser(id);
        }
    }
}
