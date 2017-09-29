using SND.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SND.Models
{

    public class StockAudit :BASE
    {

        #region Properties

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
        public string Officecode;
        public string Documentno;
        public string Workdate;
        public string Auditimpacttoreflecton;
        public string Howtoadjustretrundocument;
        public string Howtoadjustimpact;
        public string Audittype;
        public string Warehousecode;
        public string Warehousebincode;
        public string Usercodewarehouseincharge;
        public string Usercodeauditedby;
        public string Starttime;
        public string Endtime;
        public string Stockstatuscode;
        public string Numberofitems;
        public string Shortageexcessvalue;
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
        public string isDeleted { get; set; }
        public String Usercodename;
        public String Companycodename;
        public String Officecodename;
        public String Warehousecodename;
        public String Warehousebincodename;
        public String Usercodewarehouseinchargename;
        public String Usercodeauditedbyname;
        public String Stockstatuscodename;

        #endregion
        #region Functions
        public StockAudit()
        {

            NeedFK = false;
        }

        public string GetPK()
        {


            using (DAL.DataAccess.StockAuditDAO conext = new DAL.DataAccess.StockAuditDAO())
            {
                object GetGrn = conext.GetAud(this.Mapcode);
                if (GetGrn == null)
                {
                    return "N";
                }
                else { return GetGrn.ToString(); }
            }

        }
      
        public override ReponseFormat Insert(List<ErrorStack> errStack)
        {
            #region Get Pk
            string pk = GetPK();
            #endregion

            using (DAL.DataAccess.StockAuditDAO conext = new DAL.DataAccess.StockAuditDAO())
            {
                #region Step 0 Get Doc Approval
                string approv = "P";

                var AppprovalSetup = conext.GetAudApproval(pk);
                if (AppprovalSetup != null)
                { approv = AppprovalSetup.ToString(); }
                else
                {
                    approv = "P";
                }
                #endregion

                #region Grabage
                //var emp = conext.GetEMPLOYECode(this.Usercode);
                //string Employee = (emp == null) ? "" : emp.ToString();

                //DateTime work = DateHandler.ParseDate(this.Workdate);

                //conext.InsertAudit01(this.Companycode, this.Officecode, this.Usercode, this.Logincode, pk, work.ToString("dd-MMM-yyyy"), work.ToString("dd-MMM-yyyy")
                //   , work.ToString("dd-MMM-yyyy"), work.ToString("dd-MMM-yyyy"), "Y", "S", Employee, "", this.Officecode, this.Warehousecode, this.Id
                //   , "Sync From Device", approv, "F", this.Stockstatuscode);
                //conext.InsertGoodRcv1(this.Companycode, this.Officecode, this.Usercode, this.Logincode, pk, work.ToString("dd-MMM-yyyy")
                //    , "Y", "", "", this.Warehousecode, this.Warehousecode, this.Businesspartnercode, "01",
                //    "RS", this.Currencyrate, "0", "0", "0", "0", this.Details, approv, "", DateTime.Now.ToString("dd-MMM-yyyy"), "0", DateTime.Now.ToString("dd-MMM-yyyy"), "0", this.Officecodebillto, "0", "0", "0", "0", "0", this.Id);
                #endregion

                #region Step 1 If Doc is Not Present or Approcved then Through Out
                if (approv == "A")
                {

                    return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.DocAlreadyApprovedMsg, "N", "Y");

                }
                #endregion

                #region  Step 2 Check IF DOc Found OR not
                if (pk == "N")
                {
                     return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.DocNotFoundMsg, "N", "Y");


                }
                else
                {
                    return new ReponseHandler().GenerateResponse(this.Id, pk, this.GetType(), this.Usercode, MessageHandler.GenericSuccessMsg, "N", "N");
                }
                #endregion


            }






        }
        #endregion
    }
}