namespace Monitor_HCCS.View
{
    partial class BLE
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BLE));
            this.listView1 = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.buttonEX1 = new BenNHControl.ButtonEX();
            this.panelWorkArea.SuspendLayout();
            this.panelControlArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(323, 3);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Size = new System.Drawing.Size(117, 99);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(166, 3);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOK.Size = new System.Drawing.Size(117, 99);
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panelWorkArea
            // 
            this.panelWorkArea.Controls.Add(this.buttonEX1);
            this.panelWorkArea.Controls.Add(this.listView1);
            this.panelWorkArea.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelWorkArea.MinimumSize = new System.Drawing.Size(31, 38);
            this.panelWorkArea.Size = new System.Drawing.Size(921, 566);
            // 
            // panelControlArea
            // 
            this.panelControlArea.Location = new System.Drawing.Point(6, 596);
            this.panelControlArea.Size = new System.Drawing.Size(921, 109);
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(921, 566);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "1095979.png");
            // 
            // buttonEX1
            // 
            this.buttonEX1.BackColorEX = System.Drawing.Color.Transparent;
            this.buttonEX1.BackColorLeave = System.Drawing.Color.Transparent;
            this.buttonEX1.BackColorMove = System.Drawing.Color.Transparent;
            this.buttonEX1.FontM = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonEX1.ImageDefault = null;
            this.buttonEX1.ImageLeave = null;
            this.buttonEX1.ImageMove = null;
            this.buttonEX1.Location = new System.Drawing.Point(782, 348);
            this.buttonEX1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonEX1.Name = "buttonEX1";
            this.buttonEX1.Size = new System.Drawing.Size(23, 28);
            this.buttonEX1.TabIndex = 1;
            this.buttonEX1.TextColor = System.Drawing.Color.Black;
            this.buttonEX1.TextEX = "";
            // 
            // BLE
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(933, 708);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BLE";
            this.Text = "蓝 牙";
            this.Load += new System.EventHandler(this.FormBLE_Load);
            this.panelWorkArea.ResumeLayout(false);
            this.panelControlArea.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList1;
        private BenNHControl.ButtonEX buttonEX1;

        //private DevExpress.XtraEditors.ListBoxControl listBoxControl1;

    }
}