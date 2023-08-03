using CarisbrookeOpenFileService.Helper;
using CarisbrookeOpenFileService.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CarisbrookeOpenFileService.WinForms
{
    public delegate void ShipLabelTextChangeDelegate(string newText);
    public partial class frmManageSettings : Form
    {
        public event ShipLabelTextChangeDelegate ShipLabelTextChange;
        public bool _isFirstTime;
        public frmManageSettings()
        {
            InitializeComponent();
        }


        private void frmManageSettings_Load(object sender, EventArgs e)
        {
            try
            {
                BindShipComboList();
                if (Globals.CurrentShip != null && Globals.CurrentShip.Code != "")
                    cmbShip.SelectedValue = Globals.CurrentShip.Code;
            }
            catch (Exception)
            { }
        }

        private void BindShipComboList()
        {
            try
            {
                APIHelper _helper = new APIHelper();
                var lstShips = _helper.GetAllShips();
                if (lstShips == null)
                    lstShips = new List<Models.CSShipsModal>();
                lstShips.Insert(0, new Models.CSShipsModal
                {
                    Code = "",
                    Name = "--Select--"
                });
                cmbShip.DataSource = lstShips;
                cmbShip.DisplayMember = "Name";
                cmbShip.ValueMember = "Code";
            }
            catch (Exception)
            {

            }
        }

        private void cmbShip_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


            }
            catch (Exception)
            {

            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (cmbShip.SelectedValue == "")
                MessageBox.Show("Please select Ship to continue.");
            else
            {
                //Save Ship Code
                SaveShip();
                if (ShipLabelTextChange != null)
                    ShipLabelTextChange(Globals.CurrentShip.Name);
                //if (btnNext.Text.ToLower() == "next")
                //{
                //    var form = new frmManageService();
                this.Hide();
                //    form.ShowDialog();
                this.Close();
                //}
            }
        }

        private void SaveShip()
        {
            try
            {
                if (cmbShip.SelectedValue != "")
                {
                    SystemInfoHelper _helper = new SystemInfoHelper();
                    var objShip = (Models.CSShipsModal)cmbShip.SelectedItem;
                    if (objShip.Code != "")
                    {
                        string PCUniqueId = _helper.GetPCUniqueId();
                        objShip.PCUniqueId = PCUniqueId;
                        Globals.CurrentShip = objShip;
                        var objShipSystem = new ShipSystemModal
                        {
                            CreatedDate = DateTime.Now,
                            PCName = Environment.UserDomainName,
                            PCUniqueId = objShip.PCUniqueId,
                            ShipCode = objShip.Code
                        };
                        APIHelper _apihelper = new APIHelper();
                        var result = _apihelper.AddShipSystem(objShipSystem);
                        if (result.result.ToLower() == "success")
                        {
                            var res = _apihelper.GetShipSystemByPCId(objShipSystem);
                            if (res != null)
                            {
                                objShip.ShipSystemsId = res.Id;
                            }
                            _helper.SaveShipJson(objShip);
                            MessageBox.Show("Ship saved successfully.");
                        }

                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
