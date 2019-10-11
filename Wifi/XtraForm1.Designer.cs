namespace WifiTool
{
    partial class XtraForm1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XtraForm1));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.imageList1 = new System.Windows.Forms.ImageList();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
            this.myListBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold);
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(13, 13);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(182, 38);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "当前连接网络：";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold);
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Location = new System.Drawing.Point(201, 13);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(182, 38);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "未连接";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Appearance.Font = new System.Drawing.Font("微软雅黑", 22F, System.Drawing.FontStyle.Bold);
            this.simpleButton2.Appearance.Options.UseFont = true;
            this.simpleButton2.Location = new System.Drawing.Point(535, 557);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 3, 40, 3);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(150, 80);
            this.simpleButton2.TabIndex = 4;
            this.simpleButton2.Text = "退出";
            this.simpleButton2.Click += new System.EventHandler(this.TxButton3_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Appearance.Font = new System.Drawing.Font("微软雅黑", 22F, System.Drawing.FontStyle.Bold);
            this.simpleButton3.Appearance.Options.UseFont = true;
            this.simpleButton3.Location = new System.Drawing.Point(342, 557);
            this.simpleButton3.Margin = new System.Windows.Forms.Padding(3, 3, 40, 3);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(150, 80);
            this.simpleButton3.TabIndex = 5;
            this.simpleButton3.Text = "刷新";
            this.simpleButton3.Click += new System.EventHandler(this.txButton2_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "1095979.png");
            this.imageList1.Images.SetKeyName(1, "1095971.png");
            this.imageList1.Images.SetKeyName(2, "1095992.png");
            this.imageList1.Images.SetKeyName(3, "1095996.png");
            this.imageList1.Images.SetKeyName(4, "1095996.png");
            this.imageList1.Images.SetKeyName(5, "1096004.png");
            this.imageList1.Images.SetKeyName(6, "1096012.png");
            this.imageList1.Images.SetKeyName(7, "1096018.png");
            this.imageList1.Images.SetKeyName(8, "1096032.png");
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("微软雅黑", 22F, System.Drawing.FontStyle.Bold);
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Location = new System.Drawing.Point(149, 557);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 3, 40, 3);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(150, 80);
            this.simpleButton1.TabIndex = 6;
            this.simpleButton1.Text = "连接";
            this.simpleButton1.Click += new System.EventHandler(this.txButton1_Click);
            // 
            // myListBox1
            // 
            this.myListBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.myListBox1.FormattingEnabled = true;
            this.myListBox1.ItemHeight = 40;
            this.myListBox1.Location = new System.Drawing.Point(13, 57);
            this.myListBox1.Name = "myListBox1";
            this.myListBox1.Size = new System.Drawing.Size(709, 484);
            this.myListBox1.TabIndex = 7;
            this.myListBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.myListBox1_DrawItem);
            this.myListBox1.SelectedIndexChanged += new System.EventHandler(this.myListBox1_SelectedIndexChanged);
            // 
            // XtraForm1
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(734, 649);
            this.ControlBox = false;
            this.Controls.Add(this.myListBox1);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Font = new System.Drawing.Font("微软雅黑", 22F, System.Drawing.FontStyle.Bold);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "XtraForm1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WIFI";
            this.Load += new System.EventHandler(this.MyWifi_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ListBox myListBox1;
    }
}