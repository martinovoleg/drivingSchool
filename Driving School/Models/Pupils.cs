using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driving_School.Models
{
    public class Pupils
    {
        [Key]
        public int PupilId { get; set; }
        public int PersonId { get; set; }
        public bool IsCashlessPayments { get; set; }
        public int SchoolId { get; set; }

        [ForeignKey("PersonId")]
        public virtual Person person { get; set; }

    }
}
