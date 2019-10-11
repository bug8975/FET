using System;
using System.Collections.Generic;

namespace Monitor_HCCS.Model
{
    public class Info
    {
        public EventHandler InfoValueChanged;

        private List<Info> infos = new List<Info>();

        public List<Info> Infos {
            get { return infos; }
            set
            {
                if (infos.Count != value.Count)
                {
                    infos = value;
                    InfoValueChanged.Invoke(this, EventArgs.Empty);
                }
                    
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private int increment;

        public int Increment
        {
            get { return increment; }
            set { increment = value; }
        }
        private int distance;

        public int Distance
        {
            get { return distance; }
            set { distance = value; }
        }
        private string site;

        public string Site
        {
            get { return site; }
            set { site = value; }
        }
        private DateTime time;

        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }
        private int model;

        public int Model
        {
            get { return model; }
            set { model = value; }
        }

        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
