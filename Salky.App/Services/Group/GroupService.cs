using AutoMapper;
using Microsoft.Extensions.Logging;
using Salky.App.Dtos.Group;
using Salky.Domain.Contracts;

namespace Salky.App.Services.Group
{

    public class GroupService
    {
        private readonly IGroupRepository groupRepository;
        private readonly IGroupMemberRepository memberRepo;
        private readonly IUserRepository userRepo;
        private readonly IMapper mapper;
        private readonly ILogger<GroupService> log;
        private readonly ImageService imageService;
        private readonly IDispatcher dispatcher;

        public GroupService(
            IGroupRepository groupRepository,
            IGroupMemberRepository memberRepo,
            IUserRepository userRepo,
            IMapper mapper,
            ILogger<GroupService> log,
            ImageService imageService,
            IDispatcher dispatcher
            )
        {
            this.groupRepository = groupRepository;
            this.memberRepo = memberRepo;
            this.userRepo = userRepo;
            this.mapper = mapper;
            this.log = log;
            this.imageService = imageService;
            this.dispatcher = dispatcher;
        }

        public async Task<List<GroupDto>> GetAllGroupsOfUser(Guid usrId)
        {
            log.LogInformation($"{nameof(GetAllGroupsOfUser)}");
            var result = await groupRepository.GetAllGroupsOfUser(usrId);
            return mapper.Map<List<GroupDto>>(result);
        }

        public async Task<GroupDto?> GetById(Guid UserId, Guid GroupId)
        {
            if (!await memberRepo.HasMemberByUserId(UserId, GroupId)) return null;
            var group = await groupRepository.GetGroupByIdWithRolesAndConfig(GroupId);
            if (group == null) return null;
            else return mapper.Map<GroupDto>(group);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="GroupId"></param>
        /// <param name="Base64Picture"></param>
        /// <returns>The relative picture path or <see langword="null"/> when not allowed or not found</returns>
        public async Task<string?> ChangeGroupPictureUsingBase64(Guid UserId, Guid GroupId, string Base64Picture)
        {
            var member = await memberRepo.GetMemberByUserIdWithRole(UserId, GroupId);
            if (member == null) return null;
            var group = await groupRepository.GetGroupById(GroupId);
            if (group == null) throw new InvalidOperationException("Member without group");
            var oldPath = new string(group.PictureSource);
            var picturePath = imageService.SaveBase64Image(Base64Picture);
            try
            {
                if (group.ChangePicture(member, picturePath, out var @event))
                {
                    groupRepository.Update(group);
                    await groupRepository.EnsureSaveChangesAsync();
                    if (!imageService.TryDeleteImage(oldPath)) log.LogWarning($"Unable to delete old image, path : {oldPath}");
                    dispatcher.Raise(@event);
                    return picturePath;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            catch
            {
                if (!imageService.TryDeleteImage(picturePath))
                {
                    log.LogWarning($"Unable to delete image, path : {oldPath}");
                }
                throw;
            }
            return null;
        }

        public async Task<bool> RemoveGroup(Guid UserId, Guid GroupId)
        {
            var group = await groupRepository.GetGroupById(GroupId);
            if (group == null) return false;
            var member = await memberRepo.GetMemberByUserId(UserId, GroupId);
            if (member == null) return false;
            if (group.MemberCanDeleteTheGroup(member, out var @event))
            {
                groupRepository.Remove(group);
                await groupRepository.EnsureSaveChangesAsync();
                dispatcher.Raise(@event);
                return true;
            }
            return false;
        }

        public async Task<GroupDto> CreateNewPublicGroup(Guid currentUserId, string GroupName)
        {
            var group = Domain.Models.GroupModels.Group.Create(currentUserId, GroupName, out var @event);
            groupRepository.Add(group);
            await groupRepository.EnsureSaveChangesAsync();
            group = await groupRepository.GetGroupByIdWithRolesAndConfig(group.Id);
            dispatcher.Raise(@event);
            return mapper.Map<GroupDto>(group);
        }

        public async Task<GroupDto?> ChangeGroupName(Guid UserId, Guid GroupId, string NewGroupName)
        {
            var group = await groupRepository.GetGroupByIdWithRolesAndConfig(GroupId);
            if (group == null) return null;
            if (group.ChangeName(UserId, NewGroupName, out var @event))
            {
                groupRepository.Update(group);
                await groupRepository.EnsureSaveChangesAsync();
                dispatcher.Raise(@event);
                return mapper.Map<GroupDto>(group);
            }
            return null;

        }

    }
}
