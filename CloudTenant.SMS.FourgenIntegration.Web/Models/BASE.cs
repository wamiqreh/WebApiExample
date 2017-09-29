using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SND.Models
{
    public class ReponseFormatMobile
    {

        public string TableName { get; set; }

        public string ID { get; set; }
        public bool isParent { get; set; }
        public string MapCode { get; set; }
        public string ERPTableName { get; set; }
        public string IDColumn { get; set; }
        public string IsError { get; set; }
        public List<ErrorStack> ErrorStacks = new List<ErrorStack>();
        public string Message { get; set; }
    }
    public class ReponseFormat
    {

        public string TableName { get; set; }
        public string Message { get; set; }
        public string Allowed { get; set; }
        public string ID { get; set; }
        public bool isParent { get; set; }
        public string MapCode { get; set; }
        public string ERPTableName { get; set; }
        public string IDColumn { get; set; }
        public int childRecords = new int();
        public List<ReponseFormatChildern> Childerns = new List<ReponseFormatChildern>();
        public int DeleteOrder { get; set; }
        public string UserAddedby { get; set; }
        public string isDeleted { get; set; }
        public string IsError { get; set; }
       
    }
    public class ErrorStack
    {
        public string TableName { get; set; }
        public string Message { get; set; }
        public string ID { get; set; }
    }
        public class ReponseFormatChildern
    {


        public string ID { get; set; }
        public string ERPTableName { get; set; }
        public string MapCode { get; set; }
        public int DeleteOrder = 1;
        public string UserAddedby { get; set; }
        public string isDeleted { get; set; }
    }
    public class RootObject
    {

        public string table { get; set; }
        public List<BASE> rows { get; set; }
    }
    public abstract class BASE : RootObject
    {
        public BASE()
        {

            ValueType = this.table;
        }
        public bool NeedFK { get; set; }
        public string ValueType { get; set; }
        public virtual ReponseFormat Insert(List<ReponseFormat> rp,List<ErrorStack> err) { return null; }
        public virtual ReponseFormat Insert( List<ErrorStack> errStack) { return null; }
        public new virtual string ToString() { return "Base object, we dont' want base created ValueType=" + this.ValueType; }
    }
}