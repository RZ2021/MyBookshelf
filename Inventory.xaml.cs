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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Drawing;

namespace MyBookshelf
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class Inventory : Window
    {
        private int userId;
        private byte[] cover;

        public Inventory(int user)
        {
            InitializeComponent();
            userId = user;
            GetInventory();
        }

        private void GetInventory()
        {
            
            string con = @"Data Source=MasterBlaster\SQLEXPRESS;Initial Catalog=MyBookshelf;Integrated Security=True";
            string sql = "SELECT * FROM BookInventory WHERE UserId = @id";

            using (SqlConnection myConnect = new SqlConnection(con))
            {
                SqlCommand cmd = new SqlCommand(sql, myConnect);
                cmd.Parameters.AddWithValue("@id", 1);
                myConnect.Open();

                using(SqlDataReader rd = cmd.ExecuteReader())
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
                        string isbn = "ISBN: " + rd["ISBN"].ToString();
                        string notes = "Notes: " + rd["Notes"].ToString();
                        string tags = "Tags:\n" + rd["Tags"].ToString();
                        
                        if(rd["Cover"] != DBNull.Value)
                        {
                            cover = (byte[])rd["Cover"];

                        }

                        GetRows(bk, author, format, isbn, notes, tags, cover, num);

                    }

                }
            }
        }

        private void GetRows(string title, string author, string format, string isbn, string notes, string tags, byte[] covers, int num)
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

            TextBlock Isbn = new TextBlock
            {
                Name = "isbnLabel",
                Text = isbn,
                FontSize = 20,
                Margin = new Thickness(10, 10, 0, 0),
                FontFamily = new System.Windows.Media.FontFamily("Moonbeam"),
                TextWrapping = TextWrapping.Wrap
            };

            TextBlock Notes = new TextBlock
            {
                Name = "NotesLabel",
                Text = notes,
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
            sp.Children.Add(Isbn);
            sp.Children.Add(Notes);

            BookIn.Children.Add(sp);
            BookIn.Children.Add(Tags);
            BookIn.Children.Add(Covers);
        }

        private void ClearRows()
        {
            BookIn.RowDefinitions.Clear();
            BookIn.Children.Clear();
        }

        
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            ClearRows();

            ComboBoxItem item = SearchTerm.SelectedItem as ComboBoxItem;
            string choice = item.Content.ToString();
            string search = SearchBox.Text;

            if (SearchTerm.SelectedIndex > -1)
            {
                string sql;
                if (choice == "Title")
                {
                    sql = "SELECT * FROM BookInventory WHERE UserId = @id AND BookTitle Like @search";
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
                                string isbn = "ISBN: " + rd["ISBN"].ToString();
                                string notes = "Notes: " + rd["Notes"].ToString();
                                string tags = "Tags:\n" + rd["Tags"].ToString();

                                if (rd["Cover"] != DBNull.Value)
                                {
                                    cover = (byte[])rd["Cover"];

                                }

                                GetRows(bk, author, format, isbn, notes, tags, cover, num);

                            }

                        }
                    }
                    GetInventory();
                }
                else if (choice == "Author")
                {
                    sql = "SELECT * FROM BookInventory WHERE UserId = @id AND BookAuthor = @au";
                    GetInventory();
                }
                else if (choice == "Format")
                {
                    sql = "SELECT * FROM BookInventory WHERE UserId = @id AND BookTitle = @for";
                    GetInventory();
                }
                else if (choice == "ISBN")
                {
                    sql = "SELECT * FROM BookInventory WHERE UserId = @id AND ISBN = @isbn";
                    GetInventory();
                }
                else if (choice == "Notes")
                {
                    sql = "SELECT * FROM BookInventory WHERE UserId = @id AND Notes = @note";
                    GetInventory();
                }
                else if (choice == "Tags")
                {
                    sql = "SELECT * FROM BookInventory WHERE UserId = @id AND Tags = @tags";
                    GetInventory();
                }

            }
            else
            {
                MessageBox.Show("Please choose a search term");
            }


        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            NewBook nb = new NewBook(userId);
            nb.Show();
        }

        private void TitleLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ClearRows();
            GetInventory();
        }
    }
}
