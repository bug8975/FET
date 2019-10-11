namespace BenNHControl
{
    partial class RulerControl
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
            this.P1 = new System.Windows.Forms.Panel();
            this.P2 = new System.Windows.Forms.Panel();
            this.PContainer = new System.Windows.Forms.Panel();
            this.Pic = new System.Windows.Forms.PictureBox();
            this.PContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic)).BeginInit();
            this.SuspendLayout();
            // 
            // P1
            // 
            this.P1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.P1.Location = new System.Drawing.Point(50, 0);
            this.P1.Margin = new System.Windows.Forms.Padding(0);
            this.P1.Name = "P1";
            this.P1.Size = new System.Drawing.Size(535, 52);
            this.P1.TabIndex = 9;
            // 
            // P2
            // 
            this.P2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.P2.Location = new System.Drawing.Point(0, 50);
            this.P2.Margin = new System.Windows.Forms.Padding(0);
            this.P2.Name = "P2";
            this.P2.Size = new System.Drawing.Size(50, 368);
            this.P2.TabIndex = 8;
            // 
            // PContainer
            // 
            this.PContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PContainer.AutoScroll = true;
            this.PContainer.Controls.Add(this.Pic);
            this.PContainer.Location = new System.Drawing.Point(50, 50);
            this.PContainer.Margin = new System.Windows.Forms.Padding(0);
            this.PContainer.Name = "PContainer";
            this.PContainer.Size = new System.Drawing.Size(535, 368);
            this.PContainer.TabIndex = 10;
            this.PContainer.Scroll += new System.Windows.Forms.ScrollEventHandler(this.PContainer_Scroll);
            // 
            // Pic
            // 
            this.Pic.Location = new System.Drawing.Point(0, 0);
            this.Pic.Margin = new System.Windows.Forms.Padding(0);
            this.Pic.Name = "Pic";
            this.Pic.Size = new System.Drawing.Size(523, 362);
            this.Pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Pic.TabIndex = 8;
            this.Pic.TabStop = false;
            this.Pic.DoubleClick += new System.EventHandler(this.Pic_DoubleClick);
            this.Pic.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Pic_MouseDown);
            this.Pic.MouseEnter += new System.EventHandler(this.Pic_MouseEnter);
            this.Pic.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Pic_MouseMove);
            this.Pic.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Pic_MouseUp);
            // 
            // RulerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PContainer);
            this.Controls.Add(this.P1);
            this.Controls.Add(this.P2);
            this.Name = "RulerControl";
            this.Size = new System.Drawing.Size(585, 425);
            this.Load += new System.EventHandler(this.RulerControl_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.RulerControl_Paint);
            this.Resize += new System.EventHandler(this.RulerControl_Resize);
            this.PContainer.ResumeLayout(false);
            this.PContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel P1;
        private System.Windows.Forms.Panel P2;
        private System.Windows.Forms.Panel PContainer;
        private System.Windows.Forms.PictureBox Pic;



    }
}
