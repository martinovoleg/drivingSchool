using Driving_School.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Driving_School
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        private DbContext_Prog db = new DbContext_Prog(ConfigurationManager.ConnectionStrings["PCString"].ConnectionString);

        public ReportWindow()
        {
            InitializeComponent();

            var courseList = db.Courses.OrderBy(el => el.CostOfEducation);

            foreach (var course in courseList)
            {
                CoursesDataGrid.Items.Add(new Course(course));
            }

            try
            {
                SchoolsCount.Content = db.DrivingSchools.Count().ToString();
                CoursesCount.Content = db.Courses.Count().ToString();
                PupilsCount.Content = db.Pupils.Count().ToString();

                var pupils = db.Pupils.ToList();
                List<int> ages = new List<int>();
                foreach (var p in pupils)
                {
                    var person = db.Person.Where(el => el.PersonId == p.PersonId).FirstOrDefault();
                    if (person != null)
                    {
                        try
                        {
                            ages.Add(DateTime.Today.Year - person.DateOfBirth.Value.Year);
                        }
                        catch (Exception ex)
                        {
                            // skip
                        }
                    }
                }

                MostPopularAge.Content = Convert.ToInt32(ages.Average());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                var workers = db.DrivingLessons.GroupBy(p => p.CoWorkerId)
                    .Select(g => new { Id = g.Key, Count = g.Count() });

                var worker = db.CoWorkers.Where(work => work.CoWorkerId == workers.Where(el => el.Count == workers.Max(w => w.Count)).FirstOrDefault().Id).FirstOrDefault();
                var pers = db.Person.Where(p => p.PersonId == worker.PersonId).FirstOrDefault();
                MostPopularWorker.Content = pers.SecondName + " " + pers.FirstName + " " + pers.Surname;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DiagramWindow dw = new DiagramWindow();
            dw.ShowDialog();
        }
    }

    internal class Course
    {
        private DbContext_Prog db = new DbContext_Prog(ConfigurationManager.ConnectionStrings["PCString"].ConnectionString);

        public string Name { get; set; }
        public string Price { get; set; }
        public string Duration { get; set; }
        public string BeginDate { get; set; }
        public string School { get; set; }

        public Course(Courses course)
        {
            Name = course.CourseName;
            Price = course.CostOfEducation.ToString();
            Duration = course.CourseDuration.ToString();
            BeginDate = course.DateOfBeginningCourse.ToString();
            School = (db.DrivingSchools.Where(el => el.DrivingSchoolId == course.DrivingSchoolId).FirstOrDefault()).DrivingSchoolName;
        }
    }
}