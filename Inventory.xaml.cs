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
                        BookIn.RowDefinitions[num].Height = new GridLength(200);

                        StackPanel sp = new StackPanel();
                        sp.Orientation = Orientation.Vertical;
                        TextBlock bk = new TextBlock
                        {
                            Name = "BksLabel",
                            Text = "Title: " + rd["BookTitle"].ToString() + "   Author: " + rd["BookAuthor"].ToString(),
                            FontSize = 20,
                            Margin = new Thickness(10, 10, 0, 0),
                            FontFamily = new System.Windows.Media.FontFamily("Moonbeam")
                        };

                        //TextBlock au = new TextBlock
                        //{
                        //    Name = "AuthLabel",
                        //    Text = "\tAuthor: " + rd["BookAuthor"].ToString(),
                        //    FontSize = 20,
                        //    Margin = new Thickness(10, 10, 0, 0),
                        //    FontFamily = new System.Windows.Media.FontFamily("Moonbeam"),
                        //    TextWrapping = TextWrapping.Wrap
                        //};

                        TextBlock format = new TextBlock
                        {
                            Name = "ForLabel",
                            Text = "Format: " + rd["Format"].ToString(),
                            FontSize = 20,
                            Margin = new Thickness(10, 10, 0, 0),
                            FontFamily = new System.Windows.Media.FontFamily("Moonbeam"),
                            TextWrapping = TextWrapping.Wrap
                        };

                        TextBlock isbn = new TextBlock
                        {
                            Name = "isbnLabel",
                            Text = "ISBN: " + rd["ISBN"].ToString(),
                            FontSize = 20,
                            Margin = new Thickness(10, 10, 0, 0),
                            FontFamily = new System.Windows.Media.FontFamily("Moonbeam"),
                            TextWrapping = TextWrapping.Wrap
                        };

                        TextBlock notes = new TextBlock
                        {
                            Name = "NotesLabel",
                            Text = "Notes: " + rd["Notes"].ToString(),
                            FontSize = 20,
                            Margin = new Thickness(10, 10, 0, 0),
                            FontFamily = new System.Windows.Media.FontFamily("Moonbeam"),
                            TextWrapping = TextWrapping.Wrap
                        };

                        TextBlock tags = new TextBlock
                        {
                            Name = "TagLabel",
                            Text = "Tags:\n" + rd["Tags"].ToString(),
                            FontSize = 20,
                            Margin = new Thickness(0, 0, 30, 0),
                            FontFamily = new System.Windows.Media.FontFamily("Moonbeam"),
                            TextWrapping = TextWrapping.Wrap
                        };

                        System.Windows.Controls.Image covers = new System.Windows.Controls.Image
                        {
                            Name = "Covers",
                            Height = 200,
                            Width = 125,
                            Margin = new Thickness(20, 20, 20, 20),
                            Stretch = Stretch.Fill
                        };


                        if (rd["Cover"] != DBNull.Value)
                        {
                            Bitmap cov;
                            using (var ms = new MemoryStream((byte[])rd["Cover"]))
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
                                covers.Source = bi;
                            }
                        }

                        Grid.SetRow(tags, num);
                        Grid.SetColumn(tags, 2);

                        Grid.SetRow(covers, num);
                        Grid.SetColumn(covers, 0);

                        Grid.SetRow(sp, num);
                        Grid.SetColumn(sp, 1);

                        sp.Children.Add(bk);
                        sp.Children.Add(format);
                        sp.Children.Add(isbn);
                        sp.Children.Add(notes);

                        BookIn.Children.Add(sp);
                        BookIn.Children.Add(tags);
                        BookIn.Children.Add(covers);

                    }

                }
            }
        }

        private void ClearRows()
        {
            BookIn.RowDefinitions.Clear();
            BookIn.Children.Clear();
        }

        
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            ClearRows();

            //ComboBoxItem item = SearchTerm.SelectedItem as ComboBoxItem;
            //string choice = item.Content.ToString();
            //string search = SearchBox.Text;

            //if (SearchTerm.SelectedIndex > -1)
            //{
            //    string sql;
            //    if (choice == "Title")
            //    {
            //        sql = "SELECT * FROM BookInventory WHERE UserId = @id AND BookTitle = @title";
            //        GetInventory();
            //    }
            //    else if (choice == "Author")
            //    {
            //        sql = "SELECT * FROM BookInventory WHERE UserId = @id AND BookAuthor = @au";
            //        GetInventory();
            //    }
            //    else if (choice == "Format")
            //    {
            //        sql = "SELECT * FROM BookInventory WHERE UserId = @id AND BookTitle = @for";
            //        GetInventory();
            //    }
            //    else if (choice == "ISBN")
            //    {
            //        sql = "SELECT * FROM BookInventory WHERE UserId = @id AND ISBN = @isbn";
            //        GetInventory();
            //    }
            //    else if (choice == "Notes")
            //    {
            //        sql = "SELECT * FROM BookInventory WHERE UserId = @id AND Notes = @note";
            //        GetInventory();
            //    }
            //    else if (choice == "Tags")
            //    {
            //        sql = "SELECT * FROM BookInventory WHERE UserId = @id AND Tags = @tags";
            //        GetInventory();
            //    }

            //}
            //else
            //{
            //    MessageBox.Show("Please choose a search term");
            //}
            

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
