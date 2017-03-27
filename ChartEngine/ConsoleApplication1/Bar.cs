using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CE
{
    public class Bar
    {
        public string uri { get; }
        public string value { get; }
        public double height { get; set; }

        public Bar(string uri, string value)
        {
            this.uri = uri;
            this.value = value;
        }
    }
}
