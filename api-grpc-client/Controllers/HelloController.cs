﻿using grpc_open_tel;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloController : ControllerBase
{
    private readonly Greeter.GreeterClient _client;

    public HelloController(Greeter.GreeterClient client)
    {
        _client = client;
    }
    
    /// <summary>
    /// GRPC Success
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpGet(Name = "Grpc-success")]
    public async Task<IActionResult> Get([FromQuery] string name)
    {
        var reply = await _client.SayHelloAsync(new HelloRequest { Name = name });
        return Ok(reply.Message);
    }
    
    /// <summary>
    /// GRPC Error with logging
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpGet("error", Name = "Grpc-error")]
    public async Task<IActionResult> GetAgain([FromQuery] string name)
    {
        try
        {
            var reply = await _client.SayHelloAgainAsync(new HelloRequest { Name = name });
            return Ok(reply.Message);
        }
        catch (RpcException e)
        {
            return StatusCode((int) e.StatusCode, e.Status.Detail);
        }
    }
}