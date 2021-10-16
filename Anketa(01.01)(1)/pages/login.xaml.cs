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
using Anketa_01._01__1_.pages;

namespace Anketa_01._01__1_.pages
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                auth currentUser = DB.Base.auth.FirstOrDefault(x => x.login == txtLogin.Text && x.password == txtPassword.Password);
                if (currentUser != null)
                {
                    MessageBox.Show($"Добро пожаловать, {currentUser.login}!");
                    DB.currentUser = currentUser;
                    if (DB.currentUser.role == 1)
                    {
                        User.frmMain.Navigate(new Page4());
                    }
                    else if (DB.currentUser.role == 2)
                    {
                        User.frmMain.Navigate(new Page3());
                    }
                }
                else
                {
                    MessageBox.Show("Такого пользователя нет");
                }
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            User.frmMain.Navigate(new Page1());
        }
    }
}
