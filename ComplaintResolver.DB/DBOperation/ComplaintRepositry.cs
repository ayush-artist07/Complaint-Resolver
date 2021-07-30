using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintResolver.Model.Enums;
using ComplaintResolver.Model;

namespace ComplaintResolver.DB.DBOperation
{
    public class ComplaintRepositry
    {
        /// <summary>
        /// Adds details of New Complaint to the complaint table present in the Database
        /// </summary>
        /// <param name="model"> ComplaintForm model contains the data entered by the user from the form on UI</param>
        /// <returns></returns>
        public int AddComplaints(ComplaintForm model)
        {
            using (var context = new testdbEntities1())
            {
                //get the email of the user from the ComplaintForm
                string email = model.User;

                //to get the UserId of the Logged in User
                var userId = (from user in context.users
                                where user.EmailId == email
                                select user.UserId).First();

                //Complaint is for the Product of type
                string productType = Convert.ToString(model.ProductType);

                //employee with minimum Pending Complaints is assigned the new complaint for a particular Product Type
                //for each department we have altleast 1 employee
                var employeeWithComplaint = (from emp in context.employee
                                        where emp.Department == productType
                                        orderby emp.PendingComplaints
                                        select emp.Employee_Id).First();

                //increase the number of pending complaints of that Employee
                var pendingComplaintEmployee = context.employee.Where(x => x.Employee_Id == employeeWithComplaint).FirstOrDefault();
                pendingComplaintEmployee.PendingComplaints += 1;
                

                //Convert the data from Model type as taken by user to the type to be stored in Data Base 
                complaint c = new complaint()
                {
                    User_Id = userId,
                    Date_assigned = DateTime.Now,
                    ProductType = Convert.ToString(model.ProductType),
                    ComplaintType = Convert.ToString(model.ComplaintType),
                    ComplaintDescription = model.ComplaintDescription,
                    PhoneNumber = model.PhoneNumber,
                    Employee_id = employeeWithComplaint,
                    ReplyComments = "Not Available",
                    Status = Convert.ToString(ComplaintStatus.Assigned)
                };

                context.complaint.Add(c);
                context.SaveChanges();

                return c.Complaint_Id;
            }
        }

        /// <summary>
        /// Reply to the Complaint given by user is added to the Complaint table in database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddReply(ComplaintForm model)
        {
            using (var context = new testdbEntities1())
            {
                bool isReplied = false;
                //selects the complaint for which the reply is added
                complaint c = context.complaint.Where(x => x.Complaint_Id == model.Complaint_id).FirstOrDefault();

                //selects the employee who is replying to the given complaint
                var employee = context.employee.Where(x => x.Employee_Id == model.Employee_id).FirstOrDefault();

                //gets the current status as set by the employee
                string status = Convert.ToString(model.Status);

                //if there exists an employee
                if (employee != null)
                {
                    c.ReplyComments = model.ReplyComments;
                    c.Status = status;

                    if (c.Status == "Solved")
                    {
                        employee.CompletedComplaints += 1;
                        employee.PendingComplaints -= 1;

                        isReplied = true;
                    }
                }

                context.SaveChanges();
                return isReplied;
            }
        }

        /// <summary>
        /// Adds feedback to the feedback table given by user
        /// </summary>
        /// <param name="model"></param>
        public void AddFeedback(Feedback model)
        {
            using (var context = new testdbEntities1())
            {
                feedback x = new feedback()
                {
                    Complaint_Id = model.ComplaintId,
                    Message = model.Message
                };

                context.feedback.Add(x);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gives the list of complaints of a particular user.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>List of Complaints by a user</returns>
        public List<ComplaintForm> GetAllComplaint(string email)
        {
            using (var context = new testdbEntities1())
            {
                //to find the User whose all complaints needs to be extracted
                var userId =    (from user in context.users
                                where user.EmailId == email
                                select user.UserId).First();
                
                var complaintCount = context.complaint.Where(x => x.User_Id == userId).FirstOrDefault();

                //if there is any complaint
                if (complaintCount != null)
                {
                    //to get list of the Details of the Logged in User
                    var userComplaints = context.complaint.Where(x => x.User_Id == userId).ToList();

                    var userDetails = userComplaints.Select(x => new Model.ComplaintForm()
                    {
                        Complaint_id = x.Complaint_Id,
                        User_Id = x.User_Id,
                        Date_assigned = x.Date_assigned,
                        ComplaintDescription = x.ComplaintDescription,
                        PhoneNumber = x.PhoneNumber,
                        Employee_id = x.Employee_id,
                        Employee_Name = (from employee in context.employee
                                         join comp in context.complaint on employee.Employee_Id equals comp.Employee_id
                                         where employee.Employee_Id == x.Employee_id
                                         select employee.Name).First(),
                        ReplyComments = x.ReplyComments,
                        ComplaintType = (ComplaintType)Enum.Parse(typeof(ComplaintType), x.ComplaintType),
                        ProductType =   (EmployeeDepartment)Enum.Parse(typeof(EmployeeDepartment), x.ProductType),
                        Status =        (ComplaintStatus)Enum.Parse(typeof(ComplaintStatus), x.Status)


                    }).ToList();

                    return userDetails;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Get Particular Complaint 
        /// </summary>
        /// <param name="id">It represents the complaint Id as it is unique for each complain.</param>
        /// <returns>Detals of the Complaints</returns>
        public ComplaintForm GetComplaint(int id)
        {
            using (var context = new testdbEntities1())
            {

                var userComplaintsTemporary = context.complaint.Where(x => x.Complaint_Id == id).FirstOrDefault();

                if(userComplaintsTemporary != null)
                {

                    //to get the employeeId with the given complaint Id 
                    var employeeId = context.complaint.Where(x => x.Complaint_Id == id)
                                                    .Select(x => x.Employee_id).First();

                    var employeeName = (from employee in context.employee
                                        join comp in context.complaint on employee.Employee_Id equals comp.Employee_id
                                        where employee.Employee_Id == employeeId
                                        select employee.Name).FirstOrDefault();



                    ComplaintForm complaintDetails = new ComplaintForm()
                    {
                        Complaint_id = id,
                        Employee_Name = employeeName,
                        User_Id = userComplaintsTemporary.User_Id,
                        Date_assigned = userComplaintsTemporary.Date_assigned,
                        ComplaintDescription = userComplaintsTemporary.ComplaintDescription,
                        PhoneNumber = userComplaintsTemporary.PhoneNumber,
                        Employee_id = userComplaintsTemporary.Employee_id,
                        ReplyComments = userComplaintsTemporary.ReplyComments,
                        ComplaintType = (ComplaintType)Enum.Parse(typeof(ComplaintType), userComplaintsTemporary.ComplaintType),
                        ProductType = (EmployeeDepartment)Enum.Parse(typeof(EmployeeDepartment), userComplaintsTemporary.ProductType),
                        Status = (ComplaintStatus)Enum.Parse(typeof(ComplaintStatus), userComplaintsTemporary.Status)
                    };

                    return complaintDetails;
                }
                else
                {
                    return null;
                }

            }
        }

        /// <summary>
        /// Returns all the pending Complaint for an User
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public List<ComplaintForm> GetPendingComplaint(string email)
        {
            using (var context = new testdbEntities1())
            {

                var userId = (from user in context.users
                              where user.EmailId == email
                              select user.UserId).First();

                var value = context.complaint.Where(x => x.User_Id == userId).FirstOrDefault();

                if (value != null)
                {
                    //to get all the Details of the pending complaints of logged in User
                    var userComplaintsTemporary = context.complaint.Where(x => x.User_Id == userId && x.Status != "Solved").ToList();


                    var result = userComplaintsTemporary.Select(x => new Model.ComplaintForm()
                    {
                        Complaint_id = x.Complaint_Id,
                        User_Id = x.User_Id,
                        Date_assigned = x.Date_assigned,
                        ComplaintDescription = x.ComplaintDescription,
                        PhoneNumber = x.PhoneNumber,
                        Employee_id = x.Employee_id,
                        Employee_Name = (from employee in context.employee
                                         join comp in context.complaint on employee.Employee_Id equals comp.Employee_id
                                         where employee.Employee_Id == x.Employee_id
                                         select employee.Name).First(),
                        ReplyComments = x.ReplyComments,
                        ComplaintType = (ComplaintType)Enum.Parse(typeof(ComplaintType), x.ComplaintType),
                        ProductType = (EmployeeDepartment)Enum.Parse(typeof(EmployeeDepartment), x.ProductType),
                        Status = (ComplaintStatus)Enum.Parse(typeof(ComplaintStatus), x.Status)

                    }).ToList();

                    return result;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
