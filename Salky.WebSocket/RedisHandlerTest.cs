using Microsoft.Extensions.Logging;
using Salky.WebSocket.Exceptions;
using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;
using Salky.WebSocket.Models;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSocket
{
    public class RedisWebSocketMannager 
    {
        private ConcurrentDictionary<Key, SalkyWebSocket> wsConns = new ConcurrentDictionary<Key, SalkyWebSocket>();
        private ConnectionMultiplexer redis_conn;
        private IDatabase redis_db;
        private ISubscriber redis_sub;

        public ILogger<RedisWebSocketMannager> Logger { get; }

        public RedisWebSocketMannager(ILogger<RedisWebSocketMannager> logger, ConnectionMultiplexer connection)
        {
            this.redis_conn = connection;
            this.redis_db = redis_conn.GetDatabase();
            this.redis_sub = redis_conn.GetSubscriber();
            Logger = logger;
        }

        private RedisChannel GetDisconnectChannel(Key Key) => $"disconect:{Key}";

        //ESTÁ FUNÇÃO TERA QUE SER MODIFICADA NO CONTRATO
        // public bool TryDisconnectSocket(Key Key)
        public bool TryDisconnectSocket(Key Key)
        {
            if(this.wsConns.TryRemove(Key,out var rmWs))
            {
                rmWs.CloseOutputAsync(WebSocketCloseStatus.PolicyViolation,CloseDescription.DuplicatedConnection).Wait();
                var removed = redis_db.SetRemove("connections", Key.ToString());
                if (!removed) this.Logger.LogWarning("Connetion not removed from redis");
                redis_sub.Unsubscribe(GetDisconnectChannel(Key));
                return true;
            }
            if (HasRedisConnection(Key))
            {
                var receiversCount = redis_db.Publish(GetDisconnectChannel(Key),"");
                if(receiversCount == 0) this.Logger.LogWarning("No one listen for user disconnection..");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="ws"></param>
        /// <returns><see langword="true"/> if added , <see langword="false"/> if is alredy present</returns>
        public bool TryAddSocket(Key Key, SalkyWebSocket ws)
        {
            if(!this.wsConns.TryAdd(Key, ws))
                return false;
            if (!redis_db.SetAdd("connections", Key.ToString()))
            {
                //Dificil chegar aqui
                //talvez se o usuario ficar tentando entrar muitas vezes ao mesmo tempo.
                //Poderia dar lock, mas talvez fique uma bosta
                //Ou não, já que o concurrenty dictionary deve dar lock também
                if (!wsConns.Remove(Key, out _)) Logger.LogWarning("Unable to roolback the connection added");
                return false;
            }
            redis_sub.Subscribe($"disconect:{Key}", DisconnnectConnectionMessageHandler);
            return true;
        }
       
        public bool ContainsSocketKey(Key Key) => HasLocalConnection(Key) || HasRedisConnection(Key);

        private void DisconnnectConnectionMessageHandler(RedisChannel channel, RedisValue Content)
        {
            var usrId = channel.ToString().Split(':').Last();
            if (!TryDisconnectSocket(usrId)) 
                this.Logger.LogWarning("Message to remove reiceved, but cannot remove.");
        }

        private bool HasRedisConnection(string usrId) => redis_conn.GetDatabase().SetContains("connections", usrId);
        private bool HasLocalConnection(string usrId) => wsConns.ContainsKey(usrId);


        //Não da pra implementar este metodo
        //Até da, mas seria alterando o SalkyWebSocket pra um contrato
        //E criar outro, só que ao invez de de ele realmente chamar a função
        //Vai verificar se ele é uma conexão ws legitima, se for só chama a funcao do socket original
        //Se não envia uma mensagem com o comando da funçao chamada ..
        public bool TryGetSocket(Key Key, [MaybeNullWhen(false)] out SalkyWebSocket ws)
        {
            throw new NotImplementedException();
        }
    }

    public class Subscription
    {
        private Action<ConnectionMultiplexer> _unsubscribe;
        public Subscription(Action<ConnectionMultiplexer> Unsubscribe)
        {
            this._unsubscribe = Unsubscribe;
        }
        public void Unsubscribe(ConnectionMultiplexer context)
        {
            this._unsubscribe(context);
        }
    }

    public static class RedisExtensions
    {
        public static Subscription CreateSubscription<T>(this ConnectionMultiplexer connection,RedisChannel Channel,Action<T> handler)
        {
            Action<RedisChannel, RedisValue> _handler = (channel, value) =>
            {
                var msg = JsonSerializer.Deserialize<T>(value.ToString()) ?? throw new NullReferenceException();
                handler(msg);
            };
            connection.GetSubscriber().Subscribe(Channel, _handler);
            return new((con) => con.GetSubscriber().Unsubscribe(Channel,_handler));
        }

    }
}