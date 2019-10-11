using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor_HCCS.Common.UIDataSource
{
    public class ComboxDSHelper
    {
        //深度档位选择下拉框数据源
        public static readonly Dictionary<string, string[]> DepthMap = new Dictionary<string, string[]>
        {
            {"F", new string[]{"wt300a", "g100p20", "g200p35"}},
            {"G", new string[]{"wt400a", "g200p35", "g300p45"}},
            {"H", new string[]{"wt500a", "g200p35", "g400p55"}},
            {"I", new string[]{"wt600a", "g200p35", "g400p55"}},
            {"J", new string[]{"wt800a", "g200p35", "g400p55"}},
            {"K", new string[]{"wt1000a", "g300p50", "g600p80"}},
            {"L", new string[]{"wt1200a", "g300p50", "g600p80"}},
            {"S", new string[]{"wt2000a", "g300p50", "g600p105"}},
            {"R", new string[]{"wt3000a", "g300p50", "g600p105"}}
        };

        //字母对应型号
        public static readonly Dictionary<string, string> FetList = new Dictionary<string, string>
        {
            {"F", "wt300a"},
            {"G", "wt400a"},
            {"H", "wt500a"},
            {"I", "wt600a"},
            {"J", "wt800a"},
            {"K", "wt1000a"},
            {"L", "wt1200a"},
            {"M", "ex30"},
            {"N", "ex50"},
            {"O", "ex60"},
            {"P", "ex80"},
            {"Q", "ex100"},
            {"R", "wt3000a"},
            {"S", "wt2000a"}
        };
    }
}
