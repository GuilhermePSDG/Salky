using StackExchange.Redis;
using System;
using System.Text;

//ConnectionMultiplexer asd = ConnectionMultiplexer.Connect("localhost:6379");
//var handler = new RedisPoolMannager(null);
//handler.AddSocket("teste", new());

//int i = 0;
//bool toAdd = true;
//while (true)
//{
//    try
//    {
//        await Task.Delay(500);
//        if (toAdd)
//        {
//            handler.AddSocket(i.ToString(), new());
//        }
//        else
//        {
//            handler.TryRemoveSocket(i.ToString(), out _);
//        }
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.Message);
//    }
//    i++;
//    if (i == 30)
//    {
//        i = 0;
//        toAdd = !toAdd;
//    }
//}


//return;



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

string CreateKey(params string[] values)
{
    return string.Join(':', values);
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