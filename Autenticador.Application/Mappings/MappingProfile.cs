using Autenticador.Application.Features.Users;
using Autenticador.Application.Features.Users.Create;
using Autenticador.Application.Features.Users.CreateUser;
using Autenticador.Domain.Entities;
using AutoMapper;

namespace Autenticador.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponse>();

            CreateMap<User, UserWithRolesResponse>()
                .ForMember(dest => dest.Roles,
                    opt => opt.MapFrom(src =>
                    src.UserRoles.Select(ur => ur.Role.Name).ToList()));

            CreateMap<RegisterUserCommand, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<CreateUserAdminCommand, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
