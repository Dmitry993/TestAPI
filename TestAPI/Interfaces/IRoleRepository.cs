using TestAPI.Shared.DTOs;

namespace TestAPI.Interfaces
{
    public interface IRoleRepository
    {
        Task<RoleDTO> AddRoleToUser(AddRoleDTO roleDto);
    }
}
