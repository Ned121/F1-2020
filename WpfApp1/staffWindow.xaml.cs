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
    /// Логика взаимодействия для staffWindow.xaml
    /// </summary>
   
    public class staffGrid
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string patronymic { get; set; }
        public string gender { get; set; }


    }
    public partial class staffWindow : Window
    {

        List<staffGrid> staffListd = new List<staffGrid>();
        MySqlConnection cn = new MySqlConnection("server = 10.0.7.240; user id = user8; port=3306;persistsecurityinfo=True;database=user8;password=wsruser8");
        public staffWindow()
        {
            InitializeComponent();
            load();
            load1();
        }
        void load()
        {
            staffListd.Clear();
            cn.Open();
            MySqlCommand cmd1 = new MySqlCommand("SELECT id,firstName,lastName,patronymic,gender FROM staff", cn);
            DataTable dtb = new DataTable();
            dtb.Load(cmd1.ExecuteReader());
            cn.Close();
            foreach (DataRow item in dtb.Rows)
            {
                staffListd.Add(new staffGrid()
                {

                    id = item["id"].ToString(),
                    firstName = item["firstName"].ToString(),
                    lastName = item["lastName"].ToString(),
                    patronymic = item["patronymic"].ToString(),
                    gender = item["gender"].ToString()
                });
            }
            staffList.ItemsSource = null;
            staffList.Items.Clear();
            staffList.ItemsSource = staffListd;
        }
        void load1()
        {
            cn.Open();  
            MySqlCommand cmd3 = new MySqlCommand("SELECT gender FROM gender;", cn);
            DataTable dt3 = new DataTable();
            dt3.Load(cmd3.ExecuteReader());
            foreach (DataRow item in dt3.Rows)
            {
                gend.Items.Add(item[0].ToString());
            }
            cn.Close();

        }
        void check(string a)
        {

            if (a == "add")
            {

                cn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `staff` (`firstName`,`lastName`,`patronymic`,`gender`) VALUES ('" + nameF.Text.ToString().Trim() + "'," +
                    " '" + nameL.Text.ToString().Trim() + "', '" + patr.Text.ToString().Trim() + "', '" + gend.Text.ToString() + "');", cn);
                cmd.ExecuteReader();
                MessageBox.Show("Данные обновлены");

            }
            else if (a == "update")
            {

                cn.Open();
                MySqlCommand cmd2 = new MySqlCommand("update `staff` set `firstName`='" + nameF.Text.ToString().Trim() + "',`lastName`='" + nameL.Text.ToString().Trim() + "',`patronymic`='" + patr.Text.ToString().Trim() + "',`gender`='" + gend.Text.ToString() + "' where id = '" + idCheck.Text + "'", cn);

                cmd2.ExecuteReader();

                MessageBox.Show("Данные обновлены");

            }
            else if (a == "delete")
            {

                cn.Open();
                MySqlCommand cmd5 = new MySqlCommand("delete from staff where id = '" + idCheck.Text + "'", cn);

                cmd5.ExecuteReader();
                idCheck.Text = "";
                MessageBox.Show("Данные обновлены");

            }

        }
        private void staffDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(staffList.SelectedItems.Count != 0)
            {
                dynamic selectedItem = staffList.SelectedItem;
                idCheck.Text = selectedItem.id;
                nameL.Text = selectedItem.lastName;
                nameF.Text = selectedItem.firstName;
                patr.Text = selectedItem.patronymic;
                gend.Text = selectedItem.gender;
            }
        }
        private void clickAdd(object sender, RoutedEventArgs e)
        {
            if (nameL.Text.Trim() != "" && nameF.Text.Trim() != ""  && gend.Text != "")
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
            if (nameL.Text.Trim() != "" && nameF.Text.Trim() != "" && gend.Text != "" && idCheck.Text != "")
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
            if (nameL.Text.Trim() != "" && nameF.Text.Trim() != "" && gend.Text != "" && idCheck.Text != "")
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
        private void backButton(object sender, RoutedEventArgs e)
        {
            teamWindow fm = new teamWindow();
            fm.Show();
            this.Close();


        }
    }
}
