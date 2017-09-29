using Common.Utilities;
using SND.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SND.Models
{
    public class TransferOut2 : BASE
    {
        #region Properties
        public string Id;
        public string Maxid;
        public string Timestamp;
        public string Logincode;
        public string Mapcode;
        public string Usercode;
        public string Isdeleted;
        public string Stockout01id;
        public string Productcode;
        public string Productbarcode;
        public string Pricecode;
        public string Costprice;
        public string Salesprice;
        public string Retailprice;
        public string Outqty;
        public string Outvalue;
        public string Uomcode;
        public string Lotorbatchno;
        public string Productserialno;
        public string Productbinnoscode;
        public string Outqty1;
        public string Outqty2;
        public string Outqty3;
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
        public string Usercodename;
        public string Stockout01idname;
        public string Productcodename;
        public string Pricecodename;
        public string Uomcodename;
        public string Productbinnoscodename;
        public string Referencetablecodename;
        public string isDeleted { get; set; }
        public string ParentId { get; set; }
        public string ParentTableName = "TransferOut";
        #endregion

        #region Policy
        public bool AllowToPunchItemBlindlyOUT = true;
        public bool CheckOUTQtyExceed = false;
        bool AllowToPunchBarocdeFromDifferentDpt = false;
        #endregion

        #region Functions
        public TransferOut2()
        {

            NeedFK = true;
        }
     
        public string GetPK(string OT)
        {



            using (DAL.DataAccess.TransferOutDAO conext = new DAL.DataAccess.TransferOutDAO())
            {
                object Getto = conext.GetTO2(this.Id);
                if (Getto == null)
                {


                    string key = conext.GetMaxTO2(OT);
                    if (string.IsNullOrEmpty(key)) { return null; } else { return key; }
                }

                else { return Getto.ToString(); }
            }
        }
        public override ReponseFormat Insert(List<ReponseFormat> rp, List<ErrorStack> errStack)
        {
            if (this.Isdeleted != "Y")
            {
                string IsError = "N";
                try
                {

                    // return new ReponseFormat() { ID = this.Id, MapCode = pk, TableName = "GOODRECIEVE" };
                    bool rowsEffeftced = false;
                    var Master = rp.Where(m => m.TableName == ReponseHandler.GetTableNAME(typeof(TransferOut)) && m.IsError == "N" && m.ID == this.ParentId).FirstOrDefault();
                    if (Master != null)
                    {
                        #region Get Pk
                        string pk = GetPK(Master.MapCode);
                        #endregion
                        using (DAL.DataAccess.ItemDAO conext = new DAL.DataAccess.ItemDAO())
                        {
                            #region CHeck Barcode Company & Barnch
                            object brnch = conext.GetBranch(Master.MapCode, "STR_TRF_OUT1  ", " S.OUT_#");
                            object compc = conext.GetCompc(Master.MapCode, "STR_TRF_OUT1  ", " S.OUT_#");
                            var barcd = (AllowToPunchBarocdeFromDifferentDpt) ? conext.GetBarcodeOfItem(this.Productbarcode) : conext.GetBarcodeOfItemWIthDptd(this.Productbarcode, Master.MapCode);
                            var items = conext.GetItemOfBarcode(this.Productbarcode);
                            #endregion

                            #region If Barcode Valid Move
                            if (barcd != null & items != null)
                            {
                                #region Check Line is there 
                                var INDLINE = conext.GetIndLine(Master.MapCode, this.Productcode, barcd.ToString());
                                string line = (INDLINE == null) ? "" : INDLINE.ToString();
                                #endregion

                                #region InScopeVariable
                                this.Productcode = items.ToString();
                                string Isu_Qty = "0";
                                #endregion

                                #region If Line Found DO Calculation
                                if (!string.IsNullOrEmpty(line))
                                {

                                    string TO = conext.GetOutFromLine(line).ToString();
                                    DAL.ProductInformationQueryDMLs obj = new DAL.ProductInformationQueryDMLs();
                                    var BracodeDetails = obj.GetBarcodeDetailsByGRNForWeb(barcd.ToString(), Master.ID, "TOUT", compc, brnch, "", TO);


                                    if (CheckOUTQtyExceed)
                                    {
                                        if (Convert.ToDecimal(this.Outqty) > Convert.ToDecimal(BracodeDetails.POQty))
                                        {

                                            IsError = "Y";
                                            return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, "Barcode " + barcd.ToString() + " " + " Quatntity Exceed", "N", "Y");
                                            // return new ReponseFormat() { DeleteOrder = 2, ID = this.Id, MapCode = pk, TableName = "STR_STOCKOUT02PRODUCTS", isParent = false, ERPTableName = "str_trf_out2", IDColumn = "LINE#", UserAddedby = this.Usercode, isDeleted = "N", IsError = "Y", Message = "Barcode " + barcd.ToString() + " " + " Quatntity Exceed" };
                                        }
                                    }

                                }
                                else
                                {
                                    #region IF Allow Item to Pucnch Blindly
                                    if (AllowToPunchItemBlindlyOUT)

                                    {


                                    }
                                    #endregion
                                    #region OtherWise
                                    else
                                    {
                                        return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.BarcodeNotFoundInRefDocMsg, this.Isdeleted.ToString(), "Y");

                                        //  return new ReponseFormat() { DeleteOrder = 2, ID = this.Id, MapCode = null, TableName = "STR_STOCKOUT02PRODUCTS", isParent = false, ERPTableName = "str_trf_out2", IDColumn = "LINE#", UserAddedby = this.Usercode, isDeleted = "N", IsError = "Y", Message = MessageHandler.BarcodeNotFoundInRefDocMsg };
                                    }
                                    #endregion



                                }
                                #endregion



                                #region Insert & Update


                                DAL.DataAccess.TransferOutDAO trconext = new DAL.DataAccess.TransferOutDAO();


                                this.Productcode = items.ToString();
                                object SalePrice = conext.GetBarcodeTradePrice(barcd.ToString(), this.Productcode, brnch.ToString());
                                object CostPrice = conext.GetBarcodeCostPPrice(barcd.ToString(), this.Productcode, brnch.ToString());
                                if (SalePrice != null && SalePrice != null)
                                {

                                    this.Salesprice = string.IsNullOrEmpty(SalePrice.ToString()) ? "0" : SalePrice.ToString();
                                    this.Costprice = string.IsNullOrEmpty(CostPrice.ToString()) ? "0" : CostPrice.ToString();
                                }
                                if (trconext.InsertTransferOut2(Master.MapCode, this.Usercode, this.Logincode, pk, this.Productcode, this.Lotorbatchno, "Sync From Device", barcd.ToString(), "", line, this.Outqty, "0", this.Costprice, this.Salesprice, (Convert.ToDouble(this.Outqty) * Convert.ToDouble(this.Salesprice)).ToString(), this.Outqty, "0", this.Outqty1, this.Outqty2, this.Id))
                                {
                                    rowsEffeftced = true;
                                    Master.childRecords++;
                                    Master.Childerns.Add(new ReponseFormatChildern() { DeleteOrder = 2, ID = this.Id, ERPTableName = "str_trf_out2", MapCode = pk, UserAddedby = this.Usercode, isDeleted = (this.Isdeleted != null) ? (this.Isdeleted.ToString() == "Y") ? "Y" : "N" : "N" });

                                }



                                #endregion
                            }
                            #endregion
                            #region OtherWise
                            else
                            {
                                
                                return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, this.Productbarcode + " - " + "not found in specified department", this.Isdeleted.ToString(), "Y");
                                // return new ReponseFormat() { DeleteOrder = 2, ID = this.Id, MapCode = null, TableName = "STR_STOCKOUT02PRODUCTS", isParent = false, ERPTableName = "str_trf_out2", IDColumn = "LINE#", UserAddedby = this.Usercode, isDeleted = (this.Isdeleted != null) ? (this.Isdeleted.ToString() == "Y") ? "Y" : "N" : "N", IsError = "Y", Message = MessageHandler.BarcodeNotFoundMsg };
                            }
                            #endregion
                        }

                        if (rowsEffeftced)
                        {
                            return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.GenericSuccessMsg, this.Isdeleted.ToString(), "N");
                            //return new ReponseFormat() { DeleteOrder = 2, ID = this.Id, MapCode = pk, TableName = "STR_STOCKOUT02PRODUCTS", isParent = false, ERPTableName = "str_trf_out2", IDColumn = "LINE#", UserAddedby = this.Usercode, isDeleted = (this.Isdeleted != null) ? (this.Isdeleted.ToString() == "Y") ? "Y" : "N" : "N", IsError = "N", Message = MessageHandler.GenericSuccessMsg };
                        }
                        else
                        {
                            return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.NoRowsEffecttionMsg, this.Isdeleted.ToString(), "Y");
                        }
                    }
                    else
                    {

                        return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.NoParentMsg, this.Isdeleted.ToString(), "Y");
                        //return new ReponseFormat() { DeleteOrder = 2, ID = this.Id, MapCode = null, TableName = "STR_STOCKOUT02PRODUCTS", isParent = false, ERPTableName = "str_trf_out2", IDColumn = "LINE#", UserAddedby = this.Usercode, isDeleted = "N", IsError = "Y", Message = MessageHandler.NoParentMsg };
                    }
                }
                catch (Exception ex)
                {
                    Logger.CreateLog(ex.Message.ToString());
                    return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.GenericErrorMsg, this.Isdeleted.ToString(), "Y");
                    //return new ReponseFormat() { DeleteOrder = 2, ID = this.Id, MapCode = null, TableName = "STR_STOCKOUT02PRODUCTS", isParent = false, ERPTableName = "str_trf_out2", IDColumn = "LINE#", UserAddedby = this.Usercode, isDeleted =  "N", IsError = "Y", Message = ex.Message };
                }
            }
            else { errStack.Add(new ErrorStack() { ID = this.Id, TableName = "str_trf_out2" });  return null; }


        }

       
        #endregion
    }
}