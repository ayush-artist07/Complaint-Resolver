using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintResolver.Model;
using ComplaintResolver.DB.Utilities;

namespace ComplaintResolver.DB.DBOperation
{
    public class UserRepositry
    {
        /// <summary>
        /// Checks wether the email Id entered by the user already exists 
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true if email exists else false</returns>
        public bool DoesEmailExists(string email)
        {
            using (var context = new testdbEntities1())
            {
                var v = context.users.Where(x => x.EmailId == email).FirstOrDefault();

                return v == null ? false : true;
            }
        }

        /// <summary>
        /// Adds the Sign up details of the user to the Database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>User ID</returns>
        public int AddUser(UserDetail model)
        {
            using (var context = new testdbEntities1())
            {
                users u = new users()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailId = model.EmailId,
                    Password = model.Password,
                    DateOfBirth = model.DateOfBirth,
                    IsEmailVerified = model.IsEmailVerified,
                    ActivationCode = model.ActivationCode
                };

                context.users.Add(u);
                context.SaveChanges();

                return u.UserId;
            }
        }

        /// <summary>
        /// This method is used to verif email of the User using Guid
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if email is verified else false</returns>
        public bool VerifyAccount(string id)
        {
            bool status = false;
            using (var context = new testdbEntities1())
            {
                context.Configuration.ValidateOnSaveEnabled = false; // to not recheck confirm password again

                string guid = (new Guid(id)).ToString();

                var isExisting = context.users.Where(x => x.ActivationCode == guid).FirstOrDefault();

                if (isExisting != null)
                {
                    isExisting.IsEmailVerified = 1;
                    context.SaveChanges();

                    status = true;
                }
            };
            return status;
        }

        /// <summary>
        /// Checks whether email password entered exists in database or not
        /// </summary>
        /// <param name="login"></param>
        /// <returns>true if login credentials are right else false</returns>
        public bool Check(Login login)
        {
            using (var context = new testdbEntities1())
            {
                bool status = false;
                var email = context.users.Where(x => x.EmailId == login.EmailId).FirstOrDefault();

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
    }
}




