#region Modification History
// Created By:  Mirza Fahad Ali Baig
// Created On:  27th May, 2013
// Description: 
// ****************************** Modifications *********************************

// ******************************************************************************
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

using Common.Enumeration;
using DAL.DataAccess;

namespace BusinessLogic.BusinessObject
{
    public class ItemBO : IDisposable
    {
        #region Variables
        ItemDAO _itemDAO;
        #endregion

        #region Constructor
        public ItemBO()
        {
            _itemDAO = new ItemDAO();
        }
        #endregion

        #region Functions
        public DataTable GetItemByBarcode(string barcode,string articleno,string compc,string brnch,string stocktype)
        {
            return _itemDAO.GetItemByBarcode(barcode, articleno, compc, brnch, stocktype);
        }

        public DataTable GetItemByBarcodeOnLocation(string barcode, string company, string branch)
        {
            return _itemDAO.GetItemByBarcodeOnLocation(barcode, company, branch);
        }        

        public DataTable GetItemPrintInfoByBarcode(string barcode)
        {
            return _itemDAO.GetItemPrintInfoByBarcode(barcode);
        }

        public DataTable GetBarcodePrinterName()
        {
            return _itemDAO.GetBarcodePrinterName();
        }

        public DataTable SelectPoBySupAndPO(params object[] param)
        {
            return _itemDAO.SelectPoBySupAndPO(param);
        }

        public DataTable SelectSupByPO(params object[] param)
        {
            return _itemDAO.SelectSupByPO(param);
        }

        public DataTable SelectGRNBySupAndGRN(params object[] param)
        {
            return _itemDAO.SelectGRNBySupAndGRN(param);
        }

        public DataTable SelectAuditInfoByAuditNo(params object[] param)
        {
            return _itemDAO.SelectAuditInfoByAuditNo(param);
        }

        public DataTable SelectPoByBarcode(params object[] param)
        {
            return _itemDAO.SelectPoByBarCode(param);
        }

        public DataTable SelectGRNByBarCode(params object[] param)
        {
            return _itemDAO.SelectGRNByBarCode(param);
        }

        public DataTable SelectAuditByBarcode(params object[] param)
        {
            return _itemDAO.SelectAuditByBarcode(param);
        }

        public bool ShowPoOrderQty(params object[] param)
        {
            return _itemDAO.ShowPoOrderQty(param);
        }

        public void InsertBarcodePrintHistory(params object[] param)
        {
            try
            {
                _itemDAO.InsertBarCodePrintHistory(param);
            }
            catch (Exception exception)
            {
                 
            }

        }

        public void InsertBarcodePrintRequest(params object[] param)
        {
            try
            {
                _itemDAO.InsertBarCodePrintRequest(param);
            }
            catch (Exception exception)
            {
                 
            }

        }

        public string InsertHHTReceiving(List<string> listOfScannedBarcode, enHHTKeys hhtKey, params object[] param)
        {
            string hhtRecId = string.Empty;
            try
            {
                switch (hhtKey)
                {
                    case enHHTKeys.HHTReceiving:
                        hhtRecId = _itemDAO.InsertHHTReceiving(listOfScannedBarcode, HHTKeyValue.HHTReceiving, param);
                        break;
                    case enHHTKeys.GRN:
                        hhtRecId = _itemDAO.InsertHHTReceiving(listOfScannedBarcode, HHTKeyValue.GRN, param);
                        break;
                    case enHHTKeys.GapZap:
                        hhtRecId = _itemDAO.InsertHHTReceiving(listOfScannedBarcode, HHTKeyValue.GapZap, param);
                        break;
                    case enHHTKeys.CyclingStock:
                        hhtRecId = _itemDAO.InsertHHTReceiving(listOfScannedBarcode, HHTKeyValue.CyclicStock, param);
                        break;
                    case enHHTKeys.DamageStock:
                        hhtRecId = _itemDAO.InsertHHTReceiving(listOfScannedBarcode, HHTKeyValue.DamageStock, param);
                        break;
                    case enHHTKeys.BinStock:
                        hhtRecId = _itemDAO.InsertHHTReceiving(listOfScannedBarcode, HHTKeyValue.BinStock, param);
                        break;
                    case enHHTKeys.Demand:
                        hhtRecId = _itemDAO.InsertHHTReceiving(listOfScannedBarcode, HHTKeyValue.Demand, param);
                        break;
                    case enHHTKeys.TransferOut:
                        hhtRecId = _itemDAO.InsertHHTReceiving(listOfScannedBarcode, HHTKeyValue.TransferOut, param);
                        break;
                    case enHHTKeys.TransferIn:
                        hhtRecId = _itemDAO.InsertHHTReceiving(listOfScannedBarcode, HHTKeyValue.TransferIn, param);
                        break;
                }

            }
            catch (Exception exception)
            {
                 
            }

            return hhtRecId;

        }

        public DataTable GetPendingHHTItemsByUser(string userId, enHHTKeys hhtKey, string company, string branch)
        {
            DataTable tmpDT = null;
            try
            {
                switch (hhtKey)
                {
                    case enHHTKeys.CyclingStock:
                        tmpDT = _itemDAO.GetPendingHHTItemsByUser(userId, HHTKeyValue.CyclicStock, company, branch);
                        break;
                    case enHHTKeys.GapZap:
                        tmpDT = _itemDAO.GetPendingHHTItemsByUser(userId, HHTKeyValue.GapZap, company, branch);
                        break;
                    case enHHTKeys.DamageStock:
                        tmpDT = _itemDAO.GetPendingHHTItemsByUser(userId, HHTKeyValue.DamageStock, company, branch);
                        break;
                    case enHHTKeys.BinStock:
                        tmpDT = _itemDAO.GetPendingHHTItemsByUser(userId, HHTKeyValue.BinStock, company, branch);
                        break;
                }

            }
            catch (Exception exception)
            {
                 
            }

            return tmpDT;

        }

        public void InsertHHTReceivingItem(List<string> listOfScannedBarcode, string hhtRecId, params object[] param)
        {
            try
            {
                _itemDAO.InsertHHTReceivingItem(listOfScannedBarcode, hhtRecId, param);
            }
            catch (Exception exception)
            {
                 
            }
        }

        public void DeleteHHTReceivingItemForDamage(string hhtRecId, string barCode, string damageType)
        {
            try
            {
                _itemDAO.DeleteHHTReceivingItemForDamage(hhtRecId, barCode, damageType);
            }
            catch (Exception exception)
            {
                 
            }
        }

        public void DeleteHHTSelectedItem(string hhtRecId, string barCode, string expiryDate)
        {
            try
            {
                _itemDAO.DeleteHHTSelectedItem(hhtRecId, barCode, expiryDate);
            }
            catch (Exception exception)
            {
                 
            }
        }

        public string CloseHHTReceiving(string hhtRecId, enHHTKeys hhtKey,
            string userId, string company, string branch)
        {
            try
            {
                return _itemDAO.CloseHHTReceiving(hhtRecId, hhtKey, userId, company, branch);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public DataTable GetPendingReceivingByUser(string userId, string company, string branch)
        {
            return _itemDAO.GetPendingReceivingByUser(userId, company, branch);
        }
        public DataTable GetPendingGRNByUser(string userId, string company, string branch)
        {
            return _itemDAO.GetPendingGRNByUser(userId, company, branch);
        }
        public DataTable GetPendingDemandByUser(string userId, string company, string branch)
        {
            return _itemDAO.GetPendingDemandByUser(userId, company, branch);
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            _itemDAO.Dispose();
            _itemDAO = null;

        }
        #endregion
    }
}
