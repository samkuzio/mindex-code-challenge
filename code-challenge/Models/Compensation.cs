using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class Compensation
    {
        public string Employee { get; set; }
        public double Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
