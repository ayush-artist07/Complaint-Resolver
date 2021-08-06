﻿using System;
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
        ComplaintRepositry complaint = null;

        public EmployeeHomeController()
        {
            emp = new EmployeeRepositry();
            complaint = new ComplaintRepositry();
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
        /// to update the details of an employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            string email = GetIdFromEmail.UserId(id);

            if(email == null)
            {
                return View("_NoDataFound");
            }
            else
            {
                var empDetails = emp.GetProfile(email);
                return View(empDetails);
            }           
            
        }

        [HttpPost]
        public ActionResult Edit([Bind(Exclude = "PendingComplaints, CompletedComplaints")]EmployeeDetail model)
        {
            string message = null;
            bool status = false;
            ModelState.Remove("Password");

            if(ModelState.IsValid)
            {
                emp.UpdateEmployee(model);

                message = "Employee Record Updated";
                status = true;

            }

            ViewBag.Message = message;
            ViewBag.Status = status;
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

            if(result == null)
            {
                throw new Exception();
            }

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