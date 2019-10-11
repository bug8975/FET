namespace BenNHControl
{
    partial class Toast
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
            this.lblTitleBar = new System.Windows.Forms.Label();
            this.lblTitalContent = new System.Windows.Forms.Label();
            this.messageLabel = new System.Windows.Forms.Label();
            this.lifeTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lblTitleBar
            // 
            this.lblTitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(116)))), ((int)(((byte)(151)))));
            this.lblTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleBar.Location = new System.Drawing.Point(0, 0);
            this.lblTitleBar.Name = "lblTitleBar";
            this.lblTitleBar.Size = new System.Drawing.Size(354, 30);
            this.lblTitleBar.TabIndex = 0;
            // 
            // lblTitalContent
            // 
            this.lblTitalContent.AutoSize = true;
            this.lblTitalContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(116)))), ((int)(((byte)(151)))));
            this.lblTitalContent.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitalContent.ForeColor = System.Drawing.Color.White;
            this.lblTitalContent.Location = new System.Drawing.Point(11, 6);
            this.lblTitalContent.Name = "lblTitalContent";
            this.lblTitalContent.Size = new System.Drawing.Size(37, 19);
            this.lblTitalContent.TabIndex = 2;
            this.lblTitalContent.Text = "提示";
            // 
            // messageLabel
            // 
            this.messageLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(136)))));
            this.messageLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.messageLabel.ForeColor = System.Drawing.Color.White;
            this.messageLabel.Location = new System.Drawing.Point(36, 52);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(295, 83);
            this.messageLabel.TabIndex = 3;
            this.messageLabel.Text = "暂无信息!";
            // 
            // lifeTimer
            // 
            this.lifeTimer.Tick += new System.EventHandler(this.lifeTimer_Tick);
            // 
            // Toast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(136)))));
            this.ClientSize = new System.Drawing.Size(354, 100);
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.lblTitalContent);
            this.Controls.Add(this.lblTitleBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Toast";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Toast";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.Toast_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Toast_FormClosed);
            this.Load += new System.EventHandler(this.Toast_Load);
            this.Shown += new System.EventHandler(this.Toast_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitleBar;
        private System.Windows.Forms.Label lblTitalContent;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Timer lifeTimer;
    }
}