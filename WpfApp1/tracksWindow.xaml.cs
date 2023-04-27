using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для tracksWindow.xaml
    /// </summary>
    
    public class tracksGrid
    {
        public string id { get; set; }
        public string name { get; set; }

        public string dist { get; set; }
        public string year { get; set; }
        public string length { get; set; }
        public string circles { get; set; }
        public string location { get; set; }
        public BitmapFrame photo { get; set; }
        public byte[] photoB { get; set; }


    }
    public partial class tracksWindow : Window
    {

       
        byte[] imageTrack = null;
        List<tracksGrid> tracksListD = new List<tracksGrid>();
        MySqlConnection cn = new MySqlConnection("server = 10.0.7.240; user id = user8; port=3306;persistsecurityinfo=True;database=user8;password=wsruser8");
        public tracksWindow()
        {
            InitializeComponent();
            load();
            load1();
        }
        void load1()
        {
            cn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT name FROM locations;", cn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            foreach (DataRow item in dt.Rows)
            {
                locationT.Items.Add(item[0].ToString());
            }
            
            cn.Close();

        }

        void load()
        {
            tracksListD.Clear();
            cn.Open();           
            MySqlCommand cmd1 = new MySqlCommand("SELECT tracks.id i,name,dist,year,length,circles,location,photos.photo pp FROM tracks left join photos on photos.id = tracks.photo where tracks.status !='Удален'", cn);
            DataTable dtb = new DataTable();
            dtb.Load(cmd1.ExecuteReader());
            cn.Close();
            foreach (DataRow item in dtb.Rows)
            {
                byte[] imgData = (byte[])item["pp"];
                MemoryStream ms = new MemoryStream(imgData);
                tracksListD.Add(new tracksGrid()
                {
                    photo = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad),
                    id = item["i"].ToString(),
                    name = item["name"].ToString(),
                    dist = item["dist"].ToString(),
                    year = item["year"].ToString(),
                    length = item["length"].ToString(),
                    circles = item["circles"].ToString(),
                    photoB = imgData,
                    location = item["location"].ToString()
                });
            }
            tracksList.ItemsSource = null;
            tracksList.Items.Clear();
            tracksList.ItemsSource = tracksListD;
        }
        private void clickAdd(object sender, RoutedEventArgs e)
        {
            if (nameT.Text!= "" && yearT.Text.Trim() != "" && distT.Text.Trim() != "" && circlesT.Text.Trim() != "" && lengthT.Text.Trim() != "" && image.Source != null)
            {
                try
                {
                    check("add");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка подключения к базе данных///" + ex);
                }
                finally
                {
                    cn.Close();
                    load();
                }
            }
        }
        private void tracksDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (tracksList.SelectedItems.Count != 0)
            {
                dynamic selectedItem = tracksList.SelectedItem;
                nameT.Text = selectedItem.name;
                distT.Text = selectedItem.dist;
                yearT.Text = selectedItem.year;
                lengthT.Text = selectedItem.length;
                circlesT.Text = selectedItem.circles;
                locationT.Text = selectedItem.location;
                image.Source = selectedItem.photo;
                imageTrack = selectedItem.photoB;
                idCheck.Text = selectedItem.id;                
            }
        }
        void check(string a)
        {
            if (a == "update")
            {
                cn.Open();
                MySqlCommand cmd3 = new MySqlCommand("select `name` from tracks where name = @name and id != '" + idCheck.Text.ToString() + "'", cn);
                cmd3.Parameters.Add(new MySqlParameter("@name", nameT.Text));
                DataTable dtb1 = new DataTable();
                dtb1.Load(cmd3.ExecuteReader());
                var nameBol = dtb1.Rows;
                if (nameBol.Count != 0)
                {
                    MessageBox.Show("Нельзя добавить болид с одинаковым названием");
                    return;
                }
                cn.Close();
            }
            if (a == "add")
            {
                cn.Open();
                MySqlCommand cmd3 = new MySqlCommand("select `name` from tracks where name = @name", cn);
                cmd3.Parameters.Add(new MySqlParameter("@name", nameT.Text));
                DataTable dtb1 = new DataTable();
                dtb1.Load(cmd3.ExecuteReader());
                var nameBol = dtb1.Rows;
                if (nameBol.Count != 0)
                {
                    MessageBox.Show("Нельзя добавить болид с одинаковым названием");
                    return;
                }
                cn.Close();


            }
            if(Convert.ToInt32(yearT.Text.Trim()) > 2100|| Convert.ToInt32(yearT.Text.Trim()) < 1910 || Convert.ToInt32(circlesT.Text.Trim()) >120 || Convert.ToInt32(circlesT.Text.Trim()) < 1)
            {
                MessageBox.Show("Год должен быть не больше 2100 и Не меньше 1910 а круги должны быть не выше 120 и не меньше 1");
                return;
            }


            cn.Open();
           

            MySqlCommand cmd2 = new MySqlCommand("select `id`, `photo` from photos where photo = @img;", cn);
            cmd2.Parameters.Add(new MySqlParameter("@img", imageTrack));
            DataTable dtb = new DataTable();
            dtb.Load(cmd2.ExecuteReader());
            var idPhoto = dtb.Rows;
            cn.Close();
            if (idPhoto.Count == 0)
            {

                cn.Open();
                MySqlCommand cmd1 = new MySqlCommand("INSERT INTO photos (`id`, `photo`) VALUES (null, @img);", cn);
                cmd1.Parameters.Add(new MySqlParameter("@img", imageTrack));
                cmd1.ExecuteReader();
                cn.Close();
                check(a);

            }
            else if (a == "add" )
            {

                cn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `tracks` (`name`,`dist`, `length`, `location`, `year`, `circles`,`photo`,`status`) VALUES ('" + nameT.Text.ToString() + "', '" + distT.Text.ToString() + "', '" + lengthT.Text.ToString() + "', '" + locationT.Text.ToString() + "', " +
                    "'" + yearT.Text.ToString() + "','" + circlesT.Text.ToString() + "','" + idPhoto[0][0].ToString() + "', 'Работает');", cn);
                cmd.ExecuteReader();
                MessageBox.Show("Данные обновлены");
                
            }
            else if (a == "update" )
            {

                cn.Open();
                MySqlCommand cmd5 = new MySqlCommand("update `tracks` set `name`='" + nameT.Text.ToString() + "',`dist`='" + distT.Text.ToString() + "', `length`='" + lengthT.Text.ToString() + "', `location`= '" +
                    locationT.Text.ToString() + "', `circles`='" + circlesT.Text.ToString() + "',`year`= '" + yearT.Text.ToString() + "',`photo`= '" + idPhoto[0][0].ToString() + "' where id = '" + idCheck.Text + "'", cn);
               
                cmd5.ExecuteReader();

                MessageBox.Show("Данные обновлены");

            }
            else if (a == "delete")
            {

                cn.Open();
                MySqlCommand cmd5 = new MySqlCommand("update tracks set status = 'Удален' where id = '" + idCheck.Text + "'", cn);

                cmd5.ExecuteReader();
                idCheck.Text = "";
                MessageBox.Show("Данные обновлены");

            }

        }

        private void clickDel(object sender, RoutedEventArgs e)
        {
            if (idCheck.Text.Trim() != "" && nameT.Text != "" && yearT.Text.Trim() != "" && distT.Text.Trim() != "" && circlesT.Text.Trim() != "" && lengthT.Text.Trim() != "" && image.Source != null)
            {
                try
                {
                    if (MessageBox.Show("Удалить строку безвозвратно", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        check("delete");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка подключения к базе данных///" + ex);
                }
                finally
                {
                    cn.Close();
                    load();
                }
            }
        }


        private void clickUpd(object sender, RoutedEventArgs e)
        {
            if (idCheck.Text.Trim() != "" && nameT.Text != "" && yearT.Text.Trim() != "" && distT.Text.Trim() != "" && circlesT.Text.Trim() != "" && lengthT.Text.Trim() != "" && image.Source != null)
            {
                try
                {
                    check("update");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка подключения к базе данных///" + ex);
                }
                finally
                {
                    cn.Close();
                    load();
                }
            }
        }

        private void backButton(object sender, RoutedEventArgs e)
        {
            MainWindow fm = new MainWindow(2);
            fm.Show();
            this.Close();


        }
        private void charInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^.*\\s*[^А-я0-9].*$");
            e.Handled = regex.IsMatch(e.Text);

        }
        private void intInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^.*\\s*[^0-9].*$");
            e.Handled = regex.IsMatch(e.Text);

        }
        private void imageClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fm = new OpenFileDialog();

            fm.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG;";
            fm.ShowDialog();
            if (fm.FileName != "")
            {
                var bitimage = new BitmapImage(new Uri(fm.FileName));
                image.Source = bitimage;
                FileStream FStream = new FileStream(fm.FileName, FileMode.Open, FileAccess.Read);
                BinaryReader BR = new BinaryReader(FStream);
                imageTrack = BR.ReadBytes((int)FStream.Length);

            }

        }

      
    }
}
