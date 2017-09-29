#region Modificaiton Hisotory
// Created By: Mirza Fahad Ali Baig
// Created On: May29, 2013
// ********************************* Modification History *********************
// ****************************************************************************

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

namespace Common.Enumeration
{
    public enum enComDefaultKeys
    {
        [Category("ITM_I_"), Description("Show Item Image in PO, GRN & Other Docs")]
        ITMI,

        [Category("ITM_I_YN"), Description("Show Item Image in PO, GRN & Other Docs")]
        ITMIYN,

        [Category("PO_AP"), Description("Check PO #, GRN #, Capex # from stores software")]
        POAP,

        [Category("PATH"), Description("PATH OF CANE FOLDER")]
        PATH,

        [Category("PATHS"), Description("SOFTWARE PATH")]
        PATHS,

        [Category("DEFBR"), Description("DEFAULT BRANCH")]
        DEFBR,

        [Category("GLBOOK"), Description("BOOK ENTRY ALOWED IN VOUCHER (PV/RV)")]
        GLBOOK,

        [Category("SINGLE"), Description("SINGLE ENTRY SYSTEM")]
        SINGLE,

        [Category("GLAUTH"), Description("DOES VOUCHER NEEDS AUTHORIZATION")]
        GLAUTH,

        [Category("GLCHEQ"), Description("HALT IF CHEQUE NO NOT FOUND IN PRE-DEFIN")]
        GLCHEQ,

        [Category("GLDCS"), Description("G/L DEFAULT COST CENTRE FOR AUTO ENTRY")]
        GLDCS,

        [Category("GLDDPT"), Description("G/L DEFAULT DEPT  FOR AUTO ENTRY")]
        GLDDPT,

        [Category("GLDDSC"), Description("G/L DEFAULT DEPT  SUB SEC FOR AUTO ENTRY")]
        GLDDSC,

        [Category("GLEVEL"), Description("CHART OF ACOUNT LEVSLS")]
        GLEVEL,

        [Category("GLOPDT"), Description("G/L OPENING DATE OF GENERALK LDGER")]
        GLOPDT,

        [Category("TAX_01"), Description("GL DEFAULT TAX CODE")]
        TAX01,

        [Category("UN_LOD"), Description("Create Text File of INVOICES")]
        UNLOD,

        [Category("CC_JV"), Description("CREDIT COL. JV  DEBIT ACCOUNT")]
        CCJV,

        [Category("PJV_ST"), Description("PURCHASE JV SALES TAX ACCOUNT")]
        PJVST,

        [Category("TRFHO"), Description("TRANSFER BRANCH DATA TO H/O")]
        TRFHO,

        [Category("DSNSPS"), Description("DATA DAILY STOCK POSITION")]
        DSNSPS,

        [Category("TAX_PY"), Description("PAYROLL TAX DEDUCTION CODE")]
        TAXPY,

        [Category("PHOTO"), Description("EMPLOYEE PICTURE SAVING LOCATION")]
        PHOTO,

        [Category("GLDLO"), Description("G/L DEFAULT LOCATION FOR AUTO ENTRY")]
        GLDLO,

        [Category("CASHD"), Description("CASH SALES JV DEBIT A/C")]
        CASHD,

        [Category("CASHC"), Description("CASH SALES JV CREDIT A/C")]
        CASHC,

        [Category("P_ITAX"), Description("SALES RECEIPT PARTY I.TAX CODE")]
        PITAX,

        [Category("EXPAC"), Description("DEFAULT EXP. ACCOUNT CODES")]
        EXPAC,

        [Category("PNL_AC"), Description("P%L ACCOUNT")]
        PNLAC,

        [Category("LOAN"), Description("LOAN POSITION ACCOUNT")]
        LOAN,

        [Category("OUT_S"), Description("OUTSTATION Y/N")]
        OUTS,

        [Category("APR4I"), Description("CREDIT LIST 4 INV. 2 BRANCH")]
        APR4I,

        [Category("LODSB"), Description("TRANSFER BOUNCE CHEQUE TO BRANCH")]
        LODSB,

        [Category("HRDAT"), Description("Payroll Data To Branch")]
        HRDAT,

        [Category("QUOTE_DATE"), Description("DELIVERY / EXPIRY DAYS OF QUOTE")]
        QUOTEDATE,

        [Category("AUTO_I"), Description("AUTO GENERATE ITEM CODE")]
        AUTOI,

        [Category("AP_PO"), Description("Check PO #, GRN #, Capex # from stores software")]
        APPO,

        [Category("WALKING CUSTOMER"), Description("WALKING CUSTOMER")]
        DEFWC,

        [Category("FLOOD_TAX"), Description("FLOOD_TAX")]
        FLOODTAX,

        [Category("POSTS"), Description("POSTING")]
        POSTS,

        [Category("DY_C_ALRT"), Description("Day Close Alerts")]
        DYCALRT,

        [Category("P_C_ALRT"), Description("PRICE CHANGE ALERTS")]
        PCALRT,

        [Category("WCE_PRINT"), Description("PRINTER NAME TO PRINT THE BARCODE")]
        WCEPRINT
    }

    public enum enHHTKeys
    {
        HHTReceiving,
        GapZap,
        CyclingStock,
        DamageStock,
        BinStock,
        GRN,
        Demand,
        TransferIn,
        TransferOut

    }

    public enum HHTPolicy
    {
        [Description("HHT - Allow user To GoodsReturn"), Category("U,B")]
        HHTAllowUserToGoodsReturn,

        [Description("HHT - Allow user To BinStock"), Category("U,B")]
        HHTAllowUserToBinStock,

        [Description("HHT - Allow user To CyclicStock"), Category("U,B")]
        HHTAllowUserToCyclicStock,

        [Description("HHT - Allow user To DamageDisposal"), Category("U,B")]
        HHTAllowUserToDamageDisposal,

        [Description("HHT - Allow user To GapZap"), Category("U,B")]
        HHTAllowUserToGapZap,

        [Description("HHT - Allow user To Receiving"), Category("U,B")]
        HHTAllowUserToReceiving,

        [Description("HHT - Allow user To ItemInfoPrint"), Category("U,B")]
        HHTAllowUserToItemInfoPrint,

        [Description("HHT - Allow user To Demand"), Category("U,B")]
        HHTAllowUserToDemand,

        [Description("HHT - Allow user To TransferOut"), Category("U,B")]
        HHTAllowUserToTransferOut,

        [Description("HHT - Allow user To TransferIn"), Category("U,B")]
        HHTAllowUserToTransferIn,

        [Description("HHT - Allow user To Merge Cyclic Stock"), Category("U,B")]
        HHTAllowUserToMargeCyclicStock,

        [Description("HHT - Show Item Order Qty On Receiving"), Category("U,B")]
        HHTShowItemOrderQtyOnReceiving,

        [Description("Document No in Cyclic Stock is Required"), Category("C,B"),]
        DocumentNoInCyclicStockIsRequired

    }

    public enum enLoginResponse
    {
        Success,
        Failed,
        PasswordExpired,
        TimeDurationLock,
        AccountLock,
        ActivationRequired,
        InValidUsernameOrPassword,
        AccountExpired,
        NoOffice,
        NoCompany,
        NoCompanyNoOffice
    }
}