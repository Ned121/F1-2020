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
    /// Логика взаимодействия для sponWindow.xaml
    /// </summary>
    public class sponGrid
    {
        public string id { get; set; }
        public string name { get; set; }
        public string materials { get; set; }


    }
    public partial class sponWindow : Window
    {

        List<sponGrid> sponListd = new List<sponGrid>();
        MySqlConnection cn = new MySqlConnection("server = 10.0.7.240; user id = user8; port=3306;persistsecurityinfo=True;database=user8;password=wsruser8");
        public sponWindow()
        {
            InitializeComponent();
            load();
        }
        void load()
        {
            sponListd.Clear();
            cn.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT id,name,materials FROM sponsors", cn);
            DataTable dtb = new DataTable();
            dtb.Load(cmd1.ExecuteReader());
            cn.Close();
            foreach (DataRow item in dtb.Rows)
            {
                sponListd.Add(new sponGrid()
                {
                    id = item["id"].ToString(),
                    name = item["name"].ToString(),
                    materials = item["materials"].ToString()
                });
            }
            sponList.ItemsSource = null;
            sponList.Items.Clear();
            sponList.ItemsSource = sponListd;
        }
        void check(string a)
        {


            if (a == "update")
            {
                cn.Open();
                MySqlCommand cmd3 = new MySqlCommand("select `name` from sponsors where name = @name and id != '" + idCheck.Text.ToString() + "'", cn); ;
                cmd3.Parameters.Add(new MySqlParameter("@name", nameS.Text));
                DataTable dtb1 = new DataTable();
                dtb1.Load(cmd3.ExecuteReader());
                var nameBol = dtb1.Rows;

                if (nameBol.Count != 0)
                {
                    MessageBox.Show("Нельзя добавить спонсора с одинаковым названием");
                    return;
                }
                cn.Close();



            }
            if (a == "add")
            {
                cn.Open();
                MySqlCommand cmd3 = new MySqlCommand("select `name` from sponsors where name = @name", cn); ;
                cmd3.Parameters.Add(new MySqlParameter("@name", nameS.Text));
                DataTable dtb1 = new DataTable();
                dtb1.Load(cmd3.ExecuteReader());
                var nameBol = dtb1.Rows;

                if (nameBol.Count != 0)
                {
                    MessageBox.Show("Нельзя добавить спонсора с одинаковым названием");
                    return;
                }
                cn.Close();




            }





            if (a == "add")
            {

                cn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `sponsors` (`name`,`materials`) VALUES ('" + nameS.Text.ToString() + "', '" + materialsS.Text.ToString() + "');", cn);
                cmd.ExecuteReader();
                MessageBox.Show("Данные обновлены");

            }
            else if (a == "update")
            {

                cn.Open();
                MySqlCommand cmd2 = new MySqlCommand("update `sponsors` set `name`='" + nameS.Text.ToString() + "',`materials`='" + materialsS.Text.ToString() + "' where id = '" + idCheck.Text + "'", cn);

                cmd2.ExecuteReader();

                MessageBox.Show("Данные обновлены");

            }
            else if (a == "delete")
            {

                cn.Open();
                MySqlCommand cmd5 = new MySqlCommand("delete from sponsors where id = '" + idCheck.Text + "'", cn);

                cmd5.ExecuteReader();
                idCheck.Text = "";
                MessageBox.Show("Данные обновлены");

            }

        }
        private void sponDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sponList.SelectedItems.Count != 0)
            {
                dynamic selectedItem = sponList.SelectedItem;
                idCheck.Text = selectedItem.id;
                nameS.Text = selectedItem.name;
                materialsS.Text = selectedItem.materials;
            }
        }
        private void clickAdd(object sender, RoutedEventArgs e)
        {
            if (nameS.Text != "" )
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
            if (idCheck.Text != "")
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
            if (nameS.Text != "" && idCheck.Text != "")
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
        private void charInputR(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^.*\\s*[^А-яЁё].*$");
            e.Handled = regex.IsMatch(e.Text);

        }
        private void backButton(object sender, RoutedEventArgs e)
        {
            invWindow fm = new invWindow();
            fm.Show();
            this.Close();


        }
    }
}
