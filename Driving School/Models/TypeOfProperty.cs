using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driving_School.Models
{
   public class TypeOfProperty
    {
        [Key]
        public int TypeOfPropertyId { get; set; }
        public string TypeOfPropertyName { get; set; }

        public virtual ICollection<DrivingSchools> drivingSchools { get; set; }
    }
}
