using Common.Utilities;
using SND.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SND.Models
{
    public class GOODRECIEVE2 : BASE
    {


        #region Properties
        public string ParentId { get; set; }
        public string ParentTableName = "GOODRECIEVE";
        public string Id { get; set; }
        public string Maxid { get; set; }
        public string Timestamp { get; set; }
        public string Logincode { get; set; }
        public string Usercode { get; set; }//Map code req
        public string Goodsreceive01id { get; set; }
        public string Isdeleted { get; set; }
        public string Productcode { get; set; }//Map code req
        public string Productbarcode { get; set; }//Map code req from wamiq
        public string Pricecode { get; set; }
        public string Costprice { get; set; }
        public string Salesprice { get; set; }
        public string Retailprice { get; set; }
        public string Receiveqty { get; set; }
        public string Receivevalue { get; set; }
        public string Uomcode { get; set; }
        public string Lotorbatchno { get; set; }
        public string Batchmanufacturedate { get; set; }
        public string Batchexpirydate { get; set; }
        public string Productserialno { get; set; }
        public string Productbinnoscode { get; set; }
        public string Inhandqty { get; set; }
        public string Referencetablecode { get; set; }
        public string Referencedocumentid { get; set; }
        public string Details { get; set; }
        public string Receiveqty1 { get; set; }
        public string Receiveqty2 { get; set; }
        public string Receiveqty3 { get; set; }
        public string Costprice1 { get; set; }
        public string Costprice2 { get; set; }
        public string Costprice3 { get; set; }
        public string Grossvalue { get; set; }
        public string Freeofcostqty { get; set; }
        public string Discountpercentage { get; set; }
        public string Discountamount { get; set; }
        public string Grossvalueafterdiscount { get; set; }
        public string Calcsalestaxonfocqty { get; set; }
        public string Salestaxpercentage { get; set; }
        public string Salestaxamount { get; set; }
        public string Excisedutypercentage { get; set; }
        public string Excisedutyamount { get; set; }
        public string Extradiscountpercentage { get; set; }
        public string Extradiscountamount { get; set; }
        public string Commisionpercentage { get; set; }
        public string Commisionamount { get; set; }
        public string Customdutypercentage { get; set; }
        public string Customdutyamount { get; set; }
        public string Extracolumn01percentage { get; set; }
        public string Extracolumn01amount { get; set; }
        public string Extracolumn02percentage { get; set; }
        public string Extracolumn02amount { get; set; }
        public string Extracolumn03percentage { get; set; }
        public string Extracolumn03amount { get; set; }
        public string Extracolumn04percentage { get; set; }
        public string Extracolumn04amount { get; set; }
        public string Extracolumn05percentage { get; set; }
        public string Extracolumn05amount { get; set; }
        public string Extracolumn06percentage { get; set; }
        public string Extracolumn06amount { get; set; }
        public string Extracolumn07percentage { get; set; }
        public string Extracolumn07amount { get; set; }
        public string Extracolumn08percentage { get; set; }
        public string Extracolumn08amount { get; set; }
        public string Extracolumn09percentage { get; set; }
        public string Extracolumn09amount { get; set; }
        public string Extracolumn10percentage { get; set; }
        public string Extracolumn10amount { get; set; }
        public string Grossmarginpercentage { get; set; }
        public string Grossmarginamount { get; set; }
        public string Grossmarginamountpiece { get; set; }
        public string Lastcostprice { get; set; }
        public string Lastsaleprice { get; set; }
        public string Foc_Cost_Share { get; set; }
        public string Foc_Share_Formula { get; set; }
        public string Foc_Apply_On_All_Items { get; set; }
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
        public string Extrafields11 { get; set; }
        public string Extrafields12 { get; set; }
        public string Extrafields13 { get; set; }
        public string Extrafields14 { get; set; }
        public string Extrafields15 { get; set; }
        public string Extrafields16 { get; set; }
        public string Extrafields17 { get; set; }
        public string Extrafields18 { get; set; }
        public string Extrafields19 { get; set; }
        public string Extrafields20 { get; set; }
        public string Extrafields21 { get; set; }
        public string Extrafields22 { get; set; }
        public string Extrafields23 { get; set; }
        public string Extrafields24 { get; set; }
        public string Extrafields25 { get; set; }
        public string Extrafields26 { get; set; }
        public string Extrafields27 { get; set; }
        public string Extrafields28 { get; set; }
        public string Extrafields29 { get; set; }
        public string Extrafields30 { get; set; }
        public bool IsPOToGenerateUpdate = false;
        #endregion

        #region Policy

        public bool CheckTollarance = false;
        public bool AllowToPunchItemBlindlyGoodsR = false;
        #endregion

        #region Fucntions
        public override ReponseFormat Insert(List<ReponseFormat> rp, List<ErrorStack> errStack)
        {
            bool rowseffecrted = false;
            // return new ReponseFormat() { ID = this.Id, MapCode = pk, TableName = "GOODRECIEVE" };
            if (this.Isdeleted != "Y")
            {
                try
                {
                    var Master = rp.Where(m => m.TableName == ReponseHandler.GetTableNAME(typeof(GOODRECIEVE)) && m.ID == this.ParentId && m.IsError == "N").FirstOrDefault();
                    if (Master != null)

                    {
                        #region Primary Key
                        string pk = GetPK(Master.MapCode);
                        #endregion


                        using (DAL.DataAccess.ItemDAO conext = new DAL.DataAccess.ItemDAO())
                        {
                            //               #region GRN Already Exist
                            //    object GetGrn = conext.GetGrn2(this.Id);
                            //       #endregion

                            #region Check Barcode & Product IS Valid
                            var barcd = conext.GetBarcodeOfItem(this.Productbarcode);
                            var items = conext.GetItemOfBarcode(this.Productbarcode);
                            #endregion


                            if (barcd != null && items != null)
                            {
                                #region Necessary Variables
                                this.Productcode = items.ToString();
                                string GST_B = "N";
                                string GST_W = "N";
                                string MARGIN_A = "0";
                                string MARGIN_P = "0";
                                string MARGIN_T = "0";
                                string AMONT = "0";
                                string BAMONT = "0";
                                string GROSSV = "0";
                                string GST_FOC = "N";
                                string GST_I = "O";
                                string CONSUM = "0";
                                string MRP_G = "0";
                                string RATEno = "";
                                string RBT_P = "0";
                                string BON_Q = "0";
                                string EXT_A = "0";
                                string EXT_P = "0";
                                string CDT_A = "0";
                                string CDT_P = "0";
                                string PACK_QTY = "0";
                                string PACK_RATE= "0";
                                double RATBeforeRebal = 0;
                                this.Extracolumn01percentage = "0";
                                this.Foc_Share_Formula = "P";
                                this.Foc_Apply_On_All_Items = "Y";

                                #endregion



                                #region PO Coy Process Started
                                var POLINE = conext.GetPOLine(Master.MapCode, this.Productcode, barcd.ToString());

                                #endregion

                                //
                                string line = string.Empty;
                                #region POCopy IF Found
                                if (POLINE != null)
                                {


                                    line = POLINE.ToString();
                                    //Get All PO LINE ITEMS FOR GRN Copy Process
                                    var data = new DAL.ProductInformationQueryDMLs().GetPODWithAlFeilds(line);
                                    if (data != null)
                                    {
                                       
                                        if (Convert.ToDouble(this.Receiveqty) == 0)
                                        {
                                        }
                                        double rec = Convert.ToDouble(this.Receiveqty);
                                        double tolleperc = 0;
                                        double remaining = 0;
                                        this.Receiveqty1 = (data.ORD_Q != null) ? ((string.IsNullOrEmpty(data.ORD_Q.ToString())) ? "0" : data.ORD_Q.ToString()) : "0";
                                        #region Check IF tollrance IS ON (If On Then Calcalate Tollarance)

                                        if (CheckTollarance)
                                        {
                                            var remains = new DAL.DataAccess.ItemDAO().GetPOOrdRemaining(data.PO_NO.ToString(), items.ToString(), barcd.ToString());
                                            if (remains != null)
                                            {
                                                remaining = Convert.ToDouble(remains);
                                            }
                                            ////Step 1
                                            var Toll = new DAL.DataAccess.ItemDAO().GetPOTollerance(data.PO_NO.ToString());
                                            if (Toll != null) { tolleperc = Convert.ToDouble(Toll); }
                                            double ord = Convert.ToDouble(this.Receiveqty1);

                                            double minToll = ord - ((ord * tolleperc) / 100);
                                            double maxToll = ord + ((ord * tolleperc) / 100);
                                            double Orec = ord - remaining + rec;
                                            if (Orec < minToll || Orec > maxToll)
                                            {

                                                return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, "Barcode " + barcd.ToString() + " " + " Quatntity Exceed", "N", "Y");
                                            }
                                        }
                                        #endregion

                                        #region Copy ALLL GRN Properties To PO
                                        //Copied Column FROM PO
                                        PACK_QTY = (data.PACK_QTY != null) ? ((string.IsNullOrEmpty(data.PACK_QTY.ToString())) ? "0" : data.PACK_QTY.ToString()) : "0";
                                        PACK_RATE = (data.PACK_RATE != null) ? ((string.IsNullOrEmpty(data.PACK_RATE.ToString())) ? "0" : data.PACK_RATE.ToString()) : "0";
                                        this.Excisedutypercentage = (data.SED_P != null) ? ((string.IsNullOrEmpty(data.SED_P.ToString())) ? "0" : data.SED_P.ToString()) : "0";
                                        this.Discountpercentage = (data.DIS_P != null) ? ((string.IsNullOrEmpty(data.DIS_P.ToString())) ? "0" : data.DIS_P.ToString()) : "0";
                                        this.Commisionpercentage = (data.COM_P != null) ? ((string.IsNullOrEmpty(data.COM_P.ToString())) ? "0" : data.COM_P.ToString()) : "0";
                                        this.Commisionamount = (data.COM_A != null) ? ((string.IsNullOrEmpty(data.COM_A.ToString())) ? "0" : data.COM_A.ToString()) : "0";
                                        this.Salestaxpercentage = (data.GST_P != null) ? ((string.IsNullOrEmpty(data.GST_P.ToString())) ? "0" : data.GST_P.ToString()) : "0";
                                        this.Customdutypercentage = (data.CDT_P != null) ? ((string.IsNullOrEmpty(data.CDT_P.ToString())) ? "0" : data.CDT_P.ToString()) : "0";
                                        this.Salesprice = (data.ITMRT != null) ? ((string.IsNullOrEmpty(data.ITMRT.ToString())) ? "0" : data.ITMRT.ToString()) : "0";
                                        this.Costprice = (data.RAT_N != null) ? ((string.IsNullOrEmpty(data.RAT_N.ToString())) ? "0" : data.RAT_N.ToString()) : "0";
                                        this.Costprice1 = (data.RAT_G != null) ? ((string.IsNullOrEmpty(data.RAT_G.ToString())) ? "0" : data.RAT_G.ToString()) : "0";
                                        
                                        this.Receiveqty2 = (data.BON_Q != null) ? ((string.IsNullOrEmpty(data.BON_Q.ToString())) ? "0" : data.BON_Q.ToString()) : "0";
                                        this.Receiveqty3 = (data.UNIT_QTY != null) ? ((string.IsNullOrEmpty(data.UNIT_QTY.ToString())) ? "0" : data.UNIT_QTY.ToString()) : "0";
                                        GST_B = (data.GST_B != null) ? ((string.IsNullOrEmpty(data.GST_B.ToString())) ? "N" : data.GST_B.ToString()) : "N";
                                        GST_W = (data.GST_W != null) ? ((string.IsNullOrEmpty(data.GST_W.ToString())) ? "N" : data.GST_W.ToString()) : "N";
                                        GST_I = (data.GST_I != null) ? ((string.IsNullOrEmpty(data.GST_I.ToString())) ? "O" : data.GST_I.ToString()) : "O";
                                        BON_Q = (data.BON_Q != null) ? ((string.IsNullOrEmpty(data.BON_Q.ToString())) ? "0" : data.BON_Q.ToString()) : "0";
                                        MARGIN_P = (data.MARGIN_P != null) ? ((string.IsNullOrEmpty(data.MARGIN_P.ToString())) ? "0" : data.MARGIN_P.ToString()) : "0";
                                        this.Extracolumn01percentage = (data.EDAGST_P != null) ? ((string.IsNullOrEmpty(data.EDAGST_P.ToString())) ? "0" : data.EDAGST_P.ToString()) : "0";
                                        GST_FOC = (data.GST_FOC != null) ? ((string.IsNullOrEmpty(data.GST_FOC.ToString())) ? "N" : data.GST_FOC.ToString()) : "N";
                                        this.Foc_Share_Formula = (data.SHARE_FORMULA != null) ? ((string.IsNullOrEmpty(data.SHARE_FORMULA.ToString())) ? "P" : data.SHARE_FORMULA.ToString()) : "P";
                                        this.Foc_Apply_On_All_Items = (data.APPLY_ON_ALL_ITEMS != null) ? ((string.IsNullOrEmpty(data.APPLY_ON_ALL_ITEMS.ToString())) ? "Y" : data.APPLY_ON_ALL_ITEMS.ToString()) : "Y";
                                        this.Lastcostprice = (data.LAST_COST != null) ? ((string.IsNullOrEmpty(data.LAST_COST.ToString())) ? "0" : data.LAST_COST.ToString()) : "0";
                                        CONSUM = (data.CONSM != null) ? ((string.IsNullOrEmpty(data.CONSM.ToString())) ? "0" : data.CONSM.ToString()) : "0";
                                        MRP_G = (data.MRP_G != null) ? ((string.IsNullOrEmpty(data.MRP_G.ToString())) ? "0" : data.MRP_G.ToString()) : "0";
                                        RATEno = (data.RATEno != null) ? ((string.IsNullOrEmpty(data.RATEno.ToString())) ? "" : data.RATEno.ToString()) : "";
                                        RBT_P = (data.RBT_P != null) ? ((string.IsNullOrEmpty(data.RBT_P.ToString())) ? "0" : data.RBT_P.ToString()) : "0";
                                        
                                        //data.COM_A
                                       EXT_A = (data.EXT_A != null) ? ((string.IsNullOrEmpty(data.EXT_A.ToString())) ? "0" : data.EXT_A.ToString()) : "0";
                                        EXT_P = (data.EXT_P != null) ? ((string.IsNullOrEmpty(data.EXT_P.ToString())) ? "0" : data.EXT_P.ToString()) : "0";
                                        CDT_A = (data.CDT_A != null) ? ((string.IsNullOrEmpty(data.CDT_A.ToString())) ? "0" : data.CDT_A.ToString()) : "0";
                                        CDT_P = (data.CDT_P != null) ? ((string.IsNullOrEmpty(data.CDT_P.ToString())) ? "0" : data.CDT_P.ToString()) : "0";

                                        #endregion

                                        #region Computed Columns of GRN wrt Quantitty

                                        GROSSV = Math.Round((Convert.ToDouble(Costprice1) * Convert.ToDouble(this.Receiveqty)), 4).ToString();
                                        this.Discountamount = Math.Round((((Convert.ToDouble(GROSSV)) * Convert.ToDouble(this.Discountpercentage)) / 100), 4).ToString();
                                        double discountedGross = Math.Round(Convert.ToDouble(GROSSV) - Convert.ToDouble(this.Discountamount), 4);

                                        switch (GST_B)
                                        {

                                            case "N"://cost
                                                if (GST_FOC == "Y")
                                                {
                                                    double amont = Math.Round((Convert.ToDouble(this.Costprice1) * (Convert.ToDouble(BON_Q))), 4);
                                                    double texamont = (amont * Convert.ToDouble(this.Salestaxpercentage)) / 100;
                                                    this.Salestaxamount = (Math.Round(((discountedGross * Convert.ToDouble(this.Salestaxpercentage)) / 100), 4) + Math.Round(texamont, 4)).ToString();
                                                }

                                                else
                                                {
                                                    this.Salestaxamount = Math.Round(((discountedGross * Convert.ToDouble(this.Salestaxpercentage)) / 100), 4).ToString();
                                                }
                                                break;

                                            case "G"://MRP
                                                decimal fixedVal = 1.17M;
                                                // (gstAmt * 100) / ((DEFAULT_QTY + foc) * (mrp / fixedVal));
                                                if (GST_FOC == "Y")
                                                {
                                                    double amont = Math.Round(((Convert.ToDouble(MRP_G)/ Convert.ToDouble(fixedVal))  * (Convert.ToDouble(this.Receiveqty) + Convert.ToDouble(BON_Q))), 4);
                                                    //double texamont = (amont * Convert.ToDouble(this.Salestaxpercentage)) / 100;
                                                    this.Salestaxamount = (Math.Round(((amont * Convert.ToDouble(this.Salestaxpercentage)) / 100), 4).ToString());
                                                }

                                                else
                                                {
                                                    double amont = Math.Round(((Convert.ToDouble(MRP_G) / Convert.ToDouble(fixedVal)) * (Convert.ToDouble(this.Receiveqty))), 4);
                                                    //double texamont = (amont * Convert.ToDouble(this.Salestaxpercentage)) / 100;
                                                    this.Salestaxamount = (Math.Round(((amont * Convert.ToDouble(this.Salestaxpercentage)) / 100), 4).ToString());
                                                }
                                                break;
                                            case "O"://NO GST
                                                this.Salestaxpercentage = "0";
                                                this.Salestaxamount = "0";
                                                break;
                                            case "F"://FIX
                                                double totalqty = Math.Round((Convert.ToDouble(this.Receiveqty) + (Convert.ToDouble(BON_Q))), 4);
                                                this.Salestaxamount = Math.Round((totalqty * Convert.ToDouble(this.Salestaxpercentage)), 4).ToString();
                                                break;
                                        }
                                        

                                        this.Excisedutyamount = Math.Round(((discountedGross * Convert.ToDouble(this.Excisedutypercentage)) / 100), 4).ToString();

                                        double discountedGrossAfterGst = 0;
                                        if (GST_W == "Y")
                                        {
                                            //discountedGrossAfterGst = Math.Round(Convert.ToDouble(GROSSV) + Convert.ToDouble(this.Salestaxamount) + Convert.ToDouble(this.Excisedutyamount), 4);
                                            this.Extracolumn01amount = Math.Round((((Convert.ToDouble(GROSSV)) * Convert.ToDouble(this.Extracolumn01percentage)) / 100), 4).ToString();
                                        }
                                        else
                                        {
                                            discountedGrossAfterGst = Math.Round(Convert.ToDouble(discountedGross) + Convert.ToDouble(this.Salestaxamount) + Convert.ToDouble(this.Excisedutyamount), 4);
                                            this.Extracolumn01amount = Math.Round((((Convert.ToDouble(discountedGrossAfterGst)) * Convert.ToDouble(this.Extracolumn01percentage)) / 100), 4).ToString();
                                        }
                                        double BeforeRebal = Convert.ToDouble(GROSSV) - Convert.ToDouble(Discountamount) + Convert.ToDouble(Salestaxamount) + Convert.ToDouble(Excisedutyamount) - Convert.ToDouble(Extracolumn01amount);
                                        AMONT = Math.Round(BeforeRebal, 4).ToString();
                                        RATBeforeRebal = Math.Round(BeforeRebal / (Convert.ToDouble(this.Receiveqty) + Convert.ToDouble(BON_Q)), 4);


                                        this.Extracolumn02amount = Math.Round((((Convert.ToDouble(GROSSV)) * Convert.ToDouble(RBT_P)) / 100), 4).ToString();


                                        double NetAmount = BeforeRebal - Convert.ToDouble(this.Extracolumn02amount);
                                        double NetRATE = Math.Round(NetAmount / (Convert.ToDouble(this.Receiveqty) + Convert.ToDouble(BON_Q)), 4);
                                        double MarginPerPeice = Convert.ToDouble(this.Salesprice) - NetRATE;
                                        if (MARGIN_P != "0")
                                        {
                                            MARGIN_A = MarginPerPeice.ToString();
                                            MARGIN_T = Math.Round(MarginPerPeice * Convert.ToDouble(this.Receiveqty), 4).ToString();
                                        }
                                        BAMONT = NetAmount.ToString();
                                        this.Costprice = NetRATE.ToString();
                                        #endregion
                                        IsPOToGenerateUpdate = true;

                                    }


                                }
                                else
                                {
                                    #region If AllowToPunchItemBlindly
                                    if (AllowToPunchItemBlindlyGoodsR)
                                    {
                                        // }

                                        #region Assign everthing as zero
                                        this.Excisedutypercentage = "0"; this.Discountpercentage = "0"; this.Commisionpercentage = "0"; this.Commisionamount = "0"; this.Salestaxpercentage = "0";
                                        this.Customdutypercentage = "0"; this.Salesprice = "0"; this.Costprice = "0"; this.Costprice1 = "0"; this.Receiveqty2 = "0";
                                        this.Receiveqty3 = "0"; GST_B = "N"; GST_W = "N"; GST_I = "O"; BON_Q = "0"; MARGIN_P = "0"; this.Extracolumn01percentage = "0";
                                        GST_FOC = "N"; this.Foc_Share_Formula = "P"; this.Foc_Apply_On_All_Items = "Y"; this.Lastcostprice = "0";
                                        CONSUM = "0"; MRP_G = "0"; RATEno = ""; RBT_P = "0";
                                        #endregion

                                        #region Computed Column
                                        GROSSV = Math.Round((Convert.ToDouble(Costprice1) * Convert.ToDouble(this.Receiveqty)), 4).ToString();
                                        this.Discountamount = Math.Round((((Convert.ToDouble(GROSSV)) * Convert.ToDouble(this.Discountpercentage)) / 100), 4).ToString();
                                        double discountedGross = Math.Round(Convert.ToDouble(GROSSV) - Convert.ToDouble(this.Discountamount), 4);
                                        if (GST_FOC == "Y")
                                        {
                                            double amont = Math.Round((Convert.ToDouble(this.Costprice1) * (Convert.ToDouble(BON_Q))), 4);
                                            double texamont = (amont * Convert.ToDouble(this.Salestaxpercentage)) / 100;
                                            this.Salestaxamount = (Math.Round(((discountedGross * Convert.ToDouble(this.Salestaxpercentage)) / 100), 4) + Math.Round(texamont, 4)).ToString();
                                        }
                                        else
                                        {
                                            this.Salestaxamount = Math.Round(((discountedGross * Convert.ToDouble(this.Salestaxpercentage)) / 100), 4).ToString();
                                        }
                                        this.Excisedutyamount = Math.Round(((discountedGross * Convert.ToDouble(this.Excisedutypercentage)) / 100), 4).ToString();

                                        double discountedGrossAfterGst = 0;
                                        if (GST_W == "Y")
                                        {
                                            //discountedGrossAfterGst = Math.Round(Convert.ToDouble(GROSSV) + Convert.ToDouble(this.Salestaxamount) + Convert.ToDouble(this.Excisedutyamount), 4);
                                            this.Extracolumn01amount = Math.Round((((Convert.ToDouble(GROSSV)) * Convert.ToDouble(this.Extracolumn01percentage)) / 100), 4).ToString();
                                        }
                                        else
                                        {
                                            discountedGrossAfterGst = Math.Round(Convert.ToDouble(discountedGross) + Convert.ToDouble(this.Salestaxamount) + Convert.ToDouble(this.Excisedutyamount), 4);
                                            this.Extracolumn01amount = Math.Round((((Convert.ToDouble(discountedGrossAfterGst)) * Convert.ToDouble(this.Extracolumn01percentage)) / 100), 4).ToString();
                                        }
                                        double BeforeRebal = Convert.ToDouble(GROSSV) - Convert.ToDouble(Discountamount) + Convert.ToDouble(Salestaxamount) + Convert.ToDouble(Excisedutyamount) - Convert.ToDouble(Extracolumn01amount);
                                        AMONT = Math.Round(BeforeRebal, 4).ToString();
                                        RATBeforeRebal = Math.Round(BeforeRebal / Convert.ToDouble(this.Receiveqty), 4);


                                        this.Extracolumn02amount = this.Extracolumn01amount = Math.Round((((Convert.ToDouble(GROSSV)) * Convert.ToDouble(RBT_P)) / 100), 4).ToString();


                                        double NetAmount = BeforeRebal - Convert.ToDouble(this.Extracolumn02amount);
                                        double NetRATE = Math.Round(BeforeRebal / Convert.ToDouble(this.Receiveqty), 4);
                                        double MarginPerPeice = Convert.ToDouble(this.Salesprice) - NetRATE;
                                        if (MARGIN_P != "0")
                                        {
                                            MARGIN_A = MarginPerPeice.ToString();
                                            MARGIN_T = Math.Round(MarginPerPeice * Convert.ToDouble(this.Receiveqty), 4).ToString();
                                        }
                                        BAMONT = NetAmount.ToString();
                                        this.Costprice = NetRATE.ToString();
                                    }
                                    #endregion

                                    #endregion
                                    #region OtherWise
                                    else
                                    {

                                        return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.BarcodeNotFoundInRefDocMsg, this.Isdeleted.ToString(), "Y");

                                    }
                                    #endregion

                                }
                                #endregion


                                #region Insert & update
                                if (conext.InsertGoodRcv2(Master.MapCode, this.Usercode, this.Logincode, "", line, pk, this.Productcode, "", DateTime.Now.AddMonths(1).ToString("dd-MMM-yyyy"), Costprice1,
                                         this.Discountpercentage, this.Commisionpercentage, this.Salestaxpercentage, this.Excisedutypercentage, CDT_P, EXT_P, this.Discountamount
                                         , this.Commisionamount, this.Salestaxamount, this.Excisedutyamount, CDT_A, EXT_A, this.Costprice, this.Salesprice, this.Receiveqty1, this.Receiveqty,
                                         BON_Q, AMONT, "Sync From Device", GROSSV, GST_B, GST_W, this.Extracolumn01percentage, this.Extracolumn01amount, MARGIN_P, MARGIN_A, GST_FOC, MARGIN_T,
                                        this.Foc_Apply_On_All_Items, this.Foc_Share_Formula, this.Foc_Cost_Share, barcd.ToString(), "0", this.Lastcostprice, "0", this.Receiveqty, "0", GST_I, CONSUM, MRP_G, "0", "0", RBT_P, this.Extracolumn02amount, RATBeforeRebal.ToString(), BAMONT, "0", "VXX-11.13.14", "0", RATEno, this.Id))
                                {
                                  if(IsPOToGenerateUpdate)
                                    conext.InsertGoodRcv2Update(pk);
                                    rowseffecrted = true;
                                    Master.childRecords++;
                                    Master.Childerns.Add(new ReponseFormatChildern() { DeleteOrder = 2, ID = this.Id, ERPTableName = "str_goodsr_2", MapCode = pk, UserAddedby = this.Usercode, isDeleted = (this.Isdeleted != null) ? (this.Isdeleted.ToString() == "Y") ? "Y" : "N" : "N" });

                                }
                                #endregion
                            }
                            else
                            {

                                return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.BarcodeNotFoundMsg, this.Isdeleted.ToString(), "Y");
                            }
                        }

                        if (rowseffecrted)
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
                        return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.NoParentMsg, this.Isdeleted.ToString(), "Y");
                    }
                }
                catch (Exception ex)
                {
                    Logger.CreateLog(ex.Message.ToString());

                    return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.GenericErrorMsg, this.Isdeleted.ToString(), "Y");
                }
            }
            else {
                
                errStack.Add(new ErrorStack() { ID = this.Id, TableName = "str_goodsr_2" });
                return null;
            }
        }
        public GOODRECIEVE2()
        {

            NeedFK = true;
        }

        public string GetPK(string grn)
        {



            using (DAL.DataAccess.ItemDAO conext = new DAL.DataAccess.ItemDAO())
            {
                object GetGrn = conext.GetGrn2(this.Id);
                if (GetGrn == null)
                {


                    string key = conext.GetMaxGrn2(grn);
                    if (string.IsNullOrEmpty(key)) { return null; } else { return key; }
                }

                else { return GetGrn.ToString(); }
            }
        }
        #endregion


    }
}