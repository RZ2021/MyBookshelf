using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;

namespace MyBookshelf
{
    /// <summary>
    /// Interaction logic for Wishlist.xaml
    /// </summary>
    public partial class Wishlist : Window
    {
        private int userId;
        private byte[] cover;

        public Wishlist()
        {
            InitializeComponent();
        }

        private void GetInventory()
        {

            string con = @"Data Source=MasterBlaster\SQLEXPRESS;Initial Catalog=MyBookshelf;Integrated Security=True";
            string sql = "SELECT * FROM Wishlist WHERE UserId = @id";

            using (SqlConnection myConnect = new SqlConnection(con))
            {
                SqlCommand cmd = new SqlCommand(sql, myConnect);
                cmd.Parameters.AddWithValue("@id", 1);
                myConnect.Open();

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        BookIn.RowDefinitions.Add(new RowDefinition());
                        int num = BookIn.RowDefinitions.Count;
                        if (num != 0)
                        {
                            num -= 1;
                        }

                        string bk = "Title: " + rd["BookTitle"].ToString();
                        string author = "Author: " + rd["BookAuthor"].ToString();
                        string format = "Format: " + rd["Format"].ToString();
                        string why = "Why: " + rd["Why"].ToString();
                        string tags = "Tags:\n" + rd["Tags"].ToString();

                        if (rd["Cover"] != DBNull.Value)
                        {
                            cover = (byte[])rd["Cover"];

                        }

                        GetRows(bk, author, format, why, tags, cover, num);

                    }

                }
            }
        }

        private void GetRows(string title, string author, string format, string why, string tags, byte[] covers, int num)
        {
            BookIn.RowDefinitions[num].Height = new GridLength(200);

            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Vertical;

            TextBlock bk = new TextBlock
            {
                Name = "BksLabel",
                Text = title,
                FontSize = 20,
                Margin = new Thickness(10, 10, 0, 0),
                FontFamily = new System.Windows.Media.FontFamily("Moonbeam"),
                TextWrapping = TextWrapping.Wrap
            };

            TextBlock Author = new TextBlock
            {
                Name = "AuthLabel",
                Text = author,
                FontSize = 20,
                Margin = new Thickness(10, 10, 0, 0),
                FontFamily = new System.Windows.Media.FontFamily("Moonbeam"),
                TextWrapping = TextWrapping.Wrap
            };

            TextBlock Format = new TextBlock
            {
                Name = "ForLabel",
                Text = format,
                FontSize = 20,
                Margin = new Thickness(10, 10, 0, 0),
                FontFamily = new System.Windows.Media.FontFamily("Moonbeam"),
                TextWrapping = TextWrapping.Wrap
            };

            TextBlock Why = new TextBlock
            {
                Name = "whyLabel",
                Text = why,
                FontSize = 20,
                Margin = new Thickness(10, 10, 0, 0),
                FontFamily = new System.Windows.Media.FontFamily("Moonbeam"),
                TextWrapping = TextWrapping.Wrap
            };

            

            TextBlock Tags = new TextBlock
            {
                Name = "TagLabel",
                Text = tags,
                FontSize = 20,
                Margin = new Thickness(0, 0, 30, 0),
                FontFamily = new System.Windows.Media.FontFamily("Moonbeam"),
                TextWrapping = TextWrapping.Wrap
            };

            System.Windows.Controls.Image Covers = new System.Windows.Controls.Image
            {
                Name = "Covers",
                Height = 200,
                Width = 125,
                Margin = new Thickness(20, 20, 20, 20),
                Stretch = Stretch.Fill
            };

            Bitmap cov;
            using (var ms = new MemoryStream(covers))
            {
                cov = new Bitmap(ms);
            }

            using (var ms = new MemoryStream())
            {
                cov.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;

                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = ms;
                bi.EndInit();
                Covers.Source = bi;
            }

            Grid.SetRow(Tags, num);
            Grid.SetColumn(Tags, 2);

            Grid.SetRow(Covers, num);
            Grid.SetColumn(Covers, 0);

            Grid.SetRow(sp, num);
            Grid.SetColumn(sp, 1);

            sp.Children.Add(bk);
            sp.Children.Add(Author);
            sp.Children.Add(Format);
            sp.Children.Add(Why);

            BookIn.Children.Add(sp);
            BookIn.Children.Add(Tags);
            BookIn.Children.Add(Covers);

        }

        private void ClearRows()
        {
            BookIn.RowDefinitions.Clear();
            BookIn.Children.Clear();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            ClearRows();

            string search = SearchBox.Text;
            string sql = "SELECT * FROM Wishlist WHERE UserId = @id AND BookTitle + BookAuthor + Format +" +
                "Why + Tags Like @search";
            string con = @"Data Source=MasterBlaster\SQLEXPRESS;Initial Catalog=MyBookshelf;Integrated Security=True";

            using (SqlConnection myConnect = new SqlConnection(con))
            {
                SqlCommand cmd = new SqlCommand(sql, myConnect);
                cmd.Parameters.AddWithValue("@id", 1);
                cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                myConnect.Open();

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        BookIn.RowDefinitions.Add(new RowDefinition());
                        int num = BookIn.RowDefinitions.Count;
                        if (num != 0)
                        {
                            num -= 1;
                        }

                        string bk = "Title: " + rd["BookTitle"].ToString();
                        string author = "Author: " + rd["BookAuthor"].ToString();
                        string format = "Format: " + rd["Format"].ToString();
                        string why = "Why: " + rd["Why"].ToString();
                        string tags = "Tags:\n" + rd["Tags"].ToString();

                        if (rd["Cover"] != DBNull.Value)
                        {
                            cover = (byte[])rd["Cover"];

                        }

                        GetRows(bk, author, format, why, tags, cover, num);

                    }

                }
            }
        }

        private void TitleLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            GetInventory();
            SearchBox.Text = "";
        }
    }
}
