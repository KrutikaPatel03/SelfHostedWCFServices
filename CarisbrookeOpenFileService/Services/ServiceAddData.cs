using CarisbrookeOpenFileService.Helper;
using CarisbrookeOpenFileService.WinForms;
using System;
using System.Timers;

namespace CarisbrookeOpenFileService.Services
{
    public class ServiceAddData
    {
        Timer _timer = new Timer(100);
        frmManageService objForm;
        private volatile bool _executing;
        public ServiceAddData() {

        }
        public ServiceAddData(frmManageService tempForm)
        {
            objForm = tempForm;
        }
        public void OnStart()
        {
            LogHelper.writelog("---------Sync Service Started At " + DateTime.Now.ToString() + " -----------");
            _timer.Elapsed += _timer_Elapsed;            
            _timer.Start();
        }
        public void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_executing)
                return;

            _executing = true;
            try
            {
                LogHelper.writelog("Sync Service Inteval Start " + DateTime.Now.ToString());
                bool isInternetAvailable = Utility.CheckInternet();
                if (isInternetAvailable)
                {
                    LogHelper.writelog("Internet Available and start syncing proccess....");
                    _timer.Enabled = false;
                    if (StartSync())
                        objForm.NotifyMe();                    
                    setTimeInterval(5);
                }
                LogHelper.writelog("Sync Service Inteval End " + DateTime.Now.ToString());
                LogHelper.writelog(LogHelper.GetEndLine());
            }
            catch (Exception ex)
            {
                LogHelper.writelog("Sync Service _timer_Elapsed Error : " + DateTime.Now.ToString() + " " + ex.Message);
            }
            finally
            {
                _executing = false;
            }
        }
        public void OnStop()
        {
            try
            {
                _timer.Enabled = false;
                LogHelper.writelog("Sync Service OnStop called");
            }
            catch (Exception ex)
            {
                LogHelper.writelog("Sync Service OnStop error - " + ex);
            }
        }

        public bool StartSync()
        {
            bool isUpdated = false;
            FormsHelper _FormHelper = new FormsHelper();
            isUpdated = _FormHelper.StartFormsync();

            DocumentsHelper _DocumentHelper = new DocumentsHelper();
            _DocumentHelper.StartDocSync();
            LogHelper.writelog("Forms and Documents Sync Done");
            return isUpdated;
        }

        public void setTimeInterval(Double Minutes)
        {
            _timer.Enabled = false;
            _timer.Interval = Minutes * 60000;
            _timer.Enabled = true;
        }
    }
}
