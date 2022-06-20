﻿using Salky.WebSocket.Infra.Models;
using StackExchange.Redis;
using System.Text;

var resMsg = await new HttpClient().GetAsync("https://stackoverflow.com/questions/33330483/request-only-meta-tags-from-a-webpage");
var stream = resMsg.Content.ReadAsStream();








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

var usr1 = new User("Guilherme");
var usr2 = new User("Geovane");

var r1 = db.SetAdd(CreateKey("app", AppID, "connections"), new List<RedisValue>() { usr1.Id.ToString(), usr2.Id.ToString() }.ToArray());
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

string CreateKey(params string[] values)
{
    return values.Aggregate((a, b) => a + ":" + b);
}

class User
{
    public User(string Name)
    {
        this.Id = Guid.NewGuid();
        this.Name = Name;
    }
    public Guid Id { get; }
    public string Name { get; }
}
