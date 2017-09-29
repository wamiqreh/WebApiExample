#region Modification History
// Created By:  Mirza Fahad Ali Baig
// Created On:  31th May, 2013
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
namespace BusinessLogic.BusinessObject
{
    public class ListOfValuesBO : IDisposable
    {
        #region Variables
        ListOfValuesDAO _listofValuesDAO;
        #endregion

        #region Constructor
        public ListOfValuesBO()
        {
            _listofValuesDAO = new ListOfValuesDAO();
        }
        #endregion

        #region Functions
        public DataTable GetSupplier()
        {
            return _listofValuesDAO.GetSupplier();
        }


        public DataTable GetStore(string Usrid)
        {
            return _listofValuesDAO.GetStore(Usrid);
        }


        public DataTable GetSection()
        {
            return _listofValuesDAO.GetSection();
        }

        public DataTable GetWarehouse()
        {
            return _listofValuesDAO.GetWarehouse();
        }

        public DataTable GetOutTypes()
        {
            return _listofValuesDAO.GetOutTypes();
        }

        public DataTable GetReason()
        {
            return _listofValuesDAO.GetReason();
        }

        public DataTable GetDamageTypes()
        {
            return _listofValuesDAO.GetDamageTypes();
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            _listofValuesDAO.Dispose();
            _listofValuesDAO = null;

        }
        #endregion
    }
}
