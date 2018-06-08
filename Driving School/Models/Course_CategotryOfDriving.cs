using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driving_School.Models
{
    public class Course_CategotryOfDriving
    {
        [Key]
        public int Course_CategotryOfDrivingId { get; set; }
        public int? CourseId { get; set; }
        public int? CategoryOfDrivingId { get; set; }
        public int? CoWorkerId { get; set; }


        [ForeignKey("CourseId")]
        public virtual Courses course { get; set; }
        [ForeignKey("CategoryOfDrivingId")]
        public virtual CategoriesOfDriving categoryOfDriving { get; set; }
        [ForeignKey("CoWorkerId")]
        public virtual CoWorkers coWorker { get; set; }
    }
}
