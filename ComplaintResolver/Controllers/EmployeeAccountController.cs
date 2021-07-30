using ComplaintResolver.DB.DBOperation;
using ComplaintResolver.DB.Utilities;
using ComplaintResolver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ComplaintResolver.Controllers
{
    public class EmployeeAccountController : Controller
    {

        EmployeeRepositry emp = null;

        public EmployeeAccountController()
        {
            emp = new EmployeeRepositry();
        }

        /// <summary>
        /// Register a new Employee View
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Register([Bind(Exclude = "PendingComplaints, CompletedComplaints")] EmployeeDetail model)
        {
            string message = null;
            bool status = false;

            if (ModelState.IsValid)
            {
                //does Email exists                
                if (emp.DoesEmailExists(model.Email))
                {
                    ModelState.AddModelError("EmailExists", "Email Already Exists");
                    return View(model);
                }

                //password cryptography
                model.Password = EncryptPassword.Hash(model.Password);                

                //save changes
                int id = emp.AddEmployee(model);

                //Add role of the eployee to the Role table
                emp.AddRole(id, model);
                message = "Account Created";

                status = true;
            }
            else
            {
                message = "Some bugs";
            }

            ViewBag.Message = message;
            ViewBag.Status = status;
            return View(model);
        }

        /// <summary>
        /// Login Verifications for Employees
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult LoginServiceman()
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
        public ActionResult LoginServiceman(Login model)
        {

            string message = null;
            bool doesCredentialExists = emp.check(model);

            if (ModelState.IsValid)
            {
                if (doesCredentialExists)
                {
                    message = "Login Successful";
                    int timeout = model.RememberMe ? 525600 : 20; //525600min is one year

                    var ticket = new FormsAuthenticationTicket(model.EmailId, model.RememberMe, timeout);
                    string encrypted = FormsAuthentication.Encrypt(ticket);

                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeout);

                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);

                    return RedirectToAction("Index", "EmployeeHome");

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
        /// Logging Out the Employee
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LoginServiceman");
        }
    }
}