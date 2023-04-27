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
    public class pilotsGrid
    {
        public string id { get; set; }
        public string number { get; set; }
        public string team { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string patronymic { get; set; }
        public string gender { get; set; }
        public string growth { get; set; }
        public string weight { get; set; }
        public string debut { get; set; }
        public string dateBirth { get; set; }
        public BitmapFrame photo { get; set; }
        public byte[] photoB { get; set; }


    }
    public partial class pilotsWindow : Window
    {
        byte[] imagePilot = null;
       
        List<pilotsGrid> pilotsListS = new List<pilotsGrid>();
        MySqlConnection cn = new MySqlConnection("server = 10.0.7.240; user id = user8; port=3306;persistsecurityinfo=True;database=user8;password=wsruser8");
        void load1()
        {
            cn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT debut FROM debut;", cn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            foreach (DataRow item in dt.Rows)
            {
                debutP.Items.Add(item[0].ToString());
            }
            MySqlCommand cmd1 = new MySqlCommand("SELECT gender FROM gender;", cn);
            DataTable dt1 = new DataTable();
            dt1.Load(cmd1.ExecuteReader());
            foreach (DataRow item in dt1.Rows)
            {
                genderP.Items.Add(item[0].ToString());
            }
            MySqlCommand cmd2 = new MySqlCommand("SELECT name FROM team where status != 'Удален';", cn);
            DataTable dt2 = new DataTable();
            dt2.Load(cmd2.ExecuteReader());
            foreach (DataRow item in dt2.Rows)
            {
                teamP.Items.Add(item[0].ToString());
            }
            cn.Close();

        }
       
        void load()
        {
            pilotsListS.Clear();
            cn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT pilots.id,name,number,firstName,lastName,patronymic,gender,growth,weight,debut,dateBirth,photos.photo,idTeam FROM pilots LEFT JOIN photos ON pilots.photo = photos.id LEFT JOIN team ON team.id = idTeam where pilots.status != 'Удален'", cn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            cn.Close();
            foreach (DataRow item in dt.Rows)
            {
                byte[] imgData = (byte[])item["photo"];
                MemoryStream ms = new MemoryStream(imgData);
                pilotsListS.Add(new pilotsGrid()
                {
                    photo = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad),
                    id = item["id"].ToString(),
                    firstName = item["firstName"].ToString(),
                    lastName = item["lastName"].ToString(),
                    patronymic = item["patronymic"].ToString(),
                    gender = item["gender"].ToString(),
                    growth = item["growth"].ToString(),
                    weight = item["weight"].ToString(),
                    debut = item["debut"].ToString(),
                    dateBirth = Convert.ToDateTime(item["dateBirth"]).Date.ToString("dd.MM.yyyy"),
                    photoB = imgData,
                    team = item["name"].ToString(),
                    number = item["number"].ToString(),

                });
            }
            pilotsList.ItemsSource = null;
            pilotsList.Items.Clear();
            pilotsList.ItemsSource = pilotsListS;
        }
        public pilotsWindow()
        {
            InitializeComponent();
            load();
            
            load1();
        }

        private void pilotsDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (pilotsList.SelectedItems.Count != 0)
            {
                dynamic selectedItem = pilotsList.SelectedItem;
                idP.Text = selectedItem.number;
                nameL.Text = selectedItem.lastName;
                nameF.Text = selectedItem.firstName;
                patr.Text = selectedItem.patronymic;
                grow.Text = selectedItem.growth;
                weightP.Text = selectedItem.weight;
                date.Text = selectedItem.dateBirth;
                genderP.Text = selectedItem.gender;
                debutP.Text = selectedItem.debut;
                image.Source = selectedItem.photo;
                imagePilot = selectedItem.photoB;
                teamP.Text = selectedItem.team;
                idCheck.Text = selectedItem.id;
            }
        }
       

        private void imageClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fm = new OpenFileDialog();
            
            fm.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG;";
            fm.ShowDialog();
            if(fm.FileName != "")
            {
                var bitimage = new BitmapImage(new Uri(fm.FileName));
                image.Source = bitimage;
                FileStream FStream = new FileStream(fm.FileName, FileMode.Open, FileAccess.Read);
                BinaryReader BR = new BinaryReader(FStream);
                imagePilot = BR.ReadBytes((int)FStream.Length);

            }

        }

        private void backButton(object sender, RoutedEventArgs e)
        {
            MainWindow fm = new MainWindow(1);
            fm.Show();
            this.Close();
        }
        
        void check(string a)
        {
            if(a == "update")
            {  
                cn.Open();
                MySqlCommand cmd3 = new MySqlCommand("select `number` from pilots where number = @id and id != '" + idCheck.Text.ToString() + "'", cn);
                cmd3.Parameters.Add(new MySqlParameter("@id", idP.Text.Trim()));
                DataTable dtb1 = new DataTable();
                dtb1.Load(cmd3.ExecuteReader());
                var idPil = dtb1.Rows;
                if (idPil.Count != 0)
                {
                    MessageBox.Show("Нельзя добавить пилота с одинаковым номером");
                    return;
                }
                cn.Close();
                

            }
            if (a == "add")
            {
                cn.Open();
                MySqlCommand cmd3 = new MySqlCommand("select `number` from pilots where number = @id", cn);
                cmd3.Parameters.Add(new MySqlParameter("@id", idP.Text.Trim()));
                DataTable dtb1 = new DataTable();
                dtb1.Load(cmd3.ExecuteReader());
                var idPil = dtb1.Rows;
                if (idPil.Count != 0)
                {
                    MessageBox.Show("Нельзя добавить пилота с одинаковым номером");
                    return;
                }
                cn.Close();


            }

            cn.Open();
           
        
            
            MySqlCommand cmd2 = new MySqlCommand("select `id`, `photo` from photos where photo = @img;", cn);
            cmd2.Parameters.Add(new MySqlParameter("@img", imagePilot));
            DataTable dtb = new DataTable();
            dtb.Load(cmd2.ExecuteReader());
            var idPhoto = dtb.Rows;
            cn.Close();
            if (idPhoto.Count == 0)
            {
                
                cn.Open();
                MySqlCommand cmd1 = new MySqlCommand("INSERT INTO photos (`id`, `photo`) VALUES (null, @img);", cn);
                cmd1.Parameters.Add(new MySqlParameter("@img", imagePilot));
                cmd1.ExecuteReader();
                cn.Close();
                check(a);

            }
            else if (a == "add" )
            {

                cn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `pilots` (`number`,`idTeam`, `firstName`, `lastName`, `patronymic`,`gender`, `growth`, `weight`,`debut`,`dateBirth`,`photo`,`status`) VALUES ('" + idP.Text + "', (SELECT id FROM team where name='" + teamP.Text.ToString() + "'), '" + nameF.Text + "', '" + nameL.Text + "', " +
                    "'" + patr.Text + "','" + genderP.Text + "', '" + grow.Text + "', '" + weightP.Text + "', '" + debutP.Text.Replace(@"'", @"\'") + "', '" + date.SelectedDate.Value.ToString("yyyy-MM-dd HH.mm.ss") + "', '" + idPhoto[0][0].ToString() + "','Работает');", cn);
                cmd.ExecuteReader();
                cn.Close();
                cn.Open();
                MySqlCommand cmd21 = new MySqlCommand("INSERT INTO `champpil` (`idPilot`,`idTeam`,`win`,`poll`, `points`, `laps`) values ((SELECT id FROM pilots where number='" + idP.Text + "' and firstName = '" + nameF.Text + "'),(SELECT id FROM team where name='" + teamP.Text.ToString() + "'),0,0,0,0)", cn);
                cmd21.ExecuteReader();
                MessageBox.Show("Данные обновлены");
                
            }
            else if (a == "update")
            {
                
                cn.Open();
                MySqlCommand cmd5 = new MySqlCommand("update `pilots` set `number`='" + idP.Text + "',`idTeam`=(SELECT id FROM team where name='" + teamP.Text.ToString() + "'), `firstName`='" + nameF.Text + "', `lastName`= '" + nameL.Text + "', `patronymic`='" + patr.Text + "',`gender`= '" + genderP.Text + "', `growth`='" + grow.Text +
                    "', `weight`='" + weightP.Text +"',`debut`= '" + debutP.Text.Replace(@"'", @"\'") + "',`dateBirth`= '" + date.SelectedDate.Value.ToString("yyyy-MM-dd HH.mm.ss") + "',`photo`= '" + idPhoto[0][0].ToString() + "' where id = '" + idCheck.Text + "'", cn);
                
                cmd5.ExecuteReader();

                MessageBox.Show("Данные обновлены");

            }
            else if (a == "delete" )
            {
                
                cn.Open();
                MySqlCommand cmd5 = new MySqlCommand("update pilots set status = 'Удален' where id = '" + idCheck.Text + "'", cn);
                
                cmd5.ExecuteReader();
                idCheck.Text = "";
                MessageBox.Show("Данные обновлены");

            }

        }

        private void clickAdd(object sender, RoutedEventArgs e)
        {
            if (nameF.Text.Trim() != "" && date.Text != "" && idP.Text.Trim() != ""  && nameL.Text.Trim() != "" && genderP.Text.Trim() != ""  && image.Source != null)
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

        private void clickDel(object sender, RoutedEventArgs e)
        {
            if (idCheck.Text.Trim() != "" && nameF.Text.Trim() != "" && date.Text != "" && idP.Text.Trim() != "" &&  nameL.Text.Trim() != "" && genderP.Text.Trim() != ""  && image.Source != null)
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
            if (idCheck.Text.Trim() != "" && nameF.Text.Trim() != "" && date.Text != "" && idP.Text.Trim() != ""  && nameL.Text.Trim() != "" && genderP.Text.Trim() != ""  && image.Source != null)
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

        private void charInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^.*\\s*[^А-яЁё].*$");
            e.Handled = regex.IsMatch(e.Text);

        }
        private void intInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^.*\\s*[^0-9].*$");
            e.Handled = regex.IsMatch(e.Text);

        }

        

       
    }
}
