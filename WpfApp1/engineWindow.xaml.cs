using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для engineWindow.xaml
    /// </summary>
    
    public class engineGrid
    {
        public string id { get; set; }
        public string name { get; set; }
        public string capacity { get; set; }
        public string cylinder { get; set; }
        public string RPM { get; set; }
        public string brand { get; set; }


    }
    public partial class engineWindow : Window
    {
        
        List<engineGrid> engineListd = new List<engineGrid>();
        MySqlConnection cn = new MySqlConnection("server = 10.0.7.240; user id = user8; port=3306;persistsecurityinfo=True;database=user8;password=wsruser8");
        public engineWindow()
        {
            InitializeComponent();
            load();
            load1();
        }
        void load1()
        {
            cn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT brand FROM brand;", cn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            foreach (DataRow item in dt.Rows)
            {
                brandE.Items.Add(item[0].ToString());
            }
            
            cn.Close();

        }
        void load()
        {
            engineListd.Clear();
            cn.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT id,name,capacity,cylinder,RPM,brand FROM engine", cn);
            DataTable dtb = new DataTable();
            dtb.Load(cmd1.ExecuteReader());
            cn.Close();
            foreach (DataRow item in dtb.Rows)
            {
                engineListd.Add(new engineGrid()
                {
                    id = item["id"].ToString(),
                    name = item["name"].ToString(),
                    capacity = item["capacity"].ToString(),
                    cylinder = item["cylinder"].ToString(),
                    RPM = item["RPM"].ToString(),
                    brand = item["brand"].ToString()
                });
            }
            engList.ItemsSource = null;
            engList.Items.Clear();
            engList.ItemsSource = engineListd;
        }
        void check(string a)
        {

           
            if (a == "update")
            {
                cn.Open();
                MySqlCommand cmd3 = new MySqlCommand("select `name` from engine where name = @name and id != '" +idCheck.Text.ToString() + "'", cn);
                cmd3.Parameters.Add(new MySqlParameter("@name", nameE.Text));
                DataTable dtb1 = new DataTable();
                dtb1.Load(cmd3.ExecuteReader());
                var nameBol = dtb1.Rows;
                if (nameBol.Count != 0)
                {
                    MessageBox.Show("Нельзя добавить двигатель с одинаковым названием");
                    return;
                }
                cn.Close();
                


            }
            if (a == "add")
            {
                cn.Open();
                MySqlCommand cmd3 = new MySqlCommand("select `name` from engine where name = @name", cn);
                cmd3.Parameters.Add(new MySqlParameter("@name", nameE.Text));
                DataTable dtb1 = new DataTable();
                dtb1.Load(cmd3.ExecuteReader());
                var nameBol = dtb1.Rows;
                if (nameBol.Count != 0)
                {
                    MessageBox.Show("Нельзя добавить двигатель с одинаковым названием");
                    return;
                }
                cn.Close();
                



            }





            if (a == "add")
            {

                cn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `engine` (`name`,`capacity`,`cylinder`,`brand`,`RPM`) VALUES ('" + nameE.Text.ToString() + "'," +
                    "" + "'" + capacityE.Text.ToString() + "', " + "'" + cylinderE.Text.ToString() + "', " + "'" + brandE.Text.ToString() + "', " + "'" + RPME.Text.ToString() + "');", cn);
                cmd.ExecuteReader();
                MessageBox.Show("Данные обновлены");

            }
            else if (a == "update")
            {

                cn.Open();
                MySqlCommand cmd2 = new MySqlCommand("update `engine` set `name`='" + nameE.Text.ToString() + "'," +
                    "`capacity`='" + capacityE.Text.ToString() + "',`cylinder`='" + cylinderE.Text.ToString() + "',`brand`='" + brandE.Text.ToString() + "',`RPM`='" + RPME.Text.ToString() + "' where id = '" + idCheck.Text + "'", cn);

                cmd2.ExecuteReader();

                MessageBox.Show("Данные обновлены");

            }
            else if (a == "delete")
            {

                cn.Open();
                MySqlCommand cmd5 = new MySqlCommand("delete from engine where id = '" + idCheck.Text + "'", cn);

                cmd5.ExecuteReader();
                idCheck.Text = "";
                MessageBox.Show("Данные обновлены");

            }

        }
        private void engDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (engList.SelectedItems.Count != 0)
            {
                dynamic selectedItem = engList.SelectedItem;
                idCheck.Text = selectedItem.id;
                nameE.Text = selectedItem.name;
                capacityE.Text = selectedItem.capacity;
                cylinderE.Text = selectedItem.cylinder;
                brandE.Text = selectedItem.brand;
                RPME.Text = selectedItem.RPM;
            }
        }
        private void clickAdd(object sender, RoutedEventArgs e)
        {
            if (nameE.Text != "" && capacityE.Text.Trim() != "" && cylinderE.Text.Trim() != "" && brandE.Text != "" && RPME.Text.Trim() != "")
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
            if (nameE.Text != "" && brandE.Text != "" && idCheck.Text != "")
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
            if (nameE.Text != "" && capacityE.Text.Trim() != "" && cylinderE.Text.Trim() != "" && brandE.Text != "" && RPME.Text.Trim() != "" && idCheck.Text != "")
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
            bolidesWindow fm = new bolidesWindow();
            fm.Show();
            Close();


        }

      
    }

}
