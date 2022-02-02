using Microsoft.AspNetCore.Mvc;
using SerilogTimings;
using SerilogTimings.Extensions;
using ILogger = Serilog.ILogger;

namespace TimedLogSerilog.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger _logger;

    public WeatherForecastController(ILogger logger)
    {
        _logger = logger?.ForContext<WeatherForecastController>() ?? throw new ArgumentNullException(nameof(_logger));
    }

    [HttpGet(Name = "operation-time")]
    public IEnumerable<WeatherForecast> Get()
    {
        using var _ = _logger.TimeOperation("Operação ");

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [HttpGet("operation-begin", Name = "operation-begin")]
    public IEnumerable<WeatherForecast> GetOperationBegin()
    {
        using var op = Operation.Begin("Retrieving operation-begin");

        var data = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

        op.Complete();

        return data;
    }
}