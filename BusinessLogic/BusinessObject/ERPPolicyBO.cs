#region Modification History
// Created By:  Mirza Fahad Ali Baig
// Created On:  27th May, 2013
// Description: 
// ****************************** Modifications *********************************

// ******************************************************************************
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

using Common.Enumeration;
using DAL.DataAccess;

namespace BusinessLogic.BusinessObject
{
    public class ERPPolicyBO : IDisposable
    {
        #region Variables
        ERPPolicyDAO _policyDAO;
        #endregion

        #region Constructor
        public ERPPolicyBO()
        {
            _policyDAO = new ERPPolicyDAO();
        }
        #endregion

        #region Functions
        public bool IsPolicyEnableOrNot(params object[] param)
        {
            return _policyDAO.IsPolicyEnableOrNot(param);
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            _policyDAO.Dispose();
            _policyDAO = null;
        }
        #endregion
    }
}
