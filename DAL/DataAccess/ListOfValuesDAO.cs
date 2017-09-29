using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using DAL.Provider;
namespace Fourgen.POS.Services.DataAccess
{
    public class ListOfValuesDAO : IDisposable
    {
        #region Constants
        private static readonly string SELECT_SUPPLIER      = "SELECT CUSTM, DESCR FROM SAL_CUS_MAST SCM WHERE SCM.PUR_Y ='Y' ORDER BY DESCR";
        private static readonly string SELECT_STORE = "SELECT STOR#   , DESCR  FROM STR_STORES_M M WHERE ACTIV='Y'  AND EXISTS ( SELECT 1 FROM SEC_USERSTOR S WHERE S.STOR# = M.STOR# AND S.USRID like '{0}') ORDER BY DEFLT DESC ";
        private static readonly string SELECT_SECTION = " SELECT DESCR  ,SCODE  FROM Com_DeptSect WHERE  Deflt = 'Y' ORDER By 1 ";
        private static readonly string SELECT_DAMAGE_TYPES  = "SELECT CODE# AS ID, DESCR AS NAME FROM COM_CODEFILE WHERE TYPE# ='S03'";
        private static readonly string SELECT_REASON_DMD = "SELECT DESCR  ,CODE#  FROM Com_CodeFile Where Type# = '030'";
        private static readonly string SELECT_OUT_TYPES = "SELECT A.DESCR, A.CODE# FROM Com_CodeFile A WHERE  A.Type# = '033'  Order By Descr ";
        private static readonly string SELECT_WAREHOUSE = "SELECT DESCR, GODWN FROM Str_Go_Downs Order By 1 ";
        #endregion

        #region Variables
        private ConnClass _connClass = null;
        DBHelper _dbHelper = null;

        #endregion

        #region Constructor
        public ListOfValuesDAO()
        {
            _connClass = new ConnClass();
            _dbHelper = new DBHelper();
        }
        #endregion

        #region Functions
        public DataTable GetSupplier()
        {
            return _dbHelper.DataAdapter(CommandType.Text, string.Format(DAL.Utils.Utilities.GenerateProperTableName(SELECT_SUPPLIER))).Tables[0];
        }

        public DataTable GetStore(string Usrid)
        {
            return _dbHelper.DataAdapter(CommandType.Text, string.Format(DAL.Utils.Utilities.GenerateProperTableName( SELECT_STORE), Usrid)).Tables[0];
        }

        public DataTable GetSection()
        {
            return _dbHelper.DataAdapter(CommandType.Text, string.Format(DAL.Utils.Utilities.GenerateProperTableName(SELECT_SECTION))).Tables[0];
        }

        public DataTable GetReason()
        {
            return _dbHelper.DataAdapter(CommandType.Text, string.Format(DAL.Utils.Utilities.GenerateProperTableName(SELECT_REASON_DMD))).Tables[0];
        }

        public DataTable GetOutTypes()
        {
            return _dbHelper.DataAdapter(CommandType.Text, string.Format(DAL.Utils.Utilities.GenerateProperTableName(SELECT_OUT_TYPES))).Tables[0];
        }

        public DataTable GetWarehouse()
        {
            return _dbHelper.DataAdapter(CommandType.Text, string.Format(DAL.Utils.Utilities.GenerateProperTableName(SELECT_WAREHOUSE))).Tables[0];
        }


        public DataTable GetDamageTypes()
        {
            return _dbHelper.DataAdapter(CommandType.Text, string.Format(DAL.Utils.Utilities.GenerateProperTableName(SELECT_DAMAGE_TYPES))).Tables[0];
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
