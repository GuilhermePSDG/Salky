using AutoMapper;
using Salky.App.Dtos;
using Salky.App.Dtos.Group;
using Salky.App.Dtos.Message;
using Salky.App.Dtos.Users;
using Salky.App.Services;
using Salky.Domain.Models.FriendModels;
using Salky.Domain.Models.GroupModels;
using Salky.Domain.Models.UserModels;
using Salky.Domain.Salky.Domain;

namespace Salky.App.Mapping
{
    public class SalkyMapping : Profile
    {
       
        public SalkyMapping()
        {
            CreateMap<User, UserMinimalDto>()
                .ForMember(x => x.PictureSource, q => q.MapFrom(x => ImageServiceConfiguration.CreateExternalLink(x.PictureSource)));

            CreateMap<User, UserRegisterDto>();
            CreateMap<User, UserSearchDto>();
            CreateMap<User, UserLoggedDto>()
                .ForMember(x => x.PictureSource,q => q.MapFrom(x => ImageServiceConfiguration.CreateExternalLink(x.PictureSource)));


            CreateMap<Group,GroupDto>()
                .ForMember(x => x.PictureSource,q => q.MapFrom(x => ImageServiceConfiguration.CreateExternalLink(x.PictureSource)))
                .ReverseMap();
            CreateMap<Friend,UserFriendDto>()
                .ForMember(q => q.FriendFlag, q => q.MapFrom(n => n.FriendRequestFlag))
                .ReverseMap();
            CreateMap<GroupConfigDto,GroupConfig>().ReverseMap();

            CreateMap<MessageGroup,MessageDto>()
                .ForMember(q => q.PartialContents, q => q.MapFrom(n => new MessagePartialContentService().Generate(n.Content)))
                .ForMember(f=>f.SendedAt,q => q.MapFrom(n => n.CreatedDate))
                .ForMember(f=>f.Author, q => q.MapFrom(n => n.Sender))
                .ReverseMap();

            CreateMap<GroupMember,GroupMemberDto>()
                .ForMember(x => x.RoleName,q => q.MapFrom(x => x.Role.RoleName))
                .ForMember(x => x.UserName,q => q.MapFrom(x => x.User.UserName))
                .ForMember(x => x.PictureSource, q => q.MapFrom(x => ImageServiceConfiguration.CreateExternalLink(x.User.PictureSource)))
                .ReverseMap();
            CreateMap<GroupMember, GroupMemberRolesDto>()
                .ForMember(x => x.GroupRole,q => q.MapFrom(n=>n.Role))
                .ForMember(x => x.PictureSource, q => q.MapFrom(x => ImageServiceConfiguration.CreateExternalLink(x.User.PictureSource)))
                .ForMember(x => x.GroupRole, q => q.MapFrom(x => x.Role))
                .ForMember(x => x.RoleName, q => q.MapFrom(x => x.Role.RoleName))
                .ForMember(x => x.UserName, q => q.MapFrom(x => x.User.UserName));
          
        }

    }
}
