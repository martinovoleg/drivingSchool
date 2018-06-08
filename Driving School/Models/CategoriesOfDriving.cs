using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driving_School.Models
{
   public class CategoriesOfDriving
    {
        [Key]
        public int CategoryOfDrivingId { get; set; }
        public string CategoryOfDrivingName { get; set; }

        public virtual ICollection<Course_CategotryOfDriving> course_CategotryOfDrivings { get; set; }
    }
}
