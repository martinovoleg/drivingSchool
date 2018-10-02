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
                        AutoFill mw = new AutoFill();
                        mw.Show();
                        Close();
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

       
       


        
    }
}