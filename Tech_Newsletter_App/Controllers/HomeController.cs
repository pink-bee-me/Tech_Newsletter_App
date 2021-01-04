using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using Tech_Newsletter_App.ViewModels;

namespace Tech_Newsletter_App.Controllers
{
    public class HomeController : Controller


    {
        private readonly string connectionString = @"Data Source=THINKPINKBEEME1\SQLEXPRESS;Initial Catalog = Newsletter; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public ActionResult Index()/*This is the first page of the website (the default landing page). It is where the User Input form lives*/
        {
            return View();
        }

        [HttpPost] /*This is where we take the User Input from the form at the Index View and put that data in the database using Sql */
        public ActionResult SignUp(string firstName, string lastName, string emailAddress)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress))
            {

                return View("~/Views/Shared/Error.cshtml");  /* the "~" symbol means that it is a relative file path  */
            }
            else
            { /*we are using ADO.net to connect with the database and exchange informatio*/


                string queryString = @"INSERT INTO SignUps (FirstName, LastName, EmailAddress)" /*these are the column names of the table"SignUps"*/
                                    + "VALUES(@FirstName,@LastName, @EmailAddress)"; /* having parameters to transfer your data to the database helps to prevent SQL injection */

                using (SqlConnection connection = new SqlConnection(connectionString)) /* anytime you are opening a connection with an database you want owrap it in curly braces: therefore, when you are done the connection is noT left open , it always closes. */
                {

                    /*we are defining our Sql command and passing in our queryString and connection information; then, we are adding our parameter information and setting the values of those parameters*/
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar);
                    command.Parameters.Add("@EmailAddress", System.Data.SqlDbType.VarChar);

                    command.Parameters["@FirstName"].Value = firstName;
                    command.Parameters["@LastName"].Value = lastName;
                    command.Parameters["@EmailAddress"].Value = emailAddress;


                    connection.Open();
                    command.ExecuteNonQuery();/* this is technically a non-query, because we are simply inputting data into the table*/
                    connection.Close();
                }
                return View("Success");
            }
        }

        public ActionResult Admin()
        {//NOTE: BELOW IS THE ADO.NET SYNTAX ORIGINALLY USED ; NOW REPLACED BY ENTITY FRAMEWORK SYNTAX (but left the ADO.NET code for learning purposes):END NOTE
            /*going to use ADO.NET to get all the signups off the database to display for the Admin */
            //string queryString = @"SELECT Id, FirstName, LastName, EmailAddress, SocialSecurityNumber FROM SignUps";
            //List<SignUp> signups = new List<SignUp>();/*creating a List of type SignUp and namining the new list "signups"; each item in the list will be referred to as a "signup"*/

            //using (SqlConnection connection = new SqlConnection(connectionString))/*using the sql connection string to get into the database*/
            //{
            //    SqlCommand command = new SqlCommand(queryString, connection);/*passing the queryString (the instructions for the database request) and the connection string info to get SignUps Table information to display for the Admin off of the database*/

            //    connection.Open();

            //    SqlDataReader reader = command.ExecuteReader();/*read the data in the table*/

            //    while (reader.Read())
            //    {
            //        /*here we mapped the data from the SignUp table in the database to the signups list we are creating*/
            //        var signup = new SignUp();/*make a new object called "signup* of type "SignUp"(this is our model class "SignUp") made up of the information that we are reading from the database*/
            //        signup.Id = Convert.ToInt32(reader["Id"]);/*we have to convert the information gathered off the database to a format that c sharp can understand*/

            //        signup.FirstName = reader["FirstName"].ToString();
            //        signup.LastName = reader["LastName"].ToString();
            //        signup.EmailAddress = reader["EmailAddress"].ToString();
            //        signup.SocialSecurityNumber = reader["SocialSecurityNumber"].ToString();

            //        signups.Add(signup);/*we are adding this new information(signup.FirstName, signup.LastName, etc....) to our new "signups" list*/

            //    }
            //}

            //ENTITY FRAMEWORK SYNTAX STARTS HERE:

            using (NewsletterEntities db = new NewsletterEntities())//using entity syntax to instanciate the NewsletterEntities object that gives us access to the database through the Entity Framework
            {
                var signups = db.SignUps; // created the variable "signups"; which is equal to db.SignUps(which is the DbSet of the SignUps Table in the database;
                                          //and therefore, giving us access to all of the records in the database table)
                                          //NOTE: See comments in Newsletter.Context.cs for further explaination of DbSet


                //NOTE:This part of the code was used in the ADO.NET Syntax (as is), as well as here in the Entity Framework Syntax(from this line all the way down to the return view line of code)
                //here we are mapping the values from the signup model(signup) to the signup view model(signupVm)

                var SignUpVms = new List<SignUpVm>();//create a new list of view models


                foreach (var signup in signups)
                {
                    var signupVm = new SignUpVm();
                    /*here we map our properties from the signups list we created to the signupVm list we created to display to the ViewModel*/
                    /*we are using the ViewModel to display the info to the Admin User so that we can explicitly choose what data to show */
                    /* (for example: if you were needing to protect against exposure of private info (ie. social security numbers, */
                    /*credit card info., birth date, medical diagnosis, account balance, etc.) that maybe contained in the data transferred fromm the database*/

                    signupVm.FirstName = signup.FirstName;
                    signupVm.LastName = signup.LastName;
                    signupVm.EmailAddress = signup.EmailAddress;

                    SignUpVms.Add(signupVm);//then adding the newly mapped values to the view model list
                }
                return View(SignUpVms);//then passing this all to the View Model View!!!! Ta-Da!!!Thank you...thank you very much!!!! You're too kind...thank you.
            }






        }
    }
}

