using ConsumerElasticSearch.Models;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsumerElasticSearch.Servies
{
    public class ValidatorReports
    {
        public static bool Validate(Report report)
        {
            Log.Information("Starting validation process for report {ReportId}", report.reportId);

            if (!ValidateEmptyFeileds(report))
                return false;

            if (!ValidateFields(report))
                return false;

            bool isSubjectId = IsEmptyFieleds(report.reportId, "subjectId", report.subjectId);
            bool isSubjectType = IsEmptyFieleds(report.reportId, "subjectType", report.subjectType);

            if (isSubjectId == false && isSubjectType == false)
            {
                Log.Information("Report {ReportId} validated successfully", report.reportId);
                return true;
            }

            if (isSubjectId == true && isSubjectType == true)
            {
                Log.Information("Report {ReportId} validated successfully", report.reportId);
                return true;
            }

            Log.Error("Validation failed for report {ReportId}: subjectId and subjectType must both be present or both absent", report.reportId);
            return false;
        }

        private static bool ValidateEmptyFeileds(Report report)
        {
            var reportId = report.reportId;

            if (!IsEmptyFieleds(reportId, "reportId", reportId))
            {
                Log.Error("Validation failed: 'reportId' is missing or empty");
                return false;
            }

            if (!IsEmptyFieleds(reportId, "timestemp", report.timestamp))
            {
                Log.Error("Validation failed for report {ReportId}: 'timestamp' is missing or empty", reportId);
                return false;
            }

            if (!IsEmptyFieleds(reportId, "agentId", report.agentId))
            {
                Log.Error("Validation failed for report {ReportId}: 'agentId' is missing or empty", reportId);
                return false;
            }

            if (!IsEmptyFieleds(reportId, "unit", report.unit))
            {
                Log.Error("Validation failed for report {ReportId}: 'unit' is missing or empty", reportId);
                return false;
            }

            if (!IsEmptyFieleds(reportId, "theater", report.theater))
            {
                Log.Error("Validation failed for report {ReportId}: 'theater' is missing or empty", reportId);
                return false;
            }

            if (!IsEmptyFieleds(reportId, "sector", report.sector))
            {
                Log.Error("Validation failed for report {ReportId}: 'sector' is missing or empty", reportId);
                return false;
            }

            if (!IsEmptyFieleds(reportId, "location", report.location))
            {
                Log.Error("Validation failed for report {ReportId}: 'location' is missing or empty", reportId);
                return false;
            }

            if (!IsEmptyFieleds(reportId, "reportType", report.reportType))
            {
                Log.Error("Validation failed for report {ReportId}: 'reportType' is missing or empty", reportId);
                return false;
            }

            if (!IsEmptyFieleds(reportId, "priority", report.priority))
            {
                Log.Error("Validation failed for report {ReportId}: 'priority' is missing or empty", reportId);
                return false;
            }

            if (!IsEmptyFieleds(reportId, "sourceType", report.sourceType))
            {
                Log.Error("Validation failed for report {ReportId}: 'sourceType' is missing or empty", reportId);
                return false;
            }

            if (!IsEmptyFieleds(reportId, "message", report.message))
            {
                Log.Error("Validation failed for report {ReportId}: 'message' is missing or empty", reportId);
                return false;
            }

            return true;
        }

        private static bool ValidateFields(Report report)
        {
            var reportId = report.reportId;
            if (!IsValidTimeStemp(reportId, report.timestamp))
                return false;

            if (!IsPriorityAllowedValue(reportId, report.priority))
                return false;

            if (!IsReportTypeAllowedValue(reportId, report.reportType))
                return false;

            return true;
        }

        private static bool IsEmptyFieleds(string? reportId, string? fildName, string? fildValue)
        {
            if (fildValue == null) return false;

            if (string.IsNullOrEmpty(fildValue)) return false;

            if (string.IsNullOrWhiteSpace(fildValue)) return false;

            return true;
        }

        private static bool IsValidTimeStemp(string reportId, string timestamp)
        {
            if (!DateTime.TryParse(timestamp, out DateTime f))
            {
                Log.Error("Validation failed for report {ReportId}: invalid timestamp '{Timestamp}'", reportId, timestamp);
                return false;
            }

            return true;
        }

        private static bool IsPriorityAllowedValue(string reportId, string priority)
        {
            string[] allowedValues = ["Low", "Medium", "High", "Critical"];
            if (!allowedValues.Contains(priority))
            {
                Log.Error("Validation failed for report {ReportId}: invalid priority '{Priority}'", reportId, priority);
                return false;
            }

            return true;
        }

        private static bool IsReportTypeAllowedValue(string reportId, string reportType)
        {
            string[] allowedValues = ["Observation", "Movement", "Meeting", "Access", "Communication", "Logistics", "Incident"];

            if (!allowedValues.Contains(reportType))
            {
                Log.Error("Validation failed for report {ReportId}: invalid reportType '{ReportType}'", reportId, reportType);
                return false;
            }

            return true;
        }
    }
}