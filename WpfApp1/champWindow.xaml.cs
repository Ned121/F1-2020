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
    
   
    public class champGrid
    {
        public string id { get; set; }
        public string pilotTeam { get; set; }
        public string win { get; set; }
        public string poll { get; set; }
        public string points { get; set; }
        public string laps { get; set; }
        public BitmapFrame photo { get; set; }
        public string position { get; set; }
    }
    
    public partial class champWindow : Window
    {

        string ws = "";
        List<champGrid> champListd = new List<champGrid>();
        MySqlConnection cn = new MySqlConnection("server = 10.0.7.240; user id = user8; port=3306;persistsecurityinfo=True;database=user8;password=wsruser8");
        public champWindow(string w)
        {
            
            InitializeComponent();
            
            ws = w;
            nameFrom.Content = "Кубок " + w.ToLower();
            load();
            
        }

        
        void load()
        {
            champListd.Clear();
            cn.Open();
            if (ws == "Конструкторов")
            {            
                MySqlCommand cmd1 = new MySqlCommand("SELECT team.name tn,photos.photo pp  , idTeam,win,poll,points,laps FROM champconst LEFT JOIN team ON team.id =idTeam LEFT JOIN bolides ON bolides.id =team.idBolide LEFT JOIN photos ON photos.id =bolides.photo ORDER BY points DESC", cn);
                DataTable dtb = new DataTable();
                dtb.Load(cmd1.ExecuteReader());
                int i = 1;
                foreach (DataRow item in dtb.Rows)
                {
                    byte[] imgData = (byte[])item["pp"];
                    MemoryStream ms = new MemoryStream(imgData);
                    champListd.Add(new champGrid()
                    {
                        photo = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad),
                        pilotTeam = item["tn"].ToString(),
                        win = item["win"].ToString(),
                        poll = item["poll"].ToString(),
                        points = item["points"].ToString(),
                        laps = item["laps"].ToString(),
                        id = item["idTeam"].ToString(),
                        position = i.ToString()                       
                    });
                    i++;
                }
            }
            else if (ws == "Пилотов")
            {               
                MySqlCommand cmd1 = new MySqlCommand("SELECT photos.photo pp,number,firstName,idPilot,lastName,name,win,poll,points,laps FROM champpil LEFT JOIN pilots on pilots.id =idPilot LEFT JOIN team on team.id =champpil.idTeam LEFT JOIN photos on photos.id =pilots.photo ORDER BY points DESC", cn);
                DataTable dtb = new DataTable();
                dtb.Load(cmd1.ExecuteReader());
                int i = 1;
                foreach (DataRow item in dtb.Rows)
                {
                    byte[] imgData = (byte[])item["pp"];
                    MemoryStream ms = new MemoryStream(imgData);
                    champListd.Add(new champGrid()
                    {
                        photo = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad),
                        pilotTeam = item["number"].ToString() + " " + item["firstName"].ToString() + " " + item["lastName"].ToString() + " " + item["name"].ToString(),
                        win = item["win"].ToString(),
                        poll = item["poll"].ToString(),
                        points = item["points"].ToString(),
                        laps = item["laps"].ToString(),
                        id = item["idPilot"].ToString(),
                        position = i.ToString()


                    });
                    i++;
                }
            }                      
            cn.Close();         
            champList.ItemsSource = null;
            champList.Items.Clear();           
            champList.ItemsSource = champListd;            
        }
        void check()
        {
            if (ws == "Конструкторов")

            {
               cn.Open();
               MySqlCommand cmd2 = new MySqlCommand("update `champconst` set `win`='" + winC.Text.ToString() + "',`poll`='" + poulC.Text.ToString() + "',`points`='" + pointC.Text.ToString() + "',`laps`='" + lapsC.Text.ToString() + "' where idTeam = '" + idCheck.Text + "'", cn);
               cmd2.ExecuteReader();
               MessageBox.Show("Данные обновлены");      
            }
            else if (ws == "Пилотов")
            {
                cn.Open();
                MySqlCommand cmd2 = new MySqlCommand("update `champpil` set `win`='" + winC.Text.ToString() + "',`poll`='" + poulC.Text.ToString() + "',`points`='" + pointC.Text.ToString() + "',`laps`='" + lapsC.Text.ToString() + "' where idPilot = '" + idCheck.Text + "'", cn);
                cmd2.ExecuteReader();
                MessageBox.Show("Данные обновлены");

            }


            

        }
        private void champDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (champList.SelectedItems.Count != 0)
            {
                dynamic selectedItem = champList.SelectedItem;
                idCheck.Text = selectedItem.id;
                poulC.Text = selectedItem.poll;
                winC.Text = selectedItem.win;
                pointC.Text = selectedItem.points;
                lapsC.Text = selectedItem.laps;
                image.Source = selectedItem.photo;
            }
        }
       
       
        private void clickUpd(object sender, RoutedEventArgs e)
        {
            if (poulC.Text.Trim() != "" && winC.Text.Trim() != "" && lapsC.Text.Trim() != "" && pointC.Text.Trim() != "" && idCheck.Text != "")
            {
                try
                {
                    check();
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
            Regex regex = new Regex("^.*\\s*[^A-z0-9].*$");
            e.Handled = regex.IsMatch(e.Text);

        }
        private void intInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^.*\\s*[^0-9].*$");
            e.Handled = regex.IsMatch(e.Text);

        }
        private void backButton(object sender, RoutedEventArgs e)
        {
            MainWindow fm = new MainWindow(2);
            fm.Show();
            Close();


        }
    }
}
