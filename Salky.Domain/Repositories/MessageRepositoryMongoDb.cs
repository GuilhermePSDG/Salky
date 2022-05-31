using LiteDB;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Salky.Domain.Contexts;
using Salky.Domain.Contracts;
using Salky.Domain.Exceptions;
using Salky.Domain.Models.GenericsModels;
using Salky.Domain.Models.GroupModels;
using Salky.Domain.Salky.Domain;

namespace Salky.Domain.Repositories
{
    public class MessageRepositoryMongoDb : IRepositoryBase<MessageGroup>, IMessageGroupRepository
    {
        public const string DataBaseName = "SalkyMongoDb";
        public const string CollectionName = "MessageGroup";
        private MongoClient client;
        private readonly IMongoDatabase db;
        private IMongoCollection<MessageGroup> collection;
        private int ChangesCount = 0;

        public IUserRepository UserRepo { get; }

        public MessageRepositoryMongoDb(IUserRepository UserRepo,MongoClient client)
        {
            this.client = client;
            this.db = this.client.GetDatabase(DataBaseName);
            this.collection = db.GetCollection<MessageGroup>(CollectionName);
            this.UserRepo = UserRepo;
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

        public async Task<PaginationResult<MessageGroup>> GetByGroupId(Guid groupId, int currentPage, int pageSize)
        {

            try
            {
                var count = await collection.CountDocumentsAsync(x => x.GroupId == groupId);
                var pagination = new PaginationResult<MessageGroup>(currentPage, pageSize, (int)count);
                var res = await collection
                    .Find(msg => msg.GroupId == groupId)
                    .Skip((pagination.CurrentPage - 1) * pagination.PageSize)
                    .Limit(pagination.PageSize)
                    .ToListAsync();
                IncludeSender(res);
                pagination.SetResults(res);
                return pagination;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<MessageGroup?> GetById(Guid id)
        {
            var result = await collection.Find(x => x.Id == id).Limit(1).FirstOrDefaultAsync();
            await IncludeSender(result);
            return result;
        }

        public void Add(MessageGroup entity)
        {
            this.collection.InsertOne(entity);
            ChangesCount++;
        }
        public void Remove(MessageGroup entity)
        {
            this.collection.DeleteOne<MessageGroup>(x => x.Id == entity.Id);
            ChangesCount++;
        }
        public void Update(MessageGroup entity)
        {
            this.collection.FindOneAndReplace(x => x.Id == entity.Id, entity);
            ChangesCount++;
        }

        public async Task<int> SaveChangesAsync()
        {
            var oldC = ChangesCount;
            ChangesCount = 0;
            return await Task.FromResult(oldC);
        }

        public async Task<int> EnsureSaveChangesAsync()
        {
            var c = await this.SaveChangesAsync();
            if(c == 0) throw new UnableToSaveChangesException();
            return c;
        }
    }
}
