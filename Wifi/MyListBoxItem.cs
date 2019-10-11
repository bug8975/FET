using System;
using System.Drawing;

namespace WifiTool
{
    /// <summary>
    /// 自定义项
    /// </summary>
    class MyListBoxItem : IDisposable
    {
        private string _txt;
        private Image _img;
        /// <summary>
        /// 项文本
        /// </summary>
        public string Text
        {
            get { return _txt; }
            set { _txt = value; }
        }
        /// <summary>
        /// 项图标（可为null）
        /// </summary>
        public Image ItemImage
        {
            get { return _img; }
            set { _img = value; }
        }
        /// <summary>
        /// 是否显示tooltip
        /// </summary>
        public bool ShowTip { get; set; }
        /// <summary>
        /// 带图标构造函数
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="img"></param>
        public MyListBoxItem(string txt, Image img)
        {
            _txt = txt;
            _img = img;
        }
        /// <summary>
        /// 不带图标构造函数
        /// </summary>
        /// <param name="txt"></param>
        public MyListBoxItem(string txt)
        {
            _txt = txt;
        }
        /// <summary>
        /// 重写ToString获取项文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _txt;
        }

        public void Dispose()
        {
            _img = null;
        }
    }
}
