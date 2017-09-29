using Common.Utilities;
using SND.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SND.Models
{
   
         public class StockAudit4 : BASE
    {

        string AUDNO { get; set; }
        string USRID { get; set; }
        string LOGNO { get; set; }
        string EDATE { get; set; }
        string LINENO { get; set; }
        string BARCODE { get; set; }
        string ITEMS { get; set; }
        string AUD_Q { get; set; }
        string NOTES { get; set; }
        string ITEMS_NAME { get; set; }
        string ACT_BARCD { get; set; }
        string AISLESNO { get; set; }
        string BAYNO { get; set; }




    }
        public class StockAudit2 : BASE
    {

        #region Properties
        public string ParentId { get; set; }
        public override string ToString() { return "StringItem object ValueType=" + this.ValueType; }
        public string Id;
        public string Maxid;
        public string Timestamp;
        public string Logincode;
        public string Mapcode;
        public string Usercode;
        public string Isdeleted;
        public string Audit01id;
        public string Productcode;
        public string Productbarcode;
        public string Warehousebincode;
        public string Lotorbatchno;
        public string Productserialno;
        public string Productbinnoscode;
        public string Pricecode;
        public string Costprice;
        public string Salesprice;
        public string Retailprice;
        public string Systemqty;
        public string Physicalqty;
        public string Shortqty;
        public string Shortvalue;
        public string Excessqty;
        public string Excessvalue;
        public string Physicalqty1;
        public string Physicalqty2;
        public string Physicalqty3;
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
        public string Audit01idname;
        public string Productcodename;
        public string Warehousebincodename;
        public string Productbinnoscodename;
        public string Pricecodename;
        public string Aislecode;
        public string Baycode;
        public string isDeleted { get; set; }
        public string ParentTableName = "STR_STOCKAUDIT01MASTER";

#endregion


        #region Functions
        public StockAudit2()
        {

            NeedFK = true;
        }
        public string GetPK(string OT)
        {



            using (DAL.DataAccess.StockAuditDAO conext = new DAL.DataAccess.StockAuditDAO())
            {
                object Getto = conext.GetAud2(this.Id);
                if (Getto == null)
                {


                    string key = conext.GetMaxAUD2(OT);
                    if (string.IsNullOrEmpty(key)) { return null; } else { return key; }
                }

                else { return Getto.ToString(); }
            }
        }
        public string GetPK04(string OT)
        {



            using (DAL.DataAccess.StockAuditDAO conext = new DAL.DataAccess.StockAuditDAO())
            {
                object Getto = conext.GetAud4(this.Id);
                if (Getto == null)
                {


                    string key = conext.GetMaxAUD4(OT);
                    if (string.IsNullOrEmpty(key)) { return null; } else { return key; }
                }

                else { return Getto.ToString(); }
            }
        }
        public override ReponseFormat Insert(List<ReponseFormat> rp, List<ErrorStack> errStack)
        {
            if (this.Isdeleted != "N")
            {
                bool rowsEffetced = false;
                // return new ReponseFormat() { ID = this.Id, MapCode = pk, TableName = "GOODRECIEVE" };
                try
                {
                    var Master = rp.Where(m => m.TableName == ReponseHandler.GetTableNAME(typeof(StockAudit)) && m.ID == this.ParentId && m.IsError == "N").FirstOrDefault();
                    if (Master != null)
                    {
                        #region Primray Key
                        string pk = GetPK04(Master.MapCode);
                        #endregion


                        using (DAL.DataAccess.ItemDAO conext = new DAL.DataAccess.ItemDAO())
                        {
                            #region Calculations
                            string amont = "";
                            DAL.DataAccess.StockAuditDAO trconext = new DAL.DataAccess.StockAuditDAO();
                            this.Excessvalue = (!string.IsNullOrEmpty(this.Excessvalue)) ? this.Excessvalue : "0";
                            this.Shortvalue = (!string.IsNullOrEmpty(this.Shortvalue)) ? this.Shortvalue : "0";
                            this.Systemqty = (!string.IsNullOrEmpty(this.Systemqty)) ? this.Systemqty : "0";
                            this.Physicalqty = (!string.IsNullOrEmpty(this.Physicalqty)) ? this.Physicalqty : "0";
                            this.Shortqty = (!string.IsNullOrEmpty(this.Shortqty)) ? this.Shortqty : "0";
                            this.Excessqty = (!string.IsNullOrEmpty(this.Excessqty)) ? this.Excessqty : "0";

                            if (this.Excessvalue != "0" || this.Shortvalue != "0")
                            {
                                amont = Convert.ToDouble(this.Excessvalue) > 0 ? this.Excessvalue : this.Shortvalue;
                            }
                            else
                            {
                                amont = (Convert.ToDouble(this.Physicalqty) * Convert.ToDouble(this.Salesprice)).ToString();
                            }
                            this.Aislecode = (!string.IsNullOrEmpty(this.Aislecode)) ? this.Aislecode : "0";
                            this.Baycode = (!string.IsNullOrEmpty(this.Baycode)) ? this.Baycode : "0";
                            this.Productcodename = (!string.IsNullOrEmpty(this.Productcodename)) ? this.Productcodename : "0";
                            #endregion


                            #region Cheack Barcode  & Item 
                            var barcd = conext.GetBarcodeOfItem(this.Productbarcode);
                            var masterbarcd = conext.GetMasterBarcodeOfItem(this.Productbarcode);
                            var items = conext.GetItemOfBarcode(this.Productbarcode);
                            #endregion


                            if (barcd != null && items != null && masterbarcd != null)
                            {
                                var itemNAME = conext.GetDescrOfBarcode(barcd.ToString(), items.ToString());
                                this.Productcodename = (itemNAME != null) ? itemNAME.ToString() : "";
                                if (trconext.InsertAudit04(Master.MapCode, this.Usercode, this.Logincode, pk, barcd.ToString(), items.ToString(),
                                    this.Physicalqty, "Sync From Device", this.Productcodename, (string.IsNullOrEmpty(masterbarcd.ToString()) ? barcd.ToString() : masterbarcd.ToString()), this.Aislecode, this.Baycode, this.Id
                                    ))
                                {
                                    rowsEffetced = true;
                                    Master.childRecords++;
                                    Master.Childerns.Add(new ReponseFormatChildern() { DeleteOrder = 2, ID = this.Id, ERPTableName = "str_audit_04", MapCode = pk, UserAddedby = this.Usercode, isDeleted = (this.Isdeleted != null) ? (this.Isdeleted.ToString() == "Y") ? "Y" : "N" : "N" });

                                }

                                if (rowsEffetced)
                                {
                                    return new ReponseHandler().GenerateResponse(this.Id, pk, this.GetType(), this.Usercode, MessageHandler.GenericSuccessMsg, this.Isdeleted.ToString(), "N");
                                }
                                else
                                {

                                    return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.NoRowsEffecttionMsg, this.Isdeleted.ToString(), "Y");

                                }
                            }
                            else
                            {
                                return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.BarcodeNotFoundMsg, this.Isdeleted.ToString(), "Y");

                            }
                        }



                    }
                    else
                    {
                        return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.NoParentMsg, this.Isdeleted.ToString(), "Y");
                    }
                }
                catch (Exception ex)
                {
                    Logger.CreateLog(ex.Message.ToString());
                    return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.GenericErrorMsg, this.Isdeleted.ToString(), "Y");

                }
            }
            else
            {
                errStack.Add(new ErrorStack() { ID = this.Id, TableName = "STR_AUDIT_04" }); return null;
            }
        }

        public void RefershAuditPart(string docno) {
            try
            {
                if (docno != "NULL")
                {
                    int rows = new DAL.DataAccess.StockAuditDAO().DeleteAuditPorducts(docno);
                    var ClubList = new DAL.ProductInformationQueryDMLs().GetAuditProductsFromNo(docno);
                    foreach (var item in ClubList)
                    {

                        string lineno = GetPK(docno);
                        using (DAL.DataAccess.StockAuditDAO conext = new DAL.DataAccess.StockAuditDAO())
                        {
                            //seting
                            if (item.BARCODE != null && item.ITEMS != null && item.PHT_QTY != null)
                            {
                                this.Productbarcode = item.BARCODE.ToString();
                                this.Productcode = item.ITEMS.ToString();
                                this.Physicalqty = item.PHT_QTY.ToString();

                                DAL.DataAccess.ItemDAO ietmconext = new DAL.DataAccess.ItemDAO();
                                //  GetBarcodeCostPPrice GetBarcodeTradePrice
                                string amont = "";


                                object brnch = ietmconext.GetBranch(docno, "str_audit_01 ", " S.aud_#");
                                object compc = ietmconext.GetCompc(docno, "str_audit_01  ", " S.aud_#");
                                object COSTP = ietmconext.GetBarcodeCostPPrice(this.Productbarcode, this.Productcode, brnch.ToString());
                                object TRADP = ietmconext.GetBarcodeTradePrice(this.Productbarcode, this.Productcode, brnch.ToString());
                                object USERID = ietmconext.GetUserid(docno, "str_audit_01  ", " S.aud_#");
                                this.Usercode = (USERID != null) ? USERID.ToString() : "1";
                                this.Systemqty = new DAL.DataAccess.ItemDAO().GetItemtock(compc.ToString(), brnch.ToString(), brnch.ToString(), this.Productcode, this.Productbarcode);
                                this.Physicalqty = (!string.IsNullOrEmpty(this.Physicalqty)) ? this.Physicalqty : "0";
                                this.Costprice = (COSTP != null) ? COSTP.ToString() : "0";
                                this.Salesprice = (TRADP != null) ? TRADP.ToString() : "0";
                                this.Shortqty = (!string.IsNullOrEmpty(this.Shortqty)) ? this.Shortqty : "0";
                                this.Excessqty = (!string.IsNullOrEmpty(this.Excessqty)) ? this.Excessqty : "0";

                                if (Convert.ToDouble(this.Physicalqty) > Convert.ToDouble(this.Systemqty))
                                {

                                    this.Excessqty = Math.Round((Convert.ToDouble(this.Physicalqty) - Convert.ToDouble(this.Systemqty)), 4).ToString();

                                }
                                if (Convert.ToDouble(this.Physicalqty) < Convert.ToDouble(this.Systemqty))
                                {

                                    this.Shortqty = Math.Round((Convert.ToDouble(this.Systemqty) - Convert.ToDouble(this.Physicalqty)), 4).ToString();

                                }

                                amont = Math.Round((Convert.ToDouble(this.Physicalqty) * Convert.ToDouble(this.Salesprice)), 4).ToString(); ;
                                if (conext.InsertAuditProducts(docno, this.Usercode, "0", lineno, this.Productcode, this.Systemqty, this.Physicalqty, this.Shortqty, this.Excessqty
                                    , this.Costprice, this.Salesprice, amont, "Sync From Device", this.Productbarcode
                                    , this.Productcodename, "", this.Id)
                                    )
                                {



                                }

                            }
                            //conext.InsertAuditProducts()
                        }
                    }
                    //
                }
            }
            catch(Exception EX)
            {
                Logger.CreateLog(MessageHandler.GenericErrorMsg); }
      }
        #endregion
      
    }
}