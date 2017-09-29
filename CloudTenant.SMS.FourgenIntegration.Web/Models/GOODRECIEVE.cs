using Common.Utilities;
using SND.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SND.Models
{
    public class GOODRECIEVE : BASE
    {

        #region Constructors
        public GOODRECIEVE()
        {

            NeedFK = false;
        }
        #endregion

        #region Propertires

        public string ParentId { get; set; }
        public override string ToString() { return "StringItem object ValueType=" + this.ValueType; }
      
        
        public string ParentTableName = "";
        public string Id { get; set; }
        public string Maxid { get; set; }
        public string Timestamp { get; set; }
        public string Logincode { get; set; }
        public string Mapcode { get; set; }
        public string Usercode { get; set; }//Mapcode required
        public string Postedyn { get; set; }
        public string Approvalyn { get; set; }
        public string Blockedyn { get; set; }
        public string Companycode { get; set; }//Mapcode required
        public string Officecode { get; set; }//Mapcode required
        public string Referencetablecode { get; set; }
        public string Referencedocumentid { get; set; }
        public string Referencedocumentno { get; set; }
        public string Officecodebillto { get; set; }//Mapcode required
        public string Documentno { get; set; }
        public string Workdate { get; set; }
        public string Usercodereceivedby { get; set; }//Mapcode required
        public string Businesspartnercode { get; set; }//Mapcode required
        public string Businesspartneruserdefinecode { get; set; }
        public string Warehousecode { get; set; }//Mapcode required
        public string Curencycode { get; set; }//Mapcode required
        public string Currencyrate { get; set; }
        public string Islocalorimported { get; set; }
        public string Countrycode { get; set; }
        public string Stockreceivedbasedoncode { get; set; }
        public string Paymenttermcode { get; set; }//Mapcode required
        public string Numberofitems { get; set; }
        public string Billvalue { get; set; }
        public string Billdiscountpercentage { get; set; }
        public string Billdiscountamount { get; set; }
        public string Billroundamount { get; set; }
        public string Billnetvalue { get; set; }
        public string Referenceno { get; set; }
        public string Supplierdcno { get; set; }
        public string Supplierinvoiceno { get; set; }
        public string Biltyfreightno { get; set; }
        public string Vehicleno { get; set; }
        public string Details { get; set; }
        public string Datetimeofarival { get; set; }
        public string Datetimeofunloadingstarted { get; set; }
        public string Datetimeunloadingfinished { get; set; }
        public string Extrafields01 { get; set; }
        public string Extrafields02 { get; set; }
        public string Extrafields03 { get; set; }
        public string Extrafields04 { get; set; }
        public string Extrafields05 { get; set; }
        public string Extrafields06 { get; set; }
        public string Extrafields07 { get; set; }
        public string Extrafields08 { get; set; }
        public string Extrafields09 { get; set; }
        public string Extrafields10 { get; set; }
        public string Type { get; set; }
        public string Isdeleted { get; set; }

        #endregion

        #region Policy
        bool CheckIFPORequired = true;
        #endregion

        #region Functions
        public string GetPK()
        {


            using (DAL.DataAccess.ItemDAO conext = new DAL.DataAccess.ItemDAO())
            {
                object GetGrn = conext.GetGrn(this.Id);
                if (GetGrn == null)
                {
                    string key = conext.GetMaxGrn1(this.Companycode, this.Officecode);
                    if (string.IsNullOrEmpty(key)) { return null; } else { return key; }
                }
                else { return GetGrn.ToString(); }
            }

        }
        public override ReponseFormat Insert(List<ErrorStack> errStack)
        {
            bool rowseffected = false;
            try
            {

                #region Primary Key 
                string pk = GetPK();
                #endregion


                using (DAL.DataAccess.ItemDAO conext = new DAL.DataAccess.ItemDAO())
                {
                    #region Step  0 Get Document Approval
                    string approv = "P";
                    var AppprovalSetup = conext.GetGrnApproval(pk);
                    if (AppprovalSetup != null)
                    { approv = AppprovalSetup.ToString(); }
                    else
                    {       approv =  "P";
                   }
                    #endregion

                    #region Step 1 Check if Doc is Approved
                    if (approv == "A")
                    {

                        return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.DocAlreadyApprovedMsg, "N", "Y");

                    }
                    #endregion

                    #region  Step 2 PO is Approved Or found
                    if (CheckIFPORequired)
                    {
                        var GetPOApproval = new DAL.DataAccess.ItemDAO().GETGRNAPPROVED(this.Referencedocumentno, "GRN");
                        if (GetPOApproval == null)
                        {
                            return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.RefDocNotFoundMsg, "N", "Y");

                        }
                    }
                    #endregion

                    #region Step 3 Insert & Update(Merge)
                    this.Postedyn = (this.Postedyn == null) ? "N" : (string.IsNullOrEmpty(this.Postedyn.ToString())) ? "N" : (this.Postedyn.ToString().ToUpper() == "NULL") ? "N" : this.Postedyn.ToString();
                    if (string.IsNullOrEmpty(this.Currencyrate))
                    {
                        this.Currencyrate = "0";

                    }

                    DAL.DataAccess.StockAuditDAO conextEX = new DAL.DataAccess.StockAuditDAO();


                    var emp = conextEX.GetEMPLOYECode(this.Usercode);
                    string Employee = (emp == null) ? "" : emp.ToString();
                   if(string.IsNullOrEmpty(Employee)) return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.NoEmployeeMsg, "N", "Y");
                    var LocationFromStore = new DAL.DataAccess.ItemDAO().GetBarnchOfStore(this.Warehousecode);
                    if (LocationFromStore != null)
                    {
                        DateTime work = DateHandler.ParseDate(this.Workdate);
                        rowseffected = conext.InsertGoodRcv1(this.Companycode, this.Officecode, this.Usercode, this.Logincode, pk, work.ToString("dd-MMM-yyyy")
                        , "Y", "S", Employee, this.Officecode, LocationFromStore.ToString(), this.Businesspartnercode, "01",
                        "RS", this.Currencyrate, "", this.Supplierinvoiceno, "", "", "Sync From Device", approv, "Y", DateTime.Now.ToString("dd-MMM-yyyy"), "0", DateTime.Now.ToString("dd-MMM-yyyy"), "0", this.Officecodebillto, "Y", "VXX-11.13.14", "0001.GT", "", "0", this.Id, this.Referencedocumentno, this.Postedyn);

                    }
                    else
                    {

                        return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.FromStoreNotFoundfDocMsg, "N", "Y");
                    }
                }
                if (rowseffected) {
                    return new ReponseHandler().GenerateResponse(this.Id, pk, this.GetType(), this.Usercode, MessageHandler.GenericSuccessMsg, "N", "N");

                }
                else
                {


                    return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.NoRowsEffecttionMsg, "N", "Y");
                }
                #endregion

            }
            catch (Exception ex)
            {

                Logger.CreateLog(ex.Message.ToString());
                return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.GenericErrorMsg, "N", "Y");
            }
        }

        #endregion
    }
}