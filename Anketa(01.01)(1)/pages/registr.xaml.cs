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
        private auth SelectedUser;
        public Page1(auth SelectedUSer)
        {
            InitializeComponent();
            listGenders.ItemsSource = DB.Base.genders.ToList();
            listGenders.SelectedValuePath = "id";
            listGenders.DisplayMemberPath = "gender";
            this.SelectedUser = SelectedUSer;

            txtLogin.Text = SelectedUser.login;
            nameTextBox.Text = SelectedUser.users.name;
            dateBirth.SelectedDate = SelectedUser.users.dr;
            listGenders.SelectedIndex = SelectedUser.users.gender - 1;
            List<users_to_traits> trs = SelectedUser.users.users_to_traits.ToList();
            foreach (users_to_traits tr in trs)
            {
                string trName = DB.Base.traits.FirstOrDefault(x => x.id == tr.id_trait).trait;
                if ((string)goodCB.Content == trName)
                {
                    goodCB.IsChecked = true;
                }
                if ((string)nejnCB.Content == trName)
                {
                    nejnCB.IsChecked = true;
                }
                if ((string)laskovCB.Content == trName)
                {
                    laskovCB.IsChecked = true;
                }

            }
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
            users data = new users();
            data.id = logPass.id;
            data.name = nameTextBox.Text;
            data.dr = dateBirth.SelectedDate.Value;
            data.gender = (int)listGenders.SelectedValue;
            DB.Base.users.Add(data);
            if (goodCB.IsChecked == true)
            {
                users_to_traits tr = new users_to_traits();
                tr.id_user = logPass.id;
                tr.id_trait = DB.Base.traits.FirstOrDefault(x => x.trait == goodCB.Content.ToString()).id;
                DB.Base.users_to_traits.Add(tr);
            }
            if (nejnCB.IsChecked == true)
            {
                users_to_traits tr = new users_to_traits();
                tr.id_user = logPass.id;
                tr.id_trait = DB.Base.traits.FirstOrDefault(x => x.trait == nejnCB.Content.ToString()).id;
                DB.Base.users_to_traits.Add(tr);
            }
            if (laskovCB.IsChecked == true)
            {
                users_to_traits tr = new users_to_traits();
                tr.id_user = logPass.id;
                tr.id_trait = DB.Base.traits.FirstOrDefault(x => x.trait == laskovCB.Content.ToString()).id;
                DB.Base.users_to_traits.Add(tr);
            }                      
            DB.Base.SaveChanges();
            MessageBox.Show("Данные записаны успешно");//обратная связь с пользователем
        }
    }
}
