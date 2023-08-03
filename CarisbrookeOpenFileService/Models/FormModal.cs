using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarisbrookeOpenFileService.Models
{
    public class FormModal
    {
        public int ID { get; set; }
        public Nullable<System.Guid> FormID { get; set; }
        public string TemplatePath { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public Nullable<int> Issue { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public Nullable<int> Amendment { get; set; }
        public Nullable<System.DateTime> AmendmentDate { get; set; }
        public string Department { get; set; }
        public string Category { get; set; }
        public string AccessLevel { get; set; }
        public Nullable<bool> AllowsNetworkAccess { get; set; }
        public Nullable<bool> CanBeOpened { get; set; }
        public Nullable<bool> HasSavedData { get; set; }
        public Nullable<bool> IsURNBased { get; set; }
        public string URN { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string DownloadPath { get; set; }
        public string UploadType { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public double? Version { get; set; }
        public double? DocumentVersion { get; set; }
    }

    public class SyncedForm
    {
        public string FormID { get; set; }
        public string Title { get; set; }
        public string SyncType { get; set; }
        public DateTime SyncDateTime { get; set; }
    }
}
