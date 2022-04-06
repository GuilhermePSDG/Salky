using Microsoft.Extensions.DependencyInjection;
using System;
using WebSocket.Shared.DataAcess;
using WebSocket.Shared.DataAcess.Models;

namespace Wpf
{
    public class RepositoryFactory
    {
        private IServiceProvider provider;

        public RepositoryFactory(IServiceProvider serviceProvider)
        {
            this.provider = serviceProvider;
        }
        public  IRepository<T> GetRepository<T>() where T : BaseEntity
        {
            return this.provider.GetRequiredService<IRepository<T>>() ?? throw new("Repository do not exist");
        }
    }

}
