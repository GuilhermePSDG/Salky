using Salky.WebSocket.Infra.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSocket
{

    //Quando um webscoet enviar uma mensagem,
    //Ela terá 4 possiveis destinos
    //1 - Será consumida e nada de volta
    //2 - Será enviada de volta
    //3 - Será publicada no redis
    //E quando mandarem uma mensagem pelo redis
    //Ai tem que redirecionar pro route resolver
    public class RedisHandlerTest
    {
        private ConnectionMultiplexer connection;

        public RedisHandlerTest()
        {
            this.connection = ConnectionMultiplexer.Connect("localhost:6379");

        }

        public async Task SendMessage(MessageServer msg,string channel)
        {
            var db = this.connection.GetDatabase();
            await db.PublishAsync(channel, JsonSerializer.Serialize(msg));
        }


        public async Task AddListener(Action<MessageServer> Handler,string channel,string ConnectionIndentifier)
        {
            var sub = connection.GetSubscriber();

            await sub.SubscribeAsync(channel, (channel,value) =>
            {
                try
                {
                    var msg = JsonSerializer.Deserialize<MessageServer>(value.ToString()) ?? throw new NullReferenceException();
                    Handler(msg);                    
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            });
        }


        public async Task AddConnection(string ConnectionIndentifier)
        {
           
        }

    }
}
