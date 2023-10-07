using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI.DTOs;
using TestAPI.Interfaces;
using TestAPI.Shared.Enums;
using TestAPI.Shared.Parameters;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserController(IUserRepository repository, IRoleRepository roleRepository)
        {
            _userRepository = repository;
            _roleRepository = roleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPage([FromQuery]RequestParameters parameters)
        {
            return Ok(await _userRepository.GetUserPage(parameters));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            return Ok(await _userRepository.GetUserById(id));
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(CreateUserDTO userDto)
        {
            return Ok(await _userRepository.CreateUser(userDto));
        }

        [HttpPost("AddUserRole")]
        public async Task<IActionResult> AddUserRole(AddRoleDTO roleDto)
        {
            var isUserExist = await _userRepository.IsUserExist(roleDto.UserId);

            if (!isUserExist)
            {
                return NotFound("User does not exist");
            }
            if (!Enum.IsDefined(typeof(RoleEnum), roleDto.RoleName))
            {
                return BadRequest("Incorrect role");
            }

            return Ok(await _roleRepository.AddRoleToUser(roleDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userRepository.DeleteUser(id);
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound("User does not exist");
            }
            
        }
    }
}
