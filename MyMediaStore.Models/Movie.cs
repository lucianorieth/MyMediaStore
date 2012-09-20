using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MyMediaStore.Models
{
    public class Movie
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Sinopse { get; set; }
        public string Capa { get; set; }
        public string Uri { get; set; }
        public List<Category> Categories { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Format { get; set; }
    }
}
