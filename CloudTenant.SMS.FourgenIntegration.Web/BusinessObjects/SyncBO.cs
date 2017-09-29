namespace SND.BusinessObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using System.ComponentModel;
    using Models;
    using DAL;

    public class SyncBO : IDisposable
    {
        #region Variables
        private SyncDMLs _SYNCDMLs;
        #endregion

        #region Constructor
        public SyncBO()
        {
            _SYNCDMLs = new SyncDMLs();
        }
        #endregion

        #region Functions
        public bool CreateWorkItem(DAL.User _User, string receivedDataFromDevice = "Y")
        {
            return _SYNCDMLs.CreateWorkItem(_User, receivedDataFromDevice);
        }
        public bool DeleteNotNeeded(params object[] param)
        {
            return _SYNCDMLs.DeleteRow(param);
        }
        public bool DeleteNotNeededWRTUser(params object[] param)
        {
            return _SYNCDMLs.DeleteNotNeededWRTUser(param);
        }
        public int CreateWorkItem(DAL.User _User)
        {
            return _SYNCDMLs.CreateWorkItem(_User);
        }
        #endregion

        #region Functions

        #endregion

        #region Dispose
        public void Dispose()
        {
            _SYNCDMLs.Dispose();
            _SYNCDMLs = null;
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
