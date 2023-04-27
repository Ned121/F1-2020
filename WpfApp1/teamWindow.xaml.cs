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
    /// Логика взаимодействия для teamWindow.xaml
    /// </summary>
   
    public class teamGrid
    {
        public string id { get; set; }
        public string name { get; set; }
        public string nameBolide { get; set; }
        public string year { get; set; }

        public string base1 { get; set; }
        public string nameLeader { get; set; }
        public string nameAerodynamics { get; set; }
        public string nameTechDirector { get; set; }
       
        public BitmapFrame photo { get; set; }


    }
    public partial class teamWindow : Window
    {
       

        List<teamGrid> teamGridd = new List<teamGrid>();
        MySqlConnection cn = new MySqlConnection("server = 10.0.7.240; user id = user8; port=3306;persistsecurityinfo=True;database=user8;password=wsruser8");
        public teamWindow()
        {
            InitializeComponent();
            load();
            load1();
        }
        void load1()
        {
            cn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT firstName,lastName FROM staff;", cn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            foreach (DataRow item in dt.Rows)
            {
                leaderT.Items.Add(item[0].ToString()+" "+ item[1].ToString());
                teachT.Items.Add(item[0].ToString() + " " + item[1].ToString());
                aerT.Items.Add(item[0].ToString() + " " + item[1].ToString());
            }
            MySqlCommand cmd2 = new MySqlCommand("SELECT name FROM bolides where status != 'Удален';", cn);
            DataTable dt2 = new DataTable();
            dt2.Load(cmd2.ExecuteReader());
            foreach (DataRow item in dt2.Rows)
            {
                bolideT.Items.Add(item[0].ToString());
            }
            MySqlCommand cmd3 = new MySqlCommand("SELECT name FROM locations;", cn);
            DataTable dt3 = new DataTable();
            dt3.Load(cmd3.ExecuteReader());
            foreach (DataRow item in dt3.Rows)
            {
                baseT.Items.Add(item[0].ToString());
            }
            cn.Close();

        }
        void load()
        {
            teamGridd.Clear();
            cn.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT team.id i,st.firstName leName,st.lastName leNameL,staf.firstName techName,staf.lastName techNameL,sta.firstName aerName,sta.lastName aerNameL,year,base,bolides.name bn,team.name tn,photos.photo pp FROM team LEFT JOIN bolides on bolides.id = idBolide LEFT JOIN staff as staf on staf.id = idTechDirector LEFT JOIN staff as sta on sta.id = idAerodynamics LEFT JOIN staff as st on st.id = idLeader LEFT JOIN photos on photos.id = bolides.photo where team.status != 'Удален' ", cn);
            DataTable dtb = new DataTable();
            dtb.Load(cmd1.ExecuteReader());           
            cn.Close();            
            foreach (DataRow item in dtb.Rows)
            {
                byte[] imgData = (byte[])item["pp"];
                MemoryStream ms = new MemoryStream(imgData);
                teamGridd.Add(new teamGrid()
                {
                    photo = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad),
                    id = item["i"].ToString(),
                    base1 = item["base"].ToString(),
                    name = item["tn"].ToString(),
                    nameBolide = item["bn"].ToString(),
                    year = item["year"].ToString(),
                    nameLeader = item["leName"].ToString() + " " + item["leNameL"].ToString(),
                    nameAerodynamics = item["aerName"].ToString() + " " + item["aerNameL"].ToString(),
                    nameTechDirector = item["techName"].ToString() + " " + item["techNameL"].ToString()
                });
            }
            teamList.ItemsSource = null;
            teamList.Items.Clear();
            teamList.ItemsSource = teamGridd;
        }
        void check(string a)
        {

            if (a == "update")
            {
                cn.Open();
                MySqlCommand cmd3 = new MySqlCommand("select `name` from `team` where name = @name and id != '" + idCheck.Text.ToString() + "'", cn);
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
                MySqlCommand cmd3 = new MySqlCommand("select `name` from `team` where name = @name" , cn);
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
            



           
            if (a == "add" )
            {

                cn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `team` (`idBolide`,`idLeader`,`idTechDirector`, `idAerodynamics`, `name`, `year`, `base`,`status`) VALUES ((SELECT id FROM bolides where name='" + bolideT.Text.ToString() + "')," +
                    "(SELECT id FROM staff where firstName='" + leaderT.Text.ToString().Split(new char[] { ' ' }, 2)[0] + "' and lastName ='" + leaderT.Text.ToString().Split(new char[] { ' ' }, 2)[1] + "'), " +
                    "(SELECT id FROM staff where firstName='" + teachT.Text.ToString().Split(new char[] { ' ' }, 2)[0] + "' and lastName ='" + teachT.Text.ToString().Split(new char[] { ' ' }, 2)[1] + "'), " +
                    "(SELECT id FROM staff where firstName='" + aerT.Text.ToString().Split(new char[] { ' ' }, 2)[0] + "' and lastName ='" + aerT.Text.ToString().Split(new char[] { ' ' }, 2)[1] + "'), " +
                    "'" + nameT.Text.ToString() + "','" + yearT.Text.ToString() + "','" + baseT.Text.ToString() + "', 'Работает');", cn);
               
                cmd.ExecuteReader();
                cn.Close();
                cn.Open();
                MySqlCommand cmd2 = new MySqlCommand("INSERT INTO `champconst` (`idTeam`,`win`,`poll`, `points`, `laps`) values ((SELECT id FROM team where name='" + nameT.Text.ToString() + "'),0,0,0,0)", cn);
                cmd2.ExecuteReader();

                MessageBox.Show("Данные обновлены");
                


            }
            else if (a == "update" )
            {
                
                cn.Open();
                MySqlCommand cmd5 = new MySqlCommand("update `team` set `idBolide`=(SELECT id FROM bolides where name='" + bolideT.Text.ToString() + "')," +
                    "`idLeader`=(SELECT id FROM staff where firstName='" + leaderT.Text.ToString().Split(new char[] { ' ' }, 2)[0] + "' and lastName ='" + leaderT.Text.ToString().Split(new char[] { ' ' }, 2)[1] + "')," +
                    " `idAerodynamics`=(SELECT id FROM staff where firstName='" + aerT.Text.ToString().Split(new char[] { ' ' }, 2)[0] + "' and lastName ='" + aerT.Text.ToString().Split(new char[] { ' ' }, 2)[1] + "'), `name`= '" +
                    nameT.Text.ToString() + "', `year`='" + yearT.Text.ToString() + "',`base`= '" + baseT.Text.ToString() + "',`idTechDirector`= (SELECT id FROM staff where firstName='" + teachT.Text.ToString().Split(new char[] { ' ' }, 2)[0] + "' and lastName ='" + teachT.Text.ToString().Split(new char[] { ' ' }, 2)[1] + "') where id = '" + idCheck.Text + "'", cn);
               
                cmd5.ExecuteReader();

                MessageBox.Show("Данные обновлены");

            }
            else if (a == "delete")
            {

                cn.Open();
                MySqlCommand cmd5 = new MySqlCommand("update team set status = 'Удален' where id = '" + idCheck.Text + "'", cn);
                
                cmd5.ExecuteReader();
                idCheck.Text = "";
                MessageBox.Show("Данные обновлены");

            }

        }
        private void teamDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (teamList.SelectedItems.Count != 0)
            {
                dynamic selectedItem = teamList.SelectedItem;
                idCheck.Text = selectedItem.id;
                nameT.Text = selectedItem.name;
                bolideT.Text = selectedItem.nameBolide;
                yearT.Text = selectedItem.year;
                teachT.Text = selectedItem.nameTechDirector;
                aerT.Text = selectedItem.nameAerodynamics;
                leaderT.Text = selectedItem.nameLeader;
                baseT.Text = selectedItem.base1;
                image.Source = selectedItem.photo;
            }            
        }
        private void clickAdd(object sender, RoutedEventArgs e)
        {
            if (nameT.Text != "" && bolideT.Text != "" && yearT.Text.Trim() != "" && teachT.Text != "" && aerT.Text != "" && baseT.Text != "" && leaderT.Text != "")
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
            if (nameT.Text != "" && bolideT.Text != "" && yearT.Text.Trim() != "" && teachT.Text != "" && aerT.Text != "" && baseT.Text != "" && leaderT.Text != "" && idCheck.Text != "")
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
            if (nameT.Text != "" && bolideT.Text != "" && yearT.Text.Trim() != "" && teachT.Text != "" && aerT.Text != "" && baseT.Text != "" && leaderT.Text != "" && idCheck.Text != "")
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
            MainWindow fm = new MainWindow(1);
            fm.Show();
            this.Close();


        }

        private void staffButton(object sender, RoutedEventArgs e)
        {
            staffWindow fm = new staffWindow();
            fm.Show();
            this.Close();

        }
    }
}
