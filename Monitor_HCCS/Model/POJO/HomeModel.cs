using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor_HCCS.Model
{
    public class HomeModel
    {
        public string Name { get; set; }
        public string Increment { get; set; }
        public string Distance { get; set; }
        public string Site { get; set; }
        public DateTime Time { get; set; }
        public string Fet { get; set; }
        public string FileName { get; set; }
        public int ElectricQuantity { get; set; }
    }
}
