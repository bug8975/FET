﻿namespace BenNHControl
{
    partial class ButtonEX
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelControl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelControl
            // 
            this.labelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl.Location = new System.Drawing.Point(0, 0);
            this.labelControl.Name = "labelControl";
            this.labelControl.Size = new System.Drawing.Size(20, 20);
            this.labelControl.TabIndex = 0;
            this.labelControl.Click += new System.EventHandler(this.labelControl_Click);
            this.labelControl.MouseLeave += new System.EventHandler(this.labelControl_MouseLeave);
            this.labelControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelControl_MouseMove);
            // 
            // ButtonEX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelControl);
            this.Name = "ButtonEX";
            this.Size = new System.Drawing.Size(20, 20);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelControl;
    }
}
