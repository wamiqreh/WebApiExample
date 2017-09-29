#region Modification History
// Created By:  Mirza Fahad Ali Baig
// Created On:  27th May, 2013
// Description: 
// ****************************** Modifications *********************************
// Mirza Fahad Ali Baig                           Fix Damage Deletion By Type
// Mirza Fahad Ali Baig           2 October,2013  Add Expiry date
// Mirza Fahad Ali Baig           2 November,2013  Display GRN Number on Closing of HHT Receiving.
// ******************************************************************************
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using DAL.Provider;
using Common.Enumeration;
using Common.Utilities;
using System.Globalization;

namespace DAL.DataAccess
{
    public class ItemDAO : IDisposable
    {
        #region Constants
        string SELECT_BY_DAILY_TABLE_PREFIX = "SAL_POS_DD";//SAL_POS_D_10_12_2015 
        private static readonly string SELECT_ITEM_BY_BARCODE = "SELECT  BARCODE Barcode, DESCR ItemName,COLUR ,ITEMS,REO_Q,MAX_Q ,MIN_Q ,'' CLOSINGQUANTITY ,'' UNAPPROVEDQUANTITY ,'' SUPPLIER,'' DAILYMSALE ," +
           " SAL_CODENAME('DPTCD', DPTCD, NULL) AS Department, SAL_CODENAME('ICLAS', ICLAS, NULL) AS MasterCategory, " +
            "  SAL_CODENAME('MCLAS', ICLAS, NULL) AS Category, " +
                " TRADE  RetailPrice ,'' AS BranchName, -999 AS  AvailableQty,'' ONORDERQUANTITY " +
            "FROM SAL_PRODS_BARCODE_LIST WHERE (UPPER(BARCODE) = '{0}'   OR ITEMS = '{1}' ) AND STATS = 'A' ";
        private static readonly string SELECT_ITEM_BY_BARCODENEW = ""
        + "SELECT BARCODE Barcode, " + "\n"
+ "       DESCR   ItemName, " + "\n"
+ "       --SAL_CODENAME('DPTCD', DPTCD, NULL) " + "\n"
 + "        nvl( (SELECT descr From Com_CodeFile WHERE Code# = k.COLUR) ,'N/A') || ','  ||  nvl(com_codename('CODE#', k.ISIZE, '') ,'N/A')  colur, " + "\n"
 + "       ITEMS, " + "\n"
 + "       nvl(REO_Q, 0) REO_Q, " + "\n"
 + "       nvl(MAX_Q, 0) MAX_Q, " + "\n"
 + "       nvl(MIN_Q, 0) MIN_Q, " + "\n"
  + "       nvl(fn_str_item_qty@ho('{2}', " + "\n"
   + "                        '{3}', " + "\n"
   + "                        '{3}', " + "\n"
   + "                        k.ITEMS, " + "\n"
   + "                        k.BARCODE, " + "\n"
   + "                        '{4}', " + "\n"
   + "                        sysdate), " + "\n"
   + "           0) CLOSINGQUANTITY, " + "\n"
  //+ "           0 CLOSINGQUANTITY, " + "\n"
  + "       (select nvl(sum(qty), 0) " + "\n"
  + "          from (select sum(b.rec_q) qty " + "\n"
  + "                  from str_trf_ins1 a, str_trf_ins2 b " + "\n"
  + "                 where a.aprov = 'P' " + "\n"
  + "                   and a.tin_# = b.tin_# " + "\n"
  + "                   and a.brnch = '{3}' " + "\n"
  + "                   and a.compc = '{2}' " + "\n"
  + "                   and b.barcode = '{0}' " + "\n"
  + "                union all " + "\n"
  + "                select sum(d.out_q) " + "\n"
  + "                  from str_trf_out1 a, str_trf_out2 d " + "\n"
  + "                 where a.aprov = 'P' " + "\n"
  + "                   and a.brnch = '{3}' " + "\n"
  + "                   and a.compc = '{2}' " + "\n"
  + "                   and d.barcode ='{0}' " + "\n"
  + "                   and a.out_# = d.out_# " + "\n"
  + "                union all " + "\n"
  + "                select sum(f.rec_q) " + "\n"
  + "                  from str_goodsr_1 e, str_goodsr_2 f " + "\n"
  + "                 where e.aprov = 'P' " + "\n"
  + "                   and e.brnch = '{3}' " + "\n"
  + "                   and e.compc = '{2}' " + "\n"
  + "                   and f.barcode = '{0}' " + "\n"
  + "                   and e.grn_# = f.grn_# "
             + "                union all " + "\n"
    + "           select sum(f.phy_q)  " + "\n"
     + "               from str_audit_01 e, str_audit_02 f " + "\n"
     + "           where e.aprov = 'P' " + "\n"
     + "             and e.brnch = '{3}'" + "\n"
     + "              and e.compc = '{2}'" + "\n"
     + "              and f.barcode = '{0}'" + "\n"
      + "            and e.aud_# = f.aud_# )) UNAPPROVEDQUANTITY, " + "\n"
  + "       SAL_CODENAME('CUSTM', k.PURCHASE_FROM, NULL) SUPPLIER, " + "\n"
  + "       '{5}' DAILYMSALE, " + "\n"
  + "       SAL_CODENAME('DPTCD', DPTCD, NULL) AS Department, " + "\n"
  + "       SAL_CODENAME('ICLAS', ICLAS, NULL) AS MasterCategory, " + "\n"
  + "       SAL_CODENAME('MCLAS', ICLAS, NULL) AS Category, " + "\n"
  + "       sal_itemrate('TRADE', K.ITEMS,SYSDATE,'{3}',K.BARCODE,NULL) RetailPrice, " + "\n"
  + "       '' AS BranchName, " + "\n"


+@"         (  select SUM(TOT_ORD)   from 
                   ( SELECT case when sum(ord)-sum(rec)<=0 then 0 else sum(ord)-sum(rec) end  TOT_ORD,PO_NO from 
           ( select nvl(sum(q.ord_q),0) ord ,0 rec ,p.po_no
                              from str_purord_1 p, str_purord_2 q 
                           where p.po_no = q.po_no   


                              and p.brnch = '{3}' 
                            and ( q.barcode = '{0}' or q.items ='{1}') 
                            and p.compc = '{2}'  and p.aprov ='A'   
                          group by p.po_no     
                               
                                union all 
                select  0 ord ,nvl(sum(g.rec_q),0) rec ,p.po_no 
                            from str_purord_1 p, str_purord_2 q ,str_goodsr_2 g,str_goodsr_1 h 
                           where p.po_no = q.po_no  and  q.line# = g.pol_#  and h.grn_# = g.grn_#  and h.aprov ='A' 
              
                                 and p.brnch = '{3}' 
                            and ( q.barcode = '{0}' or q.items ='{1}') 
                            and p.compc = '{2}'  and p.aprov ='A'   
                      group by p.po_no  ) group by PO_NO ) ) ONORDERQUANTITY "


    + ",Items pid,descr PName,Items PItemCode ,barcd PProductBarcode,k.IMPORTED PIsThisImportedItem, k.OPDAT POpeningDate ,k.CLDAT PClosingDate ,'' PProductClassificationCode " + "\n"
 + " ,'' PProductClassificationCodeName ,'' PPRODUCTHIERARCHYCODE ,'' PPRODUCTHIERARCHYCODENAME,k.BRAND PBrandCode ,k.BRAND_NAME PBrandCodeName ,k.Dptcd PDepartmentCode " + "\n"
 + " ,k.DPTCD_NAME PDepartmentCodeName ,k.COLUR PColorCode , nvl( (SELECT descr From Com_CodeFile WHERE Code# = k.COLUR) ,'N/A') PColorCodeName   , " + "\n"
 + " k.ISIZE PSizeCode ,nvl(com_codename('CODE#', k.ISIZE, '') ,'N/A') PSizeCodeName ,'' PABCIndicationCode ,'' PABCIndicationCodeName ,'' PGroupCode ,'' PGroupCodeName, " + "\n"
 + "    '' PPrincipleCode, '' PPrincipleCodeName,'' PDivisionCode ,'' PDivisionCodeName ,'' PVariantCode ,'' PVariantCodeName ,'' PMarketingTeamCode ,'' PMarketingTeamCodeName " + "\n"
 + "   ,'' PShelfLifeMax ,'' PShelfLifeMin ,'' PShelfLifeRemainingForSale ,'' PIsThisServiceItem ,k.IS_MASTER PIsThisMasterProduct, k.SAL_Y PIsThisItemSaleable " + "\n"
 + "  ,k.PUR_Y PIsThisItemPurchaseable ,'' PIsThisFixedAssetItem ,'' PMaintainInventory ,k.MAK_Y PDoYouMakeThisItem, '' PMaintainBatchNos,'' PMaintainSerialNo " + "\n"
 + "  ,'' PIsPriceAlreadyPrinted ,'' PSeperatePriceForChildBarcode, k.FREE_FLOW PIsThisFreeFlowProduct ,''PAutoPositionInspection ,k.CARTN PPiecesInCarton,k.PACKS PPiecesInPack " + "\n"
 + "  ,'' PPerPieceGramOrMililitre ,'' PRegistrationNo ,k.WEGHT PGrossWeight,k.UOM_W PUOMCodeGrossWeight ,k.WEGHT PNetWeight ,'' PVolume,''PUOMCodeVolume " + "\n"
 + "  ,k.HIGHT PProductHeight ,k.WIDTH PProductDepth , '' PParentKey ,k.PRINT_PRICE PPrintPricedOnBarcode,k.SHNAM PPrintShortnameOnSlip,k.ORD_Q  PMaxOrderQty,k.ORD_Q PMinOrderQty " + "\n"
 + "   ,k.REO_Q PReOrderLevelQty ,k.MAX_Q PMaxQty , k.MIN_Q  PMinQty, '' PProcurementGroupCode  , '' PPOTolerence ,'' PLeadTimeTransfer ,'' PLeadTimePurchase " + "\n"
 + " , k.LEADT PLeadTimeSales,'' PLeadTimeProduction ,k.PART# PARTICLE_PARTNO,k.CATLG PCatelogDesignNo ,k.UOM_D PUnitOfMeasureCodeInventory ,'' PUnitOfMeasureCodePurchase  ,'' PUnitOfMeasureCodeSales"

  + " " + "\n"
  + "  FROM SAL_PRODS_BARCODE_LIST k " + "\n"
  + " WHERE (UPPER(BARCODE) = '{0}' OR ITEMS = '{1}') " + "\n"
  + "   AND STATS = 'A' ";



        private static readonly string SELECT_ITEM_BY_BARCODENEW_STOCK  = ""
        + "SELECT BARCODE Barcode, " + "\n"
+ "       DESCR   ItemName, " + "\n"
+ "       --SAL_CODENAME('DPTCD', DPTCD, NULL) " + "\n"
 + "        nvl( (SELECT descr From Com_CodeFile WHERE Code# = k.COLUR) ,'N/A') || ','  ||  nvl(com_codename('CODE#', k.ISIZE, '') ,'N/A')  colur, " + "\n"
 + "       ITEMS, " + "\n"
 + "       nvl(REO_Q, 0) REO_Q, " + "\n"
 + "       nvl(MAX_Q, 0) MAX_Q, " + "\n"
 + "       nvl(MIN_Q, 0) MIN_Q, " + "\n"
  + "       nvl(fn_str_item_qty@ho('{2}', " + "\n"
   + "                        '{3}', " + "\n"
   + "                        '{3}', " + "\n"
   + "                        k.ITEMS, " + "\n"
   + "                        k.BARCODE, " + "\n"
   + "                        '{4}', " + "\n"
   + "                        sysdate), " + "\n"
   + "           0) CLOSINGQUANTITY, " + "\n"
  //+ "           0 CLOSINGQUANTITY, " + "\n"
  + "       (select nvl(sum(qty), 0) " + "\n"
  + "          from (select sum(b.rec_q) qty " + "\n"
  + "                  from str_trf_ins1 a, str_trf_ins2 b " + "\n"
  + "                 where a.aprov = 'P' " + "\n"
  + "                   and a.tin_# = b.tin_# " + "\n"
  + "                   and a.brnch = '{3}' " + "\n"
  + "                   and a.compc = '{2}' " + "\n"
  + "                   and b.barcode = '{0}' " + "\n"
  + "                union all " + "\n"
  + "                select sum(d.out_q) " + "\n"
  + "                  from str_trf_out1 a, str_trf_out2 d " + "\n"
  + "                 where a.aprov = 'P' " + "\n"
  + "                   and a.brnch = '{3}' " + "\n"
  + "                   and a.compc = '{2}' " + "\n"
  + "                   and d.barcode ='{0}' " + "\n"
  + "                   and a.out_# = d.out_# " + "\n"
  + "                union all " + "\n"
  + "                select sum(f.rec_q) " + "\n"
  + "                  from str_goodsr_1 e, str_goodsr_2 f " + "\n"
  + "                 where e.aprov = 'P' " + "\n"
  + "                   and e.brnch = '{3}' " + "\n"
  + "                   and e.compc = '{2}' " + "\n"
  + "                   and f.barcode = '{0}' " + "\n"
  + "                   and e.grn_# = f.grn_# "
             + "                union all " + "\n"
    + "           select sum(f.phy_q)  " + "\n"
     + "               from str_audit_01 e, str_audit_02 f " + "\n"
     + "           where e.aprov = 'P' " + "\n"
     + "             and e.brnch = '{3}'" + "\n"
     + "              and e.compc = '{2}'" + "\n"
     + "              and f.barcode = '{0}'" + "\n"
      + "            and e.aud_# = f.aud_# )) UNAPPROVEDQUANTITY, " + "\n"
  + "       SAL_CODENAME('CUSTM', k.PURCHASE_FROM, NULL) SUPPLIER, " + "\n"
  + "       '{5}' DAILYMSALE, " + "\n"
  + "       SAL_CODENAME('DPTCD', DPTCD, NULL) AS Department, " + "\n"
  + "       SAL_CODENAME('ICLAS', ICLAS, NULL) AS MasterCategory, " + "\n"
  + "       SAL_CODENAME('MCLAS', ICLAS, NULL) AS Category, " + "\n"
  + "       TRADE RetailPrice, " + "\n"
  + "       '' AS BranchName, " + "\n"


+@"         (  select SUM(TOT_ORD)   from 
                   ( SELECT case when sum(ord)-sum(rec)<=0 then 0 else sum(ord)-sum(rec) end  TOT_ORD,PO_NO from 
           ( select nvl(sum(q.ord_q),0) ord ,0 rec ,p.po_no
                              from str_purord_1 p, str_purord_2 q 
                           where p.po_no = q.po_no   


                              and p.brnch = '{3}' 
                            and ( q.barcode = '{0}' or q.items ='{1}') 
                            and p.compc = '{2}'  and p.aprov ='A'   
                          group by p.po_no     
                               
                                union all 
                select  0 ord ,nvl(sum(g.rec_q),0) rec ,p.po_no 
                            from str_purord_1 p, str_purord_2 q ,str_goodsr_2 g,str_goodsr_1 h 
                           where p.po_no = q.po_no  and  q.line# = g.pol_#  and h.grn_# = g.grn_#  and h.aprov ='A' 
              
                                 and p.brnch = '{3}' 
                            and ( q.barcode = '{0}' or q.items ='{1}') 
                            and p.compc = '{2}'  and p.aprov ='A'   
                      group by p.po_no  ) group by PO_NO ) ) ONORDERQUANTITY "


    + ",Items pid,descr PName,Items PItemCode ,barcd PProductBarcode,k.IMPORTED PIsThisImportedItem, k.OPDAT POpeningDate ,k.CLDAT PClosingDate ,'' PProductClassificationCode " + "\n"
 + " ,'' PProductClassificationCodeName ,'' PPRODUCTHIERARCHYCODE ,'' PPRODUCTHIERARCHYCODENAME,k.BRAND PBrandCode ,k.BRAND_NAME PBrandCodeName ,k.Dptcd PDepartmentCode " + "\n"
 + " ,k.DPTCD_NAME PDepartmentCodeName ,k.COLUR PColorCode , nvl( (SELECT descr From Com_CodeFile WHERE Code# = k.COLUR) ,'N/A') PColorCodeName   , " + "\n"
 + " k.ISIZE PSizeCode ,nvl(com_codename('CODE#', k.ISIZE, '') ,'N/A') PSizeCodeName ,'' PABCIndicationCode ,'' PABCIndicationCodeName ,'' PGroupCode ,'' PGroupCodeName, " + "\n"
 + "    '' PPrincipleCode, '' PPrincipleCodeName,'' PDivisionCode ,'' PDivisionCodeName ,'' PVariantCode ,'' PVariantCodeName ,'' PMarketingTeamCode ,'' PMarketingTeamCodeName " + "\n"
 + "   ,'' PShelfLifeMax ,'' PShelfLifeMin ,'' PShelfLifeRemainingForSale ,'' PIsThisServiceItem ,k.IS_MASTER PIsThisMasterProduct, k.SAL_Y PIsThisItemSaleable " + "\n"
 + "  ,k.PUR_Y PIsThisItemPurchaseable ,'' PIsThisFixedAssetItem ,'' PMaintainInventory ,k.MAK_Y PDoYouMakeThisItem, '' PMaintainBatchNos,'' PMaintainSerialNo " + "\n"
 + "  ,'' PIsPriceAlreadyPrinted ,'' PSeperatePriceForChildBarcode, k.FREE_FLOW PIsThisFreeFlowProduct ,''PAutoPositionInspection ,k.CARTN PPiecesInCarton,k.PACKS PPiecesInPack " + "\n"
 + "  ,'' PPerPieceGramOrMililitre ,'' PRegistrationNo ,k.WEGHT PGrossWeight,k.UOM_W PUOMCodeGrossWeight ,k.WEGHT PNetWeight ,'' PVolume,''PUOMCodeVolume " + "\n"
 + "  ,k.HIGHT PProductHeight ,k.WIDTH PProductDepth , '' PParentKey ,k.PRINT_PRICE PPrintPricedOnBarcode,k.SHNAM PPrintShortnameOnSlip,k.ORD_Q  PMaxOrderQty,k.ORD_Q PMinOrderQty " + "\n"
 + "   ,k.REO_Q PReOrderLevelQty ,k.MAX_Q PMaxQty , k.MIN_Q  PMinQty, '' PProcurementGroupCode  , '' PPOTolerence ,'' PLeadTimeTransfer ,'' PLeadTimePurchase " + "\n"
 + " , k.LEADT PLeadTimeSales,'' PLeadTimeProduction ,k.PART# PARTICLE_PARTNO,k.CATLG PCatelogDesignNo ,k.UOM_D PUnitOfMeasureCodeInventory ,'' PUnitOfMeasureCodePurchase  ,'' PUnitOfMeasureCodeSales"

  + " " + "\n"
  + "  FROM SAL_PRODS_BARCODE_LIST k " + "\n"
  + " WHERE (UPPER(BARCODE) = '{0}' OR ITEMS = '{1}') " + "\n"
  + "   AND STATS = 'A' AND DPTCD='{6}' ";
       private static readonly string SELECT_ONORDER_DOCUMENT = "select TOTALQTY ,SUPPLIER,DOCUMENTNO,WORKDATE,DAYS from ( select case when sum(nvl(TOTALQTY,0) ) -sum(nvl(RECV,0) ) <=0 then 0 else sum(nvl(TOTALQTY,0) ) -sum(nvl(RECV,0) ) end TOTALQTY ,SUPPLIER,DOCUMENTNO,WORKDATE,DAYS from ( " + "\n"
 + " select sum(q.ord_q) TOTALQTY,  0 RECV, SAL_CODENAME('CUSTM', p.supcd, NULL) SUPPLIER,q.po_no DOCUMENTNO,to_char(p.wdate,'dd-Mon-YYYY')  WORKDATE, ABS((p.wdate - TRUNC(SYSDATE))) DAYS " + "\n"
 + "               from str_purord_1 p, str_purord_2 q  " + "\n"
 + "              where p.po_no = q.po_no  " + "\n"
 + "                " + "\n"
 + "                                       " + "\n"
 + "                                     " + "\n"
 + "                and p.brnch = '{3}' " + "\n"
 + "                and ( q.barcode = '{0}' or q.items ='{1}') " + "\n"
 + "                and p.compc = '{2}'  and p.aprov ='A'  " + "\n"
 + "                group by p.supcd,q.po_no,p.wdate having sum(q.ord_q) >0 " + "\n"
 + "                 " + "\n"
 + "                UNION ALL " + "\n"
 + "                 " + "\n"
 + "select 0  TOTALQTY,  sum(g.rec_q) RECV, SAL_CODENAME('CUSTM', p.supcd, NULL) SUPPLIER,q.po_no DOCUMENTNO,to_char(p.wdate,'dd-Mon-YYYY')  WORKDATE, ABS((p.wdate - TRUNC(SYSDATE))) DAYS " + "\n"
 + "               from str_purord_1 p, str_purord_2 q ,str_goodsr_2 g,str_goodsr_1 h " + "\n"
 + "              where p.po_no = q.po_no  and  q.line# = g.pol_# and h.grn_# = g.grn_#  and h.aprov ='A' " + "\n"
 + "                " + "\n"
 + "                and p.brnch = '{3}' " + "\n"
 + "                and ( q.barcode = '{0}' or q.items ='{1}') " + "\n"
 + "                and p.compc = '{2}'  and p.aprov ='A'  " + "\n"
 + "                group by p.supcd,q.po_no,p.wdate " + "\n"
 + ")group by SUPPLIER,DOCUMENTNO,WORKDATE,DAYS ) where TOTALQTY !=0  " + "\n"
 + "              " + "\n"
 + "          " + "\n"
 + "           union all " + "\n"
            + "           select SUM(TOT_ORD) , 'Total','','', 0  from  " + "\n"
            + @"         ( SELECT case when sum(ord)-sum(rec)<=0 then 0 else sum(ord)-sum(rec) end  TOT_ORD,PO_NO from  
           ( select nvl(sum(q.ord_q),0) ord ,0 rec ,p.po_no
                              from str_purord_1 p, str_purord_2 q 
                           where p.po_no = q.po_no   " + "\n"


            + "                  and p.brnch = '{3}' " + "\n"
            + "                and ( q.barcode = '{0}' or q.items ='{1}') " + "\n"
            + "                and p.compc = '{2}'  and p.aprov ='A'   " + "\n"
            + "              group by p.po_no     " + "\n"
            + "                   " + "\n"
            + "                    union all " + "\n"
            + "    select  0 ord ,nvl(sum(g.rec_q),0) rec ,p.po_no " + "\n"
            + "                from str_purord_1 p, str_purord_2 q ,str_goodsr_2 g,str_goodsr_1 h " + "\n"
            + "               where p.po_no = q.po_no  and  q.line# = g.pol_#  and h.grn_# = g.grn_#  and h.aprov ='A' " + "\n"
            + "  " + "\n"
            + "                  and p.brnch = '{3}' " + "\n"
            + "                and ( q.barcode = '{0}' or q.items ='{1}') " + "\n"
            + "                and p.compc = '{2}'  and p.aprov ='A'  " + "\n"
            + "          group by p.po_no  ) group by PO_NO )";





        //+ "           select sum(ord)-sum(rec) , 'Total','','', 0  from " + "\n"
        //+ "           ( " + "\n"
        //+ "           select nvl(sum(q.ord_q),0) ord ,0 rec " + "\n"
        //+ "                 from str_purord_1 p, str_purord_2 q " + "\n"
        //+ "                where p.po_no = q.po_no " + "\n"
        //+ "                 " + "\n"
        //+ "                                         " + "\n"
        //+ "                                       " + "\n"
        //+ "                  and p.brnch = '{3}' " + "\n"
        //+ "                and ( q.barcode = '{0}' or q.items ='{1}') " + "\n"
        //+ "                and p.compc = '{2}' " + "\n"
        //+ "                   " + "\n"
        //+ "                   " + "\n"
        //+ "                    union all " + "\n"
        //+ "    select  0 ord ,nvl(sum(g.rec_q),0) rec " + "\n"
        //+ "                from str_purord_1 p, str_purord_2 q ,str_goodsr_2 g,str_goodsr_1 h " + "\n"
        //+ "               where p.po_no = q.po_no  and  q.line# = g.pol_#  and h.grn_# = g.grn_#  and h.aprov ='A' " + "\n"
        //+ "  " + "\n"
        //+ "                  and p.brnch = '{3}' " + "\n"
        //+ "                and ( q.barcode = '{0}' or q.items ='{1}') " + "\n"
        //+ "                and p.compc = '{2}' " + "\n"
        //+ "                 )";

        //"select sum(nvl(TOTALQTY,0) ) -sum(nvl(RECV,0) ) TOTALQTY ,SUPPLIER,DOCUMENTNO,WORKDATE,DAYS from ( " + "\n"
        //+ " select sum(q.ord_q) TOTALQTY,  0 RECV, SAL_CODENAME('CUSTM', p.supcd, NULL) SUPPLIER,q.po_no DOCUMENTNO,to_char(p.wdate,'dd-Mon-YYYY')  WORKDATE, ABS((p.wdate - TRUNC(SYSDATE))) DAYS " + "\n"
        //+ "               from str_purord_1 p, str_purord_2 q  " + "\n"
        //+ "              where p.po_no = q.po_no  " + "\n"
        //+ "                " + "\n"
        //+ "                                       " + "\n"
        //+ "                                     " + "\n"
        //+ "                and p.brnch = '{3}' " + "\n"
        //+ "                and ( q.barcode = '{0}' or q.items ='{1}') " + "\n"
        //+ "                and p.compc = '{2}' " + "\n"
        //+ "                group by p.supcd,q.po_no,p.wdate " + "\n"
        //+ "                 " + "\n"
        //+ "                UNION ALL " + "\n"
        //+ "                 " + "\n"
        //+ "select 0  TOTALQTY,  sum(g.rec_q) RECV, SAL_CODENAME('CUSTM', p.supcd, NULL) SUPPLIER,q.po_no DOCUMENTNO,to_char(p.wdate,'dd-Mon-YYYY')  WORKDATE, ABS((p.wdate - TRUNC(SYSDATE))) DAYS " + "\n"
        //+ "               from str_purord_1 p, str_purord_2 q ,str_goodsr_2 g,str_goodsr_1 h " + "\n"
        //+ "              where p.po_no = q.po_no  and q.po_no = h.pono and h.grn_# = g.grn_#  and h.aprov ='A' " + "\n"
        //+ "                " + "\n"
        //+ "                and p.brnch = '{3}' " + "\n"
        //+ "                and ( q.barcode = '{0}' or q.items ='{1}') " + "\n"
        //+ "                and p.compc = '{2}' " + "\n"
        //+ "                group by p.supcd,q.po_no,p.wdate " + "\n"
        //+ ")group by SUPPLIER,DOCUMENTNO,WORKDATE,DAYS  having (sum(nvl(TOTALQTY,0) ) -sum(nvl(RECV,0) ))>0 " + "\n"
        //+ "              " + "\n"
        //+ "          " + "\n"
        //+ "           union all " + "\n"

        //+ " select sum(nvl(TOTALQTY,0) ) -sum(nvl(RECV,0) ) , 'Total','','', 0  from  " + "\n"
        //     + "       ( select sum(q.ord_q) TOTALQTY,  0 RECV " + "\n"
        //+ "               from str_purord_1 p, str_purord_2 q  " + "\n"
        //+ "              where p.po_no = q.po_no  " + "\n"
        //+ "                " + "\n"
        //+ "                                       " + "\n"
        //+ "                                     " + "\n"
        //+ "                and p.brnch = '{3}' " + "\n"
        //+ "                and ( q.barcode = '{0}' or q.items ='{1}') " + "\n"
        //+ "                and p.compc = '{2}' " + "\n"
        //+ "               " + "\n"
        //+ "                 " + "\n"
        //+ "                UNION ALL " + "\n"
        //+ "                 " + "\n"
        //+ "select 0  TOTALQTY,  sum(g.rec_q) RECV  " + "\n"
        //+ "               from str_purord_1 p, str_purord_2 q ,str_goodsr_2 g,str_goodsr_1 h " + "\n"
        //+ "              where p.po_no = q.po_no  and q.po_no = h.pono and h.grn_# = g.grn_#  and h.aprov ='A' " + "\n"
        //+ "                " + "\n"
        //+ "                and p.brnch = '{3}' " + "\n"
        //+ "                and ( q.barcode = '{0}' or q.items ='{1}') " + "\n"
        //+ "                and p.compc = '{2}' " + "\n"
        //+ "                " + "\n"
        //+ ")   having (sum(nvl(TOTALQTY,0) ) -sum(nvl(RECV,0) ))>0 ";


        private static readonly string SELECT_ONORDER_DOCUMENT1 = ""
          + "select sum(q.ord_q) TOTALQTY, SAL_CODENAME('CUSTM', p.supcd, NULL) SUPPLIER,q.po_no DOCUMENTNO,to_char(p.wdate,'dd-Mon-YYYY')  WORKDATE, ABS((wdate - TRUNC(SYSDATE))) DAYS " + "\n"
  + "             from str_purord_1 p, str_purord_2 q " + "\n"
  + "            where p.po_no = q.po_no " + "\n"
  + "              and q.line# not in (select g.pol_# " + "\n"
  + "                                    from str_goodsr_2 g " + "\n"
  + "                                   where g.pol_# = q.line#) " + "\n"
  + "              and p.brnch = '{3}' " + "\n"
  + "              and ( q.barcode = '{0}' or q.items ='{1}') " + "\n"
  + "              and p.compc = '{2}' " + "\n"
  + "              group by p.supcd,q.po_no,p.wdate"
          + " union all select nvl(sum(q.ord_q),0) TOTALQTY, 'Total','','', 0 " + "\n"
  + "               from str_purord_1 p, str_purord_2 q " + "\n"
  + "              where p.po_no = q.po_no " + "\n"
  + "                and q.line# not in (select g.pol_# " + "\n"
  + "                                      from str_goodsr_2 g " + "\n"
  + "                                     where g.pol_# = q.line#) " + "\n"
  + "               and p.brnch = '{3}' " + "\n"
  + "                and ( q.barcode = '{0}' or q.items ='{1}') " + "\n"
  + "                and p.compc = '{2}'";

        private static readonly string SELECT_STOCK_IN_HAND2 = ""
          + "select " + "\n"
  + "fn_str_item_qty@ho(C.COMPC, J.LCODE, null, k.ITEMS, k.BARCODE,  '{4}', sysdate) STOCKINHAND , J.LCODE OFFICEID ,  com_codename ('BRNCH',J.LCODE,'') OFFICENAME ,com_codename('CODE#',Z.CODE#,'') NAME " + "\n"
  + "FROM SAL_PRODS_BARCODE_LIST k " + "\n"
  + "INNER JOIN COM_LOCATION J ON J.LCODE = '{3}' " + "\n"
  + "INNER JOIN COMPANY_INFO C  ON C.COMPC  = '{2}' " + "\n"
  + "INNER JOIN com_codefile z  ON Z.CODE# = '{4}' " + "\n"
  + " where z.type#  = 'S03' " + "\n"
  + " AND (UPPER(BARCODE) = '{0}' OR ITEMS = '{1}') " + "\n"
  + "   AND K.STATS = 'A'";

        private static readonly string SELECT_STOCK_IN_HAND = "   SELECT STOCKINHAND,OFFICEID,OFFICENAME,NAME FROM (select " + "\n"

 + "  fn_str_item_qty@ho(C.COMPC, J.LCODE, null, k.ITEMS, k.BARCODE, Z.CODE#, sysdate) STOCKINHAND , J.LCODE OFFICEID ,  com_codename ('BRNCH',J.LCODE,'') OFFICENAME ,com_codename('CODE#',Z.CODE#,'') NAME " + "\n"
            + "  ,case J.LCODE when '{2}' then  0  else 1   end ord , case Z.CODE# when '{3}' then  0  else 1 END ORD2 " + "\n"
            + "  FROM SAL_PRODS_BARCODE_LIST k " + "\n"
 + "  INNER JOIN COM_LOCATION J ON J.LCODE  LIKE '%' " + "\n"
 + "  INNER JOIN COMPANY_INFO C  ON C.COMPC  LIKE '%' " + "\n"
 + " INNER JOIN com_codefile z  ON Z.CODE# like '%' " + "\n"
 + "   where z.type#  = 'S03' " + "\n"
 + "   AND (UPPER(BARCODE) = '{0}' OR '{1}' = '{1}') " + "\n"
 + "     AND K.STATS = 'A'     ORDER BY  ord,ORD2 ) WHERE STOCKINHAND!=0   ";


       private static readonly string SELECT_STOCK_IN_HANdUP= @" select d.* from (
SELECT fn_str_item_qty@ho('{1}', LCODE, null, K.ITEMS, k.BARCODE, CODE#, sysdate)  STOCKINHAND,com_codename ('BRNCH',LCODE,'') OFFICENAME, LCODE OFFICEID ,
 com_codename('CODE#', CODE#,'') NAME FROM (

select
  J.LCODE OFFICEID, J.LCODE , Z.CODE#
   FROM COM_LOCATION J
  , com_codefile z
    where z.type#  = 'S03'
and j.lcode='{2}' 
   )  ,SAL_PRODS_BARCODE_LIST K
   WHERE K.BARCODE ='{0}' 
      AND K.STATS = 'A') d

      where d.STOCKINHAND !=0 ";
        private static readonly string SELECT_STOCK_IN_HAND_IND = @"select sum(STOCKINHAND) STOCKINHAND,barcode,OFFICEID , OFFICENAME ,DESCR name from (
SELECT fn_str_item_qty@ho(1, LCODE, null, K.ITEMS, k.BARCODE, CODE#, sysdate)  STOCKINHAND,com_codename ('BRNCH',LCODE,'') OFFICENAME, LCODE OFFICEID ,k.BARCODE,k.DESCR,
 com_codename('CODE#', CODE#,'') NAME FROM (
select
  J.LCODE OFFICEID, J.LCODE , Z.CODE#
   FROM COM_LOCATION J
  , com_codefile z
    where z.code#  = '0001S03'
and j.lcode = '{0}' 
   )  ,SAL_PRODS_BARCODE_LIST K
   WHERE  K.BARCODE in('60030','40644','1498802811648',	'GA00004','GA00005','GA00006'	,'GS00007','GS00008','GS00009','GS00010','TO00011','TO00012','TO00013','PS00014','TO00015','TO00016','TO00017','TO00018','TO00019')
      AND K.STATS = 'A'  ) d
    --  where BARCODE in('60030','40644','1498802811648')
      group by  OFFICEID , OFFICENAME,barcode,DESCR";
        private static readonly string SELECT_UNAPPROVED_DOC = ""
          + "select * from (select nvl(sum(recqty), 0) RECEIVEQTY, " + "\n"
  + "       nvl(sum(recqty) - sum(sndqty), 0) TOTALQTY, " + "\n"
  + "       docno, " + "\n"
  + "       DOCTYPE, " + "\n"
  + "       to_char(WDATE,'dd-Mon-YYYY') WDATE, " + "\n"
  + "       ABS((wdate - TRUNC(SYSDATE))) DAYS,str_t STORETO,str_f STOREFROM, " + "\n"
  + "      str_codename('STOR#',str_f,null)||'/'|| nvl(str_codename('STOR#',str_t,null),nvl(sal_codename('LCODE',str_f,null),'N/A')) details, " + "\n"
  + "       (select descr " + "\n"
  + "          FROM SAL_PRODS_BARCODE_LIST k " + "\n"
  + "         where UPPER(BARCODE) = '{0}') PRODUCTNAME " + "\n"
  + "  from (select sum(b.rec_q) recqty, " + "\n"
  + "               0 sndqty, " + "\n"
  + "               a.tin_# docno, " + "\n"
  + "               'Tansfer IN' DOCTYPE, " + "\n"
  + "               A.WDATE, a.str_t,a.str_f " + "\n"
  + "          from str_trf_ins1 a, str_trf_ins2 b " + "\n"
  + "         where a.aprov = 'P' " + "\n"
  + "           and a.tin_# = b.tin_# " + "\n"
  + "           and a.brnch = '{3}' " + "\n"
  + "           and a.compc = '{2}' " + "\n"
  + "           And (b.barcode = '{0}' OR B.ITEMS = '{1}') " + "\n"
  + "         group by a.tin_#, A.WDATE, a.str_t,a.str_f " + "\n"
  + "        union all " + "\n"
  + "        select sum(d.out_q) recqty, " + "\n"
  + "                0 sndqty, " + "\n"
  + "               a.out_# docno, " + "\n"
  + "               'Tansfer OUT' DOCTYPE, " + "\n"
  + "               A.WDATE,a.str_t,a.str_f " + "\n"
  + "          from str_trf_out1 a, str_trf_out2 d " + "\n"
  + "         where a.aprov = 'P' " + "\n"
  + "           and a.brnch = '{3}' " + "\n"
  + "           and a.compc = '{2}' " + "\n"
  + "           and (D.barcode = '{0}' OR D.ITEMS = '{1}') " + "\n"
  + "           and a.out_# = d.out_# " + "\n"
  + "         group by a.out_#, A.WDATE,a.str_t,a.str_f " + "\n"
 + "        union all " + "\n"
  + "        select sum(d.phy_q) recqty, " + "\n"
  + "                0 sndqty, " + "\n"
  + "               a.aud_# docno, " + "\n"
  + "               'Stock Audit' DOCTYPE, " + "\n"
  + "               A.WDATE,'' str_t,a.str_f str_f " + "\n"
  + "          from str_audit_01 a, str_audit_02 d " + "\n"
  + "         where a.aprov = 'P' " + "\n"
  + "           and a.brnch = '{3}' " + "\n"
  + "           and a.compc = '{2}' " + "\n"
  + "           and (D.barcode = '{0}' OR D.ITEMS = '{1}') " + "\n"
  + "           and a.aud_# = d.aud_# " + "\n"
  + "         group by a.aud_#, A.WDATE,a.str_f " + "\n"
  + "        union all " + "\n"
  + "        select sum(f.rec_q) recqty, " + "\n"
  + "               0 sndqty, " + "\n"
  + "               e.grn_# docno, " + "\n"
  + "               'GRN ' DOCTYPE, " + "\n"
  + "                E.WDATE, null str_t,e.str_f str_f " + "\n"
  + "          from str_goodsr_1 e, str_goodsr_2 f " + "\n"
  + "         where e.aprov = 'P' " + "\n"
  + "           and e.brnch = '{3}' " + "\n"
  + "           and e.compc = '{2}' " + "\n"
  + "           and (f.barcode = '{0}' OR f.ITEMS = '{1}') " + "\n"
  + "           and e.grn_# = f.grn_# " + "\n"
  + "         group by e.grn_#, E.WDATE,e.str_f)    " + "\n"
  + " group by docno, DOCTYPE, WDATE,str_t,str_f order by    WDATE desc ,DOCTYPE,docno desc )  "
          + " union all " + "\n"
  + "    " + "\n"
  + "   select nvl(sum(recqty), 0) RECEIVEQTY, " + "\n"
  + "       nvl(sum(recqty) - sum(sndqty), 0) TOTALQTY, " + "\n"
  + "       '', " + "\n"
  + "       '', " + "\n"
  + "         '', " + "\n"
  + "        0,'','', " + "\n"
  + "         ' Total ' details,'' " + "\n"
  + "          " + "\n"
  + "    from (select sum(b.rec_q) recqty, " + "\n"
  + "                 0 sndqty, " + "\n"
  + "                 a.tin_# docno, " + "\n"
  + "                 'Tansfer IN' DOCTYPE, " + "\n"
  + "                 A.WDATE " + "\n"
  + "            from str_trf_ins1 a, str_trf_ins2 b " + "\n"
  + "           where a.aprov = 'P' " + "\n"
  + "             and a.tin_# = b.tin_# " + "\n"
  + "             and a.brnch = '{3}' " + "\n"
  + "             and a.compc = '{2}' " + "\n"
  + "             And (b.barcode = '{0}' OR B.ITEMS = '{1}') " + "\n"
  + "           group by a.tin_#, A.WDATE " + "\n"
  + "          union all " + "\n"
  + "          select sum(d.out_q) recqty, " + "\n"
  + "                  0 sndqty, " + "\n"
  + "                 a.out_# docno, " + "\n"
  + "                 'Tansfer OUT' DOCTYPE, " + "\n"
  + "                 A.WDATE " + "\n"
  + "            from str_trf_out1 a, str_trf_out2 d " + "\n"
  + "           where a.aprov = 'P' " + "\n"
  + "             and a.brnch = '{3}' " + "\n"
  + "             and a.compc = '{2}' " + "\n"
  + "             and (D.barcode = '{0}' OR D.ITEMS = '{1}') " + "\n"
  + "             and a.out_# = d.out_# " + "\n"
  + "           group by a.out_#, A.WDATE " + "\n"
            + "          union all " + "\n"
  + "          select sum(d.phy_q) recqty, " + "\n"
  + "                0 sndqty, " + "\n"
  + "                 a.aud_# docno, " + "\n"
  + "                 'Tansfer OUT' DOCTYPE, " + "\n"
  + "                 A.WDATE " + "\n"
  + "            from str_audit_01 a, str_audit_02 d " + "\n"
  + "           where a.aprov = 'P' " + "\n"
  + "             and a.brnch = '{3}' " + "\n"
  + "             and a.compc = '{2}' " + "\n"
  + "             and (D.barcode = '{0}' OR D.ITEMS = '{1}') " + "\n"
  + "             and a.aud_# = d.aud_# " + "\n"
  + "           group by a.aud_#, A.WDATE " + "\n"
  + "          union all " + "\n"
  + "          select sum(f.rec_q) recqty, " + "\n"
  + "                 0 sndqty, " + "\n"
  + "                 e.grn_# docno, " + "\n"
  + "                 'GRN ' DOCTYPE, " + "\n"
  + "                 E.WDATE " + "\n"
  + "            from str_goodsr_1 e, str_goodsr_2 f " + "\n"
  + "           where e.aprov = 'P' " + "\n"
  + "            and e.brnch = '{3}' " + "\n"
  + "            and e.compc = '{2}' " + "\n"
  + "            and (f.barcode = '{0}' OR  F.ITEMS = '{1}') " + "\n"
  + "             and e.grn_# = f.grn_# " + "\n"
  + "           group by e.grn_#, E.WDATE) " + "\n"
  + "   ";

        private static readonly string SELECT_POMASTERINFO = ""
          + "select po.PO_NO ID,'Y' POSTEDYN, " + "\n"
  + "       ACTIV, " + "\n"
  + "       CASE po.APROV " + "\n"
  + "         WHEN 'A' THEN " + "\n"
  + "          'Y' " + "\n"
  + "         ELSE " + "\n"
  + "          'N' " + "\n"
  + "       END APPROVALYN, " + "\n"
  + "       'N' BLOCKEDYN, " + "\n"
  + "       DlvTO OFFICECODESHIPTO, " + "\n"
  + "       com_codename('BRNCH', po.dlvto, NULL) OFFICECODE_SHIPTONAME, " + "\n"
  + "       po.dlvto OFFICECODEBILLTO, " + "\n"
  + "       com_codename('BRNCH', po.dlvto, NULL) OFFICECODE_BILLTONAME, " + "\n"
  + "       po.PO_NO DOCUMENTNO, " + "\n"
  + "       TO_CHAR(po.WDATE, 'dd-Mon-yyyy') WORKDATE, " + "\n"
  + "       po.usrid USERCODEREQUESTEDBY, " + "\n"
  + "       com_codename('USERS', po.usrid, null) USERCODEREQUESTBYNAME, " + "\n"
  + "       po.supcd BUSINESSPARTNERCODE, " + "\n"
  + "       (select s.descr from sal_cus_mast s where s.custm = po.supcd) BUSINESSPARTNERNAME, " + "\n"
  + "       po.curcd CURENCYCODE, " + "\n"
  + "       (select descr from com_currency c where c.curcd = po.curcd) CURRENCYNAME,' ' CURRENCYRATE, " + "\n"
  + "       CASE po.potyp " + "\n"
  + "         WHEN 'L' THEN " + "\n"
  + "          'Local' " + "\n"
  + "         ELSE " + "\n"
  + "          'Imported' " + "\n"
  + "       END POISLOCALORIMPORTED, " + "\n"
  + "       ' ' COUNTRYCODE, " + "\n"
  + "       '  ' COUNTRYNAME, " + "\n"
  + "       po.pterm PAYMENTTERMCODE, " + "\n"
  + "       (select descr from COM_PAYMENTT where pmtcd = po.pterm) PAYMENTTERMNAME, " + "\n"
  + "       po.tolrc TOLERENCEPERCENTAGE, " + "\n"
  + "       po.revsn POREVISIONNO, " + "\n"
  + "       po.deldt DELIVERYDATE, " + "\n"
  + "       TO_CHAR(po.povalidup2, 'dd-Mon-yyyy') POVALIDUPTO, " + "\n"
  + "       sum(pod.ord_q) NUMBEROFITEMS, " + "\n"
  + "       sum(pod.grosv + pod.gst_a) POVALUE, " + "\n"
  + "       round(avg(pod.dis_p), 2) PODISCOUNTPERCENTAGE, " + "\n"
  + "       sum(pod.dis_a) PODISCOUNTAMOUNT, " + "\n"
  + "       round(sum(pod.amont), 2) POROUNDAMOUNT, " + "\n"
  + "        " + "\n"
  + "       sum(pod.amont) PONETVALUE,'' REFERENCENO ,''DETAILS ,( SELECT DESCR FROM STR_STORES_M WHERE STOR# =PO.STR_F)ORIGIONOFSHIPMENT " + "\n"
  + "   ,''EXTRAFIELDS01,''EXTRAFIELDS02,''EXTRAFIELDS03,''EXTRAFIELDS04,''EXTRAFIELDS05,''EXTRAFIELDS06,''EXTRAFIELDS07,''EXTRAFIELDS08,''EXTRAFIELDS09,''EXTRAFIELDS10 " + "\n"
  + "  from str_purord_1 po " + "\n"
  + "  left outer join str_purord_2 pod on pod.po_no = po.po_no " + "\n"
  + " where (po.po_no = '{0}' " + "\n"
  + "    or po.supcd = '{1}' ) " + "\n"
  + "    and po.BRNCH ='{3}' " + "\n"
  + "    and po.compc ='{2}' " + "\n"
  + " group by po.PO_NO, " + "\n"
  + "          ACTIV, " + "\n"
  + "          po.APROV, " + "\n"
  + "          DlvTO, " + "\n"
  + "          po.dlvto, " + "\n"
  + "          po.dlvto, " + "\n"
  + "          po.dlvto, " + "\n"
  + "          po.PO_NO, " + "\n"
  + "          po.WDATE, " + "\n"
  + "          po.usrid, " + "\n"
  + "          po.usrid, " + "\n"
  + "          po.supcd, " + "\n"
  + "          po.supcd, " + "\n"
  + "          po.curcd, " + "\n"
  + "          po.potyp, " + "\n"
  + "          po.pterm, " + "\n"
  + "          po.tolrc, " + "\n"
  + "          po.revsn, " + "\n"
  + "          po.deldt, " + "\n"
  + "          po.povalidup2,PO.STR_F";

        private static readonly string SELECT_INDMASTERINFO = @"select po.ind_# ID,'Y' POSTEDYN, 
        ACTIV, 
        CASE po.APROV 
          WHEN 'A' THEN 
           'Y' 
          ELSE 
           'N' 
        END APPROVALYN, 
        'N' BLOCKEDYN, 
        po.ind_# DOCUMENTNO, 
        TO_CHAR(po.WDATE, 'dd-Mon-yyyy') WORKDATE, 
        po.usrid USERCODEREQUESTEDBY, 
        com_codename('USERS', po.usrid, null) USERCODEREQUESTBYNAME, 
        round(sum(pod.ind_v), 2) POROUNDAMOUNT, 
        sum(pod.Ind_q) QTY,
        po.str_f  STOREFROM,
        po.Str_t  STORETO,
        po.brnch            BRANCH ,
                  po.compc              COMPANY


   from str_indent_1 po
   left outer join str_indent_2 pod on pod.ind_# = po.ind_# 
  where (po.ind_# = '{0}' 
    ) 
     and po.Str_t ='{1}' 

  group by 
           ACTIV, 
           po.APROV, 
           po.ind_#, 
           po.WDATE, 
           po.usrid, 
           po.usrid, 
              po.str_T ,
           PO.STR_F, po.brnch   ,   po.compc  ";




        private static readonly string SELECT_PODETAIL = ""
          + "select LINE# DETAIL_ID, " + "\n"
  + "       pod.items DETAIL_PRODUCTCODE, " + "\n"
  + "       p.BARCODE DETAIL_PRODUCTBARCODE, " + "\n"
  + "       ' ' DETAIL_PRICECODE, " + "\n"
  + "       pod.rat_n DETAIL_COSTPRICE, " + "\n"
  + "       pod.itmrt DETAIL_SALESPRICE, " + "\n"
  + "       pod.itmrt DETAIL_RETAILPRICE, " + "\n"
  + "       pod.ORD_Q DETAIL_ORDERQTY, " + "\n"
  + "       (pod.grosv + pod.gst_a) DETAIL_ORDERVALUE, " + "\n"
  + "       ' ' DETAIL_UOMCODE, " + "\n"
  + "       ' ' DETAIL_UOMCODENAME, " + "\n"
  + "       ' ' DETAIL_NEEDBYDATE, " + "\n"
  + "       pod.i_h_q DETAIL_INHANDQTY, " + "\n"
  + "       pod.req_q DETAIL_REQUIREDQTY, " + "\n"
  + "       ' ' DETAIL_REFERENCETABLECODE, " + "\n"
  + "       pod.po_no DETAIL_REFERENCEDOCUMENTID, " + "\n"
  + "       ' ' DETAIL_PACKINGDETAILSCODE, " + "\n"
  + "       ' ' DETAIL_PACKAGINGDETAILS, " + "\n"
  + "       ' ' DETAIL_DETAILS, " + "\n"
  + "       ' ' DETAIL_ORDERQTY1, " + "\n"
  + "       ' ' DETAIL_ORDERQTY2, " + "\n"
  + "       ' ' DETAIL_ORDERQTY3, " + "\n"
  + "       ' ' DETAIL_COSTPRICE1, " + "\n"
  + "       ' ' DETAIL_COSTPRICE2, " + "\n"
  + "       ' ' DETAIL_COSTPRICE3, " + "\n"
  + "       pod.foc_cost_share DETAIL_FREEOFCOSTQTY, " + "\n"
  + "       pod.dis_a DETAIL_DISCOUNTAMOUNT, " + "\n"
  + "       (pod.grosv - pod.dis_a) DETAIL_GROSSVALUEAFTERDISCOUNT, " + "\n"
  + "       ' ' DETAIL_CALCSALESTAXFOCQTY, " + "\n"
  + "       pod.gst_p DETAIL_SALESTAXPER, " + "\n"
  + "       pod.gst_a DETAIL_SALESTAXAMOUNT, " + "\n"
  + "       pod.edagst_p DETAIL_EXCISEDUTYPER, " + "\n"
  + "       pod.edagst_a DETAIL_EXCISEDUTYAMOUNT, " + "\n"
  + "       pod.ext_p DETAIL_EXTRADISCOUNTPER, " + "\n"
  + "       pod.ext_a DETAIL_EXTRADISCOUNTAMOUNT, " + "\n"
  + "       pod.com_p DETAIL_COMMISIONPER, " + "\n"
  + "       pod.com_a DETAIL_COMMISIONAMOUNT, " + "\n"
  + "       pod.cdt_p DETAIL_CUSTOMDUTYPER, " + "\n"
  + "       pod.cdt_a DETAIL_CUSTOMDUTYAMT, " + "\n"
  + "       p.items PRODUCT_ID, " + "\n"
  + "       p.descr PRODUCT_NAME, " + "\n"
  + "       p.items PRODUCT_ITEMCODE, " + "\n"
  + "       p.barcd PRODUCT_PRODUCTBARCODE, " + "\n"
  + "       p.imported ISTHISIMPORTEDITEM, " + "\n"
  + "       ' ' OPENINGDATE, " + "\n"
  + "       ' ' CLOSINGDATE, " + "\n"
  + "       ' ' PRODUCTCLASSIFICATIONCODE, " + "\n"
  + "       '' PRODUCTCLASSFICATIONCODENAME, " + "\n"
  + "       ' ' PRODUCTHIERARCHYCODE, " + "\n"
  + "       ' ' PRODUCTHIERARCHYNAME, " + "\n"
  + "       p.brand BRANDCODE, " + "\n"
  + "       com_codename('CODE#', p.brand, '') PRODUCTBRANDNAME, " + "\n"
  + "       P.DPTCD DEPARTMENTCODE, " + "\n"
  + "       com_codename('DEPT', p.dptcd, '') DEPARTMENTCODENAME, " + "\n"
  + "       P.COLUR COLORCODE, " + "\n"
  + "       com_codename('CODE#', P.COLUR, '') COLORCODENAME, " + "\n"
  + "       P.ISIZE SIZECODE, " + "\n"
  + "       com_codename('CODE#', P.ISIZE, '') SIZECODENAME, " + "\n"
  + "       ' ' PRODUCTABCINDICATIONCODE, " + "\n"
  + "       ' ' PRODUCTABCINDICATIONCODENAME, " + "\n"
  + "       P.GRPNM PRODUCTGROUPCODE, " + "\n"
  + "       com_codename('CODE#', P.GRPNM, '') PRODUCTGROUPCODENAME, " + "\n"
  + "       P.PCODE PRINCIPLECODE, " + "\n"
  + "       com_codename('CODE#', P.PCODE, '') PRINCIPLECODENAME, " + "\n"
  + "       ' ' DIVISIONCODE, " + "\n"
  + "       ' ' DIVISIONCODENAME, " + "\n"
  + "       ' ' VARIANTCODE, " + "\n"
  + "       ' ' VARIANTCODENAME, " + "\n"
  + "       '' MARKETINGTEAMCODE, " + "\n"
  + "       ' ' MARKETINGCODENAME, " + "\n"
  + "       ' ' SHELFLIFEMAXIMUM, " + "\n"
  + "       ' '  SHELFLIFEMINIMUM, " + "\n"
  + "       ' ' SHELFLIFEREMAININGFORSALE,' ' SHELFLIFEREMAININGFORPRODUCTIO, " + "\n"
  + "       P.ORDBY SORTINGORDER, " + "\n"
  + "       'N' ISTHISSERVICEITEM, " + "\n"
  + "       ' ' ISTHISMASTERPRODUCT, " + "\n"
  + "       ' ' ISTHISITEM_SALEABLE, " + "\n"
  + "       ' ' ISTHISITEM_PURCHASEABLE, " + "\n"
  + "       ' ' MAINTAININVENTORY, " + "\n"
  + "       ' ' ISTHISITEM_PURCHASEABLE, " + "\n"
  + "       ' ' ISTHISFIXEDASSETITEM, " + "\n"
  + "       ' ' MAINTAININVENTORY, " + "\n"
  + "       ' ' DOYOUMAKETHISITEM, " + "\n"
  + "       ' ' MAINTAINBATCHNOS, " + "\n"
  + "       ' ' MAINTAINSERIALNOS, " + "\n"
  + "       ' ' MAINTAINSERIALNOS, " + "\n"
  + "       ' ' ISPRICEALREADYPRINTED, " + "\n"
  + "       ' ' SEPERATEPRICEFORCHILDBARCODE, " + "\n"
  + "       ' ' ISTHISFREEFLOWPRODUCT, " + "\n"
  + "       ' ' AUTOPOSTTOINSPECTION, " + "\n"
  + "       ' ' PIECESINACARTON, " + "\n"
  + "       ' ' PIECESINAPACK, " + "\n"
  + "       ' ' PERPIECEGRAMAGEORMILILITRE, " + "\n"
  + "       '  ' REGISTRATIONNO, " + "\n"
  + "       P.WEGHT GROSSWEIGHT, " + "\n"
  + "       P.UOM_W UOMCODEGROSSWEIGHT, " + "\n"
  + "       P.WEGHT NETWEIGHT, " + "\n"
  + "       ' ' VOLUME, " + "\n"
  + "       ' ' UOMCODEVOLUME, " + "\n"
  + "       ' ' PRODUCTHEIGHT, " + "\n"
  + "       ' ' PRODUCTDEPTH, " + "\n"
  + "       ' ' PARENTKEY, " + "\n"
  + "       ' ' PRINTPRICEONBARCODE, " + "\n"
  + "       ' ' PRINTSHORTNAMEONSLIP, " + "\n"
  + "       P.ORD_Q MAXIMUMORDERQUANTITY, " + "\n"
  + "       P.ORD_Q MINIMUMORDERQUANTITY, " + "\n"
  + "       P.REO_Q REORDERLEVELQUANTITY, " + "\n"
  + "       P.MIN_Q MINIMUMQUANTITY, " + "\n"
  + "       P.MAX_Q MAXIMUMQUANTITY, " + "\n"
  + "       ' ' PROCUREMENTGROUPCODE, " + "\n"
  + "       ' ' POTOLERENCE, " + "\n"
  + "       ' ' LEADTIME_TRANSFER, " + "\n"
  + "       ' ' LEADTIME_PURCHASES,' ' LEADTIME_SALES, " + "\n"
  + "       ' ' LEADTIME_PRODUCTION, " + "\n"
  + "       P.PART# ARTICLE_PARTNO, " + "\n"
  + "       P.CATLG CATALOG_DESIGNNO, " + "\n"
  + "       P.UOM_D UNITOFMEASURECODE_PURCHASES, " + "\n"
  + "       P.UOM_D UNITOFMEASURECODE_INVENTORY, " + "\n"
  + "       p.uom_d UNITOFMEASURECODE_SALES " + "\n"
  + " " + "\n"
  + "  from str_purord_2 pod " + "\n"
  + " inner join str_purord_1 po " + "\n"
  + " " + "\n"
  + "on po.po_no = pod.po_no " + "\n"
  + " inner join SAL_PRODS_BARCODE_LIST p on p.ITEMS = pod.Items " + "\n"
  + " where po.po_no = '{0}'";
        private static readonly string SELECT_TRFOUTDETAIL = @"select LINE# DETAIL_ID, 
         sto2.items DETAIL_PRODUCTCODE,
         sto2.barcode DETAIL_PRODUCTBARCODE,
         ' ' DETAIL_PRICECODE,
         sto2.costp DETAIL_COSTPRICE,
         sto2.tradp DETAIL_SALESPRICE,
         '' DETAIL_RETAILPRICE,
         sto2.out_q DETAIL_OUTQTY,
         '' DETAIL_ORDERVALUE,
         ' ' DETAIL_UOMCODE,
         ' ' DETAIL_UOMCODENAME,
         ' ' DETAIL_NEEDBYDATE,
         sto2.qty_in DETAIL_INHANDQTY,
         sto2.out_q DETAIL_REQUIREDQTY,
         ' ' DETAIL_REFERENCETABLECODE,
         sto2.out_# DETAIL_REFERENCEDOCUMENTID, 
         ' ' DETAIL_PACKINGDETAILSCODE,
         ' ' DETAIL_PACKAGINGDETAILS,
         ' ' DETAIL_DETAILS,
         ' ' DETAIL_ORDERQTY1,
         ' ' DETAIL_ORDERQTY2,
         ' ' DETAIL_ORDERQTY3,
         ' ' DETAIL_COSTPRICE1,
         ' ' DETAIL_COSTPRICE2,
         ' ' DETAIL_COSTPRICE3,
         '' DETAIL_FREEOFCOSTQTY,
         '' DETAIL_DISCOUNTAMOUNT,
         '' DETAIL_GROSSVALUEAFTERDISCOUNT,
         ' ' DETAIL_CALCSALESTAXFOCQTY,
         '' DETAIL_SALESTAXPER,
         '' DETAIL_SALESTAXAMOUNT,
         '' DETAIL_EXCISEDUTYPER,
         '' DETAIL_EXCISEDUTYAMOUNT,
         '' DETAIL_EXTRADISCOUNTPER,
         '' DETAIL_EXTRADISCOUNTAMOUNT,
         '' DETAIL_COMMISIONPER,
         '' DETAIL_COMMISIONAMOUNT,
         '' DETAIL_CUSTOMDUTYPER,
         '' DETAIL_CUSTOMDUTYAMT,
         p.items PRODUCT_ID,
         p.descr PRODUCT_NAME,
         p.items PRODUCT_ITEMCODE,
         p.barcd PRODUCT_PRODUCTBARCODE,
         p.imported ISTHISIMPORTEDITEM,
         ' ' OPENINGDATE,
         ' ' CLOSINGDATE,
         ' ' PRODUCTCLASSIFICATIONCODE,
         '' PRODUCTCLASSFICATIONCODENAME,
         ' ' PRODUCTHIERARCHYCODE,
         ' ' PRODUCTHIERARCHYNAME,
         p.brand BRANDCODE,
         com_codename('CODE#', p.brand, '') PRODUCTBRANDNAME, 
         P.DPTCD DEPARTMENTCODE,
         com_codename('DEPT', p.dptcd, '') DEPARTMENTCODENAME, 
         P.COLUR COLORCODE,
         com_codename('CODE#', P.COLUR, '') COLORCODENAME, 
         P.ISIZE SIZECODE,
         com_codename('CODE#', P.ISIZE, '') SIZECODENAME, 
         ' ' PRODUCTABCINDICATIONCODE, 
         ' ' PRODUCTABCINDICATIONCODENAME, 
         P.GRPNM PRODUCTGROUPCODE,
         com_codename('CODE#', P.GRPNM, '') PRODUCTGROUPCODENAME, 
         P.PCODE PRINCIPLECODE,
         com_codename('CODE#', P.PCODE, '') PRINCIPLECODENAME, 
         ' ' DIVISIONCODE, 
         ' ' DIVISIONCODENAME, 
         ' ' VARIANTCODE, 
         ' ' VARIANTCODENAME, 
         '' MARKETINGTEAMCODE, 
         ' ' MARKETINGCODENAME, 
         ' ' SHELFLIFEMAXIMUM, 
         ' '  SHELFLIFEMINIMUM, 
         ' ' SHELFLIFEREMAININGFORSALE,' ' SHELFLIFEREMAININGFORPRODUCTIO, 
         P.ORDBY SORTINGORDER,
         'N' ISTHISSERVICEITEM, 
         ' ' ISTHISMASTERPRODUCT, 
         ' ' ISTHISITEM_SALEABLE, 
         ' ' ISTHISITEM_PURCHASEABLE, 
         ' ' MAINTAININVENTORY, 
         ' ' ISTHISITEM_PURCHASEABLE, 
         ' ' ISTHISFIXEDASSETITEM, 
         ' ' MAINTAININVENTORY, 
         ' ' DOYOUMAKETHISITEM, 
         ' ' MAINTAINBATCHNOS, 
         ' ' MAINTAINSERIALNOS, 
         ' ' MAINTAINSERIALNOS, 
         ' ' ISPRICEALREADYPRINTED, 
         ' ' SEPERATEPRICEFORCHILDBARCODE, 
         ' ' ISTHISFREEFLOWPRODUCT, 
         ' ' AUTOPOSTTOINSPECTION, 
         ' ' PIECESINACARTON, 
         ' ' PIECESINAPACK, 
         ' ' PERPIECEGRAMAGEORMILILITRE, 
         '  ' REGISTRATIONNO, 
         P.WEGHT GROSSWEIGHT,
         P.UOM_W UOMCODEGROSSWEIGHT,
         P.WEGHT NETWEIGHT,
         ' ' VOLUME, 
         ' ' UOMCODEVOLUME, 
         ' ' PRODUCTHEIGHT, 
         ' ' PRODUCTDEPTH, 
         ' ' PARENTKEY, 
         ' ' PRINTPRICEONBARCODE, 
         ' ' PRINTSHORTNAMEONSLIP, 
         P.ORD_Q MAXIMUMORDERQUANTITY,
         P.ORD_Q MINIMUMORDERQUANTITY,
         P.REO_Q REORDERLEVELQUANTITY,
         P.MIN_Q MINIMUMQUANTITY,
         P.MAX_Q MAXIMUMQUANTITY,
         ' ' PROCUREMENTGROUPCODE, 
         ' ' POTOLERENCE, 
         ' ' LEADTIME_TRANSFER, 
         ' ' LEADTIME_PURCHASES,' ' LEADTIME_SALES, 
         ' ' LEADTIME_PRODUCTION, 
         P.PART# ARTICLE_PARTNO, 
         P.CATLG CATALOG_DESIGNNO,
         P.UOM_D UNITOFMEASURECODE_PURCHASES,
         P.UOM_D UNITOFMEASURECODE_INVENTORY,
         p.uom_d UNITOFMEASURECODE_SALES


    from str_trf_out2@getho sto2
   inner join str_trf_out1@ho sto1

  on sto1.out_# = sto2.out_# 
   inner join sal_products p on p.items = sto2.items
   where sto1.out_# = '{0}'";


        private static readonly string SELECT_INDDETAIL = @"SELECT K.BARCODE,K.ITEMS PID,IND_Q QTY,K.DESCR NAME FROM STR_INDENT_2 S  INNER JOIN SAL_PRODS_BARCODE_LIST K ON K.BARCODE = S.BARCODE  WHERE S.IND_# ='{0}'";

        private static readonly string SELECT_ITEM_BY_BARCODE_ON_LOCATION = "SELECT BARCODE Barcode, DESCR \"Item Name\", " +
            "SAL_CODENAME('DPTCD',DPTCD,NULL) AS \"Department\", SAL_CODENAME('ICLAS',ICLAS,NULL) AS \"Master Category\", " +
            "SAL_CODENAME('MCLAS',ICLAS,NULL) AS \"Category\", " +
            "SAL_ITEMRATE('TRADE', ITEMS, SYSDATE,'{1}','{0}','%') \"Retail Price\",'' AS \"Branch Name\", -999 AS \"Available Qty\" " +
            "FROM SAL_PRODS_BARCODE_LIST WHERE UPPER(BARCODE) = '{0}' AND STATS = 'A' " +
            "UNION ALL SELECT '','','','','',-999, COM_CODENAME('BRNCH',SSP.Brnch,NULL), Nvl(Sum(SSP.Closing_q),0) + Nvl(SSV.TRANS,0) " +
                  "FROM  Sal_Products SP , Str_Stockpos SSP , " +
                       "(SELECT SSV.Items,(Sum(SSV.Qty_In) - Sum(SSV.Qty_Out)) \"TRANS\" FROM Str_Stock_View SSV  " +
                         "WHERE SSV.Wdate BETWEEN To_Date('01'||To_Char(Sysdate,'MMYYYY'), 'DDMMYYYY') AND Sysdate " +
                           //"--AND Brnch = '001'  "+
                           "Group by items) SSV	 " +
                                 "WHERE  SP.Items = SSP.Items(+) " +
                                   "AND  SSP.Mnths(+) = To_Char( Add_Months(Sysdate,-1),'YYYYMM') " +
                                   "AND  SP.Items = SSV.Items(+) " +
                   //"--AND  SSP.BRNCH(+)    = '001' "+
                   "AND  UPPER(SP.BARCD) = '{0}' " +
                 "GROUP BY  SP.Items,SSP.Brnch,SSV.TRANS ";
        private static readonly string SELECT_STOCKOUT_MASTER1 = "SELECT SM.OUT_# ID,SM.COMPC COMPANYCODE, com_codename('COMPC', SM.COMPC ,'') AS COMPANYNAME, " + "\n"
+ "                  SM.OUT_#   DOCUMENTNO, '' SALESORGCODE,'' AS SALESORGCODENAME, " + "\n"
+ "                sm.brnch      OFFICECODE, com_codename('BRNCH', sm.brnch ,'') AS OFFICECODENAME,out_# DOCUMENTNO, " + "\n"
+ "                      TO_CHAR(wdate,'DD-MON-YYYY') AS WORKDATE,'0' TRANSFERTYPE, " + "\n"
+ "                     'OUT' AS TRANSFERTYPENAME, " + "\n"
+ "                     sm.godwn WAREHOUSECODE, (select descr from str_go_downs j where j.godwn = sm.godwn ) AS WAREHOUSENAME, " + "\n"
+ "                    sm.usrid USERCODESENTBY, (select descr from sec_username m where m.usrid = sm.usrid ) AS USERCODESENTBYNAME, " + "\n"
+ "                   sm.str_t   OFFICECODESENTTO, com_codename('BRNCH', sm.snd_b ,'') AS OFFICESENTTONAME, " + "\n"
+ "                  sm.dmg_code   STOCKSTATUSCODE, '' STOCKOUTTYPECODE, '' AS STOCKSTATUSTYPECODENAME, " + "\n"
+ "                    '' REFERENCENO,sm.notes DETAILS, ''EXTRAFIELDS01, " + "\n"
+ "                   ''  EXTRAFIELDS02,'' EXTRAFIELDS03,'' EXTRAFIELDS04, ''EXTRAFIELDS05,'' EXTRAFIELDS06,'' EXTRAFIELDS07, " + "\n"
+ "                  ''    EXTRAFIELDS08,'' EXTRAFIELDS09,'' EXTRAFIELDS10,'' MAPCODE,'' REFERENCETABLECODE, ''REFERENCEDOCUMENTID " + "\n"
+ "                FROM str_trf_out1@getho SM " + "\n"
+ "                WHERE sm.compc  = '{1}'  AND sm.out_#  ='{0}' AND sm.str_t ='{3}'   and sm.out_# not in (SELECT S.OUT_# FROM  STR_TRF_INS1 S )  ";
        private static readonly string SELECT_STOCKOUT_MASTER = @"  SELECT SM.OUT_# ID,SM.COMPC COMPANYCODE, com_codename('COMPC', SM.COMPC ,'') AS COMPANYNAME, 
                  SM.OUT_#   DOCUMENTNO, '' SALESORGCODE,'' AS SALESORGCODENAME, 
                sm.brnch OFFICECODE, com_codename('BRNCH', sm.brnch ,'') AS OFFICECODENAME, sm.out_# DOCUMENTNO, 
                      TO_CHAR(sm.wdate,'DD-MON-YYYY') AS WORKDATE,'0' TRANSFERTYPE, 
                     'OUT' AS TRANSFERTYPENAME,
                     sm.godwn WAREHOUSECODE, (select descr from str_go_downs j where j.godwn = sm.godwn ) AS WAREHOUSENAME,
                    sm.usrid USERCODESENTBY, (select descr from sec_username m where m.usrid = sm.usrid ) AS USERCODESENTBYNAME,
                   sm.str_t OFFICECODESENTTO, com_codename('BRNCH', sm.snd_b, '') AS OFFICESENTTONAME,
                sm.dmg_code STOCKSTATUSCODE, '' STOCKOUTTYPECODE, '' AS STOCKSTATUSTYPECODENAME,
                    '' REFERENCENO,sm.notes DETAILS, ''EXTRAFIELDS01, 
                   ''  EXTRAFIELDS02,'' EXTRAFIELDS03,'' EXTRAFIELDS04, ''EXTRAFIELDS05,'' EXTRAFIELDS06,'' EXTRAFIELDS07, 
                  ''    EXTRAFIELDS08,'' EXTRAFIELDS09,'' EXTRAFIELDS10,'' MAPCODE,'' REFERENCETABLECODE, ''REFERENCEDOCUMENTID
                FROM str_trf_out1@getho SM
               --LEFT JOIN   STR_TRF_INS1 INS ON SM.OUT_# = INS.OUT_#
                WHERE 
--INS.OUT_# IS NULL  AND sm.out_# IS NOT NULL
--                 and
sm.compc  = '{1}'
                 AND sm.out_#  ='{0}' AND sm.str_t ='{3}'  ";

        private static readonly string SELECT_STOCKOUT_MASTER_CHECK = @"  SELECT SM.OUT_# ID
                FROM str_trf_out1@getho SM
                 LEFT JOIN   STR_TRF_INS1 INS ON SM.OUT_# = INS.OUT_#
                WHERE INS.OUT_# IS NULL  AND sm.out_# IS NOT NULL
              
                 AND sm.out_#  ='{0}'  ";
        private static readonly string SELECT_STOCKin_MASTER = "SELECT SM.OUT_# ID,SM.COMPC COMPANYCODE, com_codename('COMPC', SM.COMPC ,'') AS COMPANYNAME, " + "\n"
 + "                  SM.OUT_#   DOCUMENTNO, '' SALESORGCODE,'' AS SALESORGCODENAME, " + "\n"
 + "                sm.brnch      OFFICECODE, com_codename('BRNCH', sm.brnch ,'') AS OFFICECODENAME,sm.out_# DOCUMENTNO, " + "\n"
 + "                      TO_CHAR(sm.wdate,'DD-MON-YYYY') AS WORKDATE,'0' TRANSFERTYPE, " + "\n"
 + "                     'OUT' AS TRANSFERTYPENAME, " + "\n"
 + "                     sm.godwn WAREHOUSECODE, (select descr from str_go_downs j where j.godwn = sm.godwn ) AS WAREHOUSENAME, " + "\n"
 + "                    sm.usrid USERCODERECEIVEDBY, (select descr from sec_username m where m.usrid = sm.usrid ) AS USERCODERECEIVEDBYNAME, " + "\n"
 + "                   sm.snd_t  OFFICECODERECEIVEDFROM, com_codename('BRNCH', sm.snd_t ,'') AS OFFICECODERECEIVEDFROMNAME, " + "\n"
 + "                  sm.dmg_code   STOCKSTATUSCODE, '' STOCKOUTTYPECODE, '' AS STOCKSTATUSTYPECODENAME, " + "\n"
 + "                    '' REFERENCENO,sm.notes DETAILS, ''EXTRAFIELDS01, " + "\n"
 + "                   ''  EXTRAFIELDS02,'' EXTRAFIELDS03,'' EXTRAFIELDS04, ''EXTRAFIELDS05,'' EXTRAFIELDS06,'' EXTRAFIELDS07, " + "\n"
 + "                  ''    EXTRAFIELDS08,'' EXTRAFIELDS09,'' EXTRAFIELDS10,'' MAPCODE,'' REFERENCETABLECODE, ''REFERENCEDOCUMENTID " + "\n"
 + "                FROM str_trf_ins1 SM     inner join str_trf_out1@getho jm on jm.out_# = sm.out_# " + "\n"
 + "               WHERE sm.compc  = '{0}' AND sm.brnch like '{1}' AND sm.out_#  ='{2}'";




        private static readonly string INSERT_GOODRCV1 = " insert into STR_GoodsR_1(Compc, Brnch, Usrid, Logno, Edate, Grn_#,wdate,Activ,Dtype,Rec_b,str_f,Godwn,supcd,Pterm,curcd,curat,Dchno,bilcm,bilty,Truck,notes,Aprov,update_tp,Idate,bilds,frght,bilto,prnst,ver_#,gtype,gc_no,bladj)"
+ "values('{1}','{2}','{2}','{3}', sysdate,'{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}')";

        private static readonly string INSERT_GOODRCV1COMP = "   insert into STR_GOODSR_1(COMPC, BRNCH, USRID, LOGNO, EDATE, GRN_#, WDATE, ACTIV, DTYPE, REC_B, STR_F, GODWN, SUPCD, PTERM, CURCD, CURAT, DCHNO, BILCM, BILTY, TRUCK, NOTES, APROV, UPDATE_TP, IDATE, BILDS, BILDT, FRGHT, BILTO, PRNST, VER_#, GTYPE, GC_NO, BLADJ,REFNO) "
+ " values ('{0}', '{1}', '{2}', '{3}', sysdate, '{4}', to_date('{5}', 'dd-Mon-yyyy'), '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', {14}, '{15}', '{16}', '{17}', '{18}', {19}, '{20}', '{21}', to_date('{22}','dd-Mon-yyyy'),{23} ,to_date('{24}','dd-Mon-yyyy'), {25}, '{26}', '{27}', '{28}','{29}', '{30}', {31},'{32}') ";
        private static readonly string MERGE_GOODRCV1COMP = "MERGE INTO STR_GOODSR_1 D " + "\n"
+ "   USING (select   '{0}' COMPC, '{1}' BRNCH, '{2}' USRID, '{3}' LOGNO, sysdate EDATE, '{4}' GRN_#, to_date('{5}', 'dd-Mon-yyyy') WDATE, '{6}'ACTIV, '{7}'DTYPE, '{8}'REC_B, " + "\n"
+ "    '{9}' STR_F, '{10}' GODWN, '{11}' SUPCD, '{12}' PTERM, '{13}' CURCD, '{14}' CURAT, '{15}'DCHNO, '{16}'BILCM, '{17}' BILTY,  '{18}' TRUCK, '{19}' NOTES, " + "\n"
+ "     '{20}' APROV, '{21}' UPDATE_TP, to_date('{22}','dd-Mon-yyyy') IDATE,'{23}' BILDS,to_date('{24}','dd-Mon-yyyy') BILDT, '{25}' FRGHT, '{26}' BILTO, '{27}' PRNST, " + "\n"
+ "      '{28}' VER_#,'{29}' GTYPE, '{30}' GC_NO, '{31}' BLADJ,'{32}' REFNO , '{33}' pono ,'{34}' POSTED from dual) S " + "\n"
+ "   ON (D.refno = S.refno) " + "\n"
+ "   WHEN MATCHED THEN UPDATE SET " + "\n"
+ "    " + "\n"
+ "   d.logno=s.LOGNO, d.EDATE=sysdate , d.GRN_#=GRN_#, " + "\n"
+ "   d.WDATE=s.wdate, d.ACTIV=s.activ,d.dtype=s.DTYPE,d.rec_b=s.REC_B, d.str_f=s.STR_F, d.godwn=s.GODWN, d.supcd=s.SUPCD,d.PTERM=s.pterm, d.curcd=s.CURCD, d.curat=s.CURAT " + "\n"
+ "   , d.dchno=s.DCHNO, d.bilcm=s.BILCM, d.bilty=s.BILTY, d.truck=s.TRUCK, d.notes=s.NOTES, d.aprov=s.APROV, d.update_tp=s.UPDATE_TP, d.idate=s.IDATE, " + "\n"
+ "   d.bilds=s.BILDS, d.bildt=s.BILDT, d.frght=s.FRGHT, d.bilto=s.BILTO,d.prnst=s.PRNST, d.VER_#=s.ver_#, d.gtype=s.GTYPE, d.gc_no=s.GC_NO, d.bladj=s.BLADJ ,d.pono=s.pono " + "\n"
+ "   WHEN NOT MATCHED THEN " + "\n"
+ "     insert (COMPC, BRNCH, USRID, LOGNO, EDATE, GRN_#, WDATE, ACTIV, DTYPE, REC_B, STR_F, GODWN, SUPCD, PTERM, CURCD, CURAT, DCHNO, BILCM, BILTY, TRUCK, NOTES, APROV, UPDATE_TP, IDATE, BILDS, BILDT, FRGHT, BILTO, PRNST, VER_#, GTYPE, GC_NO, BLADJ,REFNO,pono,POSTED,TTYPE ) " + "\n"
+ "  values ('{0}', '{1}', '{2}', '{3}', sysdate, '{4}', to_date('{5}', 'dd-Mon-yyyy'), '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', {14}, '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', to_date('{22}','dd-Mon-yyyy'),{23} ,to_date('{24}','dd-Mon-yyyy'), {25}, '{26}', '{27}', '{28}','{29}', '{30}', {31},'{32}','{33}','{34}','HHTMOB')";
        private static readonly string INSERT_GOODRCV2COMP = "       insert into STR_GOODSR_2(GRN_#, USRID, LOGNO, EDATE, INS_#, POL_#, LINE#, ITEMS, BATCH, EXPDT, RAT_G, DIS_P, COM_P, GST_P, SED_P, CDT_P, EXT_P, DIS_A, COM_A, GST_A, SED_A, EXT_A, CDT_A, RAT_N, ITMRT, REQ_Q, REC_Q, BON_Q, AMONT, NOTES, GROSV, GST_B, GST_W, EDAGST_P, EDAGST_A, MARGIN_P, MARGIN_A, GST_FOC, MARGIN_T, APPLY_ON_ALL_ITEMS, SHARE_FORMULA, FOC_COST_SHARE, BARCODE, BARCODE_QTY, LAST_COST, PACK_QTY, UNIT_QTY, PACK_RATE, GST_I, CONSM, MRP_G, FRGHT_ITEMS, EXPFN, RBT_P, RBT_A, RAT_BR, AMT_AR, GPL_#, VER_#, RAT_F, RATE#,REFNO)"
+ "values ('{0}', '{1}', '{2}', sysdate, '{3}', '{4}', '{5}', '{6}', '{7}', to_date('{8}', 'dd-Mon-yyyy'), {9}, {10}, {11}, {12}, {13},{14} ,{15} , {16}, {17}, {18},{19}, {20},{21} , {22}, {23},{24} , {25}, {26}, {27}, '{28}', {29}, '{30}', '{31}', {32}, {33},{34}, {35}, '{36}', {37}, '{38}', '{39}',{40} , '{41}',{42} , {43},{44} ,{45} ,{46} ,'{47}', {48},{49} ,{50} ,{51} ,{52} ,{53} , {54},{55} ,'{56}', '{57}', {58}, '{59}','{60}')";
        private static readonly string INSERT_GOODRCV2 = " insert into STR_GoodsR_2(Grn_#,Usrid,Logno,Edate,Ins_#,Pol_#,Line#,Items,batch,expdt,Rat_g,dis_p,com_p,gst_p,sed_p,Cdt_p,ext_p,dis_A,Com_a,Gst_a,Sed_a,Ext_a,Cdt_a,rat_n,Itmrt,req_q,bon_q,Amont,notes,grosv,gst_b,gst_w,edagst_p,margin_p,Gst_Foc,margin_t,Apply_On_All_Items,Share_Formula,Foc_Cost_Share,Barcode,Barcode_Qty "
+ ", Last_Cost, Pack_Qty, Unit_Qty, Pack_Rate, Gst_i, Consm, Mrp_g, Frght_Items, Expfn, Rbt_p, Rbt_a, Rat_br, Amt_Ar, Gpl_#,ver_#,Rat_f,rate#)"
+ "values('{0}','{1}','{2}', sysdate,'{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}','{33}','{34}','{35}','{37}','{38}','{39}','{40}'"
+ ",'{41}','{42}','{43}','{44}','{45}','{46}','{47}','{48}','{49}','{50}','{51}','{52}','{53}','{54}','{55}','{56}','{57}')";


        private static readonly string MERGE_GOODRCV2COMP = "MERGE INTO STR_GOODSR_2 D " + "\n"
+ "   USING (SELECT  '{0}' GRN_#, '{1}' USRID, '{2}' LOGNO, sysdate EDATE, '{3}' INS_#,  '{4}' POL_#, '{5}' LINE#, '{6}' ITEMS, '{7}' BATCH, to_date('{8}', 'dd-Mon-yyyy') EXPDT, " + "\n"
+ " {9} RAT_G, {10} DIS_P, {11} COM_P, {12} GST_P, {13} SED_P,{14} CDT_P,{15}  EXT_P, {16} DIS_A, {17} COM_A, {18} GST_A,{19} SED_A, {20} EXT_A ,{21} CDT_A , {22} MARGIN_P , " + "\n"
+ "  {23} RAT_N,{24} ITMRT, {25} REQ_Q, {26} REC_Q, {27} BON_Q, {28} AMONT, '{29}' NOTES, '{30}' GROSV, '{31}' GST_B, '{32}' GST_W, {33} EDAGST_P,{34} EDAGST_A, " + "\n"
+ "     {35} MARGIN_A,   '{36}' GST_FOC, {37} MARGIN_T,  '{38}' APPLY_ON_ALL_ITEMS,  '{39}' SHARE_FORMULA, {40} FOC_COST_SHARE , '{41}' BARCODE, {42} BARCODE_QTY, {43} LAST_COST " + "\n"
+ "   ,{44}  PACK_QTY,{45} UNIT_QTY,{46} PACK_RATE,'{47}' GST_I, {48} CONSM,{49} MRP_G,{50}  FRGHT_ITEMS,{51}  EXPFN,{52} RBT_P ,{53}  RBT_A " + "\n"
+ "   , {54} RAT_BR,{55}AMT_AR ,'{56}' GPL_#, '{57}' VER_# , {58} RAT_F, '{59}' RATE#,'{60}' REFNO FROM DUAL) S " + "\n"
+ "   ON (D.refno = S.refno) " + "\n"
+ "   WHEN MATCHED THEN UPDATE SET " + "\n"
+ "    " + "\n"
+ " d.GRN_#=s.GRN_#, d.usrid=s.USRID, d.logno=d.LOGNO, EDATE=sysdate ,d.ins_#=s.INS_#, d.pol_#=s.POL_#, d.line#=s.LINE#, d.items=s.ITEMS, d.batch=s.BATCH, " + "\n"
+ " d.expdt=s.EXPDT, d.rat_g=s.RAT_G, d.dis_p=s.DIS_P, d.com_p=s.COM_P, d.gst_p=s.GST_P, d.sed_p=s.SED_P, d.cdt_p=s.CDT_P, d.ext_p=s.EXT_P,d.dis_a=s.DIS_A, " + "\n"
+ " d.com_a=s.COM_A,d.gst_a=s.GST_A, d.sed_a=s.SED_A, d.ext_a=s.EXT_A, d.cdt_a=s.CDT_A, d.rat_n=s.RAT_N, d.itmrt=s.ITMRT, d.req_q=s.REQ_Q, d.rec_q=s.REC_Q, " + "\n"
+ " d.bon_q=s.BON_Q, d.amont=s.AMONT, d.notes=s.NOTES, d.grosv=s.GROSV, d.gst_b=s.GST_B, d.gst_w=s.GST_W, d.edagst_p=s.EDAGST_P,d.edagst_a=s.EDAGST_A, d.margin_p=s.MARGIN_P, " + "\n"
+ " d.margin_a=s.MARGIN_A,d.gst_foc=s.GST_FOC,d.margin_t=s.MARGIN_T, d.apply_on_all_items=s.APPLY_ON_ALL_ITEMS,d.share_formula=s.SHARE_FORMULA,d.foc_cost_share=s.FOC_COST_SHARE, " + "\n"
+ " d.barcode=s.BARCODE, d.BARCODE_QTY=s.BARCODE_QTY, d.last_cost=s.LAST_COST, d.pack_qty=s.PACK_QTY, d.unit_qty=s.UNIT_QTY, d.pack_rate=s.PACK_RATE, d.gst_i=s.GST_I, " + "\n"
+ "  d.consm=s.CONSM, d.mrp_g=d.MRP_G, d.frght_items=s.FRGHT_ITEMS, d.expfn=s.EXPFN, d.rbt_p=s.RBT_P, d.rbt_a=s.RBT_A, d.rat_br=s.RAT_BR,d.amt_ar=s.AMT_AR, " + "\n"
+ "  d.gpl_#=s.GPL_#, d.ver_#=s.VER_#, d.rat_f=s.RAT_F, d.rate#=s.RATE# " + "\n"
+ "   " + "\n"
+ "  " + "\n"
+ "   WHEN NOT MATCHED THEN " + "\n"
+ "          insert (GRN_#, USRID, LOGNO, EDATE, INS_#, POL_#, LINE#, ITEMS, BATCH, EXPDT, RAT_G, DIS_P, COM_P, GST_P, SED_P, CDT_P, EXT_P, DIS_A, COM_A, GST_A, SED_A, EXT_A, CDT_A,MARGIN_P, RAT_N, ITMRT, REQ_Q, REC_Q, BON_Q, AMONT, NOTES, GROSV, GST_B, GST_W, EDAGST_P, EDAGST_A, MARGIN_A, GST_FOC, MARGIN_T, APPLY_ON_ALL_ITEMS, SHARE_FORMULA, FOC_COST_SHARE, BARCODE, BARCODE_QTY, LAST_COST, PACK_QTY, UNIT_QTY, PACK_RATE, GST_I, CONSM, MRP_G, FRGHT_ITEMS, EXPFN, RBT_P, RBT_A, RAT_BR, AMT_AR, GPL_#, VER_#, RAT_F, RATE#,REFNO) " + "\n"
+ " values ('{0}', '{1}', '{2}', sysdate, '{3}', '{4}', '{5}', '{6}', '{7}', to_date('{8}', 'dd-Mon-yyyy'), {9}, {10}, {11}, {12}, {13},{14} ,{15} , {16}, {17}, {18},{19}, {20},{21} , {22}, {23},{24} , {25}, {26}, {27}, {28}, '{29}', '{30}', '{31}', '{32}', {33},{34}, {35}, '{36}', {37}, '{38}', '{39}',{40} , '{41}',{42} , {43},{44} ,{45} ,{46} ,'{47}', {48},{49} ,{50} ,{51} ,{52} ,{53} , {54},{55} ,'{56}', '{57}', {58}, '{59}','{60}')";

        private static readonly string SELECT_PREVIOUSGRN = "SELECT S.GRN_# FROM STR_GOODSR_1 S WHERE S.refno ='{0}'";
        private static readonly string SELECT_PREVIOUSGRN2 = "SELECT S.line# FROM STR_GOODSR_2 S WHERE S.refno ='{0}'";


        private static readonly string SELECT_PREVIOUSGRNAPP = "SELECT S.APROV FROM STR_GOODSR_1 S WHERE S.Grn_# ='{0}'";



        // Fixed. Barcode, Descr, Trade column sequence can't be change!
        private static readonly string SELECT_ITEM_PRINT_INFO_BY_BARCODE = "SELECT BARCODE, DESCR, TRADE FROM SAL_PRODS_BARCODE_LIST WHERE UPPER(BARCODE) = '{0}' AND STATS ='A'";
        private static readonly string SELECT_BARCODE_PRINTER_NAME = "SELECT * FROM COM_DEFAULTS WHERE PCODE ='WCE_PRINT'";

        private static readonly string SELECT_MAX_HTT_RECEVING_KEY = "SELECT (LPAD(NVL(MAX(TO_NUMBER(SUBSTR(LINE#,0,8))),0) +1,8,0) || '.' || '{0}' || '.' || '{1}') FROM HHT_RECEIVING_1 WHERE COMPC ='{0}' AND BRNCH = '{1}'";

        private static readonly string INSERT_HHT_RECEIVING = "INSERT INTO HHT_RECEIVING_1 (LINE#, COMPC, BRNCH, USRID, EDATE, CUSTM, DTYPE, STATS, PO#) " +
            "VALUES ('{0}', '{1}', '{2}', '{3}', SYSDATE, '{4}', '{5}', 'O', '{6}')";

        private static readonly string UPDATE_HHT_RECEIVING = "UPDATE HHT_RECEIVING_1  SET COMPC = '{1}' , BRNCH = '{2}' , USRID = '{3}' , EDATE =  SYSDATE, CUSTM = '{4}' , STATS = 'O' , PO# = '{5}' WHERE LINE# = '{0}'  ";


        private static readonly string CLOSE_HHT_RECEIVING_1 = "UPDATE HHT_RECEIVING_1 SET STATS = 'C' WHERE LINE# ='{0}'";

        private static readonly string SELECT_HHT_RECEIVING_1_MASTER_INFO = "SELECT COMPC, BRNCH , USRID, DTYPE FROM HHT_RECEIVING_1 WHERE LINE# = '{0}'"; //CS

        private static readonly string PARTIAL_CLOSE_HHT_RECEIVING_1 = "UPDATE HHT_RECEIVING_1 SET STATS = 'M' WHERE LINE# ='{0}'";

        private static readonly string SELECT_ITEM_BARCODE_PRINT_HISTORY_KEY = "SELECT (LPAD(NVL(MAX(TO_NUMBER(SUBSTR(LINE#,0,8))),0) +1,8,0) || '.' || '{0}' || '.' || '{1}') FROM ITEM_BARCODE_PRINT_HISTORY WHERE COMPC ='{0}' AND BRNCH = '{1}'";

        private static readonly string INSERT_ITEM_BARCODE_PRINT_HISTORY = "INSERT INTO ITEM_BARCODE_PRINT_HISTORY " +
            "(LINE#, COMPC, BRNCH, USRID, EDATE, BARCD) VALUES ('{0}','{1}','{2}','{3}',SYSDATE,'{4}')";

        private static readonly string INSERT_ITEM_BARCODE_PRINT_REQUEST = "INSERT INTO ITEM_BARCODE_PRINT_HISTORY " +
            "(LINE#, COMPC, BRNCH, USRID, EDATE, BARCD, QTY, EXPIRY_DATE) VALUES ('{0}','{1}','{2}','{3}',SYSDATE,'{4}','{5}','{6}')";

        private static readonly string SELECT_MAX_HHT_RECEIVING_2 = "SELECT (LPAD(NVL(MAX(TO_NUMBER(SUBSTR(LINE#,0,8))),0) +1,8,0) || '.' || '{0}') FROM HHT_RECEIVING_2 WHERE HHT_REC_ID ='{0}'";

        private static readonly string INSERT_HHT_RECEIVING_2 = "INSERT INTO HHT_RECEIVING_2 (LINE#, HHT_REC_ID, " +
            "BARCD, QTY, ITEMS, USRID, " +
            "EDATE, PO#, CODE#, FOC, ITEM_EXPIRY,NOTES) VALUES ('{0}','{1}','{2}','{3}', " +
            "(SELECT ITEMS FROM SAL_PRODS_BARCODE_LIST WHERE BARCODE ='{2}'), '{4}',SYSDATE,'{5}','{6}','{7}', '{8}','{9}')";

        private static readonly string DELETE_HHT_RECEIVING_2 = "DELETE HHT_RECEIVING_2 WHERE HHT_REC_ID = '{0}' " +
            "AND BARCD = '{1}' AND ITEM_EXPIRY = '{2}'";

        private static readonly string DELETE_HHT_RECEIVING_2_NON_EXPIRY = "DELETE HHT_RECEIVING_2 WHERE HHT_REC_ID = '{0}' " +
            "AND BARCD = '{1}' AND ITEM_EXPIRY IS NULL";

        private static readonly string DELETE_HHT_RECEIVING_2_DAMAGE = "DELETE HHT_RECEIVING_2 WHERE HHT_REC_ID = '{0}' " +
            "AND BARCD = '{1}' AND CODE# = '{2}'";

        private static readonly string DELETE_HHT_SELECTED_ITEM = "DELETE HHT_RECEIVING_2 WHERE HHT_REC_ID = '{0}' " +
            "AND BARCD = '{1}' AND CODE# = '{2}'";

        private static readonly string DELETE_HHT_NON_EXPIRY_SELECTED_ITEM = "DELETE HHT_RECEIVING_2 WHERE HHT_REC_ID = '{0}' " +
            "AND BARCD = '{1}' AND ITEM_EXPIRY IS NULL";

        private static readonly string SELECT_PO_OF_SUPPLIER = "SELECT PO_NO FROM STR_PURORD_1 WHERE SUPCD ='{0}' AND PO_NO = '{1}' AND COMPC = '{2}' AND BRNCH ='{3}'";

        private static readonly string SELECT_SUPPLIER_OF_PO = "SELECT SUPCD FROM STR_PURORD_1 WHERE PO_NO = '{0}' AND COMPC = '{1}' AND BRNCH ='{2}'";

        private static readonly string SELECT_GRN_OF_SUPPLIER = "SELECT GRN_# AS GRN_NO FROM STR_GOODSR_1 WHERE GRN_# ='{1}' AND SUPCD ='{0}' AND COMPC ='{2}' AND BRNCH ='{3}'";

        private static readonly string SELECT_AUDIT_INFO = "SELECT AUD_# AS AUDIT_NO FROM STR_AUDIT_01 WHERE AUD_# ='{0}' AND COMPC = '{1}' AND BRNCH ='{2}' AND APROV ='P'";

        private static readonly string SELECT_PO_ITEMS = "SELECT PO_NO, ORD_Q FROM STR_PURORD_2 WHERE PO_NO ='{0}' AND UPPER(BARCODE) = '{1}'";

        private static readonly string SELECT_GRN_ITEMS = "SELECT GRN_# AS GRN_NO, REC_Q  FROM STR_GOODSR_2 WHERE GRN_# = '{0}' AND UPPER(BARCODE) = '{1}'";

        private static readonly string SELECT_AUDIT_ITEMS = "SELECT AUD_# AS AUDIT_NO, PHY_Q FROM STR_AUDIT_02 WHERE AUD_# ='{0}' AND UPPER(BARCODE) = '{1}'";

        private static readonly string SELECT_OPEN_RECEIVING_BY_USERID = "SELECT HR1.LINE# AS ID, HR1.CUSTM AS SUPPLIER, HR2.BARCD AS BARCODE, HR2.QTY, HR2.FOC, " +
            "HR2.PO# AS PNO, CODE# AS DAMAGE_TYPE, " +
            "(SELECT DESCR FROM SAL_PRODS_BARCODE_LIST BL WHERE BL.BARCODE = HR2.BARCD ) AS ITEMS, TO_CHAR(HR2.ITEM_EXPIRY, 'dd-Mon-yyyy') AS ITEM_EXPIRY  " +
            " ,HR2.NOTES NOTES " +
            "FROM HHT_RECEIVING_1 HR1 " +
            "INNER JOIN HHT_RECEIVING_2 HR2 " +
            "ON HR1.LINE# = HR2.HHT_REC_ID " +
            "WHERE HR1.STATS ='O' " +
            "AND HR1.USRID ='{0}' AND HR1.COMPC = '{1}' AND HR1.BRNCH = '{2}' AND HR1.DTYPE ='R'";

        private static readonly string SELECT_OPEN_GRN_BY_USERID = "SELECT HR1.LINE# AS ID, HR1.CUSTM AS SUPPLIER, HR2.BARCD AS BARCODE, HR2.QTY, HR2.FOC, " +
            "HR2.PO# AS PNO, CODE# AS DAMAGE_TYPE, " +
            "(SELECT DESCR FROM SAL_PRODS_BARCODE_LIST BL WHERE BL.BARCODE = HR2.BARCD ) AS ITEMS, TO_CHAR(HR2.ITEM_EXPIRY, 'dd-Mon-yyyy') AS ITEM_EXPIRY  " +
            " ,HR2.NOTES NOTES " +
            "FROM HHT_RECEIVING_1 HR1 " +
            "INNER JOIN HHT_RECEIVING_2 HR2 " +
            "ON HR1.LINE# = HR2.HHT_REC_ID " +
            "WHERE HR1.STATS ='O' " +
            "AND HR1.USRID ='{0}' AND HR1.COMPC = '{1}' AND HR1.BRNCH = '{2}' AND HR1.DTYPE ='GRT'";

        private static readonly string SELECT_OPEN_DEMAND_BY_USERID = "SELECT HR1.LINE# AS ID, HR1.CUSTM AS SUPPLIER, HR2.BARCD AS BARCODE, HR2.QTY, HR2.FOC, " +
           "HR2.PO# AS PNO, CODE# AS DAMAGE_TYPE, " +
           "(SELECT DESCR FROM SAL_PRODS_BARCODE_LIST BL WHERE BL.BARCODE = HR2.BARCD ) AS ITEMS, TO_CHAR(HR2.ITEM_EXPIRY, 'dd-Mon-yyyy') AS ITEM_EXPIRY  " +
           " ,HR2.NOTES NOTES " +
           "FROM HHT_RECEIVING_1 HR1 " +
           "INNER JOIN HHT_RECEIVING_2 HR2 " +
           "ON HR1.LINE# = HR2.HHT_REC_ID " +
           "WHERE HR1.STATS ='O' " +
           "AND HR1.USRID ='{0}' AND HR1.COMPC = '{1}' AND HR1.BRNCH = '{2}' AND HR1.DTYPE ='DMD'";


        private static readonly string SELECT_OPEN_HHT_BY_USERID = "SELECT HR1.LINE# AS ID, HR1.CUSTM AS SUPPLIER, HR2.BARCD AS BARCODE, HR2.QTY, HR2.FOC, " +
            "HR2.PO# AS PNO, CODE# AS DAMAGE_TYPE, " +
            "(SELECT DESCR FROM SAL_PRODS_BARCODE_LIST BL WHERE BL.BARCODE = HR2.BARCD ) AS ITEMS, TO_CHAR(HR2.ITEM_EXPIRY, 'dd-Mon-yyyy') AS ITEM_EXPIRY " +
            " ,HR2.NOTES NOTES " +
            "FROM HHT_RECEIVING_1 HR1 " +
            "INNER JOIN HHT_RECEIVING_2 HR2 " +
            "ON HR1.LINE# = HR2.HHT_REC_ID " +
            "WHERE HR1.STATS ='O' " +
            "AND HR1.USRID ='{0}' AND HR1.COMPC = '{2}' AND HR1.BRNCH = '{3}' AND HR1.DTYPE ='{1}'";

        private static readonly string SELECT_GRN_NO_OF_HHT_RECEIVING = "SELECT GRN_# FROM STR_GOODSR_1 WHERE BILTY  = '{0}' AND ROWNUM =1";
        private static readonly string SELECT_RETURN_NO_OF_HHT_GOOD_RETURN = "SELECT GRS_# AS RETURN_NO FROM STR_GOODRT_1 WHERE OREF# ='{0}' AND ROWNUM =1";
        private static readonly string SELECT_BRCD_BY_ITEM = "select BARCD from SAL_PRODS_BARCODE_LIST where barcode ='{0}' ";
        private static readonly string SELECT_BRCD_BY_ITEM_DPTCD = "select BARCD from sal_prods_barcode_list where   barcode='{0}' and dptcd          = (select  dptcd from str_trf_out1 s where   s.out_#   = '{1}') ";
        private static readonly string SELECT_BRCD_BY_ITEM_DPTCDIN = "select BARCD from sal_prods_barcode_list where   barcode='{0}' and dptcd          = (select  dptcd from str_trf_out1 s where   s.out_#   = '{1}') ";
        private static readonly string SELECT_MAS_BRCD_BY_ITEM = "select BARCD from SAL_PRODS_BARCODE_LIST where barcode ='{0}' ";
        
        private static readonly string SELECT_item_BY_ITEM = "select items from SAL_PRODS_BARCODE_LIST where barcode ='{0}' ";
        /// <summary>
        /// /PK Section (WAMIQ)
        /// </summary>

        private static readonly string SELECT_MAX_GRN1 = "select STR_GRN_NO('{0}', '{1}', 'R') FROM DUAL";
        private static readonly string SELECT_MAX_GRN2 = "SELECT (LPAD(NVL(MAX(TO_NUMBER(SUBSTR(LINE#,0,5))),0) +1,5,0) || '.' || '{0}')  FROM STR_GOODSR_2 WHERE  GRN_# ='{0}'";

        /// <summary>
        /// Deleting Section
        /// </summary>
        /// 
        private static readonly string DeleteGoodRcv = "delete from {2} t where t.{3} ='{0}' and t.refno not in ({1})";
        private static readonly string DeleteGoodRcvUser = "delete from {2} t where t.{3} ='{0}' and t.refno not in ({1}) and  t.userid = {4}";

        private static readonly string PurchaseOrdLine = "    select t.line# from str_purord_1 s  inner join str_purord_2  t on s.po_no =t.po_no  where s.po_no in(select pono from str_goodsr_1 where grn_#= '{0}') and items  ='{1}' and barcode='{2}'";
        private static readonly string TRanferOutLine = "    select t.line#   from str_trf_out1@getho s  inner join str_trf_out2 t on s.out_# = t.out_#  where s.out_# in (select out_# from str_trf_ins1 where tin_# = '{0}')    and items = '{1}' and barcode='{2}'";
        private static readonly string IndentLine = "    select t.line#   from str_indent_1 s  inner join str_indent_2 t on s.ind_# = t.ind_#  where t.line# in (select line# from str_trf_out2  where out_# = '{0}')    and items = '{1}' and barcode='{2}'";

        
        private static readonly string GenericDel = "delete from {1} t where t.refno ='{0}' ";
        private static readonly string SELECT_POREMAIN = @"   select sum(ord)-sum(rec) from   ( 

 select nvl(sum(q.ord_q),0) ord, 0 rec
               from str_purord_1 p, str_purord_2 q
              where p.po_no = q.po_no
                and  p.po_no = '{0}' 
     union all
   select  0 ord ,nvl(sum(g.rec_q),0) rec
               from
               str_goodsr_2 g, str_goodsr_1 h
              where
              h.grn_# = g.grn_# 
--and  h.aprov ='A' 

                and  h.pono = '{0}' 
              
                )";
        private static readonly string SELECT_CHECK_TROUT_TRIN = @"   SELECT sum(out_q)-sum(rec_q) FROM(   select nvl(sum(sto2.out_q),0) out_q ,0 rec_q  FROM str_trf_out1@getho SM,str_trf_out2 STO2
 WHERE sm.out_#=sto2.out_# and sm.compc = '1' and sm.out_#='{0}'
    union all
select nvl(sum(b.out_q),0) out_q,nvl(sum(b.rec_q),0) rec_q from    str_trf_ins1 a,str_trf_ins2 b where a.tin_#=b.tin_#
and a.out_#='{0}')";

        private static readonly string SELECT_CHECK_IND_OUT = @"   SELECT sum(out_q)-sum(rec_q) FROM(   select nvl(sum(sto2.ind_q),0) out_q ,0 rec_q  FROM STR_INDENT_1 SM,STR_INDENT_2 STO2
 WHERE sm.Ind_#=sto2.Ind_# and sm.compc = '1' and sm.Ind_#='{0}'
    union all
select nvl(sum(b.out_q),0) out_q,nvl(sum(b.out_q),0) rec_q from    STR_TRF_OUT1 a,STR_TRF_OUT2 b,STR_INDENT_2 C  where a.Out_#=b.Out_# and c.line# =b.indl# 
and c.ind_# ='{0}')";
        
        private static readonly string SELECT_TOTALINPO = @"        select  
                 nvl(sum(g.rec_q),0) rec 
                            from str_purord_1 p, str_purord_2 q ,str_goodsr_2 g,str_goodsr_1 h 
                           where p.po_no = q.po_no  and  q.line# = g.pol_#  and h.grn_# = g.grn_# -- and h.aprov ='A' 
              
                                
                            and ( q.barcode = '{1}' or q.items ='{2}') 
                            
                           and p.po_no = '{0}'
";
        private static readonly string SELECT_TROUTAPPROV = @"    SELECT * FROM str_trf_out1@getho SM, str_trf_out2@getho STO2
WHERE sm.out_#=sto2.out_# and sm.compc = '{1}'
   AND sm.brnch = '{2}'
   AND SM.Aprov='A'
   AND SM.Out_#='{0}'  ";
        private static readonly string SELECT_INDAPPROV = @"   SELECT * FROM STR_INDENT_1 SM, STR_INDENT_2 STO2
WHERE sm.IND_#=sto2.IND_# and sm.compc = '{1}'
   AND sm.brnch = '{2}'
   AND SM.Aprov='A'
   AND SM.IND_#='{0}' ";
        
        private static readonly string SELECT_GRNAPROV = @"    select *
               from str_purord_1 p, str_purord_2 q ,
              str_goodsr_1 h  , str_goodsr_2 g 
              where p.po_no = q.po_no              
              and 
              h.grn_# = g.grn_#  
             and p.aprov ='A' 
                and  p.po_no = '{0}'  ";

        private static readonly string CHECK_POEXIST = @"    select *
         from    
              str_purord_1 h -- , str_goodsr_2 g 
              where
            --  h.grn_# = g.grn_#  
              h.aprov ='A' 
            and 
( h.po_no='{0}'  ) ";
        private static readonly string SELECT_CHECKGRNAPROVAL = @"    select h.aprov
         from    
              str_goodsr_1 h--  , str_goodsr_2 g 
              where
             -- h.grn_# = g.grn_#  
             --and h.aprov !='A' 
         --   and
( h.grn_#='{0}' or h.refno='{0}' ) ";

        private static readonly string SELECT_CHECKTRFINAPROVAL = @"      select h.aprov
         from    
              str_trf_ins1 h  --, str_trf_ins2 g 
              where
             -- h.tin_# = g.tin_#  
             --and h.aprov !='A' 
           -- and
( h.tin_#='{0}' or h.refno='{0}' )";


        private static readonly string SELECT_CHECKINDENTAPROVAL = @"    select h.aprov
         from    
              str_trf_out1 h  --, str_trf_ins2 g 
              where
             -- h.tin_# = g.tin_#  
             --and h.aprov !='A' 
           -- and
( h.out_# ='{0}' or h.refno='{0}')";

        private static readonly string SELECT_CHECKOUTEXIST = @"   
select *
         from    
              str_trf_out1@getho h  --, str_trf_out2 g 
              where
             -- h.out_# = g.out_#  
           h.aprov ='A' 
          and
( h.out_#='{0}' or h.refno='{0}')
";

        private static readonly string SELECT_BARCODEINFO = @" select '' Id ,
    '' Maxid ,
    '' Timestamp ,
    '' Usercode ,
    spbl.ITEMS Mapcode ,
    '' Logincode ,
    '' Activeyn ,
    spbl.APROV Approvalyn ,
    '' Companycode ,
    spbl.items Itemcode ,
   spbl.barcode  Productbarcode ,
    DECODE(spbl.Imported,'L','N','I','Y') Isthisimporteditem ,
    spbl.descr Name ,
    spbl.shnam Shortname ,
    spbl.IS_MASTER Isthismasterproduct ,
     spbl.ITEMS Parentkey ,
   spbl.OPDAT  Openingdate ,
   spbl.CLDAT  Closingdate ,
   ''  Productclassificationcode ,
   ''  Producthierarchycode ,
 spbl.BRAND    Brandcode ,
   spbl.Dptcd  Departmentcode ,
  spbl.COLUR   Colorcode ,
  spbl.ISIZE   Sizecode ,
  ''   Productabcindicationcode ,
   ''  Productgroupcode ,
   ''  Principlecode ,
    '' Divisioncode ,
    '' Variantcode ,
    '' Principleerpproductcode ,
    '' Marketingteamcode ,
    '' Shelflifemaximum ,
    '' Shelflifeminimum ,
    '' Shelfliferemainingforsale ,
    '' Shelfliferemainingforproductio ,
    '' Sortingorder ,
    '' Isthisserviceitem ,
    spbl.SAL_Y Isthisitem_Saleable ,
   spbl.PUR_Y  Isthisitem_Purchaseable ,
   ''  Maintaininventory ,
   ''  Doyoumakethisitem ,
   '' Isthisfixedassetitem ,
   ''  Maintainbatchnos ,
   ''  Maintainserialnos ,
   ''  Ispricealreadyprinted ,
   ''  Seperatepriceforchildbarcode ,
   spbl.FREE_FLOW  Isthisfreeflowproduct ,
    '' Autoposttoinspection , 
  spbl.""Carton_Size"" Piecesinacarton ,
   spbl.""Pack_Size""  Piecesinapack ,
   ''  Perpiecegramageormililitre ,
   ''  Registrationno ,
   ''  Grossweight ,
   ''  Uomcodegrossweight ,
   spbl.WEGHT Netweight,
   ''  Volume ,
   ''  Uomcodevolume ,
  spbl.HIGHT Productheight,
  spbl.WIDTH Productwidth,
   spbl.DEPTH Productdepth,
   ''  Article_Partno ,
   spbl.CATLG Catalog_Designno,
   ''  Unitofmeasurecode_Inventory ,
   ''  Unitofmeasurecode_Purchases ,
   ''  Unitofmeasurecode_Sales ,
   ''  Leadtime_Purchases ,
   ''  Leadtime_Sales ,
   ''  Leadtime_Production ,
   ''  Leadtime_Transfer ,
  ''  Potolerence ,
  ''   Procurementgroupcode ,
  spbl.MAX_Q Minimumquantity,
 spbl.MIN_Q Maximumquantity,
 ''    Reorderlevelquantity ,
 ''    Minimumorderquantity ,
   spbl.ORD_Q Maximumorderquantity,
 spbl.TRADE productSaleRate,
 spbl.RETLP productRetailRate,
  spbl.COSTP productCostRate,
   ''  Printshortnameonslip ,
  ''   Printpriceonbarcode ,
  ''    Parentkeyname ,
  '' priceCodeDTOList,
  ''    Usercodename ,
   spbl.COMPC Companycodename,
  ''    Productclassificationcodename ,
  ''   Producthierarchycodename ,
   spbl.BRAND_NAME Brandcodename,
    spbl.DPTCD_NAME Departmentcodename,
   com_codename('CODE#', spbl.COLUR, null)    Colorcodename ,
   com_codename('CODE#', spbl.ISIZE,null)    Sizecodename ,
  '' Productabcindicationcodename ,
  ''  Productgroupcodename ,
  ''   Principlecodename ,
  ''  Divisioncodename ,
  ''    Variantcodename ,
  ''   Marketingteamcodename ,
   ''   Uomcodegrossweightname ,
   ''  Uomcodevolumename ,
   ''  UOMCode_Inventoryname,-- Unitofmeasurecode_Inventoryname ,
   ''   UOMCode_Purchasesname,--Unitofmeasurecode_Purchasesname ,
   ''   UOMCode_Salesname ,--Unitofmeasurecode_Salesname ,
  ''   priceCode,
STR_ORD_QTY('{1}',SPBL.ITEMS,SPBL.BARCODE) POQty,SAL_CODENAME('CUSTM', SPBL.PURCHASE_FROM, NULL) SUPPLIER 
,STR_ORD_QTY_REM('{1}',SPBL.ITEMS,SPBL.BARCODE) POQtyRem
      from sal_prods_barcode_list spbl WHERE SPBL.BARCODE='{0}'  ";


        private static readonly string SELECT_BARCODEINFO_OUT = @" select '' Id ,
    '' Maxid ,
    '' Timestamp ,
    '' Usercode ,
    spbl.ITEMS Mapcode ,
    '' Logincode ,
    '' Activeyn ,
    spbl.APROV Approvalyn ,
    '' Companycode ,
    spbl.items Itemcode ,
   spbl.barcode  Productbarcode ,
    DECODE(spbl.Imported,'L','N','I','Y') Isthisimporteditem ,
    spbl.descr Name ,
    spbl.shnam Shortname ,
    spbl.IS_MASTER Isthismasterproduct ,
     spbl.ITEMS Parentkey ,
   spbl.OPDAT  Openingdate ,
   spbl.CLDAT  Closingdate ,
   ''  Productclassificationcode ,
   ''  Producthierarchycode ,
 spbl.BRAND    Brandcode ,
   spbl.Dptcd  Departmentcode ,
  spbl.COLUR   Colorcode ,
  spbl.ISIZE   Sizecode ,
  ''   Productabcindicationcode ,
   ''  Productgroupcode ,
   ''  Principlecode ,
    '' Divisioncode ,
    '' Variantcode ,
    '' Principleerpproductcode ,
    '' Marketingteamcode ,
    '' Shelflifemaximum ,
    '' Shelflifeminimum ,
    '' Shelfliferemainingforsale ,
    '' Shelfliferemainingforproductio ,
    '' Sortingorder ,
    '' Isthisserviceitem ,
    spbl.SAL_Y Isthisitem_Saleable ,
   spbl.PUR_Y  Isthisitem_Purchaseable ,
   ''  Maintaininventory ,
   ''  Doyoumakethisitem ,
   '' Isthisfixedassetitem ,
   ''  Maintainbatchnos ,
   ''  Maintainserialnos ,
   ''  Ispricealreadyprinted ,
   ''  Seperatepriceforchildbarcode ,
   spbl.FREE_FLOW  Isthisfreeflowproduct ,
    '' Autoposttoinspection , 
  spbl.""Carton_Size"" Piecesinacarton ,
   spbl.""Pack_Size""  Piecesinapack ,
   ''  Perpiecegramageormililitre ,
   ''  Registrationno ,
   ''  Grossweight ,
   ''  Uomcodegrossweight ,
   spbl.WEGHT Netweight,
   ''  Volume ,
   ''  Uomcodevolume ,
  spbl.HIGHT Productheight,
  spbl.WIDTH Productwidth,
   spbl.DEPTH Productdepth,
   ''  Article_Partno ,
   spbl.CATLG Catalog_Designno,
   ''  Unitofmeasurecode_Inventory ,
   ''  Unitofmeasurecode_Purchases ,
   ''  Unitofmeasurecode_Sales ,
   ''  Leadtime_Purchases ,
   ''  Leadtime_Sales ,
   ''  Leadtime_Production ,
   ''  Leadtime_Transfer ,
  ''  Potolerence ,
  ''   Procurementgroupcode ,
  spbl.MAX_Q Minimumquantity,
 spbl.MIN_Q Maximumquantity,
 ''    Reorderlevelquantity ,
 ''    Minimumorderquantity ,
   spbl.ORD_Q Maximumorderquantity,
 spbl.TRADE productSaleRate,
 spbl.RETLP productRetailRate,
  spbl.COSTP productCostRate,
   ''  Printshortnameonslip ,
  ''   Printpriceonbarcode ,
  ''    Parentkeyname ,
  '' priceCodeDTOList,
  ''    Usercodename ,
   spbl.COMPC Companycodename,
  ''    Productclassificationcodename ,
  ''   Producthierarchycodename ,
   spbl.BRAND_NAME Brandcodename,
    spbl.DPTCD_NAME Departmentcodename,
   com_codename('CODE#', spbl.COLUR, null)    Colorcodename ,
   com_codename('CODE#', spbl.ISIZE,null)    Sizecodename ,
  '' Productabcindicationcodename ,
  ''  Productgroupcodename ,
  ''   Principlecodename ,
  ''  Divisioncodename ,
  ''    Variantcodename ,
  ''   Marketingteamcodename ,
   ''   Uomcodegrossweightname ,
   ''  Uomcodevolumename ,
   ''  UOMCode_Inventoryname,-- Unitofmeasurecode_Inventoryname ,
   ''   UOMCode_Purchasesname,--Unitofmeasurecode_Purchasesname ,
   ''   UOMCode_Salesname ,--Unitofmeasurecode_Salesname ,
  ''   priceCode,
STR_OUT_QTY('{1}',SPBL.ITEMS,SPBL.BARCODE) POQty,SAL_CODENAME('CUSTM', SPBL.PURCHASE_FROM, NULL) SUPPLIER 
,STR_OUT_QTY_Rem('{1}',SPBL.ITEMS,SPBL.BARCODE) POQtyRem
      from sal_prods_barcode_list spbl WHERE SPBL.BARCODE='{0}'";

        private static readonly string SELECT_BARCODEINFO_TIN = @" select '' Id ,
    '' Maxid ,
    '' Timestamp ,
    '' Usercode ,
    spbl.ITEMS Mapcode ,
    '' Logincode ,
    '' Activeyn ,
    spbl.APROV Approvalyn ,
    '' Companycode ,
    spbl.items Itemcode ,
   spbl.barcode  Productbarcode ,
    DECODE(spbl.Imported,'L','N','I','Y') Isthisimporteditem ,
    spbl.descr Name ,
    spbl.shnam Shortname ,
    spbl.IS_MASTER Isthismasterproduct ,
     spbl.ITEMS Parentkey ,
   spbl.OPDAT  Openingdate ,
   spbl.CLDAT  Closingdate ,
   ''  Productclassificationcode ,
   ''  Producthierarchycode ,
 spbl.BRAND    Brandcode ,
   spbl.Dptcd  Departmentcode ,
  spbl.COLUR   Colorcode ,
  spbl.ISIZE   Sizecode ,
  ''   Productabcindicationcode ,
   ''  Productgroupcode ,
   ''  Principlecode ,
    '' Divisioncode ,
    '' Variantcode ,
    '' Principleerpproductcode ,
    '' Marketingteamcode ,
    '' Shelflifemaximum ,
    '' Shelflifeminimum ,
    '' Shelfliferemainingforsale ,
    '' Shelfliferemainingforproductio ,
    '' Sortingorder ,
    '' Isthisserviceitem ,
    spbl.SAL_Y Isthisitem_Saleable ,
   spbl.PUR_Y  Isthisitem_Purchaseable ,
   ''  Maintaininventory ,
   ''  Doyoumakethisitem ,
   '' Isthisfixedassetitem ,
   ''  Maintainbatchnos ,
   ''  Maintainserialnos ,
   ''  Ispricealreadyprinted ,
   ''  Seperatepriceforchildbarcode ,
   spbl.FREE_FLOW  Isthisfreeflowproduct ,
    '' Autoposttoinspection , 
  spbl.""Carton_Size"" Piecesinacarton ,
   spbl.""Pack_Size""  Piecesinapack ,
   ''  Perpiecegramageormililitre ,
   ''  Registrationno ,
   ''  Grossweight ,
   ''  Uomcodegrossweight ,
   spbl.WEGHT Netweight,
   ''  Volume ,
   ''  Uomcodevolume ,
  spbl.HIGHT Productheight,
  spbl.WIDTH Productwidth,
   spbl.DEPTH Productdepth,
   ''  Article_Partno ,
   spbl.CATLG Catalog_Designno,
   ''  Unitofmeasurecode_Inventory ,
   ''  Unitofmeasurecode_Purchases ,
   ''  Unitofmeasurecode_Sales ,
   ''  Leadtime_Purchases ,
   ''  Leadtime_Sales ,
   ''  Leadtime_Production ,
   ''  Leadtime_Transfer ,
  ''  Potolerence ,
  ''   Procurementgroupcode ,
  spbl.MAX_Q Minimumquantity,
 spbl.MIN_Q Maximumquantity,
 ''    Reorderlevelquantity ,
 ''    Minimumorderquantity ,
   spbl.ORD_Q Maximumorderquantity,
 spbl.TRADE productSaleRate,
 spbl.RETLP productRetailRate,
  spbl.COSTP productCostRate,
   ''  Printshortnameonslip ,
  ''   Printpriceonbarcode ,
  ''    Parentkeyname ,
  '' priceCodeDTOList,
  ''    Usercodename ,
   spbl.COMPC Companycodename,
  ''    Productclassificationcodename ,
  ''   Producthierarchycodename ,
   spbl.BRAND_NAME Brandcodename,
    spbl.DPTCD_NAME Departmentcodename,
   com_codename('CODE#', spbl.COLUR, null)    Colorcodename ,
   com_codename('CODE#', spbl.ISIZE,null)    Sizecodename ,
  '' Productabcindicationcodename ,
  ''  Productgroupcodename ,
  ''   Principlecodename ,
  ''  Divisioncodename ,
  ''    Variantcodename ,
  ''   Marketingteamcodename ,
   ''   Uomcodegrossweightname ,
   ''  Uomcodevolumename ,
   ''  UOMCode_Inventoryname,-- Unitofmeasurecode_Inventoryname ,
   ''   UOMCode_Purchasesname,--Unitofmeasurecode_Purchasesname ,
   ''   UOMCode_Salesname ,--Unitofmeasurecode_Salesname ,
  ''   priceCode,
STR_TRF_QTY('{1}',SPBL.ITEMS,SPBL.BARCODE) POQty,SAL_CODENAME('CUSTM', SPBL.PURCHASE_FROM, NULL) SUPPLIER 
,STR_TRF_QTY_Rem('{1}',SPBL.ITEMS,SPBL.BARCODE) POQtyRem
      from sal_prods_barcode_list spbl WHERE SPBL.BARCODE='{0}'";




        private static readonly string SELECT_BRACODEPRESENT = @"    select *
               from sal_prods_barcode_list sp
                WHERE  sp.barcode= '{0}'  ";

        private static readonly string SELECT_POAGAINSTGRN = "select t.pono from str_goodsr_1 t where (t.refno ='{0}' OR t.grn_# ='{0}') AND  APROV!='Y'";
        private static readonly string SELECT_ITEMPRICE = "SELECT sal_itemrate('TRADE','{1}', SYSDATE,'{2}','{0}', NULL) FROM DUAL";
        private static readonly string SELECT_ITEMPRICE_COST = "SELECT sal_itemrate('COSTP','{1}', SYSDATE,'{2}','{0}', NULL) FROM DUAL";
        
        private static readonly string SELECT_MEAN_SALE = @"   SELECT NVL(round(SUM(Im.sal_q)/30,2),0)  FROM({0}) im
 WHERE im.ITEMS  IN (SELECT ITEMS FROM SAL_PRODS_BARCODE_LIST  WHERE BARCODE ='{1}')
    and im.compc  =  '{2}'
     AND IM.Brnch = '{3}'
      AND im.wdate BETWEEN '{4}' AND '{5}' ";
        private static readonly string SELECT_BRANCH = "SELECT BRNCH FROM {1} S WHERE {2} ='{0}'";
        private static readonly string SELECT_COMP = "SELECT COMPC FROM {1} S WHERE {2} ='{0}'";
        //SELECT_COMP
        private static readonly string SELECT_STORE_OF_BRNCH = "select STOR# from str_stores_m   where  BRNCH   ='{0}' ";
        private static readonly string SELECT_BRNCH_OF_STORE = "select BRNCH  from str_stores_m   where   STOR#  ='{0}' ";
        private static readonly string SELECT_USER = "SELECT USRID FROM {1} S WHERE {2} ='{0}'";
        private static readonly string SELECT_ITEMQTY = " select fn_str_item_qty@ho('{0}','{1}','{2}','{3}','{4}','0001S03', sysdate) from dual";

        private static readonly string SELECT_item_BY_ITEM_BARCODE = "select descr from sal_prods_barcode_list where barcode ='{0}' and items ='{1}'";
        //select STOR# from str_stores_m   where  BRNCH   ='001' and deflt ='Y'
        #endregion

        #region Variables
        private ConnClass _connClass = null;
        DBHelper _dbHelper = null;

        #endregion

        #region Constructor
        public ItemDAO()
        {
            _connClass = new ConnClass();
            _dbHelper = new DBHelper();
        }
        #endregion

        #region MaxFunction
        private static readonly string SELECT_TO_FROMline = "select out_# from str_trf_out2 where   line# = '{0}'";

        public object GetOutFromLine(string line)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_TO_FROMline), line));
        }
        public string GetItemtock(string compc,string brnch,string godown,string items,string barcode)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_ITEMQTY),  compc,  brnch,  godown,  items,  barcode)).ToString();
        }
        private static readonly string SELECT_TOLLERENCE = "select nvl(p.tolrc,0) from str_purord_1 p where po_no =  '{0}' ";
        public object GetPOTollerence(string pono)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_TOLLERENCE), pono));
        }
        private static readonly string SELECT_POORDERQTY = "select nvl(sum(p.ord_q),0) from str_purord_2 p where po_no ='{0}' and barcode ='{1}' and items ='{2}' ";
        public object GetPOOrderQtyOFBarcode(string pono,string barcode,string items)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_POORDERQTY), pono, barcode, items));
        }
        public object GetStoreOfBranch(string brnch)
        {
            //Logger.CreateLog( string.Format(Utils.Utilities.GenerateProperTableName(SELECT_STORE_OF_BRNCH), brnch));
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_STORE_OF_BRNCH), brnch));
        }
        public object GetBarnchOfStore(string store)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_BRNCH_OF_STORE), store));
        }
        public object GetBranch(string docno,string table,string key)
        {
            //
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_BRANCH), docno, table, key));
        }
        public object GetCompc(string docno, string table, string key)
        {
            //
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_COMP), docno, table, key));
        }
        public object GetUserid(string docno, string table, string key)
        {
            //
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_USER), docno, table, key));
        }
        public object GetBarcodeCostPPrice(string barcode, string item, string brnch)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_ITEMPRICE_COST), barcode, item, brnch));
        }
        public object GetBarcodeTradePrice(string barcode, string item, string brnch)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_ITEMPRICE), barcode, item, brnch));
        }
        string SELECT_PO_TOLL = "  select nvl(tolrc,0)  from str_purord_1 where po_no= '{0}'";
        public object GetPOTollerance(string pono)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PO_TOLL), pono));
        }
        // STR_ORD_QTY('{0}',pod.items,pod.barcode)
        string SELECT_REMAIN_PO = "  select STR_ORD_QTY('{0}','{1}','{2}') from dual ";
        public object GetPOOrdRemaining(string pono,string items,string barcode)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_REMAIN_PO), pono, items,barcode));
        }
        public string GetMaxGrn1(string compc, string brnch)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_MAX_GRN1), compc, brnch)).ToString();
        }
        public string GetMaxGrn2(string Grn)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_MAX_GRN2), Grn)).ToString();
        }

        #endregion

        #region Functions
        public string GetDailyMeanSale(string barcode,string compc,string brnch)
        {
         
            string MVS = GetAllMvsByTypeSales("M", System.DateTime.Now.AddDays(-30).ToString("dd-MMM-yyyy"), System.DateTime.Now.ToString("dd-MMM-yyyy"));
            if (!string.IsNullOrWhiteSpace(MVS))
            {
                Logger.CreateLog(string.Format(Utils.Utilities.GenerateProperTableName(SELECT_MEAN_SALE), MVS, barcode, compc, brnch, System.DateTime.Now.AddDays(-30).ToString("dd-MMM-yyyy"), System.DateTime.Now.ToString("dd-MMM-yyyy")));
                return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_MEAN_SALE), MVS, barcode, compc, brnch, System.DateTime.Now.AddDays(-30).ToString("dd-MMM-yyyy"), System.DateTime.Now.ToString("dd-MMM-yyyy"))).ToString();
            }
            else { return "N/A"; }
        }
        //SELECT_STOCKin_MASTER
        public DataTable GetStockIN(string brnch, string docno, string compc)
        {
            //Logger.CreateLog( string.Format(Utils.Utilities.GenerateProperTableName(SELECT_STOCKOUT_MASTER, docno, compc, "%"));
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_STOCKin_MASTER), docno, compc, brnch)).Tables[0];
        }
        public DataTable GetStockOut(string brnch, string docno, string compc,string store)
        {
         var db =   _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName("SELect out_# froM str_trf_ins1 WHERE OUT_# ='{0}' AND aprov  ='A'"), docno));
            if (db == null)
            {
                Logger.CreateLog(string.Format(Utils.Utilities.GenerateProperTableName(SELECT_STOCKOUT_MASTER), docno, compc, brnch, store));
                return _dbHelper.DataAdapter(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_STOCKOUT_MASTER), docno, compc, brnch, store)).Tables[0];
            }
            else { return null; }
        }
        //
        public object GetStockOutCheck(string docno)
        {
            // Logger.CreateLog(string.Format(Utils.Utilities.GenerateProperTableName(SELECT_STOCKOUT_MASTER_CHECK), docno, compc, brnch, store));
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_STOCKOUT_MASTER_CHECK), docno));
        }
        public DataTable GetItemByBarcode(string barcode, string items, string compc, string brnch, string stocktype)
        {
            //string MeanSale = GetDailyMeanSale(barcode, compc, brnch);
            string MeanSale = "200";
            Logger.CreateLog( string.Format(Utils.Utilities.GenerateProperTableName(SELECT_ITEM_BY_BARCODENEW), barcode, "", compc, brnch, stocktype, MeanSale));
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(  SELECT_ITEM_BY_BARCODENEW), barcode, "", compc, brnch, stocktype,MeanSale)).Tables[0];
        }
        public DataTable GetItemByBarcodeStock(string barcode, string items, string compc, string brnch, string stocktype,string dcode)
        {
            string MeanSale = GetDailyMeanSale(barcode, compc, brnch);
            // string MeanSale = GetDailyMeanSale(barcode, compc, "003");
            Logger.CreateLog(string.Format(Utils.Utilities.GenerateProperTableName(SELECT_ITEM_BY_BARCODENEW), barcode, "", compc, brnch, stocktype, MeanSale));
            return _dbHelper.DataAdapter(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_ITEM_BY_BARCODENEW_STOCK), barcode, "", compc, brnch, stocktype, MeanSale, dcode)).Tables[0];
        }
        public object GetBarcodeOfItem(string item)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_BRCD_BY_ITEM), item));
        }
        public object GetBarcodeOfItemWIthDptd(string item,string Dptcd)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(SELECT_BRCD_BY_ITEM_DPTCD, item, Dptcd));
        }
        public object GetBarcodeOfItemWIthDptdIn(string item, string Dptcd)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_BRCD_BY_ITEM_DPTCD), item, Dptcd));
        }
        public object GetMasterBarcodeOfItem(string item)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_MAS_BRCD_BY_ITEM), item));
        }
        
        public object GetItemOfBarcode(string barcode)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_item_BY_ITEM), barcode));
        }
        public object GetDescrOfBarcode(string barcode,string items)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_item_BY_ITEM_BARCODE), barcode,items));
        }
        //SELECT_item_BY_ITEM
        public DataTable GetStockInHand(string barcode, string items, string compc, string brnch, string stocktype)
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_STOCK_IN_HANdUP), barcode,compc, brnch)).Tables[0];
        }
        public DataTable GetStockInHandForIndent(string offciecode,string type)
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_STOCK_IN_HAND_IND), offciecode)).Tables[0];
        }
        public DataTable GetUnApprovedDocuments(string barcode, string items, string compc, string brnch)
        {
           // Logger.CreateLog( string.Format(Utils.Utilities.GenerateProperTableName(SELECT_UNAPPROVED_DOC), barcode, "%", compc, brnch));
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_UNAPPROVED_DOC), barcode, "%", compc, brnch)).Tables[0];
        }

        public DataTable GetOnOrderDocuments(string barcode, string items, string compc, string brnch)
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_ONORDER_DOCUMENT), barcode, "%", compc, brnch)).Tables[0];
        }
        //SELECT_ONORDER_DOCUMENT
        public DataTable GetItemByBarcodeOnLocation(string barcode, string company, string branch)
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_ITEM_BY_BARCODE_ON_LOCATION), barcode, branch)).Tables[0];
        }

        public DataTable GetItemPrintInfoByBarcode(string barcode)
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_ITEM_PRINT_INFO_BY_BARCODE), barcode)).Tables[0];
        }

        public DataTable GetBarcodePrinterName()
        {
            return _dbHelper.DataAdapter(CommandType.Text, SELECT_BARCODE_PRINTER_NAME).Tables[0];
        }
        public DataTable GetBarcodeDetailByBarcodeORGRN(string barcode, string no, string type,string pono)
        {
            if (type.ToUpper() == "GRN")
            {

                //Logger.CreateLog(string.Format(Utils.Utilities.GenerateProperTableName(SELECT_BARCODEINFO), barcode, pono, no));
                return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_BARCODEINFO), barcode, pono,no)).Tables[0];
            }
            else if(type.ToUpper() == "TIN")
            {
               // Logger.CreateLog(string.Format(Utils.Utilities.GenerateProperTableName(SELECT_BARCODEINFO_TIN), barcode, pono, no));
                return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_BARCODEINFO_TIN), barcode,pono,no)).Tables[0];
            }
            else if (type.ToUpper() == "TOUT")
            {
               // Logger.CreateLog(string.Format(Utils.Utilities.GenerateProperTableName(SELECT_BARCODEINFO_OUT), barcode, pono, no));
                return _dbHelper.DataAdapter(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_BARCODEINFO_OUT), barcode, pono, no)).Tables[0];
            }
            else
            {
                return null;
            }
        }
        public DataTable GetPONumberAgainstGRN(string grnno)
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_POAGAINSTGRN), grnno)).Tables[0];

        }
        public object GETBARCODE(string barcode)
        {
            //Logger.CreateLog(string.Format(Utils.Utilities.GenerateProperTableName(SELECT_BRACODEPRESENT), barcode));
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_BRACODEPRESENT), barcode));
        }
        public object GETGRNAPPROVED(string no, string type)
        {
            if (type.ToUpper() == "GRN")
            {
                return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(CHECK_POEXIST), no));
            }
            else if (type.ToUpper() == "TIN")
            {
                return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_CHECKOUTEXIST), no));
            }
            else
            {
                return "N;Audit.";
            }
        }
        public object GETGRNAPPROVAL(string no, string type)
        {
            if (type.ToUpper() == "GRN")
            {
                return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_CHECKGRNAPROVAL), no));
            }
            else if (type.ToUpper() == "TIN")
            {
                return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_CHECKTRFINAPROVAL), no));
            }
            else if (type.ToUpper() == "TOUT")
            {
                return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_CHECKINDENTAPROVAL), no));
            }
            else
            {
                return "N;Audit.";
            }
        }
        public DataTable GetPOByPOORSupplier(string pono, string supplier, string compc, string brnch)
        {
            Logger.CreateLog(string.Format(Utils.Utilities.GenerateProperTableName(SELECT_POMASTERINFO), pono, supplier, compc, brnch));
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_POMASTERINFO), pono, supplier, compc, brnch)).Tables[0];
        }
        public DataTable GetIndByIndNo(string indno, string storeto)
        {
            return _dbHelper.DataAdapter(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_INDMASTERINFO), indno, storeto)).Tables[0];
        }
        public object GETPOREMAIN(string grnno, string compc, string brnch)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_POREMAIN), grnno, compc, brnch));
        }
        public object GETTROUTAPPROV(string outno, string compc, string brnch)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_TROUTAPPROV), outno, compc, brnch));
        }
        public object GETINDAPPROV(string indno, string compc, string brnch)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_INDAPPROV), indno, compc, brnch));
        }
        public object GETCHECKTRINANDTROUT(string outno, string compc, string brnch)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_CHECK_TROUT_TRIN), outno, compc, brnch));
        }
        public object GETCHECKOUTANDINDT(string indno, string compc, string brnch)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_CHECK_IND_OUT), indno, compc, brnch));
        }
        public object GETGRNAPROV(string grnno, string compc, string brnch)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_GRNAPROV), grnno));
        }
        public object GETGRNQTY(string grnno, string compc, string brnch,string barcode,string items)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_TOTALINPO), grnno, barcode, items));
        }
        public DataTable GetPOByPOORSupplierDetail(string pono)
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PODETAIL), pono)).Tables[0];
        }
        public DataTable GetStockOutByTrasnferOutDetail(string trout)
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_TRFOUTDETAIL), trout)).Tables[0];
        }
        public DataTable GetIndOutByIndentDetail(string trout)
        {
            return _dbHelper.DataAdapter(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_INDDETAIL), trout)).Tables[0];
        }
        public object GetGrn(string grnno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PREVIOUSGRN), grnno));
        }
        public object GetGrn2(string lineno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PREVIOUSGRN2), lineno));
        }
        public object GetGrnApproval(string grnno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PREVIOUSGRNAPP), grnno));
        }
        public object GetPOLine(string grn, string items,string barcode)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(PurchaseOrdLine), grn, items,barcode));
        }
        public object GetOutLine(string outno, string items,string barcode)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(TRanferOutLine), outno, items,barcode));
        }
        public object GetIndLine(string outno, string items, string barcode)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(IndentLine), outno, items, barcode));
        }
        #region SuppliersLOV

        private static readonly string SELECT_SUPPLIERSDETAIL = "select scm.custm ID,scm.descr NAME from sal_cus_mast scm where scm.pur_y='Y'";
        public DataTable GetSupplierDetail()
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_SUPPLIERSDETAIL))).Tables[0];
        }

        #endregion

        #region WarehousesLOV

        private static readonly string SELECT_WAREHOUSESDETAIL = "select ssm.stor# ID,ssm.descr NAME,SSM.BRNCH officeid  from str_stores_m ssm ";
        #endregion
        public DataTable GetWarehouseDetail()
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_WAREHOUSESDETAIL))).Tables[0];
        }
        #endregion
        #region IntendReasonLOV
        private static readonly string SELECT_INTENDREASON = "select code# id ,descr NAME from com_codefile where type# = '030'";
        public DataTable GetIntendReason()
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_INTENDREASON))).Tables[0];
        }

        #endregion
        #region GetTransferTypeLOV
        private static readonly string SELECT_TRANSFER_TYPE    = "select code# id,descr name from com_codefile where type# ='033'";
        public DataTable GetTransferType()
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_TRANSFER_TYPE))).Tables[0];
        }
        private static readonly string SELECT_DLP_LST = "select dcode id,descr NAME from com_dpartmnt";
        public DataTable GetAllDepartment()
        {
            return _dbHelper.DataAdapter(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_DLP_LST))).Tables[0];
        }
        
        #endregion
        #region PurchaseOrderDetail

        private static readonly string SELECT_PURORDDETAIL = "SELECT * FROM str_purord_2 where line# ='{0}' ";
        #endregion
        #region PURCHASEORS
        public DataTable GetPurOrdDetail(string docno)
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PURORDDETAIL), docno)).Tables[0];
        }
        private static readonly string SELECT_TODETAIL = "select * from str_trf_out2  where line# ='{0}' ";
        public DataTable GetTODetail(string docno)
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_TODETAIL), docno)).Tables[0];
        }
        #endregion

        #region StockTypesLOV

        private static readonly string SELECT_STOCKTYPESDETAIL = "select ccf.CODE# ID,ccf.descr NAME,'Y' ACTIVE ,ccf.deflt SALABLE from com_codefile ccf  where ccf.type#='S03' order by ccf.CODE#";

        public DataTable GetStockTypeDetail()
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_STOCKTYPESDETAIL))).Tables[0];
        }


        #endregion

        #region BayLOV

        private static readonly string SELECT_BAYDETAIL = "select CODE# ID,DESCR NAME,fld01 barcode from com_codefile where type# = 'BAY' ";

        public DataTable GetBayDetail()
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_BAYDETAIL))).Tables[0];
        }


        #endregion

        #region AislesLOV

        private static readonly string SELECT_AISLESDETAIL = "select CODE# ID,DESCR NAME,fld01 barcode from com_codefile where type# = 'AISLES' ";

        public DataTable GetAISLESDetail()
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_AISLESDETAIL))).Tables[0];
        }


        #endregion

        #region Login User

        private static readonly string SELECT_USERS = @"SELECT UI.ID,UI.USERCODE, UI.NAME, UI.PASSWORD, UI.ACTIVEYN, UI.LOCKCOUNTER, UI.EMAILID,
        UI.NEXTEXPIRYDATE, UI.LOCKAFTERHOWMANYATTEMPTS,UI.EFFECTIVEDATE, UI.PASSWORDEXPIREAFTERHOWMANYDAYS, UI.USERLEVEL, 
        UI.LOGINALLOWEDFROMTIME, UI.LOGINALLOWEDTOTIME,UC.ID AS COMPANYID,UC.NAME AS COMPANYNAME, UO.ID AS OFFICEID, 
        UO.NAME AS OFFICENAME FROM SEC_USERINFORMATION UI LEFT JOIN  (SELECT C.ID, C.NAME, UC.USERCODE,UC.COMPANYLINKEDWITHUSERCODE 
        FROM COM_COMPANY C INNER JOIN SEC_USERINFORMATIONCOMPANIES UC ON C.ID  = UC.COMPANYCODE) UC ON UI.ID = UC.COMPANYLINKEDWITHUSERCODE 
        LEFT JOIN (SELECT O.ID, O.NAME, UO.USERCODE,UO.OFFICELINKEDWITHUSERCODE FROM COM_OFFICES O INNER JOIN SEC_USERINFORMATIONOFFICES UO ON O.ID = UO.Officecode)
        UO ON UI.ID = UO.OFFICELINKEDWITHUSERCODE WHERE LOWER(UI.EMAILID) ='{0}'  AND ROWNUM  = 1";

        public DataTable GetUsersDetail(string emailAddress)
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_USERS), emailAddress.Trim().ToLower())).Tables[0];
        }


        #endregion

        public bool InsertGoodRcv1(string COMPC, string BRNCH, string USRID, string LOGNO, string GRN, string WDATE, string ACTIV, string DTYPE, string REC_B, string STR_F, string GODWN, string SUPCD,
            string PTERM, string CURCD, string CURAT, string DCHNO, string BILCM, string BILTY, string TRUCK, string NOTES,

            string APROV, string UPDATE_TP, string IDATE, string BILDS, string BILDT, string FRGHT, string BILTO, string PRNST, string VER, string GTYPE, string GC_NO, string BLADJ, string REFNO, string PONO, string POSTED )
        {
            
                //                insert into STR_GoodsR_1(Compc, Brnch, Usrid, Logno, Edate, Grn_#,wdate,Activ,Dtype,Rec_b,str_f,Godwn,supcd,Pterm,curcd,curat,Dchno,bilcm,bilty,Truck,notes,Aprov,update_tp,Idate,bilds,frght,bilto,prnst,ver_#,gtype,gc_no,bladj)
                //values('Compc', 'Brnch', 'Usrid', 'Logno', sysdate, 'Grn_#', 'wdate', 'Activ', 'Dtype', 'Rec_b', 'str_f', 'godown', 'supcd', 'Pterm', 'curcd', 'curat', 'Dchno', 'bilcm', 'bilty', 'Truck', 'notes', 'Aprov', 'update_tp', 'Idate', 'bilds', 'frght', 'bilto', 'prnst', 'ver_#', 'gtype', 'gc_no', 'bladj')

                int rowsEfect = _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(MERGE_GOODRCV1COMP),
                            COMPC, BRNCH, USRID, LOGNO, GRN, WDATE, ACTIV, DTYPE, REC_B, STR_F, GODWN, SUPCD, PTERM, CURCD, CURAT, DCHNO, BILCM, BILTY, TRUCK, NOTES, APROV, UPDATE_TP, IDATE, BILDS, BILDT, FRGHT, BILTO, PRNST, VER, GTYPE, GC_NO, BLADJ, REFNO, PONO, POSTED));
                return (rowsEfect > 0) ? true : false;
           

               
           
        }

        public bool InsertGoodRcv2(string GRN, string USRID, string LOGNO, string INS, string POL, string LINE, string ITEMS, string BATCH, string EXPDT, string RAT_G, string DIS_P, string COM_P, string GST_P, string SED_P, string CDT_P, string EXT_P, string DIS_A, string COM_A, string GST_A, string SED_A, string EXT_A, string CDT_A, string RAT_N, string ITMRT, string REQ_Q, string REC_Q, string BON_Q, string AMONT, string NOTES, string GROSV,
            string GST_B, string GST_W, string EDAGST_P, string EDAGST_A, string MARGIN_P, string MARGIN_A, string GST_FOC, string MARGIN_T, string APPLY_ON_ALL_ITEMS, string SHARE_FORMULA, string FOC_COST_SHARE, string BARCODE,
            string BARCODE_QTY, string LAST_COST, string PACK_QTY, string UNIT_QTY, string PACK_RATE, string GST_I, string CONSM, string MRP_G, string FRGHT_ITEMS, string EXPFN, string RBT_P, string RBT_A, string RAT_BR, string AMT_AR, string GPL, string VER, string RAT_F, string RATE, string REFNO)
        {
            

                int rowsEfect = _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(MERGE_GOODRCV2COMP),
                       GRN, USRID, LOGNO, INS, POL, LINE, ITEMS, BATCH, EXPDT, RAT_G, DIS_P, COM_P, GST_P, SED_P, CDT_P, EXT_P, DIS_A, COM_A, GST_A, SED_A, EXT_A, CDT_A , MARGIN_P, RAT_N, ITMRT, REQ_Q, REC_Q, BON_Q, AMONT, NOTES, GROSV, GST_B, GST_W, EDAGST_P, EDAGST_A, MARGIN_A, GST_FOC, MARGIN_T, APPLY_ON_ALL_ITEMS, SHARE_FORMULA, FOC_COST_SHARE, BARCODE, BARCODE_QTY, LAST_COST, PACK_QTY, UNIT_QTY, PACK_RATE, GST_I, CONSM, MRP_G, FRGHT_ITEMS, EXPFN, RBT_P, RBT_A, RAT_BR, AMT_AR, GPL, VER, RAT_F, RATE, REFNO));
                return (rowsEfect > 0) ? true : false;
            
           
        }
        public bool InsertGoodRcv2Update(string Line)
        {
            Common.Utilities.Logger.CreateLog("Fucntion Statted");
            Dictionary<object, object> obj = new Dictionary<object, object>();
 
            obj.Add("LINENO", Line);
            int rowsEfect = _connClass.Execute_Func(Utils.Utilities.GenerateProperTableName("update_grn_po@ho"), obj);
            Common.Utilities.Logger.CreateLog(rowsEfect.ToString());
            return (rowsEfect > 0) ? true : false;
        }
        public bool DeleteNotNeeded(string ID, string TABLENAME)
        {
            try
            {

                int rowsEfect = _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(GenericDel), ID, Utils.Utilities.GenerateProperTableName(TABLENAME)
                       ));
                return (rowsEfect > 0) ? true : false;
            }
            catch (Exception exception)
            {

                Common.Utilities.Logger.CreateLog(exception.Message);
                Common.Utilities.Logger.CreateLog(exception.StackTrace);
                return false;
            }
        }
        public bool DeleteNotNeeded(string ID, string REFNO, string TABLENAME, string IDCOULMN)
        {
            try
            {

                int rowsEfect = _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(DeleteGoodRcv), ID, REFNO, Utils.Utilities.GenerateProperTableName(TABLENAME), IDCOULMN
                       ));
                return (rowsEfect > 0) ? true : false;
            }
            catch (Exception exception)
            {

                Common.Utilities.Logger.CreateLog(exception.Message);
                Common.Utilities.Logger.CreateLog(exception.StackTrace);
                return false;
            }
        }

        public bool DeleteNotNeededForAudit(string ID, string REFNO, string TABLENAME, string IDCOULMN,string USERID)
        {
            try
            {

                int rowsEfect = _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(DeleteGoodRcvUser), ID, REFNO, Utils.Utilities.GenerateProperTableName(TABLENAME), IDCOULMN,USERID
                       ));
                return (rowsEfect > 0) ? true : false;
            }
            catch (Exception exception)
            {

                Common.Utilities.Logger.CreateLog(exception.Message);
                Common.Utilities.Logger.CreateLog(exception.StackTrace);
                return false;
            }
        }
        public DataTable SelectPoBySupAndPO(params object[] param)
        {

            Helper.FGLogger.PrintDebug( string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PO_OF_SUPPLIER), param[0], param[1], param[2], param[3]));
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PO_OF_SUPPLIER), param[0], param[1], param[2], param[3])).Tables[0];




        }

        public DataTable SelectSupByPO(params object[] param)
        {

            Helper.FGLogger.PrintDebug( string.Format(Utils.Utilities.GenerateProperTableName(SELECT_SUPPLIER_OF_PO), param[0], param[1], param[2]));
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_SUPPLIER_OF_PO), param[0], param[1], param[2])).Tables[0];


        }

        public DataTable SelectGRNBySupAndGRN(params object[] param)
        {
            try
            {
                Helper.FGLogger.PrintDebug( string.Format(Utils.Utilities.GenerateProperTableName(SELECT_GRN_OF_SUPPLIER), param[0], param[1], param[2], param[3]));
                return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_GRN_OF_SUPPLIER), param[0], param[1], param[2], param[3])).Tables[0];
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception); throw;

            }

        }

        public DataTable SelectAuditInfoByAuditNo(params object[] param)
        {
            try
            {
                Helper.FGLogger.PrintDebug( string.Format(Utils.Utilities.GenerateProperTableName(SELECT_AUDIT_INFO), param[0], param[1], param[2]));
                return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_AUDIT_INFO), param[0], param[1], param[2])).Tables[0];
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception); throw;

            }

        }

        public DataTable SelectPoByBarCode(params object[] param)
        {
            try
            {
                Helper.FGLogger.PrintDebug( string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PO_ITEMS), param[0], param[1]));
                return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PO_ITEMS), param[0], param[1])).Tables[0];
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception); throw;

            }

        }

        public DataTable SelectGRNByBarCode(params object[] param)
        {
            try
            {
                Helper.FGLogger.PrintDebug( string.Format(Utils.Utilities.GenerateProperTableName(SELECT_GRN_ITEMS), param[0], param[1]));
                return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_GRN_ITEMS), param[0], param[1])).Tables[0];
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception); throw;

            }

        }

        public DataTable SelectAuditByBarcode(params object[] param)
        {
            try
            {
                Helper.FGLogger.PrintDebug( string.Format(Utils.Utilities.GenerateProperTableName(SELECT_AUDIT_ITEMS), param[0], param[1]));
                return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_AUDIT_ITEMS), param[0], param[1])).Tables[0];
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception); throw;

            }

        }

        #region HHT RECEIVING
        public void InsertBarCodePrintHistory(params object[] param)
        {
            try
            {
                Helper.FGLogger.PrintInfo( string.Format(Utils.Utilities.GenerateProperTableName(INSERT_ITEM_BARCODE_PRINT_HISTORY), GetMaxItemBarCodeHostoryPk(param[0].ToString(), param[1].ToString()), param[0], param[1], param[2], param[3]));
                _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(INSERT_ITEM_BARCODE_PRINT_HISTORY), GetMaxItemBarCodeHostoryPk(param[0].ToString(), param[1].ToString()), param[0], param[1], param[2], param[3]));
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception.Message);
                Helper.FGLogger.PrintError(exception); throw;

            }
        }

        public void InsertBarCodePrintRequest(params object[] param)
        {
            try
            {
                Helper.FGLogger.PrintInfo( string.Format(Utils.Utilities.GenerateProperTableName(INSERT_ITEM_BARCODE_PRINT_REQUEST), GetMaxItemBarCodeHostoryPk(param[0].ToString(), param[1].ToString()), param[0], param[1], param[2], param[3], param[4], param[5]));
                _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(INSERT_ITEM_BARCODE_PRINT_REQUEST), GetMaxItemBarCodeHostoryPk(param[0].ToString(), param[1].ToString()), param[0], param[1], param[2], param[3], param[4], param[5]));
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception.Message);
                Helper.FGLogger.PrintError(exception); throw;

            }
        }

        private string GetMaxItemBarCodeHostoryPk(string company, string branch)
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_ITEM_BARCODE_PRINT_HISTORY_KEY), company, branch)).Tables[0].Rows[0][0].ToString();
        }

        public string InsertHHTReceiving(List<string> listOfScannedBarcode, string hhtKeyValue, params object[] param)
        {
            string hhtRecId = string.Empty;
            try
            {
                hhtRecId = GetMaxHTTReceivingPk(param[0].ToString(), param[1].ToString());

                Helper.FGLogger.PrintDebug( string.Format(Utils.Utilities.GenerateProperTableName(INSERT_HHT_RECEIVING), hhtRecId,
                    param[0], param[1], param[2], param[3], hhtKeyValue, param[4]));

                _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(INSERT_HHT_RECEIVING), hhtRecId,
                    param[0], param[1], param[2], param[3], hhtKeyValue, param[4]));

                foreach (string item in listOfScannedBarcode)
                {
                    string[] itemWithOtherInfo = item.Split('|');
                    InsertHHTReceiving2(hhtRecId, itemWithOtherInfo[0], Convert.ToDecimal(itemWithOtherInfo[1]),
                        param[2], param[4], itemWithOtherInfo[2], itemWithOtherInfo[3], itemWithOtherInfo[4], itemWithOtherInfo[5]);
                }
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception.Message);
                Helper.FGLogger.PrintError(exception); throw;

            }
            return hhtRecId;
        }

        public void InsertHHTReceivingItem(List<string> listOfScannedBarcode, string hhtRecId, params object[] param)
        {
            try
            {
                Helper.FGLogger.PrintDebug( string.Format(Utils.Utilities.GenerateProperTableName(UPDATE_HHT_RECEIVING), hhtRecId,
                param[0], param[1], param[2], param[3], param[4]));

                _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(UPDATE_HHT_RECEIVING), hhtRecId,
                param[0], param[1], param[2], param[3], param[4]));

                foreach (string item in listOfScannedBarcode)
                {
                    Helper.FGLogger.PrintDebug(item);
                    string[] itemWithOtherInfo = item.Split('|');
                    Helper.FGLogger.PrintDebug("@@@@@@@@@ITEM-WITH-OTHER-INFO@@@@@@   " + itemWithOtherInfo.Count().ToString());
                    InsertHHTReceiving2(hhtRecId, itemWithOtherInfo[0], Convert.ToDecimal(itemWithOtherInfo[1]),
                        param[2], param[4], itemWithOtherInfo[2], itemWithOtherInfo[3], itemWithOtherInfo[4], itemWithOtherInfo[5], null);
                }
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception.Message);
                Helper.FGLogger.PrintError(exception); throw;

            }
        }

        public void DeleteHHTReceivingItemForDamage(string hhtRecId, string barCode, string damageType)
        {
            try
            {
                Helper.FGLogger.PrintDebug( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_RECEIVING_2_DAMAGE),
                    hhtRecId, barCode, damageType));

                _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_RECEIVING_2_DAMAGE),
                    hhtRecId, barCode, damageType));
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception.Message);
                Helper.FGLogger.PrintError(exception); throw;

            }
        }

        public void DeleteHHTSelectedItem(string hhtRecId, string barCode, string expiryDate)
        {
            try
            {
                if (!string.IsNullOrEmpty(expiryDate))
                {
                    Helper.FGLogger.PrintDebug( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_SELECTED_ITEM),
                        hhtRecId, barCode, expiryDate));

                    _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_SELECTED_ITEM),
                        hhtRecId, barCode, expiryDate));
                }
                else
                {
                    Helper.FGLogger.PrintDebug( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_RECEIVING_2_NON_EXPIRY),
                                           hhtRecId, barCode));

                    _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_RECEIVING_2_NON_EXPIRY),
                        hhtRecId, barCode));
                }
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception.Message);
                Helper.FGLogger.PrintError(exception); throw;

            }
        }

        public string CloseHHTReceiving(string hhtRecID, enHHTKeys hhtKey,
            string userId, string company, string branch)
        {
            string grnNumber = string.Empty;
            try
            {
                //DataTable _masterInfoDT = _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_HHT_RECEIVING_1_MASTER_INFO, hhtRecID)).Tables[0];
                //COMPC,BRNCH,USRID,DType
                if (hhtKey == enHHTKeys.CyclingStock)
                {
                    using (DAL.DataAccess.ERPPolicyDAO _objErpPolicyDAO = new DAL.DataAccess.ERPPolicyDAO())
                    {
                        // User, COMPC, BRNCH
                        if (_objErpPolicyDAO.IsUserAllowOrNot(userId, company, branch,
                                Util.GetEnumDescription(Common.Enumeration.HHTPolicy.HHTAllowUserToMargeCyclicStock)))
                        {
                            _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(PARTIAL_CLOSE_HHT_RECEIVING_1), hhtRecID));
                            grnNumber = "N/A due to Partial Close";
                        }
                        else
                        {
                            grnNumber = FinishHHTActivity(hhtRecID);
                        }
                    }
                }
                else if (hhtKey == enHHTKeys.GRN)
                {
                    grnNumber = FinishHHTGoodReturnActivity(hhtRecID);
                }
                else
                {
                    grnNumber = FinishHHTActivity(hhtRecID);
                }

            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception.Message);
                Helper.FGLogger.PrintError(exception); throw;

                //_connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_RECEIVING_2_PEDING_RECORD, hhtRecID));
                //_connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_RECEIVING_1_PEDING_RECORD, hhtRecID));

            }

            return grnNumber;
        }

        private string FinishHHTActivity(string hhtRecID)
        {
            string grnNumber = string.Empty;
            try
            {
                _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(CLOSE_HHT_RECEIVING_1), hhtRecID));

                DataTable _tmpDT = _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_GRN_NO_OF_HHT_RECEIVING), hhtRecID)).Tables[0];
                if (_tmpDT != null && _tmpDT.Rows.Count > 0)
                {
                    grnNumber = _tmpDT.Rows[0][0].ToString();
                }
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception.Message);
                Helper.FGLogger.PrintError(exception); throw;

                //_connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_RECEIVING_2_PEDING_RECORD, hhtRecID));
                //_connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_RECEIVING_1_PEDING_RECORD, hhtRecID));

            }

            return grnNumber;
        }

        private string FinishHHTGoodReturnActivity(string hhtRecID)
        {
            string grnNumber = string.Empty;
            try
            {
                _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(CLOSE_HHT_RECEIVING_1), hhtRecID));

                DataTable _tmpDT = _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_RETURN_NO_OF_HHT_GOOD_RETURN), hhtRecID)).Tables[0];
                if (_tmpDT != null && _tmpDT.Rows.Count > 0)
                {
                    grnNumber = _tmpDT.Rows[0][0].ToString();
                }
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception.Message);
                Helper.FGLogger.PrintError(exception); throw;

                //_connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_RECEIVING_2_PEDING_RECORD, hhtRecID));
                //_connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_RECEIVING_1_PEDING_RECORD, hhtRecID));

            }

            return grnNumber;
        }

        private string GetMaxHTTReceivingPk(string company, string branch)
        {
            try
            {
                return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_MAX_HTT_RECEVING_KEY), company, branch)).Tables[0].Rows[0][0].ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void InsertHHTReceiving2(string hhtRecID, params object[] param)
        {
            try
            {
                // Not Needed! because Format has been changed on UI Level
                //param[6] = Convert.ToDateTime(param[6]).ToString("dd-MMM-yyyy");

                // If Item Has a Type then remove the previous item by type (Currently usable for Damage Stock by Damage Type)
                if (!string.IsNullOrEmpty(param[4].ToString()))
                {
                    // Print Query of Detail information before to run!
                    Helper.FGLogger.PrintDebug( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_RECEIVING_2_DAMAGE),
                         hhtRecID, param[0], param[4]));

                    // DELETE PREVIOUS ENTRY
                    _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_RECEIVING_2_DAMAGE),
                        hhtRecID, param[0], param[4]));
                }
                else
                {
                    // IF User Input the Expiry Date on Receiving
                    if (!string.IsNullOrEmpty(param[6].ToString()))
                    {
                        // Print Query of Detail information before to run!
                        Helper.FGLogger.PrintDebug( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_RECEIVING_2),
                             hhtRecID, param[0], param[6]));

                        // DELETE PREVIOUS ENTRY
                        _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_RECEIVING_2),
                            hhtRecID, param[0], param[6]));
                    }
                    else
                    {
                        // Print Query of Detail information before to run!
                        Helper.FGLogger.PrintDebug( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_RECEIVING_2_NON_EXPIRY),
                             hhtRecID, param[0]));

                        // DELETE PREVIOUS ENTRY
                        _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_RECEIVING_2_NON_EXPIRY),
                            hhtRecID, param[0]));
                    }
                }

                Helper.FGLogger.PrintDebug( string.Format(Utils.Utilities.GenerateProperTableName(INSERT_HHT_RECEIVING_2),
                    GetMaxHHTReceiving2Pk(hhtRecID), hhtRecID, param[0], param[1], param[2], param[3], param[4],
                    param[5], param[6], param[7]));

                // UPDATE ENTRY
                _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(INSERT_HHT_RECEIVING_2),
                    GetMaxHHTReceiving2Pk(hhtRecID), hhtRecID, param[0], param[1], param[2], param[3], param[4],
                    param[5], param[6], param[7]));
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception.Message);
                Helper.FGLogger.PrintError(exception); throw;

                //_connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_RECEIVING_2_PEDING_RECORD, hhtRecID));
                //_connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(DELETE_HHT_RECEIVING_1_PEDING_RECORD, hhtRecID));

            }
        }

        private string GetMaxHHTReceiving2Pk(string hhtRecID)
        {
            try
            {
                return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_MAX_HHT_RECEIVING_2), hhtRecID)).Tables[0].Rows[0][0].ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public DataTable GetPendingReceivingByUser(string userId, string company, string branch)
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_OPEN_RECEIVING_BY_USERID), userId, company, branch)).Tables[0];
        }

        public DataTable GetPendingGRNByUser(string userId, string company, string branch)
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_OPEN_GRN_BY_USERID), userId, company, branch)).Tables[0];
        }

        public DataTable GetPendingDemandByUser(string userId, string company, string branch)
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_OPEN_DEMAND_BY_USERID), userId, company, branch)).Tables[0];
        }

        public DataTable GetPendingHHTItemsByUser(string userId, string type, string company, string branch)
        {
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_OPEN_HHT_BY_USERID), userId, type, company, branch)).Tables[0];
        }



        public string TrimLastCharacter(String str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return str;
            }
            else
            {
                return str.TrimEnd(str[str.Length - 1]);
            }
        }
        public string GetAllMvsByTypeSales(string Type, string date, string to)
        {
            DateTime datef = DateTime.ParseExact(date, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            DateTime dateT = DateTime.ParseExact(to, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            string InQuery = "(";
            List<string> list = queryAppendForQuartersDaily(datef, dateT, SELECT_BY_DAILY_TABLE_PREFIX);
            foreach (var item in list)
            {
                InQuery += "'" + item + "',";
            }
            InQuery = TrimLastCharacter(InQuery);
            InQuery += ")";
            string returnStr = "";
            DataTable dt = new DataTable();
            string Sql = "";
            switch (Type)
            {

                case "D":
                    Sql =  string.Format(Utils.Utilities.GenerateProperTableName("select * from com_mv_cube_detail@ho t  where  descr in {0}"), InQuery);
                    break;
                case "M":
                    Sql =  string.Format(Utils.Utilities.GenerateProperTableName("select * from com_mv_cube_detail@ho t  where descr in {0}"), InQuery);
                    break;
                default:
                    break;

            }
            List<string> AllList = new List<string>();
            string select = " Select * from ";
            //  returnStr += select ;
            ConnClass my_func = new ConnClass();
            my_func.Open_Con();
            dt = my_func.MyDataTable(Sql);
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["DESCR"] != null)
                    {
                        AllList.Add(row["DESCR"].ToString()+"@ho");
                    }
                }
            }
            for (int i = 0; i <= AllList.Count - 1; i++)
            {
                returnStr += select + AllList[i] + "";
                if (i != (AllList.Count - 1))
                {
                    returnStr += " UNION ALL ";
                }
            }
            return Utils.Utilities.GenerateProperTableName(returnStr);
        }

        public List<string> queryAppendForQuartersDaily(DateTime _fromDate, DateTime _toDate, string SELECT_BY_DAILY_TABLE_PREFIX)
        {
            List<string> TableQuery = new List<string>();
            int MonthBack = Get_Starting_Month(_fromDate.Month);
            DateTime startDate = new DateTime(_fromDate.Year, MonthBack, 1);
            DateTime firstDayOfTheMonth = new DateTime(_toDate.Year, _toDate.Month, 1);
            DateTime endDate = firstDayOfTheMonth.AddMonths(1).AddDays(-1);
            //
            int QuatersCount = 0;

            for (DateTime i = startDate; i <= endDate; i = i.AddMonths(3))
            {
                string strQuarter = Month_LiesIn_Quarter(i.Month);
                TableQuery.Add( string.Format("{0}{1}{2}", SELECT_BY_DAILY_TABLE_PREFIX, strQuarter, i.Year));
                QuatersCount++;
            }


            return TableQuery;
        }
        public static int Get_Starting_Month(int Month)
        {
            if (Month >= 1 && Month <= 3)
                return 1;
            else if (Month >= 4 && Month <= 6)
                return 4;
            else if (Month >= 7 && Month <= 9)
                return 7;
            else
                return 10;
        }

        //public List<string> queryAppendForQuartersDaily(DateTime _fromDate, DateTime _toDate, string SELECT_BY_DAILY_TABLE_PREFIX)
        //{
        //    List<string> TableQuery = new List<string>();
        //    string removeUnionQuery = string.Empty;
        //    int inQuarter = GetQuarters(_fromDate.Date, _toDate.Date);
        //    int Month = _fromDate.Month;
        //    if (inQuarter == 1)
        //    {
        //        for (int i = 1; i <= inQuarter; i++)
        //        {
        //            string strQuarter = Month_LiesIn_Quarter(Month);
        //            TableQuery.Add( string.Format(Utils.Utilities.GenerateProperTableName("{0}{1}{2}", SELECT_BY_DAILY_TABLE_PREFIX, strQuarter, _fromDate.Year));
        //            Month += 3;
        //        }

        //    }
        //    else
        //    {
        //        for (int i = 1; i <= inQuarter; i++)
        //        {
        //            string strQuarter = Month_LiesIn_Quarter(Month);
        //            TableQuery.Add( string.Format(Utils.Utilities.GenerateProperTableName("{0}{1}{2}", SELECT_BY_DAILY_TABLE_PREFIX, strQuarter, _fromDate.Year));
        //            Month += 3;
        //        }
        //    }




        //    return TableQuery;
        //}
        public static string Month_LiesIn_Quarter(int Month)
        {
            if (Month >= 1 && Month <= 3)
                return "_01_03_";
            else if (Month >= 4 && Month <= 6)
                return "_04_06_";
            else if (Month >= 7 && Month <= 9)
                return "_07_09_";
            else
                return "_10_12_";
        }

        private static int GetQuarter(int nMonth)
        {
            if (nMonth <= 3)
                return 1;
            if (nMonth <= 6)
                return 2;
            if (nMonth <= 9)
                return 3;

            return 4;
        }
        private static int Round(double dVal)
        {
            if (dVal >= 0)
                return (int)Math.Floor(dVal);
            return (int)Math.Ceiling(dVal);
        }
        public static int GetQuarters(DateTime dt1, DateTime dt2)
        {
            double d1Quarter = GetQuarter(dt1.Month);
            double d2Quarter = GetQuarter(dt2.Month);
            double d1 = (d2Quarter - d1Quarter) + 1;
            int YearCheck = dt2.Year - dt1.Year;
            int result = 0;
            double d2 = (3 * (dt2.Year - dt1.Year));
            if (YearCheck > 0) { result = Convert.ToInt32(Round(d1 + d2)) + YearCheck; }
            else { result = Round(d1 + d2); }
            return result;
        }
        public string GetAllMvsByType(string Type, string date, string to)
        {
            string returnStr = "";
            DataTable dt = new DataTable();
            string Sql = "";
            switch (Type)
            {

                case "D":
                    Sql =  string.Format("select * from com_mv_cube_detail t  where t.qry_id = 'D' AND ( sdate<= '{0}'   AND edate >='{0}')", date);
                    break;
                case "M":
                    Sql =  string.Format("select * from com_mv_cube_detail t  where t.qry_id = 'D' and ( ( '{0}'  <sdate  )or ( '{1}'  <edate ) )", date, to);
                    break;
                case "S":
                    Sql =  string.Format("select * from  com_mv_cube_detail where dtype='M' and  qry_id ='STK_M' and  '{0}' between sdate and edate", date);
                    break;
                default:
                    break;

            }
            List<string> AllList = new List<string>();
            string select = " Select * from ";
            //  returnStr += select ;
           ConnClass my_func = new ConnClass();
            my_func.Open_Con();
            dt = my_func.MyDataTable(Sql);
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["DESCR"] != null)
                    {
                        AllList.Add(row["DESCR"].ToString());
                    }
                }
            }
            for (int i = 0; i <= AllList.Count - 1; i++)
            {
                returnStr += select + AllList[i] + "";
                if (i != (AllList.Count - 1))
                {
                    returnStr += " UNION ALL ";
                }
            }
            return returnStr;
        }



        #region HHT Policies
        public bool ShowPoOrderQty(params object[] param)
        {
            try
            {

                using (DAL.DataAccess.ERPPolicyDAO _objErpPolicyDAO = new DAL.DataAccess.ERPPolicyDAO())
                {
                    // User, COMPC, BRNCH                    
                    if (_objErpPolicyDAO.IsUserAllowOrNot(param[0], param[1], param[2],
                            Util.GetEnumDescription(Common.Enumeration.HHTPolicy.HHTShowItemOrderQtyOnReceiving)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception exception)
            {
                Helper.FGLogger.PrintError(exception); throw;
                return false;
            }
        }
        #endregion
        #endregion



        #region Dispose
        public void Dispose()
        {
            _connClass = null;
            _dbHelper = null;
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
