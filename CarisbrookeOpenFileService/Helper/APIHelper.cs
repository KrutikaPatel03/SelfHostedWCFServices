using CarisbrookeOpenFileService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CarisbrookeOpenFileService.Helper
{
    public class APIHelper
    {
        string APIUrl = System.Configuration.ConfigurationManager.AppSettings["APIUrl"];
        public List<FormModal> GetAllFormsForService()
        {
            List<FormModal> response = new List<FormModal>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage resMsg = client.GetAsync("api/Forms/GetAllFormsForService").Result;
                    if (resMsg.IsSuccessStatusCode)
                    {
                        string resStr = resMsg.Content.ReadAsStringAsync().Result;
                        response = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FormModal>>(resStr);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("GetAllFormsForService Error : " + ex.Message);
            }
            return response;
        }
        public List<DocumentModal> GetAllDocumentsForService()
        {
            List<DocumentModal> response = new List<DocumentModal>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage resMsg = client.GetAsync("api/Documents/GetAllDocumentsForService").Result;
                    if (resMsg.IsSuccessStatusCode)
                    {
                        string resStr = resMsg.Content.ReadAsStringAsync().Result;
                        response = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DocumentModal>>(resStr);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("GetAllDocumentsForService Error : " + ex.Message);
            }
            return response;
        }
        public List<CSShipsModal> GetAllShips()
        {
            List<CSShipsModal> response = new List<CSShipsModal>();
            if (Utility.CheckInternet())
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(APIUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage resMsg = client.GetAsync("api/IMOS/GetAllShips").Result;
                        if (resMsg.IsSuccessStatusCode)
                        {
                            string resStr = resMsg.Content.ReadAsStringAsync().Result;
                            response = JsonConvert.DeserializeObject<List<CSShipsModal>>(resStr);
                            if (response != null && response.Count > 0)
                                response = response.Where(x => !x.Name.ToLower().Contains("xx") && x.Name.ToLower() != "all").OrderBy(x => x.Name).ToList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.writelog("GetAllDocuments Error : " + ex.Message);
                }
            }
            if (response == null || response.Count <= 0)
                response = GetAllShipsLocally();
            return response;
        }
        public List<CSShipsModal> GetAllShipsLocally()
        {
            try
            {
                string jsonFilePath = @"C:\inetpub\wwwroot\ShipApplication\Repository\Ships.json";
                Directory.CreateDirectory(Path.GetDirectoryName(jsonFilePath));
                if (System.IO.File.Exists(jsonFilePath))
                {
                    string jsonText = System.IO.File.ReadAllText(jsonFilePath);
                    if (!string.IsNullOrEmpty(jsonText))
                    {
                        var res = JsonConvert.DeserializeObject<List<CSShipsModal>>(jsonText);
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
        public APIResponse AddShipSystem(ShipSystemModal Modal)
        {
            APIResponse response = new APIResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    StringContent content = new StringContent(JsonConvert.SerializeObject(Modal), Encoding.UTF8, "application/json");
                    // HTTP POST
                    HttpResponseMessage resMsg = client.PostAsync("api/SystemInfo/AddShipSystem", content).Result;
                    if (resMsg.IsSuccessStatusCode)
                    {
                        string resStr = resMsg.Content.ReadAsStringAsync().Result;
                        response = JsonConvert.DeserializeObject<APIResponse>(resStr);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("AddShipSystem Error : " + ex.Message);
            }
            return response;
        }
        public APIResponse AddShipSystemInfoBulk(List<ShipSystemsInfoModal> Modal)
        {
            APIResponse response = new APIResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    StringContent content = new StringContent(JsonConvert.SerializeObject(Modal), Encoding.UTF8, "application/json");
                    // HTTP POST
                    HttpResponseMessage resMsg = client.PostAsync("api/SystemInfo/AddShipSystemInfoBulk", content).Result;
                    if (resMsg.IsSuccessStatusCode)
                    {
                        string resStr = resMsg.Content.ReadAsStringAsync().Result;
                        response = JsonConvert.DeserializeObject<APIResponse>(resStr);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("AddShipSystem Error : " + ex.Message);
            }
            return response;
        }
        public ShipSystemModal GetShipSystemByPCId(ShipSystemModal Modal)
        {
            ShipSystemModal response = new ShipSystemModal();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    StringContent content = new StringContent(JsonConvert.SerializeObject(Modal), Encoding.UTF8, "application/json");
                    // HTTP POST
                    HttpResponseMessage resMsg = client.PostAsync("api/SystemInfo/GetShipSystemByPCId", content).Result;
                    if (resMsg.IsSuccessStatusCode)
                    {
                        string resStr = resMsg.Content.ReadAsStringAsync().Result;
                        response = JsonConvert.DeserializeObject<ShipSystemModal>(resStr);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("GetShipSystemByPCId Error : " + ex.Message);
            }
            return response;
        }
        public APIResponse AddShipSystemInfo(ShipSystemsInfoModal Modal)
        {
            APIResponse response = new APIResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    StringContent content = new StringContent(JsonConvert.SerializeObject(Modal), Encoding.UTF8, "application/json");
                    // HTTP POST
                    HttpResponseMessage resMsg = client.PostAsync("api/SystemInfo/AddShipSystemInfo", content).Result;
                    if (resMsg.IsSuccessStatusCode)
                    {
                        string resStr = resMsg.Content.ReadAsStringAsync().Result;
                        response = JsonConvert.DeserializeObject<APIResponse>(resStr);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("AddShipSystemInfo Error : " + ex.Message);
            }
            return response;
        }
        public APIResponse AddShipSystemEventlog(ShipSystemsEventLogModal Modal)
        {
            APIResponse response = new APIResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    StringContent content = new StringContent(JsonConvert.SerializeObject(Modal), Encoding.UTF8, "application/json");
                    // HTTP POST
                    HttpResponseMessage resMsg = client.PostAsync("api/SystemInfo/AddShipSystemEventlog", content).Result;
                    if (resMsg.IsSuccessStatusCode)
                    {
                        string resStr = resMsg.Content.ReadAsStringAsync().Result;
                        response = JsonConvert.DeserializeObject<APIResponse>(resStr);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("AddShipSystemEventlog Error : " + ex.Message);
            }
            return response;
        }
        public APIResponse AddShipSystemsService(ShipSystemsServiceModal Modal)
        {
            APIResponse response = new APIResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    StringContent content = new StringContent(JsonConvert.SerializeObject(Modal), Encoding.UTF8, "application/json");
                    // HTTP POST
                    HttpResponseMessage resMsg = client.PostAsync("api/SystemInfo/AddShipSystemsService", content).Result;
                    if (resMsg.IsSuccessStatusCode)
                    {
                        string resStr = resMsg.Content.ReadAsStringAsync().Result;
                        response = JsonConvert.DeserializeObject<APIResponse>(resStr);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("AddShipSystemsService Error : " + ex.Message);
            }
            return response;
        }
        public APIResponse AddShipSystemsProcess(ShipSystemsProcessModal Modal)
        {
            APIResponse response = new APIResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    StringContent content = new StringContent(JsonConvert.SerializeObject(Modal), Encoding.UTF8, "application/json");
                    // HTTP POST
                    HttpResponseMessage resMsg = client.PostAsync("api/SystemInfo/AddShipSystemsProcess", content).Result;
                    if (resMsg.IsSuccessStatusCode)
                    {
                        string resStr = resMsg.Content.ReadAsStringAsync().Result;
                        response = JsonConvert.DeserializeObject<APIResponse>(resStr);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("AddShipSystemsProcess Error : " + ex.Message);
            }
            return response;
        }
        public APIResponse AddOpenFileServicesEventLog(OpenFileServicesEventLogModal Modal)
        {
            APIResponse response = new APIResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    StringContent content = new StringContent(JsonConvert.SerializeObject(Modal), Encoding.UTF8, "application/json");
                    // HTTP POST
                    HttpResponseMessage resMsg = client.PostAsync("api/ServicesInfo/AddOpenFileServicesEventLog", content).Result;
                    if (resMsg.IsSuccessStatusCode)
                    {
                        string resStr = resMsg.Content.ReadAsStringAsync().Result;
                        response = JsonConvert.DeserializeObject<APIResponse>(resStr);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("AddOpenFileServicesEventLog Error : " + ex.Message);
            }
            return response;
        }
    }
}
