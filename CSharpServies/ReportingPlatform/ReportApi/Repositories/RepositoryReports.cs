using ConsumerElasticSearch.Models;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Aggregations;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ReportApi.DTOS;

namespace ReportApi.Repositories
{
    public class RepositoryReports : IRepositoryReports
    {
        private ElasticsearchClient _client;

        private ILogger<RepositoryReports> _logger;
        private const string _reportsIndex = "reports";
        public RepositoryReports(ElasticsearchClient client,ILogger<RepositoryReports> logger)
        {
            _client = client;
            _logger = logger;
        
        }


        public async Task<IEnumerable<Report>> GetByTextAsync(string text)
        {
            
            var response = await _client.SearchAsync<Report>(search => search
            .Indices(_reportsIndex).
             
             Query(query => query
            .Match(match => match
            .Field("message")
            .Query(text)
            )
            )
             .Size(1000)
             );

            if (!response.IsValidResponse)
                throw new InvalidOperationException(response.ElasticsearchServerError?.Error?.Reason);


            return response.Documents;
            
        }

        public async Task<IEnumerable<Report>> GetBySunjectIdAsync(string subjectId)
        {
            var response = await _client.SearchAsync<Report>(search => search
            .Indices(_reportsIndex)
            .Query(quary => quary

            .Term(term => term
            .Field(r => r.subjectId)
            .Value(subjectId)


            )
            )
            .Sort(r => r
             .Field(r => r.timestamp)
            ).Size(1000)
            );

            if (!response.IsValidResponse)
                throw new InvalidOperationException(response.ElasticsearchServerError?.Error?.Reason);

            return response.Documents;

        }

        public async Task<IEnumerable<Report>> GetByFieldesAsync(string? theater, string? sector, string? location)
        {

            var filters = new List<Query>();

            if (theater != null)
                filters.Add(new TermQuery 
                { 
                    Field = "theater",
                    Value = theater }
                );

            if (sector != null)
                filters.Add(new TermQuery
                { 
                    Field = "sector",
                    Value = sector
               
                });

            if (location != null)
                filters.Add(new TermQuery
                {
                    Field = "location",
                    Value = location
                });

            Query query;
            
            if (filters.Count > 0)
            {
                query = new BoolQuery { Filter = filters };
            }
            else
            {
                query = new MatchAllQuery();
            }


            var response = await _client.SearchAsync<Report>(search => search
            .Indices(_reportsIndex)
            .Query(query).Size(1000)
            );

            if (!response.IsValidResponse)
                throw new InvalidOperationException(response.ElasticsearchServerError?.Error?.Reason);


            return response.Documents;

        }

        public async Task<IEnumerable<Report>> GetByPriorityAndDateAsync(List<string>? priorities,DateTime? fromThis,DateTime? toThis)
        {
            var filters = new List<Query>();

           
            if (priorities != null)
            {
                filters.Add(new TermsQuery
                {
                    Field = ("priority"),
                    Terms = new TermsQueryField(priorities.Select(p => (FieldValue)p).ToArray())
                    
                });
            }


            if (fromThis != null ||  toThis != null)
            {
                filters.Add(new DateRangeQuery()
                {
                    Field = "timestamp",
                    Gte = fromThis,
                    Lte = toThis
                });
            }


            Query query = filters.Count > 0
                ? new BoolQuery { Filter = filters }
                : new MatchAllQuery();

            var response = await _client.SearchAsync<Report>(s => s
                .Indices(_reportsIndex)
                .Query(query).Size(1000)
            );

            if (!response.IsValidResponse)
                throw new InvalidOperationException(response.ElasticsearchServerError?.Error?.Reason);

            return response.Documents ;
        }
        public async Task<IEnumerable<Report>> GetByParametersAsync(string? text, string? theater, string? sector, string? location, string[]? priorities, DateTime? fromThis, DateTime? toThis)
        {
            
            var must = new List<Query>();
            var filters  = new List<Query>();

            if (theater != null)
                filters.Add(new TermQuery { 
                    Field = "theater"
                    ,
                    Value = theater
                });

            

            if (sector != null)
                filters.Add(new TermQuery
                {
                    Field = "sector"
                    ,
                    Value = sector
                });
           

            if (location != null)
                filters.Add(new TermQuery
                {
                    Field = "location"
                    ,
                    Value = location
                });

            if ( priorities != null )
                filters.Add(new TermsQuery
                {
                    Field = "priority"
                    ,
                    Terms = new TermsQueryField(priorities.Select(p => (FieldValue)p).ToArray())
                });
            

            if (fromThis != null || toThis != null)
                filters.Add(new DateRangeQuery
                {
                    Field = "timestamp"
                    ,
                    Gte = fromThis
                    ,Lte = toThis
                });

            if (text != null)
                must.Add(new MatchQuery
                {
                    Field = "message"
                    ,
                    Query = text
                });
            if (must.Count == 0) must = null;

            if (filters.Count == 0) filters = null;

            


            var quary = new BoolQuery { Must = must , Filter = filters };

            var response = await _client.SearchAsync<Report>(search => search
            .Indices(_reportsIndex)
            .Query(quary)
            .Size(1000)
            
            );

            if (!response.IsValidResponse)
                throw new InvalidOperationException(response.ElasticsearchServerError?.Error?.Reason);


            return response.Documents;

        }

        public async Task<StatisticsResponse> GstStatisticsAsync()
        {
            var response = await _client.SearchAsync<Report>(s => s
            .Indices("reports")
            .Size(0)
            .Aggregations(aggs => aggs
            .Add("priorities", a => a.Terms(t => t.Field("priority")))
            .Add("reportTypes", a => a.Terms(t => t.Field("reportType")))
            .Add("theaters", a => a.Terms(t => t.Field("theater")))
            
            ).Size(1000)
            );

            if (!response.IsValidResponse)
                throw new InvalidOperationException(response.ElasticsearchServerError?.Error?.Reason);

            var aggs = response.Aggregations;

            return new StatisticsResponse
            { 
                ByPriority = aggs.GetStringTerms("priorities")?.Buckets.ToDictionary(b => b.Key.ToString(), b => b.DocCount)!,
                ByReportType = aggs.GetStringTerms("reportTypes")?.Buckets.ToDictionary(b => b.Key.ToString(), b => b.DocCount)!,
                ByTheater = aggs.GetStringTerms("theaters")?.Buckets.ToDictionary(b => b.Key.ToString(), b => b.DocCount)!
            };


        }





    }
}
