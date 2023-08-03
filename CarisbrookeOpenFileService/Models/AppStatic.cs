using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarisbrookeOpenFileService.Models
{
    public class AppStatic
    {
        public static string SUCCESS = "Success";
        public static string ERROR = "Error";
        public static string DATABASENAME = "CarisbrookeShipping1234";
        public static string INSERTED = "INSERTED";

        #region TableNames

        public static string SMRForm = "SMRForm";
        public static string SMRFormCrewMembers = "SMRFormCrewMembers";
        public static string Documents = "Documents";
        public static string Forms = "Forms";

        public static string GeneralInspectionReport = "GeneralInspectionReport";
        public static string GlRCrewDocuments = "GIRCrewDocuments";
        public static string GlRSafeManningRequirements = "GIRSafeManningRequirements";
        public static string GIRRestandWorkHours = "GIRRestandWorkHours";

        public static string GIRDeficiencies = "GIRDeficiencies";
        public static string GIRPhotographs = "GIRPhotographs";
        public static string GIRDeficienciesFile = "GIRDeficienciesFiles";
        public static string DeficienciesNote = "DeficienciesNote";
        public static string GIRDeficienciesCommentFile = "GIRDeficienciesCommentFile";

        public static string SuperintendedInspectionReport = "SuperintendedInspectionReport";
        public static string SIRNotes = "SIRNotes";
        public static string SIRAdditionalNotes = "SIRAdditionalNotes";

        public static string InternalAuditForm = "InternalAuditForm";
        public static string AuditNotes = "AuditNotes";
        public static string AuditNotesAttachment = "AuditNotesAttachment";

        public static string ArrivalReports = "ArrivalReports";
        public static string DepartureReports = "DepartureReports";
        public static string DailyCargoReports = "DailyCargoReports";
        public static string DailyPositionReport = "DailyPositionReport";

        #endregion

        #region DocumentsUploadType

        public static string NEW = "NEW";
        public static string UPDATED = "UPDATED";
        public static string REMOVED = "REMOVED";

        #endregion
    }
}
