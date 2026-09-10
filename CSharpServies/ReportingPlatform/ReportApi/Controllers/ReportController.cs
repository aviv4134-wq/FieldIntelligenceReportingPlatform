using ConsumerElasticSearch.Models;
using Microsoft.AspNetCore.Mvc;
using ReportApi.Repositories;

namespace ReportApi.Controllers;




[ApiController]
[Route("api/[controller]")]

public class ReportController : ControllerBase

{
    private IRepositoryReports _repo;

    public ReportController(IRepositoryReports repo)
    {
        _repo = repo;
    }


    [HttpGet("reports/search")]
    public async Task<ActionResult<IEnumerable<Report>>> GetByText([FromQuery] string text)
    {
        var result = await _repo.GetByTextAsync(text);

        if (result == null) return StatusCode(500, "faild to connect to elastic");

        return Ok(result);

    }

    [HttpGet("subjects/{subjectId}/reports")]
    public async Task<ActionResult<IEnumerable<Report>>> GetBySubjectId(string subjectId)
    {
        var result = await _repo.GetBySunjectIdAsync(subjectId);

        if (result == null)
            return StatusCode(500, "faild to connect to elastic");

        return Ok(result);
    }

    [HttpGet("reports")]
    public async Task<ActionResult<IEnumerable<Report>>> GetByFields([FromQuery] string? theater, [FromQuery] string? sector, [FromQuery] string? location)
    {
        return Ok(await _repo.GetByFieldesAsync(theater!, sector!, location!));
    }

    [HttpGet("reports/priorities")]
    public async Task<ActionResult<IEnumerable<Report>>> GetByPriorities([FromQuery] List<string>? priorities, [FromQuery] DateTime? fromThis, [FromQuery] DateTime? toThis)
    {
        return Ok(await _repo.GetByPriorityAndDateAsync(priorities, fromThis, toThis));
    }


    [HttpGet("reports/search-parmaters")]
    public async Task<ActionResult<IEnumerable<Report>>> GetBySearchingParmeters([FromQuery] string? text, [FromQuery] string? theater, [FromQuery] string? sector, [FromQuery] string? location , [FromQuery] DateTime? fromThis, [FromQuery] DateTime? toThis, [FromQuery] string[]? priorities = null)
    {
        
        return Ok(await _repo.GetByParametersAsync(text, theater, sector, location, priorities, fromThis, toThis));
    }


    [HttpGet("statistics")]

    public async Task<ActionResult<object>> GetStatistics()
    {
        return Ok(await _repo.GstStatisticsAsync());
    }
}
