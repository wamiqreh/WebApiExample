using Common.Enumeration;
using Common.Models;
using Common.Utilities;
using DAL.DataAccess;
using Newtonsoft.Json;
using SND.BusinessObjects;
using SND.Models;
using SND.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SND.Controllers
{
    [RoutePrefix("api/Store")]
    public class StoreController : APIBase
    {
        #region Product Inquiry
        [Route("GetProductByBarcodeOrArticle")]
        [HttpPost]
        public HttpResponseMessage GetProductByBarcodeOrArticle()
        {
            try
            {

                var _Product = GetRawResponse<Product>();
                if (!Security.IsMD5ChecksumValid(_Product.barcode + _Product.article_no + _Product.device_id.ToString() + _Product.reg_id.ToString() + "88988934258f3d78ab16462fd68d6a38", _Product.checksum))
                {
                    return SendToApp(new Models.AppResponse { message = MessageHandler.NotAuthtorizedMsg, status_code = HttpStatusCode.Unauthorized });
                    //88988934258f3d78ab16462fd68d6a38
                }
                using (RemoteSearchBO _ObjBO = new RemoteSearchBO())
                {
                    var productRow = _ObjBO.GetItemInfoByBarcodeOrArticleNo(_Product.company_code, _Product.barcode, _Product.article_no, _Product.stock_status_type, _Product.office_code);
                    if (productRow != null)
                    {
                        return SendToApp(new Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK, data = productRow });
                    }
                    else
                        return SendToApp(new Models.AppResponse { message = MessageHandler.FailedMsg, status_code = HttpStatusCode.NotFound });
                }

            }
            catch (Exception exception)
            {

                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;
            }
        }
        [Route("GetProductByBarcodeOrArticleStock")]
        [HttpPost]
        public HttpResponseMessage GetProductByBarcodeOrArticleStock()
        {
            try
            {

                var _Product = GetRawResponse<Product>();
                if (!Security.IsMD5ChecksumValid(_Product.barcode + _Product.article_no + _Product.device_id.ToString() + _Product.reg_id.ToString() + "88988934258f3d78ab16462fd68d6a38", _Product.checksum))
                {
                    return SendToApp(new Models.AppResponse { message = MessageHandler.NotAuthtorizedMsg, status_code = HttpStatusCode.Unauthorized });
                    //88988934258f3d78ab16462fd68d6a38
                }
                using (RemoteSearchBO _ObjBO = new RemoteSearchBO())
                {
                    var productRow = _ObjBO.GetItemInfoByBarcodeOrArticleNoStock(_Product.company_code, _Product.barcode, _Product.article_no, _Product.stock_status_type, _Product.office_code, _Product.dcode);
                    if (productRow != null)
                    {
                        return SendToApp(new Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK, data = productRow });
                    }
                    else
                        return SendToApp(new Models.AppResponse { message = MessageHandler.FailedMsg, status_code = HttpStatusCode.NotFound });
                }

            }
            catch (Exception exception)
            {

                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;
            }
        }
        [Route("GetStockInHand")]
        [HttpPost]
        public HttpResponseMessage GetStockInHand()
        {
            try
            {
                var _Product = GetRawResponse<Product>();
                if (!Security.IsMD5ChecksumValid(_Product.barcode + _Product.article_no + _Product.device_id.ToString() + _Product.reg_id.ToString() + "88988934258f3d78ab16462fd68d6a38", _Product.checksum))
                    return SendToApp(new Models.AppResponse { message = MessageHandler.NotAuthtorizedMsg, status_code = HttpStatusCode.Unauthorized });

                using (RemoteSearchBO _ObjBO = new RemoteSearchBO())
                {
                    var productRow = _ObjBO.GetStockInHand(_Product.company_code, _Product.barcode, _Product.article_no, _Product.stock_status_type, _Product.office_code);
                    if (productRow != null)
                    {
                        return SendToApp(new Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK, data = productRow });
                    }
                    else
                        return SendToApp(new Models.AppResponse { message = MessageHandler.FailedMsg, status_code = HttpStatusCode.NotFound });
                }
            }
            catch (Exception exception)
            {

                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;

            }
        }

        [Route("GetUnapprovedDocument")]
        [HttpPost]
        public HttpResponseMessage GetUnapprovedDocument()
        {
            try
            {
                var _Product = GetRawResponse<Product>();
                if (!Security.IsMD5ChecksumValid(_Product.barcode + _Product.article_no + _Product.device_id.ToString() + _Product.reg_id.ToString() + "88988934258f3d78ab16462fd68d6a38", _Product.checksum))
                    return SendToApp(new Models.AppResponse { message = MessageHandler.NotAuthtorizedMsg, status_code = HttpStatusCode.Unauthorized });

                using (RemoteSearchBO _ObjBO = new RemoteSearchBO())
                {
                    var productRow = _ObjBO.GetUnapprovedDocument(_Product.company_code, _Product.barcode, _Product.article_no, _Product.stock_status_type, _Product.office_code);
                    if (productRow != null)
                    {
                        return SendToApp(new Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK, data = productRow });
                    }
                    else
                        return SendToApp(new Models.AppResponse { message = MessageHandler.FailedMsg, status_code = HttpStatusCode.NotFound });
                }
            }
            catch (Exception exception)
            {

                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;
            }
        }
        [Route("GetStockOutInInfoByDocumentNo")]
        [HttpPost]
        public HttpResponseMessage GetStockOutInInfoByDocumentNo()
        {
            try

            {
                Logger.CreateLog("Service Hiting");
                var _StockOutInDeviceEntity = GetRawResponse<StockOutIn>();
                if (!Security.IsMD5ChecksumValid(_StockOutInDeviceEntity.company_code.ToString() + _StockOutInDeviceEntity.office_code.ToString() + _StockOutInDeviceEntity.document_no.ToString() + _StockOutInDeviceEntity.device_id.ToString() + _StockOutInDeviceEntity.reg_id.ToString() + "88988934258f3d78ab16462fd68d6a38", _StockOutInDeviceEntity.checksum))
                    return SendToApp(new Models.AppResponse { message = MessageHandler.NotAuthtorizedMsg, status_code = HttpStatusCode.Unauthorized });

                using (RemoteSearchBO _ObjBO = new RemoteSearchBO())
                {
                    var productRow = _ObjBO.GetStockOutInInfoByDocumentNo(_StockOutInDeviceEntity.office_code, _StockOutInDeviceEntity.document_no, _StockOutInDeviceEntity.company_code);
                    if (productRow != null)
                    {
                        return SendToApp(new Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK, data = productRow });
                    }
                    else
                        return SendToApp(new Models.AppResponse { message = MessageHandler.FailedMsg, status_code = HttpStatusCode.NotFound });
                }
            }
            catch (Exception exception)
            {
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;
                throw;
            }
        }


        [Route("GetStockOnOrder")]
        [HttpPost]
        public HttpResponseMessage GetStockOnOrder()
        {
            try
            {
                var _Product = GetRawResponse<Product>();
                if (!Security.IsMD5ChecksumValid(_Product.barcode + _Product.article_no + _Product.device_id.ToString() + _Product.reg_id.ToString() + "88988934258f3d78ab16462fd68d6a38", _Product.checksum))
                    return SendToApp(new Models.AppResponse { message = MessageHandler.NotAuthtorizedMsg, status_code = HttpStatusCode.Unauthorized });

                using (RemoteSearchBO _ObjBO = new RemoteSearchBO())
                {
                    var productRow = _ObjBO.GetStockOnOrder(_Product.company_code, _Product.barcode, _Product.article_no, _Product.stock_status_type, _Product.office_code);
                    if (productRow != null)
                    {
                        return SendToApp(new Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK, data = productRow });
                    }
                    else
                        return SendToApp(new Models.AppResponse { message = MessageHandler.FailedMsg, status_code = HttpStatusCode.NotFound });
                }
            }
            catch (Exception exception)
            {
                //Logger.PrintError(exception);
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;
            }
        }



        [Route("GetPODetailsByPOOrSupplierNo")]
        [HttpPost]
        public HttpResponseMessage GetPODetailsByPOOrSupplierNo()
        {
            try
            {
                var _PO = GetRawResponse<PO>();
                if (!Security.IsMD5ChecksumValid(_PO.po_no + _PO.business_partner_code + _PO.device_id.ToString() + _PO.reg_id.ToString() + "88988934258f3d78ab16462fd68d6a38", _PO.checksum))
                    return SendToApp(new Models.AppResponse { message = MessageHandler.NotAuthtorizedMsg, status_code = HttpStatusCode.Unauthorized });

                using (RemoteSearchBO _ObjBO = new RemoteSearchBO())
                {
                    var productRow = _ObjBO.GetPODetailsByPOOrSupplierNo(_PO.company_code, _PO.office_code, _PO.po_no, _PO.business_partner_code);
                    if (productRow != null)
                    {
                        return SendToApp(new Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK, data = productRow });
                    }
                    else
                        return SendToApp(new Models.AppResponse { message = MessageHandler.FailedMsg, status_code = HttpStatusCode.NotFound });
                }
            }
            catch (Exception exception)
            {
                //   Logger.PrintError(exception);
                //   Logger.CreateLog(exception.Message);Logger.CreateLog(exception.StackTrace);throw; 
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;
            }
        }


        [Route("GetIndDetailsByIndNo")]
        [HttpPost]
        public HttpResponseMessage GetIndDetailsByIndNo()
        {
            try
            {
                var _Ind = GetRawResponse<Indents>();
                if (!Security.IsMD5ChecksumValid(_Ind.company_code + _Ind.office_code + _Ind.ind_no + _Ind.device_id.ToString() + _Ind.reg_id.ToString() + "88988934258f3d78ab16462fd68d6a38", _Ind.checksum))
                    return SendToApp(new Models.AppResponse { message = MessageHandler.NotAuthtorizedMsg, status_code = HttpStatusCode.Unauthorized });

                using (RemoteSearchBO _ObjBO = new RemoteSearchBO())
                {
                    var productRow = _ObjBO.GetIndDetailsByIndNo(_Ind.company_code, _Ind.office_code, _Ind.ind_no, "");
                    if (productRow != null)
                    {
                        return SendToApp(new Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK, data = productRow });
                    }
                    else
                        return SendToApp(new Models.AppResponse { message = MessageHandler.FailedMsg, status_code = HttpStatusCode.NotFound });
                }
            }
            catch (Exception exception)
            {
                //   Logger.PrintError(exception);
                //   Logger.CreateLog(exception.Message);Logger.CreateLog(exception.StackTrace);throw; 
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;
            }
        }



        [Route("GetOffileProductsByType")]
        [HttpPost]
        public HttpResponseMessage GetOffileProductsByType()
        {
            try
            {
                var _Offline = GetRawResponse<OffilneProducts>();
                if (!Security.IsMD5ChecksumValid(_Offline.company_code + _Offline.office_code + _Offline.type + _Offline.device_id.ToString() + _Offline.reg_id.ToString() + "88988934258f3d78ab16462fd68d6a38", _Offline.checksum))
                    return SendToApp(new Models.AppResponse { message = MessageHandler.NotAuthtorizedMsg, status_code = HttpStatusCode.Unauthorized });

                using (DAL.DataAccess.ItemDAO _ObjBO = new DAL.DataAccess.ItemDAO())
                {
                    var productRow = _ObjBO.GetStockInHandForIndent(_Offline.office_code, _Offline.type);
                    if (productRow != null)
                    {
                        return SendToApp(new Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK, data = productRow });
                    }
                    else
                        return SendToApp(new Models.AppResponse { message = MessageHandler.FailedMsg, status_code = HttpStatusCode.NotFound });
                }
            }
            catch (Exception exception)
            {
                //   Logger.PrintError(exception);
                //   Logger.CreateLog(exception.Message);Logger.CreateLog(exception.StackTrace);throw; 
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;
            }
        }



        [Route("GetBarcodeDetailsByGRN")]
        [HttpPost]
        public HttpResponseMessage GetBarcodeDetailsByGRN()
        {
            try
            {
                var _GRN = GetRawResponse<GRN>();
                if (!Security.IsMD5ChecksumValid(_GRN.barcode + _GRN.no + _GRN.device_id.ToString() + _GRN.reg_id.ToString() + "88988934258f3d78ab16462fd68d6a38", _GRN.checksum))
                    return SendToApp(new Models.AppResponse { message = MessageHandler.NotAuthtorizedMsg, status_code = HttpStatusCode.Unauthorized });

                using (RemoteSearchBO _ObjBO = new RemoteSearchBO())
                {
                    var productRow = _ObjBO.GetBarcodeDetailsByGRN(_GRN.barcode, _GRN.no, _GRN.type, _GRN.company_code, _GRN.office_code, _GRN.business_partner_code, _GRN.pono);
                    if (productRow != null)
                    {
                        return SendToApp(new Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK, data = productRow });
                    }
                    else
                        return SendToApp(new Models.AppResponse { message = MessageHandler.FailedMsg, status_code = HttpStatusCode.NotFound });
                }
            }
            catch (Exception exception)
            {
                //   Logger.PrintError(exception);
                //   Logger.CreateLog(exception.Message);Logger.CreateLog(exception.StackTrace);throw; 
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;
            }
        }


        [Route("GetAuditDetailsByAuditNo")]
        [HttpPost]
        public HttpResponseMessage GetAuditDetailsByAuditNo()
        {
            try
            {
                var aud = GetRawResponse<AUD>();
                if (!Security.IsMD5ChecksumValid(aud.audit_no + aud.company_code + aud.office_code+aud.user_id + aud.device_id.ToString() + aud.reg_id.ToString() + "88988934258f3d78ab16462fd68d6a38", aud.checksum))
                    return SendToApp(new Models.AppResponse { message = MessageHandler.NotAuthtorizedMsg, status_code = HttpStatusCode.Unauthorized });

                using (RemoteSearchBO _ObjBO = new RemoteSearchBO())
                {
                    var productRow = _ObjBO.GetAudByDocNumber(aud.audit_no, aud.company_code, aud.office_code);
                    if (productRow != null)
                    {
                        return SendToApp(new Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK, data = productRow });
                    }
                    else
                        return SendToApp(new Models.AppResponse { message = MessageHandler.FailedMsg, status_code = HttpStatusCode.NotFound });
                }
            }
            catch (Exception exception)
            {
                //   Logger.PrintError(exception);
                //   Logger.CreateLog(exception.Message);Logger.CreateLog(exception.StackTrace);throw; 
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;
            }
        }




        #endregion

        #region ListOfValues
        [Route("GetSuppliers")]
        [HttpPost]
        public HttpResponseMessage GetSuppliers()
        {
            try
            {
                var _Suppliers = GetRawResponse<Suppliers>();
                if (!Security.IsMD5ChecksumValid(_Suppliers.user_id + _Suppliers.company_code + _Suppliers.office_code + _Suppliers.device_id.ToString() + _Suppliers.reg_id.ToString() + "88988934258f3d78ab16462fd68d6a38", _Suppliers.checksum))
                    return SendToApp(new Models.AppResponse { message = MessageHandler.NotAuthtorizedMsg, status_code = HttpStatusCode.Unauthorized });

                using (RemoteSearchBO _ObjBO = new RemoteSearchBO())
                {
                    var supplierRow = _ObjBO.GetSuppliersBO();
                    if (supplierRow != null)
                    {
                        return SendToApp(new Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK, data = supplierRow });
                    }
                    else
                        return SendToApp(new Models.AppResponse { message = MessageHandler.FailedMsg, status_code = HttpStatusCode.NotFound });
                }
            }
            catch (Exception exception)
            {
                //   Logger.PrintError(exception);
                //   Logger.CreateLog(exception.Message);Logger.CreateLog(exception.StackTrace);throw; 
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;
            }
        }

        [Route("GetWarehouses")]
        [HttpPost]
        public HttpResponseMessage GetWarehouses()
        {
            try
            {
                var _Warehouse = GetRawResponse<Warehouses>();
                if (!Security.IsMD5ChecksumValid(_Warehouse.user_id + _Warehouse.company_code + _Warehouse.office_code + _Warehouse.device_id.ToString() + _Warehouse.reg_id.ToString() + "88988934258f3d78ab16462fd68d6a38", _Warehouse.checksum))
                    return SendToApp(new Models.AppResponse { message = MessageHandler.NotAuthtorizedMsg, status_code = HttpStatusCode.Unauthorized });

                using (RemoteSearchBO _ObjBO = new RemoteSearchBO())
                {
                    var warehouseRow = _ObjBO.GetWarehouseBO();
                    if (warehouseRow != null)
                    {
                        return SendToApp(new Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK, data = warehouseRow });
                    }
                    else
                        return SendToApp(new Models.AppResponse { message = MessageHandler.FailedMsg, status_code = HttpStatusCode.NotFound });
                }
            }
            catch (Exception exception)
            {
                //   Logger.PrintError(exception);
                //   Logger.CreateLog(exception.Message);Logger.CreateLog(exception.StackTrace);throw; 
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;
            }
        }


        [Route("GetStockTypes")]
        [HttpPost]
        public HttpResponseMessage GetStockTypes()
        {
            try
            {
                var _StockTypes = GetRawResponse<StockTypes>();
                if (!Security.IsMD5ChecksumValid(_StockTypes.user_id + _StockTypes.company_code + _StockTypes.office_code + _StockTypes.device_id.ToString() + _StockTypes.reg_id.ToString() + "88988934258f3d78ab16462fd68d6a38", _StockTypes.checksum))
                    return SendToApp(new Models.AppResponse { message = MessageHandler.NotAuthtorizedMsg, status_code = HttpStatusCode.Unauthorized });

                using (RemoteSearchBO _ObjBO = new RemoteSearchBO())
                {
                    var stockTypeRow = _ObjBO.GetStockTypeBO();
                    if (stockTypeRow != null)
                    {
                        return SendToApp(new Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK, data = stockTypeRow });
                    }
                    else
                        return SendToApp(new Models.AppResponse { message = MessageHandler.FailedMsg, status_code = HttpStatusCode.NotFound });
                }
            }
            catch (Exception exception)
            {
                //   Logger.PrintError(exception);
                //   Logger.CreateLog(exception.Message);Logger.CreateLog(exception.StackTrace);throw; 
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;
            }
        }

        [Route("GetBay")]
        [HttpPost]
        public HttpResponseMessage GetBay()
        {
            try
            {
                var _Bay = GetRawResponse<Bay>();
                if (!Security.IsMD5ChecksumValid(_Bay.user_id + _Bay.company_code + _Bay.office_code + _Bay.device_id.ToString() + _Bay.reg_id.ToString() + "88988934258f3d78ab16462fd68d6a38", _Bay.checksum))
                    return SendToApp(new Models.AppResponse { message = MessageHandler.NotAuthtorizedMsg, status_code = HttpStatusCode.Unauthorized });

                using (RemoteSearchBO _ObjBO = new RemoteSearchBO())
                {
                    var bayTypeRow = _ObjBO.GetBayBO();
                    if (bayTypeRow != null)
                    {
                        return SendToApp(new Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK, data = bayTypeRow });
                    }
                    else
                        return SendToApp(new Models.AppResponse { message = MessageHandler.FailedMsg, status_code = HttpStatusCode.NotFound });
                }
            }
            catch (Exception exception)
            {
                //   Logger.PrintError(exception);
                //   Logger.CreateLog(exception.Message);Logger.CreateLog(exception.StackTrace);throw; 
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;
            }
        }

        [Route("GetAisles")]
        [HttpPost]
        public HttpResponseMessage GetAisles()
        {
            try
            {
                var _Aisles = GetRawResponse<Aisles>();
                if (!Security.IsMD5ChecksumValid(_Aisles.user_id + _Aisles.company_code + _Aisles.office_code + _Aisles.device_id.ToString() + _Aisles.reg_id.ToString() + "88988934258f3d78ab16462fd68d6a38", _Aisles.checksum))
                    return SendToApp(new Models.AppResponse { message = MessageHandler.NotAuthtorizedMsg, status_code = HttpStatusCode.Unauthorized });

                using (RemoteSearchBO _ObjBO = new RemoteSearchBO())
                {
                    var aislesTypeRow = _ObjBO.GetAislesBO();
                    if (aislesTypeRow != null)
                    {
                        return SendToApp(new Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK, data = aislesTypeRow });
                    }
                    else
                        return SendToApp(new Models.AppResponse { message = MessageHandler.FailedMsg, status_code = HttpStatusCode.NotFound });
                }
            }
            catch (Exception exception)
            {
                //   Logger.PrintError(exception);
                //   Logger.CreateLog(exception.Message);Logger.CreateLog(exception.StackTrace);throw; 
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;
            }
        }
        #endregion


    }


}
