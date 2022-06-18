using Salky.WebSocket.Extensions;
using Salky.WebSocket.Handler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.UseSalkyWebSocketRouter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app
    .UseWebSockets()
    .UseMiddleware<SalkyWebSocketMiddleWare>();


app.UseHttpsRedirection();



app.Run();
