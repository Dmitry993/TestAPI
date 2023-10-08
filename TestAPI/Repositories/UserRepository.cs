using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestAPI.Context;
using TestAPI.Extensions;
using TestAPI.Interfaces;
using TestAPI.Models;
using TestAPI.Shared;
using TestAPI.Shared.Parameters;
using System.Linq.Dynamic.Core;
using TestAPI.Shared.DTOs;

namespace TestAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDTO> CreateUser(CreateUserDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.Roles = new List<Role>() { new Role { RoleName = "User" } };
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDTO>(user);
        }

        public async Task DeleteUser(int id)
        {
            var user = _context.Users.Attach(new User { Id = id });
            user.State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var user = await _context.Users.Include(user => user.Roles)
                                           .SingleOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<PaginatedResult<UserDTO>> GetUserPage(RequestParameters parameters)
        {
            var filteredData = _context.Users.Include(user => user.Roles)
                                               .Filter(parameters.SearchTerm, parameters.PropertyName)
                                               .OrderBy(parameters.OrderBy);

            var totalItems = filteredData.Count();
            var userPage = await filteredData.ToUserPageListAsync(parameters.CurrentPage, parameters.PageSize);

            return new PaginatedResult<UserDTO>
            {
                Items = _mapper.Map<List<UserDTO>>(userPage),
                TotalItems = totalItems,
                CurrentPage = parameters.CurrentPage,
                PageSize = parameters.PageSize
            };
        }

        public async Task<bool> IsLoginPasswordCorrect(LoginDTO login) => 
            await _context.Users.AnyAsync(user => user.Name == login.Username && user.Password == login.Password);

        public async Task<bool> IsUserExist(int userId) => await _context.Users.AnyAsync(user => user.Id == userId);

        public async Task<UserDTO> UpdateUser(UpdateUserDTO userDto)
        {
            var userDb = await _context.Users.FindAsync(userDto.Id);
            
            if (userDb is not null) 
            {
                var user = _mapper.Map<User>(userDto);
                _context.Entry(userDb).CurrentValues.SetValues(user);
                await _context.SaveChangesAsync();
                return _mapper.Map<UserDTO>(userDb);
            }

            return null;
        }
    }
}
