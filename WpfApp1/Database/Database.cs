using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAPSystem.Model;

namespace RAPSystem.Database
{
    class Database
    {

            private static bool reportingErrors = false;
            /*
             * These would not be hard coded in the source file normally,
             * but read from the application's settings
             * (and, ideally, with some amount of basic encryption applied)
             */
            private const string db = "kit206";
            private const string user = "kit206";
            private const string pass = "kit206";
            private const string server = "alacritas.cis.utas.edu.au";

            private static MySqlConnection conn = null;

            //This method is a gift to you because .NET's approach to converting strings to enums is so horribly broken
            public static T ParseEnum<T>(string value)
            {
                return (T)Enum.Parse(typeof(T), value);
            }

            /// <summary>
            /// Creates and returns (but does not open) the connection to the database.
            /// </summary>
            private static MySqlConnection GetConnection()
            {
                if (conn == null)
                {

                    string connectionString = String.Format("Database={0};Data Source={1};User Id={2};Password={3}", db, server, user, pass);
                    conn = new MySqlConnection(connectionString);
                }
                return conn;
            }
            //researcher list
            public static List<Researcher> FetchBasicresearcherDetails()
            {
                List<Researcher> researcher = new List<Researcher>();

                MySqlConnection conn = GetConnection();
                MySqlDataReader rdr = null;

                try
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand(
                        "select given_name, family_name, title,  id, type, ifnull(level,'Student') as level  from researcher order by family_name",
                        conn);
                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        researcher.Add(new Researcher
                        {
                            GivenName = rdr.GetString(0),
                            FamilyName = rdr.GetString(1),
                            Title = rdr.GetString(2),
                            ID = rdr.GetInt32(3),
                            Type = ParseEnum<Model.Type> (rdr.GetString(4)),
                            Level = ParseEnum<EmploymentLevel>(rdr.GetString(5)),
                        });
                    }
                }
                catch (MySqlException e)
                {
                    Console.WriteLine("loading researcher", e);
                }
                finally
                {
                    if (rdr != null)
                    {
                        rdr.Close();
                    }
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }

                return researcher;
            }

            //PublicationDetial
      /*      public static List<Researcher> FetchfullResearcherDetails()
            {
                List<Researcher> researcher = new List<Researcher>();

                MySqlConnection conn = GetConnection();
                MySqlDataReader rdr = null;

                try
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand(
                        "select ID, given_name, family_name, title, campus, Email, Photo from researcher",
                        conn);
                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        researcher.Add(new Researcher
                        {

                            ID = rdr.GetInt32(0),
                            GivenName = rdr.GetString(1),
                            FamilyName = rdr.GetString(2),
                            Title = rdr.GetString(3),
                            School = rdr.GetString(4),
                            Campus = rdr.GetString(5),
                            Email = rdr.GetString(6),
                            Photo = rdr.GetString(7),

                        });
                    }
                }
                catch (MySqlException e)
                {
                    Console.WriteLine("loading researcher", e);
                }
                finally
                {
                    if (rdr != null)
                    {
                        rdr.Close();
                    }
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }

                return researcher;
            }*/
            //researcher list

        
    }
}
