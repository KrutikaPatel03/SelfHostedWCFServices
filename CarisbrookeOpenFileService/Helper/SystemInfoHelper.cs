using CarisbrookeOpenFileService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;

namespace CarisbrookeOpenFileService.Helper
{
    public class SystemInfoHelper
    {
        private long _ShipSystemId;
        public void SyncSystemInfoData()
        {
            try
            {
                _ShipSystemId = Globals.CurrentShip.ShipSystemsId;
                UpdateSystemInfo();
                UpdateEventLog();
                UpdateServiceDetails();
                UpdateProcessDetails();
            }
            catch (Exception ex)
            {
                LogHelper.writelog("SyncSystemInfoData Error : " + ex.Message);
            }
        }

        #region Ship Settings
        public bool SaveShipJson(CSShipsModal modal)
        {
            bool res = false;
            try
            {
                string jsonFilePath = ConfigurationManager.AppSettings["ShipValuePath"];
                Directory.CreateDirectory(Path.GetDirectoryName(jsonFilePath));
                if (!File.Exists(jsonFilePath))
                    File.WriteAllText(jsonFilePath, string.Empty);
                string jsonText = File.ReadAllText(jsonFilePath);
                string jsonData = JsonConvert.SerializeObject(modal, Formatting.Indented);
                File.WriteAllText(jsonFilePath, jsonData);
                res = true;
            }
            catch (Exception ex)
            {
                LogHelper.writelog("SaveShipJson Error : " + ex.Message);
            }
            return res;
        }
        public CSShipsModal GetShipJson()
        {
            try
            {
                string jsonFilePath = ConfigurationManager.AppSettings["ShipValuePath"];
                Directory.CreateDirectory(Path.GetDirectoryName(jsonFilePath));
                if (File.Exists(jsonFilePath))
                {
                    string jsonText = File.ReadAllText(jsonFilePath);
                    if (!string.IsNullOrEmpty(jsonText))
                    {
                        CSShipsModal res = JsonConvert.DeserializeObject<CSShipsModal>(jsonText);
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("GetShipJson Error : " + ex.Message);
            }
            return null;
        }
        #endregion

        #region System Info
        public string GetPCUniqueId()
        {
            string uniqueId = string.Empty;
            try
            {
                string cpuInfo = string.Empty;
                ManagementClass mc = new ManagementClass("win32_processor");
                ManagementObjectCollection moc = mc.GetInstances();
                try
                {
                    foreach (ManagementObject mo in moc)
                    {
                        cpuInfo = mo.Properties["processorID"].Value.ToString();
                        break;
                    }
                }
                catch (Exception)
                {
                }
                try
                {
                    string drive = "C";
                    ManagementObject dsk = new ManagementObject(
                        @"win32_logicaldisk.deviceid=""" + drive + @":""");
                    dsk.Get();
                    string volumeSerial = dsk["VolumeSerialNumber"].ToString();
                    uniqueId = cpuInfo + volumeSerial;
                }
                catch (Exception)
                {
                    uniqueId = cpuInfo + Environment.MachineName;
                }
            }
            catch (Exception ex)
            {
                uniqueId = Environment.MachineName;
                LogHelper.writelog("GetPCUniqueId : " + ex.Message);
            }
            return uniqueId;
        }
        public void UpdateSystemInfo()
        {
            try
            {
                List<ShipSystemsInfoModal> AllInfos = new List<ShipSystemsInfoModal>();
                AllInfos.AddRange(SaveOSInformation());
                AllInfos.AddRange(SaveCPUInformation());
                AllInfos.AddRange(SaveProcessorInformation());
                AllInfos.AddRange(SaveNetworkInformation());
                APIHelper _helper = new APIHelper();
                _helper.AddShipSystemInfoBulk(AllInfos);
            }
            catch (Exception ex)
            {
                LogHelper.writelog("UpdateSystemInfo Error : " + ex.Message);
            }
        }
        private List<ShipSystemsInfoModal> SaveOSInformation()
        {
            List<ShipSystemsInfoModal> result = new List<ShipSystemsInfoModal>();
            try
            {
                ManagementClass ManagementClass1 = new ManagementClass("Win32_OperatingSystem");
                //Create a ManagementObjectCollection to loop through
                ManagementObjectCollection ManagemenobjCol = ManagementClass1.GetInstances();
                foreach (ManagementObject obj in ManagemenobjCol)
                {
                    try
                    {
                        AddItemToList(ref result, "Host Name", Utility.ToString(obj.Properties["CSName"].Value), "Operating System", 1);
                        AddItemToList(ref result, "OS Name", Utility.ToString(obj.Properties["Caption"].Value), "Operating System", 2);
                        AddItemToList(ref result, "OS Version", Utility.ToString(obj.Properties["Version"].Value) + " Build " + Utility.ToString(obj.Properties["BuildNumber"].Value), "Operating System", 3);
                        AddItemToList(ref result, "OS Manufacturer", Utility.ToString(obj.Properties["Manufacturer"].Value), "Operating System", 4);
                        AddItemToList(ref result, "OS Build Type", Utility.ToString(obj.Properties["BuildType"].Value), "Operating System", 5);
                        AddItemToList(ref result, "OS Architecture", Utility.ToString(obj.Properties["OSArchitecture"].Value), "Operating System", 6);
                        AddItemToList(ref result, "Product ID", Utility.ToString(obj.Properties["SerialNumber"].Value), "Operating System", 7);
                        AddItemToList(ref result, "Original Install Date", Utility.ToString(obj.Properties["InstallDate"].Value), "Operating System", 8);
                        AddItemToList(ref result, "Registered Owner", Utility.ToString(obj.Properties["RegisteredUser"].Value), "Operating System", 9);
                        AddItemToList(ref result, "System Boot Time", Utility.ToString(obj.Properties["LastBootUpTime"].Value), "Operating System", 10);
                        AddItemToList(ref result, "Boot Device", Utility.ToString(obj.Properties["SystemDevice"].Value), "Operating System", 11);
                        AddItemToList(ref result, "Input Locale", Utility.ToString(obj.Properties["Locale"].Value), "Operating System", 12);
                        AddItemToList(ref result, "Windows Directory", Utility.ToString(obj.Properties["WindowsDirectory"].Value), "Operating System", 13);
                        AddItemToList(ref result, "System Directory", Utility.ToString(obj.Properties["SystemDirectory"].Value), "Operating System", 14);
                        AddItemToList(ref result, "Available Physical Memory", Convert.ToInt64(obj.Properties["FreePhysicalMemory"].Value) / 1024 + " MB", "System", 16);
                        AddItemToList(ref result, "Virtual Memory: Max Size", Convert.ToInt64(obj.Properties["TotalVirtualMemorySize"].Value) / 1024 + " MB", "System", 17);
                        AddItemToList(ref result, "Virtual Memory: Available", Convert.ToInt64(obj.Properties["FreeVirtualMemory"].Value) / 1024 + " MB", "System", 18);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.writelog("SaveOSInformation foreach loop : " + ex.Message);
                    }
                    break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("SaveOSInformation Error : " + ex.Message);
            }
            return result;
        }
        private List<ShipSystemsInfoModal> SaveCPUInformation()
        {
            List<ShipSystemsInfoModal> result = new List<ShipSystemsInfoModal>();
            try
            {
                ManagementClass ManagementClass1 = new ManagementClass("Win32_ComputerSystem");
                //Create a ManagementObjectCollection to loop through
                ManagementObjectCollection ManagemenobjCol = ManagementClass1.GetInstances();
                foreach (ManagementObject obj in ManagemenobjCol)
                {
                    try
                    {
                        AddItemToList(ref result, "Total Physical Memory", Convert.ToInt64(obj.Properties["TotalPhysicalMemory"].Value.ToString()) / 1048576 + " MB", "System", 15);
                        AddItemToList(ref result, "System Manufacturer", Utility.ToString(obj.Properties["Manufacturer"].Value), "System", 19);
                        AddItemToList(ref result, "System Model", Utility.ToString(obj.Properties["Model"].Value), "System", 20);
                        AddItemToList(ref result, "System Type", Utility.ToString(obj.Properties["SystemType"].Value), "System", 21);
                        AddItemToList(ref result, "Domain", Utility.ToString(obj.Properties["Domain"].Value), "System", 22);
                        AddItemToList(ref result, "Logon Server", Utility.ToString(obj.Properties["UserName"].Value), "System", 23);
                        AddItemToList(ref result, "Processor(s)", Utility.ToString(obj.Properties["NumberOfProcessors"].Value), "Processor", 24);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.writelog("SaveCPUInformation foreach loop : " + ex.Message);
                    }
                    break;
                }

            }
            catch (Exception ex)
            {
                LogHelper.writelog("SaveCPUInformation : " + ex.Message);
            }
            return result;
        }
        private List<ShipSystemsInfoModal> SaveProcessorInformation()
        {
            List<ShipSystemsInfoModal> result = new List<ShipSystemsInfoModal>();
            try
            {
                StringBuilder StringBuilder1 = new StringBuilder(string.Empty);
                ManagementClass ManagementClass1 = new ManagementClass("Win32_Processor");
                //Create a ManagementObjectCollection to loop through
                ManagementObjectCollection ManagemenobjCol = ManagementClass1.GetInstances();
                //Get the properties in the class
                PropertyDataCollection properties = ManagementClass1.Properties;
                foreach (ManagementObject obj in ManagemenobjCol)
                {
                    try
                    {
                        AddItemToList(ref result, "Processor Name", Utility.ToString(obj.Properties["Name"].Value), "Processor", 25);
                        AddItemToList(ref result, "Processor Description", Utility.ToString(obj.Properties["Caption"].Value), "Processor", 26);
                        AddItemToList(ref result, "VM Monitor Mode Extensions", Utility.ToString(obj.Properties["VMMonitorModeExtensions"].Value), "Processor", 27);
                        AddItemToList(ref result, "Virtualization Enabled In Firmware", Utility.ToString(obj.Properties["VirtualizationFirmwareEnabled"].Value), "Processor", 28);
                        AddItemToList(ref result, "Second Level Address Translation", Utility.ToString(obj.Properties["SecondLevelAddressTranslationExtensions"].Value), "Processor", 29);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.writelog("SaveProcessorInformation for each loop: " + ex.Message);
                    }
                    StringBuilder1.AppendLine();
                    break;
                }

            }
            catch (Exception ex)
            {
                LogHelper.writelog("SaveProcessorInformation : " + ex.Message);
            }
            return result;
        }
        private List<ShipSystemsInfoModal> SaveNetworkInformation()
        {
            List<ShipSystemsInfoModal> result = new List<ShipSystemsInfoModal>();
            try
            {
                StringBuilder StringBuilder1 = new StringBuilder(string.Empty);
                ManagementClass ManagementClass1 = new ManagementClass("Win32_NetworkAdapterConfiguration");
                //Create a ManagementObjectCollection to loop through
                ManagementObjectCollection ManagemenobjCol = ManagementClass1.GetInstances();
                //Get the properties in the class
                PropertyDataCollection properties = ManagementClass1.Properties;
                var i = 0;
                foreach (ManagementObject obj in ManagemenobjCol)
                {
                    if (i == 0)
                    {
                        i++;
                        continue;
                    }
                    try
                    {
                        AddItemToList(ref result, "Network Card(s)", Utility.ToString(obj.Properties["Description"].Value), "Network", 30);
                        AddItemToList(ref result, "DHCP Enabled", Utility.ToString(obj.Properties["DHCPEnabled"].Value), "Network", 31);
                        AddItemToList(ref result, "IPAddress - 1", ((string[])obj.Properties["IPAddress"].Value)[0], "Network", 32);
                        AddItemToList(ref result, "IPAddress - 2", ((string[])obj.Properties["IPAddress"].Value)[1], "Network", 33);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.writelog("SaveNetworkInformation for each loop : " + ex.Message);
                    }
                    StringBuilder1.AppendLine();
                    break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("SaveNetworkInformation : " + ex.Message);
            }
            return result;
        }
        private void AddItemToList(ref List<ShipSystemsInfoModal> lst, string name, string value, string group, int displayOrder)
        {
            try
            {
                lst.Add(new ShipSystemsInfoModal
                {
                    GroupName = group,
                    PropertyName = name,
                    PropertyValue = value,
                    ShipSystemId = _ShipSystemId,
                    DisplayOrder = displayOrder
                });
            }
            catch (Exception ex)
            {
                LogHelper.writelog("AddItemToList : " + ex.Message);
            }
        }
        #endregion

        #region Event Log Info
        public void UpdateEventLog()
        {
            try
            {
                SaveEventLog("Application");
                SaveEventLog("Security");
                SaveEventLog("System");
            }
            catch (Exception ex)
            {
                LogHelper.writelog("UpdateEventLog : " + ex.Message);
            }
        }
        public void SaveEventLog(string logType)
        {
            try
            {
                APIHelper _helper = new APIHelper();
                EventLog aLog = new EventLog();
                aLog.Log = logType;// "Application"; //"System";// "Application"; //"Security";
                aLog.MachineName = ".";  // Local machine
                var lstErrorLogs = (from c in aLog.Entries.OfType<EventLogEntry>() where c.EntryType == EventLogEntryType.Error select c).ToList();
                foreach (EventLogEntry entry in lstErrorLogs)
                {
                    try
                    {
                        _helper.AddShipSystemEventlog(new ShipSystemsEventLogModal
                        {
                            EventDate = entry.TimeGenerated,
                            EventDescription = entry.Message,
                            EventId = entry.EventID,
                            EventLogType = logType,
                            EventMachineName = entry.MachineName,
                            EventSource = entry.Source,
                            EventType = "Error",
                            ShipSystemId = _ShipSystemId,
                            UserName = entry.UserName
                        });
                    }
                    catch (Exception ex)
                    {
                        LogHelper.writelog("SaveEventLog foreach loop: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("SaveEventLog : " + ex.Message);
            }
        }
        #endregion

        #region Service Info
        public void UpdateServiceDetails()
        {
            APIHelper _helper = new APIHelper();
            try
            {
                ManagementClass ManagementClass1 = new ManagementClass("Win32_Service");
                //Create a ManagementObjectCollection to loop through
                ManagementObjectCollection ManagemenobjCol = ManagementClass1.GetInstances();
                Regex regex = new Regex("-k(.+?)-p");
                foreach (ManagementObject obj in ManagemenobjCol)
                {
                    try
                    {
                        var matched = regex.Match(obj.Properties["PathName"].Value != null ? obj.Properties["PathName"].Value.ToString() : "");
                        _helper.AddShipSystemsService(new ShipSystemsServiceModal
                        {
                            ShipSystemId = _ShipSystemId,
                            Name = Utility.ToString(obj.Properties["Name"].Value),
                            Status = Utility.ToString(obj.Properties["State"].Value),
                            StartupType = Utility.ToString(obj.Properties["StartMode"].Value),
                            Description = Utility.ToString(obj.Properties["Description"].Value),
                            ProcessId = Convert.ToInt64(obj.Properties["ProcessId"].Value),
                            ServiceType = Utility.ToString(obj.Properties["ServiceType"].Value),
                            StartName = Utility.ToString(obj.Properties["StartName"].Value),
                            SystemName = Utility.ToString(obj.Properties["SystemName"].Value),
                            Title = Utility.ToString(obj.Properties["Caption"].Value),
                            GroupName = Utility.ToString(matched.Groups[1])
                        });
                    }
                    catch (Exception ex)
                    {
                        LogHelper.writelog("UpdateServiceDetails for each loop: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("UpdateServiceDetails : " + ex.Message);
            }
        }
        #endregion

        #region Process Info
        public void UpdateProcessDetails()
        {
            APIHelper _helper = new APIHelper();
            try
            {
                // Create an array to store the processes
                Process[] processList = Process.GetProcesses();
                string status = string.Empty;
                // Loop through the array of processes to show information of every process in your console
                foreach (Process process in processList)
                {
                    try
                    {
                        // Define the status from a boolean to a simple string
                        status = (process.Responding == true ? "Responding" : "Not responding");

                        // Retrieve the object of extra information of the process (to retrieve Username and Description)
                        dynamic extraProcessInfo = GetProcessExtraInformation(process.Id);

                        _helper.AddShipSystemsProcess(new ShipSystemsProcessModal
                        {
                            ShipSystemId = _ShipSystemId,
                            Name = process.ProcessName,  // Process name
                            Status = (process.Responding == true ? "Running" : "Not responding"),  // Process status
                            Description = extraProcessInfo.Description,  // Description of the process
                            ProcessId = process.Id,  // Process ID
                            Title = process.ProcessName,  // Process name
                            MemoryUsage = Utility.BytesToReadableValue(process.PrivateMemorySize64), // Memory usage
                            Username = extraProcessInfo.Username    // Username that started the process                      
                        });
                    }
                    catch (Exception ex)
                    {
                        LogHelper.writelog("UpdateProcessDetails foreach loop : " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("UpdateProcessDetails : " + ex.Message);
            }
        }

        /// <summary>
        /// Returns an Expando object with the description and username of a process from the process ID.
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public ExpandoObject GetProcessExtraInformation(int processId)
        {
            // Create a dynamic object to store some properties on it
            dynamic response = new ExpandoObject();
            response.Description = "";
            response.Username = "Unknown";

            try
            {
                // Query the Win32_Process
                string query = "Select * From Win32_Process Where ProcessID = " + processId;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                ManagementObjectCollection processList = searcher.Get();
                foreach (ManagementObject obj in processList)
                {
                    // Retrieve username 
                    string[] argList = new string[] { string.Empty, string.Empty };
                    int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                    if (returnVal == 0)
                    {
                        // return Username
                        response.Username = argList[0];
                    }

                    // Retrieve process description if exists
                    if (obj["ExecutablePath"] != null)
                    {
                        try
                        {
                            FileVersionInfo info = FileVersionInfo.GetVersionInfo(obj["ExecutablePath"].ToString());
                            response.Description = info.FileDescription;
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("GetProcessExtraInformation : " + ex.Message);
            }


            return response;
        }
        #endregion
    }
}
