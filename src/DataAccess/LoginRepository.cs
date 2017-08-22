using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using Common.Authentication;
using Contracts;
using Logger;
using Models;
using Utilities;

namespace DataAccess
{
    /// <summary>
    /// Represents login repository.
    /// </summary>
    public sealed class LoginRepository : RepositoryBase, ILoginRepository<LoginModel, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginRepository"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="authenticationSession">Authentication session.</param>
        public LoginRepository(dentistEntities entity, IAuthenticationSession authenticationSession) :
            base(entity, authenticationSession)
        {
        }

        /// <summary>
        /// Is login information valid.
        /// </summary>
        /// <param name="login">Login name.</param>
        /// <param name="password">The password.</param>
        /// <param name="loginId">Login id.</param>
        /// <returns>Returns is login information valid.</returns>
        public bool IsLoginValid(string login, SecureString password, ref int loginId)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = from c in this.Entities.Logins
                        where c.LoginName == login
                        select c;

                watch.Stop();

                Log.Debug(string.Format("Login has been received from the datasource. Took {0}", watch.Elapsed));

                if (!q.Any())
                {
                    Log.Error("There are no records returned.");
                    return false;
                }

                var firstLogin = q.FirstOrDefault();

                loginId = firstLogin.ID;

                Log.Debug("Check is the user has permission to login to the system.");

                bool isCanLogin;

                if (firstLogin.IsSA)
                {
                    Log.Debug("User is a super administrator. SA user can login to the system.");
                    isCanLogin = true;
                }
                else
                {
                    isCanLogin = firstLogin.IsCanLogin && !firstLogin.IsDeleted;
                    Log.Debug(string.Format("User isn't a super administrator. Permission to the login is {0}", isCanLogin));
                }

                if (!isCanLogin)
                {
                    return false;
                }

                using (var securePassword = new SecureString())
                {
                    var chars = firstLogin.Password.ToList();
                    foreach (var t in chars)
                    {
                        securePassword.AppendChar(t);
                    }

                    chars.Clear();

                    using (var md5Hash = MD5.Create())
                    {
                        var isLogined = new Security().VerifyMd5Hash(md5Hash, password, securePassword);
                        Log.Debug(string.Format("Credentials for the requested user is {0}", isLogined));

                        return isLogined;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return false;
            }
        }

        /// <summary>
        /// Deactivate and hide logins by staff Id
        /// </summary>
        /// <param name="staffId">Identification of the staff.</param>
        public void DeactivateAndHideLoginsByStaffId(int staffId)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var query = from c in this.Entities.Logins
                            where c.StaffID == staffId
                            select c;

                foreach (var login in query)
                {
                    login.IsDeleted = true;
                    login.IsCanLogin = false;
                }

                watch.Stop();

                Log.Debug(string.Format("All logins for staff {0} have been deactivated and hidden. Took {1}", staffId, watch.Elapsed));
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Get all records.
        /// </summary>
        /// <returns>List of all records.</returns>
        public IEnumerable<LoginModel> GetAll()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = from c in this.Entities.Logins
                        join r in this.Entities.Roles on c.RoleInSystem equals r.ID
                        join s in this.Entities.Staffs on c.StaffID equals s.ID
                        select new LoginModel
                        {
                            Id = c.ID,
                            LoginName = c.LoginName,
                            Password = string.Empty,
                            ConfirmPassword = string.Empty,
                            RoleInSystem = c.RoleInSystem,
                            RoleNameInSystem = r.Name,
                            IsHaveToChangePass = c.IsHaveToChangePass,
                            IsCanLogin = c.IsCanLogin,
                            IsSa = c.IsSA,
                            StaffId = c.StaffID,
                            StaffName = (s.SurName ?? string.Empty) + " " + (s.FirstName ?? string.Empty) + " " + (s.MiddleName ?? string.Empty),
                            IsDeleted = c.IsDeleted,
                            Changed = c.Changed,
                            ChangedBy = c.ChangedBy,
                            Created = c.Created,
                            CreatedBy = c.CreatedBy
                        };

                watch.Stop();

                Log.Debug(string.Format("All logins have been received. Count is {0}. Took {1}", q.Count(), watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Get all records include not mapped (not mapped records were created during the installation).
        /// </summary>
        /// <returns>List of all records.</returns>
        public IEnumerable<LoginModel> GetAllIncludeNotMapped()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = from c in this.Entities.Logins
                        join r in this.Entities.Roles on c.RoleInSystem equals r.ID
                        select new LoginModel
                        {
                            Id = c.ID,
                            LoginName = c.LoginName,
                            Password = c.Password,
                            RoleNameInSystem = r.Name,
                            RoleInSystem = c.RoleInSystem,
                            IsCanLogin = c.IsCanLogin,
                            IsSa = c.IsSA,
                            IsDeleted = c.IsDeleted,
                            Changed = c.Changed,
                            ChangedBy = c.ChangedBy,
                            Created = c.Created,
                            CreatedBy = c.CreatedBy
                        };

                watch.Stop();

                Log.Debug(string.Format("All logins include not mapped have been received. Count is {0}. Took {1}", q.Count(), watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Get all records except deleted record.
        /// </summary> 
        /// <returns>List of all non-deleted records.</returns>
        public IEnumerable<LoginModel> GetAllExceptDeleted()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = this.GetAll().Where(a => !a.IsDeleted && !a.IsSa);

                watch.Stop();

                Log.Debug(string.Format("Logins except deleted have been received. Took {0}", watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Get all records except deleted record.
        /// </summary>
        /// <returns>List of all records.</returns>
        public IEnumerable<LoginModel> GetAllSuperAdministratorsExceptDeleted()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = this.GetAllIncludeNotMapped().Where(a => !a.IsDeleted && a.IsSa);

                watch.Stop();

                Log.Debug(string.Format("All sa except deleted have been received. Took {0}", watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Get record by id.
        /// </summary>
        /// <param name="id">Id of the record.</param>
        /// <returns>T class.</returns>
        public LoginModel GetById(int id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = (from c in this.Entities.Logins
                         join r in this.Entities.Roles on c.RoleInSystem equals r.ID
                         join s in this.Entities.Staffs on c.StaffID equals s.ID
                         where c.ID == id
                         where !c.IsSA
                         select new LoginModel
                         {
                             Id = c.ID,
                             LoginName = c.LoginName,
                             Password = string.Empty,
                             ConfirmPassword = string.Empty,
                             RoleInSystem = c.RoleInSystem,
                             RoleNameInSystem = r.Name,
                             IsHaveToChangePass = c.IsHaveToChangePass,
                             IsCanLogin = c.IsCanLogin,
                             IsSa = c.IsSA,
                             StaffId = c.StaffID,
                             StaffName = (s.SurName ?? string.Empty) + " " + (s.FirstName ?? string.Empty) + " " + (s.MiddleName ?? string.Empty),
                             IsDeleted = c.IsDeleted,
                             Changed = c.Changed,
                             ChangedBy = c.ChangedBy,
                             Created = c.Created,
                             CreatedBy = c.CreatedBy
                         }).SingleOrDefault();

                watch.Stop();

                Log.Debug(string.Format("The login with id {0} have been received. Took {1}", id, watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return new LoginModel();
            }
        }

        /// <summary>
        /// Add a new record.
        /// </summary>
        /// <param name="tclass">Model class.</param>
        public void Add(LoginModel tclass)
        {
            try
            {
                string preparedPassword;

                var watch = new Stopwatch();
                watch.Start();

                using (var md5Hash = MD5.Create())
                {
                    preparedPassword = new Security().GetMd5Hash(md5Hash, tclass.Password);
                }

                var userId = this.AuthenticationSession.GetUserId();

                var entity = new Login
                {
                    LoginName = tclass.LoginName,
                    Password = preparedPassword,
                    IsCanLogin = tclass.IsCanLogin,
                    IsHaveToChangePass = tclass.IsHaveToChangePass,
                    RoleInSystem = tclass.RoleInSystem,
                    StaffID = tclass.StaffId,
                    IsDeleted = false,
                    IsSA = tclass.IsSa,
                    CreatedBy = userId,
                    Created = DateTime.Now,
                    ChangedBy = userId,
                    Changed = DateTime.Now
                };

                this.Entities.AddToLogins(entity);

                watch.Stop();

                Log.Debug(string.Format("A new login have been added. Took {0}", watch.Elapsed));
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Update record.
        /// </summary>
        /// <param name="tclass">Model class.</param>
        public void Update(LoginModel tclass)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var query = (from c in this.Entities.Logins
                             where c.ID == tclass.Id
                             select c).SingleOrDefault();

                var userId = this.AuthenticationSession.GetUserId();

                if (query != null)
                {
                    query.LoginName = tclass.LoginName;
                    if (!tclass.Password.IsNullOrEmpty())
                    {
                        using (var md5Hash = MD5.Create())
                        {
                            tclass.Password = new Security().GetMd5Hash(md5Hash, tclass.Password);
                        }

                        query.Password = tclass.Password;
                    }

                    query.IsCanLogin = tclass.IsCanLogin;
                    query.IsHaveToChangePass = tclass.IsHaveToChangePass;
                    query.RoleInSystem = tclass.RoleInSystem;
                    query.StaffID = tclass.StaffId;
                    query.IsSA = tclass.IsSa;
                    query.IsDeleted = tclass.IsDeleted;
                    query.ChangedBy = userId;
                    query.Changed = DateTime.Now;
                }

                watch.Stop();

                Log.Debug(string.Format("The login with id {0} has been updated. Took {1}", tclass.Id, watch.Elapsed));
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Delete record.
        /// </summary>
        /// <param name="id">The identification.</param>
        public void Delete(int id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var query = (from c in this.Entities.Logins
                             where c.ID == id && !c.IsSA
                             select c).SingleOrDefault();

                this.Entities.DeleteObject(query);

                watch.Stop();

                Log.Debug(string.Format("The login with id {0} has been deleted. Took {1}", id, watch.Elapsed));
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Try hide record.
        /// </summary>
        /// <param name="id">The identification.</param>
        /// <returns>Returns true if record was marked as deleted; otherwise, false.</returns>
        public bool TryHide(int id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var query = (from c in this.Entities.Logins
                             where c.ID == id
                             select c).SingleOrDefault();

                var userId = this.AuthenticationSession.GetUserId();

                if (query != null)
                {
                    query.IsDeleted = true;
                    query.ChangedBy = userId;
                    query.Changed = DateTime.Now;
                }

                watch.Stop();

                Log.Debug(string.Format("The login with id {0} has been hided. Took {1}", id, watch.Elapsed));

                return true;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return false;
            }
        }
    }
}
