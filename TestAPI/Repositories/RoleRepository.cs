using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestAPI.Context;
using TestAPI.Interfaces;
using TestAPI.Models;
using TestAPI.Shared.DTOs;

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
            var roleDb = _mapper.Map<Role>(roleDto);
            var isUserRoleExist = await _context.Roles
                .AnyAsync(role => role.RoleName == roleDb.RoleName && role.UserId == roleDb.UserId);

            if (isUserRoleExist)
            {
               return _mapper.Map<RoleDTO>(roleDb);
            }

            await _context.Roles.AddAsync(roleDb);
            await _context.SaveChangesAsync();

            return _mapper.Map<RoleDTO>(roleDb);
        }
    }
}
