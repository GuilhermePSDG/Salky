using AutoMapper;
using Microsoft.Extensions.Logging;
using Salky.App.Dtos.Group;
using Salky.Domain.Contracts;
using Salky.Domain.Models.GroupModels;

namespace Salky.App.Services.Group
{
    public class GroupMemberService
    {
        private readonly IGroupMemberRepository memberRepo;
        private readonly IMapper mapper;
        private readonly IGroupRepository groupRepository;
        private readonly ILogger<GroupMemberService> log;
        private readonly IFriendRepository friendRepo;
        private readonly IDispatcher dispatcher;

        public GroupMemberService(
            IGroupMemberRepository memberRepo,
            IMapper mapper,
            IGroupRepository groupRepository,
            ILogger<GroupMemberService> log,
            IFriendRepository friendRepo,
            IDispatcher dispatcher
            )
        {
            this.memberRepo = memberRepo;
            this.mapper = mapper;
            this.groupRepository = groupRepository;
            this.log = log;
            this.friendRepo = friendRepo;
            this.dispatcher = dispatcher;
        }

        public async Task<List<GroupMemberDto>> GetAllMembersOfUser(Guid UserId)
        {
            var members = await memberRepo.GetAllMembersOfUser(UserId);
            return mapper.Map<List<GroupMemberDto>>(members);
        }

        public async Task<bool> UserMakePartOfGroup(Guid UserId, Guid GroupId)
        {
            return await memberRepo.HasMemberByUserId(UserId, GroupId);
        }

        public async Task<List<GroupMemberDto>?> GetMembersOfGroup(Guid UserId, Guid GroupId)
        {
            if (!await UserMakePartOfGroup(UserId, GroupId)) return null;
            var result = await memberRepo.GetMembers(GroupId);
            return mapper.Map<List<GroupMemberDto>>(result);
        }

        public async Task<GroupMemberRolesDto?> GetMemberWithRole(Guid UserId, Guid GroupId)
        {
            if (!await UserMakePartOfGroup(UserId, GroupId)) return null;
            var result = await memberRepo.GetMemberByUserIdWithRole(UserId, GroupId);
            return mapper.Map<GroupMemberRolesDto>(result);
        }


        public async Task<GroupMemberDto?> AddNewMemberByFriendId(Guid CurrentUserId, Guid FriendId, Guid GroupId)
        {
            var friend = await friendRepo.GetById(CurrentUserId, FriendId, true);
            if (friend == null) return null;
            if (friend.CanInteractBetween())
            {
                var usr = friend.GetUserOfFriendDiferentOf(CurrentUserId);
                return await AddNewMember(CurrentUserId, usr.Id, GroupId);
            }
            return null;
        }
       
        private async Task<GroupMemberDto?> AddNewMember(Guid CurrentUserId, Guid UserToAddInGroupId, Guid GroupId)
        {
            var group = await groupRepository.GetGroupByIdWithMembersWithTracking(GroupId);
            if (group == null) return null;
            if (group.TryAddNewMember(CurrentUserId, UserToAddInGroupId, out var @event, out var newMember))
            {
                await groupRepository.EnsureSaveChangesAsync();
                var memberToReturn = await memberRepo.GetMemberByUserId(UserToAddInGroupId, GroupId) ?? throw new NullReferenceException();
                dispatcher.Raise(@event);
                return mapper.Map<GroupMemberDto>(memberToReturn);
            }
            return null;
        }

        public async Task<GroupMemberDto?> RemoveGroupMember(Guid UserId, Guid MemberToRemoveId)
        {
            var MemberToRemove = await memberRepo.GetMemberById(MemberToRemoveId);
            if (MemberToRemove == null) return null;
            var group = await groupRepository.GetGroupByIdWithMembersWithTracking(MemberToRemove.GroupId) ?? throw new InvalidOperationException($"A {nameof(GroupMember)} cannot exist without a {nameof(Group)}");
            if(group.TryRemoveMember(UserId,MemberToRemoveId,out var @event))
            {
                await groupRepository.EnsureSaveChangesAsync();
                dispatcher.Raise(@event);
                return mapper.Map<GroupMemberDto>(MemberToRemove);
            }
            return null;
        }
       


    }
}
