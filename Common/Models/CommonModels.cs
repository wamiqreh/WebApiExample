using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{

    #region Model Classes
    public class UserModel
    {
        public object Id { get; set; }
        public object TimeStamp { get; set; }
        public object UserCode { get; set; }
        public object LoginCode { get; set; }
        public object ActiveYN { get; set; }
        public object Name { get; set; }
        public object DesigntationCode { get; set; }
        public object UserLevel { get; set; }
        public object Email { get; set; }
        public object Password { get; set; }
        public object EffectiveDate { get; set; }
        public object LockAfterHowManyAttempts { get; set; }
        public object LockCounter { get; set; }
        public object LoginAllowedFromTime { get; set; }
        public object LoginAllowedToTime { get; set; }
        public object AllDevices { get; set; }
        public object PasswordExpiredAfterHowManyNoOfDays { get; set; }
        public object NextExpiryDate { get; set; }
        public object DepartmentCode { get; set; }

        public CompanyModel CompanyInfo { get; set; }
        public OfficeModel OfficeInfo { get; set; }
        //public FiscalYearModel FiscalYearInfo { get; set; }
        // public WorkDateModel WorkDateInfo { get; set; }
        // public CompanyPolicyModel CompanyPolicyInfo { get; set; }

        // public FiscalYearModel[] ListOfFiscalYears { get; set; }

        public object ListOfOffices { get; set; }
        public object DeviceInfo { get; set; }
        public object StockStatus { get; set; }
        public object warehouseList { get; set; }
        public object supplierList { get; set; }
        public object bayList { get; set; }
        public object ThirdParty { get; set; }
        public object aisleList { get; set; }
        public object TransferInReasonList { get; set; }
        public object IntendReasonList { get; set; }
        public object TransferTypeList { get; set; }
        public object IndentOfflineList { get; set; }
        public object DepartmentList { get; set; }
    }
    public class CompanyModel
    {
        public object Id { get; set; }

        public object Maxid { get; set; }

        public object Timestamp { get; set; }

        public object Usercode { get; set; }

        public object Logincode { get; set; }


        public object Activeyn { get; set; }


        public object Name { get; set; }


        public object Shortname { get; set; }

        public object Countrycode { get; set; }

        public object Currencycode { get; set; }


        public object Languagecode { get; set; }


        public object Industrycode { get; set; }

        public object Usercodename { get; set; }
        public object Countrycodename { get; set; }
        public object Currencycodename { get; set; }
        public object Languagecodename { get; set; }
        public object Industrycodename { get; set; }
    }
    public class OfficeModel
    {

        public object Id { get; set; }

        public object Maxid { get; set; }

        public object Timestamp { get; set; }

        public object Usercode { get; set; }

        public object Logincode { get; set; }

        public object Activeyn { get; set; }

        public object Companycode { get; set; }


        public object Citycode { get; set; }


        public object Name { get; set; }


        public object Shortname { get; set; }


        public object Urbanrural { get; set; }


        public object Sortby { get; set; }


        public object Hierarchycode { get; set; }


        public object Isthisvirtualoffice { get; set; }


        public object Fromdate { get; set; }

        public object Todate { get; set; }

        public object Usercodename { get; set; }
        public object Companycodename { get; set; }
    }


    //line#,out_qty,bon_qty,items,costp,tradp,amont,barcode
    public class TransferOut
    {
        public object OUT_NO { get; set; }
        public object USRID { get; set; }
        public object LOGNO { get; set; }
        public object EDATE { get; set; }
        public object LINENO { get; set; }
        public object ITEMS { get; set; }
        public object BATCH { get; set; }
        public object OUT_Q { get; set; }
        public object BON_Q { get; set; }
        public object COSTP { get; set; }
        public object TRADP { get; set; }
        public object AMONT { get; set; }
        public object NOTES { get; set; }
        public object INDLNO { get; set; }
        public object BARCODE { get; set; }
        public object ISU_Q { get; set; }
        public object GRL_NO { get; set; }
        public object EXPEN { get; set; }
        public object S2DLNO { get; set; }
        public object AVGRT { get; set; }
        public object UNITQ { get; set; }
        public object PCK_QTY { get; set; }
        public object EXPRY { get; set; }
        public object REFNO { get; set; }
        public object QTY_IN { get; set; }

    }
        public class PurchaseOrder
    {




        public object PO_NO { get; set; }
        public object USRID { get; set; }
        public object LOGNO { get; set; }
        public object EDATE { get; set; }
        public object REQLbo { get; set; }
        public object QOTLno { get; set; }
        public object LINEno { get; set; }
        public object ITEMS { get; set; }
        public object REQ_Q { get; set; }
        public object RAT_G { get; set; }
        public object DIS_P { get; set; }
        public object COM_P { get; set; }
        public object GST_P { get; set; }
        public object SED_P { get; set; }
        public object CDT_P { get; set; }
        public object EXT_P { get; set; }
        public object DIS_A { get; set; }
        public object COM_A { get; set; }
        public object GST_A { get; set; }
        public object SED_A { get; set; }
        public object EXT_A { get; set; }
        public object CDT_A { get; set; }
        public object RAT_N { get; set; }
        public object ITMRT { get; set; }
        public object ORD_Q { get; set; }
        public object BON_Q { get; set; }
        public object AMONT { get; set; }
        public object DELDT { get; set; }
        public object PACKS { get; set; }
        public object NOTES { get; set; }
        public object BARCODE { get; set; }
        public object FOC_COST_SHARE { get; set; }
        public object SHARE_FORMULA { get; set; }
        public object APPLY_ON_ALL_ITEMS { get; set; }
        public object MARGIN_T { get; set; }
        public object LAST_COST { get; set; }
        public object MARGIN_A { get; set; }
        public object MARGIN_P { get; set; }
        public object EDAGST_A { get; set; }
        public object EDAGST_P { get; set; }
        public object GST_W { get; set; }
        public object GST_FOC { get; set; }
        public object GST_B { get; set; }
        public object GROSV { get; set; }
        public object PACK_RATE { get; set; }
        public object UNIT_QTY { get; set; }
        public object PACK_QTY { get; set; }
        public object BARCODE_QTY { get; set; }
        public object I_H_Q { get; set; }
        public object CONSM { get; set; }
        public object GST_I { get; set; }
        public object MRP_G { get; set; }
        public object RATEno { get; set; }
        public object RBT_P { get; set; }
        public object RBT_A { get; set; }
        public object RAT_BR { get; set; }
        public object AMT_AR { get; set; }
        public object METER { get; set; }
        public object CONVR { get; set; }

    }


    public class StockAuditProducts {
        public object PHT_QTY { get; set; }
        public object AUDNO { get; set; }
        public object BARCODE { get; set; }
        public object ITEMS { get; set; }
      

    }
    #endregion
}
