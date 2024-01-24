using Microsoft.AspNetCore.Mvc;
using RabbitMQ.RabbitMq;

namespace RabbitMQ.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RabbitMqController : ControllerBase
{
    private readonly IRabbitMqService _rabbitMqService;
    private readonly ILogger<RabbitMqController> _logger;

    public RabbitMqController(IRabbitMqService rabbitMqService, ILogger<RabbitMqController> logger)
    {
        _rabbitMqService = rabbitMqService;
        _logger = logger;
    }

    [HttpGet]
    [Route("[action]/{message}")]
    public IActionResult SendMessage(string message)
    {
        _rabbitMqService.SendMessage(message);
        return Ok("Сообщение отправлено");
    }
}