using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SND.Utils
{
    public static class MessageHandler
    {
        public static string GenericErrorMsg = "Something went Wrong";
        public static string GenericSuccessMsg = "Added Scuessfully";
        public static string DataSuccessMsg = "Data Sync Scuessfully!";
        public static string NoRowsEffecttionMsg = "No Rows Effected";
        public static string NoParentMsg = "No Parent Found";
        public static string SuccessMsg = "Sucess";
        public static string NotAuthtorizedMsg = "Not Authorized";
        public static string FailedMsg = "Failed";
        public static string DocAlreadyApprovedMsg = "Dcoument Already Approved";
        public static string RefAlreadyApprovedMsg = "Reference Document Not Approved Yet";
        public static string RefDocNotFoundMsg = "Reference Document Not Found or Approved";
        public static string RefDocAlreadyUtilized = "Reference Document Already Utilized";
        public static string DocNotFoundMsg = "Dcoument Not Found";
        public static string BarcodeNotFoundMsg = "Barcode Not Found";
        public static string BarcodeNotFoundInRefDocMsg = "Barcode Not Found In Reference Document/Derpartment";

        public static string FromStoreNotFoundfDocMsg = "From/TO Store Not Found In Document";
        public static string TOStoreNotFoundfDocMsg = "To Store Not Found In Document";
        public static string NoEmployeeMsg = "You are not assigned as employee";
        // public static string BarcodeNotFoundInRefDocMsg = "Barcode Not Found In Refeence Dcoument";
        //Not Authorized
        //Sucess
        //Failed
    }
}