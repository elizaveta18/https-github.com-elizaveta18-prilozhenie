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
                auth CurrentUser = BaseConnect.BaseModel.auth.FirstOrDefault(x => x.login == txtLogin.Text && x.password == txtPassword.Password);
                if (CurrentUser != null)
                {//сюда напишем алгоритм перехода на страницу в зависимости от роли
                    switch (CurrentUser.role)
                    {
                        case 1:
                            MessageBox.Show("Вы вошли как администратор");
                            LoadPages.MainFrame.Navigate(new Page3());
                            break;
                        case 2:
                        default:
                            MessageBox.Show("Вы вошли как обычный пользователь");
                            LoadPages.MainFrame.Navigate(new Page1());
                            break;
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
            LoadPages.MainFrame.Navigate(new Page1());
        }

        private void txtLogin_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
