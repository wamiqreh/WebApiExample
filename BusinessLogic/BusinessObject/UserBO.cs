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
using Fourgen.POS.Services.DataAccess;
using System.Collections;

namespace BusinessLogic.BusinessObject
{
    public class UserBO : IDisposable
    {
        #region Variables
        UserDAO _userDAO;
        #endregion

        #region Constructor
        public UserBO()
        {
            _userDAO = new UserDAO();
        }
        #endregion

        #region Functions
        public DataTable GetUserByCredentails(string username, string password)
        {
            return _userDAO.GetUserByCredentials(username, password);
        }
        public DataTable GetUserByCredentialsEmail(string email, string password)
        {
            return _userDAO.GetUserByCredentialsEmail(email, password);
        }
        public bool HHTAllowUserToDemand(params object[] param)
        {
            return _userDAO.HHTAllowUserToDemand(param);
        }
        public bool HHTAllowUserToTransferOut(params object[] param)
        {
            return _userDAO.HHTAllowUserToTransferOut(param);
        }
        public bool HHTAllowUserToTransferIn(params object[] param)
        {
            return _userDAO.HHTAllowUserToTransferIn(param);
        }

        public bool HHTAllowUserToItemInfoPrint(params object[] param)
        {
            return _userDAO.HHTAllowUserToItemInfoPrint(param);
        }
        public bool HHTAllowUserToReceiving(params object[] param)
        {
            return _userDAO.HHTAllowUserToReceiving(param);
        }
        public bool HHTAllowUserToGapZap(params object[] param)
        {
            return _userDAO.HHTAllowUserToGapZap(param);
        }

        public bool HHTAllowUserToDamageDisposal(params object[] param)
        {
            return _userDAO.HHTAllowUserToDamageDisposal(param);
        }
        public bool HHTAllowUserToBinStock(params object[] param)
        {
            return _userDAO.HHTAllowUserToBinStock(param);
        }
        public bool HHTAllowUserToGoodsReturn(params object[] param)
        {
            return _userDAO.HHTAllowUserToGoodsReturn(param);
        }

        public bool HHTAllowUserToCyclicStock(params object[] param)
        {
            return _userDAO.HHTAllowUserToCyclicStock(param);
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            _userDAO.Dispose();
            _userDAO = null;

        }
        #endregion
    }
}
