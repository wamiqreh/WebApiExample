using System;
using System.Collections.Generic;
using System.Linq;


namespace DAL
{
    using Common.Models;
    using Common.Utilities;
    using System;

    using System.Collections.Generic;
    using System.Data;
    using System.Data.OracleClient;
    using System.Linq;




    public class ProductInformationQueryDMLs : IDisposable
    {



        public object GetItemInfoByBarcodeOrArticleNo(params object[] param)
        {
            try
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();


                return (ConvertItemInfoByBarcodeOrArticleNoDataRowToModel(itemContext.GetItemByBarcode(param[1].ToString(), param[2].ToString(), param[0].ToString(), param[4].ToString(), param[3].ToString()).AsEnumerable()));

            }
            catch (Exception exception)
            {
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;
                // Logger.PrintError(exception);
                return null;
            }
        }
        public object GetItemInfoByBarcodeOrArticleNoStock(params object[] param)
        {
            try
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

                
                return (ConvertItemInfoByBarcodeOrArticleNoDataRowToModel(itemContext.GetItemByBarcodeStock(param[1].ToString(), param[2].ToString(), param[0].ToString(), param[4].ToString(), param[3].ToString(), param[5].ToString()).AsEnumerable()));

            }
            catch (Exception exception)
            {
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;
                // Logger.PrintError(exception);
                return null;
            }
        }
        public object ConvertItemInfoByBarcodeOrArticleNoDataRowToModel(EnumerableRowCollection<DataRow> collection)
        {
            try
            {
                return collection.Select(
                            row => new
                            {


                                ProductBarcode = row.Field<object>("BARCODE"),
                                ProductName = row.Field<object>("ITEMNAME"),
                                ProductArticleNo = row.Field<object>("ITEMS"),
                                ClosingQuantity = row.Field<object>("CLOSINGQUANTITY"),
                                UNApprovedQuantity = row.Field<object>("UNAPPROVEDQUANTITY"),
                                OnOrderQuantity = row.Field<object>("ONORDERQUANTITY"),
                                Price = row.Field<object>("RETAILPRICE"),
                                SizeColor = row.Field<object>("COLUR"),
                                MinStock = row.Field<object>("MIN_Q"),
                                MaxStock = row.Field<object>("MAX_Q"),
                                ReOrderStock = row.Field<object>("REO_Q"),
                                Supplier = row.Field<object>("SUPPLIER"),
                                DailyMSale = row.Field<object>("DAILYMSALE"),
                                PId = row.Field<object>("PITEMCODE"),

                                PName = row.Field<object>("PNAME"),
                                PItemCode = row.Field<object>("PITEMCODE"),
                                PProductBarcode = row.Field<object>("PPRODUCTBARCODE"),

                                PIsThisImportedItem = row.Field<object>("PISTHISIMPORTEDITEM"),
                                POpeningDate = row.Field<object>("POPENINGDATE"),
                                PClosingDate = row.Field<object>("PCLOSINGDATE"),
                                PProductClassificationCode = row.Field<object>("PPRODUCTCLASSIFICATIONCODE"),
                                PProductClassificationCodeName = row.Field<object>("PPRODUCTCLASSIFICATIONCODENAME"),
                                PHierarchyCode = row.Field<object>("PPRODUCTHIERARCHYCODE"),
                                PHierarchyCodeName = row.Field<object>("PPRODUCTHIERARCHYCODENAME"),
                                PBrandCode = row.Field<object>("PBRANDCODE"),
                                PBrandCodeName = row.Field<object>("PBRANDCODENAME"),
                                PDepartmentCode = row.Field<object>("PDEPARTMENTCODE"),
                                PDepartmentCodeName = row.Field<object>("PDEPARTMENTCODENAME"),
                                PColorCode = row.Field<object>("PCOLORCODE"),
                                PColorCodeName = row.Field<object>("PCOLORCODENAME"),

                                PSizeCode = row.Field<object>("PSIZECODE"),
                                PSizeCodeName = row.Field<object>("PSIZECODENAME"),
                                PABCIndicationCode = "",
                                PABCIndicationCodeName = "",
                                PGroupCode = row.Field<object>("PGROUPCODE"),
                                PGroupCodeName = row.Field<object>("PGROUPCODENAME"),
                                PPrincipleCode = row.Field<object>("PPRINCIPLECODE"),
                                PPrincipleCodeName = row.Field<object>("PPRINCIPLECODENAME"),

                                PDivisionCode = row.Field<object>("PDIVISIONCODE"),
                                PDivisionCodeName = row.Field<object>("PDIVISIONCODENAME"),
                                PVariantCode = row.Field<object>("PVARIANTCODE"),
                                PVariantCodeName = row.Field<object>("PVARIANTCODENAME"),
                                PMarketingTeamCode = row.Field<object>("PMARKETINGTEAMCODE"),
                                PMarketingTeamCodeName = row.Field<object>("PMARKETINGTEAMCODENAME"),

                                PShelfLifeMax = row.Field<object>("PSHELFLIFEMAX"),
                                PShelfLifeMin = row.Field<object>("PSHELFLIFEMIN"),
                                PShelfLifeRemainingForSale = row.Field<object>("PSHELFLIFEREMAININGFORSALE"),
                                PShelfLifeRemainingForProductIO = "",
                                PSortingOrder = "0",
                                PIsThisServiceItem = row.Field<object>("PISTHISSERVICEITEM"),

                                PIsThisMasterProduct = row.Field<object>("PISTHISMASTERPRODUCT"),
                                PIsThisItemSaleable = row.Field<object>("PISTHISITEMSALEABLE"),
                                PIsThisItemPurchaseable = row.Field<object>("PISTHISITEMPURCHASEABLE"),
                                PIsThisFixedAssetItem = row.Field<object>("PISTHISFIXEDASSETITEM"),
                                PMaintainInventory = row.Field<object>("PMAINTAININVENTORY"),
                                PDoYouMakeThisItem = row.Field<object>("PDOYOUMAKETHISITEM"),
                                PMaintainBatchNos = row.Field<object>("PMAINTAINBATCHNOS"),
                                PMaintainSerialNo = "",
                                PIsPriceAlreadyPrinted = row.Field<object>("PISPRICEALREADYPRINTED"),
                                PSeperatePriceForChildBarcode = row.Field<object>("PSEPERATEPRICEFORCHILDBARCODE"),
                                PIsThisFreeFlowProduct = row.Field<object>("PISTHISFREEFLOWPRODUCT"),

                                PAutoPositionInspection = row.Field<object>("PAUTOPOSITIONINSPECTION"),

                                PPiecesInCarton = row.Field<object>("PPIECESINCARTON"),
                                PPiecesInPack = row.Field<object>("PPIECESINPACK"),
                                PPerPieceGramOrMililitre = row.Field<object>("PPERPIECEGRAMORMILILITRE"),
                                PRegistrationNo = row.Field<object>("PREGISTRATIONNO"),
                                PGrossWeight = row.Field<object>("PGROSSWEIGHT"),
                                PUOMCodeGrossWeight = row.Field<object>("PUOMCODEGROSSWEIGHT"),
                                PNetWeight = row.Field<object>("PNETWEIGHT"),
                                PVolume = row.Field<object>("PVOLUME"),
                                PUOMCodeVolume = row.Field<object>("PUOMCODEVOLUME"),
                                PProductHeight = row.Field<object>("PPRODUCTHEIGHT"),
                                PProductDepth = row.Field<object>("PPRODUCTDEPTH"),
                                PParentKey = row.Field<object>("PPARENTKEY"),
                                PPrintPricedOnBarcode = row.Field<object>("PPRINTPRICEDONBARCODE"),
                                PPrintShortnameOnSlip = row.Field<object>("PPRINTSHORTNAMEONSLIP"),
                                PMaxOrderQty = row.Field<object>("PMAXORDERQTY"),
                                PMinOrderQty = row.Field<object>("PMINORDERQTY"),
                                PReOrderLevelQty = row.Field<object>("PREORDERLEVELQTY"),
                                PMaxQty = row.Field<object>("PMAXQTY"),
                                PMinQty = row.Field<object>("PMINQTY"),
                                PProcurementGroupCode = row.Field<object>("PPROCUREMENTGROUPCODE"),
                                PPOTolerence = row.Field<object>("PPOTOLERENCE"),
                                PLeadTimeTransfer = row.Field<object>("PLEADTIMETRANSFER"),
                                PLeadTimePurchase = row.Field<object>("PLEADTIMEPURCHASE"),
                                PLeadTimeSales = row.Field<object>("PLEADTIMESALES"),
                                PLeadTimeProduction = row.Field<object>("PLEADTIMEPRODUCTION"),
                                PArticleNo = row.Field<object>("PARTICLE_PARTNO"),
                                PCatelogDesignNo = row.Field<object>("PCATELOGDESIGNNO"),
                                PUnitOfMeasureCodeInventory = row.Field<object>("PUNITOFMEASURECODEINVENTORY"),
                                PUnitOfMeasureCodePurchase = row.Field<object>("PUNITOFMEASURECODEPURCHASE"),
                                PUnitOfMeasureCodeSales = row.Field<object>("PUNITOFMEASURECODESALES")



                                ////New Added

                            }).First();
            }
            catch (Exception EX)
            {
                return null;
            }
        }



        public object GetStockInHand(params object[] param)
        {
            try
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

                return (ConvertStockInHandDataRowToModel(itemContext.GetStockInHand(param[1].ToString(), param[2].ToString(), param[0].ToString(), param[4].ToString(), param[3].ToString()).AsEnumerable()));

            }
            catch (Exception exception)
            {

                return null;
            }
        }
        public object ConvertStockInHandDataRowToModel(EnumerableRowCollection<DataRow> collection)
        {
            try
            {
                return collection.Select(
                            row => new
                            {
                                OfficeId = row.Field<object>("OFFICEID"),
                                OfficeName = row.Field<object>("OFFICENAME"),
                                StockType = row.Field<object>("NAME"),
                                StockInHand = row.Field<object>("STOCKINHAND"),
                            }).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }


        public object GetUnapprovedDocument(params object[] param)
        {
            try
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

                return (ConvertUnapprovedDocumentDataRowToModel(itemContext.GetUnApprovedDocuments(param[1].ToString(), param[2].ToString(), param[0].ToString(), param[4].ToString()).AsEnumerable()));

            }
            catch (Exception exception)
            {

                return null;
            }
        }
        public object ConvertUnapprovedDocumentDataRowToModel(EnumerableRowCollection<DataRow> collection)
        {
            try
            {
                return collection.Select(
                            row => new
                            {
                                Documentno = row.Field<object>("DOCNO"),
                                Doctype = row.Field<object>("DOCTYPE"),
                                Workdate = row.Field<object>("WDATE"),
                                Receiveqty = row.Field<object>("RECEIVEQTY"),
                                Details = row.Field<object>("DETAILS"),
                                ProductName = row.Field<object>("PRODUCTNAME"),
                                TotalQty = row.Field<object>("TOTALQTY"),
                                Days = row.Field<object>("DAYS")

                            }).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }



        public object GetStockOnOrder(params object[] param)
        {
            try
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

                return (ConvertGetStockOnOrderDataRowToModel(itemContext.GetOnOrderDocuments(param[1].ToString(), param[2].ToString(), param[0].ToString(), param[4].ToString()).AsEnumerable()));

            }
            catch (Exception exception)
            {

                return null;
            }
        }
        public object ConvertGetStockOnOrderDataRowToModel(EnumerableRowCollection<DataRow> collection)
        {
            try
            {
                return collection.Select(
                            row => new
                            {
                                Documentno = row.Field<object>("DOCUMENTNO"),
                                WorkDate = row.Field<object>("WORKDATE"),
                                Days = row.Field<object>("DAYS"),
                                Supplier = row.Field<object>("SUPPLIER"),
                                TotalQuantity = row.Field<object>("TOTALQTY")

                            }).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }


        public PurchaseOrder GetPODWithAlFeilds(params object[] param)
        {
            try
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

                return (ConvertGetPOWithAllFeildsDataRowToModel(itemContext.GetPurOrdDetail(param[0].ToString()).AsEnumerable()));



            }
            catch (Exception exception)
            {
                //  Logger.PrintError(exception);
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace);
                throw exception;
            }
        }
        public TransferOut GetTODWithAlFeilds(params object[] param)
        {
            try
            {
                if (param[0].ToString() != "")
                {
                    DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

                    return (ConvertGetTOWithAllFeildsDataRowToModel(itemContext.GetTODetail(param[0].ToString()).AsEnumerable()));

                }
                else { return null; }

            }
            catch (Exception exception)
            {
                //  Logger.PrintError(exception);
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace);
                throw exception;
            }
        }
        //line#,out_qty,bon_qty,items,costp,tradp,amont,barcode
        public object GetPODetailsByPOOrSupplierNo(params object[] param)
        {
            try
            {
            
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

                return (ConvertGetPODetailByPOOrArticleNoDataRowToModel(itemContext.GetPOByPOORSupplier(param[2].ToString(), param[3].ToString(), param[0].ToString(), param[1].ToString()).AsEnumerable()));



            }
            catch (Exception exception)
            {
                //  Logger.PrintError(exception);
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace);
                throw exception;
            }
        }
        public object GetIndDetailsByIndNo(params object[] param)
        {
            try
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();
                
                string store = "";
                var data = itemContext.GetStoreOfBranch(param[1].ToString());
                if (data != null) { store = data.ToString(); }
                return (ConvertGetGetIndDetailsByIndNoDataRowToModel(itemContext.GetIndByIndNo(param[2].ToString(), store).AsEnumerable()));

              //  ConvertGetPODetailByPOOrArticleNoDataRowToModel

            }
            catch (Exception exception)
            {
                //  Logger.PrintError(exception);
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace);
                throw exception;
            }
        }
        public object GetBarcodeDetailsByGRN(params object[] param)
        {
            try
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

                return (ConvertGetBarcodeDetailsByGRNDataRowToModel(itemContext.GetBarcodeDetailByBarcodeORGRN(param[0].ToString(),param[1].ToString(), param[2].ToString(), param[6].ToString()).AsEnumerable(), param[1].ToString(), param[2].ToString(), param[3].ToString(), param[4].ToString(), param[6].ToString()));



            }
            catch (Exception exception)
            {
                //  Logger.PrintError(exception);
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace);
                throw exception;
            }
        }
        public GRNProductInfo GetBarcodeDetailsByGRNForWeb(params object[] param)
        {
            try
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

                return (ConvertGetBarcodeDetailsByGRNDataRowToModelForWeb(itemContext.GetBarcodeDetailByBarcodeORGRN(param[0].ToString(), param[1].ToString(), param[2].ToString(), param[6].ToString()).AsEnumerable(), param[1].ToString(), param[2].ToString(), param[3].ToString(), param[4].ToString(), param[6].ToString()));



            }
            catch (Exception exception)
            {
                //  Logger.PrintError(exception);
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace);
                throw exception;
            }
        }
        public object GetAuditDetailsFromNo(string docno,string brnch,string compc)
        {
            try
            {
                DAL.DataAccess.StockAuditDAO itemContext = new DataAccess.StockAuditDAO();

            
                return (ConvertGetAuditDetailsFromNoRowToModel(itemContext.GetAudFromDocNo(compc, brnch,docno).AsEnumerable()));



            }
            catch (Exception exception)
            {
                //  Logger.PrintError(exception);
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace);
                throw exception;
            }
        }

        //ConvertGetAuditProductsFromNoRowToModel
        public List<StockAuditProducts> GetAuditProductsFromNo(string docno)
        {
            try
            {
                DAL.DataAccess.StockAuditDAO itemContext = new DataAccess.StockAuditDAO();


                return (ConvertGetAuditProductsFromNoRowToModel(itemContext.AuditClubProducts( docno).AsEnumerable()));



            }
            catch (Exception exception)
            {
                //  Logger.PrintError(exception);
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace);
                throw exception;
            }
        }
        private object ConvertGetAuditDetailsFromNoRowToModel(EnumerableRowCollection<DataRow> enumerableRowCollection)
        {
            try
            {
                return enumerableRowCollection.Select(
                            row => new
                            {
                                
                                DOCUMENTNO = row.Field<object>("DOCUMENTNO"),
                                OFFICECODE = row.Field<object>("OFFICECODE"),
                                WAREHOUSECODE = row.Field<object>("WAREHOUSECODE")
                                
                            }).FirstOrDefault();
            }
            catch (Exception exception)
            {
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace);
                throw exception;
            }
        }
        private  List< StockAuditProducts> ConvertGetAuditProductsFromNoRowToModel(EnumerableRowCollection<DataRow> enumerableRowCollection)
        {
            try
            {
                return enumerableRowCollection.Select(
                            row => new StockAuditProducts
                            {

                                AUDNO = row.Field<object>("AUD_#"),
                                BARCODE = row.Field<object>("BARCODE"),
                                ITEMS = row.Field<object>("ITEMS"),
                                PHT_QTY = row.Field<object>("PHY_QTY")

                            }).ToList();
            }
            catch (Exception exception)
            {
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace);
                throw exception;
            }
        }
        private string CheckGRNisApproved(string pono, string apprv, string compc, string brnch)
        {
            if (apprv == "Y")
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

                string data = itemContext.GETGRNAPROV(pono, compc, brnch).ToString();
                if (data != null)
                {
                    return "Y;ALLOWED";
                }
                else
                {
                    return "N;PO is not Approved.";
                }


            }
            else { return "N;PO is not Approved."; }

        }
        private string CheckGRNQuantity(string pono, string apprv, string compc, string brnch,string barcode,string items)
        {
            if (apprv == "Y")
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

                var data = itemContext.GETGRNQTY(pono, compc, brnch, barcode, items);
                if (data != null)
                {

                    ////Step 1
                    double tolleperc = 0;
                    var Toll = new DAL.DataAccess.ItemDAO().GetPOTollerance(pono);
                    var OrdQty = new DAL.DataAccess.ItemDAO().GetPOOrderQtyOFBarcode(pono, barcode, items);
                    if (Toll != null) { tolleperc = Convert.ToDouble(Toll); }
                    double ord = Convert.ToDouble(OrdQty.ToString());

                    double minToll = ord - ((ord * tolleperc) / 100);
                    double maxToll = ord + ((ord * tolleperc) / 100);
                    double Orec =  Convert.ToDouble(data) ;
                    if (Orec < minToll || Orec > maxToll)
                    {

                        return "N;GRN Quantity exceeds from PO Qauntity.";
                    }
                   
                    else { return "Y;ALLOWED"; }

                }
                return "N;GRN Quantity cannot be exceed from PO Qauntity.";
            }
            else { return "N;GRN Quantity cannot be exceed from PO Qauntity."; }

        }

        private string ChecekPOisApprovedANDQuantity(string pono, string apprv, string compc, string brnch)
        {
            Logger.CreateLog("Function Start");
            string apprGRN = CheckGRNisApproved(pono, apprv, compc, brnch);
            //  string quantityGRN = CheckGRNQuantity(pono, apprv, compc, brnch);
            //if (apprv == "Y")
            //{
            //    DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

            //    // var data = itemContext.GETPOREMAIN(pono, compc, brnch);
            //    if (apprGRN.Split(';')[0].ToString() == "Y")
            //    {
            //        if (quantityGRN.Split(';')[0].ToString() == "Y")
            //        {
            //            //|| apprv != "Y"
            //            return quantityGRN;
            //        }
            //        else { return quantityGRN; }

            //    }
            //    return apprGRN;
            //}
            //else {
            return apprGRN;
        //}

        }
        private string GetPOTollerence(string pono)
        {
            DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();
            var data = itemContext.GetPOTollerence(pono);
            if (data != null) { return data.ToString(); } else { return "0"; }
        }
        private GRNProductInfo ConvertGetBarcodeDetailsByGRNDataRowToModelForWeb(EnumerableRowCollection<DataRow> enumerableRowCollection, string no, string type, string compc, string brnch, string pono)
        {
            try
            {
                return enumerableRowCollection.Select(
                            row => new GRNProductInfo
                            {

                                PId = row.Field<object>("ITEMCODE"),
                                PName = row.Field<object>("NAME"),
                                PItemCode = row.Field<object>("ITEMCODE"),
                                PProductBarcode = row.Field<object>("PRODUCTBARCODE"),

                                PIsThisImportedItem = row.Field<object>("ISTHISIMPORTEDITEM"),
                                POpeningDate = row.Field<object>("OPENINGDATE"),
                                PClosingDate = row.Field<object>("CLOSINGDATE"),
                                PProductClassificationCode = row.Field<object>("PRODUCTCLASSIFICATIONCODE"),
                                PProductClassificationCodeName = row.Field<object>("PRODUCTCLASSIFICATIONCODENAME"),
                                PHierarchyCode = row.Field<object>("PRODUCTHIERARCHYCODE"),
                                PHierarchyCodeName = row.Field<object>("PRODUCTHIERARCHYCODENAME"),
                                PBrandCode = row.Field<object>("BRANDCODE"),
                                PBrandCodeName = row.Field<object>("BRANDCODENAME"),
                                PDepartmentCode = row.Field<object>("DEPARTMENTCODE"),
                                PDepartmentCodeName = row.Field<object>("DEPARTMENTCODENAME"),
                                PColorCode = row.Field<object>("COLORCODE"),
                                PColorCodeName = row.Field<object>("COLORCODENAME"),

                                PSizeCode = row.Field<object>("SIZECODE"),
                                PSizeCodeName = row.Field<object>("SIZECODENAME"),
                                PABCIndicationCode = row.Field<object>("PRODUCTABCINDICATIONCODE"),
                                PABCIndicationCodeName = row.Field<object>("PRODUCTABCINDICATIONCODENAME"),
                                PGroupCode = row.Field<object>("PRODUCTGROUPCODE"),
                                PGroupCodeName = row.Field<object>("PRODUCTGROUPCODENAME"),
                                PPrincipleCode = row.Field<object>("PRINCIPLECODE"),
                                PPrincipleCodeName = row.Field<object>("PRINCIPLECODENAME"),

                                PDivisionCode = row.Field<object>("DIVISIONCODE"),
                                PDivisionCodeName = row.Field<object>("DIVISIONCODENAME"),
                                PVariantCode = row.Field<object>("VARIANTCODE"),
                                PVariantCodeName = row.Field<object>("VARIANTCODENAME"),
                                PMarketingTeamCode = row.Field<object>("MARKETINGTEAMCODE"),
                                PMarketingTeamCodeName = row.Field<object>("MARKETINGTEAMCODENAME"),

                                PShelfLifeMax = row.Field<object>("SHELFLIFEMAXIMUM"),
                                PShelfLifeMin = row.Field<object>("SHELFLIFEMINIMUM"),
                                PShelfLifeRemainingForSale = row.Field<object>("SHELFLIFEREMAININGFORSALE"),
                                PShelfLifeRemainingForProductIO = row.Field<object>("SHELFLIFEREMAININGFORPRODUCTIO"),
                                PSortingOrder = row.Field<object>("SORTINGORDER"),
                                PIsThisServiceItem = row.Field<object>("ISTHISSERVICEITEM"),

                                PIsThisMasterProduct = row.Field<object>("ISTHISMASTERPRODUCT"),
                                PIsThisItemSaleable = row.Field<object>("ISTHISITEM_SALEABLE"),
                                PIsThisItemPurchaseable = row.Field<object>("ISTHISITEM_PURCHASEABLE"),
                                PIsThisFixedAssetItem = row.Field<object>("ISTHISFIXEDASSETITEM"),
                                PMaintainInventory = row.Field<object>("MAINTAININVENTORY"),
                                PDoYouMakeThisItem = row.Field<object>("DOYOUMAKETHISITEM"),
                                PMaintainBatchNos = row.Field<object>("MAINTAINBATCHNOS"),
                                PMaintainSerialNo = row.Field<object>("MAINTAINSERIALNOS"),
                                PIsPriceAlreadyPrinted = row.Field<object>("ISPRICEALREADYPRINTED"),
                                PSeperatePriceForChildBarcode = row.Field<object>("SEPERATEPRICEFORCHILDBARCODE"),
                                PIsThisFreeFlowProduct = row.Field<object>("ISTHISFREEFLOWPRODUCT"),

                                PAutoPositionInspection = row.Field<object>("AUTOPOSTTOINSPECTION"),
                                PPiecesInCarton = row.Field<object>("PIECESINACARTON"),
                                PPiecesInPack = row.Field<object>("PIECESINAPACK"),
                                PPerPieceGramOrMililitre = row.Field<object>("PERPIECEGRAMAGEORMILILITRE"),
                                PRegistrationNo = row.Field<object>("REGISTRATIONNO"),
                                PGrossWeight = row.Field<object>("GROSSWEIGHT"),
                                PUOMCodeGrossWeight = row.Field<object>("UOMCODEGROSSWEIGHT"),
                                PNetWeight = row.Field<object>("NETWEIGHT"),
                                PVolume = row.Field<object>("VOLUME"),
                                PUOMCodeVolume = row.Field<object>("UOMCODEVOLUME"),
                                PProductHeight = row.Field<object>("PRODUCTHEIGHT"),
                                PProductDepth = row.Field<object>("PRODUCTDEPTH"),
                                PParentKey = row.Field<object>("PARENTKEY"),
                                PPrintPricedOnBarcode = row.Field<object>("PRINTPRICEONBARCODE"),
                                PPrintShortnameOnSlip = row.Field<object>("PRINTSHORTNAMEONSLIP"),
                                PMaxOrderQty = row.Field<object>("MAXIMUMORDERQUANTITY"),
                                PMinOrderQty = row.Field<object>("MINIMUMORDERQUANTITY"),
                                PReOrderLevelQty = row.Field<object>("REORDERLEVELQUANTITY"),
                                PMaxQty = row.Field<object>("MAXIMUMQUANTITY"),
                                PMinQty = row.Field<object>("MINIMUMQUANTITY"),
                                PProcurementGroupCode = row.Field<object>("PROCUREMENTGROUPCODE"),
                                PPOTolerence = GetPOTollerence(pono),
                                PLeadTimeTransfer = row.Field<object>("LEADTIME_TRANSFER"),
                                PLeadTimePurchase = row.Field<object>("LEADTIME_PURCHASES"),
                                PLeadTimeSales = row.Field<object>("LEADTIME_SALES"),
                                PLeadTimeProduction = row.Field<object>("LEADTIME_PRODUCTION"),
                                PArticleNo = row.Field<object>("ARTICLE_PARTNO"),
                                PCatelogDesignNo = row.Field<object>("CATALOG_DESIGNNO"),
                                PUnitOfMeasureCodeInventory = row.Field<object>("UNITOFMEASURECODE_INVENTORY"),
                                PUnitOfMeasureCodePurchase = row.Field<object>("UNITOFMEASURECODE_PURCHASES"),
                                PUnitOfMeasureCodeSales = row.Field<object>("UNITOFMEASURECODE_SALES"),
                                POQty = row.Field<object>("POQTY"),
                                Supplier = row.Field<object>("SUPPLIER"),
                                POQtyRem = row.Field<object>("POQTYREM"),
                                Allowed = CheckBarcodeisPresentANDisPResentinGRN(row.Field<object>("PRODUCTBARCODE").ToString(), no, type, compc, brnch, row.Field<object>("ITEMCODE").ToString(),pono).Split(';')[0],// ChecekPOisApprovedANDQuantity(row.Field<object>("DOCUMENTNO").ToString(), row.Field<object>("APPROVALYN").ToString(), "1", row.Field<object>("OFFICECODEBILLTO").ToString()).Split(';')[0],
                                Message = CheckBarcodeisPresentANDisPResentinGRN(row.Field<object>("PRODUCTBARCODE").ToString(), no, type, compc, brnch, row.Field<object>("ITEMCODE").ToString(),pono).Split(';')[1]// ChecekPOisApprovedANDQuantity(row.Field<object>("DOCUMENTNO").ToString(), row.Field<object>("APPROVALYN").ToString(), "1", row.Field<object>("OFFICECODEBILLTO").ToString()).Split(';')[1]

                            }).FirstOrDefault();


            }
            catch (Exception exception)
            {

                throw;
            }
        }

        private object ConvertGetBarcodeDetailsByGRNDataRowToModel(EnumerableRowCollection<DataRow> enumerableRowCollection, string no, string type, string compc, string brnch,string pono)
        {
            try
            {
                return enumerableRowCollection.Select(
                            row => new GRNProductInfo
                            {

                                PId = row.Field<object>("ITEMCODE"),
                                PName = row.Field<object>("NAME"),
                                PItemCode = row.Field<object>("ITEMCODE"),
                                PProductBarcode = row.Field<object>("PRODUCTBARCODE"),

                                PIsThisImportedItem = row.Field<object>("ISTHISIMPORTEDITEM"),
                                POpeningDate = row.Field<object>("OPENINGDATE"),
                                PClosingDate = row.Field<object>("CLOSINGDATE"),
                                PProductClassificationCode = row.Field<object>("PRODUCTCLASSIFICATIONCODE"),
                                PProductClassificationCodeName = row.Field<object>("PRODUCTCLASSIFICATIONCODENAME"),
                                PHierarchyCode = row.Field<object>("PRODUCTHIERARCHYCODE"),
                                PHierarchyCodeName = row.Field<object>("PRODUCTHIERARCHYCODENAME"),
                                PBrandCode = row.Field<object>("BRANDCODE"),
                                PBrandCodeName = row.Field<object>("BRANDCODENAME"),
                                PDepartmentCode = row.Field<object>("DEPARTMENTCODE"),
                                PDepartmentCodeName = row.Field<object>("DEPARTMENTCODENAME"),
                                PColorCode = row.Field<object>("COLORCODE"),
                                PColorCodeName = row.Field<object>("COLORCODENAME"),

                                PSizeCode = row.Field<object>("SIZECODE"),
                                PSizeCodeName = row.Field<object>("SIZECODENAME"),
                                PABCIndicationCode = row.Field<object>("PRODUCTABCINDICATIONCODE"),
                                PABCIndicationCodeName = row.Field<object>("PRODUCTABCINDICATIONCODENAME"),
                                PGroupCode = row.Field<object>("PRODUCTGROUPCODE"),
                                PGroupCodeName = row.Field<object>("PRODUCTGROUPCODENAME"),
                                PPrincipleCode = row.Field<object>("PRINCIPLECODE"),
                                PPrincipleCodeName = row.Field<object>("PRINCIPLECODENAME"),

                                PDivisionCode = row.Field<object>("DIVISIONCODE"),
                                PDivisionCodeName = row.Field<object>("DIVISIONCODENAME"),
                                PVariantCode = row.Field<object>("VARIANTCODE"),
                                PVariantCodeName = row.Field<object>("VARIANTCODENAME"),
                                PMarketingTeamCode = row.Field<object>("MARKETINGTEAMCODE"),
                                PMarketingTeamCodeName = row.Field<object>("MARKETINGTEAMCODENAME"),

                                PShelfLifeMax = row.Field<object>("SHELFLIFEMAXIMUM"),
                                PShelfLifeMin = row.Field<object>("SHELFLIFEMINIMUM"),
                                PShelfLifeRemainingForSale = row.Field<object>("SHELFLIFEREMAININGFORSALE"),
                                PShelfLifeRemainingForProductIO = row.Field<object>("SHELFLIFEREMAININGFORPRODUCTIO"),
                                PSortingOrder = row.Field<object>("SORTINGORDER"),
                                PIsThisServiceItem = row.Field<object>("ISTHISSERVICEITEM"),

                                PIsThisMasterProduct = row.Field<object>("ISTHISMASTERPRODUCT"),
                                PIsThisItemSaleable = row.Field<object>("ISTHISITEM_SALEABLE"),
                                PIsThisItemPurchaseable = row.Field<object>("ISTHISITEM_PURCHASEABLE"),
                                PIsThisFixedAssetItem = row.Field<object>("ISTHISFIXEDASSETITEM"),
                                PMaintainInventory = row.Field<object>("MAINTAININVENTORY"),
                                PDoYouMakeThisItem = row.Field<object>("DOYOUMAKETHISITEM"),
                                PMaintainBatchNos = row.Field<object>("MAINTAINBATCHNOS"),
                                PMaintainSerialNo = row.Field<object>("MAINTAINSERIALNOS"),
                                PIsPriceAlreadyPrinted = row.Field<object>("ISPRICEALREADYPRINTED"),
                                PSeperatePriceForChildBarcode = row.Field<object>("SEPERATEPRICEFORCHILDBARCODE"),
                                PIsThisFreeFlowProduct = row.Field<object>("ISTHISFREEFLOWPRODUCT"),

                                PAutoPositionInspection = row.Field<object>("AUTOPOSTTOINSPECTION"),
                                PPiecesInCarton = row.Field<object>("PIECESINACARTON"),
                                PPiecesInPack = row.Field<object>("PIECESINAPACK"),
                                PPerPieceGramOrMililitre = row.Field<object>("PERPIECEGRAMAGEORMILILITRE"),
                                PRegistrationNo = row.Field<object>("REGISTRATIONNO"),
                                PGrossWeight = row.Field<object>("GROSSWEIGHT"),
                                PUOMCodeGrossWeight = row.Field<object>("UOMCODEGROSSWEIGHT"),
                                PNetWeight = row.Field<object>("NETWEIGHT"),
                                PVolume = row.Field<object>("VOLUME"),
                                PUOMCodeVolume = row.Field<object>("UOMCODEVOLUME"),
                                PProductHeight = row.Field<object>("PRODUCTHEIGHT"),
                                PProductDepth = row.Field<object>("PRODUCTDEPTH"),
                                PParentKey = row.Field<object>("PARENTKEY"),
                                PPrintPricedOnBarcode = row.Field<object>("PRINTPRICEONBARCODE"),
                                PPrintShortnameOnSlip = row.Field<object>("PRINTSHORTNAMEONSLIP"),
                                PMaxOrderQty = row.Field<object>("MAXIMUMORDERQUANTITY"),
                                PMinOrderQty = row.Field<object>("MINIMUMORDERQUANTITY"),
                                PReOrderLevelQty = row.Field<object>("REORDERLEVELQUANTITY"),
                                PMaxQty = row.Field<object>("MAXIMUMQUANTITY"),
                                PMinQty = row.Field<object>("MINIMUMQUANTITY"),
                                PProcurementGroupCode = row.Field<object>("PROCUREMENTGROUPCODE"),
                                PPOTolerence = GetPOTollerence(pono),
                                PLeadTimeTransfer = row.Field<object>("LEADTIME_TRANSFER"),
                                PLeadTimePurchase = row.Field<object>("LEADTIME_PURCHASES"),
                                PLeadTimeSales = row.Field<object>("LEADTIME_SALES"),
                                PLeadTimeProduction = row.Field<object>("LEADTIME_PRODUCTION"),
                                PArticleNo = row.Field<object>("ARTICLE_PARTNO"),
                                PCatelogDesignNo = row.Field<object>("CATALOG_DESIGNNO"),
                                PUnitOfMeasureCodeInventory = row.Field<object>("UNITOFMEASURECODE_INVENTORY"),
                                PUnitOfMeasureCodePurchase = row.Field<object>("UNITOFMEASURECODE_PURCHASES"),
                                PUnitOfMeasureCodeSales = row.Field<object>("UNITOFMEASURECODE_SALES"),
                                POQty = row.Field<object>("POQTY"),
                                Supplier = row.Field<object>("SUPPLIER"),
                                POQtyRem = row.Field<object>("POQTYREM"),
                                Allowed = CheckBarcodeisPresentANDisPResentinGRN(row.Field<object>("PRODUCTBARCODE").ToString(), no, type,compc,brnch, row.Field<object>("ITEMCODE").ToString(),pono).Split(';')[0],// ChecekPOisApprovedANDQuantity(row.Field<object>("DOCUMENTNO").ToString(), row.Field<object>("APPROVALYN").ToString(), "1", row.Field<object>("OFFICECODEBILLTO").ToString()).Split(';')[0],
                                Message = CheckBarcodeisPresentANDisPResentinGRN(row.Field<object>("PRODUCTBARCODE").ToString(), no, type, compc, brnch, row.Field<object>("ITEMCODE").ToString(),pono).Split(';')[1]// ChecekPOisApprovedANDQuantity(row.Field<object>("DOCUMENTNO").ToString(), row.Field<object>("APPROVALYN").ToString(), "1", row.Field<object>("OFFICECODEBILLTO").ToString()).Split(';')[1]

                            });


            }
            catch (Exception exception)
            {

                throw;
            }
        }

        private string CheckBarcodeisPresent(string barcode)
        {

            DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

            string data = itemContext.GETBARCODE(barcode).ToString();
            if (data != null)
            {
                return "Y;BARCODE IS PRESENT";
            }
            else
            {
                return "N;BARCODE IS NOT PRESENT";
            }



        }
        bool CheckTollarance = false;
        private string CheckGRNApprov(string no, string type, string compc, string brnch, string barcode, string ITEMS, string pono)
        {

            DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();
            //check Exist po or to
            var data = itemContext.GETGRNAPPROVED(pono, type);
            if (data != null)
            {

                //If if(approve or exceed toll
                if (type.ToUpper() == "GRN")
                {

                    var dataSub = itemContext.GETGRNAPPROVAL(no, type);
                    if (dataSub != null)
                    {

                        if (dataSub.ToString() == "A") { return "N;GRN Already approved"; }
                        else { return "Y;Allowed"; }

                    }
                    else {
                        if (CheckTollarance)
                        {
                            return CheckGRNQuantity(pono, "Y", compc, brnch, barcode, ITEMS);
                        }
                        else { return "Y;Allowed"; }

                    }

                }
                //IF IN Check Our Approval
                else if (type.ToUpper() == "TIN")
                {

                    var dataSub = itemContext.GETGRNAPPROVAL(no, type);
                    if (dataSub != null)
                    {

                        if (dataSub.ToString() == "A") { return "N;PO Already approved"; }
                        else { return "Y;You can add "; }

                    }
                    else { return "N;N/A"; }



                }
                else if (type.ToUpper() == "TOUT")
                {

                    var dataSub = itemContext.GETGRNAPPROVAL(no, type);
                    if (dataSub != null)
                    {

                        if (dataSub.ToString() == "A") { return "N;Indent Already approved"; }
                        else { return "Y;You can add "; }

                    }
                    else { return "N;N/A"; }



                }
                return "N;N/A";
            }
            else
            {
                if (type.ToUpper() == "GRN")
                {
                    return "N;PO not not found or not approved";
                }
                else if (type.ToUpper() == "TIN")
                {
                    return "N;OUT not not found or not approved";
                }
                else {
                    return "N;Indent not not found or not approved";
                }

            }



            //if (type.ToUpper() == "GRN")
            //{
            //    DataTable _poNumberAgainstGRN = itemContext.GetPONumberAgainstGRN(no);
            //    if (_poNumberAgainstGRN.Rows.Count > 0)
            //    {
            //        return CheckGRNQuantity(_poNumberAgainstGRN.Rows[0]["PONO"].ToString(), "Y", "1", "002");
            //    }
            //    else
            //        return "N;GRN is not approved.";
            //}
            //else if (type.ToUpper() == "TIN") { return "N;Tranfer In is not approved."; }
            // else { return "N;N/A."; }


        }
        private string CheckBarcodeisPresentANDisPResentinGRN(string barcode, string no, string type,string compc, string brnch,string ITEMS,string pono)
        {
            string barcodeExists = CheckBarcodeisPresent(barcode);
            string grnApproved = CheckGRNApprov(no, type, compc, brnch, barcode,ITEMS,pono);
            if (barcodeExists.Split(';')[0].ToString() == "Y")
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

                // var data = itemContext.GETPOREMAIN(pono, compc, brnch);
                if (grnApproved.Split(';')[0].ToString() == "Y")
                {
                    return grnApproved;

                }
                return grnApproved;
            }
            else { return barcodeExists; }

        }
        //GetTODWithAlFeilds
        private TransferOut ConvertGetTOWithAllFeildsDataRowToModel(EnumerableRowCollection<DataRow> enumerableRowCollection)
        {
            try
            {
                var poDataSet = enumerableRowCollection.Select(
                            row => new TransferOut
                            {


                                OUT_NO = row.Field<object>("OUT_#"),
                                USRID = row.Field<object>("USRID"),
                                LOGNO = row.Field<object>("LOGNO"),
                                EDATE = row.Field<object>("EDATE"),
                                LINENO = row.Field<object>("LINE#"),
                                ITEMS = row.Field<object>("ITEMS"),
                                BATCH = row.Field<object>("BATCH"),
                                OUT_Q = row.Field<object>("OUT_Q"),
                                BON_Q = row.Field<object>("BON_Q"),
                                COSTP = row.Field<object>("COSTP"),
                                TRADP = row.Field<object>("TRADP"),
                                AMONT = row.Field<object>("AMONT"),
                                NOTES = row.Field<object>("NOTES"),
                                INDLNO = row.Field<object>("INDL#"),
                                BARCODE = row.Field<object>("BARCODE"),
                                ISU_Q = row.Field<object>("ISU_Q"),
                                GRL_NO = row.Field<object>("GRL_#"),
                                EXPEN = row.Field<object>("EXPEN"),
                                S2DLNO = row.Field<object>("S2DL#"),
                                AVGRT = row.Field<object>("AVGRT"),
                                UNITQ = row.Field<object>("UNITQ"),
                                PCK_QTY = row.Field<object>("PCK_QTY"),
                                EXPRY = row.Field<object>("EXPRY"),
                                REFNO = row.Field<object>("REFNO"),
                                QTY_IN = row.Field<object>("QTY_IN"),

                            }).FirstOrDefault();
                return poDataSet;
            }
            catch (Exception exception)
            {

                throw;
            }
        }
        private PurchaseOrder ConvertGetPOWithAllFeildsDataRowToModel(EnumerableRowCollection<DataRow> enumerableRowCollection)
        {
            try
            {
                var poDataSet = enumerableRowCollection.Select(
                            row => new PurchaseOrder
                            {

                                PO_NO = row.Field<object>("PO_NO"),
                                USRID = row.Field<object>("USRID"),
                                LOGNO = row.Field<object>("LOGNO"),
                                EDATE = row.Field<object>("EDATE"),
                                REQLbo = row.Field<object>("REQL#"),
                                QOTLno = row.Field<object>("QOTL#"),
                                LINEno = row.Field<object>("LINE#"),
                                ITEMS = row.Field<object>("ITEMS"),
                                REQ_Q = row.Field<object>("REQ_Q"),
                                RAT_G = row.Field<object>("RAT_G"),
                                DIS_P = row.Field<object>("DIS_P"),
                                COM_P = row.Field<object>("COM_P"),
                                GST_P = row.Field<object>("GST_P"),
                                SED_P = row.Field<object>("SED_P"),
                                CDT_P = row.Field<object>("CDT_P"),
                                EXT_P = row.Field<object>("EXT_P"),
                                DIS_A = row.Field<object>("DIS_A"),
                                COM_A = row.Field<object>("COM_A"),
                                GST_A = row.Field<object>("GST_A"),
                                SED_A = row.Field<object>("SED_A"),
                                EXT_A = row.Field<object>("EXT_A"),
                                CDT_A = row.Field<object>("CDT_A"),
                                RAT_N = row.Field<object>("RAT_N"),
                                ITMRT = row.Field<object>("ITMRT"),
                                ORD_Q = row.Field<object>("ORD_Q"),
                                BON_Q = row.Field<object>("BON_Q"),
                                AMONT = row.Field<object>("AMONT"),
                                DELDT = row.Field<object>("DELDT"),
                                PACKS = row.Field<object>("PACKS"),
                                NOTES = row.Field<object>("NOTES"),
                                BARCODE = row.Field<object>("BARCODE"),
                                FOC_COST_SHARE = row.Field<object>("FOC_COST_SHARE"),
                                SHARE_FORMULA = row.Field<object>("SHARE_FORMULA"),
                                APPLY_ON_ALL_ITEMS = row.Field<object>("APPLY_ON_ALL_ITEMS"),
                                MARGIN_T = row.Field<object>("MARGIN_T"),
                                LAST_COST = row.Field<object>("LAST_COST"),
                                MARGIN_A = row.Field<object>("MARGIN_A"),
                                MARGIN_P = row.Field<object>("MARGIN_P"),
                                EDAGST_A = row.Field<object>("EDAGST_A"),
                                EDAGST_P = row.Field<object>("EDAGST_P"),
                                GST_W = row.Field<object>("GST_W"),
                                GST_FOC = row.Field<object>("GST_FOC"),
                                GST_B = row.Field<object>("GST_B"),
                                GROSV = row.Field<object>("GROSV"),
                                PACK_RATE = row.Field<object>("PACK_RATE"),
                                UNIT_QTY = row.Field<object>("UNIT_QTY"),
                                PACK_QTY = row.Field<object>("PACK_QTY"),
                                BARCODE_QTY = row.Field<object>("BARCODE_QTY"),
                                I_H_Q = row.Field<object>("I_H_Q"),
                                CONSM = row.Field<object>("CONSM"),
                                GST_I = row.Field<object>("GST_I"),
                                MRP_G = row.Field<object>("MRP_G"),
                                RATEno = row.Field<object>("RATE#"),
                                RBT_P = row.Field<object>("RBT_P"),
                                RBT_A = row.Field<object>("RBT_A"),
                                RAT_BR = row.Field<object>("RAT_BR"),
                                AMT_AR = row.Field<object>("AMT_AR"),
                                METER = row.Field<object>("METER"),
                                CONVR = row.Field<object>("CONVR")

                            }).FirstOrDefault();
                return poDataSet;
            }
            catch (Exception exception)
            {

                throw;
            }
        }
        //
        private object ConvertGetGetIndDetailsByIndNoDataRowToModel(EnumerableRowCollection<DataRow> enumerableRowCollection)
        {
            try
            {
                var poDataSet = enumerableRowCollection.Select(
                            row => new IndInfo
                            {

                                
                                ID = row.Field<object>("ID"),
                                APPROVALYN = row.Field<object>("APPROVALYN"),
                                BLOCKEDYN = row.Field<object>("BLOCKEDYN"),
                                DOCUMENTNO = row.Field<object>("DOCUMENTNO"),
                                WORKDATE = row.Field<object>("WORKDATE"),
                                USERCODEREQUESTEDBY = row.Field<object>("USERCODEREQUESTEDBY"),
                                USERCODEREQUESTBYNAME = row.Field<object>("USERCODEREQUESTBYNAME"),
                                POROUNDAMOUNT = row.Field<object>("POROUNDAMOUNT"),
                                QTY = row.Field<object>("QTY"),
                                STOREFROM = row.Field<object>("STOREFROM"),
                                STORETO = row.Field<object>("STORETO"),
                                BRANCH = row.Field<object>("BRANCH"),
                                COMPANY = row.Field<object>("COMPANY"),
                                Allowed = IndCheckIndpproveAndQty(row.Field<object>("ID").ToString(), row.Field<object>("COMPANY").ToString(), row.Field<object>("BRANCH").ToString()).Split(';')[0],//Products = GetStockInProductInfoByStockInId(GetParamsForStockinProduct(row))
                                Message = IndCheckIndpproveAndQty(row.Field<object>("ID").ToString(), row.Field<object>("COMPANY").ToString(), row.Field<object>("BRANCH").ToString()).Split(';')[1],// Products = null
                                                                                                                                                                                                                     //   Allowed = row.Field<object>("ID"),
                                                                                                                                                                                                                     // Message = row.Field<object>("ID"),



                            }).FirstOrDefault();

                using (DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO())
                {
                    

                    if (poDataSet != null)
                        poDataSet.Products = (ConvertIndentProductInfoDataRowToModel(itemContext.GetIndOutByIndentDetail(poDataSet.ID.ToString()).AsEnumerable()));
                }

                return poDataSet;
            }
            catch (Exception exception)
            {

                throw;
            }
        }
        private object ConvertGetPODetailByPOOrArticleNoDataRowToModel(EnumerableRowCollection<DataRow> enumerableRowCollection)
        {
            try
            {
                var poDataSet = enumerableRowCollection.Select(
                            row => new POInfo
                            {
                                POId = row.Field<object>("ID"),
                                POPostedYn = row.Field<object>("POSTEDYN"),
                                POApprovalYn = row.Field<object>("APPROVALYN"),
                                POBlockedYn = row.Field<object>("BLOCKEDYN"),
                                POOfficeCodeShipTo = row.Field<object>("OFFICECODESHIPTO"),
                                POOfficeCodeShipToName = row.Field<object>("OFFICECODE_SHIPTONAME"),
                                POOfficeCodeBillTo = row.Field<object>("OFFICECODEBILLTO"),
                                POOfficeCodeBillToName = row.Field<object>("OFFICECODE_BILLTONAME"),
                                PODocumentNo = row.Field<object>("DOCUMENTNO"),
                                POWorkDate = row.Field<object>("WORKDATE"),

                                POUsercodeRequestBy = row.Field<object>("USERCODEREQUESTEDBY"),
                                POUsercodeRequestByName = row.Field<object>("USERCODEREQUESTBYNAME"),
                                POBusinessPartnerCode = row.Field<object>("BUSINESSPARTNERCODE"),
                                POBusinessPartnerCodeName = row.Field<object>("BUSINESSPARTNERNAME"),
                                POCurrencyCode = row.Field<object>("CURENCYCODE"),
                                POCurrencyCodeName = row.Field<object>("CURRENCYNAME"),
                                POCurrencyRate = row.Field<object>("CURRENCYRATE"),
                                POIsLocalOrImported = row.Field<object>("POISLOCALORIMPORTED"),
                                POCountryCode = row.Field<object>("COUNTRYCODE"),
                                POCountryCodeName = row.Field<object>("COUNTRYNAME"),

                                POPaymentTermCode = row.Field<object>("PAYMENTTERMCODE"),
                                POPaymentTermCodeName = row.Field<object>("PAYMENTTERMNAME"),

                                POTolenrancePer = row.Field<object>("TOLERENCEPERCENTAGE"),
                                PORevisionNo = row.Field<object>("POREVISIONNO"),

                                PODeliveryDate = row.Field<object>("DELIVERYDATE"),
                                POValidUpTo = row.Field<object>("POVALIDUPTO"),
                                PONumberOfItems = row.Field<object>("NUMBEROFITEMS"),
                                POValue = row.Field<object>("POVALUE"),
                                PODiscountPercentage = row.Field<object>("PODISCOUNTPERCENTAGE"),
                                PODiscountAmount = row.Field<object>("PODISCOUNTAMOUNT"),
                                PORoundAmount = row.Field<object>("POROUNDAMOUNT"),
                                PONetValue = row.Field<object>("PONETVALUE"),

                                POOrigionOfShipment = row.Field<object>("ORIGIONOFSHIPMENT"),
                                POReferenceNo = row.Field<object>("REFERENCENO"),
                                PODetails = row.Field<object>("DETAILS"),
                                Allowed ="Y", // ChecekPOisApprovedANDQuantity(row.Field<object>("DOCUMENTNO").ToString(), row.Field<object>("APPROVALYN").ToString(), "1", row.Field<object>("OFFICECODEBILLTO").ToString()).Split(';')[0],
                                Message = "Allowed",// ChecekPOisApprovedANDQuantity(row.Field<object>("DOCUMENTNO").ToString(), row.Field<object>("APPROVALYN").ToString(), "1", row.Field<object>("OFFICECODEBILLTO").ToString()).Split(';')[1],
                                POExtraField01 = row.Field<object>("EXTRAFIELDS01"),
                                POExtraField02 = row.Field<object>("EXTRAFIELDS02"),
                                POExtraField03 = row.Field<object>("EXTRAFIELDS03"),
                                POExtraField04 = row.Field<object>("EXTRAFIELDS04"),
                                POExtraField05 = row.Field<object>("EXTRAFIELDS05"),
                                POExtraField06 = row.Field<object>("EXTRAFIELDS06"),
                                POExtraField07 = row.Field<object>("EXTRAFIELDS07"),
                                POExtraField08 = row.Field<object>("EXTRAFIELDS08"),
                                POExtraField09 = row.Field<object>("EXTRAFIELDS09"),
                                POExtraField10 = row.Field<object>("EXTRAFIELDS10")

                            }).ToList();

                using (DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO())
                {

                    foreach (var poRow in poDataSet)
                    {



                        poRow.Products = (ConvertGetPOItemsDetailByPOOrArticleNoDataRowToModel(itemContext.GetPOByPOORSupplierDetail(poRow.POId.ToString()).AsEnumerable()));
                    }
                }

                return poDataSet;
            }
            catch (Exception exception)
            {

                throw;
            }
        }

        private List<POProductInfo> ConvertGetPOItemsDetailByPOOrArticleNoDataRowToModel(EnumerableRowCollection<DataRow> enumerableRowCollection)
        {
            try
            {
                return enumerableRowCollection.Select(
                            row => new POProductInfo
                            {
                                POPId = row.Field<object>("DETAIL_ID"),
                                POPProductCode = row.Field<object>("DETAIL_PRODUCTCODE"),
                                POPProductBarcode = row.Field<object>("DETAIL_PRODUCTBARCODE"),
                                POPPriceCode = row.Field<object>("DETAIL_PRICECODE"),
                                POPCostPrice = row.Field<object>("DETAIL_COSTPRICE"),
                                POPSalesPrice = row.Field<object>("DETAIL_SALESPRICE"),
                                POPRetailPrice = row.Field<object>("DETAIL_RETAILPRICE"),
                                POPOrderQty = row.Field<object>("DETAIL_ORDERQTY"),
                                POPOrderValue = row.Field<object>("DETAIL_ORDERVALUE"),
                                POPUOMCode = row.Field<object>("DETAIL_UOMCODE"),
                                POPUOMCodeName = row.Field<object>("DETAIL_UOMCODENAME"),
                                POPNeedByDate = row.Field<object>("DETAIL_NEEDBYDATE"),
                                POPInHandQty = row.Field<object>("DETAIL_INHANDQTY"),
                                POPReqQty = row.Field<object>("DETAIL_REQUIREDQTY"),
                                POPReferenceTableCode = row.Field<object>("DETAIL_REFERENCETABLECODE"),
                                POPReferenceDocumentId = row.Field<object>("DETAIL_REFERENCEDOCUMENTID"),
                                POPPackagingDetailCode = row.Field<object>("DETAIL_PACKINGDETAILSCODE"),
                                POPPackagingDetails = row.Field<object>("DETAIL_PACKAGINGDETAILS"),
                                POPDetails = row.Field<object>("DETAIL_DETAILS"),
                                POPOrderQty1 = row.Field<object>("DETAIL_ORDERQTY1"),
                                POPOrderQty2 = row.Field<object>("DETAIL_ORDERQTY2"),
                                POPOrderQty3 = row.Field<object>("DETAIL_ORDERQTY3"),
                                POPCostPrice1 = row.Field<object>("DETAIL_COSTPRICE1"),
                                POPCostPrice2 = row.Field<object>("DETAIL_COSTPRICE2"),
                                POPCostPrice3 = row.Field<object>("DETAIL_COSTPRICE3"),
                                POPFreeOfCostQty = row.Field<object>("DETAIL_FREEOFCOSTQTY"),

                                POPDiscountAmount = row.Field<object>("DETAIL_DISCOUNTAMOUNT"),
                                POPGrossValueAfterDiscount = row.Field<object>("DETAIL_GROSSVALUEAFTERDISCOUNT"),
                                POPCalculateSalesTaxFOCQty = row.Field<object>("DETAIL_CALCSALESTAXFOCQTY"),
                                POPSalesTaxPer = row.Field<object>("DETAIL_SALESTAXPER"),
                                POPSalesTaxAmount = row.Field<object>("DETAIL_SALESTAXAMOUNT"),
                                POPExciseDutyPer = row.Field<object>("DETAIL_EXCISEDUTYPER"),
                                POPExciseDutyAmount = row.Field<object>("DETAIL_EXCISEDUTYAMOUNT"),
                                POPExtraDiscountPer = row.Field<object>("DETAIL_EXTRADISCOUNTPER"),
                                POPExtraDiscountAmount = row.Field<object>("DETAIL_EXTRADISCOUNTAMOUNT"),
                                POPComissionPer = row.Field<object>("DETAIL_COMMISIONPER"),
                                POPComissionAmount = row.Field<object>("DETAIL_COMMISIONAMOUNT"),
                                POPCustomDutyPer = row.Field<object>("DETAIL_CUSTOMDUTYPER"),
                                POPCustomDutyAmount = row.Field<object>("DETAIL_CUSTOMDUTYAMT"),

                                PId = row.Field<object>("PRODUCT_ID"),
                                PName = row.Field<object>("PRODUCT_NAME"),
                                PItemCode = row.Field<object>("PRODUCT_ITEMCODE"),
                                PProductBarcode = row.Field<object>("PRODUCT_PRODUCTBARCODE"),

                                PIsThisImportedItem = row.Field<object>("ISTHISIMPORTEDITEM"),
                                POpeningDate = row.Field<object>("OPENINGDATE"),
                                PClosingDate = row.Field<object>("CLOSINGDATE"),
                                PProductClassificationCode = row.Field<object>("PRODUCTCLASSIFICATIONCODE"),
                                PProductClassificationCodeName = row.Field<object>("PRODUCTCLASSFICATIONCODENAME"),
                                PHierarchyCode = row.Field<object>("PRODUCTHIERARCHYCODE"),
                                PHierarchyCodeName = row.Field<object>("PRODUCTHIERARCHYNAME"),
                                PBrandCode = row.Field<object>("BRANDCODE"),
                                PBrandCodeName = row.Field<object>("PRODUCTBRANDNAME"),
                                PDepartmentCode = row.Field<object>("DEPARTMENTCODE"),
                                PDepartmentCodeName = row.Field<object>("DEPARTMENTCODENAME"),
                                PColorCode = row.Field<object>("COLORCODE"),
                                PColorCodeName = row.Field<object>("COLORCODENAME"),

                                PSizeCode = row.Field<object>("SIZECODE"),
                                PSizeCodeName = row.Field<object>("SIZECODENAME"),
                                PABCIndicationCode = row.Field<object>("PRODUCTABCINDICATIONCODE"),
                                PABCIndicationCodeName = row.Field<object>("PRODUCTABCINDICATIONCODENAME"),
                                PGroupCode = row.Field<object>("PRODUCTGROUPCODE"),
                                PGroupCodeName = row.Field<object>("PRODUCTGROUPCODENAME"),
                                PPrincipleCode = row.Field<object>("PRINCIPLECODE"),
                                PPrincipleCodeName = row.Field<object>("PRINCIPLECODENAME"),

                                PDivisionCode = row.Field<object>("DIVISIONCODE"),
                                PDivisionCodeName = row.Field<object>("DIVISIONCODENAME"),
                                PVariantCode = row.Field<object>("VARIANTCODE"),
                                PVariantCodeName = row.Field<object>("VARIANTCODENAME"),
                                PMarketingTeamCode = row.Field<object>("MARKETINGTEAMCODE"),
                                PMarketingTeamCodeName = row.Field<object>("MARKETINGCODENAME"),

                                PShelfLifeMax = row.Field<object>("SHELFLIFEMAXIMUM"),
                                PShelfLifeMin = row.Field<object>("SHELFLIFEMINIMUM"),
                                PShelfLifeRemainingForSale = row.Field<object>("SHELFLIFEREMAININGFORSALE"),
                                PShelfLifeRemainingForProductIO = row.Field<object>("SHELFLIFEREMAININGFORPRODUCTIO"),
                                PSortingOrder = row.Field<object>("SORTINGORDER"),
                                PIsThisServiceItem = row.Field<object>("ISTHISSERVICEITEM"),

                                PIsThisMasterProduct = row.Field<object>("ISTHISMASTERPRODUCT"),
                                PIsThisItemSaleable = row.Field<object>("ISTHISITEM_SALEABLE"),
                                PIsThisItemPurchaseable = row.Field<object>("ISTHISITEM_PURCHASEABLE"),
                                PIsThisFixedAssetItem = row.Field<object>("ISTHISFIXEDASSETITEM"),
                                PMaintainInventory = row.Field<object>("MAINTAININVENTORY"),
                                PDoYouMakeThisItem = row.Field<object>("DOYOUMAKETHISITEM"),
                                PMaintainBatchNos = row.Field<object>("MAINTAINBATCHNOS"),
                                PMaintainSerialNo = row.Field<object>("MAINTAINSERIALNOS"),
                                PIsPriceAlreadyPrinted = row.Field<object>("ISPRICEALREADYPRINTED"),
                                PSeperatePriceForChildBarcode = row.Field<object>("SEPERATEPRICEFORCHILDBARCODE"),
                                PIsThisFreeFlowProduct = row.Field<object>("ISTHISFREEFLOWPRODUCT"),

                                PAutoPositionInspection = row.Field<object>("AUTOPOSTTOINSPECTION"),
                                PPiecesInCarton = row.Field<object>("PIECESINACARTON"),
                                PPiecesInPack = row.Field<object>("PIECESINAPACK"),
                                PPerPieceGramOrMililitre = row.Field<object>("PERPIECEGRAMAGEORMILILITRE"),
                                PRegistrationNo = row.Field<object>("REGISTRATIONNO"),
                                PGrossWeight = row.Field<object>("GROSSWEIGHT"),
                                PUOMCodeGrossWeight = row.Field<object>("UOMCODEGROSSWEIGHT"),
                                PNetWeight = row.Field<object>("NETWEIGHT"),
                                PVolume = row.Field<object>("VOLUME"),
                                PUOMCodeVolume = row.Field<object>("UOMCODEVOLUME"),
                                PProductHeight = row.Field<object>("PRODUCTHEIGHT"),
                                PProductDepth = row.Field<object>("PRODUCTDEPTH"),
                                PParentKey = row.Field<object>("PARENTKEY"),
                                PPrintPricedOnBarcode = row.Field<object>("PRINTPRICEONBARCODE"),
                                PPrintShortnameOnSlip = row.Field<object>("PRINTSHORTNAMEONSLIP"),
                                PMaxOrderQty = row.Field<object>("MAXIMUMORDERQUANTITY"),
                                PMinOrderQty = row.Field<object>("MINIMUMORDERQUANTITY"),
                                PReOrderLevelQty = row.Field<object>("REORDERLEVELQUANTITY"),
                                PMaxQty = row.Field<object>("MAXIMUMQUANTITY"),
                                PMinQty = row.Field<object>("MINIMUMQUANTITY"),
                                PProcurementGroupCode = row.Field<object>("PROCUREMENTGROUPCODE"),
                                PPOTolerence = row.Field<object>("POTOLERENCE"),
                                PLeadTimeTransfer = row.Field<object>("LEADTIME_TRANSFER"),
                                PLeadTimePurchase = row.Field<object>("LEADTIME_PURCHASES"),
                                PLeadTimeSales = row.Field<object>("LEADTIME_SALES"),
                                PLeadTimeProduction = row.Field<object>("LEADTIME_PRODUCTION"),
                                PArticleNo = row.Field<object>("ARTICLE_PARTNO"),
                                PCatelogDesignNo = row.Field<object>("CATALOG_DESIGNNO"),
                                PUnitOfMeasureCodeInventory = row.Field<object>("UNITOFMEASURECODE_INVENTORY"),
                                PUnitOfMeasureCodePurchase = row.Field<object>("UNITOFMEASURECODE_PURCHASES"),
                                PUnitOfMeasureCodeSales = row.Field<object>("UNITOFMEASURECODE_SALES")


                            }).ToList();
            }
            catch (Exception exception)
            {

                throw;
            }
        }




        public object GetStockOutInInfoByDocumentNo(object[] param)
        {
            try
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();
                string store = "";
                var data = itemContext.GetStoreOfBranch(param[0].ToString());
                if (data != null) { store = data.ToString(); }
                return new
                {
                   
                    stock_out = (ConvertStockOutMasterInfoDataRowToModel(itemContext.GetStockOut(param[0].ToString(), param[1].ToString(), param[2].ToString(), store).AsEnumerable())),
                    stock_in = (ConvertStockInMasterInfoDataRowToModel(itemContext.GetStockIN(param[0].ToString(), param[1].ToString(), param[2].ToString()).AsEnumerable())),
                };

                //                using (ADOContext DB = new ADOContext())
                //                {
                //                    IDbCommand _Command = DB.CreateCommand(ADOHelper.GetParamterizedQuery(@"
                //SELECT SM.ID, SM.COMPANYCODE, COM_CODENAME(1,'COM_COMPANY',SM.COMPANYCODE, NULL,NULL) AS COMPANYNAME, 
                //       SM.DOCUMENTNO, SM.SALESORGCODE, COM_CODENAME(1,'COM_SALESORGANIZATION',SM.SALESORGCODE,NULL,NULL) AS SALESORGCODENAME,
                //       SM.OFFICECODE, COM_CODENAME(1,'COM_OFFICES', SM.OFFICECODE,NULL,NULL) AS OFFICECODENAME, SM.DOCUMENTNO,
                //       TO_CHAR(SM.WORKDATE,'DD-MON-YYYY') AS WORKDATE, SM.TRANSFERTYPE, 
                //       COM_CODENAME(1,'STR_TRANSFERTYPES', SM.TRANSFERTYPE,NULL,NULL ) AS TRANSFERTYPENAME, 
                //       SM.WAREHOUSECODE, COM_CODENAME(1,'STR_PRODUCTWAREHOUSE', SM.WAREHOUSECODE,NULL,NULL) AS WAREHOUSENAME,
                //       SM.USERCODESENTBY, COM_CODENAME(1,'SEC_USERINFORMATION', SM.USERCODESENTBY, NULL,NULL ) AS USERCODESENTBYNAME,
                //       SM.OFFICECODESENTTO, COM_CODENAME(1,'COM_OFFICES', SM.OFFICECODESENTTO,NULL,NULL) AS OFFICESENTTONAME,
                //       SM.STOCKSTATUSCODE, SM.STOCKOUTTYPECODE, COM_CODENAME(1,'STR_STOCKSTATUSTYPES',SM.STOCKOUTTYPECODE,NULL,NULL) AS STOCKSTATUSTYPECODENAME,
                //       SM.REFERENCENO, SM.DETAILS, SM.EXTRAFIELDS01,
                //       SM.EXTRAFIELDS02, SM.EXTRAFIELDS03, SM.EXTRAFIELDS04, SM.EXTRAFIELDS05, SM.EXTRAFIELDS06, SM.EXTRAFIELDS07,
                //       SM.EXTRAFIELDS08, SM.EXTRAFIELDS09, SM.EXTRAFIELDS10, SM.MAPCODE, SM.REFERENCETABLECODE, SM.REFERENCEDOCUMENTID
                //FROM STR_STOCKOUT01MASTER SM
                //WHERE SM.COMPANYCODE  = {0} AND SM.OFFICECODESENTTO = {1} AND SM.DOCUMENTNO  ='{2}' AND SM.BLOCKEDYN  = 'N'", false, true, param));

                //                    IDbCommand _CommandStockIn = DB.CreateCommand(ADOHelper.GetParamterizedQuery(@"
                //SELECT SM.ID, SM.COMPANYCODE, COM_CODENAME(1,'COM_COMPANY',SM.COMPANYCODE, NULL,NULL) AS COMPANYNAME, 
                //       SM.DOCUMENTNO, SM.SALESORGCODE, COM_CODENAME(1,'COM_SALESORGANIZATION',SM.SALESORGCODE,NULL,NULL) AS SALESORGCODENAME,
                //       SM.OFFICECODE, COM_CODENAME(1,'COM_OFFICES', SM.OFFICECODE,NULL,NULL) AS OFFICECODENAME, SM.DOCUMENTNO,
                //       TO_CHAR(SM.WORKDATE,'DD-MON-YYYY') AS WORKDATE, SM.TRANSFERTYPE, 
                //       COM_CODENAME(1,'STR_TRANSFERTYPES', SM.TRANSFERTYPE,NULL,NULL ) AS TRANSFERTYPENAME, 
                //       SM.WAREHOUSECODE, COM_CODENAME(1,'STR_PRODUCTWAREHOUSE', SM.WAREHOUSECODE,NULL,NULL) AS WAREHOUSENAME,
                //       SM.USERCODERECEIVEDBY, COM_CODENAME(1,'SEC_USERINFORMATION', SM.USERCODERECEIVEDBY, NULL,NULL ) AS USERCODERECEIVEDBYNAME,
                //       SM.OFFICECODERECEIVEDFROM, COM_CODENAME(1,'COM_OFFICES', SM.OFFICECODERECEIVEDFROM,NULL,NULL) AS OFFICECODERECEIVEDFROMNAME,
                //       SM.STOCKSTATUSCODE, COM_CODENAME(1,'STR_STOCKSTATUSTYPES',SM.STOCKSTATUSCODE,NULL,NULL) AS STOCKSTATUSCODENAME, SM.STOCKTYPECODE, COM_CODENAME(1,'STR_STOCKSTATUSTYPES',SM.STOCKTYPECODE,NULL,NULL) AS STOCKSTATUSTYPECODENAME,
                //       SM.REFERENCENO, SM.DETAILS, SM.EXTRAFIELDS01,
                //       SM.EXTRAFIELDS02, SM.EXTRAFIELDS03, SM.EXTRAFIELDS04, SM.EXTRAFIELDS05, SM.EXTRAFIELDS06, SM.EXTRAFIELDS07,
                //       SM.EXTRAFIELDS08, SM.EXTRAFIELDS09, SM.EXTRAFIELDS10, SM.MAPCODE, SM.REFERENCETABLECODE, SM.REFERENCEDOCUMENTID, SM.REFERENCEDOCUMENTNO
                //FROM STR_STOCKIN01MASTER SM
                //INNER JOIN STR_STOCKOUT01MASTER SOM
                //    ON (SM.OFFICECODE  = SOM.OFFICECODESENTTO AND SM.REFERENCEDOCUMENTID = SOM.ID)
                //WHERE SOM.COMPANYCODE  = {0} AND SOM.OFFICECODESENTTO = {1} AND SM.OFFICECODE = {1} AND SOM.DOCUMENTNO  ='{2}' AND SOM.BLOCKEDYN  = 'N' AND SM.BLOCKEDYN  = 'N'", false, true, param));
                //                    return new
                //                    {
                //                        stock_out = (ConvertStockOutMasterInfoDataRowToModel(DB.ExecuteDataSet(_Command).Tables[0].AsEnumerable())),
                //                        stock_in = (ConvertStockInMasterInfoDataRowToModel(DB.ExecuteDataSet(_CommandStockIn).Tables[0].AsEnumerable())),
                //                    };
                //                }
                return "";
            }
            catch (Exception exception)
            {

                return null;
            }
        }

        private object ConvertStockOutMasterInfoDataRowToModel(EnumerableRowCollection<DataRow> collection)
        {
            try
            {
                var stockOutDataSet = collection.Select(
                            row => new StockOutInfo
                            {
                                Id = row.Field<object>("ID"),
                                CompanyCode = row.Field<object>("COMPANYCODE"),
                                CompanyName = row.Field<object>("COMPANYNAME"),
                                document_no = row.Field<object>("DOCUMENTNO"),
                                SalesOrganizationCode = row.Field<object>("SALESORGCODE"),
                                SalesOrganizationName = row.Field<object>("SALESORGCODENAME"),
                                OfficeCode = row.Field<object>("OFFICECODE"),
                                OfficeCodeName = row.Field<object>("OFFICECODENAME"),
                                WorkDate = row.Field<object>("WORKDATE"),
                                TrasnferType = row.Field<object>("TRANSFERTYPE"),
                                TransferTypeName = row.Field<object>("TRANSFERTYPENAME"),
                                WarehouseCode = row.Field<object>("WAREHOUSECODE"),
                                WarehosueName = row.Field<object>("WAREHOUSENAME"),
                                userCodeSentBy = row.Field<object>("USERCODESENTBY"),
                                UserCodeSentByName = row.Field<object>("USERCODESENTBYNAME"),
                                OfficeCodeSentTo = row.Field<object>("OFFICECODESENTTO"),
                                OfficeCodeSentToName = row.Field<object>("OFFICESENTTONAME"),
                                StockStatusCode = row.Field<object>("STOCKSTATUSCODE"),
                                StockOutTypeCode = row.Field<object>("STOCKOUTTYPECODE"),
                                StockStatusTypeCodeName = row.Field<object>("STOCKSTATUSTYPECODENAME"),
                                ReferenceNo = row.Field<object>("REFERENCENO"),
                                Details = row.Field<object>("DETAILS"),
                                ExtraField01 = row.Field<object>("EXTRAFIELDS01"),
                                ExtraField02 = row.Field<object>("EXTRAFIELDS02"),
                                ExtraField03 = row.Field<object>("EXTRAFIELDS03"),
                                ExtraField04 = row.Field<object>("EXTRAFIELDS04"),
                                ExtraField05 = row.Field<object>("EXTRAFIELDS05"),
                                ExtraField06 = row.Field<object>("EXTRAFIELDS06"),
                                ExtraField07 = row.Field<object>("EXTRAFIELDS07"),
                                ExtraField08 = row.Field<object>("EXTRAFIELDS08"),
                                ExtraField09 = row.Field<object>("EXTRAFIELDS09"),
                                ExtraField10 = row.Field<object>("EXTRAFIELDS10"),
                                MapCode = row.Field<object>("MAPCODE"),
                                RefTableCode = row.Field<object>("REFERENCETABLECODE"),
                                RefTableDocumentId = row.Field<object>("REFERENCEDOCUMENTID"),
                                Allowed = TROutCheckTROUTApproveAndQty(row.Field<object>("ID").ToString(), row.Field<object>("COMPANYCODE").ToString(), row.Field<object>("WAREHOUSECODE").ToString()).Split(';')[0],//Products = GetStockInProductInfoByStockInId(GetParamsForStockinProduct(row))
                                Message = TROutCheckTROUTApproveAndQty(row.Field<object>("ID").ToString(), row.Field<object>("COMPANYCODE").ToString(), row.Field<object>("WAREHOUSECODE").ToString()).Split(';')[1],// Products = null
                            }).FirstOrDefault();
                using (DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO())
                {

                    //foreach (var stockOutRow in stockOutDataSet)
                    //{
                    
                    if (stockOutDataSet!=null)
                        stockOutDataSet.Products = (ConvertStockOutProductInfoDataRowToModel(itemContext.GetStockOutByTrasnferOutDetail(stockOutDataSet.Id.ToString()).AsEnumerable()));
                    //}
                }

                return stockOutDataSet;
            }
            catch (Exception EX)
            {
                throw;
            }
        }
        private object ConvertStockInMasterInfoDataRowToModel(EnumerableRowCollection<DataRow> collection)
        {
            try
            {
                return collection.Select(
                            row => new
                            {
                                Id = row.Field<object>("ID"),
                                CompanyCode = row.Field<object>("COMPANYCODE"),
                                CompanyName = row.Field<object>("COMPANYNAME"),
                                document_no = row.Field<object>("DOCUMENTNO"),
                                SalesOrganizationCode = row.Field<object>("SALESORGCODE"),
                                SalesOrganizationName = row.Field<object>("SALESORGCODENAME"),
                                OfficeCode = row.Field<object>("OFFICECODE"),
                                OfficeCodeName = row.Field<object>("OFFICECODENAME"),
                                WorkDate = row.Field<object>("WORKDATE"),
                                TrasnferType = row.Field<object>("TRANSFERTYPE"),
                                TransferTypeName = row.Field<object>("TRANSFERTYPENAME"),
                                WarehouseCode = row.Field<object>("WAREHOUSECODE"),
                                WarehouseName = row.Field<object>("WAREHOUSENAME"),
                                userCodeReceivedBy = row.Field<object>("USERCODERECEIVEDBY"),
                                UserCodeReceivedByName = row.Field<object>("USERCODERECEIVEDBYNAME"),

                                StockStatusCodeName = row.Field<object>("STOCKSTATUSCODENAME"),
                                StockStatusCode = row.Field<object>("STOCKSTATUSCODE"),
                                StockTypeCode = row.Field<object>("STOCKTYPECODE"),
                                StockTypeCodeName = row.Field<object>("STOCKSTATUSTYPECODENAME"),
                                ReferenceNo = row.Field<object>("REFERENCENO"),
                                Details = row.Field<object>("DETAILS"),
                                ExtraField01 = row.Field<object>("EXTRAFIELDS01"),
                                ExtraField02 = row.Field<object>("EXTRAFIELDS02"),
                                ExtraField03 = row.Field<object>("EXTRAFIELDS03"),
                                ExtraField04 = row.Field<object>("EXTRAFIELDS04"),
                                ExtraField05 = row.Field<object>("EXTRAFIELDS05"),
                                ExtraField06 = row.Field<object>("EXTRAFIELDS06"),
                                ExtraField07 = row.Field<object>("EXTRAFIELDS07"),
                                ExtraField08 = row.Field<object>("EXTRAFIELDS08"),
                                ExtraField09 = row.Field<object>("EXTRAFIELDS09"),
                                ExtraField10 = row.Field<object>("EXTRAFIELDS10"),
                                MapCode = row.Field<object>("MAPCODE"),
                                RefTableCode = row.Field<object>("REFERENCETABLECODE"),
                                RefTableDocumentId = row.Field<object>("REFERENCEDOCUMENTID"),
                                RefTableDocumentNo = row.Field<object>("REFERENCEDOCUMENTNO"),
                            }).FirstOrDefault();
            }
            catch (Exception EX)
            {
                throw;
            }
        }
        private string CheckTROUTApprov(string outno, string compc, string brnch)
        {

            DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

            var data = itemContext.GETTROUTAPPROV(outno, compc, brnch);
            if (data != null)
            {

                return "Y;Tranfer Out is approved.";


            }
            return "N;Tranfer Out is not approved.";


        }
        private string CheckIndApprov(string indno, string compc, string brnch)
        {

            DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

            var data = itemContext.GETINDAPPROV(indno, compc, brnch);
            if (data != null)
            {

                return "Y;Indent  is approved.";


            }
            return "N;Indent is not approved.";


        }
        private string TROutCheckTROUTApproveAndQty(string outno, string compc, string brnch)
        {
            string troutApproved = CheckTROUTApprov(outno, compc, brnch);
            string troutQty = CheckTRINAndTROut(outno, compc, brnch);
            if (troutApproved.Split(';')[0].ToString() == "Y")
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

                // var data = itemContext.GETPOREMAIN(pono, compc, brnch);
                if (troutQty.Split(';')[0].ToString() == "Y")
                {
                    return troutQty;

                }
                return troutQty;
            }
            else { return troutApproved; }

        }

        private string IndCheckIndpproveAndQty(string indno, string compc, string brnch)
        {
            string troutApproved = CheckIndApprov(indno, compc, brnch);
            string troutQty = CheckOUTAndInd(indno, compc, brnch);
            if (troutApproved.Split(';')[0].ToString() == "Y")
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

                // var data = itemContext.GETPOREMAIN(pono, compc, brnch);
                if (troutQty.Split(';')[0].ToString() == "Y")
                {
                    return troutQty;

                }
                return troutQty;
            }
            else { return troutApproved; }

        }
        private string CheckTRINAndTROut(string outno, string compc, string brnch)
        {

            DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

            var data = itemContext.GETCHECKTRINANDTROUT(outno, compc, brnch);
            if (data != null)
            {
                if (Convert.ToInt32(data.ToString()) <= 0)
                {
                    //|| apprv != "Y"
                    return "N;Transfer Out Quantity cannot be exceed from Transfer In Qauntity.";
                }
                else { return "Y;ALLOWED"; }



            }
            return "N;Transfer Out Quantity cannot be exceed from Transfer In Qauntity.";


        }
        private string CheckOUTAndInd(string indno, string compc, string brnch)
        {

            DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();

            var data = itemContext.GETCHECKOUTANDINDT(indno, compc, brnch);
            if (data != null)
            {
                if (Convert.ToInt32(data.ToString()) <= 0)
                {
                    //|| apprv != "Y"
                    return "N;Transfer Out Quantity cannot be exceed from Transfer In Qauntity.";
                }
                else { return "Y;ALLOWED"; }



            }
            return "N;Transfer Out Quantity cannot be exceed from Transfer In Qauntity.";


        }
        private object[] GetParamsForStockoutProduct(DataRow row)
        {
            return new object[] { row.Field<object>("COMPANYCODE"), row.Field<object>("OFFICECODESENTTO"), row.Field<object>("ID") };
        }
        private object[] GetParamsForStockinProduct(DataRow row)
        {
            return new object[] { row.Field<object>("COMPANYCODE"), row.Field<object>("OFFICECODE"), row.Field<object>("ID") };
        }

        public object GetStockOutProductInfoByStockOutId(object[] param)
        {
            try
            {

                return "";

            }
            catch (Exception exception)
            {
                return null;
            }
        }
        public object GetStockInProductInfoByStockInId(object[] param)
        {
            try
            {

                return "";

            }
            catch (Exception exception)
            {

                return null;
            }
        }
        // BARCODE,ITEMS PID, IND_Q QTY
        private object ConvertIndentProductInfoDataRowToModel(EnumerableRowCollection<DataRow> enumerableRowCollection)
        {
            try
            {
                return enumerableRowCollection.Select(
                            row => new
                            {
                                //BARCODE,
                                //ITEMS PID,
                                //IND_Q QTY
                                BARCODE = row.Field<object>("BARCODE"),
                                PID = row.Field<object>("PID"),
                                IND_Q = row.Field<object>("QTY"),
                                NAME = row.Field<object>("NAME")
                            }).ToList();
            }
            catch (Exception exception)
            {

                throw;
            }
        }
        private object ConvertStockOutProductInfoDataRowToModel(EnumerableRowCollection<DataRow> enumerableRowCollection)
        {
            try
            {
                return enumerableRowCollection.Select(
                            row => new
                            {
                                SOPId = row.Field<object>("DETAIL_ID"),
                                SOPProductCode = row.Field<object>("DETAIL_PRODUCTCODE"),
                                SOPProductBarcode = row.Field<object>("DETAIL_PRODUCTBARCODE"),
                                SOPPriceCode = row.Field<object>("DETAIL_PRICECODE"),
                                SOPCostPrice = row.Field<object>("DETAIL_COSTPRICE"),
                                SOPSalePrice = row.Field<object>("DETAIL_SALESPRICE"),
                                SOPRetailPrice = row.Field<object>("DETAIL_RETAILPRICE"),
                                SOPOutQty = row.Field<object>("DETAIL_OUTQTY"),

                                PId = row.Field<object>("PRODUCT_ID"),
                                PName = row.Field<object>("PRODUCT_NAME"),
                                PItemCode = row.Field<object>("PRODUCT_ITEMCODE"),
                                PProductBarcode = row.Field<object>("PRODUCT_PRODUCTBARCODE"),

                                PIsThisImportedItem = row.Field<object>("ISTHISIMPORTEDITEM"),
                                POpeningDate = row.Field<object>("OPENINGDATE"),
                                PClosingDate = row.Field<object>("CLOSINGDATE"),
                                PProductClassificationCode = row.Field<object>("PRODUCTCLASSIFICATIONCODE"),
                                PProductClassificationCodeName = row.Field<object>("PRODUCTCLASSFICATIONCODENAME"),
                                PHierarchyCode = row.Field<object>("PRODUCTHIERARCHYCODE"),
                                PHierarchyCodeName = row.Field<object>("PRODUCTHIERARCHYNAME"),
                                PBrandCode = row.Field<object>("BRANDCODE"),
                                PBrandCodeName = row.Field<object>("PRODUCTBRANDNAME"),
                                PDepartmentCode = row.Field<object>("DEPARTMENTCODE"),
                                PDepartmentCodeName = row.Field<object>("DEPARTMENTCODENAME"),
                                PColorCode = row.Field<object>("COLORCODE"),
                                PColorCodeName = row.Field<object>("COLORCODENAME"),

                                PSizeCode = row.Field<object>("SIZECODE"),
                                PSizeCodeName = row.Field<object>("SIZECODENAME"),
                                PABCIndicationCode = row.Field<object>("PRODUCTABCINDICATIONCODE"),
                                PABCIndicationCodeName = row.Field<object>("PRODUCTABCINDICATIONCODENAME"),
                                PGroupCode = row.Field<object>("PRODUCTGROUPCODE"),
                                PGroupCodeName = row.Field<object>("PRODUCTGROUPCODENAME"),
                                PPrincipleCode = row.Field<object>("PRINCIPLECODE"),
                                PPrincipleCodeName = row.Field<object>("PRINCIPLECODENAME"),

                                PDivisionCode = row.Field<object>("DIVISIONCODE"),
                                PDivisionCodeName = row.Field<object>("DIVISIONCODENAME"),
                                PVariantCode = row.Field<object>("VARIANTCODE"),
                                PVariantCodeName = row.Field<object>("VARIANTCODENAME"),
                                PMarketingTeamCode = row.Field<object>("MARKETINGTEAMCODE"),
                                PMarketingTeamCodeName = row.Field<object>("MARKETINGCODENAME"),

                                PShelfLifeMax = row.Field<object>("SHELFLIFEMAXIMUM"),
                                PShelfLifeMin = row.Field<object>("SHELFLIFEMINIMUM"),
                                PShelfLifeRemainingForSale = row.Field<object>("SHELFLIFEREMAININGFORSALE"),
                                PShelfLifeRemainingForProductIO = row.Field<object>("SHELFLIFEREMAININGFORPRODUCTIO"),
                                PSortingOrder = row.Field<object>("SORTINGORDER"),
                                PIsThisServiceItem = row.Field<object>("ISTHISSERVICEITEM"),

                                PIsThisMasterProduct = row.Field<object>("ISTHISMASTERPRODUCT"),
                                PIsThisItemSaleable = row.Field<object>("ISTHISITEM_SALEABLE"),
                                PIsThisItemPurchaseable = row.Field<object>("ISTHISITEM_PURCHASEABLE"),
                                PIsThisFixedAssetItem = row.Field<object>("ISTHISFIXEDASSETITEM"),
                                PMaintainInventory = row.Field<object>("MAINTAININVENTORY"),
                                PDoYouMakeThisItem = row.Field<object>("DOYOUMAKETHISITEM"),
                                PMaintainBatchNos = row.Field<object>("MAINTAINBATCHNOS"),
                                PMaintainSerialNo = row.Field<object>("MAINTAINSERIALNOS"),
                                PIsPriceAlreadyPrinted = row.Field<object>("ISPRICEALREADYPRINTED"),
                                PSeperatePriceForChildBarcode = row.Field<object>("SEPERATEPRICEFORCHILDBARCODE"),
                                PIsThisFreeFlowProduct = row.Field<object>("ISTHISFREEFLOWPRODUCT"),

                                PAutoPositionInspection = row.Field<object>("AUTOPOSTTOINSPECTION"),
                                PPiecesInCarton = row.Field<object>("PIECESINACARTON"),
                                PPiecesInPack = row.Field<object>("PIECESINAPACK"),
                                PPerPieceGramOrMililitre = row.Field<object>("PERPIECEGRAMAGEORMILILITRE"),
                                PRegistrationNo = row.Field<object>("REGISTRATIONNO"),
                                PGrossWeight = row.Field<object>("GROSSWEIGHT"),
                                PUOMCodeGrossWeight = row.Field<object>("UOMCODEGROSSWEIGHT"),
                                PNetWeight = row.Field<object>("NETWEIGHT"),
                                PVolume = row.Field<object>("VOLUME"),
                                PUOMCodeVolume = row.Field<object>("UOMCODEVOLUME"),
                                PProductHeight = row.Field<object>("PRODUCTHEIGHT"),
                                PProductDepth = row.Field<object>("PRODUCTDEPTH"),
                                PParentKey = row.Field<object>("PARENTKEY"),
                                PPrintPricedOnBarcode = row.Field<object>("PRINTPRICEONBARCODE"),
                                PPrintShortnameOnSlip = row.Field<object>("PRINTSHORTNAMEONSLIP"),
                                PMaxOrderQty = row.Field<object>("MAXIMUMORDERQUANTITY"),
                                PMinOrderQty = row.Field<object>("MINIMUMORDERQUANTITY"),
                                PReOrderLevelQty = row.Field<object>("REORDERLEVELQUANTITY"),
                                PMaxQty = row.Field<object>("MAXIMUMQUANTITY"),
                                PMinQty = row.Field<object>("MINIMUMQUANTITY"),
                                PProcurementGroupCode = row.Field<object>("PROCUREMENTGROUPCODE"),
                                PPOTolerence = row.Field<object>("POTOLERENCE"),
                                PLeadTimeTransfer = row.Field<object>("LEADTIME_TRANSFER"),
                                PLeadTimePurchase = row.Field<object>("LEADTIME_PURCHASES"),
                                PLeadTimeSales = row.Field<object>("LEADTIME_SALES"),
                                PLeadTimeProduction = row.Field<object>("LEADTIME_PRODUCTION"),
                                PArticleNo = row.Field<object>("ARTICLE_PARTNO"),
                                PCatelogDesignNo = row.Field<object>("CATALOG_DESIGNNO"),
                                PUnitOfMeasureCodeInventory = row.Field<object>("UNITOFMEASURECODE_INVENTORY"),
                                PUnitOfMeasureCodePurchase = row.Field<object>("UNITOFMEASURECODE_PURCHASES"),
                                PUnitOfMeasureCodeSales = row.Field<object>("UNITOFMEASURECODE_SALES")


                            }).ToList();
            }
            catch (Exception exception)
            {

                throw;
            }
        }
        private object ConvertStockInProductInfoDataRowToModel(EnumerableRowCollection<DataRow> enumerableRowCollection)
        {
            try
            {
                return enumerableRowCollection.Select(
                            row => new
                            {
                                SOPId = row.Field<object>("DETAIL_ID"),
                                SOPProductCode = row.Field<object>("DETAIL_PRODUCTCODE"),
                                SOPProductBarcode = row.Field<object>("DETAIL_PRODUCTBARCODE"),
                                SOPPriceCode = row.Field<object>("DETAIL_PRICECODE"),
                                SOPCostPrice = row.Field<object>("DETAIL_COSTPRICE"),
                                SOPSalePrice = row.Field<object>("DETAIL_SALEPRICE"),
                                SOPRetailPrice = row.Field<object>("DETAIL_RETAILPRICE"),
                                SOPInQty = row.Field<object>("DETAIL_INQTY"),
                                SOPInValue = row.Field<object>("DETAIL_INVALUE"),
                                SOPUOMCode = row.Field<object>("DETAIL_UOMCODE"),
                                SOPLotOrBatchNo = row.Field<object>("DETAIL_LOTORBATCHNO"),
                                SOPProductSerialNo = row.Field<object>("DETAIL_PRODUCTSERIALNO"),
                                SOPInQty1 = row.Field<object>("DETAIL_INQTY1"),
                                SOPInQty2 = row.Field<object>("DETAIL_INQTY2"),
                                SOPInQty3 = row.Field<object>("DETAIL_INQTY3"),
                                SOPRefTableCode = row.Field<object>("DETAIL_REFTABLECODE"),
                                SOPRefDocumentId = row.Field<object>("DETAIL_REFDOCUMENTID"),
                                SOPDetails = row.Field<object>("DETAIL_DETAILS"),
                                SOPExtraField01 = row.Field<object>("DETAIL_EXTRAFILED01"),
                                SOPExtraField02 = row.Field<object>("DETAIL_EXTRAFIELD02"),
                                SOPExtraField03 = row.Field<object>("DETAIL_EXTRAFIELD03"),
                                SOPExtraField04 = row.Field<object>("DETAIL_EXTRAFIELD04"),
                                SOPExtraField05 = row.Field<object>("DETAIL_EXTRAFIELD05"),
                                SOPExtraField06 = row.Field<object>("DETAIL_EXTRAFIELD06"),
                                SOPExtraField07 = row.Field<object>("DETAIL_EXTRAFIELD07"),
                                SOPExtraField08 = row.Field<object>("DETAIL_EXTRAFIELD08"),
                                SOPExtraField09 = row.Field<object>("DETAIL_EXTRAFIELD09"),
                                SOPExtraField10 = row.Field<object>("DETAIL_EXTRAFILED10"),


                                PId = row.Field<object>("PRODUCT_ID"),
                                PName = row.Field<object>("PRODUCT_NAME"),
                                PItemCode = row.Field<object>("PRODUCT_ITEMCODE"),
                                PProductBarcode = row.Field<object>("PRODUCT_PRODUCTBARCODE"),

                                PIsThisImportedItem = row.Field<object>("ISTHISIMPORTEDITEM"),
                                POpeningDate = row.Field<object>("OPENINGDATE"),
                                PClosingDate = row.Field<object>("CLOSINGDATE"),
                                PProductClassificationCode = row.Field<object>("PRODUCTCLASSIFICATIONCODE"),
                                PProductClassificationCodeName = row.Field<object>("PRODUCTCLASSFICATIONCODENAME"),
                                PHierarchyCode = row.Field<object>("PRODUCTHIERARCHYCODE"),
                                PHierarchyCodeName = row.Field<object>("PRODUCTHIERARCHYNAME"),
                                PBrandCode = row.Field<object>("BRANDCODE"),
                                PBrandCodeName = row.Field<object>("PRODUCTBRANDNAME"),
                                PDepartmentCode = row.Field<object>("DEPARTMENTCODE"),
                                PDepartmentCodeName = row.Field<object>("DEPARTMENTCODENAME"),
                                PColorCode = row.Field<object>("COLORCODE"),
                                PColorCodeName = row.Field<object>("COLORCODENAME"),

                                PSizeCode = row.Field<object>("SIZECODE"),
                                PSizeCodeName = row.Field<object>("SIZECODENAME"),
                                PABCIndicationCode = row.Field<object>("PRODUCTABCINDICATIONCODE"),
                                PABCIndicationCodeName = row.Field<object>("PRODUCTABCINDICATIONCODENAME"),
                                PGroupCode = row.Field<object>("PRODUCTGROUPCODE"),
                                PGroupCodeName = row.Field<object>("PRODUCTGROUPCODENAME"),
                                PPrincipleCode = row.Field<object>("PRINCIPLECODE"),
                                PPrincipleCodeName = row.Field<object>("PRINCIPLECODENAME"),

                                PDivisionCode = row.Field<object>("DIVISIONCODE"),
                                PDivisionCodeName = row.Field<object>("DIVISIONCODENAME"),
                                PVariantCode = row.Field<object>("VARIANTCODE"),
                                PVariantCodeName = row.Field<object>("VARIANTCODENAME"),
                                PMarketingTeamCode = row.Field<object>("MARKETINGTEAMCODE"),
                                PMarketingTeamCodeName = row.Field<object>("MARKETINGCODENAME"),

                                PShelfLifeMax = row.Field<object>("SHELFLIFEMAXIMUM"),
                                PShelfLifeMin = row.Field<object>("SHELFLIFEMINIMUM"),
                                PShelfLifeRemainingForSale = row.Field<object>("SHELFLIFEREMAININGFORSALE"),
                                PShelfLifeRemainingForProductIO = row.Field<object>("SHELFLIFEREMAININGFORPRODUCTIO"),
                                PSortingOrder = row.Field<object>("SORTINGORDER"),
                                PIsThisServiceItem = row.Field<object>("ISTHISSERVICEITEM"),

                                PIsThisMasterProduct = row.Field<object>("ISTHISMASTERPRODUCT"),
                                PIsThisItemSaleable = row.Field<object>("ISTHISITEM_SALEABLE"),
                                PIsThisItemPurchaseable = row.Field<object>("ISTHISITEM_PURCHASEABLE"),
                                PIsThisFixedAssetItem = row.Field<object>("ISTHISFIXEDASSETITEM"),
                                PMaintainInventory = row.Field<object>("MAINTAININVENTORY"),
                                PDoYouMakeThisItem = row.Field<object>("DOYOUMAKETHISITEM"),
                                PMaintainBatchNos = row.Field<object>("MAINTAINBATCHNOS"),
                                PMaintainSerialNo = row.Field<object>("MAINTAINSERIALNOS"),
                                PIsPriceAlreadyPrinted = row.Field<object>("ISPRICEALREADYPRINTED"),
                                PSeperatePriceForChildBarcode = row.Field<object>("SEPERATEPRICEFORCHILDBARCODE"),
                                PIsThisFreeFlowProduct = row.Field<object>("ISTHISFREEFLOWPRODUCT"),

                                PAutoPositionInspection = row.Field<object>("AUTOPOSTTOINSPECTION"),
                                PPiecesInCarton = row.Field<object>("PIECESINACARTON"),
                                PPiecesInPack = row.Field<object>("PIECESINAPACK"),
                                PPerPieceGramOrMililitre = row.Field<object>("PERPIECEGRAMAGEORMILILITRE"),
                                PRegistrationNo = row.Field<object>("REGISTRATIONNO"),
                                PGrossWeight = row.Field<object>("GROSSWEIGHT"),
                                PUOMCodeGrossWeight = row.Field<object>("UOMCODEGROSSWEIGHT"),
                                PNetWeight = row.Field<object>("NETWEIGHT"),
                                PVolume = row.Field<object>("VOLUME"),
                                PUOMCodeVolume = row.Field<object>("UOMCODEVOLUME"),
                                PProductHeight = row.Field<object>("PRODUCTHEIGHT"),
                                PProductDepth = row.Field<object>("PRODUCTDEPTH"),
                                PParentKey = row.Field<object>("PARENTKEY"),
                                PPrintPricedOnBarcode = row.Field<object>("PRINTPRICEONBARCODE"),
                                PPrintShortnameOnSlip = row.Field<object>("PRINTSHORTNAMEONSLIP"),
                                PMaxOrderQty = row.Field<object>("MAXIMUMORDERQUANTITY"),
                                PMinOrderQty = row.Field<object>("MINIMUMORDERQUANTITY"),
                                PReOrderLevelQty = row.Field<object>("REORDERLEVELQUANTITY"),
                                PMaxQty = row.Field<object>("MAXIMUMQUANTITY"),
                                PMinQty = row.Field<object>("MINIMUMQUANTITY"),
                                PProcurementGroupCode = row.Field<object>("PROCUREMENTGROUPCODE"),
                                PPOTolerence = row.Field<object>("POTOLERENCE"),
                                PLeadTimeTransfer = row.Field<object>("LEADTIME_TRANSFER"),
                                PLeadTimePurchase = row.Field<object>("LEADTIME_PURCHASES"),
                                PLeadTimeSales = row.Field<object>("LEADTIME_SALES"),
                                PLeadTimeProduction = row.Field<object>("LEADTIME_PRODUCTION"),
                                PArticleNo = row.Field<object>("ARTICLE_PARTNO"),
                                PCatelogDesignNo = row.Field<object>("CATALOG_DESIGNNO"),
                                PUnitOfMeasureCodeInventory = row.Field<object>("UNITOFMEASURECODE_INVENTORY"),
                                PUnitOfMeasureCodePurchase = row.Field<object>("UNITOFMEASURECODE_PURCHASES"),
                                PUnitOfMeasureCodeSales = row.Field<object>("UNITOFMEASURECODE_SALES")


                            }).ToList();
            }
            catch (Exception exception)
            {

                throw;
            }
        }

        #region SuppliersLOV

        public object GetSuppliersDML()
        {
            try
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();
                return (ConvertGetSuppliersDataRowToModel(itemContext.GetSupplierDetail().AsEnumerable()));
            }
            catch (Exception exception)
            {
                //  Logger.PrintError(exception);
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace);
                throw exception;
            }
        }

        private List<SuppliersInfo> ConvertGetSuppliersDataRowToModel(EnumerableRowCollection<DataRow> enumerableRowCollection)
        {
            try
            {
                return enumerableRowCollection.Select(
                            row => new SuppliersInfo
                            {
                                Id = row.Field<object>("ID"),
                                Name = row.Field<object>("NAME")


                            }).ToList();
            }
            catch (Exception exception)
            {

                throw;
            }
        }

        #endregion

        #region WarehousesLOV

        public object GetWarehouseDML()
        {
            try
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();
                return (ConvertGetWarehousesDataRowToModel(itemContext.GetWarehouseDetail().AsEnumerable()));
            }
            catch (Exception exception)
            {
                //  Logger.PrintError(exception);
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace);
                throw exception;
            }
        }

        private List<WarehousesInfo> ConvertGetWarehousesDataRowToModel(EnumerableRowCollection<DataRow> enumerableRowCollection)
        {
            try
            {
                return enumerableRowCollection.Select(
                            row => new WarehousesInfo
                            {
                                Id = row.Field<object>("ID"),
                                Name = row.Field<object>("NAME")


                            }).ToList();
            }
            catch (Exception exception)
            {

                throw;
            }
        }

        #endregion

        #region StockTypeLOV

        public object GetStockTypeDML()
        {
            try
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();
                return (ConvertGetStockTypesDataRowToModel(itemContext.GetStockTypeDetail().AsEnumerable()));
            }
            catch (Exception exception)
            {
                //  Logger.PrintError(exception);
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace);
                throw exception;
            }
        }

        private List<StockTypeInfo> ConvertGetStockTypesDataRowToModel(EnumerableRowCollection<DataRow> enumerableRowCollection)
        {
            try
            {
                return enumerableRowCollection.Select(
                            row => new StockTypeInfo
                            {
                                Id = row.Field<object>("ID"),
                                Name = row.Field<object>("NAME"),
                                Saleableyn = row.Field<object>("SALABLE"),
                                Activeyn = row.Field<object>("ACTIVE")

                            }).ToList();

            }
            catch (Exception exception)
            {

                throw;
            }
        }

        #endregion

        #region BayLOV

        public object GetBayDML()
        {
            try
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();
                return (ConvertGetBayDataRowToModel(itemContext.GetBayDetail().AsEnumerable()));
            }
            catch (Exception exception)
            {
                //  Logger.PrintError(exception);
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace);
                throw exception;
            }
        }

        private List<Bay> ConvertGetBayDataRowToModel(EnumerableRowCollection<DataRow> enumerableRowCollection)
        {
            try
            {
                return enumerableRowCollection.Select(
                            row => new Bay
                            {
                                Id = row.Field<object>("ID"),
                                Name = row.Field<object>("NAME")


                            }).ToList();
            }
            catch (Exception exception)
            {

                throw;
            }
        }

        #endregion

        #region AislesLOV

        public object GetAislesDML()
        {
            try
            {
                DAL.DataAccess.ItemDAO itemContext = new DataAccess.ItemDAO();
                return (ConvertGetAislesDataRowToModel(itemContext.GetAISLESDetail().AsEnumerable()));
            }
            catch (Exception exception)
            {
                //  Logger.PrintError(exception);
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace);
                throw exception;
            }
        }

        private List<Aisles> ConvertGetAislesDataRowToModel(EnumerableRowCollection<DataRow> enumerableRowCollection)
        {
            try
            {
                return enumerableRowCollection.Select(
                            row => new Aisles
                            {
                                Id = row.Field<object>("ID"),
                                Name = row.Field<object>("NAME")


                            }).ToList();
            }
            catch (Exception exception)
            {

                throw;
            }
        }

        #endregion

        #region Dispose
        public void Dispose()
        {

        }
        #endregion
    }
    
         public class IndInfo
    {
        public object Products { get; set; }

        public object ID { get; set; }
        public object APPROVALYN { get; set; }
        public object BLOCKEDYN { get; set; }
        public object DOCUMENTNO { get; set; }
        public object WORKDATE { get; set; }
        public object USERCODEREQUESTEDBY { get; set; }
        public object USERCODEREQUESTBYNAME { get; set; }
        public object POROUNDAMOUNT { get; set; }
        public object QTY { get; set; }
        public object STOREFROM { get; set; }

        public object STORETO { get; set; }
        public object BRANCH { get; set; }
        public object COMPANY { get; set; }
       
        public object Allowed { get; set; }
        public object Message { get; set; }

    }
    public class POInfo
    {
        public object Products { get; set; }

        public object POId { get; set; }
        public object POPostedYn { get; set; }
        public object POApprovalYn { get; set; }
        public object POBlockedYn { get; set; }
        public object POOfficeCodeShipTo { get; set; }
        public object POOfficeCodeShipToName { get; set; }
        public object POOfficeCodeBillTo { get; set; }
        public object POOfficeCodeBillToName { get; set; }
        public object PODocumentNo { get; set; }
        public object POWorkDate { get; set; }

        public object POUsercodeRequestBy { get; set; }
        public object POUsercodeRequestByName { get; set; }
        public object POBusinessPartnerCode { get; set; }
        public object POBusinessPartnerCodeName { get; set; }
        public object POCurrencyCode { get; set; }
        public object POCurrencyCodeName { get; set; }
        public object POCurrencyRate { get; set; }
        public object POIsLocalOrImported { get; set; }
        public object POCountryCode { get; set; }
        public object POCountryCodeName { get; set; }

        public object POPaymentTermCode { get; set; }
        public object POPaymentTermCodeName { get; set; }

        public object POTolenrancePer { get; set; }
        public object PORevisionNo { get; set; }

        public object PODeliveryDate { get; set; }
        public object POValidUpTo { get; set; }
        public object PONumberOfItems { get; set; }
        public object POValue { get; set; }
        public object PODiscountPercentage { get; set; }
        public object PODiscountAmount { get; set; }
        public object PORoundAmount { get; set; }
        public object PONetValue { get; set; }

        public object POOrigionOfShipment { get; set; }
        public object POReferenceNo { get; set; }
        public object PODetails { get; set; }
        public object POExtraField01 { get; set; }
        public object POExtraField02 { get; set; }
        public object POExtraField03 { get; set; }
        public object POExtraField04 { get; set; }
        public object POExtraField05 { get; set; }
        public object POExtraField06 { get; set; }
        public object POExtraField07 { get; set; }
        public object POExtraField08 { get; set; }
        public object POExtraField09 { get; set; }
        public object POExtraField10 { get; set; }
        public object Allowed { get; set; }
        public object Message { get; set; }

    }
    public class POProductInfo
    {
        public object POPId { get; set; }
        public object POPProductCode { get; set; }
        public object POPProductBarcode { get; set; }
        public object POPPriceCode { get; set; }
        public object POPCostPrice { get; set; }
        public object POPSalesPrice { get; set; }
        public object POPRetailPrice { get; set; }
        public object POPOrderQty { get; set; }
        public object POPOrderValue { get; set; }
        public object POPUOMCode { get; set; }
        public object POPUOMCodeName { get; set; }
        public object POPNeedByDate { get; set; }
        public object POPInHandQty { get; set; }
        public object POPReqQty { get; set; }
        public object POPReferenceTableCode { get; set; }
        public object POPReferenceDocumentId { get; set; }
        public object POPPackagingDetailCode { get; set; }
        public object POPPackagingDetails { get; set; }
        public object POPDetails { get; set; }
        public object POPOrderQty1 { get; set; }
        public object POPOrderQty2 { get; set; }
        public object POPOrderQty3 { get; set; }
        public object POPCostPrice1 { get; set; }
        public object POPCostPrice2 { get; set; }
        public object POPCostPrice3 { get; set; }
        public object POPFreeOfCostQty { get; set; }

        public object POPDiscountAmount { get; set; }
        public object POPGrossValueAfterDiscount { get; set; }
        public object POPCalculateSalesTaxFOCQty { get; set; }
        public object POPSalesTaxPer { get; set; }
        public object POPSalesTaxAmount { get; set; }
        public object POPExciseDutyPer { get; set; }
        public object POPExciseDutyAmount { get; set; }
        public object POPExtraDiscountPer { get; set; }
        public object POPExtraDiscountAmount { get; set; }
        public object POPComissionPer { get; set; }
        public object POPComissionAmount { get; set; }
        public object POPCustomDutyPer { get; set; }
        public object POPCustomDutyAmount { get; set; }

        public object PId { get; set; }
        public object PName { get; set; }
        public object PItemCode { get; set; }
        public object PProductBarcode { get; set; }

        public object PIsThisImportedItem { get; set; }
        public object POpeningDate { get; set; }
        public object PClosingDate { get; set; }
        public object PProductClassificationCode { get; set; }
        public object PProductClassificationCodeName { get; set; }
        public object PHierarchyCode { get; set; }
        public object PHierarchyCodeName { get; set; }
        public object PBrandCode { get; set; }
        public object PBrandCodeName { get; set; }
        public object PDepartmentCode { get; set; }
        public object PDepartmentCodeName { get; set; }
        public object PColorCode { get; set; }
        public object PColorCodeName { get; set; }

        public object PSizeCode { get; set; }
        public object PSizeCodeName { get; set; }
        public object PABCIndicationCode { get; set; }
        public object PABCIndicationCodeName { get; set; }
        public object PGroupCode { get; set; }
        public object PGroupCodeName { get; set; }
        public object PPrincipleCode { get; set; }
        public object PPrincipleCodeName { get; set; }

        public object PDivisionCode { get; set; }
        public object PDivisionCodeName { get; set; }
        public object PVariantCode { get; set; }
        public object PVariantCodeName { get; set; }
        public object PMarketingTeamCode { get; set; }
        public object PMarketingTeamCodeName { get; set; }

        public object PShelfLifeMax { get; set; }
        public object PShelfLifeMin { get; set; }
        public object PShelfLifeRemainingForSale { get; set; }
        public object PShelfLifeRemainingForProductIO { get; set; }
        public object PSortingOrder { get; set; }
        public object PIsThisServiceItem { get; set; }

        public object PIsThisMasterProduct { get; set; }
        public object PIsThisItemSaleable { get; set; }
        public object PIsThisItemPurchaseable { get; set; }
        public object PIsThisFixedAssetItem { get; set; }
        public object PMaintainInventory { get; set; }
        public object PDoYouMakeThisItem { get; set; }
        public object PMaintainBatchNos { get; set; }
        public object PMaintainSerialNo { get; set; }
        public object PIsPriceAlreadyPrinted { get; set; }
        public object PSeperatePriceForChildBarcode { get; set; }
        public object PIsThisFreeFlowProduct { get; set; }

        public object PAutoPositionInspection { get; set; }
        public object PPiecesInCarton { get; set; }
        public object PPiecesInPack { get; set; }
        public object PPerPieceGramOrMililitre { get; set; }
        public object PRegistrationNo { get; set; }
        public object PGrossWeight { get; set; }
        public object PUOMCodeGrossWeight { get; set; }
        public object PNetWeight { get; set; }
        public object PVolume { get; set; }
        public object PUOMCodeVolume { get; set; }
        public object PProductHeight { get; set; }
        public object PProductDepth { get; set; }
        public object PParentKey { get; set; }
        public object PPrintPricedOnBarcode { get; set; }
        public object PPrintShortnameOnSlip { get; set; }
        public object PMaxOrderQty { get; set; }
        public object PMinOrderQty { get; set; }
        public object PReOrderLevelQty { get; set; }
        public object PMaxQty { get; set; }
        public object PMinQty { get; set; }
        public object PProcurementGroupCode { get; set; }
        public object PPOTolerence { get; set; }
        public object PLeadTimeTransfer { get; set; }
        public object PLeadTimePurchase { get; set; }
        public object PLeadTimeSales { get; set; }
        public object PLeadTimeProduction { get; set; }
        public object PArticleNo { get; set; }
        public object PCatelogDesignNo { get; set; }
        public object PUnitOfMeasureCodeInventory { get; set; }
        public object PUnitOfMeasureCodePurchase { get; set; }
        public object PUnitOfMeasureCodeSales { get; set; }
    }
    public class GRNProductInfo
    {


        public object PId { get; set; }
        public object PName { get; set; }
        public object PItemCode { get; set; }
        public object PProductBarcode { get; set; }

        public object PIsThisImportedItem { get; set; }
        public object POpeningDate { get; set; }
        public object PClosingDate { get; set; }
        public object PProductClassificationCode { get; set; }
        public object PProductClassificationCodeName { get; set; }
        public object PHierarchyCode { get; set; }
        public object PHierarchyCodeName { get; set; }
        public object PBrandCode { get; set; }
        public object PBrandCodeName { get; set; }
        public object PDepartmentCode { get; set; }
        public object PDepartmentCodeName { get; set; }
        public object PColorCode { get; set; }
        public object PColorCodeName { get; set; }

        public object PSizeCode { get; set; }
        public object PSizeCodeName { get; set; }
        public object PABCIndicationCode { get; set; }
        public object PABCIndicationCodeName { get; set; }
        public object PGroupCode { get; set; }
        public object PGroupCodeName { get; set; }
        public object PPrincipleCode { get; set; }
        public object PPrincipleCodeName { get; set; }

        public object PDivisionCode { get; set; }
        public object PDivisionCodeName { get; set; }
        public object PVariantCode { get; set; }
        public object PVariantCodeName { get; set; }
        public object PMarketingTeamCode { get; set; }
        public object PMarketingTeamCodeName { get; set; }
        public object Supplier { get; set; }
        public object PShelfLifeMax { get; set; }
        public object PShelfLifeMin { get; set; }
        public object PShelfLifeRemainingForSale { get; set; }
        public object PShelfLifeRemainingForProductIO { get; set; }
        public object PSortingOrder { get; set; }
        public object PIsThisServiceItem { get; set; }

        public object PIsThisMasterProduct { get; set; }
        public object PIsThisItemSaleable { get; set; }
        public object PIsThisItemPurchaseable { get; set; }
        public object PIsThisFixedAssetItem { get; set; }
        public object PMaintainInventory { get; set; }
        public object PDoYouMakeThisItem { get; set; }
        public object PMaintainBatchNos { get; set; }
        public object PMaintainSerialNo { get; set; }
        public object PIsPriceAlreadyPrinted { get; set; }
        public object PSeperatePriceForChildBarcode { get; set; }
        public object PIsThisFreeFlowProduct { get; set; }

        public object PAutoPositionInspection { get; set; }
        public object PPiecesInCarton { get; set; }
        public object PPiecesInPack { get; set; }
        public object PPerPieceGramOrMililitre { get; set; }
        public object PRegistrationNo { get; set; }
        public object PGrossWeight { get; set; }
        public object PUOMCodeGrossWeight { get; set; }
        public object PNetWeight { get; set; }
        public object PVolume { get; set; }
        public object PUOMCodeVolume { get; set; }
        public object PProductHeight { get; set; }
        public object PProductDepth { get; set; }
        public object PParentKey { get; set; }
        public object PPrintPricedOnBarcode { get; set; }
        public object PPrintShortnameOnSlip { get; set; }
        public object PMaxOrderQty { get; set; }
        public object PMinOrderQty { get; set; }
        public object PReOrderLevelQty { get; set; }
        public object PMaxQty { get; set; }
        public object PMinQty { get; set; }
        public object PProcurementGroupCode { get; set; }
        public object PPOTolerence { get; set; }
        public object PLeadTimeTransfer { get; set; }
        public object PLeadTimePurchase { get; set; }
        public object PLeadTimeSales { get; set; }
        public object PLeadTimeProduction { get; set; }
        public object PArticleNo { get; set; }
        public object PCatelogDesignNo { get; set; }
        public object PUnitOfMeasureCodeInventory { get; set; }
        public object PUnitOfMeasureCodePurchase { get; set; }
        public object PUnitOfMeasureCodeSales { get; set; }
        public object Allowed { get; set; }
        public object Message { get; set; }

        public object POQty { get; set; }
        public object POQtyRem { get; set; }
    }

    public class StockOutInfo
    {
        public object Products { get; set; }
        public object Id { get; set; }
        public object CompanyCode { get; set; }
        public object CompanyName { get; set; }
        public object document_no { get; set; }
        public object SalesOrganizationCode { get; set; }
        public object SalesOrganizationName { get; set; }
        public object OfficeCode { get; set; }
        public object OfficeCodeName { get; set; }
        public object WorkDate { get; set; }
        public object TrasnferType { get; set; }
        public object TransferTypeName { get; set; }
        public object WarehouseCode { get; set; }
        public object WarehosueName { get; set; }
        public object userCodeSentBy { get; set; }
        public object UserCodeSentByName { get; set; }
        public object OfficeCodeSentTo { get; set; }
        public object OfficeCodeSentToName { get; set; }
        public object StockStatusCode { get; set; }
        public object StockOutTypeCode { get; set; }
        public object StockStatusTypeCodeName { get; set; }
        public object ReferenceNo { get; set; }
        public object Details { get; set; }
        public object ExtraField01 { get; set; }
        public object ExtraField02 { get; set; }
        public object ExtraField03 { get; set; }
        public object ExtraField04 { get; set; }
        public object ExtraField05 { get; set; }
        public object ExtraField06 { get; set; }
        public object ExtraField07 { get; set; }
        public object ExtraField08 { get; set; }
        public object ExtraField09 { get; set; }
        public object ExtraField10 { get; set; }
        public object MapCode { get; set; }
        public object RefTableCode { get; set; }
        public object RefTableDocumentId { get; set; }
        public object Allowed { get; set; }
        public object Message { get; set; }

    }
    public class SuppliersInfo
    {
        public object Suppliers { get; set; }
        public object Id { get; set; }
        public object Name { get; set; }
    }

    public class WarehousesInfo
    {
        public object Suppliers { get; set; }
        public object Id { get; set; }
        public object Name { get; set; }
    }
    public class StockTypeInfo
    {
        public object Suppliers { get; set; }
        public object Id { get; set; }
        public object Name { get; set; }
        public object Saleableyn { get; set; }
        public object Activeyn { get; set; }
    }
    public class Bay
    {
        public object Id { get; set; }
        public object Name { get; set; }
    }

    public class Aisles
    {
        public object Id { get; set; }
        public object Name { get; set; }
    }
}