using Common.Utilities;
using DAL.Provider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;


public class LoginDMLs : IDisposable
{
    #region variables
    DBHelper _dbHelper = null;
    #endregion
    #region login
    [Description("Update Retry Counter in case of wrong credentials")]
    public void IncreaseUserLockCounter(object userId)
    {
        try
        {
            using (ConnClass DB = new ConnClass())
            {
               // DB.Execute_DML( string.Format("UPDATE SEC_USERNAME SET LOCKL  = LOCKL + 1 WHERE ID = {0}", userId));

            }
        }
        catch (Exception exception)
        {
            Logger.CreateLog(exception.StackTrace);
            throw;
        }
    }

    [Description("User credentials validation for login")]
    public UserModel GetLogininfoByCredentials(string emailAddress, string password)
    {
        try
        {
            using (ConnClass DB = new ConnClass())
            {
                DataTable dt = DB.MyDataTable( string.Format(@"select * from (
SELECT UI.USRID ID, UI.USRID USERCODE, UI.DESCR NAME, UI.PASWD PASSWORD, UI.ACTIV ACTIVEYN, UI.LOCKL LOCKCOUNTER, UI.EMAIL EMAILID, UI.EXPDT NEXTEXPIRYDATE,
                               UI.CONTR LOCKAFTERHOWMANYATTEMPTS, UI.EDATE EFFECTIVEDATE, UI.PLIFE PASSWORDEXPIREAFTERHOWMANYDAYS, UI.ULEVL USERLEVEL, UI.EDATE LOGINALLOWEDFROMTIME,
                               UI.EXPDT LOGINALLOWEDTOTIME, SUC.COMPC AS COMPANYID, SAL_CODENAME('COMPC',SUC.COMPC,NULL)  AS COMPANYNAME, SUB.BRNCH AS OFFICEID, SAL_CODENAME('LCODE',SUB.BRNCH,NULL) AS OFFICENAME
                               FROM SEC_USERNAME UI LEFT JOIN SEC_USERBRCH SUB ON UI.USRID =  SUB.USRID  LEFT JOIN SEC_USERCMPN SUC ON UI.USRID = SUC.USRID
                               inner join com_location loc on loc.lcode = SUB.BRNCH
                               WHERE LOWER(UI.DESCR) = '{0}'    
                               order by loc.deflt desc) 
                               where ROWNUM = 1 ", emailAddress.Trim().ToLower()));
                return dt.AsEnumerable().Select(
                    row => new UserModel
                    {

                        Id = row.Field<object>("ID"),
                        Name = row.Field<object>("NAME"),
                        Password = row.Field<object>("PASSWORD"),
                        ActiveYN = row.Field<object>("ACTIVEYN"),
                        LockCounter = row.Field<object>("LOCKCOUNTER"),
                        Email = row.Field<object>("EMAILID"),
                        NextExpiryDate = row.Field<object>("NEXTEXPIRYDATE"),
                        LockAfterHowManyAttempts = row.Field<object>("LOCKAFTERHOWMANYATTEMPTS"),
                        EffectiveDate = row.Field<object>("EFFECTIVEDATE"),
                        PasswordExpiredAfterHowManyNoOfDays = row.Field<object>("PASSWORDEXPIREAFTERHOWMANYDAYS"),
                        UserLevel = row.Field<object>("USERLEVEL"),
                        LoginAllowedFromTime = row.Field<object>("LOGINALLOWEDFROMTIME"),
                        LoginAllowedToTime = row.Field<object>("LOGINALLOWEDTOTIME"),
                        CompanyInfo = new CompanyModel() { Id = row.Field<object>("COMPANYID"), Name = row.Field<object>("COMPANYNAME") },
                        OfficeInfo = new OfficeModel() { Id = row.Field<object>("OFFICEID"), Name = row.Field<object>("OFFICENAME") }
                    }).FirstOrDefault();
            }
        }
        catch (Exception exception)
        {
            Logger.CreateLog(exception.StackTrace);
            throw;
        }
    }

    [Description("Reset User Login Counter after successfull login")]
    public decimal TrackUserLoginHistory(object userId, object companyId, object officeId)
    {
        try
        {
            using (ConnClass DB = new ConnClass())
            {
                // Reset Counter
                DB.Execute_DML( string.Format("UPDATE SEC_USERNAME SET LOCKL  = 0 WHERE USRID = {0} ", userId));

                // Get Id
                string Qry = @"select max(to_number(SERNO))+1 SERNO from sec_lhistory where  ascii(lower(usrid) ) < '97'";
                DataTable result = new DataTable();

                result= _dbHelper.DataAdapter(CommandType.Text, Qry).Tables[0];

                var id = result.Rows[0]["SERNO"].ToString();

                // Track Login Details
                DB.Execute_DML( string.Format(DAL.Utils.Utilities.GenerateProperTableName("INSERT INTO sec_lhistory (SERNO,USRID,SYSTM,LDATE,LTIME,WSTNO,STATS,PASWD,OUTDT) VALUES ({0}, {1},'CLOUD',SYSDATE,SYSDATE, 'CLOUD USER','Y','',SYSDATE)"), id, userId));


                return Convert.ToDecimal(id);
            }
        }
        catch (Exception exception)
        {
            Logger.CreateLog(exception.StackTrace);
            throw;
        }
    }


    //  select brnch, com_codename('BRNCH', BRNCH, NULL) NAME from SEC_USERBRCH where usrid ='1'
    [Description("Offces Assigned to Users")]
    public DataTable GetUsersOffices(string userid)
    {
        try
        {
            using (ConnClass DB = new ConnClass())
            {
                DataTable dt = DB.MyDataTable( string.Format(DAL.Utils.Utilities.GenerateProperTableName(@"  select '' ID,C.DESCR NAME,S.BRNCH MAPCODE ,C.SRVR_ADRES SEVERADDRESS from SEC_USERBRCH S INNER JOIN COM_LOCATION C  ON C.LCODE=S.BRNCH where S.usrid ='{0}'"), userid));
                return dt;
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

    }
    #endregion
}



