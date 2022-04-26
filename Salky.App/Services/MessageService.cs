using AutoMapper;
using Salky.App.Dtos.Message;
using Salky.Domain;
using Salky.Domain.Salky.Domain;
using Salky.Persistence.Persist;

namespace Salky.App.Services
{
    public class MessageService
    {
        private readonly IMapper mapper;
        private readonly ContactRepository contactRepo;
        private readonly MessageRepository messageRepository;

        public MessageService(IMapper mapper, ContactRepository ContactRepository, MessageRepository messageRepository)
        {
            this.mapper = mapper;
            this.contactRepo = ContactRepository;
            this.messageRepository = messageRepository;
        }

            
        public async Task<List<MessageDto>> GetMessagesByContactId(Guid currentUser,Guid contactId)
        {
            var msgs = await messageRepository.GetByContactId(currentUser,contactId);
            return mapper.Map<List<MessageDto>>(msgs);
        }

        public async Task UpdateMessageStatus(Guid currentUserId,Guid messageId,MessageStatus newStatus)
        {
            var msg =(await this.messageRepository.GetByMessageId(currentUserId, messageId)).ThrowIfNull("Mensagem não encontrada");
            msg.Status = newStatus;
            this.messageRepository.Update(msg);
            await this.messageRepository.SaveChangesAsync();
        }
        public record class Messages(MessageDto MessageSender, MessageDto MessageReceiver);
        public async Task<Messages> AdicionarMensagemParaAmbos(Guid ownerId,Guid contactId,string content)
        {
            //Recupera o contato passado via parametro
            //O dono do contato deve ser igual ao ownerId, que é o usuario que está logado
            var senderContact = 
                (await this.contactRepo.GetById(ownerId,contactId,false))
                .ThrowIfNull("Contato não encontrado.");
            //Recupera o contato que irá receber a mensagem
            var receiverContact =
                (await this.contactRepo.GetContactByUsersIds(senderContact.UserContactId,ownerId,false))
                .ThrowIfNull("Usuario que iria receber a mensagem, não possui o usuario que a enviou como amigo.");
            
            var messageSender = new Message()
            {
                ContatoId = senderContact.Id,
                UserSenderId = ownerId,
                UserReceiverId = senderContact.UserContactId,
                Content = content,
            };
            var messageReceiver = new Message()
            {
                ContatoId = receiverContact.Id,
                UserSenderId = ownerId,
                UserReceiverId = senderContact.UserContactId,
                Content = content,
            };
            messageRepository.Add(messageSender);
            messageRepository.Add(messageReceiver);

            if(!(await messageRepository.SaveChangesAsync() > 0)) throw new Exception("Cannot save");
            return new(
                this.mapper.Map<MessageDto>(messageSender),
                this.mapper.Map<MessageDto>(messageReceiver)
                );
        }


    }
}
