using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Windows.Shapes;

namespace WpfApp1
{
   
    public class spravGrid
    {
        public string sprav { get; set; }

    }

    public partial class spravWindow : Window
    {
        string znach = "ff";
        string znach2 = "ff";
        public spravWindow(string s)
        {
            znach = s.ToLower();
            znach2 = s;
            if (znach == "locations")
            {
                znach2 = "name";

            }
            InitializeComponent();
            load();
        }
        

        List<spravGrid> spravListS = new List<spravGrid>();
        MySqlConnection cn = new MySqlConnection("server = 10.0.7.240; user id = user8; port=3306;persistsecurityinfo=True;database=user8;password=wsruser8");
        void load()
        {
            spravListS.Clear();
            cn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT `" + znach2 + "` FROM `" + znach + "`", cn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            cn.Close();
            foreach (DataRow item in dt.Rows)
            {
                spravListS.Add(new spravGrid()
                {
                    sprav = item[znach2].ToString(),

                });
            }
            spravList.ItemsSource = null;
            spravList.Items.Clear();
            spravList.ItemsSource = spravListS;
        }
       
        private void spravDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (spravList.SelectedItems.Count != 0)
            {
                dynamic selectedItem = spravList.SelectedItem;
                spravZ.Text = selectedItem.sprav;
                idCheck.Text = selectedItem.sprav;
            }
        }


      

        private void backButton(object sender, RoutedEventArgs e)
        {
            adminWindow fm = new adminWindow();
            fm.Show();
            this.Close();
        }

        void check(string a)
        {
            if (a == "update")
            {
                cn.Open();
                MySqlCommand cmd3 = new MySqlCommand("select `" + znach2 + "` from `" + znach + "` where `" + znach2 + "` = @id and `" + znach2 + "` != '" + idCheck.Text.ToString() + "'", cn);
                cmd3.Parameters.Add(new MySqlParameter("@id", spravZ.Text));
                DataTable dtb1 = new DataTable();
                dtb1.Load(cmd3.ExecuteReader());
                var idPil = dtb1.Rows;
                if (idPil.Count != 0)
                {
                    MessageBox.Show("Нельзя добавить одинаковые данные");
                    return;
                }
                cn.Close();


            }
            if (a == "add")
            {
                cn.Open();
                MySqlCommand cmd3 = new MySqlCommand("select `" + znach2 + "` from `" + znach + "` where `" + znach2 + "` = @id", cn);
                cmd3.Parameters.Add(new MySqlParameter("@id", spravZ.Text));
                DataTable dtb1 = new DataTable();
                dtb1.Load(cmd3.ExecuteReader());
                var idPil = dtb1.Rows;
                if (idPil.Count != 0)
                {
                    MessageBox.Show("Нельзя добавить одинаковые данные");
                    return;
                }
                cn.Close();


            }

            if (a == "add")
            {

                cn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `" + znach + "` (`" + znach2 + "`) VALUES ('" + spravZ.Text + "');", cn);
                cmd.ExecuteReader();
                
                MessageBox.Show("Данные обновлены");

            }
            else if (a == "update")
            {

                cn.Open();
                MySqlCommand cmd5 = new MySqlCommand("update `" + znach + "` set `" + znach2 + "`='" + spravZ.Text + "' where `" + znach2 + "` = '" + idCheck.Text + "'", cn);

                cmd5.ExecuteReader();

                MessageBox.Show("Данные обновлены");

            }
            

        }

        private void clickAdd(object sender, RoutedEventArgs e)
        {
            if (spravZ.Text != "")
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

        

        private void clickUpd(object sender, RoutedEventArgs e)
        {
            if (idCheck.Text.Trim() != "" && spravZ.Text != "")
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

     




    }
}
