using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    // Our type definition for a ReportingStructure
    public class ReportingStructure
    {
        // No Id key as this object is created dynamically.
        // Class Composition (instead of inheritance) - HAS-A employee instead of IS-A employee.
        public Employee Employee { get; set; }
        public int NumberOfReports { get; set; }
    }
}