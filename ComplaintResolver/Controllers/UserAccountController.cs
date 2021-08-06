using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ComplaintResolver.Model;
using ComplaintResolver.DB.DBOperation;
using ComplaintResolver.DB.Utilities;
using System.Net.Mail;

namespace ComplaintResolver.Controllers
{
    public class UserAccountController : Controller
    {
        // GET: UserAccount        
        UserRepositry user = null;


        public UserAccountController()
        {
            user = new UserRepositry();
        }

        /// <summary>
        /// To Register new user to the App
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
                return RedirectToAction("Login");
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register([Bind(Exclude = "IsEmailVerified, ActivationCode")] UserDetail model)
        {
            string message = null;
            bool status = false;

            if (ModelState.IsValid)
            {
                //does Email exists                
                if (user.DoesEmailExists(model.EmailId))
                {
                    ModelState.AddModelError("EmailExists", "Email Already Exists");
                    return View(model);
                }

                //get Activation code
                model.ActivationCode = Guid.NewGuid().ToString();

                //password cryptography
                model.Password = EncryptPassword.Hash(model.Password);
                model.ConfirmPassword = EncryptPassword.Hash(model.Password); //for password is same or not validation during save changes

                model.IsEmailVerified = 0;

                //save changes
                user.AddUser(model);
                message = "Account Created";

                sendVerificationEmail(model.EmailId, model.ActivationCode);
                message = "Account created email send for the verification to your Email Id :-  " + model.EmailId;

                status = true;
            }
            else
            {
                message = "Please fill the form again as it has some errors";
            }

            ViewBag.Message = message;
            ViewBag.Status = status;
            return View(model);
        }

        /// <summary>
        /// This method is used to verify the Email of the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool status = false;
            string message = null;

            if (user.VerifyAccount(id))
            {
                message = "Your Account has been activated successfully. Click Here to Login.";
                status = true;
            }
            else
            {
                message = "Invalid Request";
                status = false;
            }

            ViewBag.Status = status;
            ViewBag.Message = message;
            return View();
        }

        /// <summary>
        /// Verfies Login credentials of the user
        /// </summary>
        /// <returns>returns view based upon user exists or not</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(Login model, string returnUrl)
        {

            string message = null;
            bool doesCredentialExists = user.Check(model);

            if (ModelState.IsValid)
            {
                if (doesCredentialExists)
                {
                    message = "Login Successful";
                    int timeout = model.RememberMe ? 525600 : 20; //525600min is one year

                    //The forms authentication ticket is used to tell the ASP.NET application who you are.
                    var ticket = new FormsAuthenticationTicket(model.EmailId, model.RememberMe, timeout);

                    //encrypts the ticket to string for use in Cookies
                    string encrypted = FormsAuthentication.Encrypt(ticket);

                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeout);

                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);

                    return RedirectToAction("Index", "UserHome");
                }
                else
                {
                    message = "Invalid Request";
                }
            }

            ViewBag.Status = doesCredentialExists;
            ViewBag.Message = message;
            return View();
        }

        /// <summary>
        /// Log out from the current session
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        /// <summary>
        /// to send Verfication Mail to the User to verify his her email
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="activationCode"></param>

        [NonAction]
        public void sendVerificationEmail(string emailId, string activationCode)
        {
            var verifyUrl = "/Account/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("ayush11808629@gmail.com", "Gada Electronics");
            var toEmail = new MailAddress(emailId);
            var fromPassword = "02Feb@2001";

            string subject = "Your account is successfully created!";

            string body = "<br/><br /> We are exicited to tell you that your Gada Electronics account is created. Please Click on the below " +
                "to verify your account<br /> <br /><a href= '" + link + "'>" + link + "</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(fromEmail.Address, fromPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })

                smtp.Send(message);
        }
    }
}