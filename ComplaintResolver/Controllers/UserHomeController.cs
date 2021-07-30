using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComplaintResolver.DB;
using ComplaintResolver.DB.DBOperation;
using ComplaintResolver.Model;

namespace ComplaintResolver.Controllers
{
    public class UserHomeController : Controller
    {
        // GET: UserHome
        ComplaintRepositry complaint = null;

        public UserHomeController()
        {
            complaint = new ComplaintRepositry();
        } 

        /// <summary>
        /// Home Page for the User
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Serviceman"))
            {
                return this.RedirectToAction("Index", "EmployeeHome");
            }
            return View();
        }

        /// <summary>
        /// Complaint Form to be filled by the user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Complaint()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Complaint([Bind(Exclude = "User_Id, Employee_id")] ComplaintForm model)
        {
            var status = false;
            string message = null;

            if (ModelState.IsValid)
            {
                int complaintId = complaint.AddComplaints(model);
                status = true;
                message = "Your Complaint has been submitted your complaint ID  " + complaintId;
            }
            else
            {
                message = "Your complaint was not submitted Try Again";
            }

            ViewBag.Message = message;
            ViewBag.Status = status;
            return View();
        }

        /// <summary>
        /// Returns all the Complaints by an user
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllComplaints()
        {
            List<ComplaintForm> Usercomplaints = complaint.GetAllComplaint(User.Identity.Name);

            if (Usercomplaints == null)
            {
                return View("NoDataFound");
            }
            return View(Usercomplaints);

        }

        /// <summary>
        /// Pending Complaints for a user
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPendingComplaints()
        {
            List<ComplaintForm> UserPendingComplaints = complaint.GetPendingComplaint(User.Identity.Name);

            if (UserPendingComplaints.Count <= 0)
            {
                return View("NoDataFound");
            }
            return View(UserPendingComplaints);

        }

        /// <summary>
        /// Details of a Particular Complaint Selected by user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            ComplaintForm c = complaint.GetComplaint(id);

            if(c == null)
            {
                return View("_NoDataFound");
            }
            return View(c);
        }

        /// <summary>
        /// Details page for pending Complaints to be resolved
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DetailsPending(int id)
        {
            ComplaintForm comp = complaint.GetComplaint(id);
            return View(comp);
        }

        /// <summary>
        /// About Complaint Resolver
        /// </summary>
        /// <returns></returns>
        public ActionResult AboutUs()
        {
            return View();
        }

        /// <summary>
        /// Complaints are replied throgh this view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Serviceman")]
        public ActionResult Reply(int id)
        {
            var result = complaint.GetComplaint(id);
            return View(result);
        }

        [HttpPost]
        public ActionResult Reply(ComplaintForm model)
        {
            string message = null;

            bool hasReplied = complaint.AddReply(model);

            if (hasReplied == false)
            {
                message = "Changes unSucessfull";
                return RedirectToAction("Error");
            }

            message = "Change Sucessfull";            

            ViewBag.Status = hasReplied;
            ViewBag.Message = message;
            return View();
        }

        /// <summary>
        /// To add feedbackon the solved complaints
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddUserFeedback(int id)
        {
            ViewBag.ComplaintId = id;
            return View();
        }
        [HttpPost]
        public ActionResult AddUserFeedback(Feedback model)
        {
            bool status = false;
            string message = null;

            if (ModelState.IsValid)
            {
                complaint.AddFeedback(model);
                message = "Feedback Sent";
                status = true;
            }

            ViewBag.Status = status;
            ViewBag.Message = message;
            return View();
        }
    }
}