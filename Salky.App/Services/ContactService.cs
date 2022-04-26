using AutoMapper;
using Salky.App.Dtos.Contact;
using Salky.Persistence.Persist;


namespace Salky.App.Services
{

    public class ContactService
    {
        public Guid Id = Guid.NewGuid();
        private readonly ContactRepository contactRepo;
        private readonly UserRepository userRepo;
        private readonly IMapper mapper;

        public ContactService(ContactRepository contactRepo, UserRepository userRepo, IMapper mapper)
        {
            this.contactRepo = contactRepo;
            this.userRepo = userRepo;
            this.mapper = mapper;
        }

        public async Task<List<ContactDto>> GetAllAsync(Guid userId)
        {
            var res = await contactRepo.GetAllByUserId(userId, true);
            return mapper.Map<List<ContactDto>>(res);
        }


        public async Task<ContactDto> AddContactByUserNameAsync(Guid userId, string contactUserName)
        {
            //Procura o usuario pelo username e verifica se é nulo.
            var userContact = (await userRepo.GetUserByName(contactUserName))
                .ThrowIfNull("Usuario não encontrado");
            //Verifica se o Id do usuario é o mesmo do usuario atual
            if (userContact.Id.Equals(userId)) 
                throw new Exception("Usuario não pode se adicionar.");
            //Verifica se o usuario atual já possui este contato
            if (await contactRepo.HasContact(userId, userContact.Id)) 
                throw new Exception("Usuario já possui este contato");
            //Caso tenha passado...
            //Adiciona o contato.
            contactRepo.AddContact(userId, userContact.Id);
            //Tenta salvar.
            if (await contactRepo.SaveChangesAsync() < 0)
                throw new Exception("Não foi possivel salvar o contato");
            //Recupera o contato
            var contact = (await contactRepo.GetContactByUsersIds(userId, userContact.Id, true))
                .ThrowIfNull("Não foi possivel recuperar o contato após adicionar.");
            //Mapeia
            return mapper.Map<ContactDto>(contact);
        }

        public async Task<ContactDto> GetUserByName(string UserName)
        {
            var user = await this.userRepo.GetUserByName(UserName, true);
            var contactDto = this.mapper.Map<ContactDto>(user.ThrowIfNull("Usuario não encontrado"));
            return contactDto;
        }

    }
}
