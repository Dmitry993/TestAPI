using AutoMapper;
using TestAPI.DTOs;
using TestAPI.Models;

namespace TestAPI.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<AddRoleDTO, Role>();
        }
    }
}
