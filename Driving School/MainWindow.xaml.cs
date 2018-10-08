using Driving_School.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Driving_School
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DbContext_Prog db = new DbContext_Prog(ConfigurationManager.ConnectionStrings["PCString"].ConnectionString);
        private int DrivingSchoolId = 0;

        private void SetDisplayDates()
        {
            StartDate.DisplayDate = DateTime.Now.Date;
            StartDate.DisplayDateStart = new DateTime(1990, 1, 1);

            CarProductionYear.DisplayDate = DateTime.Now.Date;
            CarProductionYear.DisplayDateStart = new DateTime(1990, 1, 1);
            CarProductionYear.DisplayDateEnd = DateTime.Now.Date;

            LessonDate.DisplayDate = DateTime.Now.Date;
            LessonDate.DisplayDateStart = new DateTime(1990, 1, 1);

            CarAdmissionYear.DisplayDate = DateTime.Now.Date;
            CarAdmissionYear.DisplayDateStart = new DateTime(1990, 1, 1);
            CarAdmissionYear.DisplayDateEnd = DateTime.Now.Date;

            PupilBirthDate.DisplayDate = new DateTime(DateTime.Now.Year - 16, 1, 1); 
            PupilBirthDate.DisplayDateEnd = new DateTime(DateTime.Now.Year - 16, 12, 31);

            WorkerBirthDate.DisplayDate = new DateTime(DateTime.Now.Year - 16, 1, 1);
            WorkerBirthDate.DisplayDateEnd = new DateTime(DateTime.Now.Year - 16, 12, 31);
        }

        private void SetBlackOutDates()
        {
            StartDate.BlackoutDates.Add(new CalendarDateRange(new DateTime(0001, 1, 1), new DateTime(1989, 12, 31)));

            LessonDate.BlackoutDates.Add(new CalendarDateRange(new DateTime(0001, 1, 1), new DateTime(1989, 12, 31)));

            CarProductionYear.BlackoutDates.Add(new CalendarDateRange(new DateTime(0001, 1, 1), new DateTime(1989, 12, 31)));
            CarAdmissionYear.BlackoutDates.Add(new CalendarDateRange(new DateTime(0001, 1, 1), new DateTime(1989, 12, 31)));

            CarProductionYear.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.Date, DateTime.MaxValue));
            CarAdmissionYear.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.Date, DateTime.MaxValue));

            PupilBirthDate.BlackoutDates.Add(new CalendarDateRange(new DateTime(0001, 1, 1), new DateTime(DateTime.Now.Year - 120, 12, 31)));
            PupilBirthDate.BlackoutDates.Add(new CalendarDateRange(new DateTime(DateTime.Now.Year - 16, 12, 31), DateTime.Now.Date));

            WorkerBirthDate.BlackoutDates.Add(new CalendarDateRange(new DateTime(0001, 1, 1), new DateTime(DateTime.Now.Year - 120, 12, 31)));
            WorkerBirthDate.BlackoutDates.Add(new CalendarDateRange(new DateTime(DateTime.Now.Year - 16, 12, 31), DateTime.Now.Date));
        }

        public MainWindow()
        {
            InitializeComponent();
            SetDisplayDates();
            SetBlackOutDates();

            // получаем все автошколы

            var drivingShoolsList = db.DrivingSchools.ToList();
            var typeOfPropertyList = db.TypeOfProperty.Select(el => el.TypeOfPropertyName).ToList();
            var categoriesOfDriving = db.CategoriesOfDriving.ToList();
            var pupilsList = db.Pupils.ToList();

            DrivingSchoolsListBox.ItemsSource = drivingShoolsList;
            DrivingSchoolsListBox.DisplayMemberPath = "DrivingSchoolName";

            // инициализация выпадающего списка с типами собственности
            TypeOfProperty.ItemsSource = typeOfPropertyList;
            // инициализация выпадающего списка с категориями вождения
            CategoryOfDriving.ItemsSource = categoriesOfDriving;
            CategoryOfDriving.DisplayMemberPath = "CategoryOfDrivingName";
        }

        private void DrivingSchoolsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            saveMessage.Content = null;

            var typeOfPropertyList = db.TypeOfProperty.ToList();
            Addresses DrivingSchoolAdress = null;
            var selectedDrivingSchool = (DrivingSchools)DrivingSchoolsListBox.SelectedItem;

            if (selectedDrivingSchool != null)
            {
                DrivingSchoolAdress = db.Addresses.FirstOrDefault(el =>
            el.AddressId == selectedDrivingSchool.AddressId);
            }

            ListDrivingSchoolsGrid.DataContext = DrivingSchoolsListBox.SelectedItem;

            if (selectedDrivingSchool != null)
            {
                TypeOfProperty.Text = typeOfPropertyList.FirstOrDefault(el =>
            el.TypeOfPropertyId == selectedDrivingSchool.TypeOfPropertyId)?.TypeOfPropertyName;
            }

            District.Text = DrivingSchoolAdress?.District;
            City.Text = DrivingSchoolAdress?.City;
            Street.Text = DrivingSchoolAdress?.Street;
            Home.Text = DrivingSchoolAdress?.Home;

            //------------------------------------------------------------
            // получаем все курсы
            if (selectedDrivingSchool != null)
            {
                DrivingSchoolId = selectedDrivingSchool.DrivingSchoolId;
                var courseList = db.Courses.Where(el => el.DrivingSchoolId == selectedDrivingSchool.DrivingSchoolId).ToList();
                var pupilsList = db.Pupils.Where(el => el.SchoolId == selectedDrivingSchool.DrivingSchoolId).Select(el => new
                {
                    el.PupilId,
                    el.IsCashlessPayments, 
                    el.SchoolId, 
                    el.PersonId,
                    FullName = el.person.SecondName + " " + el.person.FirstName + " " + el.person.Surname

                }).ToList();
                var workersList = db.CoWorkers.Where(el => el.SchoolId == selectedDrivingSchool.DrivingSchoolId).Select(el => new
                {
                    el.CoWorkerId,
                    el.SchoolId,
                    el.PersonId,
                    FullName = el.person.SecondName + " " + el.person.FirstName + " " + el.person.Surname

                }).ToList();
                var carsList = db.Cars.Where(el => el.SchoolId == selectedDrivingSchool.DrivingSchoolId).ToList();
                var lessonsList = db.DrivingLessons.Where(el => el.SchoolId == selectedDrivingSchool.DrivingSchoolId).ToList();

                CoursesListBox.ItemsSource = courseList;
                CoursesListBox.DisplayMemberPath = "CourseName";
                PupilsListBox.ItemsSource = pupilsList;
                PupilsListBox.DisplayMemberPath = "FullName";
                WorkersListBox.ItemsSource = workersList;
                WorkersListBox.DisplayMemberPath = "FullName";
                CarsListBox.ItemsSource = carsList;
                CarsListBox.DisplayMemberPath = "CarModel";
                LessonsListBox.ItemsSource = lessonsList;
                LessonsListBox.DisplayMemberPath = "SessionDate";
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var drivingShoolsList = db.DrivingSchools.ToList();

            // создаем новую автошколу
            var newDrivingSchool = new DrivingSchools
            {
                DrivingSchoolName = "Без названия"
            };

            db.DrivingSchools.Add(newDrivingSchool);
            db.SaveChanges();

            // обновляем данные в списке
            DrivingSchoolsListBox.ItemsSource = db.DrivingSchools.ToList();
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            var drivingShoolsList = db.DrivingSchools.ToList();
            var selectedDrivingSchool = (DrivingSchools)DrivingSchoolsListBox.SelectedItem;

            if (selectedDrivingSchool != null)
            {
                var drSchool = db.DrivingSchools.First(el => el.DrivingSchoolId == selectedDrivingSchool.DrivingSchoolId);

                db.DrivingSchools.Remove(drSchool);
                db.SaveChanges();
            }

            // обновляем данные в списке
            DrivingSchoolsListBox.ItemsSource = db.DrivingSchools.ToList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var drivingShoolsList = db.DrivingSchools.ToList();
            var selectedDrivingSchool = (DrivingSchools)DrivingSchoolsListBox.SelectedItem;

            try
            {
                if (selectedDrivingSchool != null)
                {
                    var drSchool = db.DrivingSchools.First(el => el.DrivingSchoolId == selectedDrivingSchool.DrivingSchoolId);

                    drSchool.DrivingSchoolName = DrivingSchoolNameTB.Text;
                    drSchool.DrivingSchoolSite = DrivingSchoolSiteTB.Text;

                    if (TypeOfProperty.SelectedItem != null)
                    {
                        drSchool.TypeOfPropertyId = db.TypeOfProperty.
                        FirstOrDefault(el => el.TypeOfPropertyName == TypeOfProperty.SelectedItem.ToString())?.TypeOfPropertyId;
                    }

                    if (drSchool.address != null)
                    {
                        drSchool.address.City = City.Text;
                        drSchool.address.District = District.Text;
                        drSchool.address.Street = Street.Text;
                        drSchool.address.Home = Home.Text;
                    }
                    else
                    {
                        var DrivingSchoolAdress = new Addresses
                        {
                            City = City.Text,
                            District = District.Text,
                            Street = Street.Text,
                            Home = Home.Text
                        };

                        db.Addresses.Add(DrivingSchoolAdress);
                        drSchool.AddressId = DrivingSchoolAdress.AddressId;

                        db.SaveChanges();
                    }

                    db.SaveChanges();
                }

                saveMessage.Content = "Сохранено!";
            }
            catch (Exception ex)
            {
                saveMessage.Content = ex.Message;
            }
        }

        private void CourseSaveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCourse = (Courses)CoursesListBox.SelectedItem;

            try
            {
                if (selectedCourse != null)
                {
                    var selCourse = db.Courses.Where(el => el.CourseId == selectedCourse.CourseId).FirstOrDefault();

                    selCourse.CourseName = CourseName.Text;
                    selCourse.CourseDuration = Convert.ToDouble(CourseDuration.Text);
                    selCourse.DateOfBeginningCourse = (System.DateTime)StartDate.SelectedDate;
                    selCourse.TrainingPeriod = Convert.ToDouble(DrivingDuration.Text);
                    selCourse.CostOfEducation = Convert.ToDouble(CourseCost.Text);
                    selCourse.CostOfGasolineAndFuel = Convert.ToDouble(GSMCost.Text);

                    db.SaveChanges();

                    var selectedCategoryOfDriving = (CategoriesOfDriving)CategoryOfDriving.SelectedItem;

                    if (selectedCategoryOfDriving != null)
                    {
                        foreach (var course_category in db.Course_CategotryOfDriving.ToList())
                        {
                            if (course_category.CourseId == selCourse.CourseId)
                            {
                                db.Course_CategotryOfDriving.Remove(course_category);
                                db.SaveChanges();
                            }
                        }

                        var newCourse_Category = new Course_CategotryOfDriving
                        {
                            CategoryOfDrivingId = selectedCategoryOfDriving.CategoryOfDrivingId,
                            CourseId = selCourse.CourseId
                        };
                        db.Course_CategotryOfDriving.Add(newCourse_Category);
                        db.SaveChanges();
                    }
                }

                saveMessage2.Content = "Сохранено!";
                var courseList = db.Courses.Where(el => el.DrivingSchoolId == ((DrivingSchools)DrivingSchoolsListBox.SelectedItem).DrivingSchoolId).ToList();
                CoursesListBox.ItemsSource = courseList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CourseAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedDrivingSchool = (DrivingSchools)DrivingSchoolsListBox.SelectedItem;

                if (DrivingSchoolsListBox.SelectedItem != null)
                {
                    var newCourse = new Courses
                    {
                        DrivingSchoolId = ((DrivingSchools)DrivingSchoolsListBox.SelectedItem).DrivingSchoolId,
                        CourseName = "Новый курс"
                    };

                    db.Courses.Add(newCourse);
                    db.SaveChanges();

                    var courseList = db.Courses.Where(el => el.DrivingSchoolId == selectedDrivingSchool.DrivingSchoolId).ToList();
                    CoursesListBox.ItemsSource = courseList;
                    CoursesListBox.SelectedItem = CoursesListBox.Items[CoursesListBox.Items.Count - 1];
                }
                else
                {
                    MessageBox.Show("Сначала выберите автошколу");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CourseDelButton_Click(object sender, RoutedEventArgs e)
        {
            if (DrivingSchoolsListBox.SelectedItem != null)
            {
                var selectedCourse = (Courses)CoursesListBox.SelectedItem;
                db.Courses.Remove(db.Courses.Where(el => el.CourseId == selectedCourse.CourseId).FirstOrDefault());
                db.SaveChanges();
                saveMessage2.Content = "Успешно удалено!";
                var courseList = db.Courses.Where(el => el.DrivingSchoolId == ((DrivingSchools)DrivingSchoolsListBox.SelectedItem).DrivingSchoolId).ToList();
                CoursesListBox.ItemsSource = courseList;
            }
            else
            {
                MessageBox.Show("Сначала выберите автошколу");
            }
        }

        private void CoursesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CoursesListBox.SelectedItem != null)
            {
                var selectedCourse = (Courses)CoursesListBox.SelectedItem;
                var categoryOfDrivingList = db.CategoriesOfDriving.ToList();

                CourseName.Text = selectedCourse.CourseName;
                CourseDuration.Text = selectedCourse.CourseDuration.ToString();

                if (selectedCourse.DateOfBeginningCourse != null)
                {
                    StartDate.SelectedDate = selectedCourse.DateOfBeginningCourse;
                }

                DrivingDuration.Text = selectedCourse.TrainingPeriod.ToString();
                CourseCost.Text = selectedCourse.CostOfEducation.ToString();
                GSMCost.Text = selectedCourse.CostOfGasolineAndFuel.ToString();

                try
                { 
                        CategoryOfDriving.SelectedItem = selectedCourse.course_CategotryOfDrivings.First();
                    
                }
                catch (Exception ex)
                {
                    CategoryOfDriving.SelectedItem = categoryOfDrivingList.First();
                }

            }
        }

        private void PupilSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Pupils selectedPupil = null;

            if (PupilsListBox.SelectedItem != null)
            {
                int plpId = (int)PupilsListBox.SelectedItem.GetType().GetProperty("PupilId").GetValue(PupilsListBox.SelectedItem, null);
                selectedPupil = db.Pupils.First(el => el.PupilId == plpId);
            }

            try
            {
                if (selectedPupil != null)
                {
                    if (selectedPupil.PupilId == 0)
                    {
                        var newPerson = new Person
                        {
                            FirstName = PupilFirstName.Text,
                            SecondName = PupilSecondName.Text,
                            Surname = PupilSurName.Text,
                            DateOfBirth = PupilBirthDate.SelectedDate,
                            PathToPhoto = PupilPhotoPath.Text,
                            SocialStatus = PupilStatus.Text,
                            PlaceOfStudy = PupilStudyPlace.Text,
                            PlaceOfWork = PupilWorkPlace.Text
                        };

                        db.Person.Add(newPerson);
                        db.SaveChanges();

                        var newPupil = new Pupils
                        {
                            SchoolId = ((DrivingSchools)DrivingSchoolsListBox.SelectedItem).DrivingSchoolId,
                            PersonId = db.Person.Max(el => el.PersonId),
                            IsCashlessPayments = (bool)PupilPayment.IsChecked
                        };

                        db.Pupils.Add(newPupil);
                        db.SaveChanges();
                    }
                    else
                    {
                        var selPupil = db.Pupils.Where(el => el.PupilId == selectedPupil.PupilId).FirstOrDefault();

                        selPupil.SchoolId = ((DrivingSchools)DrivingSchoolsListBox.SelectedItem).DrivingSchoolId;
                        selPupil.IsCashlessPayments = (bool)PupilPayment.IsChecked;

                        var selPerson = db.Person.Where(el => el.PersonId == selPupil.PersonId).FirstOrDefault();

                        selPerson.FirstName = PupilFirstName.Text;
                        selPerson.SecondName = PupilSecondName.Text;
                        selPerson.Surname = PupilSurName.Text;
                        selPerson.DateOfBirth = PupilBirthDate.SelectedDate;
                        selPerson.PathToPhoto = PupilPhotoPath.Text;
                        selPerson.SocialStatus = PupilStatus.Text;
                        selPerson.PlaceOfStudy = PupilStudyPlace.Text;
                        selPerson.PlaceOfWork = PupilWorkPlace.Text;

                        db.SaveChanges();
                    }
                }

                saveMessage3.Content = "Сохранено!";

                var pupilsList = db.Pupils.Where(el => el.SchoolId == DrivingSchoolId).Select(el => new
                {
                    el.PupilId,
                    el.IsCashlessPayments,
                    el.SchoolId,
                    el.PersonId,
                    FullName = el.person.SecondName + " " + el.person.FirstName + " " + el.person.Surname

                }).ToList();

                PupilsListBox.ItemsSource = pupilsList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PupilAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedDrivingSchool = (DrivingSchools)DrivingSchoolsListBox.SelectedItem;
                var pupilList = db.Pupils.Where(el => el.SchoolId == selectedDrivingSchool.DrivingSchoolId).ToList();

                if (DrivingSchoolsListBox.SelectedItem != null)
                {
                    var newPupil = new Pupils();
                    PupilSecondName.Text = "Иванов";
                    PupilFirstName.Text = "";
                    PupilSurName.Text = "";
                    PupilBirthDate.SelectedDate = null;
                    PupilStatus.Text = "";
                    PupilPhotoPath.Text = "";
                    PupilPayment.IsChecked = false;
                    PupilWorkPlace.Text = "";
                    PupilStudyPlace.Text = "";

                    pupilList.Add(newPupil);

                    var pupilsListNew = pupilList.Select(el => new
                    {
                        el.PupilId,
                        el.IsCashlessPayments,
                        el.SchoolId,
                        el.PersonId,
                        FullName = el.person.SecondName + " " + el.person.FirstName + " " + el.person.Surname

                    }).ToList();

                    PupilsListBox.ItemsSource = pupilsListNew;
                    PupilsListBox.SelectedItem = PupilsListBox.Items[PupilsListBox.Items.Count - 1];
                }
                else
                {
                    MessageBox.Show("Сначала выберите автошколу");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void PupilDelButton_Click(object sender, RoutedEventArgs e)
        {
            if (DrivingSchoolsListBox.SelectedItem != null)
            {
                Pupils selectedPupil = null;

                if (PupilsListBox.SelectedItem != null)
                {
                    int plpId = (int)PupilsListBox.SelectedItem.GetType().GetProperty("PupilId").GetValue(PupilsListBox.SelectedItem, null);
                    selectedPupil = db.Pupils.First(el => el.PupilId == plpId);
                }

                if (selectedPupil != null)
                {
                    db.Pupils.Remove(db.Pupils.Where(el => el.PupilId == selectedPupil.PupilId).FirstOrDefault());
                    db.SaveChanges();
                }

                saveMessage3.Content = "Успешно удалено!";

                var pupilsList = db.Pupils.Where(el => el.SchoolId == DrivingSchoolId).Select(el => new
                {
                    el.PupilId,
                    el.IsCashlessPayments,
                    el.SchoolId,
                    el.PersonId,
                    FullName = el.person.SecondName + " " + el.person.FirstName + " " + el.person.Surname

                }).ToList();
                PupilsListBox.ItemsSource = pupilsList;
            }
            else
            {
                MessageBox.Show("Сначала выберите автошколу");
            }
        }

        private void PupilListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PupilsListBox.SelectedItem != null)
            {
                Pupils selectedPupil = null;

                if (PupilsListBox.SelectedItem != null)
                {
                    int plpId = (int)PupilsListBox.SelectedItem.GetType().GetProperty("PupilId").GetValue(PupilsListBox.SelectedItem, null);
                    selectedPupil = db.Pupils.First(el => el.PupilId == plpId);
                }

                var selectedPupilPerson = db.Person.Where(el => el.PersonId == selectedPupil.PersonId).FirstOrDefault();

                if (selectedPupilPerson != null)
                {
                    PupilFirstName.Text = selectedPupilPerson.FirstName;
                    PupilSecondName.Text = selectedPupilPerson.SecondName;
                    PupilSurName.Text = selectedPupilPerson.Surname;
                    PupilBirthDate.SelectedDate = selectedPupilPerson.DateOfBirth;
                    PupilStatus.Text = selectedPupilPerson.SocialStatus;
                    PupilPhotoPath.Text = selectedPupilPerson.PathToPhoto;
                    PupilStudyPlace.Text = selectedPupilPerson.PlaceOfStudy;
                    PupilWorkPlace.Text = selectedPupilPerson.PlaceOfWork;
                    PupilPayment.IsChecked = selectedPupil.IsCashlessPayments;
                }
            }
        }

        private void WorkerSaveButton_Click(object sender, RoutedEventArgs e)
        {
         
            CoWorkers selectedWorker = null;

            if (WorkersListBox.SelectedItem != null)
            {
                int Id = (int)WorkersListBox.SelectedItem.GetType().GetProperty("CoWorkerId").GetValue(WorkersListBox.SelectedItem, null);
                selectedWorker = db.CoWorkers.First(el => el.CoWorkerId == Id);
            }

            try
            {
                if (selectedWorker != null)
                {
                    if (selectedWorker.CoWorkerId == 0)
                    {
                        var newPerson = new Person
                        {
                            FirstName = WorkerFirstName.Text,
                            SecondName = WorkerSecondName.Text,
                            Surname = WorkerSurName.Text,
                            DateOfBirth = WorkerBirthDate.SelectedDate,
                            PathToPhoto = WorkerPhotoPath.Text,
                            SocialStatus = WorkerStatus.Text,
                        };

                        db.Person.Add(newPerson);
                        db.SaveChanges();

                        var newWorker = new CoWorkers
                        {
                            SchoolId = ((DrivingSchools)DrivingSchoolsListBox.SelectedItem).DrivingSchoolId,
                            PersonId = db.Person.Max(el => el.PersonId)
                        };

                        db.CoWorkers.Add(newWorker);
                        db.SaveChanges();
                    }
                    else
                    {
                        var selWorker = db.CoWorkers.Where(el => el.CoWorkerId == selectedWorker.CoWorkerId).FirstOrDefault();

                        selWorker.SchoolId = ((DrivingSchools)DrivingSchoolsListBox.SelectedItem).DrivingSchoolId;

                        var selPerson = db.Person.Where(el => el.PersonId == selWorker.PersonId).FirstOrDefault();

                        selPerson.FirstName = WorkerFirstName.Text;
                        selPerson.SecondName = WorkerSecondName.Text;
                        selPerson.Surname = WorkerSurName.Text;
                        selPerson.DateOfBirth = WorkerBirthDate.SelectedDate;
                        selPerson.PathToPhoto = WorkerPhotoPath.Text;
                        selPerson.SocialStatus = WorkerStatus.Text;

                        db.SaveChanges();
                    }
                }

                saveMessage4.Content = "Сохранено!";
                var workersList = db.CoWorkers.Where(el => el.SchoolId == DrivingSchoolId).Select(el => new
                {
                    el.CoWorkerId,
                    el.SchoolId,
                    el.PersonId,
                    FullName = el.person.SecondName + " " + el.person.FirstName + " " + el.person.Surname

                }).ToList();
                WorkersListBox.ItemsSource = workersList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void WorkerAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedDrivingSchool = (DrivingSchools)DrivingSchoolsListBox.SelectedItem;
                var workersList = db.CoWorkers.Where(el => el.SchoolId == selectedDrivingSchool.DrivingSchoolId).ToList();


                if (DrivingSchoolsListBox.SelectedItem != null)
                {
                    var newWorker = new CoWorkers();
                    WorkerFirstName.Text = "";
                    WorkerSecondName.Text = "Иванов";
                    WorkerSurName.Text = "";
                    WorkerBirthDate.SelectedDate = null;
                    WorkerStatus.Text = "";
                    WorkerPhotoPath.Text = "";

                    workersList.Add(newWorker);

                    var workersListNew = workersList.Select(el => new
                    {
                        el.CoWorkerId,
                        el.SchoolId,
                        el.PersonId,
                        FullName = el.person.SecondName + " " + el.person.FirstName + " " + el.person.Surname

                    }).ToList();

                    WorkersListBox.ItemsSource = workersListNew;
                    WorkersListBox.SelectedItem = WorkersListBox.Items[WorkersListBox.Items.Count - 1];
                }
                else
                {
                    MessageBox.Show("Сначала выберите автошколу");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void WorkerDelButton_Click(object sender, RoutedEventArgs e)
        {
            if (DrivingSchoolsListBox.SelectedItem != null)
            {
                CoWorkers selectedWorker = null;

                if (WorkersListBox.SelectedItem != null)
                {
                    int Id = (int)WorkersListBox.SelectedItem.GetType().GetProperty("CoWorkerId").GetValue(WorkersListBox.SelectedItem, null);
                    selectedWorker = db.CoWorkers.First(el => el.CoWorkerId == Id);
                }

                if (selectedWorker != null)
                {
                    db.CoWorkers.Remove(db.CoWorkers.Where(el => el.CoWorkerId == selectedWorker.CoWorkerId).FirstOrDefault());
                    db.SaveChanges();
                }

                saveMessage4.Content = "Успешно удалено!";
                var workersList = db.CoWorkers.Where(el => el.SchoolId == DrivingSchoolId).Select(el => new
                {
                    el.CoWorkerId,
                    el.SchoolId,
                    el.PersonId,
                    FullName = el.person.SecondName + " " + el.person.FirstName + " " + el.person.Surname

                }).ToList();
                WorkersListBox.ItemsSource = workersList;
            }
            else
            {
                MessageBox.Show("Сначала выберите автошколу");
            }
        }

        private void WorkersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WorkersListBox.SelectedItem != null)
            {
                CoWorkers selectedWorker = null;

                if (WorkersListBox.SelectedItem != null)
                {
                    int Id = (int)WorkersListBox.SelectedItem.GetType().GetProperty("CoWorkerId").GetValue(WorkersListBox.SelectedItem, null);
                    selectedWorker = db.CoWorkers.First(el => el.CoWorkerId == Id);
                }

                var selectedWorkerPerson = db.Person.Where(el => el.PersonId == selectedWorker.PersonId).FirstOrDefault();

                if (selectedWorkerPerson != null)
                {
                    WorkerFirstName.Text = selectedWorkerPerson.FirstName;
                    WorkerSecondName.Text = selectedWorkerPerson.SecondName;
                    WorkerSurName.Text = selectedWorkerPerson.Surname;
                    WorkerBirthDate.SelectedDate = selectedWorkerPerson.DateOfBirth;
                    WorkerStatus.Text = selectedWorkerPerson.SocialStatus;
                    WorkerPhotoPath.Text = selectedWorkerPerson.PathToPhoto;
                }
            }
        }

        private void CarSaveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCar = (Cars)CarsListBox.SelectedItem;

            try
            {
                if (selectedCar != null)
                {
                    if (selectedCar.CarId == 0)
                    {
                        var newCar = new Cars
                        {
                            CarType = CarType.Text,
                            CarModel = CarModel.Text,
                            CarProductionYear = (DateTime)CarProductionYear.SelectedDate,
                            YearOfAdmissionToDrivingSchool = (DateTime)CarAdmissionYear.SelectedDate,
                            SchoolId = ((DrivingSchools)DrivingSchoolsListBox.SelectedItem).DrivingSchoolId
                        };

                        db.Cars.Add(newCar);
                        db.SaveChanges();
                    }
                    else
                    {
                        var selCar = db.Cars.Where(el => el.CarId == selectedCar.CarId).FirstOrDefault();

                        selCar.SchoolId = ((DrivingSchools)DrivingSchoolsListBox.SelectedItem).DrivingSchoolId;
                        selCar.CarId = db.Cars.Max(el => el.CarId);

                        selCar.CarType = CarType.Text;
                        selCar.CarModel = CarModel.Text;
                        selCar.CarProductionYear = (DateTime)CarProductionYear.SelectedDate;
                        selCar.YearOfAdmissionToDrivingSchool = (DateTime)CarAdmissionYear.SelectedDate;

                        db.SaveChanges();
                    }
                }

                saveMessage5.Content = "Сохранено!";
                var carsList = db.Cars.Where(el => el.SchoolId == ((DrivingSchools)DrivingSchoolsListBox.SelectedItem).DrivingSchoolId).ToList();
                CarsListBox.ItemsSource = carsList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CarAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedDrivingSchool = (DrivingSchools)DrivingSchoolsListBox.SelectedItem;
                var carsList = db.Cars.Where(el => el.SchoolId == selectedDrivingSchool.DrivingSchoolId).ToList();
                if (DrivingSchoolsListBox.SelectedItem != null)
                {
                    var newCar = new Cars();

                    carsList.Add(newCar);

                    CarsListBox.ItemsSource = carsList;
                    CarsListBox.SelectedItem = CarsListBox.Items[CarsListBox.Items.Count - 1];
                }
                else
                {
                    MessageBox.Show("Сначала выберите автошколу");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CarDelButton_Click(object sender, RoutedEventArgs e)
        {
            if (DrivingSchoolsListBox.SelectedItem != null)
            {
                var selectedCar = (Cars)CarsListBox.SelectedItem;
                if (selectedCar != null)
                {
                    db.Cars.Remove(db.Cars.Where(el => el.CarId == selectedCar.CarId).FirstOrDefault());
                    db.SaveChanges();
                }

                saveMessage5.Content = "Успешно удалено!";
                var carsList = db.Cars.Where(el => el.SchoolId == ((DrivingSchools)DrivingSchoolsListBox.SelectedItem).DrivingSchoolId).ToList();
                CarsListBox.ItemsSource = carsList;
            }
            else
            {
                MessageBox.Show("Сначала выберите автошколу");
            }
        }

        private void CarsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CarsListBox.SelectedItem != null)
            {
                var selectedCar = (Cars)CarsListBox.SelectedItem;

                CarType.Text = selectedCar.CarType;
                CarModel.Text = selectedCar.CarModel;
                CarProductionYear.SelectedDate = selectedCar.CarProductionYear;
                CarAdmissionYear.SelectedDate = selectedCar.YearOfAdmissionToDrivingSchool;
            }
        }

        private void LessonSaveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedLesson = (DrivingLessons)LessonsListBox.SelectedItem;

            try
            {
                if (selectedLesson != null)
                {
                    if (selectedLesson.DrivingLessonsId == 0)
                    {
                        var newLesson = new DrivingLessons
                        {
                            CarId = Convert.ToInt32(LessonCarId.Text),
                            CoWorkerId = Convert.ToInt32(LessonWorkerId.Text),
                            SessionDate = (DateTime)LessonDate.SelectedDate,
                            SchoolId = ((DrivingSchools)DrivingSchoolsListBox.SelectedItem).DrivingSchoolId
                        };

                        db.DrivingLessons.Add(newLesson);
                        db.SaveChanges();
                    }
                    else
                    {
                        var selLesson = db.DrivingLessons.Where(el => el.DrivingLessonsId == selectedLesson.DrivingLessonsId).FirstOrDefault();

                        selLesson.SchoolId = ((DrivingSchools)DrivingSchoolsListBox.SelectedItem).DrivingSchoolId;
                        selLesson.DrivingLessonsId = db.DrivingLessons.Max(el => el.DrivingLessonsId);

                        selLesson.CarId = Convert.ToInt32(LessonCarId.Text);
                        selLesson.CoWorkerId = Convert.ToInt32(LessonWorkerId.Text);
                        selLesson.SessionDate = (DateTime)LessonDate.SelectedDate;

                        db.SaveChanges();
                    }
                }

                saveMessage6.Content = "Сохранено!";
                var lessonsList = db.DrivingLessons.Where(el => el.SchoolId == ((DrivingSchools)DrivingSchoolsListBox.SelectedItem).DrivingSchoolId).ToList();
                LessonsListBox.ItemsSource = lessonsList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LessonAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedDrivingSchool = (DrivingSchools)DrivingSchoolsListBox.SelectedItem;
                var lessonsList = db.DrivingLessons.Where(el => el.SchoolId == selectedDrivingSchool.DrivingSchoolId).ToList();
                if (DrivingSchoolsListBox.SelectedItem != null)
                {
                    var newLesson = new DrivingLessons();

                    lessonsList.Add(newLesson);

                    LessonsListBox.ItemsSource = lessonsList;
                    LessonsListBox.SelectedItem = LessonsListBox.Items[LessonsListBox.Items.Count - 1];
                }
                else
                {
                    MessageBox.Show("Сначала выберите автошколу");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void LessonDelButton_Click(object sender, RoutedEventArgs e)
        {
            if (DrivingSchoolsListBox.SelectedItem != null)
            {
                var selectedLesson = (DrivingLessons)LessonsListBox.SelectedItem;
                if (selectedLesson != null)
                {
                    db.DrivingLessons.Remove(db.DrivingLessons.Where(el => el.DrivingLessonsId == selectedLesson.DrivingLessonsId).FirstOrDefault());
                    db.SaveChanges();
                }

                saveMessage6.Content = "Успешно удалено!";
                var lessonsList = db.DrivingLessons.Where(el => el.SchoolId == ((DrivingSchools)DrivingSchoolsListBox.SelectedItem).DrivingSchoolId).ToList();
                LessonsListBox.ItemsSource = lessonsList;
            }
            else
            {
                MessageBox.Show("Сначала выберите автошколу");
            }
        }

        private void LessonsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LessonsListBox.SelectedItem != null)
            {
                var selectedLesson = (DrivingLessons)LessonsListBox.SelectedItem;

                LessonCarId.Text = selectedLesson.car.CarType + " " + selectedLesson.car.CarModel;
                LessonWorkerId.Text = selectedLesson.coWorker.person.SecondName + " " + selectedLesson.coWorker.person.FirstName + " " + selectedLesson.coWorker.person.Surname;
                LessonDate.SelectedDate = selectedLesson.SessionDate;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow mw = new ReportWindow();
            mw.Show();
        }

        private void sortListDrivingSchools_Click(object sender, RoutedEventArgs e)
        {
            if (sortListDrivingSchools.Content.ToString() == "По убыванию")
            {
                sortListDrivingSchools.Content = "По возрастанию";
                DrivingSchoolsListBox.ItemsSource = db.DrivingSchools.OrderByDescending(el => el.DrivingSchoolName).ToList();
            }
            else
            {
                sortListDrivingSchools.Content = "По убыванию";
                DrivingSchoolsListBox.ItemsSource = db.DrivingSchools.OrderBy(el => el.DrivingSchoolName).ToList();
            }
        }

        private void sortListCourses_Click(object sender, RoutedEventArgs e)
        {
            if (sortListCourses.Content.ToString() == "По убыванию")
            {
                sortListCourses.Content = "По возрастанию";
                CoursesListBox.ItemsSource = db.Courses.Where(el => el.DrivingSchoolId == DrivingSchoolId).OrderByDescending(el => el.CourseName).ToList();
            }
            else
            {
                sortListCourses.Content = "По убыванию";
                CoursesListBox.ItemsSource = db.Courses.Where(el => el.DrivingSchoolId == DrivingSchoolId).OrderBy(el => el.CourseName).ToList();
            }
        }

        private void sortListPupils_Click(object sender, RoutedEventArgs e)
        {
            var pupilsList = db.Pupils.Where(el => el.SchoolId == DrivingSchoolId).Select(el => new
            {
                el.PupilId,
                el.IsCashlessPayments,
                el.SchoolId,
                el.PersonId,
                FullName = el.person.SecondName + " " + el.person.FirstName + " " + el.person.Surname

            }).ToList();

            if (sortListPupils.Content.ToString() == "По убыванию")
            {
                sortListPupils.Content = "По возрастанию";
                PupilsListBox.ItemsSource = pupilsList.OrderByDescending(el => el.FullName);
            }
            else
            {
                sortListPupils.Content = "По убыванию";
                PupilsListBox.ItemsSource = pupilsList.OrderBy(el => el.FullName).ToList();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            QuerysWindow mw = new QuerysWindow();
            mw.Show();
        }

        private void sortWorkerList_Click(object sender, RoutedEventArgs e)
        {
            var workersList = db.CoWorkers.Where(el => el.SchoolId == DrivingSchoolId).Select(el => new
            {
                el.CoWorkerId,
                el.SchoolId,
                el.PersonId,
                FullName = el.person.SecondName + " " + el.person.FirstName + " " + el.person.Surname

            }).ToList();

            if (sortWorkerList.Content.ToString() == "По убыванию")
            {
                sortWorkerList.Content = "По возрастанию";
                WorkersListBox.ItemsSource = workersList.OrderByDescending(el => el.FullName).ToList();
            }
            else
            {
                sortWorkerList.Content = "По убыванию";
                WorkersListBox.ItemsSource = workersList.OrderBy(el => el.FullName).ToList();
            }
        }

        private void sortCarList_Click(object sender, RoutedEventArgs e)
        {
            if (sortCarList.Content.ToString() == "По убыванию")
            {
                sortCarList.Content = "По возрастанию";
                CarsListBox.ItemsSource = db.Cars.Where(el => el.SchoolId == DrivingSchoolId).OrderByDescending(el => el.CarModel).ToList();
            }
            else
            {
                sortCarList.Content = "По убыванию";
                CarsListBox.ItemsSource = db.Cars.Where(el => el.SchoolId == DrivingSchoolId).OrderBy(el => el.CarModel).ToList();
            }
        }

        private void sortLessonList_Click(object sender, RoutedEventArgs e)
        {
            if (sortLessonList.Content.ToString() == "По убыванию")
            {
                sortLessonList.Content = "По возрастанию";
                LessonsListBox.ItemsSource = db.DrivingLessons.Where(el => el.SchoolId == DrivingSchoolId).OrderByDescending(el => el.SessionDate).ToList();
            }
            else
            {
                sortLessonList.Content = "По убыванию";
                LessonsListBox.ItemsSource = db.DrivingLessons.Where(el => el.SchoolId == DrivingSchoolId).OrderBy(el => el.SessionDate).ToList();
            }
        }

     
    }
}
