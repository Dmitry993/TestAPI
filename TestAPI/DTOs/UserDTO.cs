using TestAPI.Models;

namespace TestAPI.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }

        public IReadOnlyList<RoleDTO> Roles { get; set; }
    }
}
