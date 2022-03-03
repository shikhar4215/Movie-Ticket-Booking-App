using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlex
{
    class App
    {

        int PASSWORD_ATTEMPTS = 5;
        int MAX_MOVIES = 10;
        List<Movie> movies;

        static string[] number_words = new string[] { "First", "Second", "Third", "Fourth", "Fifth", "Sixth", "Seventh", "Eighth", "Ninth", "Tenth" };

        // create an object with custom attempts and max_movies 
        public App(int attempts, int max_movies)
        {
            PASSWORD_ATTEMPTS = attempts;
            MAX_MOVIES = max_movies;
        }

        public App()
        {

        }

        // Entry point of the app
        public void App_Start()
        {
            // setting foregroundcolor for the app
            Console.ForegroundColor = ConsoleColor.White;
            movies = new List<Movie>();
            while (true)
            {
                Console.Clear();
                // getting homescreen displayed and selection from the user
                int selection = home();
                // admin screen if selection = 1 
                if (selection == 1)
                {
                    Console.Clear();
                    bool result;
                    // authenticate admin by getting password 
                    // with maximum attempts
                    if (!authenticate_admin(out result))
                    {
                        if (result)
                        {
                            Console.Clear();
                            do
                            {
                                Console.Clear();
                                // get movies to be displayed from admin
                                movies = input_movies();
                                Console.Clear();
                                print_movies();
                            } while (!get_confirmation_movies());
                        }
                    }
                }
                // guest screen 
                else if (selection == 2)
                {
                    Console.Clear();
                    // user screen flow
                    UserScreen();
                }
            }
        }

        // Print inputted movies on the screen
        void print_movies()
        {
            int count = 0;
            foreach (Movie m in movies)
            {
                count++;
                Console.WriteLine("\t " + count + " " + m.Name + " {" + m.rating + "}");
            }
        }

        // Get confirmation from the admin
        // if the inputted movies are to be ok.
        bool get_confirmation_movies()
        {
            int top = Console.CursorTop;
            while (true)
            {
                Console.Write("\n Your Movies Playing Today Are Listed Above. Are you satisfied (Y/N):     ");
                Console.SetCursorPosition(Console.CursorLeft - 4, Console.CursorTop);
                string response = Console.ReadLine();
                if (response == "Y" || response == "y" || response == "yes")
                {
                    return true;
                }
                else if (response == "N" || response == "n" || response == "no")
                {
                    return false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(0, top + 1);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, top);
                    Console.WriteLine("\n Please enter a valid response...");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        // Generate movieplex header on the home screen
        void header()
        {
            string s = "************************************";
            Console.SetCursorPosition((Console.WindowWidth - s.Length) / 2, Console.CursorTop);
            Console.WriteLine(s);
            Console.SetCursorPosition((Console.WindowWidth - s.Length) / 2, Console.CursorTop);
            Console.WriteLine("*** Welcome to MoviePlex Theatre ***");
            Console.SetCursorPosition((Console.WindowWidth - s.Length) / 2, Console.CursorTop);
            Console.WriteLine(s);
        }


        // Display home screen 
        int home()
        {
            header();

            Console.WriteLine("\n\n Please Select From the Following Options:");
            Console.WriteLine(" 1: Administrator");
            Console.WriteLine(" 2: Guests");
            int top = Console.CursorTop + 1;
            Console.Write("\n Selection: ");
            while (true)
            {
                int selection;
                if (!input_in_range(1, 2, out selection))
                {
                    Console.SetCursorPosition(0, top);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a valid selection...");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\n Selection:  ");
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                }
                else
                {
                    return selection;
                }
            }
        }

        // gets password from the admin and validating it 
        // until maximum attemts
        // storing result in the `res` variable 

        // if admin wants to leave the password screen by passing Esc
        // it returns true.
        bool authenticate_admin(out bool res)
        {
            res = false;
            for (int i = PASSWORD_ATTEMPTS - 1; i >= 0; i--)
            {
                Console.Write("\nPlease Enter The Admin Password: ");
                if (!attempt_auth(out res))
                {
                    if (res) return false;
                    if (i == 0) return false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\nYou entered an Invalid Password.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("You have {0} more attempts to enter the correct password OR Press Esc to go back to the previous screen.", i);
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        bool attempt_auth(out bool res)
        {
            res = false;
            SecureString password;
            if (!getPassword(out password))
            {
                string PassWord = new System.Net.NetworkCredential(string.Empty, password).Password;
                bool result = Administrator.matchPassword(PassWord);
                res = result;
                return false;
            }
            else
            {
                return true;
            }
        }

        // Gets the movies from the admin
        List<Movie> input_movies()
        {
            Console.WriteLine("Welcome MoviePlex Administrator\n");
            Console.Write("How many movies are playing today?: ");

            int number_of_movies;
            List<Movie> movies = new List<Movie>();
            do
            {
                bool result = input_in_range(1, MAX_MOVIES, out number_of_movies);
                if (result)
                {
                    Console.WriteLine("\n");
                    Console.WriteLine(" Valid Movie Ratings: G, PG, PG-13, R, NC-17");
                    //Console.WriteLine(" Age Must be 1-100");
                    for (int i = 1; i <= number_of_movies; i++)
                    {
                        Console.WriteLine("\n");
                        string movie_name;
                        Movie_Restriction rest_obj;
                        int top = Console.CursorTop;
                        do
                        {
                            Console.Write("Please Enter The {0} Movie's Name: ", number_words[i - 1]);
                            movie_name = Console.ReadLine();
                            if (movie_name.Trim().Length == 0)
                            {
                                Console.SetCursorPosition(0, top);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Please Enter a valid movie name...\n");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                break;
                            }
                        } while (true);

                        top = Console.CursorTop;
                        do
                        {
                            Console.Write("Please Enter the Age Limit or Rating For The {0} Movie:         ", number_words[i - 1]);
                            Console.SetCursorPosition(Console.CursorLeft - 8, Console.CursorTop);
                                                //Column porision of cursor,row position of cursor
                            string rating = Console.ReadLine();
                            int age;
                            if (Int32.TryParse(rating, out age))
                            {
                                if (age <= 0 || age >= 100)
                                {
                                    Console.SetCursorPosition(0, top);
                                    Console.Write(new string(' ', Console.WindowWidth));
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Please Enter a valid age...\n");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    continue;
                                }
                            }

                            if (!Movie_Restriction.getRestrictionObject(rating, out rest_obj))
                            {
                                Console.SetCursorPosition(0, top);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Please Enter a valid rating...\n");
                                Console.ForegroundColor = ConsoleColor.White;
                                continue;
                            }
                            break;
                        } while (true);
                        Movie movie = new Movie(movie_name, rest_obj);
                        movies.Add(movie);
                    }
                    break;
                }
                else
                {
                    Console.SetCursorPosition(0, 2);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a number in range 1" +
                        "-10...\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("How many movies are playing today?:     ");
                    Console.SetCursorPosition(Console.CursorLeft - 4, Console.CursorTop);
                                            //Column porision of cursor,row position of cursor
                }
            } while (true);
            return movies;
        }

        // displays the user screen
        void UserScreen()
        {
            header();
            if (movies.Count() == 0)
            {
                Console.WriteLine("\n\n Oops! Currently, There are no movies to Display.");
                Console.WriteLine("\n\n Press any key to go to the main screen...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("\n\n There are {0} movies playing today. Please choose from the following movies:\n", movies.Count());
            print_movies();
            Console.WriteLine("\n\n");

            int input_top = Console.CursorTop;

            while (true)
            {
                int top = input_top;
                int movie;
                Console.Write("Which Movie Would You Like To Watch: ");
                while (true)
                {
                    if (!input_in_range(1, movies.Count(), out movie))
                    {
                        Console.SetCursorPosition(0, top);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please enter a valid selection...\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Which Movie Would You Like To Watch:   ");
                        Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop);
                                                  //Column porision of cursor,row position of cursor
                    }
                    else break;
                }

                Console.WriteLine("\n\n");
                top = Console.CursorTop;
                int age;
                Console.Write("Please Enter Your Age For Verification: ");

                while (true)
                {
                    if (!input_in_range(1, 100, out age))
                    {
                        Console.SetCursorPosition(0, top);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please enter a valid age...\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Please Enter Your Age For Verification:    ");
                        Console.SetCursorPosition(Console.CursorLeft - 3, Console.CursorTop);
                    }
                    else break;
                }

                Movie selected_movie = movies.ElementAt(movie - 1);

                if (!selected_movie.is_eligible_to_watch(age))
                {
                    for (int i = Console.CursorTop; i >= input_top; i--)
                    {
                        Console.SetCursorPosition(0, i);
                        Console.Write(new string(' ', Console.WindowWidth));
                    }
                    Console.SetCursorPosition(0, input_top);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You cannot watch this movie as per the guidelines.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Please Select the movie again...\n");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n\n Enjoy The Movie!");
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine("\n Press M to go back to the Guest Main Menu.");
                    Console.WriteLine("\n Press S to go to the Start Page.");

                    ConsoleKeyInfo keyInfo;
                    do
                    {
                        keyInfo = Console.ReadKey(intercept: true);
                    }
                    while (keyInfo.KeyChar != 'M' && keyInfo.KeyChar != 'm' && keyInfo.KeyChar != 'S' && keyInfo.KeyChar != 's');

                    if (keyInfo.KeyChar == 'S' || keyInfo.KeyChar == 's')
                    {
                        return;
                    }
                    for (int i = Console.CursorTop; i >= input_top; i--)
                    {
                        Console.SetCursorPosition(0, i);
                        Console.Write(new string(' ', Console.WindowWidth));
                    }
                    Console.SetCursorPosition(0, input_top);

                }
            }

        }


        // gets the number from the user which is between min and max (inclusive)
        // rejects every input other than number
        // stores the result in `selection` variable if all goes right
        bool input_in_range(int min, int max, out int selection)
        {
            selection = -1;
            if (min > max)
            {
                throw new Exception("Invalid Arguments");
            }
            int max_digits = max.ToString().Length;

            string curr_input = "";
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(intercept: true);

                if (char.IsDigit(keyInfo.KeyChar))
                {
                    if (curr_input.Length < max_digits)
                    {
                        curr_input += keyInfo.KeyChar.ToString();
                        Console.Write(keyInfo.KeyChar);
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && curr_input.Length > 0)
                {
                    curr_input = curr_input.Length != 0 ? curr_input.Substring(0, curr_input.Length - 1) : "";
                    Console.Write("\b \b");
                }
            } while (keyInfo.Key != ConsoleKey.Enter);


            if (Int32.TryParse(curr_input, out selection))
            {
                if (selection < min) return false;
                if (selection > max) return false;
                return true;
            }
            return false;
        }

        // displays the '*' character for every character typed
        // and stores the result in `password`
        // if `Esc` is typed, it returns true
        bool getPassword(out SecureString password)
        {
            ConsoleKeyInfo keyInfo;
            password = new SecureString();
            do
            {
                keyInfo = Console.ReadKey(intercept: true);
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    return true;
                }
                if (!char.IsControl(keyInfo.KeyChar))
                {
                    password.AppendChar(keyInfo.KeyChar);
                    Console.Write('*');
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.RemoveAt(password.Length - 1);
                    Console.Write("\b \b");
                }
            } while (keyInfo.Key != ConsoleKey.Enter);

            return false;
        }

    }
}
