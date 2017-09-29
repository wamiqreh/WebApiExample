namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    

    using System.Data;
   

    public class SyncDMLs : IDisposable
    {
        #region Functions
        public bool CreateWorkItem(User _User, string receivedDataFromDevice = "Y")
        {
            try
            {
               
                return true;
            }
            catch (Exception exception)
            {
                //Logger.PrintError(exception);
                return false;
            }
        }
        
            
        public bool DeleteRow(params object[] param)
        {
            try
            {
                using (DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO())
                {

                    return itemContext.DeleteNotNeeded(param[0].ToString(), param[1].ToString());

                }

            }
            catch (Exception exception)
            {
                //  Logger.PrintError(exception);

                throw exception;
            }
        }
        public bool DeleteNotNeeded(params object[] param)
        {
            try
            {
                using (DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO()) {

                    return itemContext.DeleteNotNeeded(param[0].ToString(), param[1].ToString(), param[2].ToString(), param[3].ToString());

                }
                
            }
            catch (Exception exception)
            {
                //  Logger.PrintError(exception);
                
                throw exception;
            }
        }
        public bool DeleteNotNeededWRTUser(params object[] param)
        {
            try
            {
                using (DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO())
                {

                    return itemContext.DeleteNotNeededForAudit(param[0].ToString(), param[1].ToString(), param[2].ToString(), param[3].ToString(), param[4].ToString());

                }

            }
            catch (Exception exception)
            {
                //  Logger.PrintError(exception);

                throw exception;
            }
        }
        public int CreateWorkItem(User _User)
        {
            try
            {
                //using (ADOContext DB = new ADOContext())
                //{
                //    decimal id = new CommonDMLs().GetNextId(SequenceStatement.SynSequence, DB);
                //    IDbCommand _Command = DB.CreateCommand( string.Format(@"SELECT ID FROM SEC_USERLOGEDONDEVICES WHERE NAME = '{0}'", _User.device_id));
                //    decimal deviceId = Convert.ToDecimal(DB.ExecuteScalar(_Command));

                //    _Command = DB.CreateCommand( string.Format(@"INSERT INTO SYN_DATASYNC01MASTER(ID, TIMESTAMP, USERCODE, LOGINCODE, ACTIVEYN, COMPANYCODE, OFFICECODE, DEVICECODE, LASTSYNCTIME,
                //                            RECEIVEDDATAFROMDEVICE, DATALOADEDSUCCESSFULLYATCLOUD, DATASENTTODEVICE, DATALOADEDSUCCESSFULLTATDEVICE, STATUS, PRIORITYCODE)
                //                VALUES({0}, SYSDATE, {1}, {2}, 'Y', {3}, {4}, '{5}', '{6}','{7}','{8}','{9}', '{10}', '{11}', {12})",
                //                 id, _User.user_id, _User.login_code, _User.company_code, _User.office_code, deviceId, _User.last_sync ?? "",
                //                 "Y", "N", "N", "N", "Q", _User.priority_code));
                //    DB.ExecuteNonQuery(_Command);

                //    _Command = DB.CreateCommand( string.Format(@"UPDATE SEC_USERLOGEDONDEVICES SET GCMREGISTRATIONID = '{0}' WHERE ID = {1}", _User.reg_id, deviceId));
                //    DB.ExecuteNonQuery(_Command);
                //    return Convert.ToInt32(id);
                //}
                return 1;

            }
            catch (Exception exception)
            {
               // Logger.PrintError(exception);
                return 0;
            }
        }
        #endregion

        #region Dispose all the Variables and other unused objects to free memory for application optimization & performance
        public void Dispose()
        {
        }
        #endregion
    }
}
