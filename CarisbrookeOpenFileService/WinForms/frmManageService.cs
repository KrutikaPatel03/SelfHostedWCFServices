using CarisbrookeOpenFileService.Helper;
using CarisbrookeOpenFileService.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Device.Location;
using System.Windows.Forms;

namespace CarisbrookeOpenFileService.WinForms
{
    public partial class frmManageService : Form
    {
        #region Properties
        private CorsEnabledServiceHost host;
        private ServiceAddData serv;
        private SyncSystemInfoService sysInfo;
        private ContextMenu contextMenu1;
        private MenuItem menuItem1, menuItem2;
        #endregion

        #region Forms Events
        public frmManageService()
        {
            try
            {
                InitializeComponent();
                this.FormClosing += this.frmManageService_FormClosing;
                this.contextMenu1 = new ContextMenu();
                this.menuItem1 = new MenuItem();
                this.menuItem2 = new MenuItem();

                // Initialize contextMenu1
                this.contextMenu1.MenuItems.AddRange(
                            new MenuItem[] { this.menuItem2, this.menuItem1 });

                // Initialize menuItem2
                this.menuItem2.Index = 0;
                this.menuItem2.Text = "S&ettings";
                this.menuItem2.Click += new EventHandler(this.btnSettingsApplication_Click);

                // Initialize menuItem1
                this.menuItem1.Index = 1;
                this.menuItem1.Text = "E&xit";
                this.menuItem1.Click += new EventHandler(this.btnExitApplication_Click);



                notifyIcon1.ContextMenu = this.contextMenu1;

                //Initialize notification double click
                this.notifyIcon1.MouseDoubleClick += new MouseEventHandler(notifyIcon1_MouseDoubleClick);
            }
            catch (Exception)
            { }
        }

        private void frmManageService_Load(object sender, EventArgs e)
        {
            try
            {
                btnStartService.Text = "Stop Service";
                SystemInfoHelper _helper = new SystemInfoHelper();
                Form form;
                var objShip = _helper.GetShipJson();
                if (objShip == null)
                {
                    form = new frmManageSettings();
                    form.Tag = true;
                    form.ShowDialog();
                }
                else
                    Globals.CurrentShip = objShip;
                SyncServiceStatus(true);
                lblSelectedShip.Text = Globals.CurrentShip.Name;
                LogHelper.writelog("Start forms sync...");
                serv = new ServiceAddData(this);
                sysInfo = new SyncSystemInfoService();
                lblLastUpdatedTime.Text = "started at " + DateTime.Now.ToString();
                StartWCFService();
            }
            catch (Exception ex)
            {

            }
        }
        private void frmManageService_Resize(object sender, EventArgs e)
        {
            try
            {
                if (FormWindowState.Minimized == this.WindowState)
                {
                    this.notifyIcon1.BalloonTipText = "File Service is running...";
                    notifyIcon1.Visible = true;
                    notifyIcon1.ShowBalloonTip(500);
                    Hide();
                }
                else if (FormWindowState.Normal == this.WindowState)
                {
                    notifyIcon1.Visible = false;
                    Show();
                }
            }
            catch (Exception)
            {

            }
        }
        private void frmManageService_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                this.Hide();
            }
        }
        #endregion

        #region Buttons Events
        private void btnStartService_Click(object sender, EventArgs e)
        {
            switch (btnStartService.Text)
            {
                case "Stop Service": //Stop Service
                    if (host.State == System.ServiceModel.CommunicationState.Opened)
                    {
                        host.Close();
                        serv.OnStop();
                        sysInfo.OnStop();
                        //Notify Service is Stoped
                        lblLastUpdatedTime.Text = "stopped at " + DateTime.Now.ToString();
                        this.notifyIcon1.BalloonTipText = "Service is stopped..";
                        notifyIcon1.Visible = true;
                        notifyIcon1.ShowBalloonTip(500);
                    }
                    btnStartService.Text = "Start Service";
                    break;
                case "Start Service": //Stop Service
                    if (host.State == System.ServiceModel.CommunicationState.Closed)
                    {
                        StartWCFService();
                        lblLastUpdatedTime.Text = "started at " + DateTime.Now.ToString();
                    }
                    btnStartService.Text = "Stop Service";
                    break;
                default:
                    break;
            }
        }
        private void btnUpdateForms_Click(object sender, EventArgs e)
        {
            UpdateForms();
        }
        private void btnExitApplication_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you really want to exit?", "File Service", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                SyncServiceStatus(false);
                notifyIcon1.Visible = false;
                notifyIcon1.Dispose();
                if (host.State == System.ServiceModel.CommunicationState.Opened)
                    host.Close();
                serv.OnStop();
                sysInfo.OnStop();
                //Delete Json Files
                try
                {
                    FormsHelper _FormHelper = new FormsHelper();
                    _FormHelper.DeleteOldFile(ConfigurationManager.AppSettings["FormsListPath"], "");

                    DocumentsHelper _DocHelper = new DocumentsHelper();
                    _DocHelper.DeleteOldFile(ConfigurationManager.AppSettings["DocumentsListPath"], "");
                }
                catch (Exception)
                {
                }
                Environment.Exit(0);
            }
        }
        private void btnSettingsApplication_Click(object sender, EventArgs e)
        {
            var form = new frmManageSettings();
            form.ShipLabelTextChange += frmManageService_ShipLabelTextChange;
            form.ShowDialog();
        }
        #endregion

        #region Notification Events
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
        }
        public void NotifyMe()
        {
            notifyIcon1.BalloonTipText = "Files are upto date...";
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(500);
        }
        #endregion

        #region Methods
        private void StartWCFService()
        {
            try
            {
                LogHelper.writelog("Starting service hosting...");
                var epAddress = ConfigurationManager.AppSettings["ServiceEndPoint"];
                Uri baseAddress = new Uri(epAddress);
                host = new CorsEnabledServiceHost(typeof(OpenFileService), baseAddress);
                host.Open();
                this.notifyIcon1.BalloonTipText = "Service is started... Please wait till files are updated.. We will notify you..";
                this.notifyIcon1.BalloonTipTitle = "Carisbrooke File Service";
                this.notifyIcon1.Text = "Carisbrooke File Service";
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
                LogHelper.writelog("Host ready on " + epAddress + " ...");
            }
            catch (Exception ex)
            {
                LogHelper.writelog("StartWCFService - " + ex.Message + " ...");
            }
            try
            {
                serv.OnStart();
            }
            catch (Exception ex)
            {
                LogHelper.writelog("StartWCFService Sync function call- " + ex.Message + " ...");
            }
            try
            {
                sysInfo.OnStart();
            }
            catch (Exception ex)
            {
                LogHelper.writelog("StartWCFService sysInfo function call- " + ex.Message + " ...");
            }
        }
        private void UpdateForms()
        {
            try
            {
                serv.StartSync();
                notifyIcon1.BalloonTipText = "Files are upto date...";
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
                new System.Threading.ManualResetEvent(false).WaitOne();
            }
            catch (Exception ex)
            {
            }
        }
        void frmManageService_ShipLabelTextChange(string newText)
        {
            lblSelectedShip.Text = newText;
        }

        private void SyncServiceStatus(bool isStart)
        {
            try
            {
                GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
                watcher.Start();
                var whereat = watcher.Position.Location;
                string Lat = "0", Lon = "0";
                if (watcher.TryStart(false, TimeSpan.FromSeconds(3)))
                {
                    if (!whereat.IsUnknown)
                    {
                        Lat = whereat.Latitude.ToString("0.000000");
                        Lon = whereat.Longitude.ToString("0.000000");
                    }
                }
                string ver = ConfigurationManager.AppSettings["AppVersion"];
                var objServiceInfo = new Models.OpenFileServicesEventLogModal
                {
                    IsActive = isStart,
                    RunningVersion = ver,
                    ShipSystemId = Globals.CurrentShip.ShipSystemsId,
                    Latitude = Convert.ToDecimal(Lat),
                    Longitude = Convert.ToDecimal(Lon)
                };

                APIHelper _apihelper = new APIHelper();
                var result = _apihelper.AddOpenFileServicesEventLog(objServiceInfo);
            }
            catch (Exception ex)
            {
                LogHelper.writelog("SyncServiceStatus function call- " + ex.Message + " ...");
            }
        }
        #endregion
    }
}
