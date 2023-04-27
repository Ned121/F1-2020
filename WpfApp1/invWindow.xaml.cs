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
    /// Логика взаимодействия для invWindow.xaml
    /// </summary>
  
    public class invGrid
    {
        public string id { get; set; }
        public string team { get; set; }

        public string sponsor { get; set; }
        public string quantity { get; set; }

    }
    public partial class invWindow : Window
    {

        
        List<invGrid> invListD = new List<invGrid>();
        MySqlConnection cn = new MySqlConnection("server = 10.0.7.240; user id = user8; port=3306;persistsecurityinfo=True;database=user8;password=wsruser8");
        public invWindow()
        {
            InitializeComponent();
            load();
            load1();
        }
        void load1()
        {
            cn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT name FROM team;", cn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            foreach (DataRow item in dt.Rows)
            {
                teamI.Items.Add(item[0].ToString());
            }
            MySqlCommand cmd1 = new MySqlCommand("SELECT name FROM sponsors;", cn);
            DataTable dt1 = new DataTable();
            dt1.Load(cmd1.ExecuteReader());
            foreach (DataRow item in dt1.Rows)
            {
                sponsorI.Items.Add(item[0].ToString());
            }
            
            cn.Close();

        }

        void load()
        {
            invListD.Clear();
            cn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT sponsors.name s, team.name t,investments.id i,idSponsor,idTeam,quantity FROM investments LEFT JOIN team ON team.id = idTeam LEFT JOIN sponsors on sponsors.id = idSponsor", cn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            cn.Close();
            foreach (DataRow inv in dt.Rows)
            {
                invListD.Add(new invGrid()
                {
                    id = inv["i"].ToString(),
                    team = inv["t"].ToString(),
                    sponsor = inv["s"].ToString(),
                    quantity = inv["quantity"].ToString(),
                });
            }
            invList.ItemsSource = null;
            invList.Items.Clear();
            invList.ItemsSource = invListD;
        }
        private void clickAdd(object sender, RoutedEventArgs e)
        {
            if (quantityI.Text.Trim() != "" && sponsorI.Text != "" && teamI.Text != "")
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
        private void invDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (invList.SelectedItems.Count != 0)
            {
                dynamic selectedItem = invList.SelectedItem;
                quantityI.Text = selectedItem.quantity;
                teamI.Text = selectedItem.team;
                sponsorI.Text = selectedItem.sponsor;
                idCheck.Text = selectedItem.id;                
            }
        }
        void check(string a)
        {
            
            if (a == "add" )
            {

                cn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `investments` (`idSponsor`,`idTeam`, `quantity`) VALUES ((SELECT id FROM sponsors where name='" + sponsorI.Text.ToString() + "'), (SELECT id FROM team where name='" + teamI.Text.ToString() + "'), '" + quantityI.Text + "');", cn);
                cmd.ExecuteReader();
                MessageBox.Show("Данные обновлены");
               
               
            }
            else if (a == "update" )
            {

                cn.Open();
                MySqlCommand cmd5 = new MySqlCommand("update `investments` set `idSponsor`=(SELECT id FROM sponsors where name='" + sponsorI.Text.ToString() + "'),`idTeam`=(SELECT id FROM team where name='" + teamI.Text.ToString() + "'), `quantity`='" + quantityI.Text + "' where id = '" + idCheck.Text + "'", cn);
                
                cmd5.ExecuteReader();

                MessageBox.Show("Данные обновлены");

            }
            else if (a == "delete")
            {

                cn.Open();
                MySqlCommand cmd5 = new MySqlCommand("delete from investments where id = '" + idCheck.Text + "'", cn);

                cmd5.ExecuteReader();
                idCheck.Text = "";
                MessageBox.Show("Данные обновлены");

            }

        }

        private void clickDel(object sender, RoutedEventArgs e)
        {
            if (idCheck.Text.Trim() != "")
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
            if (idCheck.Text.Trim() != "" && quantityI.Text.Trim() != "" && sponsorI.Text != "" && teamI.Text != "")
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
            this.Close();


        }
        
        private void sponsorClick(object sender, RoutedEventArgs e)
        {
            sponWindow fm = new sponWindow();
            fm.Show();
            Close();


        }

        private void intInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^.*\\s*[^0-9].*$");
            e.Handled = regex.IsMatch(e.Text);

        }
        

       
    }
}
