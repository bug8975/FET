using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WifiTool
{
    class MyListBox : ListBox
    {
        /// <summary>
        /// 是否带图标
        /// </summary>
        private bool HasIcon { get; set; }
        /// <summary>
        /// 图标宽度（仅在HasIcon属性为true时有效）
        /// </summary>
        private int IconWidth { get; set; }
        /// <summary>
        /// 图标高度（仅在HasIcon属性为true时有效）
        /// </summary>
        private int IconHeight { get; set; }

        ToolTip tip = new ToolTip();

        public MyListBox()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
        }
        /// <summary>
        /// 设置图标大小（若不带图标就无需设置）
        /// </summary>
        /// <param name="w">图标宽度</param>
        /// <param name="h">图标高度</param>
        public void SetIconSize(int w, int h)
        {
            this.HasIcon = true;
            this.IconWidth = w;
            this.IconHeight = h;
            this.ItemHeight = h;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();

            Graphics g = e.Graphics;
            StringFormat sf = new StringFormat();
            sf.Trimming = StringTrimming.EllipsisCharacter; //超出指定矩形区域部分用"..."替代
            sf.LineAlignment = StringAlignment.Center;//垂直居中
            try
            {
                MyListBoxItem item = (MyListBoxItem)Items[e.Index];

                SizeF size = g.MeasureString(item.Text, e.Font); //获取项文本尺寸
                if (HasIcon) //带图标时
                {
                    if (size.Width > e.Bounds.Width - this.IconWidth) //项文本宽度超过 项宽-图标宽度
                    {
                        item.ShowTip = true; //显示tooltip
                    }
                    /* 获取指定矩形区域，注意不能直接用项所在矩形，否则DrawString时会出现自动换行
                     * 的情况。前面说超出指定矩形区域用“...”替代 指的是DrawString方法会先塞满整个
                     * 矩形区域，如果区域高度够时，就会出现自动换行的情况 ******/
                    RectangleF rectF = new RectangleF(e.Bounds.Left,
                        e.Index * this.ItemHeight + (this.ItemHeight - size.Height) / 2.0f,
                        e.Bounds.Width - this.IconWidth, size.Height);
                    //写 项文本
                    g.DrawString(item.Text, e.Font, new SolidBrush(e.ForeColor), rectF, sf);
                    if (item.ItemImage != null) //在项右侧 画图标
                    {
                        /* 注意不能用DrawImage(img, x, y)方法，务必指定图标的大小，
                         * 否则会导致图标被放大，读者不妨一试 :)  *****/
                        g.DrawImage(item.ItemImage, e.Bounds.Right - this.IconWidth, e.Bounds.Top, this.IconWidth, this.IconHeight);
                    }
                }
                else //不带图标
                {
                    if (size.Width > e.Bounds.Width) //项文本宽度超过 项宽
                    {
                        item.ShowTip = true; //显示tooltip
                    }
                    //获取指定矩形区域
                    RectangleF rectF = new RectangleF(e.Bounds.Left,
                        e.Index * this.ItemHeight + (this.ItemHeight - size.Height) / 2.0f,
                        e.Bounds.Width, size.Height);
                    //写 项文本
                    g.DrawString(item.Text, e.Font, new SolidBrush(e.ForeColor), rectF, sf);
                }
            }
            catch { } //忽略异常

            base.OnDrawItem(e);
        }
        /// <summary>
        /// 重写鼠标移动事件
        /// </summary>
        /// <param name="e"></param>
        //protected override void OnMouseMove(MouseEventArgs e)
        //{
        //    base.OnMouseMove(e);
        //    int idx = IndexFromPoint(e.Location); //获取鼠标所在的项索引
        //    if (idx == MyListBox.NoMatches) //鼠标所在位置没有 项
        //    {
        //        tip.SetToolTip(this, ""); //设置提示信息为空
        //    }
        //    else
        //    {
        //        MyListBoxItem item = (MyListBoxItem)this.Items[idx];
        //        if (item.ShowTip)
        //        {
        //            string txt = this.Items[idx].ToString(); //获取项文本
        //            tip.SetToolTip(this, txt); //设置提示信息
        //        }
        //        else
        //        {
        //            tip.SetToolTip(this, ""); //设置提示信息为空
        //        }
        //    }
        //}
    }
}
