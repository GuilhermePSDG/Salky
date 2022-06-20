using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;
using Microsoft.EntityFrameworkCore;
using Salky.Domain.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Salky.API;
using Salky.Domain.Models.GroupModels;
using System.Security.Cryptography;
using Salky.Domain.Models.UserModels;
using StackExchange.Redis;
using Salky.WebSocket.Infra.Models;
using Salky.App.Services;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Npgsql;
using Salky.App.Security;
using Salky.Domain;
using Salky.Domain.Contracts;
using Salky.WebSocket.Handler;
using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket;
using Salky.WebSocket.Infra.Socket;

string AppID = "1";




using (ConnectionMultiplexer connectionRedis = ConnectionMultiplexer.Connect("localhost:6379,password=senhadoredis"))
{
    //obter o database para envio de comandos ao Redis
    IDatabase clientRedis = connectionRedis.GetDatabase();
    //gravando uma chave
    clientRedis.StringSet("admin_sistema", "Desenvolvedor Ninja");
    //lendo uma chave
    Console.WriteLine(clientRedis.StringGet("admin_sistema"));
    //definindo 600 segundos como tempo de expiração
    clientRedis.KeyExpire("admin_sistema", TimeSpan.FromSeconds(600));
    //consultando o tempo de expiração da chave
    Console.WriteLine(clientRedis.KeyTimeToLive("admin_sistema"));
    //retirando o tempo de expiração da chave tornando-a permanente
    clientRedis.KeyPersist("admin_sistema");
    //apagando uma chave
    clientRedis.KeyDelete("admin_sistema");
    //fechando a conexão com o Redis
    connectionRedis.Close();
}


Console.WriteLine("Hello");


var conR = ConnectionMultiplexer.Connect("localhost:6379");

var sub = conR.GetSubscriber();
var db = conR.GetDatabase();
var channel = "a";
//db.StringSet("I am a string", "I am the string value");
//db.StringSet(CreateKey("app", AppID,"client","gui"),"");
//db.StringSet(CreateKey("app", AppID,"client","fi"),"");
//db.StringSet(CreateKey("app", AppID,"client", "fqwe"),"");
//db.StringSet(CreateKey("app", AppID,"client", "fqwe"),"");

var usr1 = new User("Guilherme");
var usr2 = new User("Geovane");

var r1 =db.SetAdd(CreateKey("app", AppID,"connections"), new List<RedisValue>() { usr1.Id.ToString(), usr2.Id.ToString() }.ToArray());
return;





var h1 = (RedisChannel channel, RedisValue value) =>
{
    Console.WriteLine($"In Handler 1 - From Channel : {channel} - Value : {value}");
};
var h2 = (RedisChannel channel, RedisValue value) =>
{
    Console.WriteLine($"In Handler 2 - From Channel : {channel} - Value : {value}");
};
await sub.SubscribeAsync(channel, h1);
await sub.SubscribeAsync(channel, h2);

await sub.PublishAsync(channel, "Msg1");
await sub.PublishAsync(channel, "Msg2");
await Task.Delay(200);
await sub.UnsubscribeAsync(channel, h1);
Console.WriteLine("Handler 1 out");
await sub.PublishAsync(channel, "Msg3");

while (true) ;

void AddConnection(string AppId ,string ConnectionId,Action<MessageServer> Handler)
{

}
string CreateKey(params string[] values)
{
    var str = "";
    foreach (var x in values) str += x+":";
    return str.Trim(':');
}
void RouteResolver(MessageServer msg)
{

}
class User{
    public User(string Name)
    {
        this.Id = Guid.NewGuid();
        this.Name = Name;
    }

    public Guid Id { get; }
    public string Name { get; }
}

//public class RedisConnectionMannager /*: IConnectionManager*/
//{
//    public RedisConnectionMannager(ConnectionMultiplexer connection, IConnectionManager Previus)
//    {
//        var thDId = Thread.GetDomainID();
//        this.ConnectionPoolId = Guid.NewGuid().ToString();
//        this.AppRedisBasePoolPath = CreateKey("app", thDId);
//        this.AppRedisConnectionsPath = CreateKey(AppRedisBasePoolPath, "connections");
//        this.connection = connection;
//        this.Previus = Previus;
//    }
//    public RedisConnectionMannager(ConnectionMultiplexer connection)
//    {
//        var thDId = Thread.GetDomainID();
//        this.ConnectionPoolId = "root";
//        this.AppRedisBasePoolPath = CreateKey("app", thDId);
//        this.AppRedisConnectionsPath = CreateKey(AppRedisBasePoolPath, "connections");
//        this.connection = connection;
//    }
//    public Dictionary<string, SalkyWebSocket> Sockets = new();
//    public Dictionary<string, IConnectionManager> Pools = new();

//    public string AppRedisConnectionsPath { get; }
//    public string AppRedisBasePoolPath { get; }

//    private ConnectionMultiplexer connection;
//    private IDatabase redis_db => connection.GetDatabase();
//    public string ConnectionPoolId { get; }
//    public int ConnectionsCount => Sockets.Count;
//    public int PoolsCount => this.Pools.Count;
//    public IConnectionManager Previus { get; }
//    public void AddSocket(string key, SalkyWebSocket data)
//    {
//        var added = redis_db.SetAdd(AppRedisConnectionsPath, new List<RedisValue>() { key }.ToArray());
//        if (added == 0) throw new Exception("User is alredy on redis database, please remove beffore add");
//        this.Sockets.Add(key, data);
//    }
//    private string CreateKey(params object[] values)
//    {
//        var str = "";
//        foreach (var x in values) str += x.ToString() + ":";
//        return str.Trim(':');
//    }

//    public bool ContainsSocketKey(string key)
//    {
//        return redis_db.SetContains(AppRedisConnectionsPath, key);
//    }

//    public IConnectionManager CreateConnectionPool(string connectionPoolId, params string[] participantsId)
//    {
//        var added = redis_db.SetAdd(AppRedisConnectionsPath, participantsId.Select(x => new RedisValue(x)).ToArray());
//        if (added != participantsId.Length) throw new InvalidOperationException();
//        //PROBLEMA, COMO A POOLID NÃO ESTÁ SENDO CONSIDERADA, VAI DAR ERROR NA HORA DE ADICIONAR.
//        var @new = new RedisConnectionMannager(this.connection, this);
//        participantsId.Where(x => Sockets.ContainsKey(x)).Select(x => new { sock = Sockets[x], key = x }).ToList().ForEach(x => @new.AddSocket(x.key, x.sock));
//        return @new;
//    }
//}


