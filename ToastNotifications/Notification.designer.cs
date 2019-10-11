namespace ToastNotifications
{
    partial class Notification
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Notification));
            this.lifeTimer = new System.Windows.Forms.Timer(this.components);
            this.labelControlBody = new System.Windows.Forms.Label();
            this.labelControlTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lifeTimer
            // 
            this.lifeTimer.Tick += new System.EventHandler(this.lifeTimer_Tick);
            // 
            // labelControlBody
            // 
            this.labelControlBody.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelControlBody.BackColor = System.Drawing.Color.Transparent;
            this.labelControlBody.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControlBody.ForeColor = System.Drawing.Color.White;
            this.labelControlBody.Location = new System.Drawing.Point(7, 29);
            this.labelControlBody.Name = "labelControlBody";
            this.labelControlBody.Size = new System.Drawing.Size(236, 42);
            this.labelControlBody.TabIndex = 0;
            this.labelControlBody.Text = "Body goes here and here and here and here and here";
            this.labelControlBody.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelControlBody.Click += new System.EventHandler(this.labelControlRO_Click);
            // 
            // labelControlTitle
            // 
            this.labelControlTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelControlTitle.Font = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControlTitle.ForeColor = System.Drawing.Color.Gainsboro;
            this.labelControlTitle.Location = new System.Drawing.Point(3, 1);
            this.labelControlTitle.Name = "labelControlTitle";
            this.labelControlTitle.Size = new System.Drawing.Size(253, 21);
            this.labelControlTitle.TabIndex = 0;
            this.labelControlTitle.Text = "title goes here";
            this.labelControlTitle.Click += new System.EventHandler(this.labelControlTitle_Click);
            // 
            // Notification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(255, 80);
            this.ControlBox = false;
            this.Controls.Add(this.labelControlTitle);
            this.Controls.Add(this.labelControlBody);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Notification";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "EDGE Shop Flag Notification";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.Notification_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Notification_FormClosed);
            this.Load += new System.EventHandler(this.Notification_Load);
            this.Shown += new System.EventHandler(this.Notification_Shown);
            this.Click += new System.EventHandler(this.Notification_Click);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer lifeTimer;
        private System.Windows.Forms.Label labelControlBody;
        private System.Windows.Forms.Label labelControlTitle;
    }
}