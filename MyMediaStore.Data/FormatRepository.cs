using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMediaStore.Models;

namespace MyMediaStore.Data
{
    public class FormatRepository
    {
        public static string GetFormatId(string format)
        {
            switch (format)
            {
                case "Não sei":
                    {
                        return "1";
                        break;
                    }
                case ".avi":
                    {
                        return "2";
                        break;
                    }
            }

            return "1";
        }

        public static List<Format> GetAll()
        {
            List<Format> formats = new List<Format>();

            Format defaultItem = new Format();
            defaultItem.Id = "1";
            defaultItem.Description = "Não sei";
            formats.Add(defaultItem);

            Format format = new Format();
            format.Id = "2";
            format.Description = ".avi";
            formats.Add(format);

            Format format2 = new Format();
            format2.Id = "3";
            format2.Description = ".rmvb";
            formats.Add(format2);

            Format format3 = new Format();
            format3.Id = "4";
            format3.Description = ".mkv";
            formats.Add(format3);

            return formats;
        }
    }
}
