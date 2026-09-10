using Confluent.Kafka;
using ConsumerElasticSearch.Models;
using ConsumerElasticSearch.Servies;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Nodes;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Text.Json;



namespace ConsumerElasticSearch
{
    class Program
    {
        static async Task Main()
        {
            IConfiguration builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            Serilog.Debugging.SelfLog.Enable(Console.Error);

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder)
            .CreateLogger();



            string bootstrap = builder["kafka:BootStrapServer"]!;
            string topic = builder["kafka:Topic:rawdata"]!;
            string groupId = builder["kafka:GroupId"]!;

            var settings = new ElasticsearchClientSettings(new Uri("http://localhost:9200"))
                .DefaultMappingFor<Report>(m => m.IdProperty(r => r.reportId));

            var client = new ElasticsearchClient(settings);

            

            var service = new ServiceCollection();

            service.AddSerilog();

            service.AddSingleton(client);
            
            

            var serviesProvidor = service.BuildServiceProvider();

            using ( var scope = serviesProvidor.CreateScope())
            {

                var elasticClient = scope.ServiceProvider.GetRequiredService<ElasticsearchClient>();

                var response  = await elasticClient.Indices.CreateAsync("reports", c => c
                
                .Mappings(mappings => mappings
               
                .Properties<Report>(p => p
                .Keyword(r => r.reportId)
                .Date(r => r.timestamp)
                .Keyword(r => r.agentId)
                .Keyword(r => r.unit)
                .Keyword(r => r.theater)
                .Keyword(r => r.sector)
                .Keyword(r => r.location)
                .Keyword(r => r.reportType)
                .Keyword(r => r.priority)
                .Keyword(r => r.sourceType)
                .Text(r => r.message)
                .Keyword(r => r.subjectId)
                .Keyword(r => r.subjectType)
                .Date(r => r.processedAt)      
                )));

                

                Log.Information("index created");
                
            }

            
            
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrap,
                EnableAutoCommit = false,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            
            var consumer = new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe(topic);
            

            while (true)
            {
                try
                {


                    var result = consumer.Consume(TimeSpan.FromSeconds(2));
                    if (result == null || result.Message?.Value == null)
                        continue;

                    var report = JsonSerializer.Deserialize<Report>(result.Message.Value);


                    bool isValidReport = ValidatorReports.Validate(report);
                    consumer.Commit();

                    if (!isValidReport)
                        continue;

                    var response = await client.PingAsync();

                    if (!response.IsValidResponse)
                        Log.Error("the server offline");

                    var IsExsists = await client.ExistsAsync("reports", report.reportId);

                    if (!IsExsists.Exists)
                    {
                        report.processedAt = DateTime.UtcNow;
                        var IsRportCreated = await client.IndexAsync(report, x => x.Index("reports"));

                        if (!IsRportCreated.IsValidResponse)
                            Log.Error("report not created ");

                        else Log.Information("report add to elastic search");

                    }
                    
                    else Log.Error("the report {report.reportId} is duplicate", report.reportId);

                    
                }
                catch (Exception ex)
                {

                    Log.Error(ex.Message);
                    Log.Fatal("close program");
                    Log.CloseAndFlush();
                    throw;
                }
                
               
            }
        }
    }
}