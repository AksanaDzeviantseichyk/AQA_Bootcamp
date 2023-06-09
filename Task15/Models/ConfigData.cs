using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task15.Enum;

namespace Task15.Models
{
    public class ConfigData
    {
        public string BaseUrl { get; set; }
        public Browsers BrowserName { get; set; }
        public IEnumerable<string> Options { get; set; }
        public int SmallWait { get; set; }
        public int MediumWait { get; set; }
        public int LargeWait { get; set; }

    }
}
