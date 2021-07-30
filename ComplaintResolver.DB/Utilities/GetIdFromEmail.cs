using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintResolver.DB.Utilities
{
    public static class GetIdFromEmail
    {
        public static string UserId(int userID)
        {
            using(var context = new testdbEntities1())
            {

                var employeeEmail = context.employee.Where(x => x.Employee_Id == userID)
                                    .Select(x => x.Email).FirstOrDefault();

                return employeeEmail;
            }
        }
    }
}
