using Microsoft.AspNetCore;
using WebSocket;

try
{
    WebHost.CreateDefaultBuilder(new string[0])
    .UseStartup<Startup>()
    .Build()
    .Run();
}
catch (Exception ex)
{
    Console.WriteLine("ERRO FATAL NO SERVIDOR");
    Console.WriteLine(ex.ToString());
    Console.WriteLine(ex.Message);
}