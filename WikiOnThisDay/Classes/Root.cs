using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiOnThisDay.Classes
{
    public class Root
    {
        public string wikipedia { get; set; }
        public string date { get; set; }
        public List<Event> events { get; set; }
    }
}
