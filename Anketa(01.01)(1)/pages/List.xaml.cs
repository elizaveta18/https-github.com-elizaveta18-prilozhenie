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
using ClassLibrary1;

namespace Anketa_01._01__1_
{
    /// <summary>
    /// Логика взаимодействия для List.xaml
    /// </summary>
    public partial class List : Page
    {
        public List<auth> Anketa_01;
        List<users> users;
        List<users> l1;
        PageChange pc = new PageChange();
        public List()
        {
            InitializeComponent();
            users = DB.Base.users.ToList();
            lbUsersList.ItemsSource = users;
            lbGenderFilter.ItemsSource = DB.Base.genders.ToList();
            lbGenderFilter.SelectedValuePath = "id";
            lbGenderFilter.DisplayMemberPath = "gender";
            l1 = users;
            DataContext = pc;
        }      
        private void lbTraits_Loaded(object sender, RoutedEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            int index = Convert.ToInt32(lb.Uid);
            lb.ItemsSource = DB.Base.users_to_traits.Where(x => x.id_user == index).ToList();
            lb.DisplayMemberPath = "traits.trait";
        }       
        private void Filter(object sender, RoutedEventArgs e)
        {
            //по строкам
            try
            {
                int start = Convert.ToInt32(txtOT.Text) - 1;
                int finish = Convert.ToInt32(txtDO.Text);
                l1 = users.Skip(start).Take(finish - start).ToList();
            }
            catch
            {
                //null
            }
            //по полу
            try
            {
                if (lbGenderFilter.SelectedIndex != -1)
                    l1 = l1.Where(x => x.gender == Convert.ToInt32(lbGenderFilter.SelectedValue)).ToList();
            }
            catch
            {
                //null
            }
            //по имени
            try
            {
                if (txtNameFilter.Text != "")
                {
                    l1 = l1.Where(x => x.name.Contains(txtNameFilter.Text)).ToList();
                }
            }
            catch
            {
                //null
            }
            lbUsersList.ItemsSource = l1;
            pc.Countlist = l1.Count;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            lbUsersList.ItemsSource = users;//в качестве источника данных исходный список
            l1 = users;
            lbGenderFilter.SelectedIndex = -1; //сбрасываем выбранный элемент списка
            txtNameFilter.Text = "";//сбрасываем фильтр на строку
            txtOT.Text = "";
            txtDO.Text = "";
        }

        private void btnNaz_Click(object sender, RoutedEventArgs e)
        {
            User.frmMain.GoBack();
        }
        private void Udal_Click(object sender, RoutedEventArgs e)
        { 
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить пользователя из системы?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            users Udal = (users)lbUsersList.SelectedItem;
            auth SelectedUser = DB.Base.auth.FirstOrDefault(x => x.id == Udal.id);
            DB.Base.auth.Remove(SelectedUser);
            DB.Base.SaveChanges();
            MessageBox.Show("Выбранный пользователь удален");
            TimeSpan.FromSeconds(3);
            lbUsersList.ItemsSource = DB.Base.users.ToList();
        }
        private void red_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            int id = Convert.ToInt32(b.Uid);
            auth tUser = DB.Base.auth.FirstOrDefault(x => x.id == id);
            User.frmMain.Navigate(new Page5(tUser));
            lbUsersList.ItemsSource = DB.Base.users.ToList();
        }
        private void tbStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
        private void GoPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;//определяем, какой текстовый блок был нажат           
            //изменение номера страници при нажатии на кнопку
            switch (tb.Uid)
            {
                case "prev":
                    pc.CurrentPage--;
                    break;
                case "next":
                    pc.CurrentPage++;
                    break;
                default:
                    pc.CurrentPage = Convert.ToInt32(tb.Text);
                    break;
            }
            //определение списка
            lbUsersList.ItemsSource = l1.Skip(pc.CurrentPage * pc.CountPage - pc.CountPage).Take(pc.CountPage).ToList();
            txtCurentPage.Text = "Текущая страница: " + (pc.CurrentPage).ToString();
        }
        private void txtPageCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pc.CountPage = Convert.ToInt32(txtPageCount.Text);
            }
            catch
            {
                pc.CountPage = l1.Count;
            }
            pc.Countlist = users.Count;
            lbUsersList.ItemsSource = l1.Skip(0).Take(pc.CountPage).ToList();
        }

        private void btnVoz_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            DateTime[] mas = new DateTime[users.Count];
            foreach (users us in users)
            {
                mas[i] = us.dr;
                i++;
            }
            MessageBox.Show("Средний возраст: " + Func.AgeAVG(mas).ToString());
        }
        private void UserImage_Loaded(object sender, RoutedEventArgs e)
        {
            Image IMG = sender as Image;
            int ind = Convert.ToInt32(IMG.Uid);
            users U = DB.Base.users.FirstOrDefault(x => x.id == ind);
            BitmapImage BI;
            switch (U.gender)
            {
                case 1:
                    BI = new BitmapImage(new Uri(@"/Image/mal.jpg", UriKind.Relative));
                    break;
                case 2:
                    BI = new BitmapImage(new Uri(@"/Image/dev.jpg", UriKind.Relative));
                    break;
                default:
                    BI = new BitmapImage(new Uri(@"/Image/dr.jpg", UriKind.Relative));
                    break;
            }
            IMG.Source = BI;//помещаем картинку в image
        }
    }
}