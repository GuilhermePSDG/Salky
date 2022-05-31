using AutoMapper;
using Salky.App.Dtos.Users;
using Salky.Domain.Contracts;
using Microsoft.Extensions.Logging;

namespace Salky.App.Services.User
{
    public class UserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository userRepo;
        private readonly ILogger<UserService> log;
        private readonly IDispatcher dispatcher;
        private readonly ImageService imageService;

        public UserService(
            IMapper mapper,
            IUserRepository userRepo,
            ILogger<UserService> log,
            IDispatcher dispatcher,
            ImageService imageService
            )
        {
            _mapper = mapper;
            this.userRepo = userRepo;
            this.log = log;
            this.dispatcher = dispatcher;
            this.imageService = imageService;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="GroupId"></param>
        /// <param name="Base64Picture"></param>
        /// <returns>The relative picture path or <see langword="null"/> when not allowed or not found</returns>
        public async Task<string?> ChangePictureUsingBase64(Guid UserId, string Base64Picture)
        {
            var user = await userRepo.GetById(UserId, false);
            if (user == null) return null;
            var oldPath = new string(user.PictureSource);
            var picturePath = imageService.SaveBase64Image(Base64Picture);

            try
            {
                user.ChangePicture(picturePath, out var @event);
                userRepo.Update(user);
                await userRepo.EnsureSaveChangesAsync();
                this.dispatcher.Raise(@event);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Erro ao modificar a foto do usuario.");
                imageService.TryDeleteImage(picturePath);
                throw;
            }
            if (!imageService.TryDeleteImage(oldPath))
            {
                log.LogWarning($"Unable to delete image, path : {oldPath}");
            }
            //dispatcher.Raise(new UserPictureChanged(user.Id, user.PictureSource));
            return picturePath;

        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            var user = await userRepo.GetById(userId, false);
            if (user == null) return false;
            userRepo.Remove(user);
            return await userRepo.EnsureSaveChangesAsync() > 0;
        }

        public async Task<List<UserSearchDto>> SearchUsersByName(string name)
        {
            var users = await userRepo.FindUsersByName(userName: name);
            return _mapper.Map<List<UserSearchDto>>(users);
        }

        public async Task<UserMinimalDto?> GetUserByName(string name)
        {
            var user = await userRepo.GetUserByName(userName: name);
            if (user == null) return null;
            return _mapper.Map<UserMinimalDto>(user);
        }
        public async Task<UserMinimalDto?> GetUserById(Guid userId)
        {
            var user = await userRepo.GetById(userId, true);
            if (user == null) return null;
            return _mapper.Map<UserMinimalDto>(user);
        }

        public async Task<UserMinimalDto?> UpdateUser(Guid userId, UserUpdateDto userDTO)
        {
            var user = await userRepo.GetById(userId, false);
            if (user == null) return null;
            user = _mapper.Map(userDTO, user);
            userRepo.Update(user);
            if (await userRepo.EnsureSaveChangesAsync() == 0) return null;
            return _mapper.Map<UserMinimalDto>(user);
        }

    }

}
