using CarisbrookeOpenFileService.Helper;
using System;
using System.Timers;

namespace CarisbrookeOpenFileService.Services
{
    public class SyncSystemInfoService
    {
        Timer _timer = new Timer();
        private volatile bool _executing;
        static string _ScheduledRunningTime = "12:00 PM";
        public void OnStart()
        {
            LogHelper.writelog("---------Sync System Info Service Started At " + DateTime.Now.ToString() + " -----------");
            _timer.Interval = TimeSpan.FromMinutes(1).TotalMilliseconds;//Every one minute
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Start();
        }
        public void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_executing)
                return;

            _executing = true;
            try
            {
                string _CurrentTime = String.Format("{0:t}", DateTime.Now);
                if (_CurrentTime == _ScheduledRunningTime)
                {
                    LogHelper.writelog("System Info Interval Start " + DateTime.Now.ToString());
                    _timer.Enabled = false;
                    StartSync();                    
                    LogHelper.writelog("System Info Interval End " + DateTime.Now.ToString());
                    LogHelper.writelog(LogHelper.GetEndLine());
                    setTimeInterval(1);
                }
            }
            catch (Exception ex)
            {
                LogHelper.writelog("System Info _timer_Elapsed Error : " + DateTime.Now.ToString() + " " + ex.Message);
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
                LogHelper.writelog("System Info OnStop called");
            }
            catch (Exception ex)
            {
                LogHelper.writelog("System Info OnStop error - " + ex);
            }
        }

        public bool StartSync()
        {
            bool isUpdated = false;
            SystemInfoHelper _helper = new SystemInfoHelper();
            _helper.SyncSystemInfoData();

            LogHelper.writelog("System Info Sync Done");
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
