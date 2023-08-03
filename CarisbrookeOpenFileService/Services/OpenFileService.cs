using CarisbrookeOpenFileService.Helper;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;

namespace CarisbrookeOpenFileService.Services
{
    public class OpenFileService : IOpenFileService
    {
        public string OpenFile(string value)
        {
            string status = string.Empty;
            try
            {
                LogHelper.writelog("File to open - " + value);
                value = value.Replace("&quot;", "");
                string path = string.Format(@"{0}", value);
                try
                {
                    var regAdobe = Registry.CurrentUser.OpenSubKey(@"Software\Adobe\Acrobat Reader\DC\Privileged", true);
                    regAdobe.SetValue("bProtectedMode", 0);
                }
                catch (Exception e)
                {
                    LogHelper.writelog("" + e.Message);
                }
                ProcessStartInfo psi = new ProcessStartInfo();                
                psi.FileName = Path.GetFileName(path);
                psi.WorkingDirectory = Path.GetDirectoryName(path);
                psi.WindowStyle = ProcessWindowStyle.Maximized;
                Process.Start(psi);
                status = "";               
            }
            catch (Exception ex)
            {
                status = string.Format("OpenFile - Error - {0}", ex.Message);
                LogHelper.writelog(status);
            }
            return status;
        }
        public string OpenFolder(string value)
        {
            string status = string.Empty;
            try
            {
                LogHelper.writelog("Folder to open - " + value);
                value = value.Replace("&quot;", "");
                string path = string.Format(@"{0}", value);
                Process.Start("explorer.exe", path);
                status = "";
            }
            catch (Exception ex)
            {
                status = string.Format("OpenFolder - Error - {0}", ex.Message);
                LogHelper.writelog(status);
            }
            return status;
        }        
    }
}
