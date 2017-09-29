using Common.Utilities;
using DAL;
using SND.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SND.Models
{

    public class TransferIn2 : BASE
    {


        #region Properties
        public string ParentTableName = "STR_STOCKIN01MASTER";
        public string ParentId { get; set; }
        public override string ToString() { return "StringItem object ValueType=" + this.ValueType; }
        public string Id;
        public string Maxid;
        public string Timestamp;
        public string Logincode;
        public string Usercode;
        public string Isdeleted;
        public string Stockin01id;
        public string Productcode;
        public string Productbarcode;
        public string Pricecode;
        public string Costprice;
        public string Salesprice;
        public string Retailprice;
        public string Inqty;
        public string Invalue;
        public string Uomcode;
        public string Uomname;

        public string Productbinnoscode;
        public string Lotorbatchno;
        public string Productserialno;
        public string Batchexpirydate;
        public string Inqty1;
        public string Inqty2;
        public string Inqty3;
        public string Referencedocumentmenuid;
        public string Referencetablecode;
        public string Referencedocumentid;
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
        public string Productname;
        public int TotalStock = 0;
        public double TotalStockValue = 0;
        int childCount = 0;
        public string Usercodename;
        public string Stockin01idname;
        public string Productcodename;
        public string Pricecodename;
        public string Uomcodename;
        public string Referencetablecodename;
        public string Productbinnoscodename;
        public string isDeleted { get; set; }

        #endregion


        #region Policy
        public bool AllowToPunchItemBlindlyIN = true;
        public bool CheckINQtyExceed = false;
        public bool AllowToPunchBarocdeFromDifferentDpt = true;
        #endregion

        #region Functions
        public string GetPK(string IN)
        {



            using (DAL.DataAccess.TransferDAO conext = new DAL.DataAccess.TransferDAO())
            {
                object Getto = conext.GetTRFIN2(this.Id);
                if (Getto == null)
                {


                    string key = conext.GetMaxTRFIN2(IN);
                    if (string.IsNullOrEmpty(key)) { return null; } else { return key; }
                }

                else { return Getto.ToString(); }
            }
        }
        public override ReponseFormat Insert(List<ReponseFormat> rp, List<ErrorStack> errStack)
        {
            if (this.Isdeleted != "Y")
            {
                bool rowseffected = false;
                try
                {
                    // return new ReponseFormat() { ID = this.Id, MapCode = pk, TableName = "GOODRECIEVE" };
                    string IsError = "N";
                    var Master = rp.Where(m => m.TableName == ReponseHandler.GetTableNAME(typeof(TransferIn)) && m.ID == this.ParentId && m.IsError == "N").FirstOrDefault();
                    if (Master != null)
                    {

                        #region GetPk
                        string pk = GetPK(Master.MapCode);
                        #endregion

                        DateTime exp = DateHandler.ParseDate(this.Batchexpirydate);
                        using (DAL.DataAccess.ItemDAO conext = new DAL.DataAccess.ItemDAO())
                        {
                            //  ,rec_q,bon_q,costp,tradp

                            DAL.DataAccess.TransferDAO trconext = new DAL.DataAccess.TransferDAO();

                            #region CHeck Barcode Company & Barnch
                            var barcd = conext.GetBarcodeOfItem(this.Productbarcode);
                            
                            var items = conext.GetItemOfBarcode(this.Productbarcode);
                            object brnch = conext.GetBranch(Master.MapCode, "STR_TRF_INS1  ", " S.TIN_#");
                            object compc = conext.GetCompc(Master.MapCode, "STR_TRF_INS1  ", " S.TIN_#");
                            #endregion

                            #region If Barcode Valid Move
                            if (barcd != null & items != null)
                            {


                                #region InScopeVariable
                                this.Productcode = items.ToString();
                                string Out_Qty = "0";
                                #endregion

                                #region Check Line is there 
                                var TOLINE = conext.GetOutLine(Master.MapCode, this.Productcode, barcd.ToString());
                                string line = (TOLINE == null) ? "" : TOLINE.ToString();
                                #endregion


                                #region If Line Found DO Calculation
                                if (!string.IsNullOrEmpty(line))
                                {
                                    ///////
                                  



                                    string TO = conext.GetOutFromLine(line).ToString();
                                    if (!AllowToPunchBarocdeFromDifferentDpt)
                                    {
                                        var itemPresentInBarode = conext.GetBarcodeOfItemWIthDptd(barcd.ToString(), TO);
                                        if (barcd == null)
                                        {
                                            return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, this.Productbarcode + " - " + "not found in specified department", this.Isdeleted.ToString(), "Y");
                                        }
                                    }
                                    DAL.ProductInformationQueryDMLs obj = new DAL.ProductInformationQueryDMLs();
                                    var BracodeDetails = obj.GetBarcodeDetailsByGRNForWeb(barcd.ToString(), Master.ID, "TIN", compc, brnch, "", TO);


                                    if (CheckINQtyExceed)
                                    {
                                        if (Convert.ToDecimal(this.Inqty) > Convert.ToDecimal(BracodeDetails.POQty))
                                        {

                                            IsError = "Y";
                                            return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, "Barcode " + barcd.ToString() + " " + " Quatntity Exceed", "N", "Y");
                                        }
                                    }




                                    this.Salesprice = "0";
                                    this.Costprice = "0";
                                    var ToDetail = new DAL.ProductInformationQueryDMLs().GetTODWithAlFeilds(line);
                                    if (ToDetail != null)
                                    {
                                        
                                        Out_Qty = ToDetail.OUT_Q.ToString();

                                    }
                                    object SalePrice = ToDetail.TRADP;
                                    object CostPrice = ToDetail.COSTP;
                                    if (SalePrice != null && SalePrice != null)
                                    {

                                        this.Salesprice = string.IsNullOrEmpty(SalePrice.ToString()) ? "0" : SalePrice.ToString();
                                        this.Costprice = string.IsNullOrEmpty(CostPrice.ToString()) ? "0" : CostPrice.ToString();
                                    }

                                }
                                #endregion
                                else
                                {
                                    #region IF Allow Item to Pucnch Blindly
                                    if (AllowToPunchItemBlindlyIN)

                                    {
                                        object SalePrice = conext.GetBarcodeTradePrice(barcd.ToString(), this.Productcode, brnch.ToString());
                                        object CostPrice = conext.GetBarcodeCostPPrice(barcd.ToString(), this.Productcode, brnch.ToString());
                                        if (SalePrice != null && SalePrice != null)
                                        {

                                            this.Salesprice = string.IsNullOrEmpty(SalePrice.ToString()) ? "0" : SalePrice.ToString();
                                            this.Costprice = string.IsNullOrEmpty(CostPrice.ToString()) ? "0" : CostPrice.ToString();
                                        }

                                    }
                                    #endregion
                                    #region OtherWise
                                    else
                                    {

                                        return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.BarcodeNotFoundInRefDocMsg, this.Isdeleted.ToString(), "Y");
                                    }
                                    #endregion



                                }
                                #region Insert & Update
                                if (trconext.InsertTRFIN02(Master.MapCode, this.Usercode, this.Logincode, pk, this.Productcode, this.Lotorbatchno, exp.AddMonths(1).ToString("dd-MMM-yyyy")
                                        , this.Inqty, this.Inqty1, this.Costprice, this.Salesprice, (Convert.ToDouble(this.Inqty) * Convert.ToDouble(this.Salesprice)).ToString(), "Sync From Device"
                                        , line, "", barcd.ToString(), Out_Qty, "", this.Id)

                                       )
                                {

                                    rowseffected = true;
                                    Master.childRecords++;
                                    Master.Childerns.Add(new ReponseFormatChildern() { DeleteOrder = 2, ID = this.Id, ERPTableName = "str_trf_ins2", MapCode = pk, UserAddedby = this.Usercode, isDeleted = (this.Isdeleted != null) ? (this.Isdeleted.ToString() == "Y") ? "Y" : "N" : "N" });

                                }
                                #endregion

                            }
                            #endregion

                            #region OtherWise
                            else
                            {
                                return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.BarcodeNotFoundMsg, this.Isdeleted.ToString(), "Y");
                            }
                            #endregion
                        }
                        if (rowseffected)
                        {
                            return new ReponseHandler().GenerateResponse(this.Id, pk, this.GetType(), this.Usercode, MessageHandler.GenericSuccessMsg, this.Isdeleted.ToString(), "N");
                        }
                        else
                        {
                            return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.NoRowsEffecttionMsg, this.Isdeleted.ToString(), "Y");
                        }

                    }
                    else { return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.NoParentMsg, this.Isdeleted.ToString(), "Y"); }
                }
                catch (Exception ex)
                {
                    Logger.CreateLog(ex.Message.ToString());

                    return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.GenericErrorMsg, this.Isdeleted.ToString(), "Y");
                }
            }
            else
            {
                errStack.Add(new ErrorStack() { ID = this.Id, TableName = "str_trf_ins2" });
                return null;
         

            }
            }

        public TransferIn2()
        {

            NeedFK = true;
        }
        #endregion
       
    }
  
}

      
       
 