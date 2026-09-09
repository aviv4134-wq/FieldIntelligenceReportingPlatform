using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConsumerElasticSearch.Models
{
    public class Report
    {
        public string? reportId { get; set; } = string.Empty;
        public string? @timestamp { get; set; } = string.Empty;

        public string? agentId { get; set; } = string.Empty;

        public string? unit { get; set; } = string.Empty;

        public string? theater { get; set; } = string.Empty;

        public string? sector { get; set; } = string.Empty;

        public string? location { get; set; } = string.Empty;

        public string? reportType { get; set; } = string.Empty;

        public string? priority { get; set; } = string.Empty;

        public string? sourceType { get; set; } = string.Empty;

        public string? message { get; set; } = string.Empty;

        public string? subjectId { get; set; } = string.Empty;

        public string? subjectType { get; set; } = string.Empty;

        public DateTime? processedAt { get; set; } = null;

        

    }
}
