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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private DbContext_Prog db = new DbContext_Prog(ConfigurationManager.ConnectionStrings["PCString"].ConnectionString);

        public LoginWindow()
        {
            InitializeComponent();
            AutoFill();
            //clearDB();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var usersList = db.Users.ToList();

            if (usersList.Count > 0)
            {
                foreach (var user in usersList)
                {
                    if (user.UserLogin == Login2.Text && user.UserPass == Password.Password.ToString())
                    {
                        MessageBox.Show("Успешная авторизация!");
                        MainWindow mw = new MainWindow();
                        mw.Show();
                        this.Close();
                        return;
                    }
                }
                MessageBox.Show("Неправильный логин или пароль.");
                return;
            }
            else
            {
                MessageBox.Show("Ни один пользователь ещё не зарегистрирован. Сначала зарегистрируйтесь.");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var usersList = db.Users.ToList();
            foreach (var user in usersList) // если пользователь уже есть
            {
                if (user.UserLogin.ToString() == Login.Text.ToString())
                {
                    MessageBox.Show("Пользователь с таким именем уже существует!");
                    return;
                }
            }

            if (Password1.Password.ToString() == Password2.Password.ToString())
            {
                var newUser = new Users
                {
                    UserLogin = Login.Text.ToString(),
                    UserPass = Password1.Password.ToString(),
                };
                try
                {
                    db.Users.Add(newUser);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Введенные пароли не совпадают.");
                return;
            }

            MessageBox.Show("Успешная регистрация.");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            HelloGrid.Visibility = Visibility.Hidden;
            LoginGrid.Visibility = Visibility.Visible;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            HelloGrid.Visibility = Visibility.Hidden;
            RegisterGrid.Visibility = Visibility.Visible;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            HelloGrid.Visibility = Visibility.Visible;
            RegisterGrid.Visibility = Visibility.Hidden;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            HelloGrid.Visibility = Visibility.Visible;
            LoginGrid.Visibility = Visibility.Hidden;
        }

       
        private DateTime GenRandomDate(DateTime from, DateTime to, Random random = null)
        {
            if (from >= to)
            {
                throw new Exception("Параметр \"from\" должен быть меньше параметра \"to\"!");
            }
            if (random == null)
            {
                random = new Random();
            }
            int daysDiff = (to - from).Days;
            return from.AddDays(random.Next(daysDiff));
        }

        private void clearDB()
        {
            db.Database.ExecuteSqlCommand("DELETE FROM Course_CategotryOfDriving");
            db.Database.ExecuteSqlCommand("DELETE FROM Courses");
            db.Database.ExecuteSqlCommand("DELETE FROM DrivingSchools");
            db.Database.ExecuteSqlCommand("DELETE FROM TypeOfProperties");
            db.Database.ExecuteSqlCommand("DELETE FROM CategoriesOfDrivings");
            db.Database.ExecuteSqlCommand("DELETE FROM People");
            db.Database.ExecuteSqlCommand("DELETE FROM CoWorkers");
            db.Database.ExecuteSqlCommand("DELETE FROM Cars");
            db.Database.ExecuteSqlCommand("DELETE FROM Addresses");
            db.Database.ExecuteSqlCommand("DELETE FROM DrivingLessons");
            db.Database.ExecuteSqlCommand("DELETE FROM Pupils");
        }


        private void AutoFill()
        {

            MessageAutoClose mw = new MessageAutoClose("Идет автозаполнение БД....");
            mw.Show();

            Random rnd = new Random();

            //string[] cities = { "Амстердам", "Антверпен", "Архангельск", "Астрахань", "Атланта", "Афины", "Баден (Аргау)","Базель",
            //    "Донецк", "Макеевка",};

            //string[] streets = { "Малиновского", "Малоозерная", "Малосадовая", "Малый Садовый", "Малышева", "Малышкина", "Малышко","Мамаева Кургана",
            //    "Маметовой", "Мануильского",};

            //string[] districts = { "Абазинский", "Абанский", "Абатский", "Абзелиловский", "Абинский", "Абыйский улус", "Адамовский","Адыге-Хабльский",
            //    "Азнакаевский", "Азовский немецкий национальный",};

            //for (int i = 0; i < 2000; i++)
            //{
            //    var home = Convert.ToString(rnd.Next(1, 9000));
            //    var districtIndex = Convert.ToInt32(rnd.Next(0, districts.Count() - 1));
            //    var streetIndex = Convert.ToInt32(rnd.Next(0, streets.Count() - 1));
            //    var citiyIndex = Convert.ToInt32(rnd.Next(0, cities.Count() - 1));

            //    var addr = new Addresses
            //    {
            //        District = districts[districtIndex],
            //        City = cities[citiyIndex],
            //        Street = streets[streetIndex],
            //        Home = home
            //    };

            //    db.Addresses.Add(addr);

            //}

            //db.SaveChanges();

            //string[] typesOfProp = { "Частная", "Государственая" };

            //foreach (var typeP in typesOfProp)
            //{
            //    var typeOfProp = new TypeOfProperty
            //    {
            //        TypeOfPropertyName = typeP
            //    };

            //    db.TypeOfProperty.Add(typeOfProp);
            //}


            //db.SaveChanges();



            string [] drivingSchoolNames =  { "Серна - 2000", "АвтоКураж", "АвтоМаг", "Унисерв", "Альбатрос", "АВЕТА", "АВ-К","Жайворонок",
                "Автогор", "Перспект"};

            var addrList = db.Addresses.ToList();
            var typesOfPropList = db.TypeOfProperty.ToList();
            int addrListCount = addrList.Count() - 1;
            int drivingSchoolNamesCount = drivingSchoolNames.Count() - 1;
            int typesOfPropListCount = typesOfPropList.Count() - 1;

            for (int i = 0; i < 2000; i++)
                {
                   
                    var site = Convert.ToString(rnd.Next(1, 9000));
                    var addrIndex = Convert.ToInt32(rnd.Next(0, addrListCount));
                    var nameIndex = Convert.ToInt32(rnd.Next(0, drivingSchoolNamesCount));
                    var typeOfPropIndex = Convert.ToInt32(rnd.Next(0, typesOfPropListCount));

                    var dr = new DrivingSchools
                    {
                        DrivingSchoolName = drivingSchoolNames[nameIndex] + "№ " + (i + 1).ToString(),
                        DrivingSchoolSite = "www.auto" + (i + 1).ToString() + "school" + site + ".ru",
                        AddressId = addrList[addrIndex].AddressId,
                        TypeOfPropertyId = typesOfPropList[typeOfPropIndex].TypeOfPropertyId
                    };

                    db.DrivingSchools.Add(dr);
                }

            db.SaveChanges();

            string []carNames = { "Alfa Romeo", "Bentley", "Bugatti", "BMW", "Acura", "Aston Martin", "Audi","Brilliance",
                "Cadillac", "Changan", "Chery", "Chevrolet", "Chrysler", "Citroen", "Datsun",
             "DongFeng", "DS", "FAW", "Ferrari", "Fiat", "Ford", "Foton", "Geely",};


            string[] carTypes = { "Купе", "Седан", "Хэтчбек", "Универсал", "Лимузин", "Пикап", "Кроссовер","Фургон",
                "Минивен", "Внедорожник",};

            var drList = db.DrivingSchools.ToList();
            int carNamesCount = carNames.Count() - 1;
            int drListCount = drList.Count() - 1;
            int carTypesCount = carTypes.Count() - 1;

            for (int i = 0; i < 2000; i++)
            {
                var drIndex = Convert.ToInt32(rnd.Next(0, drListCount));
                var carModelIndex = Convert.ToInt32(rnd.Next(0, carNamesCount));
                var carTypeIndex = Convert.ToInt32(rnd.Next(0, carTypesCount));
                var carProductionYear = GenRandomDate(new DateTime(1990, 1, 1), new DateTime(DateTime.Now.Year - 1, 1, 1));

                var car = new Cars
                {
                    SchoolId = drList[drIndex].DrivingSchoolId,
                    CarModel = carNames[carModelIndex],
                    CarType = carTypes[carTypeIndex],
                    CarProductionYear = carProductionYear,
                    YearOfAdmissionToDrivingSchool = GenRandomDate(carProductionYear, new DateTime(DateTime.Now.Year - 1, 1, 1))
                };

                db.Cars.Add(car);
                db.SaveChanges(); 
            }


            string []categoryOfDrivingNames = {"A", "B", "C", "D", "E"};

            foreach (var catName in categoryOfDrivingNames)
            {
                var carDr = new CategoriesOfDriving();
                carDr.CategoryOfDrivingName = catName;
                db.CategoriesOfDriving.Add(carDr);
            }

            db.SaveChanges();


            string[] personNames = { "Олег", "Иван", "Николай", "Андрей", "Евгений", "Александр", "Петр", "Василий", "Артем", "Павел", "Дмитрий" };

            string[] personSurnames = { "Абрамов", "Авдеев", "Агафонов ", "Аксёнов ", "Александров " , "Алексеев", "Андреев", "Анисимов", "Антонов ", "Артемьев" };

            string[] personSecondNames = { "Назарович", "Наумович", "Николаевич", "Олегович", "Павлович" };


            for (int i = 0; i < 5000; i++)
            {
                var secIndex = Convert.ToInt32(rnd.Next(0, personSurnames.Count() - 1));
                var nameIndex = Convert.ToInt32(rnd.Next(0, personNames.Count() - 1));
                var surIndex = Convert.ToInt32(rnd.Next(0, personSurnames.Count() - 1));

                var pers = new Person
                {
                    Surname = personSecondNames[secIndex],
                    FirstName = personNames[nameIndex],
                    SecondName = personSurnames[surIndex],
                    DateOfBirth = GenRandomDate(new DateTime(DateTime.Now.Year - 120, 12, 31), new DateTime(DateTime.Now.Year - 16, 12, 31)),
                    SocialStatus = "Холостой",
                    PlaceOfStudy = "Высшее"
                };

                db.Person.Add(pers);
            }

            db.SaveChanges();


            for (int i = 0; i < 1000; i++)
            {
                var drIndex = Convert.ToInt32(rnd.Next(0, drList.Count() - 1));

                var secIndex = Convert.ToInt32(rnd.Next(0, personSurnames.Count() - 1));
                var nameIndex = Convert.ToInt32(rnd.Next(0, personNames.Count() - 1));
                var surIndex = Convert.ToInt32(rnd.Next(0, personSurnames.Count() - 1));
                var courseDuration = Convert.ToInt32(rnd.Next(10, 100));

                var curs = new Courses
                {
                    DrivingSchoolId = drList[drIndex].DrivingSchoolId,
                    CourseName = "Курс № " + (i + 1).ToString(),
                    DateOfBeginningCourse = GenRandomDate(new DateTime(1990, 1, 1), new DateTime(DateTime.Now.Year+1, 12, 31)),
                    CourseDuration = courseDuration,
                    CostOfEducation = courseDuration * 10,
                    CostOfGasolineAndFuel = courseDuration * 10 + 2000,
                    TrainingPeriod = courseDuration + Convert.ToInt32(rnd.Next(10, 100))
                };

                db.Courses.Add(curs);
            }

            db.SaveChanges();

            var persons = db.Person.ToList();

            for (int i = 0; i < 3000; i++)
            {
                var drIndex = Convert.ToInt32(rnd.Next(0, drList.Count() - 1));

                var pup = new Pupils
                {
                    SchoolId = drList[drIndex].DrivingSchoolId,
                    PersonId = persons[i].PersonId,
                    IsCashlessPayments = Convert.ToBoolean(rnd.Next(0, 1))
                };

                db.Pupils.Add(pup);
            }

            db.SaveChanges();


            for (int i = 3000; i < 5000; i++)
            {
                var drIndex = Convert.ToInt32(rnd.Next(0, drList.Count() - 1));

                var worker = new CoWorkers
                {
                    SchoolId = drList[drIndex].DrivingSchoolId,
                    PersonId = persons[i].PersonId, 
                };

                db.CoWorkers.Add(worker);
            }

            db.SaveChanges();


            var cars = db.Cars.ToList();
            var coWorkers = db.CoWorkers.ToList();

            for (int i = 0; i < 5000; i++)
            {
                var drIndex = Convert.ToInt32(rnd.Next(0, drList.Count() - 1));
                var carIndex = Convert.ToInt32(rnd.Next(0, cars.Count() - 1));
                var cowIndex = Convert.ToInt32(rnd.Next(0, coWorkers.Count() - 1));

                var les = new DrivingLessons
                {
                    SchoolId = drList[drIndex].DrivingSchoolId,
                    CoWorkerId = coWorkers[cowIndex].CoWorkerId,
                    CarId = cars[carIndex].CarId,
                    SessionDate = GenRandomDate(new DateTime(DateTime.Now.Year, 10, 1), new DateTime(DateTime.Now.Year + 1, 12, 31)),
                };

                db.DrivingLessons.Add(les);
            }

            db.SaveChanges();


            var courseList = db.Courses.ToList();
            var categories = db.CategoriesOfDriving.ToList();


            for (int i = 0; i < 1000; i++)
            {
                var courseIndex = Convert.ToInt32(rnd.Next(0, courseList.Count() - 1));
                var catIndex = Convert.ToInt32(rnd.Next(0, categories.Count() - 1));
                var cowIndex = Convert.ToInt32(rnd.Next(0, coWorkers.Count() - 1));

                var courseCateg = new Course_CategotryOfDriving
                {
                    CourseId = courseList[courseIndex].CourseId,
                    CoWorkerId = coWorkers[cowIndex].CoWorkerId,
                    CategoryOfDrivingId = categories[catIndex].CategoryOfDrivingId
                };

                db.Course_CategotryOfDriving.Add(courseCateg);
            }

            db.SaveChanges();

            mw.Close();
        }
    }
}