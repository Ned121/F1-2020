using Microsoft.Win32;
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
    public class racesGrid
    {
        public string id { get; set; }
        
        public string name { get; set; }
        public string team { get; set; }
        public string track { get; set; }
        public string km { get; set; }
        public string time { get; set; }
        public string circles { get; set; }
        public string place { get; set; }
        public string pit { get; set; }
        public string accident { get; set; }
        public BitmapFrame photo { get; set; }
    }
    public partial class racesWindow : Window
    {
        string granRaceCheck = "";
        string granTypeCheck = "";
       
        public racesWindow(string granRace, string typeRace)
        {
            InitializeComponent();
            granRaceCheck = granRace;
            granTypeCheck = typeRace;
            lableGran.Content = granRace + " " + typeRace;
            
            
                
                
                
            
            if (granTypeCheck == "Квалификация")
            {
                pitR.Visibility = Visibility.Hidden;
                pitLab.Visibility = Visibility.Hidden;
                accidentR.Visibility = Visibility.Hidden;
                acciLab.Visibility = Visibility.Hidden;

            }
            
            else if (granTypeCheck.ToString().Split(new char[] { ' ' }, 2)[0] == "Практика")
            {


                placLab.Visibility = Visibility.Hidden;
                placeT.Visibility = Visibility.Hidden;
                pitR.Visibility = Visibility.Hidden;
                pitLab.Visibility = Visibility.Hidden;
                accidentR.Visibility = Visibility.Hidden;
                acciLab.Visibility = Visibility.Hidden;
                

            }
            
            load1();
            load();
        }



        List<racesGrid> racesListD = new List<racesGrid>();
        MySqlConnection cn = new MySqlConnection("server = 10.0.7.240; user id = user8; port=3306;persistsecurityinfo=True;database=user8;password=wsruser8");

        void load1()
        {
            cn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT number,firstName, lastName FROM pilots;", cn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            foreach (DataRow item in dt.Rows)
            {
                pilotT.Items.Add(item[0].ToString() + " " + item[1].ToString() + " " + item[2].ToString());
            }
            MySqlCommand cmd1 = new MySqlCommand("SELECT place FROM places;", cn);
            DataTable dt1 = new DataTable();
            dt1.Load(cmd1.ExecuteReader());
            foreach (DataRow item in dt1.Rows)
            {
                placeT.Items.Add(item[0].ToString());
            }
            MySqlCommand cmd2 = new MySqlCommand("SELECT name FROM tracks where status != 'Удален';", cn);
            DataTable dt2 = new DataTable();
            dt2.Load(cmd2.ExecuteReader());
            foreach (DataRow item in dt2.Rows)
            {
                trackT.Items.Add(item[0].ToString());
                
            }
            MySqlCommand cmd3 = new MySqlCommand("SELECT name FROM team where status != 'Удален';", cn);
            DataTable dt3 = new DataTable();
            dt3.Load(cmd3.ExecuteReader());
            foreach (DataRow item in dt3.Rows)
            {
                teamT.Items.Add(item[0].ToString());

            }
            cn.Close();

        }

        void load()
        {
            racesListD.Clear();
            cn.Open();
            DataTable dt1 = new DataTable();
            if (granTypeCheck.ToString().Split(new char[] { ' ' }, 2)[0] == "Практика")
            {
                MySqlCommand cmd1 = new MySqlCommand("SELECT photos.photo pp, tracks.name t,team.name te,practices.id i,pilots.number pn,firstName,lastName,`km/h`,time,practices.circles c FROM practices left join pilots on idPilot =pilots.id left join team on practices.idTeam =team.id left join tracks on idTrack = tracks.id left join bolides on bolides.id = idBolide left join photos on bolides.photo = photos.id where practices.number = " +
                    "'"+ granTypeCheck.ToString().Split(new char[] { ' ' }, 2)[1].ToString() + "' and grandPrix = '" + granRaceCheck+ "'  ", cn);               
                dt1.Load(cmd1.ExecuteReader());
            }         
            else if (granTypeCheck == "Квалификация")
            {
                MySqlCommand cmd1 = new MySqlCommand("SELECT photos.photo pp, pilots.number pn,firstName,lastName,tracks.name t,team.name te,qualifications.id i,`km/h`,time,place,qualifications.circles c FROM qualifications left join pilots on idPilot =pilots.id left join team on qualifications.idTeam =team.id left join tracks on idTrack = tracks.id left join bolides on bolides.id = idBolide left join photos on bolides.photo = photos.id " +
                    "where grandPrix = '" + granRaceCheck + "'   ", cn);             
                dt1.Load(cmd1.ExecuteReader());
            }
            else if (granTypeCheck == "Гонка")
            {
                MySqlCommand cmd1 = new MySqlCommand("SELECT photos.photo pp, pilots.number pn,firstName,lastName,tracks.name t,team.name te,races.id i,`pit`,`km/h`,time,place,races.circles c,accident FROM races left join pilots on idPilot =pilots.id left join team on races.idTeam =team.id left join tracks on idTrack = tracks.id left join bolides on bolides.id = idBolide left join photos on bolides.photo = photos.id " +
                    "where grandPrix = '" + granRaceCheck + "'  ", cn);               
                dt1.Load(cmd1.ExecuteReader());               
            }
            cn.Close();          
            foreach (DataRow item in dt1.Rows)
            {
                if (granTypeCheck.ToString().Split(new char[] { ' ' }, 2)[0] == "Практика")
                {
                    byte[] imgData = (byte[])item["pp"];
                    MemoryStream ms = new MemoryStream(imgData);     
                    racesListD.Add(new racesGrid()
                    {
                        photo = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad),
                        id = item["i"].ToString(),
                        name = item["pn"].ToString() + " " + item["firstName"].ToString() + " " + item["lastName"].ToString(),
                        team = item["te"].ToString(),
                        track = item["t"].ToString(),
                        km = item["km/h"].ToString(),
                        time = item["time"].ToString(),
                        circles = item["c"].ToString()
                    });
                }
                else if (granTypeCheck == "Квалификация")
                {
                    byte[] imgData = (byte[])item["pp"];
                    MemoryStream ms = new MemoryStream(imgData);
                    racesListD.Add(new racesGrid()
                    {
                        photo = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad),
                        id = item["i"].ToString(),
                        name = item["pn"].ToString() + " " + item["firstName"].ToString() + " " + item["lastName"].ToString(),
                        team = item["te"].ToString(),
                        track = item["t"].ToString(),
                        km = item["km/h"].ToString(),
                        time = item["time"].ToString(),
                        circles = item["c"].ToString(),
                        place = item["place"].ToString()
                    });
                }
                else if (granTypeCheck == "Гонка")
                {
                    byte[] imgData = (byte[])item["pp"];
                    MemoryStream ms = new MemoryStream(imgData);
                    racesListD.Add(new racesGrid()
                    {
                        photo = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad),
                        id = item["i"].ToString(),
                        name = item["pn"].ToString() + " " + item["firstName"].ToString() + " " + item["lastName"].ToString(),
                        team = item["te"].ToString(),
                        track = item["t"].ToString(),
                        km = item["km/h"].ToString(),
                        time = item["time"].ToString(),
                        circles = item["c"].ToString(),
                        place = item["place"].ToString(),
                        pit = item["pit"].ToString(),
                        accident = item["accident"].ToString()
                    });
                }
            }
            racesList.ItemsSource = null;
            racesList.Items.Clear();
            racesList.ItemsSource = racesListD;
        }
        private void clickAdd(object sender, RoutedEventArgs e)
        {
            if (kmR.Text.Trim() != "" && timeR.Text.Trim() != "" && circlesR.Text.Trim() != "" && trackT.Text.Trim() != "" && pilotT.Text.Trim() != "")
            {
                if (granTypeCheck == "Квалификация")
                {
                    if (placeT.Text.Trim() != "")
                    {
                        try
                        {
                            check("add", "qualifications");
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
                }else if (granTypeCheck == "Гонка")
                {
                    if (pitR.Text.Trim() != "" && accidentR.Text.Trim() != "" && placeT.Text.Trim() != "")
                    {
                        try
                        {
                            check("add", "races");
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
                else if (granTypeCheck.ToString().Split(new char[] { ' ' }, 2)[0] == "Практика")
                {
                    try
                    {
                        check("add", "practices");
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
        private void racesDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (racesList.SelectedItems.Count != 0)
            {
                if (granTypeCheck == "Квалификация")
                {
                    dynamic selectedItem = racesList.SelectedItem;
                    pilotT.Text = selectedItem.name;
                    trackT.Text = selectedItem.track;
                    teamT.Text = selectedItem.team;
                    circlesR.Text = selectedItem.circles;
                    kmR.Text = selectedItem.km;
                    placeT.Text = selectedItem.place;
                    idCheck.Text = selectedItem.id;
                    timeR.Text = selectedItem.time;
                    image.Source = selectedItem.photo;
                }
                else if (granTypeCheck == "Гонка")
                {
                    dynamic selectedItem = racesList.SelectedItem;
                    pilotT.Text = selectedItem.name;
                    trackT.Text = selectedItem.track;
                    teamT.Text = selectedItem.team;
                    circlesR.Text = selectedItem.circles;
                    timeR.Text = selectedItem.time;
                    kmR.Text = selectedItem.km;
                    placeT.Text = selectedItem.place;
                    pitR.Text = selectedItem.pit;
                    idCheck.Text = selectedItem.id;
                    accidentR.Text = selectedItem.accident;
                    image.Source = selectedItem.photo;

                }
                else if (granTypeCheck.ToString().Split(new char[] { ' ' }, 2)[0] == "Практика")
                {
                    dynamic selectedItem = racesList.SelectedItem;
                    pilotT.Text = selectedItem.name;
                    trackT.Text = selectedItem.track;
                    teamT.Text = selectedItem.team;
                    timeR.Text = selectedItem.time;
                    circlesR.Text = selectedItem.circles;
                    kmR.Text = selectedItem.km;
                    idCheck.Text = selectedItem.id;
                    image.Source = selectedItem.photo;

                }               
            }
        }
        void check(string a, string b)
        {
            if(Convert.ToBoolean(timeR.Tag) == false && a !="delete" || Convert.ToBoolean(kmR.Tag) == false && a != "delete")
            {
                MessageBox.Show("Формат км/ч(xxx.xxx) или формат времени(чч:мм'сс.мс) указаны не верно");
                return;
            }
            

          
            
            if (a == "add" )
            {
                if(racesList.Items.Count < 20)
                {
                    if (granTypeCheck == "Квалификация")
                    {
                        cn.Open();
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO `qualifications` (`idPilot`,`idTeam`, `idTrack`, `km/h`, `circles`, `time`, `place`, `grandPrix`) VALUES ((SELECT id FROM pilots where number='" + pilotT.Text.ToString().Split(new char[] { ' ' }, 3)[0] + "' and firstName='" + pilotT.Text.ToString().Split(new char[] { ' ' }, 3)[1] + "'and lastName='" + pilotT.Text.ToString().Split(new char[] { ' ' }, 3)[2] + "'), (SELECT id FROM team where name='" + teamT.Text.ToString() + "'), (SELECT id FROM tracks where name='" + trackT.Text.ToString() + "'), '" + kmR.Text.ToString().Replace(",", ".") + "', " +
                            "'" + circlesR.Text.ToString() + "','" + timeR.Text.ToString().Replace(@"'", @"\'") + "','" + placeT.Text.ToString() + "','" + granRaceCheck.ToString() + "');", cn);
                        
                        cmd.ExecuteReader();
                        MessageBox.Show("Данные обновлены");
                    }
                    else if (granTypeCheck == "Гонка")
                    {
                        cn.Open();
                        
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO `races` (`idPilot`,`idTeam`, `idTrack`, `km/h`, `circles`, `time`, `place`,`pit`, `accident`, `grandPrix`) VALUES ((SELECT id FROM pilots where number='" + pilotT.Text.ToString().Split(new char[] { ' ' }, 3)[0] + "' and firstName='" + pilotT.Text.ToString().Split(new char[] { ' ' }, 3)[1] + "'and lastName='" + pilotT.Text.ToString().Split(new char[] { ' ' }, 3)[2] + "'),(SELECT id FROM team where name='" + teamT.Text.ToString() + "'),  (SELECT id FROM tracks where name='" + trackT.Text.ToString() + "'), '" + kmR.Text.ToString().Replace(",", ".") + "', " +
                            "'" + circlesR.Text.ToString() + "','" + timeR.Text.ToString().Replace(@"'", @"\'") + "','" + placeT.Text.ToString() + "','" + pitR.Text.ToString() + "','" + accidentR.Text.ToString() + "','" + granRaceCheck.ToString() + "');", cn);
                       
                        cmd.ExecuteReader();
                        MessageBox.Show("Данные обновлены");

                    }
                    else if (granTypeCheck.ToString().Split(new char[] { ' ' }, 2)[0] == "Практика")
                    {
                        cn.Open();
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO `practices` (`idPilot`,`idTeam`, `idTrack`, `km/h`, `circles`, `time`, `grandPrix`, `number` ) VALUES ((SELECT id FROM pilots where number='" + pilotT.Text.ToString().Split(new char[] { ' ' }, 3)[0] + "' and firstName='" + pilotT.Text.ToString().Split(new char[] { ' ' }, 3)[1] + "'and lastName='" + pilotT.Text.ToString().Split(new char[] { ' ' }, 3)[2] + "'),   (SELECT id FROM team where name='" + teamT.Text.ToString() + "'),(SELECT id FROM tracks where name='" + trackT.Text.ToString() + "'), '" + kmR.Text.ToString().Replace(",", ".") + "', " +
                            "'" + circlesR.Text.ToString() + "','" + timeR.Text.ToString().Replace(@"'", @"\'") + "','" + granRaceCheck.ToString()+ "','" + granTypeCheck.ToString().Split(new char[] { ' ' }, 2)[1].ToString() + "');", cn);
                       
                        cmd.ExecuteReader();
                        MessageBox.Show("Данные обновлены");

                    }
                    

                }else
                {
                    MessageBox.Show("Максимум 20 позиций");
                    return;
                }

                
                
            }
            else if (a == "update" )
            {
                if (granTypeCheck == "Квалификация")
                {
                    cn.Open();
                    MySqlCommand cmd = new MySqlCommand("update `qualifications` set `idPilot`=(SELECT id FROM pilots where number='" + pilotT.Text.ToString().Split(new char[] { ' ' }, 3)[0] + "' and firstName='" + pilotT.Text.ToString().Split(new char[] { ' ' }, 3)[1] + "'and lastName='" + pilotT.Text.ToString().Split(new char[] { ' ' }, 3)[2] + "'),`idTeam`=(SELECT id FROM team where name='" + teamT.Text.ToString() + "'), `idTrack`=(SELECT id FROM tracks where name='" + trackT.Text.ToString() + "'), `circles`= '" +
                        circlesR.Text + "', `time`='" + timeR.Text.Replace(@"'", @"\'") + "',`place`= '" + placeT.Text + "',`km/h`='" + kmR.Text.ToString().Replace(",", ".") + "' where id = '" + idCheck.Text + "'", cn);
                   
                    cmd.ExecuteReader();
                    MessageBox.Show("Данные обновлены");
                }
                else if (granTypeCheck == "Гонка")
                {
                    cn.Open();
                    MySqlCommand cmd5 = new MySqlCommand("update `races` set `idPilot`=(SELECT id FROM pilots where number='" + pilotT.Text.ToString().Split(new char[] { ' ' }, 3)[0] + "' and firstName='" + pilotT.Text.ToString().Split(new char[] { ' ' }, 3)[1] + "'and lastName='" + pilotT.Text.ToString().Split(new char[] { ' ' }, 3)[2] + "'),`idTeam`=(SELECT id FROM team where name='" + teamT.Text.ToString() + "'), `idTrack`=(SELECT id FROM tracks where name='" + trackT.Text.ToString() + "'), `circles`= '" +
                        circlesR.Text + "', `time`='" + timeR.Text.Replace(@"'", @"\'") + "',`place`= '" + placeT.Text + "',`km/h`='" + kmR.Text.ToString().Replace(",", ".") + "',`pit`= '" + pitR.Text.ToString() + "',`accident`= '" + accidentR.Text.ToString() + "' where id = '" + idCheck.Text + "'", cn);
                    
                    cmd5.ExecuteReader();

                    MessageBox.Show("Данные обновлены");

                }
                else if (granTypeCheck.ToString().Split(new char[] { ' ' }, 2)[0] == "Практика")
                {
                    cn.Open();
                    MySqlCommand cmd = new MySqlCommand("update `practices` set `idPilot`=(SELECT id FROM pilots where number='" + pilotT.Text.ToString().Split(new char[] { ' ' }, 3)[0] + "' and firstName='" + pilotT.Text.ToString().Split(new char[] { ' ' }, 3)[1] + "'and lastName='" + pilotT.Text.ToString().Split(new char[] { ' ' }, 3)[2] + "'),`idTeam`=(SELECT id FROM team where name='" + teamT.Text.ToString() + "'), `idTrack`=(SELECT id FROM tracks where name='" + trackT.Text.ToString() + "'),`km/h`='" + kmR.Text.ToString().Replace(",",".") + "', `circles`= '" +
                       circlesR.Text + "', `time`='" + timeR.Text.Replace(@"'", @"\'") + "' where id = '" + idCheck.Text + "'", cn);
                   
                    cmd.ExecuteReader();
                    MessageBox.Show("Данные обновлены");

                }

                

            }
            else if (a == "delete")
            {

                cn.Open();
                MySqlCommand cmd5 = new MySqlCommand("delete from "+ b + " where id = '" + idCheck.Text + "'", cn);

                cmd5.ExecuteReader();
                idCheck.Text = "";
                MessageBox.Show("Данные обновлены");

            }

        }

        private void clickDel(object sender, RoutedEventArgs e)
        {
            if (idCheck.Text.Trim() != "")
            {
                if (granTypeCheck == "Квалификация")
                {
                    try
                    {
                        if (MessageBox.Show("Удалить строку безвозвратно", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            check("delete", "qualifications");
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
                else if (granTypeCheck == "Гонка")
                {
                    try
                    {
                        if (MessageBox.Show("Удалить строку безвозвратно", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            check("delete", "races");
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
                else if (granTypeCheck.ToString().Split(new char[] { ' ' }, 2)[0] == "Практика")
                {
                    try
                    {
                        if (MessageBox.Show("Удалить строку безвозвратно", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            check("delete", "practices");
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
        }


        private void clickUpd(object sender, RoutedEventArgs e)
        {
            if (kmR.Text.Trim() != "" && timeR.Text.Trim() != "" && circlesR.Text.Trim() != "" && trackT.Text.Trim() != "" && pilotT.Text.Trim() != "" && idCheck.Text != "")
            {
                if (granTypeCheck == "Квалификация")
                {
                    if (placeT.Text.Trim() != "")
                    {
                        try
                        {
                            check("update", "qualifications");
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
                else if (granTypeCheck == "Гонка")
                {
                    if (pitR.Text.Trim() != "" && accidentR.Text.Trim() != "" && placeT.Text.Trim() != "")
                    {
                        try
                        {
                            check("update", "races");
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
                else if (granTypeCheck.ToString().Split(new char[] { ' ' }, 2)[0] == "Практика")
                {
                    try
                    {
                        check("update", "practices");
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

        private void backButton(object sender, RoutedEventArgs e)
        {
            MainWindow fm = new MainWindow(2);
            fm.Show();
            this.Close();


        }
        private void charInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^.*\\s*[^A-яA-z].*$");
            e.Handled = regex.IsMatch(e.Text);

        }
        private void intInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^.*\\s*[^0-9].*$");
            e.Handled = regex.IsMatch(e.Text);

        }
        private void kmInput(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex("^[0-9]{1,3}([,.][0-9]{1,3})?$", RegexOptions.Multiline);

            kmR.Tag = regex.IsMatch(kmR.Text);
            
            //".*\\s*[0 - 9][.,][0 - 9]{3,3}?$"





        }
        private void timeInput(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex("([01]?[0-9]|2[0-3])?(:[0-5][0-9]|[0-1])'([0-5][0-9]|[0-1]).[0-9]{1,4}", RegexOptions.Multiline);
            
            timeR.Tag = regex.IsMatch(timeR.Text);
            

        }
      
        void FindAndReplace(object FindText, object ReplaceText, Microsoft.Office.Interop.Word.Application App)
        {
            App.Selection.Find.Execute(ref FindText, true, true, false, false, false, true, false, 1, ref ReplaceText, 2, false, false, false, false);
        }
        private void repButton(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Word.Application App = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document Doc = App.Documents.Add(Visible: true);
            cn.Open();
            try
            {
                DataTable dt2 = new DataTable();
                if (granTypeCheck.ToString().Split(new char[] { ' ' }, 2)[0] == "Практика")
                {
                    MySqlCommand cmd1 = new MySqlCommand("SELECT tracks.name Трасса,team.name  Команда,pilots.number Номер,firstName Имя,lastName Фамилия,`km/h` as 'Км/ч',`time` AS 'Время',practices.circles AS 'Кругов' FROM practices left join pilots on idPilot =pilots.id left join team on practices.idTeam =team.id left join tracks on idTrack = tracks.id left join bolides on bolides.id = idBolide where practices.number = " +
                        "'" + granTypeCheck.ToString().Split(new char[] { ' ' }, 2)[1].ToString() + "' and grandPrix = '" + granRaceCheck + "'  ", cn);
                    dt2.Load(cmd1.ExecuteReader());
                }
                else if (granTypeCheck == "Квалификация")
                {
                    MySqlCommand cmd1 = new MySqlCommand("SELECT pilots.number Номер,firstName Имя,lastName Фамилия,tracks.name Трасса,team.name Команда,`km/h` as 'Км/ч',time Время,place Место,qualifications.circles Кругов c FROM qualifications left join pilots on idPilot =pilots.id left join team on qualifications.idTeam =team.id left join tracks on idTrack = tracks.id left join bolides on bolides.id = idBolide  " +
                        "where grandPrix = '" + granRaceCheck + "'   ", cn);
                    dt2.Load(cmd1.ExecuteReader());
                }
                else if (granTypeCheck == "Гонка")
                {
                    MySqlCommand cmd1 = new MySqlCommand("SELECT pilots.number Номер,firstName Имя,lastName Фамилия,tracks.name Трасса,team.name Команда,`pit` as 'Пит-стопов',`km/h` as 'Км/ч',time Время,place Место,races.circles Кругов,accident Статус FROM races left join pilots on idPilot =pilots.id left join team on races.idTeam =team.id left join tracks on idTrack = tracks.id left join bolides on bolides.id = idBolide " +
                        "where grandPrix = '" + granRaceCheck + "'  ", cn);
                    dt2.Load(cmd1.ExecuteReader());
                }
               
                Doc = App.Documents.Add(System.IO.Directory.GetCurrentDirectory() + @"\shablon.docx");
                Doc.Activate();
                //Doc = App.Documents.Open(@"shablon.docx");
                FindAndReplace("name", D.Text, App);
                FindAndReplace("date1", granRaceCheck, App);
                FindAndReplace("date2", granTypeCheck, App);
               
                FindAndReplace("dataS", DateTime.Today, App);

                Microsoft.Office.Interop.Word.Table Tab = Doc.Tables[1];
                for (int i = 0; i <= dt2.Rows.Count - 1; i++)
                {
                    Tab.Rows.Add();
                }
                for (int i = 0; i < dt2.Columns.Count - 1; i++)
                {
                    Tab.Columns.Add();
                }
                Tab.Borders.Enable = 1;
                int Row = 0, Colums = 0, RowS = 0, Count = 0;
                foreach (Microsoft.Office.Interop.Word.Row row in Tab.Rows)
                {
                    object[] sa = dt2.Rows[RowS].ItemArray;
                    Colums = 0;
                    Row = 0;
                    foreach (Microsoft.Office.Interop.Word.Cell item in row.Cells)
                    {
                        if (item.RowIndex == 1)
                        {
                            item.Range.Text = dt2.Columns[Colums].ColumnName;
                            Colums++;
                        }
                        else
                        {
                            item.Range.Text = sa[Row].ToString();
                            Row++;
                        }
                    }
                    Count++;
                    RowS++;
                    if (Count == RowS)
                        RowS = RowS - 1;
                }
                cn.Close();
                SaveFileDialog fm = new SaveFileDialog();
                fm.Filter = "DOCX files(*.docx)|*.docx|All files(*.*)|*.*";
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.FileName = "Отчет";
                saveFileDialog.DefaultExt = ".docx";
                saveFileDialog.Filter = "DOCX files(*.docx)|*.docx|All files(*.*)|*.*";
                saveFileDialog.ShowDialog();
                Doc.SaveAs2(saveFileDialog.FileName);
            }
            catch (Exception ex) { MessageBox.Show("Ошибка Word" + ex.ToString()); }
            finally { Doc.Close(); }

        }
    }
    

}
