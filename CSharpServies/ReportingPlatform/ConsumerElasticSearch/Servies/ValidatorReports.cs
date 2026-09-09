using ConsumerElasticSearch.Models;
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

        public static bool Validate(string rawReport)
        {
            var report = JsonSerializer.Deserialize<Report>(rawReport);
            
            if ( ! ValidateEmptyFeileds(report))
                return false;

            if ( ! ValidateFields(report))
                return false;

            bool isSubjectId = IsEmptyFieleds(report.reportId, "subjectId", report.subjectId);

            bool isSubjectType = IsEmptyFieleds(report.reportId, "subjectType", report.subjectType);

            if (isSubjectId == false && isSubjectType == false)
            {
                return true;
            }

            if (isSubjectId == true && isSubjectType == true)
            {

                return true;
            }

            return false;



            

        }


        private static bool ValidateEmptyFeileds(Report report)
        {
           var reportId = report.reportId;
           
            if ( ! IsEmptyFieleds( reportId, "reportId", reportId))
                return false;

            if (!IsEmptyFieleds(reportId, "timestemp", report.timestamp))
                return false;

            if (!IsEmptyFieleds(reportId, "agentId", report.agentId))
                return false;

            if ( ! IsEmptyFieleds(reportId, "unit", report.unit))
                return false;

            if (!IsEmptyFieleds(reportId, "theater", report.theater))
                return false;

            if (!IsEmptyFieleds(reportId, "sector", report.sector))
                return false;

            if (!IsEmptyFieleds(reportId, "location", report.location))
                return false;

            if (!IsEmptyFieleds(reportId, "reportType", report.reportType))
                return false;

            if (!IsEmptyFieleds(reportId, "priority", report.priority))
                return false;

            if (!IsEmptyFieleds(reportId, "sourceType", report.sourceType))
                return false;

            if (!IsEmptyFieleds(reportId, "message", report.message))
                return false;

            return true;


                
        }

        private static bool ValidateFields(Report report)
        {
           var reportId = report.reportId;
            if ( ! IsValidTimeStemp(reportId,report.timestamp))
                return false;

            if (!IsPriorityAllowedValue(reportId, report.priority))

                return false;

            if (!IsReportTypeAllowedValue(reportId, report.reportType))
                    return false;

            return true;
        }

        



        private static bool IsEmptyFieleds(string? reportId ,string? fildName,string? fildValue)
        {
            if (fildValue == null) return false;

            if (string.IsNullOrEmpty(fildValue)) return false;

            if (string.IsNullOrWhiteSpace(fildValue)) return false;

            return true;

        }

        private static bool IsValidTimeStemp(string reportId, string timestamp)
        {
            if (! DateTime.TryParse(timestamp, out DateTime f))
            {
                
                return false;
            }
            
            return true;
        }

        private static bool IsPriorityAllowedValue(string reportId, string priority)
        {
            string[] allowedValues = ["Low","Medium","High","Critical"];
            if (!allowedValues.Contains(priority))
            {
                
                return false;
            }

            return true;
        }

        private static bool IsReportTypeAllowedValue(string reportId,string reportType)
        {
            string[] allowedValues = ["Observation", "Movement","Meeting", "Access","Communication", "Logistics", "Incident"];
            
            if ( ! allowedValues.Contains(reportType))
            {

                return false;
            }
            

            return true;
        
        }

        //public bool IsTwoFieldsExists( feil)
        //{
        //    if (report.subjectId == null) return false;

        //    if (string.IsNullOrEmpty(fildValue)) return false;

        //    if (string.IsNullOrWhiteSpace(fildValue)) return false;

        //}

    }
}
