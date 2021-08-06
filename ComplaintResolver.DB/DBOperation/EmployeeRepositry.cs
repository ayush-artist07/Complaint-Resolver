using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintResolver.Model.Enums;
using ComplaintResolver.Model;
using ComplaintResolver.DB.Utilities;

namespace ComplaintResolver.DB.DBOperation
{
    public class EmployeeRepositry
    {
        /// <summary>
        /// Checks whether an email already exists for an employee
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true if email exists else false </returns>
        public bool DoesEmailExists(string email)
        {
            using (var context = new testdbEntities1())
            {
                var v = context.employee.Where(x => x.Email == email).FirstOrDefault();

                return v != null;
            }
        }

        /// <summary>
        /// To add a new employee to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Employee Id</returns>
        public int AddEmployee(EmployeeDetail model)
        {
            using (var context = new testdbEntities1())
            {
                employee emp = new employee()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    Phone = model.Phone,
                    Country = Convert.ToString(model.employeecountry),
                    Department = Convert.ToString(model.employeerole.Role) == "Admin" ? "Others" : Convert.ToString(model.Department),
                    PendingComplaints = 0,
                    CompletedComplaints = 0
                };

                context.employee.Add(emp);
                context.SaveChanges();

                return emp.Employee_Id;
            }
        }

        /// <summary>
        /// Add Employees Role in employee role table
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns>Role Id</returns>
        public int AddRole(int id, EmployeeDetail model)
        {
            using (var context = new testdbEntities1())
            {
                employee_role emp = new employee_role()
                {
                    Employee_Id = id,
                    Role = Convert.ToString(model.employeerole.Role)
                };

                context.employee_role.Add(emp);
                context.SaveChanges();

                return emp.Employee_Role_Id;
            }
        }

        /// <summary>
        /// Updates changes made to the Employee record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateEmployee(EmployeeDetail model)
        {
            using (var context = new testdbEntities1())
            {
                var employeeTemp = context.employee.Where(x => x.Employee_Id == model.EmployeeId).FirstOrDefault();

                if(employeeTemp != null)
                {
                    employeeTemp.Name = model.Name;
                    employeeTemp.Email = model.Email;
                    employeeTemp.Phone = model.Phone;
                    employeeTemp.Country = Convert.ToString(model.employeecountry);
                    employeeTemp.Department = Convert.ToString(model.Department);

                    var empRole = employeeTemp.employee_role.Where(x => x.Employee_Id == model.EmployeeId).FirstOrDefault();

                    empRole.Role =Convert.ToString(model.employeerole.Role);
                }
                else
                {
                    return 0;
                }

                context.SaveChanges();
                return employeeTemp.Employee_Id;
            }
        }


        /// <summary>
        /// Verifies the credential entered by employee for login does exist or not
        /// </summary>
        /// <param name="login"></param>
        /// <returns> true if user exists else false</returns>
        public bool check(Login login)
        {
            using (var context = new testdbEntities1())
            {
                bool status = false;
                var email = context.employee.Where(x => x.Email == login.EmailId).FirstOrDefault();

                if (email != null)
                {
                    if (string.Compare(EncryptPassword.Hash(login.Password), email.Password) == 0)
                    {
                        status = true;
                    }
                }
                return status;
            }
        }

        /// <summary>
        /// Returns Details of Logged in Employee
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public EmployeeDetail GetProfile(string email)
        {
            using (var context = new testdbEntities1())
            {
                var employee = context.employee.Where(x => x.Email == email).FirstOrDefault();

                EmployeeDetail employeeDetails = new EmployeeDetail()
                {
                    EmployeeId = employee.Employee_Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Phone = employee.Phone,
                    employeecountry = (EmployeeCountry)Enum.Parse(typeof(EmployeeCountry), employee.Country),
                    Department = (EmployeeDepartment)Enum.Parse(typeof(EmployeeDepartment), employee.Department),
                    PendingComplaints = employee.PendingComplaints,
                    CompletedComplaints = employee.CompletedComplaints

                };
                return employeeDetails;
            }
        }

        /// <summary>
        /// Deletes an employee from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteEmployee(int id)
        {
            using (var context = new testdbEntities1())
            {
                var employeeInfo = context.employee.FirstOrDefault(x => x.Employee_Id == id);

                if (employeeInfo != null)
                {
                    context.employee.Remove(employeeInfo);
                    context.SaveChanges();
                    return true;
                }
                return false;
            };
        }

        /// <summary>
        /// Returns all the Employees with their details
        /// </summary>
        /// <returns></returns>
        public List<EmployeeDetail> GetAllEmployees()
        {
            using (var context = new testdbEntities1())
            {
                var employee = context.employee.ToList();

                var result = employee.Select(x => new Model.EmployeeDetail()
                {
                    EmployeeId = x.Employee_Id,
                    Name = x.Name,
                    Email = x.Email,
                    Phone = x.Phone,
                    employeecountry = (EmployeeCountry)Enum.Parse(typeof(EmployeeCountry), x.Country),
                    Department = (EmployeeDepartment)Enum.Parse(typeof(EmployeeDepartment), x.Department),
                    PendingComplaints = x.PendingComplaints,
                    CompletedComplaints = x.CompletedComplaints

                }).ToList();

                return result;
            }
        }

        /// <summary>
        /// Returns all the complaints in the database
        /// </summary>
        /// <returns></returns>
        public List<ComplaintForm> GetAllComplaint()
        {
            using (var context = new testdbEntities1())
            {
                var value = (from employee in context.employee
                             join comp in context.complaint on employee.Employee_Id equals comp.Employee_id
                             select new { Id = comp.Complaint_Id, Name = employee.Name, Date = comp.Date_assigned, Phone = comp.PhoneNumber, Product = comp.ProductType, Status = comp.Status }).ToList();

                if (value != null)
                {

                    var result = value.Select(x => new Model.ComplaintForm()
                    {
                        Complaint_id = x.Id,
                        Employee_Name = x.Name,
                        Date_assigned = x.Date,
                        PhoneNumber = x.Phone,
                        ProductType = (EmployeeDepartment)Enum.Parse(typeof(EmployeeDepartment), x.Product),
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

        /// <summary>
        /// returns all the pending complaints in the database
        /// </summary>
        /// <returns>List of Complaints</returns>
        public List<ComplaintForm> GetPendingComplaints()
        {
            using (var context = new testdbEntities1())
            {
                var allPendingComplaintsTemp = context.complaint.Where(x => x.Status != "Solved").ToList();

                if (allPendingComplaintsTemp == null)
                {
                    return null;
                }
                else
                {
                    var allPendingComplaints = allPendingComplaintsTemp.Select(x => new ComplaintForm()
                    {
                        Complaint_id = x.Complaint_Id,
                        Employee_id = x.Employee_id,
                        Date_assigned = x.Date_assigned,
                        PhoneNumber = x.PhoneNumber,
                        ProductType = (EmployeeDepartment)Enum.Parse(typeof(EmployeeDepartment), x.ProductType)
                    }).ToList();

                    return allPendingComplaints;
                }
            }
        }

        /// <summary>
        /// List of complaints that has been solved
        /// </summary>
        /// <returns></returns>
        public List<ComplaintForm> GetCompletedComplaints()
        {
            using (var context = new testdbEntities1())
            {
                var allCompletedComplaintsTemp = context.complaint.Where(x => x.Status == "Solved").ToList();

                if (allCompletedComplaintsTemp == null)
                {
                    return null;
                }
                else
                {
                    var allCompletedComplaints = allCompletedComplaintsTemp.Select(x => new ComplaintForm()
                    {

                        Complaint_id = x.Complaint_Id,
                        Employee_id = x.Employee_id,
                        Date_assigned = x.Date_assigned,
                        PhoneNumber = x.PhoneNumber,
                        ProductType = (EmployeeDepartment)Enum.Parse(typeof(EmployeeDepartment), x.ProductType),

                    }).ToList();

                    return allCompletedComplaints;
                }
            }
        }

        /// <summary>
        /// Get Pending Complaints for an Employee
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public List<ComplaintForm> GetServicemanPendingComplaints(string email)
        {
            using (var context = new testdbEntities1())
            {
                var employeeId = context.employee.Where(x => x.Email == email)
                                 .Select(x => x.Employee_Id).FirstOrDefault();

                var complaintsTemp = context.complaint.Where(x => x.Employee_id == employeeId && x.Status != "Solved").ToList();

                if (complaintsTemp == null)
                {
                    return null;
                }
                else
                {
                    var complaints = complaintsTemp.Select(x => new Model.ComplaintForm()
                    {
                        Complaint_id = x.Complaint_Id,
                        User_Id = x.User_Id,
                        Date_assigned = x.Date_assigned,
                        ComplaintDescription = x.ComplaintDescription,
                        PhoneNumber = x.PhoneNumber,
                        Employee_id = x.Employee_id,
                        ReplyComments = x.ReplyComments == null ? "Not Available" : x.ReplyComments,
                        ComplaintType = (ComplaintType)Enum.Parse(typeof(ComplaintType), x.ComplaintType),
                        ProductType = (EmployeeDepartment)Enum.Parse(typeof(EmployeeDepartment), x.ProductType)


                    }).ToList();

                    return complaints;
                }
            }
        }

        /// <summary>
        /// GEt Completed Complaints for an Employee
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>

        public List<ComplaintForm> GetServicemanCompletedComplaints(string email)
        {
            using (var context = new testdbEntities1())
            {
                var employeeId = context.employee.Where(x => x.Email == email)
                                .Select(x => x.Employee_Id).FirstOrDefault();

                var complaintsTemp = context.complaint.Where(x => x.Employee_id == employeeId && x.Status == "Solved").ToList();

                if (complaintsTemp == null)
                {
                    return null;
                }
                else
                {
                    var complaints = complaintsTemp.Select(x => new Model.ComplaintForm()
                    {
                        Complaint_id = x.Complaint_Id,
                        User_Id = x.User_Id,
                        Date_assigned = x.Date_assigned,
                        ComplaintDescription = x.ComplaintDescription,
                        PhoneNumber = x.PhoneNumber,
                        Employee_id = x.Employee_id,
                        ReplyComments = x.ReplyComments == null ? "Not Available" : x.ReplyComments,
                        ComplaintType = (ComplaintType)Enum.Parse(typeof(ComplaintType), x.ComplaintType),
                        ProductType = (EmployeeDepartment)Enum.Parse(typeof(EmployeeDepartment), x.ProductType)

                    }).ToList();

                    return complaints;
                }
            }
        }

        /// <summary>
        /// Get All Complaints for an Employee
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public List<ComplaintForm> GetServicemanAllComplaints(string email)
        {
            using (var context = new testdbEntities1())
            {
                var employeeId = context.employee.Where(x => x.Email == email)
                    .Select(x => x.Employee_Id).FirstOrDefault();

                var complaintsTemp = context.complaint.Where(x => x.Employee_id == employeeId).ToList();

                if (complaintsTemp == null)
                {
                    return null;
                }
                else
                {
                    var result = complaintsTemp.Select(x => new Model.ComplaintForm()
                    {
                        Complaint_id = x.Complaint_Id,
                        User_Id = x.User_Id,
                        Date_assigned = x.Date_assigned,
                        ComplaintDescription = x.ComplaintDescription,
                        PhoneNumber = x.PhoneNumber,
                        Employee_id = x.Employee_id,
                        ReplyComments = x.ReplyComments == null ? "Not Available" : x.ReplyComments,
                        ComplaintType = (ComplaintType)Enum.Parse(typeof(ComplaintType), x.ComplaintType),
                        ProductType = (EmployeeDepartment)Enum.Parse(typeof(EmployeeDepartment), x.ProductType)

                    }).ToList();

                    return result;
                }
            }
        }

        /// <summary>
        /// Extracts all the feedbacks received for all the user
        /// </summary>
        /// <returns></returns>
        public List<Feedback> GetAllFeedbacks()
        {
            using (var context = new testdbEntities1())
            {
                var value = (from f in context.feedback
                             join c in context.complaint on f.Complaint_Id equals c.Complaint_Id
                             join e in context.employee on c.Employee_id equals e.Employee_Id
                             select new { Complaint_Id = c.Complaint_Id, Employee_ID = c.Employee_id, Name = e.Name,
                                        Date = c.Date_assigned, Product = c.ProductType, Message = f.Message, 
                                        Description = c.ComplaintDescription }).ToList();

                if (value != null)
                {
                    var result = value.Select(x => new Model.Feedback()
                    {
                        ComplaintId = x.Complaint_Id,
                        Message = x.Message,
                        complaints = new ComplaintForm()
                        {
                            Employee_id = x.Employee_ID,
                            Employee_Name = x.Name,
                            ComplaintDescription = x.Description,
                            ProductType = (EmployeeDepartment)Enum.Parse(typeof(EmployeeDepartment), x.Product),
                            Date_assigned = x.Date
                        }

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
