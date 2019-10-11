namespace WifiTool
{
    partial class MyWifi
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txTableLayoutPanel1 = new TX.Framework.WindowUI.Controls.TXTableLayoutPanel();
            this.labelControl1 = new System.Windows.Forms.Label();
            this.txButton3 = new TX.Framework.WindowUI.Controls.TXButton();
            this.txButton2 = new TX.Framework.WindowUI.Controls.TXButton();
            this.txButton1 = new TX.Framework.WindowUI.Controls.TXButton();
            this.labelControl2 = new System.Windows.Forms.Label();
            this.myListBox1 = new System.Windows.Forms.ListBox();
            this.txTableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txTableLayoutPanel1
            // 
            this.txTableLayoutPanel1.AutoSize = true;
            this.txTableLayoutPanel1.BackColor = System.Drawing.Color.AliceBlue;
            this.txTableLayoutPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(168)))), ((int)(((byte)(192)))));
            this.txTableLayoutPanel1.ColumnCount = 10;
            this.txTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.txTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.txTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.txTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.txTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.txTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.txTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.txTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.txTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.txTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.txTableLayoutPanel1.Controls.Add(this.labelControl1, 0, 0);
            this.txTableLayoutPanel1.Controls.Add(this.txButton3, 9, 2);
            this.txTableLayoutPanel1.Controls.Add(this.txButton2, 7, 2);
            this.txTableLayoutPanel1.Controls.Add(this.txButton1, 5, 2);
            this.txTableLayoutPanel1.Controls.Add(this.labelControl2, 2, 0);
            this.txTableLayoutPanel1.Controls.Add(this.myListBox1, 0, 1);
            this.txTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.txTableLayoutPanel1.Name = "txTableLayoutPanel1";
            this.txTableLayoutPanel1.RowCount = 3;
            this.txTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.txTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 78F));
            this.txTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.txTableLayoutPanel1.Size = new System.Drawing.Size(600, 450);
            this.txTableLayoutPanel1.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.AutoSize = true;
            this.txTableLayoutPanel1.SetColumnSpan(this.labelControl1, 2);
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl1.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl1.Location = new System.Drawing.Point(3, 0);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(114, 45);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "当前连接的网络";
            this.labelControl1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txButton3
            // 
            this.txButton3.AutoSize = true;
            this.txButton3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txButton3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txButton3.Image = null;
            this.txButton3.Location = new System.Drawing.Point(543, 399);
            this.txButton3.Name = "txButton3";
            this.txButton3.Size = new System.Drawing.Size(54, 48);
            this.txButton3.TabIndex = 2;
            this.txButton3.Text = "退出";
            this.txButton3.UseVisualStyleBackColor = true;
            this.txButton3.Click += new System.EventHandler(this.TxButton3_Click);
            // 
            // txButton2
            // 
            this.txButton2.AutoSize = true;
            this.txButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txButton2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txButton2.Image = null;
            this.txButton2.Location = new System.Drawing.Point(423, 399);
            this.txButton2.Name = "txButton2";
            this.txButton2.Size = new System.Drawing.Size(54, 48);
            this.txButton2.TabIndex = 1;
            this.txButton2.Text = "刷新";
            this.txButton2.UseVisualStyleBackColor = true;
            this.txButton2.Click += new System.EventHandler(this.txButton2_Click);
            // 
            // txButton1
            // 
            this.txButton1.AutoSize = true;
            this.txButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txButton1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txButton1.Image = null;
            this.txButton1.Location = new System.Drawing.Point(303, 399);
            this.txButton1.Name = "txButton1";
            this.txButton1.Size = new System.Drawing.Size(54, 48);
            this.txButton1.TabIndex = 0;
            this.txButton1.Text = "连接";
            this.txButton1.UseVisualStyleBackColor = true;
            this.txButton1.Click += new System.EventHandler(this.txButton1_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.AutoSize = true;
            this.txTableLayoutPanel1.SetColumnSpan(this.labelControl2, 2);
            this.labelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl2.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl2.Location = new System.Drawing.Point(123, 0);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(114, 45);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "未连接";
            this.labelControl2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // myListBox1
            // 
            this.myListBox1.BackColor = System.Drawing.Color.AliceBlue;
            this.txTableLayoutPanel1.SetColumnSpan(this.myListBox1, 10);
            this.myListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myListBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.myListBox1.FormattingEnabled = true;
            this.myListBox1.IntegralHeight = false;
            this.myListBox1.ItemHeight = 21;
            this.myListBox1.Location = new System.Drawing.Point(3, 48);
            this.myListBox1.Name = "myListBox1";
            this.myListBox1.Size = new System.Drawing.Size(594, 345);
            this.myListBox1.TabIndex = 5;
            this.myListBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.myListBox1_DrawItem);
            // 
            // MyWifi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.ClientSize = new System.Drawing.Size(600, 450);
            this.Controls.Add(this.txTableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MyWifi";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MyWifi";
            this.Load += new System.EventHandler(this.MyWifi_Load);
            this.txTableLayoutPanel1.ResumeLayout(false);
            this.txTableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TX.Framework.WindowUI.Controls.TXTableLayoutPanel txTableLayoutPanel1;
        private System.Windows.Forms.Label labelControl1;
        private TX.Framework.WindowUI.Controls.TXButton txButton3;
        private TX.Framework.WindowUI.Controls.TXButton txButton2;
        private TX.Framework.WindowUI.Controls.TXButton txButton1;
        private System.Windows.Forms.Label labelControl2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ListBox myListBox1;
    }
}