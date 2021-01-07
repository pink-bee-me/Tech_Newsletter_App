using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tech_Newsletter_App.Models;
using Tech_Newsletter_App.ViewModels;


namespace Tech_Newsletter_App.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
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
                //(Note:Entity Framework serves as a wrapper for condensed ADO.NET code)

                using (NewsletterEntities db = new NewsletterEntities())//using entity syntax to instanciate the NewsletterEntities object that gives us access to the database through the Entity Framework
                {               //Alternate way to display only the signed up people***the "WHERE" part below is LINQ syntax and we are looking for the signups that the value of removed is null))
                                // var signups = db.SignUps.Where(x => x.Removed == null).ToList();  // created the variable "signups"; which is equal to db.SignUps(which is the DbSet of the SignUps Table in the database;
                                //and therefore, giving us access to all of the records in the database table)
                                //NOTE: See comments in Newsletter.Context.cs for further explaination of DbSet
                    var signups = (from c in db.SignUps
                                   where c.Removed == null
                                   select c).ToList();

                    //NOTE:This part of the code was used in the ADO.NET Syntax (as is), as well as here in the Entity Framework Syntax(from this line all the way down to the return view line of code)
                    //here we are mapping the values from the signup model(signup) to the signup view model(signupVm)

                    var signupVms = new List<SignUpVm>();//create a new list of view models


                    foreach (var signup in signups)//loop through the database info that has been mapped to a model
                    {
                        var signupVm = new SignUpVm();
                        /*here we map our properties from the signups list we created to the signupVm list we created to display to the ViewModel*/
                        /*we are using the ViewModel to display the info to the Admin User so that we can explicitly choose what data to show */
                        /* (for example: if you were needing to protect against exposure of private info (ie. social security numbers, */
                        /*credit card info., birth date, medical diagnosis, account balance, etc.) that maybe contained in the data transferred fromm the database*/
                        signupVm.Id = signup.Id;
                        signupVm.FirstName = signup.FirstName;
                        signupVm.LastName = signup.LastName;
                        signupVm.EmailAddress = signup.EmailAddress;

                        signupVms.Add(signupVm);//then adding the newly mapped values to the view model list
                    }

                    return View(signupVms);//then passing this all to the View Model View!!!! Ta-Da!!!Thank you...thank you very much!!!! You're too kind...thank you.

                }
            }

        }
        public ActionResult Unsubscribe(int Id)

        {
            using (NewsletterEntities db = new NewsletterEntities())
            {

                var signup = db.SignUps.Find(Id);
                signup.Removed = DateTime.Now;
                db.SaveChanges();
            }

            return RedirectToAction("Index"); // returning the index
        }
    }
}

//Note: Why do we even want to create a ViewModel?
//Here's why: We cannot see everything that may happen in the future...not even the future that is in store for our application. 
//So, by creating a viewmodel now, we can prevent any conflict that may arise in the future due to privacy or security issues-- or even changes in general how ever many that may be.
//With the creation of the viewmodel, we have enhanced the separation of concerns by deccoupling the presentation of the data even further away from the logic of our application and thereby, made it even less disruptive to make changes to this particular area of the program
//while greatly lessening the overall effect that any issues or changes to this instance of the data model could in anyway conflict or cause problems within the program or the database itself.(example of using the Decoupling concept to increase the separation of concerns within the MVC design pattern)