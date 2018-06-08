using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driving_School.Models
{
   public class Addresses
    {
        [Key]
        public int AddressId { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Home { get; set; }

        public virtual ICollection<DrivingSchools> drivingSchools { get; set; }
    }
}
