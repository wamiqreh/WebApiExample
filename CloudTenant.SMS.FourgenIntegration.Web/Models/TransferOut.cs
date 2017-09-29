using Common.Utilities;
using SND.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SND.Models
{
    public class TransferOut :BASE
    {
        #region Properties
        public string Id;
        public string Maxid;
        public string Timestamp;
        public string Logincode;
        public string Mapcode;
        public string Usercode;
        public string Postedyn;
        public string Approvalyn;//there
        public string Blockedyn;
        public string Companycode;//there
        public string Salesorgcode;
        public string Officecode;//brnch
        public string Referencetablecode;
        public string Referencedocumentid;
        public string Referencedocumentno;
        public string Documentno;
        public string Workdate;//there wdate
        public string Transfertype;
        public string Warehousecode;//there
        public string Usercodesentby;//snt_b
        public string Officecodesentto;//thre snd_b
        public string Stockstatuscode;
        public string Stockouttypecode;
        public string Referenceno;
        public string Details;//notes
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
        public string Usercodesentbyname;
        public string Officecodesenttoname;
        public string Stockstatuscodename;
        public string Stockouttypecodename;
        public string Departmentcode; 
        public string isDeleted { get; set; }
        public string ParentId { get; set; }
        public string ParentTableName = "";
        #endregion

        #region  Policy

        bool CheckIFIndRequired = false;

        #endregion

        #region Functions
        public override string ToString() { return "StringItem object ValueType=" + this.ValueType; }
        public string GetPK()
        {


            using (DAL.DataAccess.TransferOutDAO conext = new DAL.DataAccess.TransferOutDAO())
            {
                object GetGrn = conext.GetTO(this.Id);
                if (GetGrn == null)
                {
                    string key = conext.GetMaxTO(this.Companycode, this.Officecode);
                    if (string.IsNullOrEmpty(key)) { return null; } else { return key; }
                }
                else { return GetGrn.ToString(); }
            }

        }

        public override ReponseFormat Insert(List<ErrorStack> errStack)
        {
            bool rowsEffected = false; ;
            try
            {

                #region GetPk
                string pk = GetPK();
                #endregion
                using (DAL.DataAccess.TransferOutDAO conext = new DAL.DataAccess.TransferOutDAO())
                {

                   

                    #region Step 0 GetApproval
                    string approv = "P";
                    var AppprovalSetup = conext.GetOutApproval(pk);
                    if (AppprovalSetup != null)
                    { approv = AppprovalSetup.ToString(); }
                    else
                    {

                        

                            approv =  "P";
                        
                    }
                    #endregion

                    #region Step 1 If Aprroved Throud Out
                    if (approv == "A")
                    {


                        return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.DocAlreadyApprovedMsg, "N", "Y");
                        //  return new ReponseFormat() { DeleteOrder = 1, ID = this.Id, MapCode = pk, TableName = "STR_STOCKOUT01MASTER", isParent = true, ERPTableName = "str_trf_out1", IDColumn = "OUT_#", UserAddedby = this.Usercode, isDeleted = "N", IsError = "Y", Message = MessageHandler.DocAlreadyApprovedMsg };

                    }
                    #endregion


                    #region Step 1.5 Check if ref Doc is pending or no present
                                  
                    var obj = conext.GetRefTRFOUT(this.Referencedocumentno);
                    if (CheckIFIndRequired)
                    {
                        if (obj == null)
                        {
                            return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.RefDocNotFoundMsg, "N", "Y");
                            //  return new ReponseFormat() { DeleteOrder = 1, ID = this.Id, MapCode = null, TableName = "STR_STOCKIN01MASTER", isParent = true, ERPTableName = "str_trf_ins1", IDColumn = "TIN_#", UserAddedby = this.Usercode, isDeleted = "N", IsError = "Y", Message = MessageHandler.RefDocNotFoundMsg };
                        }

                    }
                    #endregion

                    #region Step 2 Insert and Update
                    this.Postedyn = (this.Postedyn == null) ? "N" : (string.IsNullOrEmpty(this.Postedyn.ToString())) ? "N" : (this.Postedyn.ToString().ToUpper() == "NULL") ? "N" : this.Postedyn.ToString();
                    var fromLocation = new DAL.DataAccess.ItemDAO().GetStoreOfBranch(this.Officecode);
                    if (fromLocation != null)
                    {

                        var emp = conext.GetEMPLOYECode(this.Usercode);
                        string Employee = (emp == null) ? "" : emp.ToString();
                        if (string.IsNullOrEmpty(Employee)) return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.NoEmployeeMsg, "N", "Y");
                        DateTime work = DateHandler.ParseDate(this.Workdate);
                        rowsEffected = conext.InsertTransferOut1(this.Companycode, this.Officecode , this.Usercode, this.Logincode, pk, work.ToString("dd-MMM-yyyy"), work.ToString("dd-MMM-yyyy"),
                                fromLocation.ToString(), Employee,this.Warehousecode, this.Companycode, this.Officecode, this.Officecode, "Sync From Device", approv, this.Stockstatuscode, this.Id, this.Postedyn,this.Departmentcode);

                    }
                    else {
                        return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.FromStoreNotFoundfDocMsg, "N", "Y");
                        // return new ReponseFormat() { DeleteOrder = 1, ID = this.Id, MapCode = null, TableName = "STR_STOCKOUT01MASTER", isParent = true, ERPTableName = "str_trf_out1", IDColumn = "OUT_#", UserAddedby = this.Usercode, isDeleted = "N", IsError = "Y", Message = MessageHandler.FromStoreNotFoundfDocMsg };


                    }
                    #endregion
                }

                if (rowsEffected) {
                    return new ReponseHandler().GenerateResponse(this.Id, pk, this.GetType(), this.Usercode, MessageHandler.GenericSuccessMsg, "N", "N");
                }
                else {
                    return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.NoRowsEffecttionMsg, "N", "Y");
                    //  return new ReponseFormat() { DeleteOrder = 1, ID = this.Id, MapCode = null, TableName = "STR_STOCKOUT01MASTER", isParent = true, ERPTableName = "str_trf_out1", IDColumn = "OUT_#", UserAddedby = this.Usercode, isDeleted = "N", IsError = "Y", Message = MessageHandler.NoRowsEffecttionMsg};
                }
                

            }
            catch (Exception ex) {
                Logger.CreateLog(ex.Message.ToString());
                return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.GenericErrorMsg, "N", "Y");
                //return new ReponseFormat() { DeleteOrder = 1, ID = this.Id, MapCode = null, TableName = "STR_STOCKOUT01MASTER", isParent = true, ERPTableName = "str_trf_out1", IDColumn = "OUT_#", UserAddedby = this.Usercode, isDeleted = "N", IsError = "Y", Message = MessageHandler.GenericErrorMsg };
            }

        }
        public TransferOut()
        {

            NeedFK = false;
        }
        #endregion


    }
}
