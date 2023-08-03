using CarisbrookeOpenFileService.Models;
using System;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Net.NetworkInformation;
using System.ComponentModel;

namespace CarisbrookeOpenFileService.Helper
{
    public static class Utility
    {
        public static ServiceConnectModal ReadWriteServiceConfigJson(ServiceConnectModal objSettings)
        {
            try
            {
                string jsonText = string.Empty;
                string jsonFilePath = ConfigurationManager.AppSettings["ServiceConfigPath"];
                Directory.CreateDirectory(Path.GetDirectoryName(jsonFilePath));
                //if (File.Exists(jsonFilePath))
                //    jsonText = File.ReadAllText(jsonFilePath);
                //else {                    
                //    jsonText = JsonConvert.SerializeObject(objSettings);
                //    File.WriteAllText(jsonFilePath, jsonText);
                //}
                //if (!string.IsNullOrEmpty(jsonText))
                //    return JsonConvert.DeserializeObject<ServiceConnectModal>(jsonText);                    
                //else
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.writelog("ReadServiceConfigJson Error : " + ex.Message);
                return null;
            }
        }

        public static bool CheckInternet()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GetLocalDBConnStr(ServerConnectModal dbConnModal)
        {
            string connetionString = string.Empty;
            try
            {
                if (dbConnModal != null)
                {
                    connetionString = "Data Source=" + dbConnModal.ServerName + ";Initial Catalog=" + dbConnModal.DatabaseName + ";User ID=" + dbConnModal.UserName + ";Password=" + dbConnModal.Password + "";
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog(ex.Message);
            }
            return connetionString;
        }

        public static ServerConnectModal ReadDBConfigJson()
        {
            try
            {
                string jsonText = string.Empty;
                string jsonFilePath = ConfigurationManager.AppSettings["DBConfigPath"];
                Directory.CreateDirectory(Path.GetDirectoryName(jsonFilePath));
                if (System.IO.File.Exists(jsonFilePath))
                {
                    jsonText = System.IO.File.ReadAllText(jsonFilePath);
                }
                if (!string.IsNullOrEmpty(jsonText))
                {
                    ServerConnectModal Modal = JsonConvert.DeserializeObject<ServerConnectModal>(jsonText);
                    return Modal;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogHelper.writelog("DBConfig Create Error : " + ex.Message);
                return null;
            }
        }

        public static string ToString(object value)
        {
            try
            {
                return Convert.ToString(value);
            }
            catch { return string.Empty; }
        }
        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            DataTable table = new DataTable();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
        public static List<T> ToListof<T>(this DataTable dt) //, List<string> excludedColumnNames=null
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();

            var objectProperties = typeof(T).GetProperties(flags);
            var targetList = dt.AsEnumerable().Select(dataRow =>
            {
                var instanceOfT = Activator.CreateInstance<T>();

                foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
                {
                    //if (excludedColumnNames != null && excludedColumnNames.IndexOf(properties.Name) >= 0)
                    //    continue;
                    //else
                    //{
                    var convertedValue = GetValueByDataType(properties.PropertyType, dataRow[properties.Name]);
                    //properties.SetValue(instanceOfT, dataRow[properties.Name], null);
                    properties.SetValue(instanceOfT, convertedValue, null);
                    // }                    
                }
                return instanceOfT;
            }).ToList();

            return targetList;
        }

        private static object GetValueByDataType(Type propertyType, object o)
        {
            if (o.ToString() == "null")
            {
                return null;
            }
            if (propertyType == (typeof(Guid)) || propertyType == typeof(Guid?))
            {
                return Guid.Parse(o.ToString());
            }
            else if (propertyType == typeof(int) || propertyType == typeof(int?) || propertyType.IsEnum)
            {
                return Convert.ToInt32(o);
            }
            else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
            {
                return Convert.ToDecimal(o);
            }
            else if (propertyType == typeof(double) || propertyType == typeof(double?))
            {
                return Convert.ToDouble(o);
            }
            else if (propertyType == typeof(long) || propertyType == typeof(long?))
            {
                return Convert.ToInt64(o);
            }
            else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
            {
                return Convert.ToBoolean(o);
            }
            else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
            {
                return Convert.ToDateTime(o);
            }
            return o.ToString();
        }

        /// <summary>
        /// Method that converts bytes to its human readable value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string BytesToReadableValue(long number)
        {
            List<string> suffixes = new List<string> { " B", " KB", " MB", " GB", " TB", " PB" };

            for (int i = 0; i < suffixes.Count; i++)
            {
                long temp = number / (int)Math.Pow(1024, i + 1);

                if (temp == 0)
                {
                    return (number / (int)Math.Pow(1024, i)) + suffixes[i];
                }
            }

            return number.ToString();
        }
    }
}
