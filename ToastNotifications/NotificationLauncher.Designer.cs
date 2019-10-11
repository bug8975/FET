namespace ToastNotifications
{
    partial class NotificationLauncher
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
            this.labelControlTitle = new System.Windows.Forms.Label();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.labelControlBody = new System.Windows.Forms.Label();
            this.textBoxBody = new System.Windows.Forms.TextBox();
            this.labelControlDuration = new System.Windows.Forms.Label();
            this.comboBoxDuration = new System.Windows.Forms.ComboBox();
            this.labelControlAnimation = new System.Windows.Forms.Label();
            this.comboBoxAnimation = new System.Windows.Forms.ComboBox();
            this.comboBoxAnimationDirection = new System.Windows.Forms.ComboBox();
            this.labelControlAnimationDirection = new System.Windows.Forms.Label();
            this.buttonShowNotification = new System.Windows.Forms.Button();
            this.comboBoxSound = new System.Windows.Forms.ComboBox();
            this.labelControlSound = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelControlTitle
            // 
            this.labelControlTitle.AutoSize = true;
            this.labelControlTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControlTitle.Location = new System.Drawing.Point(13, 18);
            this.labelControlTitle.Name = "labelControlTitle";
            this.labelControlTitle.Size = new System.Drawing.Size(32, 13);
            this.labelControlTitle.TabIndex = 0;
            this.labelControlTitle.Text = "Title";
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.ImeMode = System.Windows.Forms.ImeMode.On;
            this.textBoxTitle.Location = new System.Drawing.Point(89, 15);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(191, 21);
            this.textBoxTitle.TabIndex = 1;
            this.textBoxTitle.Text = "My Notification";
            // 
            // labelControlBody
            // 
            this.labelControlBody.AutoSize = true;
            this.labelControlBody.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControlBody.Location = new System.Drawing.Point(13, 52);
            this.labelControlBody.Name = "labelControlBody";
            this.labelControlBody.Size = new System.Drawing.Size(35, 13);
            this.labelControlBody.TabIndex = 2;
            this.labelControlBody.Text = "Body";
            // 
            // textBoxBody
            // 
            this.textBoxBody.Location = new System.Drawing.Point(89, 49);
            this.textBoxBody.Multiline = true;
            this.textBoxBody.Name = "textBoxBody";
            this.textBoxBody.Size = new System.Drawing.Size(191, 47);
            this.textBoxBody.TabIndex = 3;
            this.textBoxBody.Text = "My notification message goes here";
            // 
            // labelControlDuration
            // 
            this.labelControlDuration.AutoSize = true;
            this.labelControlDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControlDuration.Location = new System.Drawing.Point(13, 118);
            this.labelControlDuration.Name = "labelControlDuration";
            this.labelControlDuration.Size = new System.Drawing.Size(55, 13);
            this.labelControlDuration.TabIndex = 4;
            this.labelControlDuration.Text = "Duration";
            // 
            // comboBoxDuration
            // 
            this.comboBoxDuration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDuration.FormattingEnabled = true;
            this.comboBoxDuration.Items.AddRange(new object[] {
            "sticky (click to dismiss)",
            "1",
            "3",
            "5",
            "10"});
            this.comboBoxDuration.Location = new System.Drawing.Point(89, 118);
            this.comboBoxDuration.Name = "comboBoxDuration";
            this.comboBoxDuration.Size = new System.Drawing.Size(191, 20);
            this.comboBoxDuration.TabIndex = 5;
            // 
            // labelControlAnimation
            // 
            this.labelControlAnimation.AutoSize = true;
            this.labelControlAnimation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControlAnimation.Location = new System.Drawing.Point(14, 146);
            this.labelControlAnimation.Name = "labelControlAnimation";
            this.labelControlAnimation.Size = new System.Drawing.Size(62, 13);
            this.labelControlAnimation.TabIndex = 6;
            this.labelControlAnimation.Text = "Animation";
            // 
            // comboBoxAnimation
            // 
            this.comboBoxAnimation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAnimation.FormattingEnabled = true;
            this.comboBoxAnimation.Location = new System.Drawing.Point(89, 143);
            this.comboBoxAnimation.Name = "comboBoxAnimation";
            this.comboBoxAnimation.Size = new System.Drawing.Size(191, 20);
            this.comboBoxAnimation.TabIndex = 7;
            // 
            // comboBoxAnimationDirection
            // 
            this.comboBoxAnimationDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAnimationDirection.FormattingEnabled = true;
            this.comboBoxAnimationDirection.Location = new System.Drawing.Point(89, 168);
            this.comboBoxAnimationDirection.Name = "comboBoxAnimationDirection";
            this.comboBoxAnimationDirection.Size = new System.Drawing.Size(191, 20);
            this.comboBoxAnimationDirection.TabIndex = 9;
            // 
            // labelControlAnimationDirection
            // 
            this.labelControlAnimationDirection.AutoSize = true;
            this.labelControlAnimationDirection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControlAnimationDirection.Location = new System.Drawing.Point(13, 171);
            this.labelControlAnimationDirection.Name = "labelControlAnimationDirection";
            this.labelControlAnimationDirection.Size = new System.Drawing.Size(58, 13);
            this.labelControlAnimationDirection.TabIndex = 8;
            this.labelControlAnimationDirection.Text = "Direction";
            // 
            // buttonShowNotification
            // 
            this.buttonShowNotification.Location = new System.Drawing.Point(89, 242);
            this.buttonShowNotification.Name = "buttonShowNotification";
            this.buttonShowNotification.Size = new System.Drawing.Size(105, 21);
            this.buttonShowNotification.TabIndex = 10;
            this.buttonShowNotification.Text = "Show Notification";
            this.buttonShowNotification.UseVisualStyleBackColor = true;
            this.buttonShowNotification.Click += new System.EventHandler(this.buttonShowNotification_Click);
            // 
            // comboBoxSound
            // 
            this.comboBoxSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSound.FormattingEnabled = true;
            this.comboBoxSound.Location = new System.Drawing.Point(89, 195);
            this.comboBoxSound.Name = "comboBoxSound";
            this.comboBoxSound.Size = new System.Drawing.Size(191, 20);
            this.comboBoxSound.TabIndex = 12;
            this.comboBoxSound.SelectedIndexChanged += new System.EventHandler(this.comboBoxSound_SelectedIndexChanged);
            // 
            // labelControlSound
            // 
            this.labelControlSound.AutoSize = true;
            this.labelControlSound.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControlSound.Location = new System.Drawing.Point(13, 198);
            this.labelControlSound.Name = "labelControlSound";
            this.labelControlSound.Size = new System.Drawing.Size(43, 13);
            this.labelControlSound.TabIndex = 11;
            this.labelControlSound.Text = "Sound";
            // 
            // NotificationLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 274);
            this.Controls.Add(this.comboBoxSound);
            this.Controls.Add(this.labelControlSound);
            this.Controls.Add(this.buttonShowNotification);
            this.Controls.Add(this.comboBoxAnimationDirection);
            this.Controls.Add(this.labelControlAnimationDirection);
            this.Controls.Add(this.comboBoxAnimation);
            this.Controls.Add(this.labelControlAnimation);
            this.Controls.Add(this.comboBoxDuration);
            this.Controls.Add(this.labelControlDuration);
            this.Controls.Add(this.textBoxBody);
            this.Controls.Add(this.labelControlBody);
            this.Controls.Add(this.textBoxTitle);
            this.Controls.Add(this.labelControlTitle);
            this.Name = "NotificationLauncher";
            this.Text = "Toast Notification Launcher";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelControlTitle;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.Label labelControlBody;
        private System.Windows.Forms.TextBox textBoxBody;
        private System.Windows.Forms.Label labelControlDuration;
        private System.Windows.Forms.ComboBox comboBoxDuration;
        private System.Windows.Forms.Label labelControlAnimation;
        private System.Windows.Forms.ComboBox comboBoxAnimation;
        private System.Windows.Forms.ComboBox comboBoxAnimationDirection;
        private System.Windows.Forms.Label labelControlAnimationDirection;
        private System.Windows.Forms.Button buttonShowNotification;
        private System.Windows.Forms.ComboBox comboBoxSound;
        private System.Windows.Forms.Label labelControlSound;
    }
}