using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BenNHControl
{
    public partial class OverridFormEX : FormEX
    {
        public OverridFormEX()
        {
            InitializeComponent();
        }

        private void OverridFormEX_Load(object sender, EventArgs e)
        {

        }

        public new void Show()
        {
            Console.WriteLine("重写成功！");
            base.Show();
        }
    }
}
