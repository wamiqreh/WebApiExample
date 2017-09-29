using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SND.BusinessObjects
{
    using DAL;
    using System;
    using System.Collections.Generic;



    public class RemoteSearchBO : IDisposable
    {
        #region Properties
        ProductInformationQueryDMLs _ProductInformationQueryDMLs;

        #endregion

        #region Constructor & Functions
        public RemoteSearchBO()
        {
            _ProductInformationQueryDMLs = new ProductInformationQueryDMLs();

        }
        public object GetItemInfoByBarcodeOrArticleNo(params object[] param)
        {
            if (string.IsNullOrEmpty(param[1].ToString()) && string.IsNullOrEmpty(param[2].ToString()))
                return null;
            else
            {
                param[1] = string.IsNullOrEmpty(param[1].ToString()) ? "%" : param[1].ToString();
                param[2] = string.IsNullOrEmpty(param[2].ToString()) ? "%" : param[2].ToString();
                return _ProductInformationQueryDMLs.GetItemInfoByBarcodeOrArticleNo(param);
            }
        }
        public object GetItemInfoByBarcodeOrArticleNoStock(params object[] param)
        {
            if (string.IsNullOrEmpty(param[1].ToString()) && string.IsNullOrEmpty(param[2].ToString()))
                return null;
            else
            {
                param[1] = string.IsNullOrEmpty(param[1].ToString()) ? "%" : param[1].ToString();
                param[2] = string.IsNullOrEmpty(param[2].ToString()) ? "%" : param[2].ToString();
                return _ProductInformationQueryDMLs.GetItemInfoByBarcodeOrArticleNoStock(param);
            }
        }
        public object GetStockInHand(params object[] param)
        {
            if (string.IsNullOrEmpty(param[1].ToString()))
                return null;
            else
            {
                param[1] = string.IsNullOrEmpty(param[1].ToString()) ? "%" : param[1].ToString();
                return _ProductInformationQueryDMLs.GetStockInHand(param);
            }
        }
        public object GetUnapprovedDocument(params object[] param)
        {
            if (string.IsNullOrEmpty(param[1].ToString()))
                return null;
            else
            {
                param[1] = string.IsNullOrEmpty(param[1].ToString()) ? "%" : param[1].ToString();
                return _ProductInformationQueryDMLs.GetUnapprovedDocument(param);
            }
        }
        public object GetAudByDocNumber(params object[] param)
        {
            if (string.IsNullOrEmpty(param[0].ToString()))
                return null;
            else
            {
                param[2] = string.IsNullOrEmpty(param[2].ToString()) ? "%" : param[2].ToString();
                param[1] = string.IsNullOrEmpty(param[1].ToString()) ? "%" : param[1].ToString();
                return _ProductInformationQueryDMLs.GetAuditDetailsFromNo(param[0].ToString(),param[2].ToString(),param[1].ToString() );
            }
        }
        public object GetStockOutInInfoByDocumentNo(params object[] param)
        {
            if (string.IsNullOrEmpty(param[0].ToString()) || string.IsNullOrEmpty(param[1].ToString()) || string.IsNullOrEmpty(param[2].ToString()))
                return null;
            else
            {
                return _ProductInformationQueryDMLs.GetStockOutInInfoByDocumentNo(param);
                // return _ProductInformationQueryDMLs.GetUnapprovedDocument(param);
                ///  return _ProductInformationQueryDMLs.GetStockOutInInfoByDocumentNo(param);
            }
        }

        public object GetStockOnOrder(params object[] param)
        {
            if (string.IsNullOrEmpty(param[2].ToString()))
                return null;
            else
            {
                param[2] = string.IsNullOrEmpty(param[2].ToString()) ? "%" : param[2].ToString();
                return _ProductInformationQueryDMLs.GetStockOnOrder(param);
            }
        }
        public object GetPODetailsByPOOrSupplierNo(params object[] param)
        {
            if (string.IsNullOrEmpty(param[2].ToString()) && string.IsNullOrEmpty(param[3].ToString()))
                return null;
            else
            {
                param[2] = string.IsNullOrEmpty(param[2].ToString()) ? "%" : param[2].ToString();
                param[3] = string.IsNullOrEmpty(param[3].ToString()) ? "%" : param[3].ToString();
                return _ProductInformationQueryDMLs.GetPODetailsByPOOrSupplierNo(param);
            }
        }
        public object GetIndDetailsByIndNo(params object[] param)
        {
            if (string.IsNullOrEmpty(param[2].ToString()) && string.IsNullOrEmpty(param[3].ToString()))
                return null;
            else
            {
                param[2] = string.IsNullOrEmpty(param[2].ToString()) ? "%" : param[2].ToString();
                param[3] = string.IsNullOrEmpty(param[3].ToString()) ? "%" : param[3].ToString();
                return _ProductInformationQueryDMLs.GetIndDetailsByIndNo(param);
            }
        }
        public object GetBarcodeDetailsByGRN(params object[] param)
        {
            if (string.IsNullOrEmpty(param[0].ToString()) && string.IsNullOrEmpty(param[1].ToString()))
                return null;
            else
            {             
               
                return _ProductInformationQueryDMLs.GetBarcodeDetailsByGRN(param);
            }
        }

        #region SuppliersLOV

        public object GetSuppliersBO(){
                return _ProductInformationQueryDMLs.GetSuppliersDML();
        }

        #endregion

        #region WarehousesLOV

        public object GetWarehouseBO()
        {
            return _ProductInformationQueryDMLs.GetWarehouseDML();

        }
        #endregion

        #region StockTypesLOV

        public object GetStockTypeBO()
        {
            return _ProductInformationQueryDMLs.GetStockTypeDML();
        }

        #endregion
        #region BayLOV

        public object GetBayBO()
        {
            return _ProductInformationQueryDMLs.GetBayDML();
        }

        #endregion
        #region AislesLOV

        public object GetAislesBO()
        {
            return _ProductInformationQueryDMLs.GetAislesDML();
        }

        #endregion
        

        #endregion

        #region Dispose
        public void Dispose()
        {
            _ProductInformationQueryDMLs.Dispose();
            _ProductInformationQueryDMLs = null;
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}