using Common.Utilities;
using DAL.Provider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataAccess
{
   public class LoginDAO : IDisposable
    {
        #region Constants
        private static readonly string  SELECT_DEVICE = "SELECT APROV, ID FROM SEC_USERLOGEDONDEVICES D WHERE D.NAME = '{0}' AND D.ACTIV = 'Y'";
        private static readonly string SELECT_MAX_DEVICE_ID = "SELECT NVL( MAX(ID),0)+1 FROM   SEC_USERLOGEDONDEVICES";
        private static readonly string SELECT_MAX_DEVICE_ID_USER = "SELECT NVL(MAX(ID),0) + 1 FROM SEC_USERLOGEDONDEVICES WHERE DEVICELINKEDWITHUSERCODE =  {0} ";

        private static readonly string SELECT_DEVICE_INFO = "SELECT ID,MAXID,TIMESTAMP,USRID AS USERCODE,LOGINCODE,ACTIV ACTIVEYN,NAME,BRANDNAME,MODELNAME,OSVERSIONNO,RESOLUTION ,GCMREGISTRATIONID,DEVICELINKEDWITHUSERCODE,MAPCODE,APROV APROVALYN,COMPC COMPANYCODE,USRID  FROM SEC_USERLOGEDONDEVICES  WHERE ID = {0}";
        #endregion
        #region Variables
        private ConnClass _connClass = null;
        DBHelper _dbHelper = null;

        #endregion

        #region Constructor
        public LoginDAO()
        {
            _connClass = new ConnClass();
            _dbHelper = new DBHelper();
        }
        public string GetMaxDeviceID()
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_MAX_DEVICE_ID))).ToString();
        }
        public string GetMaxDeviceIDForUser(string userid)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_MAX_DEVICE_ID_USER), userid)).ToString();
        }
        public DataTable GetDeviceInfo(string deviceid)
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_DEVICE_INFO), deviceid)).Tables[0];
        }
        [Description("Check If Device is Registered")]
        public decimal IsDeviceRegistered(string userId, decimal loginCode, string deviceId, string registrationId,
          string brandName, string modelName, string osVersion, string resolution, string companyId)
        {
            try
            {

                //
                DBHelper _dbHelper = new DBHelper();
                {


                    DataTable tmpDT = _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_DEVICE), deviceId)).Tables[0];
                   
                    bool isPrevouslyApproved = false;
                    //////////////Check if it is approved previously
                    if (tmpDT.Rows.Count > 0)
                    {
                        foreach (DataRow col in tmpDT.Rows)
                        {

                            if ((col[0].ToString() == "A"))
                            {

                                isPrevouslyApproved = true;
                                break;
                            }
                        }
                    }
                    ///Get Approved STATUS
                    string Aprov = (isPrevouslyApproved) ? "A" : "P";
                    //Create a new device ID
                    decimal id = Convert.ToDecimal(GetMaxDeviceID());
                    decimal Mxid = Convert.ToDecimal(GetMaxDeviceIDForUser(userId));
                    string INSERT_DEVICE =  string.Format(Utils.Utilities.GenerateProperTableName(
                     @"INSERT INTO SEC_USERLOGEDONDEVICES (ID, MAXID, TIMESTAMP, USRID, LOGINCODE, 
                            ACTIV, NAME, BRANDNAME, MODELNAME, OSVERSIONNO,
                            RESOLUTION, GCMREGISTRATIONID, DEVICELINKEDWITHUSERCODE, APROV, COMPC,edate,LOGNO) 
                         VALUES ({0}, {1}, SYSDATE, {2}, {3}, 'Y', '{4}','{6}', '{7}', '{8}', '{9}',
                                '{5}', {2}, '{11}', {10},sysdate,'1')"), id, Mxid, userId, loginCode, deviceId, registrationId,
                          brandName, modelName, osVersion, resolution, companyId, Aprov);
                 int rowsEffect = _dbHelper.ExecuteNonQuery(CommandType.Text, INSERT_DEVICE);
                    if (isPrevouslyApproved && rowsEffect >0) return id;
                    else
                    return 0;


                }
            }
            catch (Exception exception)
            {
                Logger.CreateLog(exception.Message);
                Logger.CreateLog(exception.StackTrace);
                throw;
            }
        }

        [Description("Get Device Info By Id")]
        public object GetDeviceInfoById(decimal deviceId)
        {
            try
            {
                DBHelper _dbHelper = new DBHelper();
                {
                    DataTable dt = GetDeviceInfo(deviceId.ToString());
                    List<dynamic> dynamicDt = dt.ToDynamic();
                    return dynamicDt.FirstOrDefault();
                }
            }
            catch (Exception exception)
            {
                Logger.CreateLog(exception.Message);
                Logger.CreateLog(exception.StackTrace);
                throw;
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
    public static class DataTableExtensions
    {
        public static List<dynamic> ToDynamic(this DataTable dt)
        {
            var dynamicDt = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }
            return dynamicDt;
        }
    }
}
