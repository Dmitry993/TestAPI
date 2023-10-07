using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestAPI.Context;
using TestAPI.DTOs;
using TestAPI.Interfaces;
using TestAPI.Models;

namespace TestAPI.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public RoleRepository(IMapper mapper, ApplicationContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<RoleDTO> AddRoleToUser(AddRoleDTO roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            var isUserRoleExist = await _context.Roles
                .AnyAsync(role => role.RoleName == role.RoleName && role.UserId == role.UserId);

            if (isUserRoleExist)
            {
               return _mapper.Map<RoleDTO>(role);
            }

            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();

            return _mapper.Map<RoleDTO>(role);
        }
    }
}
