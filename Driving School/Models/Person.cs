using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driving_School.Models
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string SecondName { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? DateOfBirth { get; set; }
        public string PathToPhoto { get; set; }
        public string SocialStatus { get; set; }
        public string PlaceOfStudy { get; set; }
        public string PlaceOfWork { get; set; }

        public virtual ICollection<Pupils> pupils { get; set; }
        public virtual ICollection<CoWorkers> coWorkers { get; set; }
    }
}
