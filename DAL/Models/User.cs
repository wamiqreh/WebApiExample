using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class User : CommonParams
    {
        public string username { get; set; }
        public string password { get; set; }

        public string user_id { get; set; }
        public string company_code { get; set; }
        public string office_code { get; set; }
        public decimal login_code { get; set; }

        public decimal priority_code { get; set; }

        public string file { get; set; }
        public string extension { get; set; }
        public string filename { get; set; }
    }

    public class Product : User
    {
        public string barcode { get; set; }
        public string article_no { get; set; }
        public string stock_status_type { get; set; }
    }

    public class Media : User
    {
        public Decimal Id { get; set; }

        public string Activeyn { get; set; }

        public string Apppagename { get; set; }

        public Decimal Apppagecode { get; set; }

        public Decimal Attachmenttypecode { get; set; }

        public string Attachfilename { get; set; }

        public string Filedatetime { get; set; }

        public Decimal? Filesize { get; set; }

        public string Details { get; set; }

        public string Isthisdefault { get; set; }

        public decimal ReferenceTablecodeId { get; set; }
        public decimal ReferenceDocumentId { get; set; }

    }

    public class PO : User
    {
        public string po_no { get; set; }
        public string business_partner_code { get; set; }
        public decimal stock_status_type { get; set; }
    }

    public class StockOutIn : User
    {
        public string document_no { get; set; }

        public string stock_out_id { get; set; }
    }
}
