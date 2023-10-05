using TestAPI.DTOs;
using TestAPI.Models;
using TestAPI.Shared;
using TestAPI.Shared.Parameters;

namespace TestAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDTO> GetUserById(int id);
        Task<UserDTO> CreateUser(CreateUserDTO user);
        Task<UserDTO> UpdateUser(UpdateUserDTO user);
        Task<PaginatedResult<UserDTO>> GetUserPage(RequestParameters parameters);
        Task DeleteUser(int id);
    }
}
