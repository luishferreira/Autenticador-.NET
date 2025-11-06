using Autenticador.Application.Features.Users;
using Autenticador.Application.Features.Users.Create;
using Autenticador.Domain.Entities;
using AutoMapper;

namespace Autenticador.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<CreateUserCommand, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
