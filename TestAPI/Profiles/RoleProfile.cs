using AutoMapper;
using TestAPI.Models;
using TestAPI.Shared.DTOs;

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
