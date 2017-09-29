using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SND.Models
{
    public class PurchaseOrderTemp : BASE
    {
        public PurchaseOrderTemp()
        {

            NeedFK = true;
        }

      


        public string ParentId { get; set; }
        public string ParentTableName = "PURCHASEORDER";
        public string Id { get; set; }
        public string Maxid { get; set; }
        public string Timestamp { get; set; }
        public string Logincode { get; set; }
        public string Usercode { get; set; }//Map code req
        public string Goodsreceive01id { get; set; }
        public string Isdeleted { get; set; }
        public string Productcode { get; set; }//Map code req
        public string Productbarcode { get; set; }//Map code req from wamiq
        public string Pricecode { get; set; }
        public string Costprice { get; set; }
        public string Salesprice { get; set; }
        public string Retailprice { get; set; }
        public string Receiveqty { get; set; }
        public string Receivevalue { get; set; }
        public string Uomcode { get; set; }
        public string Lotorbatchno { get; set; }
        public string Batchmanufacturedate { get; set; }
        public string Batchexpirydate { get; set; }
        public string Productserialno { get; set; }
        public string Productbinnoscode { get; set; }
        public string Inhandqty { get; set; }
        public string Referencetablecode { get; set; }
        public string Referencedocumentid { get; set; }
        public string Details { get; set; }
        public string Receiveqty1 { get; set; }
        public string Receiveqty2 { get; set; }
        public string Receiveqty3 { get; set; }
        public string Costprice1 { get; set; }
        public string Costprice2 { get; set; }
        public string Costprice3 { get; set; }
        public string Grossvalue { get; set; }
        public string Freeofcostqty { get; set; }
        public string Discountpercentage { get; set; }
        public string Discountamount { get; set; }
        public string Grossvalueafterdiscount { get; set; }
        public string Calcsalestaxonfocqty { get; set; }
        public string Salestaxpercentage { get; set; }
        public string Salestaxamount { get; set; }
        public string Excisedutypercentage { get; set; }
        public string Excisedutyamount { get; set; }
        public string Extradiscountpercentage { get; set; }
        public string Extradiscountamount { get; set; }
        public string Commisionpercentage { get; set; }
        public string Commisionamount { get; set; }
        public string Customdutypercentage { get; set; }
        public string Customdutyamount { get; set; }
        public string Extracolumn01percentage { get; set; }
        public string Extracolumn01amount { get; set; }
        public string Extracolumn02percentage { get; set; }
        public string Extracolumn02amount { get; set; }
        public string Extracolumn03percentage { get; set; }
        public string Extracolumn03amount { get; set; }
        public string Extracolumn04percentage { get; set; }
        public string Extracolumn04amount { get; set; }
        public string Extracolumn05percentage { get; set; }
        public string Extracolumn05amount { get; set; }
        public string Extracolumn06percentage { get; set; }
        public string Extracolumn06amount { get; set; }
        public string Extracolumn07percentage { get; set; }
        public string Extracolumn07amount { get; set; }
        public string Extracolumn08percentage { get; set; }
        public string Extracolumn08amount { get; set; }
        public string Extracolumn09percentage { get; set; }
        public string Extracolumn09amount { get; set; }
        public string Extracolumn10percentage { get; set; }
        public string Extracolumn10amount { get; set; }
        public string Grossmarginpercentage { get; set; }
        public string Grossmarginamount { get; set; }
        public string Grossmarginamountpiece { get; set; }
        public string Lastcostprice { get; set; }
        public string Lastsaleprice { get; set; }
        public string Foc_Cost_Share { get; set; }
        public string Foc_Share_Formula { get; set; }
        public string Foc_Apply_On_All_Items { get; set; }
        public string Extrafields01 { get; set; }
        public string Extrafields02 { get; set; }
        public string Extrafields03 { get; set; }
        public string Extrafields04 { get; set; }
        public string Extrafields05 { get; set; }
        public string Extrafields06 { get; set; }
        public string Extrafields07 { get; set; }
        public string Extrafields08 { get; set; }
        public string Extrafields09 { get; set; }
        public string Extrafields10 { get; set; }
        public string Extrafields11 { get; set; }
        public string Extrafields12 { get; set; }
        public string Extrafields13 { get; set; }
        public string Extrafields14 { get; set; }
        public string Extrafields15 { get; set; }
        public string Extrafields16 { get; set; }
        public string Extrafields17 { get; set; }
        public string Extrafields18 { get; set; }
        public string Extrafields19 { get; set; }
        public string Extrafields20 { get; set; }
        public string Extrafields21 { get; set; }
        public string Extrafields22 { get; set; }
        public string Extrafields23 { get; set; }
        public string Extrafields24 { get; set; }
        public string Extrafields25 { get; set; }
        public string Extrafields26 { get; set; }
        public string Extrafields27 { get; set; }
        public string Extrafields28 { get; set; }
        public string Extrafields29 { get; set; }
        public string Extrafields30 { get; set; }
    
    }


}