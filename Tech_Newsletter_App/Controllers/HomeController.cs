using System.Web.Mvc;
using Tech_Newsletter_App.Models;

namespace Tech_Newsletter_App.Controllers
{
    public class HomeController : Controller


    {


        public ActionResult Index()       /*This is the first page of the website (the default landing page). It is where the User Input form lives*/
        {
            return View();
        }


        [HttpPost]                       /*This is where we take the User Input from the form at the Index View and put that data in the database using Sql */
        public ActionResult SignUp(string firstName, string lastName, string emailAddress)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress))
            {

                return View("~/Views/Shared/Error.cshtml");  /* the "~" symbol means that it is a relative file path  */
            }
            else
            {

                using (NewsletterEntities db = new NewsletterEntities())
                {
                    var signup = new SignUp();
                    signup.FirstName = firstName;
                    signup.LastName = lastName;
                    signup.EmailAddress = emailAddress;

                    db.SignUps.Add(signup);
                    db.SaveChanges();

                }
            }

            return View("Success");
        }
    }
}






////we are using ADO.net to connect with the database and exchange informatio*/


//string queryString = @"INSERT INTO SignUps (FirstName, LastName, EmailAddress)" /*these are the column names of the table"SignUps"*/
//                    + "VALUES(@FirstName,@LastName, @EmailAddress)"; /* having parameters to transfer your data to the database helps to prevent SQL injection */

//using (SqlConnection connection = new SqlConnection(connectionString)) /* anytime you are opening a connection with an database you want owrap it in curly braces: therefore, when you are done the connection is noT left open , it always closes. */
//{

//    /*we are defining our Sql command and passing in our queryString and connection information; then, we are adding our parameter information and setting the values of those parameters*/
//    SqlCommand command = new SqlCommand(queryString, connection);
//    command.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar);
//    command.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar);
//    command.Parameters.Add("@EmailAddress", System.Data.SqlDbType.VarChar);

//    command.Parameters["@FirstName"].Value = firstName;
//    command.Parameters["@LastName"].Value = lastName;
//    command.Parameters["@EmailAddress"].Value = emailAddress;


//    connection.Open();
//    command.ExecuteNonQuery();/* this is technically a non-query, because we are simply inputting data into the table*/
//    connection.Close();
//}


