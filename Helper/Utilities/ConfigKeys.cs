using System.Configuration;

namespace Helper.Utilities
{
    public static class ConfigKeys
    {
        #region Barcode Label Printing Fonts
        public static string[] CommonFont
        {
            get
            {
                return ConfigurationManager.AppSettings["CommonFont"].Split(',');
            }
        }
        public static string[] ItemNameFont
        {
            get
            {
                return ConfigurationManager.AppSettings["ItemNameFont"].Split(',');
            }
        }
        public static string[] ItemCostPriceFont
        {
            get
            {
                return ConfigurationManager.AppSettings["ItemCostPriceFont"].Split(',');
            }
        }
        public static string[] ItemBarcodeFont
        {
            get
            {
                return ConfigurationManager.AppSettings["ItemBarcodeFont"].Split(',');
            }
        }
        public static string[] ItemBarcodeNoFont
        {
            get
            {
                return ConfigurationManager.AppSettings["ItemBarcodeNoFont"].Split(',');
            }
        }
        #endregion
    }
}
