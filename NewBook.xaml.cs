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
    /// Interaction logic for NewBook.xaml
    /// </summary>
    public partial class NewBook : Window
    {
        private int userId;
        public NewBook(int user)
        {
            InitializeComponent();
            userId = user;
        }

        private void CoverButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog pic = new Microsoft.Win32.OpenFileDialog();
            pic.Title = "Select the cover";
            pic.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png";
            Nullable<bool> result = pic.ShowDialog();
            if(result == true)
            {
                Cover.ImageSource = new BitmapImage(new Uri(pic.FileName));
                
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string title = BookTitle.Text;
            string author = BookAuthor.Text;
            var item = FormatBox.SelectedItem as ComboBoxItem;
            string format = item.Content.ToString();
            int isbn = Convert.ToInt32(IsbnBox.Text);
            string notes = NotesBox.Text;

            List<string> BookTags = new List<string>();
            foreach(var check in TagsBox.SelectedItems.ToString())
            {
                BookTags.Add(check.ToString());
            }
            string tags = String.Join(",", BookTags);
            byte[] cover = GetCover((BitmapSource)Cover.ImageSource);
            

            string server = @"Data Source=MasterBlaster\SQLEXPRESS;Initial Catalog=MyBookshelf;Integrated Security=True";
            using(SqlConnection con = new SqlConnection(server))
            {
                string sql = "INSERT INTO BookInventory (UserId, Booktitle, BookAuthor, Format, ISBN, Notes, Tags, Cover) VALUES (@id, @bt, @ba," +
                    "@for, @isbn, @note, @tag, @cov)";

                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", userId);
                cmd.Parameters.AddWithValue("@bt", title);
                cmd.Parameters.AddWithValue("@ba", author);
                cmd.Parameters.AddWithValue("@for", format);
                cmd.Parameters.AddWithValue("@isbn", isbn);
                cmd.Parameters.AddWithValue("@note", notes);
                cmd.Parameters.AddWithValue("@tag", tags);
                cmd.Parameters.AddWithValue("@cov", cover);
                int result = cmd.ExecuteNonQuery();
                con.Close();
                if(result > 0)
                {
                    MessageBox.Show("Your book has been added!");
                }

            }
        }

        private byte[] GetCover(BitmapSource cover)
        {
            using(var stream = new MemoryStream())
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(cover));
                encoder.Save(stream);
                return stream.ToArray();
            }
        }
    }
}
