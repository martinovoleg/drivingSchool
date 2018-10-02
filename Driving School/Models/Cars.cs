using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driving_School.Models
{
    public class Cars
    {
        [Key]
        public int CarId { get; set; }
        public string CarType { get; set; }
        public string CarModel { get; set; }
        public int SchoolId { get; set; }

        [Column(TypeName = "Date")]
        public DateTime CarProductionYear { get; set; }
        [Column(TypeName = "Date")]
        public DateTime YearOfAdmissionToDrivingSchool { get; set; }
        

        public virtual ICollection<DrivingLessons> drivingLessons { get; set; }

    }
}
