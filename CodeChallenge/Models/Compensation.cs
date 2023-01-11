using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    // Type definition of Compensation
    public class Compensation
    {
        // PrimaryKey to uniquely identify this record in the data table
        [Key]
        public String CompensationId { get; set; }
        // decimal supports up to 16 bytes of data (support larger salaries)
        public decimal Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
        public String EmployeeId { get; set; }
        // Use of a foreign Key here is important (unlike with ReportingStructure) 
        // ... as it will enforce referential integrity between entities (stored in the DB).
        // Also, querying with Linq will automatically navigate relationships based on this key (better performance).
        [ForeignKey("EmployeeId")]
        // Virtual keyword here allows for "lazy loading" of Employee entity to improve performance.
        public virtual Employee Employee { get; set; }
    }
}