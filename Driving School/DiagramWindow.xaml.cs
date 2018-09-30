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
using Driving_School.Models;
using LiveCharts;
using LiveCharts.Wpf;

namespace Driving_School
{
    /// <summary>
    /// Interaction logic for DiagramWindow.xaml
    /// </summary>
    public partial class DiagramWindow : Window
    {
        private DbContext_Prog db = new DbContext_Prog(ConfigurationManager.ConnectionStrings["PCString"].ConnectionString);

        public SeriesCollection SeriesChart1 { get; set; }
        public List<string> Labels1 { get; set; }
        public Func<double, string> Formatter1 { get; set; }

        public DiagramWindow()
        {
            InitializeComponent();

            SeriesChart1 = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = new ChartValues<double>()
                }
            };

            Dictionary<int, int> countPupilsInAutoschools = new Dictionary<int, int>();

            foreach (var sch in db.DrivingSchools.ToList())
            {
                countPupilsInAutoschools.Add(sch.DrivingSchoolId, db.Pupils.Where(el => el.SchoolId == sch.DrivingSchoolId).Count());
            }
            Labels1 = new List<string>();

            foreach (var elem in countPupilsInAutoschools.OrderBy(el => el.Key))
            {
                Labels1.Add(db.DrivingSchools.Where(ds => ds.DrivingSchoolId == elem.Key).First().DrivingSchoolName);

                SeriesChart1[0].Values.Add(Convert.ToDouble(elem.Value));
            }

            Formatter1 = value => value.ToString("N");

            PieChart.Series = new SeriesCollection();

            var pupilsList = db.Pupils.ToList();
            var ages = new Dictionary<int, int>();
            foreach (var pupil in pupilsList)
            {
                var person = db.Person.Where(el => el.PersonId == pupil.PersonId).FirstOrDefault();
                int age;
                try
                {
                    age = DateTime.Today.Year - person.DateOfBirth.Value.Year;
                }
                catch
                {
                    continue;
                }

                if (ages.ContainsKey(age))
                {
                    ages[age]++;
                }
                else
                {
                    ages[age] = 1;
                }
            }
            PointLabel = chartPoint =>
    string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            foreach (var elem in ages)
            {
                PieChart.Series.Add(new PieSeries { Title = elem.Key.ToString(), Values = new ChartValues<int> { elem.Value }, DataLabels = true, LabelPoint = PointLabel });
            }

            DataContext = this;
        }

        public Func<ChartPoint, string> PointLabel { get; set; }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
    }
}