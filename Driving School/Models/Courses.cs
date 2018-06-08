using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driving_School.Models
{
    public class Courses
    {
        [Key]
        public int CourseId { get; set; }
        public int DrivingSchoolId { get; set; }
        public string CourseName { get; set; }
        [Column(TypeName = "Date")]
        public DateTime DateOfBeginningCourse { get; set; }
        public double CourseDuration { get; set; }
        public double TrainingPeriod { get; set; }
        public double CostOfEducation { get; set; }
        public double CostOfGasolineAndFuel { get; set; }

        [ForeignKey("DrivingSchoolId")]
        public virtual DrivingSchools drivingSchool { get; set; }

        public virtual ICollection<Course_CategotryOfDriving> course_CategotryOfDrivings { get; set; }
    }
}
