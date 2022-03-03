using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlex
{
    // Movie class to store movies
    class Movie
    {
        public string Name { get; set; }

        public Movie_Restriction rating;

        public Movie(string name, Movie_Restriction ratings)
        {
            this.Name = name;
            this.rating = ratings;
        }

        public bool is_eligible_to_watch(int age)
        {
            return rating.is_eligible_to_watch(age);
        }

        
    }
    
    // Movie_Restriction method to store
    // for each movie its rating restrictions
    // or minimum age 
    class Movie_Restriction
    {
        public film_ratings rating;
        public int age_restricted;

        public Movie_Restriction(int age)
        {
            this.rating = film_ratings.None;
            this.age_restricted = age;
        }

        public Movie_Restriction(film_ratings rating)
        {
            this.rating = rating;
        }

        // Calculate if someone with the age `age` parameter
        // can see the movie which is counted based on
        // the rating or the minimum age 
        public bool is_eligible_to_watch(int age)
        {
            if (rating != film_ratings.None)
            {
                switch (rating)
                {
                    case film_ratings.G:
                        return true;
                    case film_ratings.PG:
                        return age >= 10;
                    case film_ratings.PG13:
                        return age >= 13;
                    case film_ratings.R:
                        return age >= 15;
                    case film_ratings.NC17:
                        return age >= 17;
                    default:
                        return false;
                }
            }
            else
            {
                return age >= this.age_restricted;
            }
        }

        // method to get Movie_Restriction object created based on
        // rating string passed
        // IF the rating is valid, it returns true and sets the `obj` to the newly 
        // created object.
        public static bool getRestrictionObject(string rating, out Movie_Restriction obj)
        {
            obj = null;
            rating = rating.ToUpper();

            int result;
            if (Int32.TryParse(rating, out result))
            {
                obj = new Movie_Restriction(result);
                obj.rating = film_ratings.None;
                return true;
            }
            else
            {
                switch (rating)
                {
                    case "G":
                        obj = new Movie_Restriction(film_ratings.G);
                        return true;
                    case "PG":
                        obj = new Movie_Restriction(film_ratings.PG);
                        return true;
                    case "PG13":
                    case "PG-13":
                        obj = new Movie_Restriction(film_ratings.PG13);
                        return true;
                    case "R":
                        obj = new Movie_Restriction(film_ratings.R);
                        return true;
                    case "NC17":
                    case "NC-17":
                        obj = new Movie_Restriction(film_ratings.NC17);
                        return true;
                    default:
                        return false;
                }
            }
        }

        // overrided method ToString
        public override string ToString()
        {
            if (this.rating == film_ratings.None)
            {
                return this.age_restricted.ToString();
            }
            else
            {
                switch (rating)
                {
                    case film_ratings.G:
                        return "G";
                    case film_ratings.PG:
                        return "PG";
                    case film_ratings.PG13:
                        return "PG-13";
                    case film_ratings.R:
                        return "R";
                    case film_ratings.NC17:
                        return "NC-17";
                    default:
                        return "None";
                }
            }
        }
    }

    // film ratings
    //Constants
    public enum film_ratings
    {
        G,
        PG,
        PG13,
        R,
        NC17,
        None
    }
}
