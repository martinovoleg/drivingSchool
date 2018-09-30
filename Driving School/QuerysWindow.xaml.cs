using Driving_School.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Data.Linq.SqlClient;

namespace Driving_School
{
    public partial class QuerysWindow : Window
    {
        private DbContext_Prog db = new DbContext_Prog(ConfigurationManager.ConnectionStrings["PCString"].ConnectionString);

        public QuerysWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var drivingSchools = db.DrivingSchools.ToList();
            var courses = db.Courses.ToList();

            var result = (from course in courses

                          join school in drivingSchools on course.DrivingSchoolId equals school.DrivingSchoolId
                          join type in db.TypeOfProperty on school.TypeOfPropertyId equals type.TypeOfPropertyId
                          join adr in db.Addresses on school.AddressId equals adr.AddressId

                          select new { school.DrivingSchoolName, course.CourseName, type.TypeOfPropertyName, adr.City }).ToList();

            CoursesDataGrid.ItemsSource = result;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var drivingSchools = db.DrivingSchools.ToList();
            var courses = db.Courses.ToList();

            var result = (from course in courses

                          join school in drivingSchools on course.DrivingSchoolId equals school.DrivingSchoolId
                          join type in db.TypeOfProperty on school.TypeOfPropertyId equals type.TypeOfPropertyId
                          join adr in db.Addresses on school.AddressId equals adr.AddressId
                          from category in db.CategoriesOfDriving
                          where category.CategoryOfDrivingName == "B"

                          select new { school.DrivingSchoolName, course.CourseName, type.TypeOfPropertyName, adr.City, category.CategoryOfDrivingName }).ToList();

            CoursesDataGrid.ItemsSource = result;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var drivingSchools = db.DrivingSchools.ToList();
            var courses = db.Courses.ToList();

            var result = (from course in courses

                          join school in drivingSchools on course.DrivingSchoolId equals school.DrivingSchoolId
                          join type in db.TypeOfProperty on school.TypeOfPropertyId equals type.TypeOfPropertyId
                          join adr in db.Addresses on school.AddressId equals adr.AddressId
                          from category in db.Course_CategotryOfDriving
                          join cat in db.CategoriesOfDriving on category.CategoryOfDrivingId equals cat.CategoryOfDrivingId
                          join cow in db.CoWorkers on category.CoWorkerId equals cow.CoWorkerId
                          join pers in db.Person on cow.PersonId equals pers.PersonId
                          where course.DateOfBeginningCourse == Convert.ToDateTime("01.01.2018")

                          select new
                          {
                              school.DrivingSchoolName,
                              course.CourseName,
                              type.TypeOfPropertyName,
                              adr.City,
                              cat.CategoryOfDrivingName,
                              pers.SecondName,
                              course.DateOfBeginningCourse.Date
                          }).ToList();

            CoursesDataGrid.ItemsSource = result;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var drivingSchools = db.DrivingSchools.ToList();
            var courses = db.Courses.ToList();

            var result = (from course in courses

                          join school in drivingSchools on course.DrivingSchoolId equals school.DrivingSchoolId
                          join type in db.TypeOfProperty on school.TypeOfPropertyId equals type.TypeOfPropertyId
                          join adr in db.Addresses on school.AddressId equals adr.AddressId
                          from category in db.Course_CategotryOfDriving
                          join cat in db.CategoriesOfDriving on category.CategoryOfDrivingId equals cat.CategoryOfDrivingId
                          join cow in db.CoWorkers on category.CoWorkerId equals cow.CoWorkerId
                          join pers in db.Person on cow.PersonId equals pers.PersonId
                          where course.DateOfBeginningCourse >= Convert.ToDateTime("01.01.2017") && course.DateOfBeginningCourse <= Convert.ToDateTime("01.01.2018")

                          select new
                          {
                              school.DrivingSchoolName,
                              course.CourseName,
                              type.TypeOfPropertyName,
                              adr.City,
                              cat.CategoryOfDrivingName,
                              pers.SecondName,
                              course.DateOfBeginningCourse.Date
                          }).ToList();

            CoursesDataGrid.ItemsSource = result;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var drivingSchools = db.DrivingSchools.ToList();
            var courses = db.Courses.ToList();

            var result = (from course in courses

                          join school in drivingSchools on course.DrivingSchoolId equals school.DrivingSchoolId
                          join type in db.TypeOfProperty on school.TypeOfPropertyId equals type.TypeOfPropertyId
                          join adr in db.Addresses on school.AddressId equals adr.AddressId

                          join pup in db.Pupils on school.DrivingSchoolId equals pup.SchoolId
                          join pers in db.Person on pup.PersonId equals pers.PersonId
                          //where SqlMethods.Like(pers.Surname, "%" + pers.FirstName + "%," + pers.SecondName)
                          where pers.FirstName.Contains("Олег") // аналог строки выше

                          select new
                          {
                              school.DrivingSchoolName,
                              course.CourseName,
                              type.TypeOfPropertyName,
                              adr.City,
                              pers.Surname,
                              pers.SecondName,
                              pers.FirstName,
                          }).ToList();

            CoursesDataGrid.ItemsSource = result;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var drivingSchools = db.DrivingSchools.ToList();
            var courses = db.Courses.ToList();
            var adrCities = db.Addresses.Select(el => el.City);

            var result = (from course in courses

                          join school in drivingSchools on course.DrivingSchoolId equals school.DrivingSchoolId
                          join type in db.TypeOfProperty on school.TypeOfPropertyId equals type.TypeOfPropertyId
                          join adr in db.Addresses on school.AddressId equals adr.AddressId

                          join pup in db.Pupils on school.DrivingSchoolId equals pup.SchoolId
                          join pers in db.Person on pup.PersonId equals pers.PersonId

                          where adrCities.Contains(adr.City)

                          select new
                          {
                              school.DrivingSchoolName,
                              course.CourseName,
                              type.TypeOfPropertyName,
                              adr.City,
                              pers.Surname,
                              pers.SecondName,
                              pers.FirstName,
                              course.DateOfBeginningCourse.Date
                          }).ToList();

            CoursesDataGrid.ItemsSource = result;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            var drivingSchools = db.DrivingSchools.ToList();
            var courses = db.Courses.ToList();

            var result = (from course in courses

                          join school in drivingSchools on course.DrivingSchoolId equals school.DrivingSchoolId
                          join type in db.TypeOfProperty on school.TypeOfPropertyId equals type.TypeOfPropertyId
                          join adr in db.Addresses on school.AddressId equals adr.AddressId
                          from category in db.Course_CategotryOfDriving // левое объединение
                          join cat in db.CategoriesOfDriving on category.CategoryOfDrivingId equals cat.CategoryOfDrivingId
                          join cow in db.CoWorkers on category.CoWorkerId equals cow.CoWorkerId
                          join pers in db.Person on cow.PersonId equals pers.PersonId
                          join les in db.DrivingLessons on cow.CoWorkerId equals les.CoWorkerId
                          join car in db.Cars on les.CarId equals car.CarId

                          select new
                          {
                              school.DrivingSchoolName,
                              course.CourseName,
                              type.TypeOfPropertyName,
                              adr.City,
                              les?.SessionDate,
                              car?.CarModel,
                              course.DateOfBeginningCourse.Date
                          }).ToList();

            CoursesDataGrid.ItemsSource = result;
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            var result = (from dr in db.DrivingSchools

                          join pup in db.Pupils on dr.DrivingSchoolId equals pup.SchoolId
                          join pers in db.Person on pup.PersonId equals pers.PersonId

                          select new
                          {
                              dr.DrivingSchoolName,
                              pers.FirstName,
                              pers.SecondName,
                              pers.Surname
                          }).ToList();

            var res = result.Distinct();

            CoursesDataGrid.ItemsSource = res;
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            var result = (from dr in db.DrivingSchools

                          join cur in db.Courses on dr.DrivingSchoolId equals cur.DrivingSchoolId

                          select new
                          {
                              dr.DrivingSchoolName,
                              cur.CourseName,
                              countCurse = db.Courses.Where(el => el.DrivingSchoolId == dr.DrivingSchoolId).Count()
                          }).ToList();

            var res = result.Distinct();

            CoursesDataGrid.ItemsSource = res;
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            var prom = db.DrivingSchools.Select(el => new
            {
                el.DrivingSchoolName,
                CourseDuration = db.Courses.Where(l => l.DrivingSchoolId == el.DrivingSchoolId).Sum(s => s.CourseDuration)
            }).ToList();

            var res = prom.Distinct();

            CoursesDataGrid.ItemsSource = res;
        }
    }
}