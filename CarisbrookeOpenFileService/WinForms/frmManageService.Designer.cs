namespace CarisbrookeOpenFileService.WinForms
{
    partial class frmManageService
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManageService));
            this.btnStartService = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblLastUpdatedTime = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnExitApplication = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblSelectedShip = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartService
            // 
            this.btnStartService.Location = new System.Drawing.Point(34, 94);
            this.btnStartService.Name = "btnStartService";
            this.btnStartService.Size = new System.Drawing.Size(92, 33);
            this.btnStartService.TabIndex = 0;
            this.btnStartService.Text = "Start Service";
            this.btnStartService.UseVisualStyleBackColor = true;
            this.btnStartService.Click += new System.EventHandler(this.btnStartService_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Service";
            // 
            // lblLastUpdatedTime
            // 
            this.lblLastUpdatedTime.AutoSize = true;
            this.lblLastUpdatedTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastUpdatedTime.Location = new System.Drawing.Point(57, 59);
            this.lblLastUpdatedTime.Name = "lblLastUpdatedTime";
            this.lblLastUpdatedTime.Size = new System.Drawing.Size(16, 15);
            this.lblLastUpdatedTime.TabIndex = 4;
            this.lblLastUpdatedTime.Text = "...";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "...";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // btnExitApplication
            // 
            this.btnExitApplication.Location = new System.Drawing.Point(132, 94);
            this.btnExitApplication.Name = "btnExitApplication";
            this.btnExitApplication.Size = new System.Drawing.Size(92, 33);
            this.btnExitApplication.TabIndex = 5;
            this.btnExitApplication.Text = "Exit";
            this.btnExitApplication.UseVisualStyleBackColor = true;
            this.btnExitApplication.Click += new System.EventHandler(this.btnExitApplication_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblSelectedShip);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(6, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 131);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Info";
            // 
            // lblSelectedShip
            // 
            this.lblSelectedShip.AutoSize = true;
            this.lblSelectedShip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedShip.Location = new System.Drawing.Point(51, 25);
            this.lblSelectedShip.Name = "lblSelectedShip";
            this.lblSelectedShip.Size = new System.Drawing.Size(16, 15);
            this.lblSelectedShip.TabIndex = 6;
            this.lblSelectedShip.Text = "...";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Ship : ";
            // 
            // frmManageService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 154);
            this.Controls.Add(this.btnExitApplication);
            this.Controls.Add(this.lblLastUpdatedTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStartService);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmManageService";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "File Service";
            this.Load += new System.EventHandler(this.frmManageService_Load);
            this.Resize += new System.EventHandler(this.frmManageService_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnStartService;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblLastUpdatedTime;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button btnExitApplication;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblSelectedShip;
        private System.Windows.Forms.Label label3;
    }
}