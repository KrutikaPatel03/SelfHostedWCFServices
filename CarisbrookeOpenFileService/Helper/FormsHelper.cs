using CarisbrookeOpenFileService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace CarisbrookeOpenFileService.Helper
{
    public class FormsHelper
    {
        public bool StartFormsync()
        {
            bool isUpdated = false;
            APIHelper _helper = new APIHelper();
            List<SyncedForm> SyncedFormList = new List<SyncedForm>();
            List<FormModal> APIFormsList = _helper.GetAllFormsForService();
            if (APIFormsList != null && APIFormsList.Count > 0)
            {
                var LocalFormsList = GetLocalForms(APIFormsList);
                if (LocalFormsList == null || LocalFormsList.Count <= 0)
                {
                    foreach (var ApiDoc in APIFormsList)
                    {
                        bool isCreatedOnLocal = CreatePhysicalLocation(ApiDoc);
                    }
                    isUpdated = true;
                }
                else
                {
                    foreach (var ApiDoc in APIFormsList)
                    {
                        FormModal LocalDoc = LocalFormsList.Where(x => x.FormID == ApiDoc.FormID).FirstOrDefault();
                        if (LocalDoc != null)
                        {
                            // Exist in local machine - check for update
                            UpdateFormProcess(ApiDoc, LocalDoc, ref SyncedFormList);
                        }
                        else
                        {
                            // Not Exist in local machine - Insert New Form
                            InsertFormProcess(ApiDoc, ref SyncedFormList);
                        }
                        //bool isCreatedOnLocal = CreatePhysicalLocation(ApiDoc);
                    }
                    if (SyncedFormList != null && SyncedFormList.Count > 0)
                    {
                        isUpdated = true;
                        UpdateLocalForms(APIFormsList);
                    }
                }
            }
            PrintSyncList(SyncedFormList);
            return isUpdated;
        }
        public void InsertFormProcess(FormModal ApiDoc, ref List<SyncedForm> SyncedFormList)
        {
            if (ApiDoc.IsDeleted == false)
            {
                bool isCreatedOnLocal = CreatePhysicalLocation(ApiDoc);
                if (isCreatedOnLocal == true)
                    AddSyncList(ApiDoc, ref SyncedFormList, AppStatic.INSERTED);
            }
        }
        public void UpdateFormProcess(FormModal ApiDoc, FormModal LocalDoc, ref List<SyncedForm> SyncedFormList)
        {
            try
            {
                if (LocalDoc.DocumentVersion != ApiDoc.DocumentVersion || !System.IO.File.Exists(LocalDoc.TemplatePath))
                {
                    string oldFilePath = LocalDoc.TemplatePath;
                    string oldFileDownloadPath = LocalDoc.DownloadPath;
                    bool res = DownloadUpdatedFormDoc(LocalDoc, ApiDoc);
                    //if (res)
                    //{
                    //    LocalDoc.TemplatePath = ApiDoc.TemplatePath;
                    //    LocalDoc.DownloadPath = ApiDoc.DownloadPath;
                    //    LocalDoc.UploadType = ApiDoc.UploadType;
                    //    LocalDoc.DocumentVersion = ApiDoc.DocumentVersion;                        
                    if (res == true)
                    {
                        //        DeleteOldFile(oldFilePath, oldFileDownloadPath);
                        AddSyncList(ApiDoc, ref SyncedFormList, AppStatic.UPDATED);
                    }
                    //}
                }
                if (LocalDoc.IsDeleted != ApiDoc.IsDeleted)
                    LocalDoc.IsDeleted = ApiDoc.IsDeleted;
            }
            catch (Exception ex)
            {
                LogHelper.writelog("UpdateFormProcess : " + ex.Message);
            }

        }
        public List<FormModal> GetLocalForms(List<FormModal> APIFormsList)
        {
            List<FormModal> LocalFormsList = new List<FormModal>();
            try
            {
                string jsonText = string.Empty;
                string jsonFilePath = ConfigurationManager.AppSettings["FormsListPath"];
                Directory.CreateDirectory(Path.GetDirectoryName(jsonFilePath));
                if (File.Exists(jsonFilePath))
                    jsonText = File.ReadAllText(jsonFilePath);
                if (!string.IsNullOrEmpty(jsonText))
                    LocalFormsList = JsonConvert.DeserializeObject<List<FormModal>>(jsonText);
                else
                {
                    jsonText = JsonConvert.SerializeObject(APIFormsList);
                    File.WriteAllText(jsonFilePath, jsonText);
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("GetLocalForms : " + ex.Message);
            }
            return LocalFormsList;
        }

        public List<FormModal> UpdateLocalForms(List<FormModal> APIFormsList)
        {
            List<FormModal> LocalFormsList = new List<FormModal>();
            try
            {
                string jsonText = string.Empty;
                string jsonFilePath = ConfigurationManager.AppSettings["FormsListPath"];
                Directory.CreateDirectory(Path.GetDirectoryName(jsonFilePath));
                if (File.Exists(jsonFilePath))
                {
                    jsonText = JsonConvert.SerializeObject(APIFormsList);
                    File.WriteAllText(jsonFilePath, jsonText);
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("UpdateLocalForms : " + ex.Message);
            }
            return LocalFormsList;
        }

        public bool CreatePhysicalLocation(FormModal modal)
        {
            bool res = false;
            try
            {
                string OfficeAPPUrl = ConfigurationManager.AppSettings["OfficeAPPUrl"];
                if (string.IsNullOrEmpty(modal.DownloadPath))
                    modal.DownloadPath = modal.TemplatePath.Replace(@"C:\ProgramData\Carisbrooke Shipping Ltd\ISM Dashboard\", "");

                string DownloadPath = @"" + OfficeAPPUrl + modal.DownloadPath;
                DownloadPath = DownloadPath.Replace("\\", "/");
                string prefixLocalPath = @"C:\ProgramData\Carisbrooke Shipping Ltd\ISM Dashboard\";
                string downloadLocalPath = prefixLocalPath + modal.DownloadPath;
                if (modal.DownloadPath != null)
                {
                    if (!File.Exists(downloadLocalPath))
                    {
                        try
                        {
                            string dirName = Path.GetDirectoryName(downloadLocalPath);
                            if (!Directory.Exists(dirName))
                                Directory.CreateDirectory(dirName);
                        }
                        catch (Exception e)
                        {
                            LogHelper.writelog("CreatePhysicalLocation : Create directory error" + e.Message + ". Path :" + downloadLocalPath);
                        }
                        try
                        {
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(DownloadPath);
                            using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                            {
                                Byte[] buffer = new Byte[(Int32)webResponse.ContentLength]; //new Byte[32 * 1024];
                                using (Stream bin = webResponse.GetResponseStream())
                                {
                                    using (FileStream fs = File.Create(downloadLocalPath))
                                    {

                                        int read = bin.Read(buffer, 0, buffer.Length);
                                        while (read > 0)
                                        {
                                            fs.Write(buffer, 0, read);
                                            read = bin.Read(buffer, 0, buffer.Length);
                                        }
                                    }
                                    File.SetLastWriteTime(downloadLocalPath, webResponse.LastModified);
                                }
                            }
                            res = true;
                        }
                        catch (Exception e)
                        {
                            LogHelper.writelog("CreatePhysicalLocation : Download error : " + e.Message + ". Path :" + downloadLocalPath);
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("CreatePhysicalLocation " + ex.Message);
                res = false;
            }
            return res;
        }
        public bool DownloadUpdatedFormDoc(FormModal LocalDoc, FormModal ApiDoc)
        {
            bool res = false;
            try
            {
                string oldFilePath = LocalDoc.TemplatePath;
                string oldFileName = Path.GetFileName(oldFilePath);
                string oldFileNameOrig = Path.GetFileNameWithoutExtension(oldFilePath);
                string oldFileExtension = Path.GetExtension(oldFilePath);
                try
                {
                    // Delete Old Files
                    if (File.Exists(oldFilePath))
                    {
                        string newFilePath = oldFilePath.Replace(oldFileName, oldFileNameOrig + "_" + LocalDoc.DocumentVersion + oldFileExtension);
                        File.Move(oldFilePath, newFilePath);
                    }
                }
                catch (Exception e)
                {
                    LogHelper.writelog("DownloadUpdatedFormDoc :- Delete Old Files :" + e.Message);
                }
                if (string.IsNullOrEmpty(LocalDoc.DownloadPath))
                    LocalDoc.DownloadPath = LocalDoc.TemplatePath.Replace(@"C:\ProgramData\Carisbrooke Shipping Ltd\ISM Dashboard\", "");
                if (string.IsNullOrEmpty(ApiDoc.DownloadPath))
                    ApiDoc.DownloadPath = ApiDoc.TemplatePath.Replace(@"C:\ProgramData\Carisbrooke Shipping Ltd\ISM Dashboard\", "");
                else
                    ApiDoc.DownloadPath = Regex.Replace(ApiDoc.DownloadPath, @"C:\\ProgramData\\Carisbrooke Shipping Ltd\\ISM Dashboard\\", "", RegexOptions.IgnoreCase); //ApiDoc.DownloadPath.Replace(@"C:\ProgramData\Carisbrooke Shipping Ltd\ISM Dashboard\", "");
                string oldFileDownloadPath = LocalDoc.DownloadPath;
                string prefixServerPath = string.Format(@"{0}", ConfigurationManager.AppSettings["PrefixServerPath"]);
                string ServerDeletePath = prefixServerPath + oldFileDownloadPath;
                // Create New Files
                string OfficeAPPUrl = ConfigurationManager.AppSettings["OfficeAPPUrl"];
                string DownloadPath = @"" + OfficeAPPUrl + ApiDoc.DownloadPath;
                string downloadServerPath = prefixServerPath + ApiDoc.DownloadPath;
                string downloadLocalPath = ApiDoc.TemplatePath;

                try
                {
                    string dirName = Path.GetDirectoryName(downloadLocalPath);
                    if (!Directory.Exists(dirName))
                        Directory.CreateDirectory(dirName);                    
                }
                catch (Exception e)
                {
                    LogHelper.writelog("DownloadUpdatedFormDoc : Create Directory error : " + e.Message + ". Path : " + downloadLocalPath);
                }

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(DownloadPath);
                    using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                    {
                        Byte[] buffer = new Byte[(Int32)webResponse.ContentLength]; //new Byte[32 * 1024];
                        using (Stream bin = webResponse.GetResponseStream())
                        {
                            using (FileStream fs = File.Create(downloadLocalPath))
                            {

                                int read = bin.Read(buffer, 0, buffer.Length);
                                while (read > 0)
                                {
                                    fs.Write(buffer, 0, read);
                                    read = bin.Read(buffer, 0, buffer.Length);
                                }
                            }
                            File.SetLastWriteTime(downloadLocalPath, webResponse.LastModified);
                        }
                    }
                    res = true;
                }
                catch (Exception e)
                {
                    LogHelper.writelog("DownloadUpdatedFormDoc : Download error : " + e.Message + ". Path : " + DownloadPath);
                }
               
            }
            catch (Exception ex)
            {
                LogHelper.writelog("DownloadUpdatedFormDoc :" + ex.Message);
            }
            return res;
        }
        public bool DeleteOldFile(string FilePath, string DownloadPath)
        {
            bool res = false;
            try
            {
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
                if (!string.IsNullOrWhiteSpace(DownloadPath))
                {
                    string prefixServerPath = string.Format(@"{0}", ConfigurationManager.AppSettings["PrefixServerPath"]);
                    string ServerDeletePath = prefixServerPath + DownloadPath;
                    if (File.Exists(ServerDeletePath))
                        File.Delete(ServerDeletePath);
                }

            }
            catch (Exception ex)
            {
                LogHelper.writelog("DeleteOldFile :" + ex.Message);
            }
            return res;
        }
        public void AddSyncList(FormModal modal, ref List<SyncedForm> SyncDocList, string SyncedType)
        {
            try
            {
                SyncedForm SyncedDoc = new SyncedForm();
                SyncedDoc.FormID = modal.FormID.ToString();
                SyncedDoc.Title = modal.Title;
                SyncedDoc.SyncType = SyncedType;
                SyncedDoc.SyncDateTime = DateTime.Now;
                SyncDocList.Add(SyncedDoc);
            }
            catch (Exception ex)
            {
                LogHelper.writelog("AddSyncList : " + ex.Message);
            }
        }
        public void PrintSyncList(List<SyncedForm> SyncDocList)
        {
            if (SyncDocList != null && SyncDocList.Count > 0)
            {
                LogHelper.writelog("Total Synced Forms " + SyncDocList.Count + " At " + DateTime.Now);
                foreach (var syncDoc in SyncDocList)
                {
                    try
                    {
                        LogHelper.writelog(syncDoc.FormID + " " + syncDoc.Title + " " + syncDoc.SyncType + " At " + DateTime.Now);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.writelog("SyncFormList" + ex.Message);
                    }
                }
            }
        }
    }
}
