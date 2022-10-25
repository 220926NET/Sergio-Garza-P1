using Microsoft.AspNetCore.Mvc;

using System.Text.Json;

using Services;
using Models;

namespace AP.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private Authentication _accessability; 

    public AuthenticationController(ILogger<AuthenticationController> logger)
    {
        _logger = logger;
        _accessability = new Authentication();
    }

    [HttpPost("register")]
    public ActionResult<string> Registration([FromBody] JsonElement json) {
        string? username = JsonSerializer.Deserialize<string>(json.GetProperty("username"));
        string? password = JsonSerializer.Deserialize<string>(json.GetProperty("password"));

        if (username != null && password != null) {
            if (_accessability.Register(username, password)) {
                return Created("", "Registration successful");
            }
            else    return BadRequest("Username is already taken!");
        }
        else    return BadRequest("Invalid Input");
    }

    [HttpPost("login")]
    public ActionResult<string> Verify([FromBody] JsonElement json) {
        string? username = JsonSerializer.Deserialize<string>(json.GetProperty("username"));
        string? password = JsonSerializer.Deserialize<string>(json.GetProperty("password"));
        
        if (username != null && password != null) {
            int userId = _accessability.LogIn(username, password);

            if (userId != 0) {
                this.Response.Headers.Add("Current-User", userId.ToString());
                
                return Accepted("", "Logged In!");
            }
            else    return BadRequest("Invalid Credentials");
        }
        else    return BadRequest("Invalid Input");
    }


    // public ActionResult<string> Registration(string username, string password) {
    //     if (_accessability.Register(username, password)) {
    //         return CreatedAtAction("Registration successful", true);
    //     }
    //     else {
    //         return BadRequest("Username is already taken!");
    //     }
    // }


    // public ActionResult<string> something([FromBody] JsonElement json) {
    //     string? username = JsonSerializer.Deserialize<string>(json.GetProperty("username"));
    //     string? password = JsonSerializer.Deserialize<string>(json.GetProperty("password"));

    //     if (_accessability.LogIn(username, password)) {
    //         return Accepted("", "Employee");
    //     }
    //     else {
    //         return BadRequest("Invalid Credentials");
    //     }
    // }

    // [HttpPost("login2")]
    // public ActionResult<string> Verify(string username, string password) {
    //     if (_accessability.LogIn(username, password)) {
    //         return Accepted("Employee");
    //     }
    //     else {
    //         return BadRequest("Invalid Credentials");
    //     }
    // }
}

/*
HttpResponseMessage response =new HttpResponseMessage(HttpStatusCode.OK);
response.Headers.Add("MyHeader", "MyHeaderValue");
return ResponseMessage(response);


FOR RETURNING ACTION TYPES
*/