using System;

namespace CarisbrookeOpenFileService.Models
{
    public class DocumentModal
    {
        public int DocID { get; set; }
        public Nullable<System.Guid> DocumentID { get; set; }
        public Nullable<System.Guid> ParentID { get; set; }
        public string Number { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string DownloadPath { get; set; }
        public string UploadType { get; set; }
        public double? Version { get; set; }
        public double? DocumentVersion { get; set; }
        public string SectionType { get; set; }
    }
    public class SyncedDocument
    {
        public string DocumentID { get; set; }
        public string ParentID { get; set; }
        public string Title { get; set; }
        public string DocType { get; set; }
        public string SyncType { get; set; }
        public DateTime SyncDateTime { get; set; }
    }
}
