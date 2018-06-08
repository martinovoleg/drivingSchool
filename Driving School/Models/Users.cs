using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driving_School.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string UserLogin { get; set; }
        public string UserPass { get; set; }
        //public bool IsAdmin { get; set; }
    }
}
