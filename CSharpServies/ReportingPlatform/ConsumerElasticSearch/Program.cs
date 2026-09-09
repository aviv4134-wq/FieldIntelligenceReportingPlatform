using Confluent.Kafka;
using ConsumerElasticSearch.Models;
using ConsumerElasticSearch.Servies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;



namespace ConsumerElasticSearch
{
    class Program
    {
        static void Main()
        {
            IConfiguration builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            string bootstrap = builder["kafka:BootStrapServer"]!;
            string topic = builder["kafka:Topic:rawdata"]!;
            string groupId = builder["kafka:GroupId"]!;

            var service = new ServiceCollection();

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
                var result = consumer.Consume(TimeSpan.FromSeconds(2));
                if (result == null || result.Message?.Value == null)
                    continue;
               
                var report = result.Message.Value;


                bool isValidReport = ValidatorReports.Validate(report);
                consumer.Commit();

                if (!isValidReport) 
                    continue;

                Console.WriteLine(report);

            }
        }
    }
}