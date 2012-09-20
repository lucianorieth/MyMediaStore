using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MyMediaStore.Models;

namespace MyMediaStore.Data
{
    public class CategoryRepository
    {
        /// <summary>
        /// Return all categories
        /// </summary>
        /// <returns></returns>
        public static List<Category> LoadAll()
        {
            List<Category> list = new List<Category>();

            var consulta = from c in XElement.Load(@"categories.xml").Elements("row").Elements("category")
                           select c;

            foreach (var registro in consulta)
            {
                Category category = new Category()
                {
                    Id = registro.Element("id").Value,
                    Description = registro.Element("description").Value
                };

                list.Add(category);
            }
            return list;
        }

        public static string GetCurrentID()
        {
            string lastId = "0";

            try
            {
                var consulta = from c in XElement.Load(@"categories.xml").Elements("row")
                               select c.Elements("category").Last();


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

        public static string GetCategoryId(string description)
        {
            string id = "";
            var consulta = from c in XElement.Load(@"categories.xml").Elements("row").Elements("category")
                           where c.Element("description").Value == description
                           select c;


            foreach (var registro in consulta)
            {
                id = registro.Element("id").Value;
            }

            return id;
        }

        public bool VerifyCategoryExist(string id)
        {
            var consulta = from c in XElement.Load(@"categories.xml").Elements("row").Elements("category")
                           where c.Element("id").Value == id
                           select c;

            foreach (var registro in consulta)
            {
                return true;
            }

            return false;
        }

        public void Save(Category category)
        {
            if (VerifyCategoryExist(category.Id))
            {
                XElement xml = XElement.Load(@"categories.xml");
                IEnumerable<XElement> elements = xml.Elements();

                foreach (var item in elements.Elements("category"))
                {
                    if (item.Element("id").Value == category.Id)
                    {
                        item.Element("description").Value = category.Description;    
                    }
                }
                xml.Save(@"categories.xml");
            }
            else
            {
                XDocument xmlDoc = XDocument.Load(@"categories.xml");
                xmlDoc.Element("db").Element("row").Add(new XElement("category",
                                                                     new XElement("id", category.Id),
                                                                     new XElement("description", category.Description)));

                xmlDoc.Save("categories.xml");
            }
        }

        public void Delete(Category category)
        {
            if (VerifyCategoryExist(category.Id))
            {
                XElement xml = XElement.Load(@"categories.xml");
                IEnumerable<XElement> elements = xml.Elements();

                foreach (var item in elements.Elements("category"))
                {
                    if (item.Element("id").Value == category.Id)
                    {
                        item.Remove();
                        break;
                    }
                }
                xml.Save(@"categories.xml");
            }
        }
    }
}
