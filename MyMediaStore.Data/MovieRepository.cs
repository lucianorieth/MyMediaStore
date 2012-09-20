
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MyMediaStore.Models;

namespace MyMediaStore.Data
{
    public class MovieRepository
    {
        public static List<Movie> LoadAll()
        {

            List<Movie> list = new List<Movie>();

            var consulta = from c in XElement.Load(@"movies.xml").Elements("row").Elements("movie")
                           orderby c.Element("id").Value descending 
                           select c;

            foreach (var registro in consulta)
            {
                Movie movie = new Movie()
                                  {
                                      Id = registro.Element("id").Value,
                                      Title = registro.Element("title").Value,
                                      Sinopse = registro.Element("sinopse").Value,
                                      Capa = registro.Element("capa").Value,
                                      Type = registro.Element("type").Value,
                                      Uri = registro.Element("uri").Value,
                                      Status = registro.Element("status").Value,
                                      Format = registro.Element("format").Value
                                  };

                list.Add(movie);
            }
            return list;
        }

        public void Insert(Movie movie)
        {
            XDocument xmlDoc = XDocument.Load("movies.xml");

            StringBuilder categorias = new StringBuilder();
            int countCategories = 0;
            foreach (var category in movie.Categories)
            {
                categorias.Append(category.Description);
                if (movie.Categories.Count > 1 && countCategories < movie.Categories.Count -1)
                    categorias.Append(",");

                countCategories++;
            }

            if (string.IsNullOrEmpty(movie.Id))
                movie.Id = GetCurrentID();

            xmlDoc.Element("db").Element("row").Add(new XElement("movie",
                                                                 new XElement("id", movie.Id),
                                                                 new XElement("title", movie.Title),
                                                                 new XElement("sinopse", movie.Sinopse),
                                                                 new XElement("capa", movie.Capa),
                                                                 new XElement("uri", movie.Uri),
                                                                 new XElement("type", movie.Type),
                                                                 new XElement("status", movie.Status),
                                                                 new XElement("categories", categorias.ToString()),
                                                                 new XElement("format", movie.Format)));

            xmlDoc.Save("movies.xml");

        }

        public string GetCurrentID()
        {
            string lastId = "0";

            try
            {
                var consulta = from c in XElement.Load(@"movies.xml").Elements("row")
                               select c.Elements("movie").Last();


                foreach (var registro in consulta)
                {
                    lastId = registro.Element("id").Value;
                }

                try
                {
                    //Tentar converter para somar
                    lastId = (Convert.ToInt32(lastId) + 1).ToString();
                }
                catch
                {
                    lastId = "1";
                }
            }
            catch
            {

                lastId = "1";
            }

            return lastId;
        }

        public static List<Category> GetMovieCategories(string id)
        {
            List<Category> list = new List<Category>();

            var consulta = from c in XElement.Load(@"movies.xml").Elements("row").Elements("movie")
                           where (string)c.Element("id") == id
                           select c;

            foreach (var registro in consulta)
            {
                string categorias = registro.Element("categories").Value;
                string[] splitCategories = categorias.Split(Convert.ToChar(","));

                foreach (var splitCategory in splitCategories)
                {
                    Category category = new Category();
                    category.Description = splitCategory;
                    category.Id = CategoryRepository.GetCategoryId(splitCategory);
                    list.Add(category);
                }
            }

            return list;
        }

        public static Movie GetMovie(string id)
        {
            Movie movie = new Movie();

            var consulta = from c in XElement.Load(@"movies.xml").Elements("row").Elements("movie")
                           where (string)c.Element("id") == id
                           select c;

            foreach (var registro in consulta)
            {
                movie = new Movie()
                {
                    Id = registro.Element("id").Value,
                    Title = registro.Element("title").Value,
                    Sinopse = registro.Element("sinopse").Value,
                    Capa = registro.Element("capa").Value,
                    Type = registro.Element("type").Value,
                    Uri = registro.Element("uri").Value,
                    Status = registro.Element("status").Value,
                    Format = registro.Element("format").Value,
                };
            }
            return movie;
        }

        public void Save(Movie movie)
        {
            if (VerifyCategoryExist(movie.Id))
            {
                XElement xml = XElement.Load(@"movies.xml");
                IEnumerable<XElement> elements = xml.Elements();

                StringBuilder categorias = new StringBuilder();
                int countCategories = 0;
                foreach (var category in movie.Categories)
                {
                    categorias.Append(category.Description);
                    if (movie.Categories.Count > 1 && countCategories < movie.Categories.Count - 1)
                        categorias.Append(",");

                    countCategories++;
                }

                foreach (var item in elements.Elements("movie"))
                {
                    if (item.Element("id").Value == movie.Id)
                    {
                        item.Element("title").Value = movie.Title;
                        item.Element("sinopse").Value = movie.Sinopse;
                        item.Element("capa").Value = movie.Capa;
                        item.Element("uri").Value = movie.Uri;
                        item.Element("type").Value = movie.Type;
                        item.Element("status").Value = movie.Status;
                        item.Element("categories").Value = categorias.ToString();
                        item.Element("format").Value = movie.Format;
                    }
                }
                xml.Save(@"movies.xml");
            }
            else
            {
                XDocument xmlDoc = XDocument.Load("movies.xml");

                StringBuilder categorias = new StringBuilder();
                int countCategories = 0;
                foreach (var category in movie.Categories)
                {
                    categorias.Append(category.Description);
                    if (movie.Categories.Count > 1 && countCategories < movie.Categories.Count - 1)
                        categorias.Append(",");

                    countCategories++;
                }

                if (string.IsNullOrEmpty(movie.Id))
                    movie.Id = GetCurrentID();

                xmlDoc.Element("db").Element("row").Add(new XElement("movie",
                                                                     new XElement("id", movie.Id),
                                                                     new XElement("title", movie.Title),
                                                                     new XElement("sinopse", movie.Sinopse),
                                                                     new XElement("capa", movie.Capa),
                                                                     new XElement("uri", movie.Uri),
                                                                     new XElement("type", movie.Type),
                                                                     new XElement("status", movie.Status),
                                                                     new XElement("categories", categorias.ToString()),
                                                                     new XElement("format", movie.Format)));

                xmlDoc.Save("movies.xml");
            }
        }

        public bool VerifyCategoryExist(string id)
        {
            var consulta = from c in XElement.Load(@"movies.xml").Elements("row").Elements("movie")
                           where c.Element("id").Value == id
                           select c;

            foreach (var registro in consulta)
            {
                return true;
            }

            return false;
        }
    }
}
