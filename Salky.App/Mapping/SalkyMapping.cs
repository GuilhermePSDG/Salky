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
            CreateMap<UserRegisterDto, User>().ReverseMap();
            CreateMap<UserSearchDto, User>().ReverseMap();
            CreateMap<UserLoggedDto, User>().ReverseMap();
            CreateMap<UserUpdateDto, User>().ReverseMap();
            CreateMap<GroupSearchDto, Group>().ReverseMap();
            CreateMap<User, UserMinimalDto>().ReverseMap();
            CreateMap<Group,GroupDto>().ReverseMap();
            CreateMap<Friend,UserFriendDto>()
                .ForMember(q => q.FriendFlag, q => q.MapFrom(n => n.FriendRequestFlag))
                .ReverseMap();
            CreateMap<GroupConfigDto,GroupConfig>().ReverseMap();

            CreateMap<MessageGroup,MessageDto>()
                .ForMember(q => q.Embeds, q => q.MapFrom(n => PartialContentService.CreateEmbeds(n.Content)))
                .ForMember(f=>f.SendedAt,q => q.MapFrom(n => n.CreatedDate))
                .ForMember(f=>f.Author, q => q.MapFrom(n => n.Sender))
                .ReverseMap();

            CreateMap<GroupMember,GroupMemberDto>()
                .ForMember(x => x.RoleName,q => q.MapFrom(x => x.Role.RoleName))
                .ForMember(x => x.UserName,q => q.MapFrom(x => x.User.UserName))
                .ForMember(x => x.PictureSource,q => q.MapFrom(x => x.User.PictureSource))
                .ReverseMap();
            CreateMap<GroupMember, GroupMemberRolesDto>()
                .ForMember(x => x.GroupRole,q => q.MapFrom(n=>n.Role))
                .ForMember(x => x.GroupRole, q => q.MapFrom(x => x.Role))
                .ForMember(x => x.RoleName, q => q.MapFrom(x => x.Role.RoleName))
                .ForMember(x => x.UserName, q => q.MapFrom(x => x.User.UserName))
                .ForMember(x => x.PictureSource, q => q.MapFrom(x => x.User.PictureSource))
                .ReverseMap();
          

        }

    }
}
