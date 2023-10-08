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

        /// <summary>
        /// Gets the page of users.
        /// </summary>
        /// <remarks>
        /// To get the desired page and number of elements, add values to the CurrentPage and PageSize properties:
        /// 
        ///     CurrentPage: 1,
        ///     PageSize: 10
        ///     
        /// To sort, add the property name and sort direction.
        /// To sort in ascending order use ASC, in descending order DESC:
        /// 
        ///     OrderBy: "Name ASC"
        /// 
        /// To search for each property of the Role and User model, use the searchTerm and PropertyName properties.
        /// PropertyName - the name of the property that will be searched for. SearchTerm - the string that will be used to search for matches.
        /// The property name must be capitalized:
        /// 
        ///     SearchTerm: "user",
        ///     PropertyName: "Name"
        ///
        /// For the Role model, the property name must be written together with the object name:
        /// 
        ///     SearchTerm: "support",
        ///     PropertyName: "Role.RoleName"
        /// 
        /// Example:
        /// 
        ///     GET "api/User?CurrentPage=1&amp;PageSize=10&amp;OrderBy=Name%20ASC&amp;SearchTerm=User&amp;PropertyName=Name"
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> GetUserPage([FromQuery]RequestParameters parameters)
        {
            return Ok(await _userRepository.GetUserPage(parameters));
        }

        /// <summary>
        /// Gets user by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            return Ok(await _userRepository.GetUserById(id));
        }

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <remarks>
        /// Age should be greater than zero.
        /// 
        /// Name should contain at least 1 character.
        /// 
        /// Password should contain at least 6 character.
        /// 
        /// For example:
        /// 
        ///     POST api/User/CreateUser
        ///     {        
        ///       "name": "Mike",
        ///       "password": "123456",
        ///       "age": 20
        ///       "email": "Mike.Andrew@gmail.com"
        ///     }
        /// </remarks>
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

        /// <summary>
        /// Adds a new role for the user.
        /// </summary>
        /// <remarks>
        /// To add a role, add the ID of the user to whom you want to add the role and the name of the role.
        /// Possible roles:"User","Support","Admin",SuperAdmin. Role names should be capitalized.
        /// 
        /// For example:
        /// 
        ///     POST api/User/AddUserRole
        ///     {
        ///       "userId": 3,
        ///       "roleName": "Support"
        ///     }
        /// </remarks>
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

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <remarks>
        /// Age should be greater than zero.
        /// 
        /// Name should contain at least 1 character.
        /// 
        /// Password should contain at least 6 character.
        /// 
        /// For example:
        /// 
        ///     PUT api/User
        ///     {        
        ///       "name": "Mike",
        ///       "password": "123456",
        ///       "age": 20
        ///       "email": "Mike.Andrew@gmail.com"
        ///     }
        /// </remarks>
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

        /// <summary>
        /// Deletes the user by ID.
        /// </summary>
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
