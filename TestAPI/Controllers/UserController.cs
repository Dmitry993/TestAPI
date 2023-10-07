using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TestAPI.DTOs;
using TestAPI.Interfaces;
using TestAPI.Shared.Enums;
using TestAPI.Shared.Parameters;

namespace TestAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private const string emailPattern = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository repository, IRoleRepository roleRepository, IMapper mapper)
        {
            _userRepository = repository;
            _roleRepository = roleRepository;
            _mapper = mapper;
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
            var validationResult = ValidateUser(_mapper.Map<UserDTO>(userDto));

            if (validationResult == "Ok")
            {
                return Ok(await _userRepository.CreateUser(userDto));
            }

            return BadRequest(validationResult);
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

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserDTO userDto)
        {
            var validationResult = ValidateUser(_mapper.Map<UserDTO>(userDto));

            if (validationResult == "Ok")
            {
                var user = await _userRepository.UpdateUser(userDto);
                return user is null ? NotFound("User does not exist") : Ok(user);
            }

            return BadRequest(validationResult);
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

        private string ValidateUser(UserDTO userDto) => userDto switch
        {
            { Age: <= 0 } => "Age must be greater than zero",
            UserDTO user when !Regex.IsMatch(user.Email, emailPattern) => "Incorrect email",
            UserDTO user when string.IsNullOrWhiteSpace(user.Name) => "Name must contain at least 1 character",
            UserDTO user when user.Password is null || user.Password.Length < 6
                => "Password must contain at least 6 character",
            _ => "Ok"
        };
    }
}
