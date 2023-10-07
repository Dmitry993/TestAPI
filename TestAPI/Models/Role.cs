namespace TestAPI.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
