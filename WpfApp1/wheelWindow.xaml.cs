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
    /// Логика взаимодействия для wheelWindow.xaml
    /// </summary>

    public class wheelGrid
    {
        public string id { get; set; }
        public string name { get; set; }
        public string radius { get; set; }


    }
    public partial class wheelWindow : Window
    {
        
        List<wheelGrid> wheelListd = new List<wheelGrid>();
        MySqlConnection cn = new MySqlConnection("server = 10.0.7.240; user id = user8; port=3306;persistsecurityinfo=True;database=user8;password=wsruser8");
        public wheelWindow()
        {
            InitializeComponent();
            load();
        }
        void load()
        {
            wheelListd.Clear();
            cn.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT id,name,radius FROM wheels", cn);
            DataTable dtb = new DataTable();
            dtb.Load(cmd1.ExecuteReader());
            cn.Close();
            foreach (DataRow item in dtb.Rows)
            {
                wheelListd.Add(new wheelGrid()
                {
                    id = item["id"].ToString(),
                    name = item["name"].ToString(),
                    radius = item["radius"].ToString()
                });
            }
            wheelList.ItemsSource = null;
            wheelList.Items.Clear();
            wheelList.ItemsSource = wheelListd;
        }
        void check(string a)
        {

            
            if (a == "update")
            {
                cn.Open();
                MySqlCommand cmd3 = new MySqlCommand("select `name` from wheels where name = @name and id != '" + idCheck.Text.ToString() + "'", cn); ;
                cmd3.Parameters.Add(new MySqlParameter("@name", nameW.Text));
                DataTable dtb1 = new DataTable();
                dtb1.Load(cmd3.ExecuteReader());
                var nameBol = dtb1.Rows;
                
                if (nameBol.Count != 0)
                {
                    MessageBox.Show("Нельзя добавить колеса с одинаковым названием");
                    return;
                }
                cn.Close();



            }
            if (a == "add")
            {
                cn.Open();
                MySqlCommand cmd3 = new MySqlCommand("select `name` from wheels where name = @name", cn); ;
                cmd3.Parameters.Add(new MySqlParameter("@name", nameW.Text));
                DataTable dtb1 = new DataTable();
                dtb1.Load(cmd3.ExecuteReader());
                var nameBol = dtb1.Rows;
               
                if (nameBol.Count != 0)
                {
                    MessageBox.Show("Нельзя добавить колеса с одинаковым названием");
                    return;
                }
                cn.Close();




            }





            if (a == "add")
            {

                cn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `wheels` (`name`,`radius`) VALUES ('" + nameW.Text.ToString() + "', '" + radiusW.Text.ToString() + "');", cn);
                cmd.ExecuteReader();
                MessageBox.Show("Данные обновлены");

            }
            else if (a == "update")
            {

                cn.Open();
                MySqlCommand cmd2 = new MySqlCommand("update `wheels` set `name`='" + nameW.Text.ToString() + "',`radius`='" + radiusW.Text.ToString() + "' where id = '" + idCheck.Text + "'", cn);

                cmd2.ExecuteReader();

                MessageBox.Show("Данные обновлены");

            }
            else if (a == "delete")
            {

                cn.Open();
                MySqlCommand cmd5 = new MySqlCommand("delete from wheels where id = '" + idCheck.Text + "'", cn);
                
                cmd5.ExecuteReader();
                idCheck.Text = "";
                MessageBox.Show("Данные обновлены");

            }

        }
        private void wheelDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (wheelList.SelectedItems.Count != 0)
            {
                dynamic selectedItem = wheelList.SelectedItem;
                idCheck.Text = selectedItem.id;
                nameW.Text = selectedItem.name;
                radiusW.Text = selectedItem.radius;
            }
        }
        private void clickAdd(object sender, RoutedEventArgs e)
        {
            if (nameW.Text != "" && radiusW.Text.Trim() != "")
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
            if (nameW.Text != "" && radiusW.Text.Trim() != "" && idCheck.Text != "")
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
            if (nameW.Text != "" && radiusW.Text.Trim() != "" && idCheck.Text != "")
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
            this.Close();


        }
    }
}
