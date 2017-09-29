using Common.Utilities;
using SND.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace SND.Models
{

    public class TransferIn : BASE
    {
        #region
        public string Id;
        public string Maxid;
        public string Timestamp;
        public string Logincode;
        public string Mapcode;
        public string Usercode;
        public string Postedyn;
        public string Approvalyn;
        public string Blockedyn;
        public string Companycode;
        public string Salesorgcode;
        public string Officecode;
        public string Referencetablecode;
        public string Referencedocumentid;
        public string Referencedocumentno;
        public string Documentno;
        public string Workdate;
        public string Transfertype;
        public string Warehousecode;
        public string Usercodereceivedby;
        public string Officecodereceivedfrom;
        public string Stockstatuscode;
        public string Stocktypecode;
        public string Referenceno;
        public string Details;
        public string Extrafields01;
        public string Extrafields02;
        public string Extrafields03;
        public string Extrafields04;
        public string Extrafields05;
        public string Extrafields06;
        public string Extrafields07;
        public string Extrafields08;
        public string Extrafields09;
        public string Extrafields10; 
public string Usercodename;
        public string Companycodename;
        public string Salesorgcodename;
        public string Officecodename;
        public string Referencetablecodename;
        public string Transfertypename;
        public string Warehousecodename;
        public string Usercodereceivedbyname;
        public string Officecodereceivedfromname;
        public string Stockstatuscodename;
        public string Stocktypecodename;

        public string Warehousename;
        public string Stockstatusname;
        public string Stocktypename;
        public string transferType;
        public int Totalquantity = 0;
        public double Totalvalue = 0;
        public string isDeleted { get; set; }
        #endregion

        #region Policy
        public bool AllowToPunchItemBlindly = true;
        bool CheckIFOutRequired = true;
        bool CheckOUTUtilized = false;
        #endregion

        #region Functions

        public string GetPK()
        {


            using (DAL.DataAccess.TransferDAO conext = new DAL.DataAccess.TransferDAO())
            {
                object GetGrn = conext.GetTRFIN(this.Id);
                if (GetGrn == null)
                {
                    string key = conext.GetMaxTRFIN(this.Companycode, this.Officecode);
                    if (string.IsNullOrEmpty(key)) { return null; } else { return key; }
                }
                else { return GetGrn.ToString(); }
            }

        }

        public TransferIn()
        {

            NeedFK = false;
        }
        public override ReponseFormat Insert(List<ErrorStack> errStack)
        {
            bool rowsEffected = false;
            try
            {
                #region Step -1 GetTrfUltilized
                DAL.DataAccess.ItemDAO conextItem = new DAL.DataAccess.ItemDAO();
                if (conextItem.GetStockOutCheck(this.Referencedocumentno) == null) {
                    return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.RefDocAlreadyUtilized, "N", "Y");
                }
                #endregion


                #region Get Primaray Key

                string pk = GetPK();
                #endregion



                #region Step 0 Get Approval
                DAL.DataAccess.TransferDAO conext = new DAL.DataAccess.TransferDAO();

                string approv = "P";
                var AppprovalSetup = conext.GetInApproval(pk);
                if (AppprovalSetup != null)
                { approv = AppprovalSetup.ToString(); }
                else
                {
                        approv =  "P";
                    
                }
                #endregion


                #region Step 1 Check if Doc is Approved
                if (approv == "A")
                {

                    return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.DocAlreadyApprovedMsg, "N", "Y");

                }

                #endregion



                #region Step 2 Check if ref Doc is pending or no present
                var obj = conext.GetRefTRFIN(this.Referencedocumentno);
                if (CheckIFOutRequired) {
                    if (obj == null) {

                        return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.RefDocNotFoundMsg, "N", "Y");
                    }

                }
                #endregion



                #region Insert & Update
                this.Postedyn = (this.Postedyn == null) ? "N" : (string.IsNullOrEmpty(this.Postedyn.ToString())) ? "N" : (this.Postedyn.ToString().ToUpper() == "NULL") ? "N" : this.Postedyn.ToString();

                var emp = conext.GetEMPLOYECode(this.Usercode);
                string Employee = (emp == null) ? "" : emp.ToString();
                if (string.IsNullOrEmpty(Employee)) return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.NoEmployeeMsg, "N", "Y");
                DateTime work = DateHandler.ParseDate(this.Workdate);


               
                var fromLocation = new DAL.DataAccess.ItemDAO().GetStoreOfBranch(this.Officecodereceivedfrom);
                var toLocation = new DAL.DataAccess.ItemDAO().GetStoreOfBranch(this.Officecode);
                if (fromLocation != null && toLocation != null)
                {

                    if (obj == null)
                    {


                        conext.InsertTRFIN01WOR(this.Companycode, this.Officecode, this.Usercode, this.Logincode, work.ToString("dd-MMM-yyyy"), work.ToString("dd-MMM-yyyy"),
                         fromLocation.ToString(), Employee, toLocation.ToString(), this.Companycode, this.Officecode, this.Officecode, "Sync From Device", approv, this.Stockstatuscode, this.Id, pk, this.Postedyn);


                    }
                    else
                    {
                        string sentBy = "";
                        string date = System.DateTime.Now.ToString("dd-MMM-yyyy");
                        var SentBy = new DAL.DataAccess.TransferOutDAO().GetSNT_BY(obj.ToString());
                        var Date = new DAL.DataAccess.TransferOutDAO().GetOutDateTo(obj.ToString());
                        if (SentBy != null)
                            sentBy = SentBy.ToString();
                        if (Date != null)
                            date = Date.ToString();
                        rowsEffected = conext.InsertTRFIN01(this.Companycode, this.Officecode, this.Usercode, this.Logincode, obj.ToString(), work.ToString("dd-MMM-yyyy"), work.ToString("dd-MMM-yyyy"),
                          fromLocation.ToString(), Employee, toLocation.ToString(), this.Companycode, this.Officecode, this.Officecode, "Sync From Device", approv, this.Stockstatuscode, this.Id, pk, sentBy, date, this.Postedyn);

                    }



                }
                else
                {
                    return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.FromStoreNotFoundfDocMsg, "N", "Y");
                    //FromStoreNotFoundfDocMsg
                }
                #endregion


                if (rowsEffected) {
                    return new ReponseHandler().GenerateResponse(this.Id, pk, this.GetType(), this.Usercode, MessageHandler.GenericSuccessMsg, "N", "N");
                }
                else {
                    return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.NoRowsEffecttionMsg, "N", "Y");

                }
                

            }
            catch(Exception ex) {
                Logger.CreateLog(ex.Message.ToString());
                return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.GenericErrorMsg, "N", "Y");

            }

        }
        #endregion
    }
}