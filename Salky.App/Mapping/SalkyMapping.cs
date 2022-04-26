using AutoMapper;
using Salky.App.Dtos.Auth;
using Salky.App.Dtos.Contact;
using Salky.App.Dtos.Message;
using Salky.App.Dtos.Users;
using Salky.Domain;
using Salky.Domain.Salky.Domain;

namespace Salky.App.Mapping
{
    public class SalkyMapping : Profile
    {
        public SalkyMapping()
        {
            CreateMap<UserRegisterDto, User>().ReverseMap();
            CreateMap<UserLoggedDto, User>().ReverseMap();

            CreateMap<Contato, ContactDto>()
               .ForMember(x => x.PictureSource, q => q.MapFrom(src => src.UserContact.PictureSource))
               .ForMember(x => x.UserName, q => q.MapFrom(src => src.UserContact.UserName))
               .ForMember(x => x.ContactId, q => q.MapFrom(src => src.Id))
               .ForMember(f => f.UserContactId, q => q.MapFrom(src => src.UserContactId))
               .ReverseMap();
            CreateMap<User, ContactDto>()
                .ForMember(x=>x.ContactId,q => q.MapFrom(src => Guid.Empty))
                .ReverseMap();
            CreateMap<User, Dtos.Users.UserDto>().ReverseMap();
            CreateMap<MessageDto,Message>()
                .ForMember(f=>f.CreatedDate,q => q.MapFrom(n => n.SendedAt))
                .ReverseMap();
        }
    }
}
