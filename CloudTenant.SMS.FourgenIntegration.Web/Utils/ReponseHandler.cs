using SND.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SND.Utils
{
    public  class ReponseHandler
    {
        List<ReponseFormat> list;
        public void ReponseHandlerPrepare() {
            list = new List<ReponseFormat>();
            list.Add(new Models.ReponseFormat() { IDColumn= "GRN_", ERPTableName = "str_goodsr_1", DeleteOrder=1, TableName= "STR_GOODSRECEIVE01MASTER", isParent=true });
            list.Add(new Models.ReponseFormat() { IDColumn = "LINE", ERPTableName = "str_goodsr_2", DeleteOrder = 2, TableName = "STR_GOODSRECEIVE02PRODUCTS", isParent = false });
            list.Add(new Models.ReponseFormat() { IDColumn = "IND_#", ERPTableName = "STR_INDENT_1", DeleteOrder = 1, TableName = "STR_INDENT01MASTER", isParent = true });
            list.Add(new Models.ReponseFormat() { IDColumn = "LINE#", ERPTableName = "STR_INDENT_2", DeleteOrder = 2, TableName = "STR_INDENT02PRODUCTS", isParent = false });  
            list.Add(new Models.ReponseFormat() { IDColumn = "AUD_#", ERPTableName = "str_audit_01", DeleteOrder = 1, TableName = "STR_STOCKAUDIT01MASTER", isParent = true });
            list.Add(new Models.ReponseFormat() { IDColumn = "LINE#", ERPTableName = "str_audit_04", DeleteOrder = 2, TableName = "STR_STOCKAUDIT02PRODUCTS", isParent = false });
            list.Add(new Models.ReponseFormat() { IDColumn = "TIN_#", ERPTableName = "str_trf_ins1", DeleteOrder = 1, TableName = "STR_STOCKIN01MASTER", isParent = true });
            list.Add(new Models.ReponseFormat() { IDColumn = "LINE#", ERPTableName = "str_trf_ins2", DeleteOrder = 2, TableName = "STR_STOCKIN02PRODUCTS", isParent = false });
            list.Add(new Models.ReponseFormat() { IDColumn = "LINE_no", ERPTableName = "str_trf_ins4", DeleteOrder = 3, TableName = "STR_STOCKIN03PRODUCTSREJECTION", isParent = false });
            list.Add(new Models.ReponseFormat() { IDColumn = "OUT_#", ERPTableName = "str_trf_out1", DeleteOrder = 1, TableName = "STR_STOCKOUT01MASTER", isParent = true, });
            list.Add(new Models.ReponseFormat() { IDColumn = "LINE#", ERPTableName = "str_trf_out2", DeleteOrder = 2, TableName = "STR_STOCKOUT02PRODUCTS", isParent = false, });


        }

        public  ReponseFormat GenerateResponse(string Id, string MapCode, Type ClassName,string UserCode,string Message,string isDel,string isError) {
            ReponseHandlerPrepare();
            string classname = ClassName.Name;
           string orgnameclassname = "";
           string isDeleted = (isError  =="N")?   (isDel != null) ? (isDel.ToString() == "Y") ? "Y" : "N" : "N"  :  "N" ;
           if (classname == typeof(GOODRECIEVE).Name.ToString()) { orgnameclassname = "STR_GOODSRECEIVE01MASTER"; }
           else if (classname == typeof(GOODRECIEVE2).Name.ToString()) { orgnameclassname = "STR_GOODSRECEIVE02PRODUCTS"; }
            else if (classname == typeof(Indent).Name.ToString()) { orgnameclassname = "STR_INDENT01MASTER"; }
            else if (classname == typeof(Indent2).Name.ToString()) { orgnameclassname = "STR_INDENT02PRODUCTS"; }
            else if (classname == typeof(StockAudit).Name.ToString()) { orgnameclassname = "STR_STOCKAUDIT01MASTER"; }
            else if (classname == typeof(StockAudit2).Name.ToString()) { orgnameclassname = "STR_STOCKAUDIT02PRODUCTS"; }
            else if (classname == typeof(TransferIn).Name.ToString()) { orgnameclassname = "STR_STOCKIN01MASTER"; }
            else if (classname == typeof(TransferIn2).Name.ToString()) { orgnameclassname = "STR_STOCKIN02PRODUCTS"; }
            else if (classname == typeof(TransferIn3).Name.ToString()) { orgnameclassname = "STR_STOCKIN03PRODUCTSREJECTION"; }
            else if (classname == typeof(TransferOut).Name.ToString()) { orgnameclassname = "STR_STOCKOUT01MASTER"; }
            else if (classname == typeof(TransferOut2).Name.ToString()) { orgnameclassname = "STR_STOCKOUT02PRODUCTS"; }
           
            var data = list.Where(m => m.TableName == orgnameclassname).FirstOrDefault();

            return new ReponseFormat() { DeleteOrder = data.DeleteOrder, ID = Id, MapCode = MapCode, TableName = orgnameclassname, isParent = data.isParent, ERPTableName = data.ERPTableName, IDColumn = data.IDColumn, UserAddedby = UserCode, isDeleted = isDeleted, IsError = isError, Message = Message };




        }
        public static string GetTableNAME(Type ClassName)
        {

            string classname = ClassName.Name;
            string orgnameclassname = "";
            if (classname == typeof(GOODRECIEVE).Name.ToString()) { orgnameclassname = "STR_GOODSRECEIVE01MASTER"; }
            else if (classname == typeof(GOODRECIEVE2).Name.ToString()) { orgnameclassname = "STR_GOODSRECEIVE02PRODUCTS"; }
            else if (classname == typeof(Indent).Name.ToString()) { orgnameclassname = "STR_INDENT01MASTER"; }
            else if (classname == typeof(Indent2).Name.ToString()) { orgnameclassname = "STR_INDENT02PRODUCTS"; }
            else if (classname == typeof(StockAudit).Name.ToString()) { orgnameclassname = "STR_STOCKAUDIT01MASTER"; }
            else if (classname == typeof(StockAudit2).Name.ToString()) { orgnameclassname = "STR_STOCKAUDIT02PRODUCTS"; }
            else if (classname == typeof(TransferIn).Name.ToString()) { orgnameclassname = "STR_STOCKIN01MASTER"; }
            else if (classname == typeof(TransferIn2).Name.ToString()) { orgnameclassname = "STR_STOCKIN02PRODUCTS"; }
            else if (classname == typeof(TransferIn3).Name.ToString()) { orgnameclassname = "STR_STOCKIN03PRODUCTSREJECTION"; }
            else if (classname == typeof(TransferOut).Name.ToString()) { orgnameclassname = "STR_STOCKOUT01MASTER"; }
            else if (classname == typeof(TransferOut2).Name.ToString()) { orgnameclassname = "STR_STOCKOUT02PRODUCTS"; }

            return orgnameclassname;

        }

    }
}