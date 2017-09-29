using Common.Utilities;
using SND.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SND.Models
{

    public class Indent2 : BASE
    {
        public Indent2()
        {

            NeedFK = true;
        }
        public string Id;
        public string Maxid;
        public string Timestamp;
        public string Logincode;
        public string Usercode;
        public string Isdeleted;
        public string Indent02id;
        public string Indent01id;
        public string Productcode;
        public string Productbarcode;
        public string Pricecode;
        public string Costprice;
        public string Salesprice;
        public string Retailprice;
        public string Indentqty;
        public string Indentvalue;
        public int Uomcode;
        public string Needbydate;
        public string Indentqty1;
        public string Indentqty2;
        public string Indentqty3;
        public string Machinecode;
        public string Referencedocumentmenuid;
        public string Referencedocumentid;
        public string Referencetablecode;
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
        public string Indent01idname;
        public string Usercodename;
        public string Indent02idname;
        public string Productcodename;
        public string Uomcodename;
        public string Referencetablecodename;
        public string ParentId; 

        public string isDeleted { get; set; }
        public string GetPK(string ind)
        {



            using (DAL.DataAccess.IndentDAO conext = new DAL.DataAccess.IndentDAO())
            {
                object Getind = conext.GetIND2(this.Id);
                if (Getind == null)
                {


                    string key = conext.GetMaxIND2(ind);
                    if (string.IsNullOrEmpty(key)) { return null; } else { return key; }
                }

                else { return Getind.ToString(); }
            }
        }

        public override ReponseFormat Insert(List<ReponseFormat> rp, List<ErrorStack> errStack)
        {
            bool rowseffected = false;
            // return new ReponseFormat() { ID = this.Id, MapCode = pk, TableName = "GOODRECIEVE" };
            try
            {
                var Master = rp.Where(m => m.TableName == ReponseHandler.GetTableNAME(typeof(Indent)) && m.ID == this.ParentId && m.IsError == "N").FirstOrDefault();
                if (Master != null)
                {
                    string pk = GetPK(Master.MapCode);
                    using (DAL.DataAccess.ItemDAO conext = new DAL.DataAccess.ItemDAO())
                    {
                        string amont = "";
                        DAL.DataAccess.IndentDAO trconext = new DAL.DataAccess.IndentDAO();

                        this.Productcodename = (!string.IsNullOrEmpty(this.Productcodename)) ? this.Productcodename : "0";

                        //Foc_Share_Formula,apply on ll item
                        var barcd = conext.GetBarcodeOfItem(this.Productbarcode);
                        var masterbarcd = conext.GetMasterBarcodeOfItem(this.Productbarcode);
                        var items = conext.GetItemOfBarcode(this.Productbarcode);

                        if (barcd != null && items != null && masterbarcd != null)
                        {//STR_STOCKAUDIT01MASTER//STR_STOCKAUDIT02PRODUCTS
                         //if (trconext.InsertAudit02(Master.MapCode, this.Usercode, this.Logincode, pk, this.Productcode, this.Systemqty, this.Physicalqty, this.Shortqty, this.Excessqty
                         //    , this.Costprice, this.Salesprice, amont, "Sync From Device", barcd.ToString()
                         //    , this.Productcodename, "", this.Id)
                         //    )
                            this.Indentqty = (this.Indentqty != null) ? Indentqty.ToString() : "0";
                            object brnch = conext.GetBranch(Master.MapCode, "STR_INDENT_1  ", " S.IND_#");
                            object compc = conext.GetCompc(Master.MapCode, "STR_INDENT_1  ", " S.IND_#");
                            object COSTP = conext.GetBarcodeCostPPrice(this.Productbarcode, this.Productcode, brnch.ToString());
                            object TRADP = conext.GetBarcodeTradePrice(this.Productbarcode, this.Productcode, brnch.ToString());
                            string ind_val = Math.Round(Convert.ToDouble(this.Indentqty) * Convert.ToDouble(TRADP.ToString())).ToString();
                            string inq = new DAL.DataAccess.ItemDAO().GetItemtock(compc.ToString(), brnch.ToString(), brnch.ToString(), this.Productcode, this.Productbarcode);

                            var itemNAME = conext.GetDescrOfBarcode(barcd.ToString(), items.ToString());
                            this.Productcodename = (itemNAME != null) ? itemNAME.ToString() : "";
                            if (
                                trconext.InsertIndent02(Master.MapCode, this.Usercode, "0", pk, items.ToString(), inq, this.Indentqty, COSTP.ToString(), ind_val, DateTime.Now.ToString("dd-MMM-yyyy")
                                , "Sync From Device", barcd.ToString(), TRADP.ToString(), this.Id
                                ))
                            {
                                rowseffected = true;
                                Master.childRecords++;
                                Master.Childerns.Add(new ReponseFormatChildern() { DeleteOrder = 2, ID = this.Id, ERPTableName = "STR_INDENT_2", MapCode = pk, UserAddedby = this.Usercode, isDeleted = (this.Isdeleted != null) ? (this.Isdeleted.ToString() == "Y") ? "Y" : "N" : "N" });

                            }
                        }
                        else {

                            return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.BarcodeNotFoundMsg, this.Isdeleted.ToString(), "Y");
                        }
                    }
                    if (rowseffected) {

                        return new ReponseHandler().GenerateResponse(this.Id, pk, this.GetType(), this.Usercode, MessageHandler.GenericSuccessMsg, this.Isdeleted.ToString(), "N");

                    } else {
                        return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.NoRowsEffecttionMsg, this.Isdeleted.ToString(), "Y");
                    }

                    
                }
                else {
                    return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.NoParentMsg, this.Isdeleted.ToString(), "Y");
                }
            }
            catch(Exception ex) {

                Logger.CreateLog(ex.Message.ToString());
                return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.GenericErrorMsg, this.Isdeleted.ToString(), "Y");
            }
        }

    }
}