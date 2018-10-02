using Driving_School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Driving_School
{
    /// <summary>
    /// Логика взаимодействия для Автозаполнение.xaml
    /// </summary>
    public partial class AutoFill : Window
    {
        private DbContext_Prog db = new DbContext_Prog(System.Configuration.ConfigurationManager.ConnectionStrings["PCString"].ConnectionString);

        public AutoFill()
        {
            InitializeComponent();
           
        }

        private void NewWindowHandler(object sender, RoutedEventArgs e)
        {
            Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
        }

        private void ThreadStartingPoint()
        {
            MessageAutoClose tempWindow = new MessageAutoClose();
            tempWindow.Show();
            Dispatcher.Run();
        }

        void CloseWindowSafe(Window w)
        {
            if (w.Dispatcher.CheckAccess())
                w.Close();
            else
                w.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(w.Close));
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

            MessageBox.Show("БД очищена!");
        }


        private void AutoFillDb()
        {

           // ThreadStartingPoint();

           Random rnd = new Random();

            string[] cities = { "Амстердам", "Антверпен", "Архангельск", "Астрахань", "Атланта", "Афины", "Баден (Аргау)","Базель",
                "Донецк", "Макеевка",};

            string[] streets = { "Малиновского", "Малоозерная", "Малосадовая", "Малый Садовый", "Малышева", "Малышкина", "Малышко","Мамаева Кургана",
                "Маметовой", "Мануильского",};

            string[] districts = { "Абазинский", "Абанский", "Абатский", "Абзелиловский", "Абинский", "Абыйский улус", "Адамовский","Адыге-Хабльский",
                "Азнакаевский", "Азовский немецкий национальный",};

            var addressesList = new List<Addresses>();

            for (int i = 0; i < 2000; i++)
            {
                var home = Convert.ToString(rnd.Next(1, 9000));
                var districtIndex = Convert.ToInt32(rnd.Next(0, districts.Count() - 1));
                var streetIndex = Convert.ToInt32(rnd.Next(0, streets.Count() - 1));
                var citiyIndex = Convert.ToInt32(rnd.Next(0, cities.Count() - 1));

                var addr = new Addresses
                {
                    District = districts[districtIndex],
                    City = cities[citiyIndex],
                    Street = streets[streetIndex],
                    Home = home
                };

                addressesList.Add(addr);
                //mw.progAutoFillDb.Value += 0.0045248868778281;
            }

            db.Addresses.AddRange(addressesList);
            db.SaveChanges();

            string[] typesOfProp = { "Частная", "Государственая" };

            foreach (var typeP in typesOfProp)
            {
                var typeOfProp = new TypeOfProperty
                {
                    TypeOfPropertyName = typeP
                };

                db.TypeOfProperty.Add(typeOfProp);
            }


            db.SaveChanges();


            string[] drivingSchoolNames =  { "Серна - 2000", "АвтоКураж", "АвтоМаг", "Унисерв", "Альбатрос", "АВЕТА", "АВ-К","Жайворонок",
                "Автогор", "Перспект"};

            var addrList = db.Addresses.ToList();
            var typesOfPropList = db.TypeOfProperty.ToList();
            int addrListCount = addrList.Count() - 1;
            int drivingSchoolNamesCount = drivingSchoolNames.Count() - 1;
            int typesOfPropListCount = typesOfPropList.Count() - 1;
            var resultListDrivingSchools = new List<DrivingSchools>();

            for (int i = 0; i < 100; i++)
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

                resultListDrivingSchools.Add(dr);
               // mw.progAutoFillDb.Value += 0.0045248868778281;
            }

            db.DrivingSchools.AddRange(resultListDrivingSchools);
            db.SaveChanges();



            string[] carNames = { "Alfa Romeo", "Bentley", "Bugatti", "BMW", "Acura", "Aston Martin", "Audi","Brilliance",
                "Cadillac", "Changan", "Chery", "Chevrolet", "Chrysler", "Citroen", "Datsun",
             "DongFeng", "DS", "FAW", "Ferrari", "Fiat", "Ford", "Foton", "Geely",};


            string[] carTypes = { "Купе", "Седан", "Хэтчбек", "Универсал", "Лимузин", "Пикап", "Кроссовер","Фургон",
                "Минивен", "Внедорожник",};

            var drList = db.DrivingSchools.ToList();
            int carNamesCount = carNames.Count() - 1;
            int drListCount = drList.Count() - 1;
            int carTypesCount = carTypes.Count() - 1;

            var resultListCars = new List<Cars>();

            for (int i = 0; i < 3000; i++)
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

                resultListCars.Add(car);
                //mw.progAutoFillDb.Value += 0.0045248868778281;
            }

            db.Cars.AddRange(resultListCars);
            db.SaveChanges();

            string[] categoryOfDrivingNames = { "A", "B", "C", "D", "E" };

            foreach (var catName in categoryOfDrivingNames)
            {
                var carDr = new CategoriesOfDriving();
                carDr.CategoryOfDrivingName = catName;
                db.CategoriesOfDriving.Add(carDr);
            }

            db.SaveChanges();


            string[] personNames = { "Олег", "Иван", "Николай", "Андрей", "Евгений", "Александр", "Петр", "Василий", "Артем", "Павел", "Дмитрий" };

            string[] personSurnames = { "Абрамов", "Авдеев", "Агафонов ", "Аксёнов ", "Александров ", "Алексеев", "Андреев", "Анисимов", "Антонов ", "Артемьев" };

            string[] personSecondNames = { "Назарович", "Наумович", "Николаевич", "Олегович", "Павлович" };


            var resultListPerson = new List<Person>();

            for (int i = 0; i < 5000; i++)
            {
                var secIndex = Convert.ToInt32(rnd.Next(0, personSecondNames.Count() - 1));
                var nameIndex = Convert.ToInt32(rnd.Next(0, personNames.Count() - 1));
                var surIndex = Convert.ToInt32(rnd.Next(0, personSurnames.Count() - 1));

                var pers = new Person
                {
                    Surname = personSecondNames[secIndex],
                    FirstName = personNames[nameIndex],
                    SecondName = personSurnames[surIndex],
                    DateOfBirth = GenRandomDate(new DateTime(DateTime.Now.Year - 40, 12, 31), new DateTime(DateTime.Now.Year - 16, 12, 31)),
                    SocialStatus = "Холостой",
                    PlaceOfStudy = "Высшее"
                };

                resultListPerson.Add(pers);
               // mw.progAutoFillDb.Value += 0.0045248868778281;
            }
            db.Person.AddRange(resultListPerson);
            db.SaveChanges();


            var resultListCourses = new List<Courses>();

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
                    DateOfBeginningCourse = GenRandomDate(new DateTime(1990, 1, 1), new DateTime(DateTime.Now.Year + 1, 12, 31)),
                    CourseDuration = courseDuration,
                    CostOfEducation = courseDuration * 10,
                    CostOfGasolineAndFuel = courseDuration * 10 + 2000,
                    TrainingPeriod = courseDuration + Convert.ToInt32(rnd.Next(10, 100))
                };

                resultListCourses.Add(curs);
              //  mw.progAutoFillDb.Value += 0.0045248868778281;
            }

            db.Courses.AddRange(resultListCourses);
            db.SaveChanges();

            var persons = db.Person.ToList();

            var resultListPupils = new List<Pupils>();

            for (int i = 0; i < 3000; i++)
            {
                var drIndex = Convert.ToInt32(rnd.Next(0, drList.Count() - 1));

                var pup = new Pupils
                {
                    SchoolId = drList[drIndex].DrivingSchoolId,
                    PersonId = persons[i].PersonId,
                    IsCashlessPayments = Convert.ToBoolean(rnd.Next(0, 1))
                };

                resultListPupils.Add(pup);
                //mw.progAutoFillDb.Value += 0.0045248868778281;
            }

            db.Pupils.AddRange(resultListPupils);
            db.SaveChanges();

            var resultListCoWorkers = new List<CoWorkers>();

            for (int i = 3000; i < 5000; i++)
            {
                var drIndex = Convert.ToInt32(rnd.Next(0, drList.Count() - 1));

                var worker = new CoWorkers
                {
                    SchoolId = drList[drIndex].DrivingSchoolId,
                    PersonId = persons[i].PersonId,
                };

                resultListCoWorkers.Add(worker);
               // mw.progAutoFillDb.Value += 0.0045248868778281;
            }

            db.CoWorkers.AddRange(resultListCoWorkers);
            db.SaveChanges();


            var cars = db.Cars.ToList();
            var coWorkers = db.CoWorkers.ToList();

            var resultListDrivingLessons = new List<DrivingLessons>();

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

                resultListDrivingLessons.Add(les);
              //  mw.progAutoFillDb.Value += 0.0045248868778281;
            }

            db.DrivingLessons.AddRange(resultListDrivingLessons);
            db.SaveChanges();


            var courseList = db.Courses.ToList();
            var categories = db.CategoriesOfDriving.ToList();


            var resultListCourse_CategotryOfDriving = new List<Course_CategotryOfDriving>();

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

                resultListCourse_CategotryOfDriving.Add(courseCateg);
                //mw.progAutoFillDb.Value += 0.0045248868778281;
            }

            db.Course_CategotryOfDriving.AddRange(resultListCourse_CategotryOfDriving);
            db.SaveChanges();

            MessageBox.Show("Готово!");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            AutoFillDb();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            clearDB();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }
    }
}
