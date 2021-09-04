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

namespace MyBookshelf
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class Inventory : Window
    {
        public Inventory()
        {
            InitializeComponent();
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
                        BookIn.RowDefinitions[num].Height = new GridLength(150);

                        StackPanel sp = new StackPanel();

                        Label bk = new Label
                        {
                            Name = "BksLabel",
                            Content = rd["BookTitle"].ToString(),
                            FontSize = 16,
                            Margin = new Thickness(0, 0, 0, 0),
                            HorizontalContentAlignment = HorizontalAlignment.Left,
                            FontFamily = new FontFamily("Moonbeam")

                        };
                        Grid.SetRow(bk, num);
                        Grid.SetColumn(bk, 1);

                        Label au = new Label
                        {
                            Name = "AuthLabel",
                            Content = rd["BookAuthor"].ToString(),
                            FontSize = 16,
                            Margin = new Thickness(50, 0, 0, 0),
                            FontFamily = new FontFamily("Moonbeam")

                        };
                        Grid.SetRow(au, num);
                        Grid.SetColumn(au, 1);

                        Label format = new Label
                        {
                            Name = "ForLabel",
                            Content = rd["Format"].ToString(),
                            FontSize = 16,
                            Margin = new Thickness(50, 500, 0, 0),
                            FontFamily = new FontFamily("Moonbeam")

                        };
                        Grid.SetRow(format, num);
                        Grid.SetColumn(format, 1);

                        Label isbn = new Label
                        {
                            Name = "isbnLabel",
                            Content = rd["ISBN"].ToString(),
                            FontSize = 16,
                            Margin = new Thickness(0, 50, 0, 0),
                            FontFamily = new FontFamily("Moonbeam")

                        };
                        Grid.SetRow(isbn, num);
                        Grid.SetColumn(isbn, 1);

                        Label notes = new Label
                        {
                            Name = "NotesLabel",
                            Content = rd["Notes"].ToString(),
                            FontSize = 16,
                            Margin = new Thickness(0, 100, 0, 0),
                            FontFamily = new FontFamily("Moonbeam")

                        };
                        Grid.SetRow(notes, num);
                        Grid.SetColumn(notes, 1);

                        Label tags = new Label
                        {
                            Name = "TagLabel",
                            Content = rd["Tags"].ToString(),
                            FontSize = 16,
                            Margin = new Thickness(0, 0, 0, 0),
                            FontFamily = new FontFamily("Moonbeam")
                            

                        };
                        Grid.SetRow(tags, num);
                        Grid.SetColumn(tags, 2);
                        BookIn.Children.Add(sp);
                        BookIn.Children.Add(bk);
                        BookIn.Children.Add(au);
                        BookIn.Children.Add(format);
                        BookIn.Children.Add(isbn);
                        BookIn.Children.Add(notes);
                        BookIn.Children.Add(tags);
                    }
                }
            }
            


        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            GetInventory();
        }
    }
}
