#region Modification History
// Created By:  Mirza Fahad Ali Baig
// Created On:  3rd June, 2013
// Description: 
// ****************************** Modifications *********************************

// ******************************************************************************
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using DAL.Provider;
using Helper;

using System.Collections;
using Common.Utilities;

namespace Fourgen.POS.Services.DataAccess
{
    public class UserDAO : IDisposable
    {
        #region Constants
        private static readonly string SELECT_USER_BY_CREDENTIAL = @"SELECT U.USRID, EXPDT, UC.COMPC, UB.BRNCH,
                 SAL_CODENAME('COMPC',UC.COMPC,NULL) AS COMPANY_NAME, SAL_CODENAME('BRNCH',UB.BRNCH,NULL) AS BRANCH_NAME
                 FROM SEC_USERNAME U                 
                 LEFT JOIN SEC_USERCMPN UC
                      ON U.USRID = UC.USRID 
                 LEFT JOIN SEC_USERBRCH UB
                      ON U.USRID = UB.USRID 
                 WHERE UPPER(U.DESCR) = '{0}' 
                    AND U.PASWD = (SELECT COM_BALANCE.PayableZ(UPPER('{1}') || U.USRID || U.DESCR) FROM DUAL)";
        private static readonly string SELECT_USER_BY_CREDENTIAL_EMAIL = @"SELECT U.USRID, EXPDT, UC.COMPC, UB.BRNCH,
                 SAL_CODENAME('COMPC',UC.COMPC,NULL) AS COMPANY_NAME, SAL_CODENAME('BRNCH',UB.BRNCH,NULL) AS BRANCH_NAME
                 FROM SEC_USERNAME U                 
                 LEFT JOIN SEC_USERCMPN UC
                      ON U.USRID = UC.USRID 
                 LEFT JOIN SEC_USERBRCH UB
                      ON U.USRID = UB.USRID 
                 WHERE UPPER(U.EMAIL) = '{0}' 
                    AND U.PASWD = (SELECT COM_BALANCE.PayableZ(UPPER('{1}') || U.USRID || U.DESCR) FROM DUAL)";

        //private static readonly string SELECT_USER_BY_CREDENTIAL = "SELECT U.USRID, U.EXPDT, " +
        //       "(SELECT UC.COMPC FROM SEC_USERCMPN UC WHERE UC.USRID = U.USRID AND ROWNUM =1) AS COMPANY, " +
        //       "(SELECT UB.BRNCH FROM SEC_USERBRCH UB WHERE UB.USRID = U.USRID AND ROWNUM =1) AS BRANCH " +
        //       "FROM SEC_USERNAME U WHERE UPPER(U.DESCR) = '{0}' AND U.PASWD = " +
        //       "(SELECT COM_BALANCE.PayableZ(UPPER('{1}') || U.USRID || U.DESCR) FROM DUAL)";


        #endregion

        #region Variables
        private ConnClass _connClass = null;
        DBHelper _dbHelper = null;

        #endregion

        #region Constructor
        public UserDAO()
        {
            _connClass = new ConnClass();
            _dbHelper = new DBHelper();
        }
        #endregion

        #region Functions
        public DataTable GetUserByCredentials(string username, string password)
        {
            try
            {
                FGLogger.PrintDebug("Login->" +  string.Format(SELECT_USER_BY_CREDENTIAL, username, password));
                return _dbHelper.DataAdapter(CommandType.Text,  string.Format(SELECT_USER_BY_CREDENTIAL, username, password)).Tables[0];
            }
            catch (Exception exception)
            {
                FGLogger.PrintError(exception);
                return null;
            }
        }
        public DataTable GetUserByCredentialsEmail(string email, string password)
        {
            try
            {
                FGLogger.PrintDebug("Login->" +  string.Format(DAL.Utils.Utilities.GenerateProperTableName(SELECT_USER_BY_CREDENTIAL_EMAIL), email, password));
                return _dbHelper.DataAdapter(CommandType.Text,  string.Format(DAL.Utils.Utilities.GenerateProperTableName(SELECT_USER_BY_CREDENTIAL_EMAIL), email, password)).Tables[0];
            }
            catch (Exception exception)
            {
                FGLogger.PrintError(exception);
                return null;
            }
        }
        public bool HHTAllowUserToDemand(params object[] param)
        {
           try
            {
                using (DAL.DataAccess.ERPPolicyDAO _objErpPolicyDAO = new DAL.DataAccess.ERPPolicyDAO())
                {
                  // User, COMPC, BRNCH      
                   return _objErpPolicyDAO.IsUserAllowOrNot(param[0], param[1], param[2],
                            Util.GetEnumDescription(Common.Enumeration.HHTPolicy.HHTAllowUserToDemand));
                }
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception);
                return false;
            }
        }
        public bool HHTAllowUserToTransferOut(params object[] param)
        {
            try
            {
                using (DAL.DataAccess.ERPPolicyDAO _objErpPolicyDAO = new DAL.DataAccess.ERPPolicyDAO())
                {
                    // User, COMPC, BRNCH      
                    return _objErpPolicyDAO.IsUserAllowOrNot(param[0], param[1], param[2],
                             Util.GetEnumDescription(Common.Enumeration.HHTPolicy.HHTAllowUserToTransferOut));
                }
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception);
                return false;
            }
        }
        public bool HHTAllowUserToTransferIn(params object[] param)
        {
            try
            {
                using (DAL.DataAccess.ERPPolicyDAO _objErpPolicyDAO = new DAL.DataAccess.ERPPolicyDAO())
                {
                    // User, COMPC, BRNCH      
                    return _objErpPolicyDAO.IsUserAllowOrNot(param[0], param[1], param[2],
                             Util.GetEnumDescription(Common.Enumeration.HHTPolicy.HHTAllowUserToTransferIn));
                }
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception);
                return false;
            }
        }


        public bool HHTAllowUserToItemInfoPrint(params object[] param)
        {
            try
            {
                using (DAL.DataAccess.ERPPolicyDAO _objErpPolicyDAO = new DAL.DataAccess.ERPPolicyDAO())
                {
                    // User, COMPC, BRNCH      
                    return _objErpPolicyDAO.IsUserAllowOrNot(param[0], param[1], param[2],
                             Util.GetEnumDescription(Common.Enumeration.HHTPolicy.HHTAllowUserToItemInfoPrint));
                }
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception);
                return false;
            }
        }
        public bool HHTAllowUserToReceiving(params object[] param)
        {
            try
            {
                using (DAL.DataAccess.ERPPolicyDAO _objErpPolicyDAO = new DAL.DataAccess.ERPPolicyDAO())
                {
                    // User, COMPC, BRNCH      
                    return _objErpPolicyDAO.IsUserAllowOrNot(param[0], param[1], param[2],
                             Util.GetEnumDescription(Common.Enumeration.HHTPolicy.HHTAllowUserToReceiving));
                }
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception);
                return false;
            }
        }
        public bool HHTAllowUserToGapZap(params object[] param)
        {
            try
            {
                using (DAL.DataAccess.ERPPolicyDAO _objErpPolicyDAO = new DAL.DataAccess.ERPPolicyDAO())
                {
                    // User, COMPC, BRNCH      
                    return _objErpPolicyDAO.IsUserAllowOrNot(param[0], param[1], param[2],
                             Util.GetEnumDescription(Common.Enumeration.HHTPolicy.HHTAllowUserToGapZap));
                }
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception);
                return false;
            }
        }


        public bool HHTAllowUserToCyclicStock(params object[] param)
        {
            try
            {
                using (DAL.DataAccess.ERPPolicyDAO _objErpPolicyDAO = new DAL.DataAccess.ERPPolicyDAO())
                {
                    // User, COMPC, BRNCH      
                    return _objErpPolicyDAO.IsUserAllowOrNot(param[0], param[1], param[2],
                             Util.GetEnumDescription(Common.Enumeration.HHTPolicy.HHTAllowUserToCyclicStock));
                }
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception);
                return false;
            }
        }
        public bool HHTAllowUserToDamageDisposal(params object[] param)
        {
            try
            {
                using (DAL.DataAccess.ERPPolicyDAO _objErpPolicyDAO = new DAL.DataAccess.ERPPolicyDAO())
                {
                    // User, COMPC, BRNCH      
                    return _objErpPolicyDAO.IsUserAllowOrNot(param[0], param[1], param[2],
                             Util.GetEnumDescription(Common.Enumeration.HHTPolicy.HHTAllowUserToDamageDisposal));
                }
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception);
                return false;
            }
        }
        public bool HHTAllowUserToBinStock(params object[] param)
        {
            try
            {
                using (DAL.DataAccess.ERPPolicyDAO _objErpPolicyDAO = new DAL.DataAccess.ERPPolicyDAO())
                {
                    // User, COMPC, BRNCH      
                    return _objErpPolicyDAO.IsUserAllowOrNot(param[0], param[1], param[2],
                             Util.GetEnumDescription(Common.Enumeration.HHTPolicy.HHTAllowUserToBinStock));
                }
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception);
                return false;
            }
        }
        public bool HHTAllowUserToGoodsReturn(params object[] param)
        {
            try
            {
                using (DAL.DataAccess.ERPPolicyDAO _objErpPolicyDAO = new DAL.DataAccess.ERPPolicyDAO())
                {
                    // User, COMPC, BRNCH      
                    return _objErpPolicyDAO.IsUserAllowOrNot(param[0], param[1], param[2],
                             Util.GetEnumDescription(Common.Enumeration.HHTPolicy.HHTAllowUserToGoodsReturn));
                }
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception);
                return false;
            }
        }

        #endregion

        #region Dispose
        public void Dispose()
        {
            _connClass = null;
            _dbHelper = null;
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
