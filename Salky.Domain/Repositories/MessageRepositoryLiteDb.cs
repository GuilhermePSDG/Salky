using LiteDB;
using Microsoft.EntityFrameworkCore;
using Salky.Domain.Contexts;
using Salky.Domain.Contracts;
using Salky.Domain.Exceptions;
using Salky.Domain.Models.GenericsModels;
using Salky.Domain.Models.GroupModels;
using Salky.Domain.Salky.Domain;

namespace Salky.Domain.Repositories
{
    public class MessageRepositoryLiteDb : IRepositoryBase<MessageGroup>, IMessageGroupRepository
    {
        public const string ConnectionString = "MessageLite.db";
        public IUserRepository UserRepo { get; }

        public MessageRepositoryLiteDb(IUserRepository userRepo)
        {
            UserRepo = userRepo;
            using (var db = new LiteDatabase(ConnectionString))
            {
                var col = db.GetCollection<MessageGroup>();
                col.EnsureIndex(x => x.Id);
                col.EnsureIndex(x => x.GroupId);
                col.EnsureIndex(x => x.SenderId);
            }
        }
        public async Task<PaginationResult<MessageGroup>> GetByGroupId(Guid groupId, int currentPage, int pageSize)
        {
            using(var db = new LiteDatabase(ConnectionString))
            {
                var col = db.GetCollection<MessageGroup>();
                var count = col.Count(x => x.GroupId == groupId);
                var pagination = new PaginationResult<MessageGroup>(currentPage, pageSize, count);
                var res = col.Find(msg => msg.GroupId == groupId, ((pagination.CurrentPage - 1) * pagination.PageSize), pagination.PageSize).ToList();
                IncludeSender(res);
                pagination.SetResults(res);
                return await Task.FromResult(pagination);
            }
        }
        private void IncludeSender(List<MessageGroup> msg)
        {
            msg.GroupBy(q => q.SenderId).ToList().ForEach(async group =>
            {
                var user = await this.UserRepo.GetById(group.Key, false);
                foreach (var msg in group)
                {
                    msg.Sender = user;
                }
            });
        }
        private async Task IncludeSender(MessageGroup msg)
        {
            var user = await this.UserRepo.GetById(msg.SenderId, false);
            msg.Sender = user;
        }

        public async Task<MessageGroup?> GetById(Guid id)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var col = db.GetCollection<MessageGroup>();
                var msg = col.FindById(id);
                await IncludeSender(msg);
                return await Task.FromResult(msg);
            }
        }

        public void Add(MessageGroup entity)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var col = db.GetCollection<MessageGroup>();
                col.Insert(entity);
                ChangesCount++;
            }
        }

        public  void Remove(MessageGroup entity)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var col = db.GetCollection<MessageGroup>();
                col.Delete(entity.Id);
                ChangesCount++;
            }
        }
        public void Update(MessageGroup entity)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var col = db.GetCollection<MessageGroup>();
                col.Update(entity);
                ChangesCount++;
            }
        }
        private int ChangesCount = 0;


        public async Task<int> SaveChangesAsync()
        {
            var oldCount = ChangesCount;
            ChangesCount = 0;
            return await Task.FromResult(oldCount);
        }

        public async Task<int> EnsureSaveChangesAsync()
        {
            var c = await this.SaveChangesAsync();
            if (c == 0) throw new UnableToSaveChangesException();
            return c;
        }
    }
}
