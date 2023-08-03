using CarisbrookeOpenFileService.Helper;
using CarisbrookeOpenFileService.WinForms;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace CarisbrookeOpenFileService
{
    class Program
    {
        // private static Timer aTimer;
        static void Main(string[] args)
        {
            try
            {
                try
                {
                    foreach (Process proc in Process.GetProcesses())
                    {
                        if (proc.ProcessName.Equals(Process.GetCurrentProcess().ProcessName) && proc.Id != Process.GetCurrentProcess().Id)
                        {
                            proc.Kill();
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    if (IsApplicationAlreadyRunning())
                    {
                        MessageBox.Show("Another instance of this application is running. Please close that service before starting new.");
                        Environment.Exit(0);
                    }
                }

                Form form;
                //SystemInfoHelper _helper = new SystemInfoHelper();
                //var objShip = _helper.GetShipJson();
                //if (objShip == null)
                //{
                //    form = new frmManageSettings();
                //    form.Tag = true;
                //}     

                //else
                form = new frmManageService();

                Application.EnableVisualStyles();
                Application.Run(form);

                new ManualResetEvent(false).WaitOne();

            }
            catch (Exception ex)
            {
                var notification = new NotifyIcon()
                {
                    Visible = true,
                    Icon = System.Drawing.SystemIcons.Information,
                    Text = "Carisbrooke File Service",
                    BalloonTipTitle = "Carisbrooke File Service",
                    BalloonTipText = "Service is failed due to : " + ex.Message,
                };
                // Display for 5 seconds.
                notification.ShowBalloonTip(10000);
                LogHelper.writelog(" Error:--" + ex.Message);
            }
        }

        static bool IsApplicationAlreadyRunning()
        {
            string proc = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(proc);
            if (processes.Length > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
