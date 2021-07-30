using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComplaintResolver.DB.Utilities;
using ComplaintResolver.DB.DBOperation;
using ComplaintResolver.Model;
using System.Web.Security;

namespace ComplaintResolver.Controllers
{
    public class EmployeeHomeController : Controller
    {
        // GET: EmployeeHome
        EmployeeRepositry emp = null;

        public EmployeeHomeController()
        {
            emp = new EmployeeRepositry();
        }

        /// <summary>
        /// Home Page for Admin and Serviceman
        /// </summary>
        /// <returns></returns>

        [Authorize(Roles = "Admin , Serviceman")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Delete an Employee from the DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        
        [Authorize(Roles ="Admin")]
        public ActionResult DeleteEmployee(int id)
        {
            bool isDeleted = emp.DeleteEmployee(id);

            return RedirectToAction("AllEmployees");
        }

        /// <summary>
        /// User Profile for all the details of the Employee
        /// </summary>
        /// <returns></returns>

        [Authorize(Roles = "Admin , Serviceman")]
        public ActionResult Profile()
        {
            string employeeEmail = User.Identity.Name;

            var employeeProfile = emp.GetProfile(employeeEmail);
            return View(employeeProfile);
        }

        /// <summary>
        /// Returns all the employee in the Database
        /// </summary>
        /// <returns></returns>

        [Authorize(Roles = "Admin")]
        public ActionResult AllEmployees()
        {
            var result = emp.GetAllEmployees();
            return View(result);
        }

        /// <summary>
        /// All complaints that has been lodged on portal till date
        /// </summary>
        /// <returns></returns>

        [Authorize(Roles = "Admin")]
        public ActionResult AllComplaints()
        {
            var result = emp.GetAllComplaint();

            if (result.Count > 0)
            {
                return View(result);
            }
            return View("_NoDataFound");

        }

        /// <summary>
        /// Get list of all the pending complaints
        /// </summary>
        /// <returns></returns>

        [Authorize(Roles = "Admin")]
        public ActionResult PendingComplaints()
        {

            var result = emp.GetPendingComplaints();
            if (result.Count > 0)
            {
                return View(result);
            }
            return View("_NoDataFound");

        }

        /// <summary>
        /// Complaints that have recieved replies
        /// </summary>
        /// <returns></returns>
        
        [Authorize(Roles = "Admin")]
        public ActionResult CompletedComplaints()
        {
            var result = emp.GetCompletedComplaints();
            if (result.Count > 0)
            {
                return View(result);
            }
            return View("_NoDataFound");

        }

        /// <summary>
        /// All Complaints of a particular ServiceMan
        /// </summary>
        /// <returns></returns>

        [Authorize(Roles = "Serviceman")]
        public ActionResult ServicemanAllComplaints()
        {
            string email = User.Identity.Name;
            var result = emp.GetServicemanAllComplaints(email);

            if (result.Count > 0)
            {
                return View(result);
            }
            return View("_NoDataFound");
        }

        /// <summary>
        /// Pending Complaints of a ServiceMan
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Serviceman")]
        public ActionResult ServicemanPendingComplaints()
        {

            string email = User.Identity.Name;
            var result = emp.GetServicemanPendingComplaints(email);

            if (result.Count > 0)
            {
                return View(result);
            }

            return View("_NoDataFound");
        }

        /// <summary>
        /// Completed Complaint by a Serviceman
        /// </summary>
        /// <returns></returns>

        [Authorize(Roles = "Serviceman")]
        public ActionResult ServicemanCompletedComplaints()
        {
            string email = User.Identity.Name;
            var result = emp.GetServicemanCompletedComplaints(email);

            if (result.Count > 0)
            {
                return View(result);
            }
            return View("_NoDataFound");
        }

        /// <summary>
        /// Feedbacks Recived for the complaints
        /// </summary>
        /// <returns></returns>

        [Authorize(Roles = "Admin")]
        public ActionResult AllFeedbacksRecieved()
        {
            var feedbacks = emp.GetAllFeedbacks();
            return View(feedbacks);
        }

    }
}