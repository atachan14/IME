using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IME
{
    public class ButtonData
    {
        public string? Id { get; set; }
        public string[]? Value0 { get; set; }
        public string[]? Value1 { get; set; }
        public string[]? Value2 { get; set; }
        public string[]? Value3 { get; set; }
        public string[]? Value4 { get; set; }

        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> TagType { get; set; }
      = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

        public Dictionary<string, Dictionary<string, string>> ExeTags { get; set; }
        = new Dictionary<string, Dictionary<string, string>>
        {
            {
                "Short", new Dictionary<string, string>
                {
                    {"0","current0" },
                    {"1","current1" },
                    {"2","current2" },
                    {"3","current3" },
                    {"4","current4" }
                }
            },
            {
                "Long", new Dictionary<string, string>
                {
                    {"0","none" },
                    {"1","none" },
                    {"2","none" },
                    {"3","none" },
                    {"4","none" }
                }
            }
        };
        

       
       
    }
}
