using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookshelf
{
    public class Books
    {
        public string BookTitle
        {
            get;
            set;
        }

        public string BookAuthor
        {
            get;
            set;
        }
        public string BookFormat { get; set; }
        public string BookIsbn { get; set; }
        public string BookNotes { get; set; }
        public string BookTags { get; set; }

    }
}
