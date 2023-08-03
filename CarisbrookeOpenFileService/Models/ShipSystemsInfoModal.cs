using System;

namespace CarisbrookeOpenFileService.Models
{
    public class ShipSystemModal
    {
        public long Id { get; set; }
        public string ShipCode { get; set; }
        public string PCName { get; set; }
        public string PCUniqueId { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
    }

    public class ShipSystemsEventLogModal
    {
        public Guid Id { get; set; }
        public Nullable<long> ShipSystemId { get; set; }
        public Nullable<int> EventId { get; set; }
        public string EventSource { get; set; }
        public string EventDescription { get; set; }
        public Nullable<DateTime> EventDate { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public string UserName { get; set; }
        public string EventMachineName { get; set; }
        public string EventType { get; set; }
        public string EventLogType { get; set; }
    }
    public class ShipSystemsInfoModal
    {
        public Guid Id { get; set; }
        public Nullable<long> ShipSystemId { get; set; }
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public string GroupName { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class ShipSystemsServiceModal
    {
        public System.Guid Id { get; set; }
        public long ShipSystemId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ServiceType { get; set; }
        public string Status { get; set; }
        public string StartupType { get; set; }
        public string StartName { get; set; }
        public string SystemName { get; set; }
        public Nullable<long> ProcessId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public String GroupName { get; set; }
    }
    public class ShipSystemsProcessModal
    {
        public System.Guid Id { get; set; }
        public Nullable<long> ShipSystemId { get; set; }
        public Nullable<long> ProcessId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Username { get; set; }
        public string MemoryUsage { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
    public class OpenFileServicesEventLogModal
    {
        public Guid Id { get; set; }
        public long? ShipSystemId { get; set; }
        public string RunningVersion { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
