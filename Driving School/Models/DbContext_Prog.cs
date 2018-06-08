using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driving_School.Models
{
    public class DbContext_Prog : DbContext
    {
        public DbContext_Prog(string connection)
        {
            Database.Connection.ConnectionString = connection;
        }

        public DbSet<Addresses> Addresses { get; set; }
        public DbSet<Cars> Cars { get; set; }
        public DbSet<CategoriesOfDriving> CategoriesOfDriving { get; set; }
        public DbSet<Course_CategotryOfDriving> Course_CategotryOfDriving { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<CoWorkers> CoWorkers { get; set; }
        public DbSet<DrivingLessons> DrivingLessons { get; set; }
        public DbSet<DrivingSchools> DrivingSchools { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Pupils> Pupils { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<TypeOfProperty> TypeOfProperty { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
