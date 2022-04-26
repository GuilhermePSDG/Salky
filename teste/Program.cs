using Salky.Shared;

string WEB_SOCKET_URL = "ws://localhost:5000/";

await TestaEnvioMensagem();
async Task TestaConexão()
{
    var rng = new Random();
    var arr = rng.Next(1000000, 9999999).ToString().Select(q => byte.Parse(q.ToString())).ToArray();
    var n = Convert.ToBase64String(arr,0,arr.Length);
    Console.WriteLine(n);
    var socket = new SalkyWebSocketClient(new Uri(WEB_SOCKET_URL), new Usuario(n, "none"));
    await socket.SendMessage(new Message());
    socket.Dispose();
}

async Task TestaOnline()
{
    var socket1 = new SalkyWebSocketClient(new Uri(WEB_SOCKET_URL), new Usuario("usuarioTestaUserOnline1", Guid.NewGuid().ToString()));
    var socket2 = new SalkyWebSocketClient(new Uri(WEB_SOCKET_URL), new Usuario("usuarioTestaUserOnline2", Guid.NewGuid().ToString()));

    await socket1.MudarVisibilidade(true);
    await socket2.MudarVisibilidade(true);

    var visiveis = await socket1.ObterUsuariosVisiveis();
    var visiveis2 = await socket2.ObterUsuariosVisiveis();
    socket1.Dispose();
    socket2.Dispose();
}

async Task TestaEnvioMensagem()
{
    var joao = new Usuario("João", Guid.NewGuid().ToString());
    var maria = new Usuario("Maria", Guid.NewGuid().ToString());
    var joaoSocket = new SalkyWebSocketClient(new Uri(WEB_SOCKET_URL), joao);
    var mariaSocket = new SalkyWebSocketClient(new Uri(WEB_SOCKET_URL), maria);

    await joaoSocket.MudarVisibilidade(true);
    await mariaSocket.MudarVisibilidade(true);


    joaoSocket.OnMessageReiceved += (sender, msg) =>
    {
        Console.WriteLine($"Usuario : {(sender as SalkyWebSocketClient).user.Apelido} recebeu mensagem de {msg.Rota.De}\nConteudo : {msg.Json}\n\n");
    };
    mariaSocket.OnMessageReiceved += (sender, msg) =>
    {
        Console.WriteLine($"Usuario : {(sender as SalkyWebSocketClient).user.Apelido} recebeu mensagem de {msg.Rota.De}\nConteudo : {msg.Json}\n\n");
    };

    await joaoSocket.SendMessage(new Message(new Rota(joao, maria, PathEndPoints.EnviarMensagem), "Olá mariaSocket"));
    await mariaSocket.SendMessage(new Message(new Rota(maria, joao, PathEndPoints.EnviarMensagem), "Olá joaoSocket"));


    while (true) ;
    joaoSocket.Dispose();
    mariaSocket.Dispose();
}