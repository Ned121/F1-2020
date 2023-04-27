using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
	static class D
	{
		public static string Text { get; set; }
	}

	public partial class authWindow : Window
    {
        public authWindow()
        {
            InitializeComponent();
        }
		List<string[]> data = new List<string[]>();
		MySqlConnection cn = new MySqlConnection("server = 10.0.7.240; user id = user8; port=3306;persistsecurityinfo=True;database=user8;password=wsruser8");
		string Pass = "", Role = "";
		int count = 0;
		public static string text = string.Empty;
		
		public static Bitmap SetBitmap(int Width, int Height)//Капча
		{
			Random rnd = new Random();
			Bitmap result = new Bitmap(Width, Height);
			int Xpos = rnd.Next(0, Width - 100);
			int Ypos = rnd.Next(15, Height - 15);
			Brush[] colors = { Brushes.Black, Brushes.Red, Brushes.RoyalBlue, Brushes.Green };
			Graphics g = Graphics.FromImage((System.Drawing.Image)result);
			g.Clear(Color.Gray);
			text = String.Empty;
			string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
			for (int i = 0; i < 5; ++i)
				text += ALF[rnd.Next(ALF.Length)];
			g.DrawString(text,
			new Font("Arial", 20),
			colors[rnd.Next(colors.Length)],
			new PointF(Xpos, Ypos));
			g.DrawLine(Pens.Black,
			new System.Drawing.Point(0, 0),
			new System.Drawing.Point(Width - 1, Height - 1));
			g.DrawLine(Pens.Black,
			new System.Drawing.Point(0, Height - 1),
			new System.Drawing.Point(Width - 1, 0));
			for (int i = 0; i < Width; ++i)
				for (int j = 0; j < Height; ++j)
					if (rnd.Next() % 20 == 0)
						result.SetPixel(i, j, Color.White);

			return result;
		}
		public static BitmapImage Convert2(Bitmap src)
		{
			MemoryStream ms = new MemoryStream();
			((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
			BitmapImage image = new BitmapImage();
			image.BeginInit();
			ms.Seek(0, SeekOrigin.Begin);
			image.StreamSource = ms;
			image.EndInit();
			return image;
		}


		private void buttonAuthorizClick(object sender, RoutedEventArgs e)
		{
			if (loginBox.Text != "" && pasBox.Password != "")
			{
				if (capIm.Visibility == Visibility.Hidden || texboxCap.Text.ToLower() == text.ToLower())
				{
					MySqlCommand cmd = new MySqlCommand("SELECT password,role,firstName,lastName FROM users where email='" + loginBox.Text + "' and Password='" + pasBox.Password + "' and Status='Работает';", cn);
					try
					{
						cn.Open();
						MySqlDataReader read = cmd.ExecuteReader();
						if (read.Read())
						{
							Pass = read[0].ToString();
							Role = read[1].ToString();
							switch (read[1].ToString())
							{
								case "Администратор":
									{
										
										adminWindow wind = new adminWindow();
										wind.Show();
										this.Hide();
										break;
									}
								case "Менеджер Р":
									{
										MainWindow wind = new MainWindow(2);
										wind.Show();
										this.Hide();
										D.Text = read[3].ToString() + " "+ read[2].ToString();
										break;
									}
								case "Менеджер К":
									{
										MainWindow wind = new MainWindow(1);
										wind.Show();
										this.Hide();
										break;
									}

							}
						}
						else
						{
							MessageBox.Show("Логин или пароль неверен!!!");
							count++;
						}
						capIm.Source = Convert2(SetBitmap(Convert.ToInt32(capIm.Width), Convert.ToInt32(capIm.Height)));
						if (count == 2)
                        {
							capIm.Visibility = Visibility.Visible;
							texboxCap.Visibility = Visibility.Visible;

						}
					}
					catch (Exception Ex)
					{
						MessageBox.Show("Ошибка подключения к БД!!!" + Ex.ToString());
					}
					finally { cn.Close(); }
                }
                else if (texboxCap.Text.ToLower() != text.ToLower())
                {
					MessageBox.Show("Введите капчу верно");
					capIm.Source = Convert2(SetBitmap(Convert.ToInt32(capIm.Width), Convert.ToInt32(capIm.Height)));
				}	
				
				
			}
			else
			{
				MessageBox.Show("Заполните поле Логина и Пароля");
			}
		}
		
		
	}
}
