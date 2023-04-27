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
    public class usersGrid
    {
        public string id { get; set; }
        public string number { get; set; }
        public string emai { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string patronymic { get; set; }
        public string gender { get; set; }
        public string password { get; set; }
        public string statusR { get; set; }
        public string roleS { get; set; }
        public string dateBirth { get; set; }
   

    }
    public partial class adminWindow : Window
    {
        

        List<usersGrid> usersListS = new List<usersGrid>();
        MySqlConnection cn = new MySqlConnection("server = 10.0.7.240; user id = user8; port=3306;persistsecurityinfo=True;database=user8;password=wsruser8");
        void load1()
        {
            cn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT role FROM role ;", cn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            foreach (DataRow item in dt.Rows)
            {
                role.Items.Add(item[0].ToString());
            }
            MySqlCommand cmd1 = new MySqlCommand("SELECT gender FROM gender;", cn);
            DataTable dt1 = new DataTable();
            dt1.Load(cmd1.ExecuteReader());
            foreach (DataRow item in dt1.Rows)
            {
                genderP.Items.Add(item[0].ToString());
            }
            MySqlCommand cmd2 = new MySqlCommand("SELECT status FROM status;", cn);
            DataTable dt2 = new DataTable();
            dt2.Load(cmd2.ExecuteReader());
            foreach (DataRow item in dt2.Rows)
            {
                status.Items.Add(item[0].ToString());
            }
            cn.Close();

        }

        void load()
        {
            usersListS.Clear();
            cn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT id,phone,firstName,lastName,patronymic,gender,email,status,role,date,password FROM users", cn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            cn.Close();
            foreach (DataRow item in dt.Rows)
            {
                
                usersListS.Add(new usersGrid()
                {
                    
                    id = item["id"].ToString(),
                    firstName = item["firstName"].ToString(),
                    lastName = item["lastName"].ToString(),
                    patronymic = item["patronymic"].ToString(),
                    gender = item["gender"].ToString(),
                    password = item["password"].ToString(),
                    statusR = item["status"].ToString(),
                    roleS = item["role"].ToString(),
                    dateBirth = Convert.ToDateTime(item["date"]).Date.ToString("dd.MM.yyyy"),
                    
                    emai = item["email"].ToString(),
                    number = item["phone"].ToString(),

                });
            }
            usersList.ItemsSource = null;
            usersList.Items.Clear();
            usersList.ItemsSource = usersListS;
        }
        public adminWindow()
        {
            InitializeComponent();
            load();

            load1();
        }

        private void usersDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (usersList.SelectedItems.Count != 0)
            {
                dynamic selectedItem = usersList.SelectedItem;
                phone.Text = selectedItem.number;
                nameL.Text = selectedItem.lastName;
                nameF.Text = selectedItem.firstName;
                patr.Text = selectedItem.patronymic;
                email.Text = selectedItem.emai;
                passw.Text = selectedItem.password;
                date.Text = selectedItem.dateBirth;
                genderP.Text = selectedItem.gender;
                role.Text = selectedItem.roleS;
                status.Text = selectedItem.statusR;
                idCheck.Text = selectedItem.id;
            }
        }

        private void backButton(object sender, RoutedEventArgs e)
        {
            authWindow fm = new authWindow();
            fm.Show();
            this.Close();
        }

        void check(string a)
        {
            if (Convert.ToBoolean(email.Tag) == false && a != "delete")
            {
                MessageBox.Show("Email указан не верно");
                return;
            }
            if (a == "update")
            {
                cn.Open();
                MySqlCommand cmd3 = new MySqlCommand("select `email` from users where email = @id and id != '" + idCheck.Text.ToString() + "'", cn);
                cmd3.Parameters.Add(new MySqlParameter("@id", email.Text.Trim()));
                DataTable dtb1 = new DataTable();
                dtb1.Load(cmd3.ExecuteReader());
                var idPil = dtb1.Rows;
                if (idPil.Count != 0)
                {
                    MessageBox.Show("Нельзя добавить пользователя с одинаковым ел. почтой");
                    return;
                }
                cn.Close();


            }
            if (a == "add")
            {
                cn.Open();
                MySqlCommand cmd3 = new MySqlCommand("select `email` from users where email = @id", cn);
                cmd3.Parameters.Add(new MySqlParameter("@id", email.Text.Trim()));
                DataTable dtb1 = new DataTable();
                dtb1.Load(cmd3.ExecuteReader());
                var idPil = dtb1.Rows;
                if (idPil.Count != 0)
                {
                    MessageBox.Show("Нельзя добавить пользователя с одинаковым ел. почтой");
                    return;
                }
                cn.Close();


            }

            



            if (a == "add")
            {

                cn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `users` (`phone`,`password`, `firstName`, `lastName`, `patronymic`,`gender`, `date`, `status`,`role`,`email`) VALUES ('" + phone.Text + "', '" + passw.Text + "', '" + nameF.Text + "', '" + nameL.Text + "', " +
                    "'" + patr.Text + "','" + genderP.Text + "', '" + date.SelectedDate.Value.ToString("yyyy-MM-dd HH.mm.ss") + "', '" + status.Text + "', '" + role.Text + "', '" + email.Text + "');", cn);
                cmd.ExecuteReader();

                MessageBox.Show("Данные обновлены");

            }
            else if (a == "update")
            {

                cn.Open();
                MySqlCommand cmd5 = new MySqlCommand("update `users` set `phone`='" + phone.Text + "',`password`='" + passw.Text + "', `firstName`='" + nameF.Text + "', `lastName`= '" + nameL.Text + "', `patronymic`='" + patr.Text + "',`gender`= '" + genderP.Text + "', `date`='" + date.SelectedDate.Value.ToString("yyyy-MM-dd HH.mm.ss") +
                    "', `status`='" + status.Text + "',`role`= '" + role.Text + "',`email`= '" + email.Text + "' where id = '" + idCheck.Text + "'", cn);

                cmd5.ExecuteReader();

                MessageBox.Show("Данные обновлены");

            }
            else if (a == "delete")
            {


                cn.Open();
                MySqlCommand cmd5 = new MySqlCommand("update `users` set status = 'Удален' where id = '" + idCheck.Text + "'", cn);

                cmd5.ExecuteReader();
                idCheck.Text = "";
                MessageBox.Show("Данные обновлены");

            }

        }

        private void clickAdd(object sender, RoutedEventArgs e)
        {
            if (nameF.Text.Trim() != "" && date.Text != "" && phone.Text.Trim() != "" && nameL.Text.Trim() != "" && genderP.Text.Trim() != "")
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
            if (idCheck.Text.Trim() != "" && nameF.Text.Trim() != "" && date.Text != "" && phone.Text.Trim() != "" && nameL.Text.Trim() != "" && genderP.Text.Trim() != "")
            {
                try
                {
                    
                    check("delete");
                    
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
            if (idCheck.Text.Trim() != "" && nameF.Text.Trim() != "" && date.Text != "" && phone.Text.Trim() != "" && nameL.Text.Trim() != "" && genderP.Text.Trim() != "")
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
        private void emailInput(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", RegexOptions.IgnoreCase);

            email.Tag = regex.IsMatch(email.Text);

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

        private void spravButton(object sender, RoutedEventArgs e)
        {
            if (spravC.Text != "")
            {

                spravWindow fm = new spravWindow(spravC.Text);
                fm.Show();
                Close();


            }
            else MessageBox.Show("Выберите справочник");
        }
    }
}
