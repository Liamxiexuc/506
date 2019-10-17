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

        public static Student FetchStudentDetails(int Id)
        {
            Student Students = new Student();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select title, given_name, family_name, id, campus," +
                            "email, photo, ifnull(level,'Student') as level, unit, ifnull(degree,'') as degree, ifnull(supervisor_id, 0) as supervisor_id, utas_start, current_start from researcher where id=?id", conn);

                cmd.Parameters.AddWithValue("id", Id);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Students = (new Student
                    {
                        Title = rdr.GetString(0),
                        GivenName = rdr.GetString(1),
                        FamilyName = rdr.GetString(2),
                        ID = rdr.GetInt32(3),
                        Campus = ParseEnum<Campus>(rdr.GetString(4).Replace(" ", "")),
                        Email = rdr.GetString(5),
                        Photo = rdr.GetString(6),
                        Level = ParseEnum<EmploymentLevel>(rdr.GetString(7)),
                        Unit = rdr.GetString(8),
                        Degree = rdr.GetString(9),
                        SupervisorId = rdr.GetInt32(10),
                        UtasStart = rdr.GetDateTime(11),
                        CurrentStart = rdr.GetDateTime(12),
                        CurrentJob = "Student",
                        FullName = rdr.GetString(1) + " " + rdr.GetString(2),
                        Tenure = Math.Round((Convert.ToDouble((DateTime.Now - rdr.GetDateTime(11)).TotalDays) / 365), 2)
                    });
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to database: " + e);
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

            return Students;
        }

        public static string FetchSupervisorName(int Id)
        {
            string Name;
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select given_name, family_name from researcher " +
                                                 "where id = (select supervisor_id from researcher where id=?id)", conn);
            cmd.Parameters.AddWithValue("id", Id);
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                Name = rdr.GetString(0) + " " + rdr.GetString(1);
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
                return Name;
            }
            else
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
                return "No Data Found";
            }
        }

        public static int FetchTotalPublication(int Id)
        {
            int Total;
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select count(doi) from researcher_publication " +
                                                    "where researcher_id=?id", conn);
            cmd.Parameters.AddWithValue("id", Id);
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                Total = rdr.GetInt32(0);
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
                return Total;
            }
            else
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
                return 0;
            }
        }

        public static Staff FetchStaffDetails(int Id)
        {
            Staff StaffDetails = new Staff();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select title, given_name, family_name, id, campus," +
                    "email, photo, ifnull(level,'Student') as level, unit, utas_start, current_start from researcher where id=?id", conn);

                cmd.Parameters.AddWithValue("id", Id);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    StaffDetails = (new Staff
                    {
                        Title = rdr.GetString(0),
                        GivenName = rdr.GetString(1),
                        FamilyName = rdr.GetString(2),
                        ID = rdr.GetInt32(3),
                        Campus = ParseEnum<Campus>(rdr.GetString(4).Replace(" ", "")),
                        Email = rdr.GetString(5),
                        Photo = rdr.GetString(6),
                        Level = ParseEnum<EmploymentLevel>(rdr.GetString(7)),
                        Unit = rdr.GetString(8),
                        UtasStart = rdr.GetDateTime(9),
                        CurrentStart = rdr.GetDateTime(10),
                        CurrentJob = JobNameTrans(rdr.GetString(7)),
                        FullName = rdr.GetString(1) + " " + rdr.GetString(2),
                        Tenure = Math.Round((Convert.ToDouble((DateTime.Now - rdr.GetDateTime(9)).TotalDays) / 365), 2),
                    });
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to database: " + e);
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

            return StaffDetails;
        }

        public static string JobNameTrans(string Job)
        {
            string JobName;
            switch (Job)
            {
                case "Student":
                    JobName = "Student";
                    break;
                case "A":
                    JobName = "Postdoc";
                    break;
                case "B":
                    JobName = "Lecturer";
                    break;
                case "C":
                    JobName = "Senior Lecturer";
                    break;
                case "D":
                    JobName = "Associate Professor";
                    break;
                case "E":
                    JobName = "Professor";
                    break;
                default:
                    JobName = "None";
                    break;
            }
            return JobName;
        }

        public static double FetchThreeYearAverage(int Id)
        {
            double ThreeYearAverage;
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select count(A.doi) from researcher_publication as A, " +
                                                 "publication as B where A.doi=B.doi and A.researcher_id=?id and B.year in (2017, 2018, 2019)", conn);
            //datediff(Year(Now()), P.year) < 4
            cmd.Parameters.AddWithValue("id", Id);
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                ThreeYearAverage = Math.Round((Convert.ToDouble(rdr.GetInt32(0))) / 3, 2);
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
                return ThreeYearAverage;
            }
            else
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
                return 0;
            }
        }

        public static List<Position> FetchPreviousPosition(int Id)
        {
            List<Position> PreviousPosition = new List<Position>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select level, start, ifnull(end,current_date()) as end, id from position where id=?id", conn);

                cmd.Parameters.AddWithValue("id", Id);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    PreviousPosition.Add(new Position
                    {
                        positionName = JobNameTrans(rdr.GetString(0)),
                        startDate = rdr.GetDateTime(1),
                        endDate = rdr.GetDateTime(2),
                    });
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to database: " + e);
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

            return PreviousPosition;
        }

        public static List<Publication> FetchPublications(int Id)
        {
            List<Publication> Publicationlist = new List<Publication>();
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;
            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select A.title, A.year, A.doi from publication as A, researcher_publication as B, researcher as C where A.doi=B.doi and C.id=B.researcher_id and B.researcher_id=?id order by A.year desc, A.title", conn);

                cmd.Parameters.AddWithValue("id", Id);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Publicationlist.Add(new Publication
                    {
                        title = rdr.GetString(0),
                        year = rdr.GetInt32(1),
                        DOI = rdr.GetString(2)
                    });
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to database: " + e);
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

            return Publicationlist;
        }

        public static Publication FetchPublicationDetails(string Doi)
        {
            Publication PublicationDetails = new Publication();
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;
            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select title, year, type, available, doi, authors, cite_as from publication where doi=?doi", conn);

                cmd.Parameters.AddWithValue("doi", Doi);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    PublicationDetails = (new Publication
                    {
                        title = rdr.GetString(0),
                        year = rdr.GetInt32(1),
                        type = ParseEnum<PublicationType>(rdr.GetString(2)),
                        availableDate = rdr.GetDateTime(3),
                        DOI = rdr.GetString(4),
                        author = rdr.GetString(5),
                        citeAs = rdr.GetString(6),
                        age = Convert.ToInt32((DateTime.Now - rdr.GetDateTime(3)).TotalDays)
                    });
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to database: " + e);
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

            return PublicationDetails;
        }

        public static int FetchTotalSupervisions(int Id)
        {
            int Total;
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select count(id) from researcher where supervisor_id = ?id", conn);

            cmd.Parameters.AddWithValue("id", Id);
            rdr = cmd.ExecuteReader();

            Total = rdr.Read() ? rdr.GetInt32(0) : 0;

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return Total;
        }

        public static string FetchPerformance(EmploymentLevel level, double ThreeYearAverage)
        {
            string Performance;
            double Require;
            switch (level.ToString())
            {
                case "A":
                    Require = 0.5;
                    break;
                case "B":
                    Require = 1;
                    break;
                case "C":
                    Require = 2;
                    break;
                case "D":
                    Require = 3.2;
                    break;
                case "E":
                    Require = 4;
                    break;
                default:
                    return "";
            }

            //Transfer to string type
            Performance = (ThreeYearAverage / Require).ToString("0.#%");

            return Performance;
        }

        public static List<Student> FetchSupervisionName(int Id)
        {
            List<Student> studentList = new List<Student>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select given_name, family_name from researcher where supervisor_id=?id order by given_name", conn);

                cmd.Parameters.AddWithValue("id", Id);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    studentList.Add(new Student
                    {
                       // GivenName = rdr.GetString(0),
                       // FamilyName = rdr.GetString(1),
                        FullName = rdr.GetString(0) + " " + rdr.GetString(1)

                    });
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to database: " + e);
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

            return studentList;
        }

    }
}
