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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(int w)
        {
            
            InitializeComponent();
            load1();
            if(w == 2)
            {
                teamB.Visibility = Visibility.Hidden;
                bolB.Visibility = Visibility.Hidden;
                invB.Visibility = Visibility.Hidden;
                pilB.Visibility = Visibility.Hidden;

            }else if(w == 1)
            {
                trB.Visibility = Visibility.Hidden;
                granPrix.Visibility = Visibility.Hidden;
                typeRace.Visibility = Visibility.Hidden;
                champ.Visibility = Visibility.Hidden;
                cupB.Visibility = Visibility.Hidden;
                l1.Visibility = Visibility.Hidden;
                l2.Visibility = Visibility.Hidden;
                l3.Visibility = Visibility.Hidden;
                l4.Visibility = Visibility.Hidden;
                raceB.Visibility = Visibility.Hidden;
            }

        }

        MySqlConnection cn = new MySqlConnection("server = 10.0.7.240; user id = user8; port=3306;persistsecurityinfo=True;database=user8;password=wsruser8");
        void load1()
        {
            cn.Open(); 
            MySqlCommand cmd = new MySqlCommand("SELECT grandPrix FROM grandprix;", cn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            foreach (DataRow item in dt.Rows)
            {
                granPrix.Items.Add(item[0].ToString());
            } 
            cn.Close();

        }
        private void buttonTeam(object sender, RoutedEventArgs e)
        {
            teamWindow fm = new teamWindow();
            fm.Show();
            Close();
        }
        private void buttonPilot(object sender, RoutedEventArgs e)
        {
            pilotsWindow fm = new pilotsWindow();
            fm.Show();
            Close();

        }
        
        private void buttonBolide(object sender, RoutedEventArgs e)
        {
            bolidesWindow fm = new bolidesWindow();
            fm.Show();
            Close();
        }
        private void buttonTrack(object sender, RoutedEventArgs e)
        {
            tracksWindow fm = new tracksWindow();
            fm.Show();
            Close();
        }

        private void buttonRace(object sender, RoutedEventArgs e)
        {
            if(granPrix.Text != "" && typeRace.Text != "")
            {
                
                racesWindow fm = new racesWindow(granPrix.Text, typeRace.Text);
                fm.Show();
                Close();


            }
            else MessageBox.Show("Выберите гран-при и тип ");

        }

        private void champClick(object sender, RoutedEventArgs e)
        {
            if (champ.Text != "")
            {

                champWindow fm = new champWindow(champ.Text);
                fm.Show();
                Close();


            }
            else MessageBox.Show("Выберите тип кубка");

        }

        private void invClick(object sender, RoutedEventArgs e)
        {
            invWindow fm = new invWindow();
            fm.Show();
            Close();

        }

        private void buttonBack(object sender, RoutedEventArgs e)
        {
            authWindow fm = new authWindow();
            fm.Show();
            this.Close();

        }
    }
}
