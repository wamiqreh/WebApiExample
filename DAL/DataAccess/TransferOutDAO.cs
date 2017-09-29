using DAL.Provider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataAccess
{
   public class TransferOutDAO : IDisposable
    {
        #region Constants
        private static readonly string MERGE_TRF_OUT2 =  "MERGE INTO str_trf_out2 D " + "\n"
 + "   USING (select '{0}' out_# ,'{1}' usrid,'{2}' logno,sysdate edate,'{3}' line#,'{4}' items,'{5}' batch,'{6}' notes,'{7}' barcode,'{8}' grl_#,'{9}' indl#, " + "\n"
 + "  {10} out_q,{11} bon_q,{12} costp,{13} tradp,{14} amont,{15} isu_q,{16} qty_in,{17} unitq, {18} pck_qty ,'{19}' REFNO from dual) S " + "\n"
 + " " + "\n"
 + "   ON ( d.refno  = S.refno) " + "\n"
 + "   WHEN MATCHED THEN UPDATE SET " + "\n"
 + "  d.out_# =s.out_# ,d.usrid=s.usrid,d.logno=s.logno ,d.edate=s.edate,d.line#=s.line# ,d.items=s.items,d.batch=s.batch ,d.out_q=s.out_q " + "\n"
 + "  ,d.bon_q=s.bon_q ,d.costp=s.costp,d.tradp=s.tradp ,d.amont=s.amont,d.notes =s.notes,d.indl# =s.indl#,d.barcode=s.barcode,d.isu_q=s.isu_q " + "\n"
 + "  ,d.grl_#=s.grl_# ,d.qty_in=s.qty_in,d.pck_qty =s.pck_qty " + "\n"
 + "   WHEN NOT MATCHED THEN " + "\n"
 + "  insert (out_#,usrid,logno,edate,line#,items,batch,notes,barcode,grl_#,indl#,out_q,bon_q,costp,tradp,amont,isu_q,qty_in,unitq,pck_qty,REFNO) " + "\n"
 + "  values ('{0}','{1}','{2}',sysdate,'{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},{11},{12},{13},{14},{15},{16},{17},{18},'{19}')";
        private static readonly string MERGE_TRF_OUT1 =  "MERGE INTO str_trf_out1 D " + "\n"
 + "   USING (select  '{0}' compc,'{1}' brnch,'{2}' usrid,'{3}' logno,sysdate edate ,'{4}' out_#, to_date('{5}', 'dd-Mon-yyyy') wdate, to_date('{6}', 'dd-Mon-yyyy') PDATE,'Y' ACTIV,'F' DAMAG,'0001.TI' TRTP#, " + "\n"
 + "   '{7}' STR_F,'{8}' SNT_B,'{9}' STR_T,'{10}' Snd_c,'{11}' SND_B,'{12}' Godwn,'{13}' NOTES,'{14}' APROV,'{15}' DMG_CODE,'{16}' REFNO,'{17}' POSTED, '{18}' dptcd from dual   ) S " + "\n"
 + "   ON ( d.refno  = S.refno) " + "\n"
 + "   WHEN MATCHED THEN UPDATE SET " + "\n"
 + "   d.logno=s.logno,d.edate=s.edate,d.out_#=s.out_#,d.wdate=s.wdate,d.pdate=s.pdate, " + "\n"
 + "   d.activ=s.activ,d.damag=s.damag,d.trtp#=s.trtp#,d.str_f=s.str_f,d.snt_b=s.snt_b,d.str_t=s.str_t,d.snd_c=s.snd_c " + "\n"
 + "   ,d.snd_b=s.snd_b ,d.godwn =s.godwn ,d.notes=s.notes ,d.aprov=s.aprov ,d.dmg_code=s.dmg_code ,d.dptcd =s.dptcd" + "\n"
 + "   WHEN NOT MATCHED THEN " + "\n"
 + "    insert   (compc,brnch,usrid,logno,edate,out_#,wdate,PDATE,ACTIV,DAMAG,TRTP#,STR_F,SNT_B,STR_T,Snd_c,SND_B,Godwn,NOTES,APROV,DMG_CODE,REFNO,POSTED,TTYPE,dptcd) " + "\n"
 + "   values ('{0}','{1}','{2}','{3}',sysdate,'{4}', to_date('{5}', 'dd-Mon-yyyy'), to_date('{6}', 'dd-Mon-yyyy'),'Y','F','0001.TI','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','HHTMOB','{18}')";

        private static readonly string SELECT_MAX_TO = "select STR_GRN_NO('{0}', '{1}', 'TO') FROM DUAL";
        private static readonly string SELECT_MAX_TO2 = "SELECT (LPAD(NVL(MAX(TO_NUMBER(SUBSTR(LINE#,0,5))),0) +1,5,0) || '.' || '{0}')  FROM STR_TRF_OUT2 WHERE  OUT_# ='{0}'";
        private static readonly string SELECT_EMPLOYEE = "SELECT E.ECODE FROM SEC_USERNAME  S INNER JOIN EMP_PERSONAL E ON E.ECODE =S.ECODE WHERE S.USRID ='{0}'";

        private static readonly string SELECT_PREVIOUSTRFOUT = "SELECT S.OUT_# FROM STR_TRF_OUT1 S WHERE S.refno ='{0}'";
        private static readonly string SELECT_PREVIOUSTRFOUT2 = "SELECT K.LINE# FROM STR_TRF_OUT2 K WHERE K.REFNO ='{0}'";

        private static readonly string SELECT_PREVIOUSOUTAPP = "SELECT S.APROV FROM STR_TRF_OUT1 S WHERE S.OUT_# ='{0}'";

        private static readonly string SELECT_OUT_DATE = "SELECT to_char(S.wdate,'dd-Mon-yyyy') FROM STR_TRF_OUT1 S WHERE S.OUT_# ='{0}'";
        private static readonly string SELECT_SENT_BY = "SELECT S.Snt_b  FROM STR_TRF_OUT1 S WHERE S.OUT_# ='{0}'";
        private static readonly string TRFOUT = " select m.Ind_# from STR_INDENT_1  m where m.Ind_#  = '{0}' and m.aprov ='A' ";
      
        #endregion

        #region Variables
        private ConnClass _connClass = null;
        DBHelper _dbHelper = null;

        #endregion

        #region Events
        public object GetRefTRFOUT(string refno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(TRFOUT), refno));
        }
        public object GetSNT_BY(string docno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_SENT_BY), docno));
        }
        public object GetOutDateTo(string docno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_OUT_DATE), docno));
        }
        public string GetMaxTO(string compc, string brnch)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_MAX_TO), compc, brnch)).ToString();
        }
        public string GetMaxTO2( string to)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_MAX_TO2), to)).ToString();
        }
        public object GetEMPLOYECode(string id)
        {
            
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_EMPLOYEE), id));
        }
        public object GetOutApproval(string grnno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PREVIOUSOUTAPP), grnno));
        }
        public object GetTO(string grnno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PREVIOUSTRFOUT), grnno));
        }
        public object GetTO2(string lineno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PREVIOUSTRFOUT2), lineno));
        }
        public bool InsertTransferOut1(string compc, string brnch, string usrid, string logno, string outNO,string  wdate,string  PDATE,  string STR_F, string SNT_B, string STR_T, string Snd_c, string SND_B, string Godwn, string NOTES, string APROV, string DMG_CODE, string REFNO,string POSTED,string dptcd)
        {
           
             
                int rowsEfect = _connClass.Execute_DML( string.Format(MERGE_TRF_OUT1,
                           compc, brnch, usrid, logno, outNO,wdate,PDATE,STR_F,SNT_B,STR_T,Snd_c,SND_B,Godwn,NOTES,APROV,DMG_CODE,REFNO,POSTED,dptcd));
                return (rowsEfect > 0) ? true : false;
           

              
        }
        public bool InsertTransferOut2(string outno, string usrid, string logno, string lineno, string items, string batch, string notes, string barcode, string grlno, string indlno, string out_q, string bon_q, string costp, string tradp, string amont, string isu_q, string qty_in, string unitq, string pck_qty, string REFNO)
        {
           

                int rowsEfect = _connClass.Execute_DML( string.Format(MERGE_TRF_OUT2,
                           outno,usrid,logno,lineno,items,batch,notes,barcode,grlno,indlno,out_q,bon_q,costp,tradp,amont,isu_q,qty_in,unitq,pck_qty,REFNO));
                return (rowsEfect > 0) ? true : false;
            
        }


        #endregion

        #region Constructor
        public TransferOutDAO()
        {
            _connClass = new ConnClass();
            _dbHelper = new DBHelper();
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            _connClass = null;
            _dbHelper = null;
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
