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
        }

        private void GetInventory()
        {
            Books book = new Books();
            string con = @"Data Source=MasterBlaster\SQLEXPRESS;Initial Catalog=MyBookshelf;Integrated Security=True";
            using (SqlConnection myConnect = new SqlConnection(con))
            {
                string sql = "SELECT * FROM BookInventory WHERE UserId = @id";
                SqlCommand cmd = new SqlCommand(sql, myConnect);
                cmd.Parameters.AddWithValue("@id", 1);
                myConnect.Open();

                using(SqlDataReader rd = cmd.ExecuteReader())
                {
                    while(rd.Read())
                    {
                        BookIn.RowDefinitions.Add(new RowDefinition());
                        int num = BookIn.RowDefinitions.Count;
                        if (num != 0)
                        {
                            num -= 1;
                        }
                        BookIn.RowDefinitions[num].Height = new GridLength(175);
                        
                        StackPanel sp = new StackPanel();
                        sp.Orientation = Orientation.Vertical;
                        TextBlock bk = new TextBlock
                        {
                            Name = "BksLabel",
                            Text = "Title: " + rd["BookTitle"].ToString(),
                            FontSize = 20,
                            Margin = new Thickness(10, 10, 0, 0),
                            FontFamily = new FontFamily("Moonbeam")
                        };
                       
                        TextBlock au = new TextBlock
                        {
                            Name = "AuthLabel",
                            Text = "Author: " + rd["BookAuthor"].ToString(),
                            FontSize = 20,
                            Margin = new Thickness(10, 10, 0, 0),
                            FontFamily = new FontFamily("Moonbeam"),
                            TextWrapping = TextWrapping.Wrap
                        };
                       
                        TextBlock format = new TextBlock
                        {
                            Name = "ForLabel",
                            Text = "Format: " + rd["Format"].ToString(),
                            FontSize = 20,
                            Margin = new Thickness(10, 10, 0, 0),
                            FontFamily = new FontFamily("Moonbeam"),
                            TextWrapping = TextWrapping.Wrap
                        };
                        
                        TextBlock isbn = new TextBlock
                        {
                            Name = "isbnLabel",
                            Text = "ISBN: " + rd["ISBN"].ToString(),
                            FontSize = 20,
                            Margin = new Thickness(10, 10, 0, 0),
                            FontFamily = new FontFamily("Moonbeam"),
                            TextWrapping = TextWrapping.Wrap
                        };
                        
                        TextBlock notes = new TextBlock
                        {
                            Name = "NotesLabel",
                            Text = "Notes: " + rd["Notes"].ToString(),
                            FontSize = 20,
                            Margin = new Thickness(10, 10, 0, 0),
                            FontFamily = new FontFamily("Moonbeam"),
                            TextWrapping = TextWrapping.Wrap
                        };
                        
                        TextBlock tags = new TextBlock
                        {
                            Name = "TagLabel",
                            Text = "Tags:\n" + rd["Tags"].ToString(),
                            FontSize = 20,
                            Margin = new Thickness(0, 0, 30, 0),
                            FontFamily = new FontFamily("Moonbeam"),
                            TextWrapping = TextWrapping.Wrap
                        };

                        Image covers = new Image
                        {
                            Name = "Covers",
                            Height = 200,
                            Width = 150,
                            Margin = new Thickness(20,20,20,20)
                        };

                        byte[] by = GetCovers(rd["Cover"]);

                        Grid.SetRow(tags, num);
                        Grid.SetColumn(tags, 2);

                        Grid.SetRow(covers, num);
                        Grid.SetColumn(covers, 0);

                        Grid.SetRow(sp, num);
                        Grid.SetColumn(sp, 1);

                        sp.Children.Add(bk);
                        sp.Children.Add(au);
                        sp.Children.Add(format);
                        sp.Children.Add(isbn);
                        sp.Children.Add(notes);

                        BookIn.Children.Add(sp);
                        BookIn.Children.Add(tags);

                    }
                }
            }
        }

        private byte[] GetCovers(Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();

        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            GetInventory();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            NewBook nb = new NewBook(userId);
            nb.Show();
        }
    }
}
