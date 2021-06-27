using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiOnThisDay.Classes
{
    public class Event
    {
        public string year { get; set; }
        public string description { get; set; }
        public List<Wikipedia> wikipedia { get; set; }
    }
}
