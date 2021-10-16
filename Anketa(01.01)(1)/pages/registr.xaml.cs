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
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
            listGenders.ItemsSource = DB.Base.genders.ToList();
            listGenders.SelectedValuePath = "id";
            listGenders.DisplayMemberPath = "gender";
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            User.frmMain.GoBack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //записываем данные в таблицу auth
            auth logPass = new auth() { login = txtLogin.Text, password = txtPass.Password, role = 2 };//создать новую запись
            DB.Base.auth.Add(logPass);//добавить в модель
            DB.Base.SaveChanges();//сонхронизировать с сервером
            //создаем запись в таблице Users, соответствующую данной
            users User = new users() { name = nameTextBox.Text, id = logPass.id, gender = (int)listGenders.SelectedValue, dr = (DateTime)dateBirth.SelectedDate };
            DB.Base.users.Add(User);
            foreach (traits t in lbTarits.SelectedItems)
            {
                users_to_traits UTT = new users_to_traits();
                UTT.id_user = User.id;
                UTT.id_trait = t.id;
                DB.Base.users_to_traits.Add(UTT);
            }
            DB.Base.SaveChanges();
            MessageBox.Show("Данные записаны успешно");//обратная связь с пользователем
        }
    }
}
