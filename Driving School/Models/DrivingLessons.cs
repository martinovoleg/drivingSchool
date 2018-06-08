using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driving_School.Models
{
   public class DrivingLessons
    {
        [Key]
        public int DrivingLessonsId { get; set; }
        public int CoWorkerId { get; set; }
        public int CarId { get; set; }
        [Column(TypeName = "Date")]
        public DateTime SessionDate { get; set; }
        public int SchoolId { get; set; }

        [ForeignKey("CoWorkerId")]
        public virtual CoWorkers coWorker { get; set; }
        [ForeignKey("CarId")]
        public virtual Cars car { get; set; }
    }
}
