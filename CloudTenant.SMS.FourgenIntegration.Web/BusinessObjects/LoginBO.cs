using Common.Utilities;
using DAL;
using DAL.Provider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using Common.Models;
using BusinessLogic.BusinessObject;
using Common.Enumeration;

namespace SND.BusinessObjects
{
    public class LoginBO : IDisposable
    {
        #region variables
        DBHelper _dbHelper = null;
        UserBO _UserBO = new UserBO();
        DataTable dt = new DataTable();
        #endregion

        #region login
        [Description("Login user into the system")]
        public Dictionary<enLoginResponse, UserModel> GetLogininfoByCredentials(string username, string password)
        {
            Dictionary<enLoginResponse, UserModel> dictionaryOfUserInfo = new Dictionary<enLoginResponse, UserModel>();

            try
            {
                using (LoginDMLs _objUserDMLs = new LoginDMLs())
                {
                    var record = _objUserDMLs.GetLogininfoByCredentials(username, password);
                    if (record != null)
                    {
                        //Logger.PrintInfo("Password: " + Helper.Security.GenerateHashWithSalt(password, Helper.Security.GenerateHashWithSalt(record.Email, password)));
                        // Check if Password Match
                      
                     
                        dt = _UserBO.GetUserByCredentails(record.Name.ToString(), password);
                       
                           // UserBO.GetUserByCredentialsEmail(record.Email.ToString(), password);
                        if (dt.Rows.Count <= 0)
                        {
                            _objUserDMLs.IncreaseUserLockCounter(record.Id);
                            dictionaryOfUserInfo.Add(enLoginResponse.InValidUsernameOrPassword, record);
                        }

                        // Check if Account is not expired
                        else if (record.NextExpiryDate != null && Convert.ToDateTime(record.NextExpiryDate).Date < DateTime.Now.Date)
                        {
                            dictionaryOfUserInfo.Add(enLoginResponse.AccountExpired, record);
                        }
                        // Check if Login Retry conter limit Reached its max.
                        //else if (Convert.ToDecimal(record.LockCounter) > Convert.ToDecimal(record.LockAfterHowManyAttempts))
                        //{
                        //    dictionaryOfUserInfo.Add(enLoginResponse.AccountLock, record);
                        //}
                        // Check if Account is not in activate state (User confirmation is required)
                        else if (record.ActiveYN.ToString() == "N")
                        {
                            dictionaryOfUserInfo.Add(enLoginResponse.ActivationRequired, record);
                        }
                        else if (string.IsNullOrEmpty(record.OfficeInfo.Id.ToString()) && string.IsNullOrEmpty(record.OfficeInfo.Id.ToString()))
                        {
                            dictionaryOfUserInfo.Add(enLoginResponse.NoCompanyNoOffice, record);
                        }
                        else if (string.IsNullOrEmpty(record.CompanyInfo.Id.ToString()))
                        {
                            dictionaryOfUserInfo.Add(enLoginResponse.NoCompany, record);
                        }
                        else if (string.IsNullOrEmpty(record.OfficeInfo.Id.ToString()))
                        {
                            dictionaryOfUserInfo.Add(enLoginResponse.NoOffice, record);
                        }
                        else
                        {
                            // Check if user has time duration limitation for login
                            //if (record.LoginAllowedFromTime.HasValue && record.LoginAllowedToTime.HasValue)
                            //{
                            //    if (DateTime.Now.TimeOfDay > record.LoginAllowedToTime.Value.TimeOfDay ||
                            //        DateTime.Now.TimeOfDay < record.LoginAllowedFromTime.Value.TimeOfDay)
                            //    {
                            //        dictionaryOfUserInfo.Add(enLoginResponse.TimeDurationLock, record);
                            //    }
                            //}
                        }


                        if (dictionaryOfUserInfo.Count <= 0)
                        {
                            // Track User Login Time
                            //record.LoginCode = _objUserDMLs.TrackUserLoginHistory(record.Id, record.CompanyInfo.Id, record.OfficeInfo.Id);

                            // Fetch Active Fiscal Years for users
                            #region Commited due to future working
                            /* using (CommonDMLs _ObjCommonDMLs = new CommonDMLs())
                             {
                                 // Load Policies
                                 record.CompanyPolicyInfo = (CompanyPolicyModel)_ObjCommonDMLs.GetCompanytPolicy(record.CompanyInfo.Id, record.OfficeInfo.Id);
                                 record.CompanyPolicyInfo = record.CompanyPolicyInfo ?? new CompanyPolicyModel();

                                 record.ListOfFiscalYears = _ObjCommonDMLs.GetActiveFiscalYearList(record.CompanyInfo.Id ?? 0, record.OfficeInfo.Id ?? 0, record.CompanyPolicyInfo.DateFormat ?? "");
                                 if (record.ListOfFiscalYears != null && record.ListOfFiscalYears.Count() > 0)
                                 {
                                     record.FiscalYearInfo = new FiscalYearModel()
                                     {
                                         Id = record.ListOfFiscalYears[0].Id,
                                         ShortFiscalYearCode = record.ListOfFiscalYears[0].ShortFiscalYearCode,
                                         FullFiscalYearCode = record.ListOfFiscalYears[0].FullFiscalYearCode,
                                         FromDateClient = record.ListOfFiscalYears[0].FromDateClient,
                                         ToDateClient = record.ListOfFiscalYears[0].ToDateClient,
                                         FromDateServer = record.ListOfFiscalYears[0].FromDateServer,
                                         ToDateServer = record.ListOfFiscalYears[0].ToDateServer
                                     };

                                     // Get Work Date Information
                                     record.WorkDateInfo = _ObjCommonDMLs.GetWorkDateInfoByCalenderId(record.CompanyInfo.Id ?? 0, record.OfficeInfo.Id ?? 0, record.FiscalYearInfo.Id);
                                     record.WorkDateInfo.WorkDate = DateTime.ParseExact(record.WorkDateInfo.WorkDate, "dd-MMM-yy", System.Globalization.CultureInfo.InvariantCulture).ToString(Common.Helper.CTExtensions.ConvertUIFormatToCSharpFormat(record.CompanyPolicyInfo.DateFormat));

                                 }
                                 else
                                 {
                                     record.ListOfFiscalYears = new FiscalYearModel[] { };
                                     record.FiscalYearInfo = new FiscalYearModel();
                                     record.WorkDateInfo = new WorkDateModel();
                                 }

                             }*/
                            #endregion


                            // Set Object with success response before to send
                            dictionaryOfUserInfo.Add(enLoginResponse.Success, record);
                        }
                    }
                    else
                        dictionaryOfUserInfo.Add(enLoginResponse.InValidUsernameOrPassword, record);
                }

                return dictionaryOfUserInfo;
            }
            catch (Exception)
            {
                dictionaryOfUserInfo.Add(enLoginResponse.Failed, null);
                throw;
            }
        }

        [Description("Offces Assigned to Users")]
        public DataTable GetUsersOffices(string userid)
        {
            try
            {
                using (LoginDMLs _objUserDMLs = new LoginDMLs())
                {
               return     _objUserDMLs.GetUsersOffices(userid);
                }
            }
            catch (Exception exception)
            {
                Logger.CreateLog(exception.StackTrace);
                throw;
            }
        }
        #endregion


        #region Dispose
        public void Dispose()
        {

            GC.SuppressFinalize(this);
        }
        #endregion

    }
   

}