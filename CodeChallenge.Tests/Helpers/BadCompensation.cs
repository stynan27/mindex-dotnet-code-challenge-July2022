using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Tests.Integration.Helpers
{
    public class BadCompensation
    {
        // String instead of decimal...
        public String Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}