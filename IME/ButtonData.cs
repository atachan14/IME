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
        public List<string>? Value0 { get; set; } 
        public List<string>? Value1 { get; set; }
        public List<string>? Value2 { get; set; }
        public List<string>? Value3 { get; set; }
        public List<string>? Value4 { get; set; }

        public Dictionary<string, Dictionary<string, string>>? ExeTags { get; set; }

    }
}
