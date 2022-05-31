using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Salky.App.Services;
[Route("")]
public class PingController : ControllerBase{
    [HttpGet("")]
    public IActionResult Ping(){
        return Ok("pong");
    }

}