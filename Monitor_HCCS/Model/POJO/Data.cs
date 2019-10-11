using Monitor_HCCS.Common;
using System;
using System.Collections.Generic;

namespace Monitor_HCCS.Model
{
    public class Data
    {
        private int percentage = 0;
        private List<byte> sendByList = new List<byte>();
        private byte[] bledata;
        private bool isPaintPictureFlag = false;
        //private int dataCount = Convert.ToInt32(XmlHelper.getValue("dataCount"));

        public delegate void ValueChangedHandler(object sender, EventArgs e);

        //public event ValueChangedHandler ValueChangedInBledata;
        public event ValueChangedHandler ValueChangedInPercentage;
        //public event ValueChangedHandler ValueChangedInSendByList;

        public string Name { get; set; }
        public string LastId { get; set; }
        public int HZCount { get; set; }
        public bool IsPaintPictureFlag
        {
            get { return isPaintPictureFlag; }
            set { isPaintPictureFlag = value; }
        }
        private object gridDataSource;

        public object GridDataSource
        {
            get { return gridDataSource; }
            set { gridDataSource = value; }
        }
        public double[] ModbusData { get; set; }

        public int DataCount { get; set; }
        public byte[] Bledata
        {
            get { return bledata; }
            set { bledata = value; }
        }

        public int Percentage
        {
            get { return percentage; }
            set
            {
                if (value != percentage)
                {
                    percentage = value;
                    ValueChangedInPercentage.Invoke(this, null);
                }                                    
            }
        }
        public List<byte> SendByList
        {
            get { return sendByList; }
            set { sendByList = value; }
        }

        public double[][] DGVDataSource { get; set; }
        public double[][] ChartDataSource { get; set; }
    }
}
