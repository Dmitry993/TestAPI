using AutoMapper;
using TestAPI.DTOs;
using TestAPI.Models;

namespace TestAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UpdateUserDTO, User>();
            CreateMap<CreateUserDTO, User>();
        }
    }
}
