using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;
using Salky.WebSocket.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSocket
{


    public class RedisHandlerTest : IPoolMannager
    {
        public string ConnectionPoolId => throw new NotImplementedException();
       
        public void AddSocket(Key Key, SalkyWebSocket ws)
        {
            //1 - Verificar no redis se já não tem essa chave
            //     caso exista, vai lançar uma exceção
            //     também será preciso enviar uma ordem para desconectar
            //     a outra instancia
            //2 - Adicionar o socket em tabela hash
            //3 - Adicionar a referencia do usuario
            //     com a pool no redis
            var db = connection.GetDatabase();
            if(db.SetContains($"connections:", Key.ToString()))
            {

            }
            
            throw new NotImplementedException();
        }
        public int AddManyInPool(Key PoolKey, Key[] ClientsKey)
        {
            throw new NotImplementedException();
        }
        public bool AddOneInPool(Key PoolKey, Key ClientKey)
        {
            throw new NotImplementedException();
        }
        public bool ContainsSocketKey(Key Key)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<KeyValuePair<string, Storage>> GetStorageOfManyInPool(Key PoolKey)
        {
            throw new NotImplementedException();
        }
        public int RemoveAllFromPool(Key PoolKey)
        {
            throw new NotImplementedException();
        }
        public int RemoveOneFromPool(Key PoolKey, Key ClientKey)
        {
            throw new NotImplementedException();
        }
        public Task<int> SendToAllInPool(Key PoolKey, MessageServer message)
        {
            throw new NotImplementedException();
        }
        public Task<bool> SendToOneInPool(Key PoolKey, Key ClientKey, MessageServer message)
        {
            throw new NotImplementedException();
        }
        public bool TryGetSocket(Key Key, [MaybeNullWhen(false)] out SalkyWebSocket ws)
        {
            throw new NotImplementedException();
        }
        public bool TryRemoveSocket(Key Key, [MaybeNullWhen(false)] out SalkyWebSocket ws)
        {
            throw new NotImplementedException();
        }

    
      
        private ConnectionMultiplexer connection;
        public RedisHandlerTest()
        {
            this.connection = ConnectionMultiplexer.Connect("localhost:6379");

        }
        

        public async Task SendMessage(MessageServer msg, string channel)
        {
            var db = this.connection.GetDatabase();
            await db.PublishAsync(channel, JsonSerializer.Serialize(msg));
        }



        public async Task AddListener(Action<MessageServer> Handler, string channel, string ConnectionIndentifier)
        {
            var sub = connection.GetSubscriber();

            await sub.SubscribeAsync(channel, (channel, value) =>
            {
                try
                {
                    var msg = JsonSerializer.Deserialize<MessageServer>(value.ToString()) ?? throw new NullReferenceException();
                    Handler(msg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            });
        }

    }
}
