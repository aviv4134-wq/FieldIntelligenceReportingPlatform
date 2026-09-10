using ConsumerElasticSearch.Models;
using Microsoft.AspNetCore.Mvc;
using ReportApi.Repositories;

namespace ReportApi.Controllers;




[ApiController]
[Route("api/[controller]")]

public class ReportController : ControllerBase

{
    private IRepositoryReports _repo;
    private ILogger<ReportController> _logger;

    public ReportController(IRepositoryReports repo,ILogger<ReportController> logger)
    {
        _repo = repo;
        _logger = logger;
    }


    [HttpGet("reports/search")]
    public async Task<ActionResult<IEnumerable<Report>>> GetByText([FromQuery] string text)
    {
        _logger.LogInformation("request reports by text started");
        var result = await _repo.GetByTextAsync(text);

        _logger.LogInformation("seccess reqest");
        return Ok(result);

    }

    [HttpGet("subjects/{subjectId}/reports")]
    public async Task<ActionResult<IEnumerable<Report>>> GetBySubjectId(string subjectId)
    {
        _logger.LogInformation("request reports by subjectID started");
        var result = await _repo.GetBySunjectIdAsync(subjectId);


        _logger.LogInformation("seccess reqest");

        return Ok(result);
    }

    [HttpGet("reports")]
    public async Task<ActionResult<IEnumerable<Report>>> GetByFields([FromQuery] string? theater, [FromQuery] string? sector, [FromQuery] string? location)
    {
        _logger.LogInformation("request reports by fieldes started");
        
        var result = await _repo.GetByFieldesAsync(theater!, sector!, location!);
        _logger.LogInformation("seccess reqest");
        return Ok(result);
    }

    [HttpGet("reports/priorities")]
    public async Task<ActionResult<IEnumerable<Report>>> GetByPriorities([FromQuery] List<string>? priorities, [FromQuery] DateTime? fromThis, [FromQuery] DateTime? toThis)
    {
        _logger.LogInformation("request reports by priorityis started");
        var resoult = await _repo.GetByPriorityAndDateAsync(priorities, fromThis, toThis);
        _logger.LogInformation("seccess reqest");
        return Ok(resoult);
    }


    [HttpGet("reports/search-parmaters")]
    public async Task<ActionResult<IEnumerable<Report>>> GetBySearchingParmeters([FromQuery] string? text, [FromQuery] string? theater, [FromQuery] string? sector, [FromQuery] string? location , [FromQuery] DateTime? fromThis, [FromQuery] DateTime? toThis, [FromQuery] string[]? priorities = null)
    {
        _logger.LogInformation("request reports by searching parameters started");
        var resoult = await _repo.GetByParametersAsync(text, theater, sector, location, priorities, fromThis, toThis);
        _logger.LogInformation("seccess reqest");
        return Ok(resoult);
    }


    [HttpGet("statistics")]

    public async Task<ActionResult<object>> GetStatistics()
    {
        _logger.LogInformation("request reports statistics started");
        var resoult = await _repo.GstStatisticsAsync();
        _logger.LogInformation("seccess reqest");
        return Ok(resoult);
    }
}
