global using Salky.App.Services;
global using Salky.WebSocket.Infra.Models;
global using Salky.WebSocket.Infra.Routing;
global using Salky.WebSocket.Infra.Routing.Atributes;
global using Salky.WebSocket.Infra.Socket;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Salky.App.Interfaces;
using System.Text;
using System.Text.Json.Serialization;
using Salky.API;
using Salky.WebSocket.Extensions;
using Salky.App.Security;
using Newtonsoft.Json.Converters;
using Salky.Domain.Repositories;
using Salky.Domain.Contracts;
using Salky.Domain.Contexts;
using Salky.Domain.Models.UserModels;
using Microsoft.Extensions.FileProviders;
using System.Net;
using Salky.Domain;
using Salky.App.Services.User;
using Salky.App.Services.Group;
using Salky.App.Services.Friends;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




builder.Services.AddControllers()
        .AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
        )
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling =
            Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
        }
        );

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWTKEY"))),
                  ValidateIssuer = false,
                  ValidateAudience = false,
                  ValidateLifetime = true,
              };
          });
//builder.Services.AddIdentityCore<User>(options =>
//{
//    options.Password.RequireDigit = false;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireUppercase = false;
//    options.Password.RequiredLength = 2;
//    options.User.RequireUniqueEmail = false;
//})
//    //.AddRoles<Role>()
//    //.AddRoleManager<RoleManager<Role>>()
//    //.AddSignInManager<SignInManager<User>>()
//    //.AddRoleValidator<RoleValidator<Role>>()
//    //.AddEntityFrameworkStores<SalkyDbContext>()
//    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordMannager, PasswordHasher>();
builder.Services.AddScoped<IFriendRepository,FriendRepository>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<MongoClient>(x => new MongoClient(builder.Configuration.GetConnectionString("MongoClient")));
builder.Services.AddScoped<IMessageGroupRepository, MessageRepositoryMongoDb>();
builder.Services.AddScoped<IMessageFriendRepository, MessageFriendRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IGroupMemberRepository, GroupMemberRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<GroupMessageService>();
builder.Services.AddScoped<GroupService>();
builder.Services.AddScoped<GroupMemberService>();
builder.Services.AddScoped<FriendService>();
builder.Services.AddScoped<FriendMessageService>();
builder.Services.AddScoped<ImageService>();
builder.UseLocalSqlite<SalkyDbContext>();
builder.Services.AddCors();

builder.Services.RegisterEvents();

#region WebSocket
builder.Services.InjectRequiredSalkyWebSocketDependencys();
builder.Services.UseSalkyWebSocketRouter();
#endregion
var app = builder.Build();


Directory.CreateDirectory(ImageRepositoryConfig.FullPath);
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(ImageRepositoryConfig.FullPath),
    RequestPath = new PathString($"/{ImageRepositoryConfig.FolderName}"),
});

app.UseSalkyWebSocket();
Dispatcher.InstanceFactory = () => app.Services.CreateScope().ServiceProvider.GetRequiredService<IDispatcher>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    Environment.SetEnvironmentVariable("JWTKEY", "JwtHighSecuritySecret");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
}
//app.MigrateDatabase<SalkyDbContext>();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
