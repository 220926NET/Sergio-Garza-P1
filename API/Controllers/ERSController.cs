using Microsoft.AspNetCore.Mvc;

using System.Text.Json;

using Services;
using Models;

namespace AP.Controllers;

[ApiController]
[Route("[controller]")]
public class ERSController : ControllerBase
{
    private readonly ILogger<ERSController> _logger;
    private ERSService _service; 

    public ERSController(ILogger<ERSController> logger)
    {
        _logger = logger;
        _service = new ERSService();
    }

    [HttpPost("ticket")]
    public ActionResult<string> SubmitTicket([FromBody] JsonElement json) {
        //TicketInput inputs = JsonSerializer.Deserialize<TicketInput>(json);

        // string? empId = JsonSerializer.Deserialize<string>(json.GetProperty("author").GetRawText());
        // string? amount = JsonSerializer.Deserialize<string>(json.GetProperty("amount").GetRawText());
        // string? description = JsonSerializer.Deserialize<string>(json.GetProperty("description").GetRawText());

        // int empId = (int) json.GetInt32();
        // decimal amount = json.GetDecimal();
        // string description = json.GetString();

        // if (amount == null) return BadRequest("Requests must specify an amount to be submitted");
        // else if (description == null)   return BadRequest("Requests must have a description to be submitted");
        // else if (empId == null) return BadRequest("Must specify ID of employee submitting request");

        // if (inputs.amount == null) return BadRequest("Requests must specify an amount to be submitted");
        // else if (inputs.description == null)   return BadRequest("Requests must have a description to be submitted");
        // else if (inputs.empId == null) return BadRequest("Must specify ID of employee submitting request");

        Dictionary<string, JsonElement> inputs = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
        int? author = int.Parse(inputs["author"].ToString());
        decimal? amount = decimal.Parse(inputs["amount"].ToString());
        string? description = inputs["description"].ToString();
        if (amount == null || amount == 0) return BadRequest("Requests must specify an amount to be submitted");
        else if (description == "" || description == null)   return BadRequest("Requests must have a description to be submitted");
        else if (author == null) return BadRequest("Must specify ID of employee submitting request");

        else {
            //int ticketNum = _service.AddTicket(int.Parse(empId), decimal.Parse(amount), description);
            int ticketNum = _service.AddTicket((int) author, (decimal) amount, description);
            return Created("", ticketNum.ToString());
        }
    }

    [HttpGet("tickets")]
    public ActionResult<string> ViewTickets(){//, string? author) {
        //if (author != null) {}
        bool idGiven = int.TryParse(this.Request.Headers["Current-User"], out int userId);

        if (idGiven) {
            List<Ticket>? tickets = null;
            tickets = _service.Tickets(userId);

            if (tickets != null) {
                return Ok(JsonSerializer.Serialize<List<Ticket>>(tickets));
            }
            else    return Ok("No ticket requests found");
        }

        return BadRequest("User must be logged in to access functionality");
    }

    [HttpPut("ticket")]
    public ActionResult<string> ProcessTicket(int ticket, string status) {
        bool idGiven = int.TryParse(this.Request.Headers["Current-User"], out int userId);

        if (idGiven) {
            if (_service.ManagerCheck(userId)) {
                if (status == "Approved" || status == "Denied") {
                    _service.ReviewTicket(ticket, userId, status);
                    return Accepted("", $"Ticket #{ticket}'s status changed to {status}");
                }
                else    return BadRequest("Status Input Invalid");
            }
            else    return BadRequest("Only Managers can process requests");
        }

        return BadRequest("User must be logged in to access functionality");
    }
}

public class TicketInput
{
    public int empId;
    public string description;
    public decimal amount;

    public TicketInput(int empId, string description, decimal amount) {
        this.empId = empId;
        this.description = description;
        this.amount = amount;
    }
}