using ConsumerElasticSearch.Models;
using ReportApi.DTOS;

namespace ReportApi.Repositories
{
    public interface IRepositoryReports
    {
        public Task<IEnumerable<Report>> GetByTextAsync(string text);

        public Task<IEnumerable<Report>> GetBySunjectIdAsync(string subjectId);

        public Task<IEnumerable<Report>> GetByFieldesAsync(string theater,string sector,string location);

        public Task<IEnumerable<Report>> GetByPriorityAndDateAsync(List<string>? priorities, DateTime? fromThis, DateTime? toThis);

       
        public Task<IEnumerable<Report>> GetByParametersAsync(string? text , string? theater, string? sector, string? location,string[]? priorities, DateTime? fromThis, DateTime? toThis);

        public Task<StatisticsResponse> GstStatisticsAsync();
    }
}
