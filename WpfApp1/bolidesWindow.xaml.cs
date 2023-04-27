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
    public class bolidesGrid
    {
        public string id { get; set; }
        public string nameS { get; set; }
        
        public string nameE { get; set; }
        public string height { get; set; }
        public string weight { get; set; }
        public string RPM { get; set; }
        public string capacity { get; set; }
        public string cylinder { get; set; }
        public string width { get; set; }
        public string name { get; set; }
        public string radius { get; set; }
        public string nameW { get; set; }
    
        public BitmapFrame photo { get; set; }
        public byte[] photoB { get; set; }


    }
    public partial class bolidesWindow : Window
    {
        
       
        byte[] imageBolide = null;
        List<bolidesGrid> bolidesListD = new List<bolidesGrid>();
        MySqlConnection cn = new MySqlConnection("server = 10.0.7.240; user id = user8; port=3306;persistsecurityinfo=True;database=user8;password=wsruser8");
        public bolidesWindow()
        {
            InitializeComponent();
            load();
            load1();
        }
        void load1()
        {
            cn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT name FROM wheels;", cn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            foreach (DataRow item in dt.Rows)
            {
                wheelB.Items.Add(item[0].ToString());
            }
            MySqlCommand cmd1 = new MySqlCommand("SELECT name FROM engine;", cn);
            DataTable dt1 = new DataTable();
            dt1.Load(cmd1.ExecuteReader());
            foreach (DataRow item in dt1.Rows)
            {
                engineB.Items.Add(item[0].ToString());
            }
            MySqlCommand cmd2 = new MySqlCommand("SELECT name FROM chassis;", cn);
            DataTable dt2 = new DataTable();
            dt2.Load(cmd2.ExecuteReader());
            foreach (DataRow item in dt2.Rows)
            {
                chas.Items.Add(item[0].ToString());
            }
            cn.Close();

        }

        void load()
        {
            bolidesListD.Clear();
            cn.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT capacity,cylinder,radius,RPM, wheels.name wn,photos.photo pp,chassis.name cn,engine.name en,bolides.id,idChassis,idWheel,idEngine," +
                "weight,width,bolides.name bn,height  FROM bolides LEFT JOIN wheels ON wheels.id =idWheel LEFT JOIN photos ON photos.id =bolides.photo LEFT JOIN chassis ON chassis.id =idChassis LEFT JOIN engine ON engine.id =idEngine where bolides.status !='Удален'", cn);
            DataTable dtb = new DataTable();
            dtb.Load(cmd1.ExecuteReader());             
            cn.Close();
            foreach (DataRow item in dtb.Rows)
            {
                byte[] imgData = (byte[])item["pp"];
                MemoryStream ms = new MemoryStream(imgData);
                bolidesListD.Add(new bolidesGrid()
                {
                    photo = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad),
                    id = item["id"].ToString(),
                    name = item["bn"].ToString(),

                    nameS = item["cn"].ToString(),
                    nameW = item["wn"].ToString(),
                    nameE = item["en"].ToString(),
                    height = item["height"].ToString(),
                    weight = item["weight"].ToString(),
                    width = item["width"].ToString(),
                    RPM = item["RPM"].ToString(),
                    capacity = item["capacity"].ToString(),
                    photoB = imgData,
                    cylinder = item["cylinder"].ToString(),
                    radius = item["radius"].ToString(),

                });
            }
            bolidesList.ItemsSource = null;
            bolidesList.Items.Clear();
            bolidesList.ItemsSource = bolidesListD;
        }
        private void clickAdd(object sender, RoutedEventArgs e)
        {
            if (engineB.Text.Trim() != "" && wheelB.Text != "" && weightB.Text.Trim() != "" && widthB.Text.Trim() != ""  && image.Source != null)
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
        private void bolidesDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(bolidesList.SelectedItems.Count != 0)
            {
                dynamic selectedItem = bolidesList.SelectedItem;
                widthB.Text = selectedItem.width;
                weightB.Text = selectedItem.weight;  
                chas.Text = selectedItem.nameS;
                wheelB.Text = selectedItem.nameW;
                engineB.Text = selectedItem.nameE;
                image.Source = selectedItem.photo;
                imageBolide = selectedItem.photoB;
                idCheck.Text = selectedItem.id;
                nameB.Text = selectedItem.name;
            }           
        }
        void check(string a)
        {

            if (a == "update")
            {
                cn.Open();
                MySqlCommand cmd3 = new MySqlCommand("select `name` from bolides where name = @name and id != '" + idCheck.Text.ToString() + "'", cn);
                cmd3.Parameters.Add(new MySqlParameter("@name", nameB.Text));
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
                MySqlCommand cmd3 = new MySqlCommand("select `name` from bolides where name = @name" , cn);
                cmd3.Parameters.Add(new MySqlParameter("@name", nameB.Text));
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
           

            cn.Open();
            /*MySqlCommand cmd4 = new MySqlCommand("SELECT id FROM chassis where name='" + chas.Text.ToString() + "';", cn);
            DataTable dt4 = new DataTable();

            dt4.Load(cmd4.ExecuteReader());
            if (dt4.Rows.Count != 0)
            {
                chasId = dt4.Rows[0][0].ToString();

            }
            MySqlCommand cmd8 = new MySqlCommand("SELECT id FROM engine where name='" + engineB.Text.ToString() + "';", cn);
            DataTable dt8 = new DataTable();

            dt8.Load(cmd8.ExecuteReader());
            if (dt8.Rows.Count != 0)
            {
               engId = dt8.Rows[0][0].ToString();

            }
            MySqlCommand cmd9 = new MySqlCommand("SELECT id FROM wheels where name='" + wheelB.Text.ToString() + "';", cn);
            DataTable dt9 = new DataTable();

            dt9.Load(cmd9.ExecuteReader());
            if (dt9.Rows.Count != 0)
            {
                wheelId = dt9.Rows[0][0].ToString();

            }*/

            MySqlCommand cmd2 = new MySqlCommand("select `id`, `photo` from photos where photo = @img;", cn);
            cmd2.Parameters.Add(new MySqlParameter("@img", imageBolide));
            DataTable dtb = new DataTable();
            dtb.Load(cmd2.ExecuteReader());
            var idPhoto = dtb.Rows;
            cn.Close();
            if (idPhoto.Count == 0)
            {

                cn.Open();
                MySqlCommand cmd1 = new MySqlCommand("INSERT INTO photos (`id`, `photo`) VALUES (null, @img);", cn);
                cmd1.Parameters.Add(new MySqlParameter("@img", imageBolide));
                cmd1.ExecuteReader();
                cn.Close();
                check(a);

            }
            else if (a == "add" )
            {

                cn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `bolides` (`idChassis`,`idEngine`, `idWheel`, `weight`, `width`, `name`,`photo`,`status`) VALUES ((SELECT id FROM chassis where name = '" + chas.Text.ToString() + "'),(SELECT id FROM engine where name = '" + engineB.Text.ToString() + "'), (SELECT id FROM wheels where name='" + wheelB.Text.ToString() + "'), '" + weightB.Text + "', " +
                    "'" + widthB.Text + "','" + nameB.Text + "','" + idPhoto[0][0].ToString() + "', 'Работает');", cn);
                cmd.ExecuteReader();
                MessageBox.Show("Данные обновлены");
              
            }
            else if (a == "update" )
            {
                
                cn.Open();
                MySqlCommand cmd5 = new MySqlCommand("update `bolides` set `idChassis`=(SELECT id FROM chassis where name = '" + chas.Text.ToString() + "'),`idEngine`=(SELECT id FROM engine where name = '" + engineB.Text.ToString() + "'), `idWheel`=(SELECT id FROM wheels where name='" + wheelB.Text.ToString() + "'), `weight`= '" +
                    weightB.Text + "', `width`='" + widthB.Text + "',`name`= '" + nameB.Text + "',`photo`= '" + idPhoto[0][0].ToString() + "' where id = '" + idCheck.Text + "'", cn);
                
                cmd5.ExecuteReader();

                MessageBox.Show("Данные обновлены");

            }
            else if (a == "delete")
            {

                cn.Open();
                MySqlCommand cmd5 = new MySqlCommand("update bolides set status = 'Удален' where id = '" + idCheck.Text + "'", cn);
                
                cmd5.ExecuteReader();
                idCheck.Text = "";
                MessageBox.Show("Данные обновлены");

            }

        }

        private void clickDel(object sender, RoutedEventArgs e)
        {
            if (idCheck.Text.Trim() != "" && engineB.Text.Trim() != "" && nameB.Text != "" && wheelB.Text != "" && weightB.Text.Trim() != "" && widthB.Text.Trim() != "" && image.Source != null)
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
            if (idCheck.Text.Trim() != "" && engineB.Text.Trim() != "" && nameB.Text != "" && wheelB.Text != "" && weightB.Text.Trim() != "" && widthB.Text.Trim() != "" && engineB.Text.Trim() != ""  && image.Source != null)
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
            MainWindow fm = new MainWindow(1);
            fm.Show();
            Close();


        }
        private void charInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^.*\\s*[^A-z0-9].*$");
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
                imageBolide = BR.ReadBytes((int)FStream.Length);

            }

        }

        private void chassisClick(object sender, RoutedEventArgs e)
        {
            chassisWindow fm = new chassisWindow();
            fm.Show();
            Close();

        }

        private void wheelsClick(object sender, RoutedEventArgs e)
        {
            wheelWindow fm = new wheelWindow();
            fm.Show();
            Close();

        }

        private void enginesClick(object sender, RoutedEventArgs e)
        {
            engineWindow fm = new engineWindow();
            fm.Show();
            Close();

        }
    }
}
