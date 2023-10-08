using AutoMapper;
using TestAPI.Models;
using TestAPI.Shared.DTOs;

namespace TestAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UpdateUserDTO, User>();
            CreateMap<CreateUserDTO, User>();
            CreateMap<CreateUserDTO, UserDTO>();
            CreateMap<UpdateUserDTO, UserDTO>();

        }
    }
}
