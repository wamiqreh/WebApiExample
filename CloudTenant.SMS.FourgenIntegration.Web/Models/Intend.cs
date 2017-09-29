using Common.Utilities;
using SND.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SND.Models
{

    public class Indent : BASE
    {
        public Indent()
        {

            NeedFK = false;
        }
        public string Id;
        public string Maxid;
        public string Timestamp;
        public string Logincode;
        public string Usercode;
        public string Postedyn;
        public string Approvalyn;
        public string Blockedyn;
        public string Companycode;
        public string Salesorgcode;
        public string Officecode;
        public string Documentno;
        public string Workdate;
        public string Usercoderequestedby;
        public string Officecoderequiredfrom;
        public string Reasoncode;
        public string Prioritycode;
        public string Needbydate;
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
        public string Usercoderequestedbyname;
        public string Officecoderequiredfromname;
        public string Reasoncodename;
        public string Prioritycodename;
        public string TotalQuantity;
        public string Mapcode;
        public string Isdeleted;
        
        public string GetPK()
        {


            using (DAL.DataAccess.IndentDAO conext = new DAL.DataAccess.IndentDAO())
            {
                object GetIND = conext.GetIND(this.Id);
                if (GetIND == null)
                {
                    string key = conext.GetMaxIND(this.Companycode, this.Officecode);
                    if (string.IsNullOrEmpty(key)) { return null; } else { return key; }
                }
                else { return GetIND.ToString(); }
            }

        }
        //STR_STOCKAUDIT01MASTER//STR_STOCKAUDIT02PRODUCTS
        public override ReponseFormat Insert(List<ErrorStack> errStack)
        {
            bool rowsEffected = false;
            try
            {
                string pk = GetPK();
                using (DAL.DataAccess.IndentDAO conext = new DAL.DataAccess.IndentDAO())
                {
                    string approv = "P";

                    var AppprovalSetup = conext.GetINDApproval(pk);
                    if (AppprovalSetup != null)
                    { approv = AppprovalSetup.ToString(); }
                    else
                    {

                        if (!string.IsNullOrEmpty(this.Approvalyn) && this.Approvalyn != "NULL")
                        {

                            approv = (this.Approvalyn == "Y") ? "A" : "P";
                        }
                    }
                    #region Step 1 Check if Doc is Approved
                    if (approv == "A")
                    {

                        return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.DocAlreadyApprovedMsg, "N", "Y");

                    }
                    #endregion
                    this.Postedyn = (this.Postedyn == null) ? "N" : (string.IsNullOrEmpty(this.Postedyn.ToString())) ? "N" : (this.Postedyn.ToString().ToUpper() == "NULL") ? "N" : this.Postedyn.ToString();
                    var emp = conext.GetEMPLOYECode(this.Usercode);
                    string Employee = (emp == null) ? "" : emp.ToString();
                    if (string.IsNullOrEmpty(Employee)) return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.NoEmployeeMsg, "N", "Y");
                    DateTime work = DateHandler.ParseDate(this.Workdate);

                    var Storeto = new DAL.DataAccess.ItemDAO().GetStoreOfBranch(this.Officecoderequiredfrom);
                    var StoreFROM = new DAL.DataAccess.ItemDAO().GetStoreOfBranch(this.Officecode);

                    if (Storeto != null && StoreFROM != null)
                    {


                        rowsEffected = conext.InsertIndent01(this.Companycode, this.Officecode, this.Usercode, "0", pk, work.ToString("dd-MMM-yyyy"), "Y", approv, Employee

                               , StoreFROM.ToString(), Storeto.ToString(), this.Reasoncode, this.Id, "Sync From Device", this.Postedyn
                               );

                    }

                    else {

                        return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.FromStoreNotFoundfDocMsg, "N", "Y");

                    }


                }

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
            
    }
}