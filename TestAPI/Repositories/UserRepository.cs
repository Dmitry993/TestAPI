using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestAPI.Context;
using TestAPI.DTOs;
using TestAPI.Interfaces;
using TestAPI.Models;
using TestAPI.Shared;
using TestAPI.Shared.Parameters;

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

        public async Task DeleteUser(int id)
        {
            var user = _context.Users.Attach(new User { Id = id });
            user.State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<UserDTO>(user);
        }

        public Task<PaginatedResult<User>> GetUserPage(RequestParameters parameters)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO> UpdateUser(UpdateUserDTO userDto)
        {
            var userDb = await _context.Users.FindAsync(userDto.Id);

            if (userDb is not null) 
            {
                _context.Entry(userDb).CurrentValues.SetValues(userDto);
                await _context.SaveChangesAsync();
                return _mapper.Map<UserDTO>(userDb);
            }

            return null;
        }
    }
}
