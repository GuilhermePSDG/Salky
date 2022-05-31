using AutoMapper;
using Salky.App.Dtos.Message;
using Salky.Domain.Contracts;
using Salky.Domain.Models.GenericsModels;
using Salky.Domain.Models.GroupModels;

namespace Salky.App.Services.Group
{
    public class GroupMessageService
    {
        private readonly IMapper mapper;
        private readonly IMessageGroupRepository messageRepository;
        private readonly IGroupRepository groupRepository;
        private readonly IGroupMemberRepository memberRepo;
        private readonly IDispatcher dispatcher;

        public GroupMessageService(
            IMapper mapper,
            IMessageGroupRepository messageRepository,
            IGroupRepository groupRepository,
            IGroupMemberRepository memberRepo,
            IDispatcher dispatcher
            )
        {
            this.mapper = mapper;
            this.messageRepository = messageRepository;
            this.groupRepository = groupRepository;
            this.memberRepo = memberRepo;
            this.dispatcher = dispatcher;
        }

        public async Task<PaginationResult<MessageDto>?> GetMessagesOfGroup(Guid currentUser, Guid groupId, int currentPage, int pageSize)
        {
            if (!await memberRepo.HasMemberByUserId(currentUser, groupId)) return null;
            var msgs = await messageRepository.GetByGroupId(groupId, currentPage, pageSize);
            return msgs.CastTo(mapper.Map<MessageDto>);
        }

        public async Task<MessageDto?> AddMessage(Guid ownerId, Guid groupId, string content)
        {
            if (!await memberRepo.HasMemberByUserId(ownerId, groupId)) return null;
            var msg = new MessageGroup()
            {
                Content = content,
                GroupId = groupId,
                SenderId = ownerId,
            };
            messageRepository.Add(msg);
            await messageRepository.EnsureSaveChangesAsync();
            msg = await messageRepository.GetById(msg.Id);
            return mapper.Map<MessageDto>(msg);
        }

        public async Task<MessageDto?> RemoveMessage(Guid userId, Guid messageId)
        {
            var message = await messageRepository.GetById(messageId);
            if (message == null) return null;
            var groupMember = await memberRepo.GetMemberByUserIdWithRole(userId, message.GroupId);
            if (groupMember == null) return null;
            var group = await groupRepository.GetGroupByIdWithRolesAndConfig(groupMember.GroupId);
            if (group == null) return null;
            if (group.MemberCanDeleteMessage(groupMember, message, out var @event))
            {
                messageRepository.Remove(message);
                await messageRepository.EnsureSaveChangesAsync();
                dispatcher.Raise(@event);
                return mapper.Map<MessageDto>(message);
            }
            return null;

        }

    }
}
