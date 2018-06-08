using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driving_School.Models
{
    public class Reviews
    {
        [Key]
        public int ReviewId { get; set; }
        public int DrivingSchoolId { get; set; }
        public string ReviewName { get; set; }

        [ForeignKey("DrivingSchoolId")]
        public virtual DrivingSchools drivingSchool { get; set; }
    }
}
