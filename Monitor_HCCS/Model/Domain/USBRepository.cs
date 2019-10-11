using Monitor_HCCS.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor_HCCS.Model.Domain
{
    public class USBRepository : IUSBRepository
    {

        public string FindFileByName(string name)
        {
            try
            {
                string sql1 = "select filename from info where name = '" + name + "'";
                DataTable dt = SQLiteHelper.ExecuteDataTable(sql1);
                string filename = dt.Rows[0].ItemArray[0].ToString();
                return filename;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return "";
            }
        }

    }
}
