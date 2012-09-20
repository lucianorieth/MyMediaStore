using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMediaStore.Models;

namespace MyMediaStore.Data
{
    public class MovieTypeRepository
    {
        public static string GetMovieTypeId(string type)
        {
            switch (type)
            {
                case "Não sei":
                    return "1";
                    break;
                case "Filme":
                    return "2";
                    break;
                case "Série":
                    return "3";
                    break;
                case "Clip":
                    return "4";
                    break;
                case "Trailer":
                    return "5";
                    break;

            }

            return "1";
        }

        public static List<MovieType> GetAll()
        {
            List<MovieType> movieTypes = new List<MovieType>();

            MovieType defaultItem = new MovieType();
            defaultItem.Id = "1";
            defaultItem.Description = "Não sei";
            movieTypes.Add(defaultItem);

            MovieType movieType1 = new MovieType();
            movieType1.Id = "2";
            movieType1.Description = "Filme";
            movieTypes.Add(movieType1);

            MovieType movieType2 = new MovieType();
            movieType2.Id = "3";
            movieType2.Description = "Séries";
            movieTypes.Add(movieType2);

            MovieType movieType3 = new MovieType();
            movieType3.Id = "4";
            movieType3.Description = "Clips";
            movieTypes.Add(movieType3);

            return movieTypes;
        }
    }
}
