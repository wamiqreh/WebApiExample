using Common.Utilities;
using SND.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SND.Models
{

    public class TransferIn3 : BASE
    {


        #region Properties
        public string ParentId { get; set; }
        public override string ToString() { return "StringItem object ValueType=" + this.ValueType; }
        public string Id;
        public string Maxid;
        public string Timestamp;
        public string Logincode;
        public string Usercode;
        public string Stockin01id;
        public string Stockin02id;
        public string Rejectionqty;
        public string Stockrejectionreasoncode;
        public string Reason;
        public string Rejectionqty1;
        public string Rejectionqty2;
        public string Rejectionqty3;
        public string Details;
        public string ProductName;
        public string ProductBarcode;
        public string Isdeleted { get; set; }

        public string ParentTableName = "STR_STOCKIN01MASTER";
        #endregion



        #region Functions
        public TransferIn3()
        {

            NeedFK = true;
        }
        public string GetPK(string IN)
        {



            using (DAL.DataAccess.TransferDAO conext = new DAL.DataAccess.TransferDAO())
            {
                object Getto = conext.GetTRFIN3(this.Id);
                if (Getto == null)
                {


                    string key = conext.GetMaxTRFIN3(IN);
                    if (string.IsNullOrEmpty(key)) { return null; } else { return key; }
                }

                else { return Getto.ToString(); }
            }
        }
        public override ReponseFormat Insert(List<ReponseFormat> rp ,List<ErrorStack> errStack)
        {
            bool rowsEffetced = false;
            try
            {
                // return new ReponseFormat() { ID = this.Id, MapCode = pk, TableName = "GOODRECIEVE" };
                var MasterMain = rp.Where(m => m.TableName == "STR_STOCKIN01MASTER" && m.ID == this.ParentId).FirstOrDefault();
                var Master = rp.Where(m => m.TableName == "STR_STOCKIN02PRODUCTS" && m.IsError == "N" && (m.MapCode == this.Stockin02id || m.ID == this.Stockin02id)).FirstOrDefault();
                if (Master != null)
                {
                    string pk = GetPK(Master.MapCode);

                    if (MasterMain != null)
                    {

                        using (DAL.DataAccess.TransferDAO conext = new DAL.DataAccess.TransferDAO())
                        {

                            //  ,rec_q,bon_q,costp,tradp

                            if (conext.InsertTRFIN03(MasterMain.MapCode, Master.MapCode, this.Usercode, this.Logincode, pk, this.Rejectionqty, this.Stockrejectionreasoncode, this.Reason, this.Id)

                               )
                            {
                                rowsEffetced = true;
                                Master.childRecords++;
                                Master.Childerns.Add(new ReponseFormatChildern() { DeleteOrder = 3, ID = this.Id, ERPTableName = "str_trf_ins4", MapCode = pk, UserAddedby = this.Usercode, isDeleted = (this.Isdeleted != null) ? (this.Isdeleted.ToString() == "Y") ? "Y" : "N" : "N" });

                            }
                        }

                    }
                    if (rowsEffetced) { return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.GenericSuccessMsg, this.Isdeleted.ToString(), "N"); }
                    else {


                        return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.NoRowsEffecttionMsg, this.Isdeleted.ToString(), "Y");
                    }
                   
                }
                else { return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.NoParentMsg, this.Isdeleted.ToString(), "Y"); }
            }
            catch(Exception ex) {
                Logger.CreateLog(ex.Message.ToString());
                return new ReponseHandler().GenerateResponse(this.Id, null, this.GetType(), this.Usercode, MessageHandler.GenericErrorMsg, this.Isdeleted.ToString(), "Y");
            }
        }

        #endregion
      
    }
}