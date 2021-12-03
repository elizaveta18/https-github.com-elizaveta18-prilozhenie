using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
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
using Microsoft.Win32;

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
            System.Windows.Controls.Image IMG = sender as System.Windows.Controls.Image;
            int ind = Convert.ToInt32(IMG.Uid);
            users U = DB.Base.users.FirstOrDefault(x => x.id == ind);//запись о текущем пользователе
            usersimage UI = DB.Base.usersimage.FirstOrDefault(x => x.id_user == ind);//получаем запись о картинке для текущего пользователя
            BitmapImage BI = new BitmapImage();
            if (UI != null)//если для текущего пользователя существует запись о его катринке
            {
                if (UI.path != null)//если присутствует путь к картинке
                {
                    BI = new BitmapImage(new Uri(UI.path, UriKind.Relative));
                }
                else//если присутствуют двоичные данные
                {
                    BI.BeginInit();//начать инициализацию BitmapImage (для помещения данных из какого-либо потока)
                    BI.StreamSource = new MemoryStream(UI.image);//помещаем в источник данных двоичные данные из потока
                    BI.EndInit();//закончить инициализацию
                }
            }
            else//если в базе не содержится картинки, то ставим заглушку
            {
                switch (U.gender)//в зависимости от пола пользователя устанавливаем ту или иную картинку
                {
                    case 1:
                        BI = new BitmapImage(new Uri(@"/Image/maik.jpg", UriKind.Relative));
                        break;
                    case 2:
                        BI = new BitmapImage(new Uri(@"/Image/deva.jpg", UriKind.Relative));
                        break;
                    default:
                        BI = new BitmapImage(new Uri(@"/Image/cheba.jpg", UriKind.Relative));
                        break;
                }
            }
            IMG.Source = BI;//помещаем картинку в image
        }
        private void BtmAddImage_Click(object sender, RoutedEventArgs e)
        {
            Button BTN = (Button)sender;
            int ind = Convert.ToInt32(BTN.Uid);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".jpg"; // задаем расширение по умолчанию
            openFileDialog.Filter = "Изображения |*.jpg;*.png"; // задаем фильтр на форматы файлов
            var result = openFileDialog.ShowDialog();
            if (result == true)//если файл выбран
            {
                System.Drawing.Image UserImage = System.Drawing.Image.FromFile(openFileDialog.FileName);//создаем изображение
                ImageConverter IC = new ImageConverter();//конвертер изображения в массив байт
                byte[] ByteArr = (byte[])IC.ConvertTo(UserImage, typeof(byte[]));//непосредственно конвертация
                usersimage UI = new usersimage() { id_user = ind, image = ByteArr, avatar = false };//создаем новый объект usersimage
                DB.Base.usersimage.Add(UI);//добавляем его в модель
                DB.Base.SaveChanges();//синхронизируем с базой
                MessageBox.Show("Картинка пользователя добавлена в базу");
            }
            else
            {
                MessageBox.Show("Операция выбора изображения отменена");
            }
        }
        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            RadioButton RB = (RadioButton)sender;
            switch (RB.Uid)
            {
                case "name":
                    l1 = l1.OrderBy(x => x.name).ToList();
                    break;
                case "DR":
                    l1 = l1.OrderBy(x => x.dr).ToList();
                    break;
            }
            if (RBReverse.IsChecked == true) l1.Reverse();
            lbUsersList.ItemsSource = l1;
        }
        private void PrAddImage_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}