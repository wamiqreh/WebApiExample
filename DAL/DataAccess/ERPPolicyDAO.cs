#region Modification History
// Created By:  Mirza Fahad Ali Baig
// Created On:  2-May-2013
// Description: Class for Generic Methods To Check the Policy against users & Companies
// ****************************** Modifications *********************************

// ******************************************************************************
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Data;

using System.Web;


using DAL.Provider;
using Helper;

namespace DAL.DataAccess
{
    public class ERPPolicyDAO : IDisposable
    {
        #region Constants
        private static readonly string SELECT_POLICY_DETAILS_FOR_USERS = "SELECT DESCR, POLCY, ACTIV, DEFVAL, " +
                    "CASE PLEVL WHEN 'C' THEN DEFVAL ELSE (SELECT VAL FROM SAL_POS_USER_POLICY WHERE POLCY =SPP.POLCY AND POLUSRID ='{0}' ) END AS USERLEVEL " +
                    "FROM SAL_POS_POLICY SPP WHERE COMPC ='{1}' AND BRNCH ='{2}' AND DESCR ='{3}'";
        private static readonly string SELECT_POLICY_DETAILS = "SELECT * FROM SAL_POS_POLICY WHERE COMPC ='{0}' AND BRNCH ='{1}' AND DESCR ='{2}'";


        #endregion

        #region Variables
        private ConnClass my_func = null;
        DBHelper myRetrunFunc = null;

        #endregion

        #region Constructor
        public ERPPolicyDAO()
        {
            my_func = new ConnClass();
            myRetrunFunc = new DBHelper();
        }

        #endregion

        #region Functions
        public bool IsUserAllowOrNot(params object[] param)
        {
            try
            {
                DataTable polcyDT = myRetrunFunc.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_POLICY_DETAILS_FOR_USERS), param)).Tables[0];
                if (polcyDT != null && polcyDT.Rows.Count > 0)
                {
                    if ((polcyDT.Rows[0]["ACTIV"].ToString() == "Y") &&
                        ((polcyDT.Rows[0]["DEFVAL"].ToString() == "Y" && string.IsNullOrEmpty(polcyDT.Rows[0]["USERLEVEL"].ToString()))
                            || (polcyDT.Rows[0]["USERLEVEL"].ToString() == "Y")))
                        return true;
                    else
                        return false;
                }
                else
                    return false;

            }
            catch (Exception exception)
            {
                FGLogger.PrintError(exception);
                return false;
            }
        }

        public bool IsPolicyEnableOrNot(params object[] param)
        {
            try
            {
                DataTable polcyDT = myRetrunFunc.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_POLICY_DETAILS), param)).Tables[0];
                if (polcyDT != null && polcyDT.Rows.Count > 0)
                {
                    if (polcyDT.Rows[0]["ACTIV"].ToString() == "Y" && polcyDT.Rows[0]["DEFVAL"].ToString() == "Y")
                        return true;
                    else
                        return false;
                }
                else
                    return false;

            }
            catch (Exception exception)
            {
                FGLogger.PrintError(exception);
                return false;
            }

        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            my_func = null;
            myRetrunFunc = null;
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
