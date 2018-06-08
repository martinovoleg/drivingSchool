using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driving_School.Models
{
    public class DrivingSchools
    {
        [Key]
        public int DrivingSchoolId { get; set; }
        public string DrivingSchoolName { get; set; }
        public string DrivingSchoolSite { get; set; }
        public int? TypeOfPropertyId { get; set; }
        public int? AddressId { get; set; }

        [ForeignKey("TypeOfPropertyId")]
        public virtual TypeOfProperty typeOfProperty { get; set; }
        [ForeignKey("AddressId")]
        public virtual Addresses address { get; set; }

        public virtual ICollection<Reviews> reviews { get; set; }
        public virtual ICollection<Courses> courses { get; set; }
    }
}