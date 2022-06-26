using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Salky.App.Services;
[Route("")]
public class PingController : ControllerBase{
    public IConfiguration Configuration { get; }

    public PingController(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    [HttpGet("")]
    public IActionResult Ping(){
        return Ok("pong");
    }
    [HttpGet("config")]
    public IActionResult Config()
    {
        return Ok(this.Configuration.GetSection("ConnectionStrings").GetValue<string>("LocalSqlite"));

    }

}
