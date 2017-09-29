using DAL.Provider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataAccess
{
   public class TransferDAO : IDisposable
    {

        #region Constants
        private static readonly string MERGE_TRFIN2 = "MERGE INTO str_trf_ins2 D " + "\n"
 + "    USING (select '{0}' tin_#,'{1}' usrid,'{2}' logno,sysdate edate,'{3}' line#,'{4}' items,'{5}' batch,to_date('{6}', 'dd-Mon-yyyy') expdt,{7} rec_q,{8} bon_q, " + "\n"
 + "    {9} costp,{10} tradp,{11} amont,'{12}' notes,'{13}' tol_#,'{14}' oline,'{15}' barcode,{16} out_q,'{17}' indl#,'{18}' refno from dual) S " + "\n"
 + "           " + "\n"
 + "    ON ( d.refno  = S.refno) " + "\n"
 + "    WHEN MATCHED THEN UPDATE SET " + "\n"
 + "  d.tin_# =s.tin_# ,d.usrid=s.usrid ,d.logno=s.logno ,d.edate =s.edate,d.line#=s.line# ,d.items=s.items,d.batch=s.batch,d.expdt =s.expdt " + "\n"
 + "  ,d.rec_q=s.rec_q,d.bon_q=s.bon_q,d.costp=s.costp,d.tradp=s.tradp,d.amont=s.amont,d.notes=s.notes,d.tol_#=s.tol_#,d.oline=s.oline " + "\n"
 + "  ,d.barcode=s.barcode,d.out_q=s.out_q,d.indl#=s.indl# " + "\n"
 + "    WHEN NOT MATCHED THEN " + "\n"
 + "  insert  ( tin_#,usrid,logno,edate,line#,items,batch,expdt,rec_q,bon_q,costp,tradp,amont,notes,tol_#,oline,barcode,out_q,indl#,refno) " + "\n"
 + "     values('{0}','{1}','{2}',sysdate,'{3}','{4}','{5}', to_date('{6}', 'dd-Mon-yyyy'),{7},{8},{9},{10},{11},'{12}','{13}','{14}','{15}',{16},'{17}','{18}')";
        private static readonly string MERGE_TRFIN = "MERGE INTO str_trf_ins1 D " + "\n"
 + "    USING (select  '{0}' compc,'{1}' brnch,'{2}' usrid,'{3}' logno,sysdate edate ,'{4}' out_#, to_date('{5}', 'dd-Mon-yyyy') wdate, to_date('{6}', 'dd-Mon-yyyy') PDATE,'Y' ACTIV,'F' DAMAG,'0001.TI' TRTP#, " + "\n"
 + "    '{7}' STR_F,'{8}' rec_b,'{9}' STR_T,'{10}' Snd_c,'{11}' SND_B,'{12}' Godwn,'{13}' NOTES,'{14}' APROV,'{15}' DMG_CODE,'{16}' REFNO ,'{17}' tin_# ,'{18}' Snd_t ,to_date('{19}', 'dd-Mon-yyyy') out_d,'{20}' POSTED  from dual   ) S " + "\n"
 + "    ON ( d.refno  = S.refno) " + "\n"
 + "    WHEN MATCHED THEN UPDATE SET " + "\n"
 + "    d.compc =s.compc ,d.brnch=s.brnch,d.usrid=s.usrid,d.logno=s.logno,d.edate=s.edate,d.out_#=s.out_#,d.wdate=s.wdate,d.pdate=s.pdate, " + "\n"
 + "    d.activ=s.activ,d.damag=s.damag,d.trtp#=s.trtp#,d.str_f=s.str_f,d.rec_b=s.rec_b,d.str_t=s.str_t,d.snd_c=s.snd_c " + "\n"
 + "    ,d.snd_b=s.snd_b ,d.godwn =s.godwn ,d.notes=s.notes ,d.aprov=s.aprov ,d.dmg_code=s.dmg_code ,d.Snd_t=s.Snd_t,d.out_d=s.out_d    " + "\n"
 + "    WHEN NOT MATCHED THEN " + "\n"
 + "     insert   (compc,brnch,usrid,logno,edate,out_#,wdate,PDATE,ACTIV,DAMAG,TRTP#,STR_F,rec_b,STR_T,Snd_c,SND_B,Godwn,NOTES,APROV,DMG_CODE,REFNO,tin_#,Snd_t ,out_D,POSTED,TTYPE ) " + "\n"
 + "    values ('{0}','{1}','{2}','{3}',sysdate,'{4}', to_date('{5}', 'dd-Mon-yyyy'), to_date('{6}', 'dd-Mon-yyyy'),'Y','F','0001.TI','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}',to_date('{19}', 'dd-Mon-yyyy'),'{20}','HHTMOB')";
        private static readonly string TRFIN = " select m.out_# from str_trf_out1@getho  m where m.out_#  = '{0}' and m.aprov ='A' ";
      
        
        private static readonly string MERGE_TRFINWOR = "MERGE INTO str_trf_ins1 D " + "\n"
 + "     USING (select  '{0}' compc,'{1}' brnch,'{2}' usrid,'{3}' logno,sysdate edate , to_date('{4}', 'dd-Mon-yyyy') wdate, to_date('{5}', 'dd-Mon-yyyy') PDATE,'Y' ACTIV,'F' DAMAG,'0001.TI' TRTP#, " + "\n"
 + "     '{6}' STR_F,'{7}' rec_b,'{8}' STR_T,'{9}' Snd_c,'{10}' SND_B,'{11}' Godwn,'{12}' NOTES,'{13}' APROV,'{14}' DMG_CODE,'{15}' REFNO ,'{16}' tin_# ,'{17}' POSTED  from dual   ) S " + "\n"
 + "     ON ( d.refno  = S.refno) " + "\n"
 + "     WHEN MATCHED THEN UPDATE SET " + "\n"
 + "     d.logno=s.logno,d.edate=s.edate,d.wdate=s.wdate,d.pdate=s.pdate, " + "\n"
 + "     d.activ=s.activ,d.damag=s.damag,d.trtp#=s.trtp#,d.str_f=s.str_f,d.rec_b=s.rec_b,d.str_t=s.str_t,d.snd_c=s.snd_c " + "\n"
 + "     ,d.snd_b=s.snd_b ,d.godwn =s.godwn ,d.notes=s.notes ,d.aprov=s.aprov ,d.dmg_code=s.dmg_code " + "\n"
 + "     WHEN NOT MATCHED THEN " + "\n"
 + "      insert   (compc,brnch,usrid,logno,edate,wdate,PDATE,ACTIV,DAMAG,TRTP#,STR_F,rec_b,STR_T,Snd_c,SND_B,Godwn,NOTES,APROV,DMG_CODE,REFNO,tin_# ,POSTED,TTYPE ) " + "\n"
 + "     values ('{0}','{1}','{2}','{3}',sysdate, to_date('{4}', 'dd-Mon-yyyy'), to_date('{5}', 'dd-Mon-yyyy'),'Y','F','0001.TI','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','HHTMOB')";

        private static readonly string MERGE_TRFINWOR2 = "MERGE INTO str_trf_ins2 D " + "\n"
 + "    USING (select '{0}' tin_#,'{1}' usrid,'{2}' logno,sysdate edate,'{3}' line# ,'{4}' items,'{5}' batch,to_date('{6}', 'dd-Mon-yyyy') expdt,{7} rec_q,{8} bon_q, " + "\n"
 + "    {9} costp,{10} tradp,{11} amont,'{12}' notes,'{13}' barcode,{14} out_q,'{15}' indl#,'{16}' refno from dual) S " + "\n"
 + "           " + "\n"
 + "    ON ( d.refno  = S.refno) " + "\n"
 + "    WHEN MATCHED THEN UPDATE SET " + "\n"
 + "  d.tin_# =s.tin_# ,d.usrid=s.usrid ,d.logno=s.logno ,d.edate =s.edate,d.line#=s.line# ,d.items=s.items,d.batch=s.batch,d.expdt =s.expdt " + "\n"
 + "  ,d.rec_q=s.rec_q,d.bon_q=s.bon_q,d.costp=s.costp,d.tradp=s.tradp,d.amont=s.amont,d.notes=s.notes " + "\n"
 + "  ,d.barcode=s.barcode,d.out_q=s.out_q,d.indl#=s.indl# " + "\n"
 + "    WHEN NOT MATCHED THEN " + "\n"
 + "  insert  ( tin_#,usrid,logno,edate,line#,items,batch,expdt,rec_q,bon_q,costp,tradp,amont,note,barcode,out_q,indl#,refno) " + "\n"
 + "     values('{0}','{1}','{2}',sysdate,'{3}','{4}','{5}', to_date('{6}', 'dd-Mon-yyyy'),{7},{8},{9},{10},{11},'{12}','{13}',{14},'{15}','{16}')";

        private static readonly string MERGE_TRFIN03 = "MERGE INTO str_trf_ins4 D " + "\n"
 + "    USING (select '{0}' tin_#,'{1}' line_no_tin2,'{2}' Usrid,'{3}' logno,sysdate edate,'{4}' line_no,{5} rej_q,'{6}' rtncd,'{7}'notes,'{8}'refno from dual) S " + "\n"
 + "     ON ( d.refno  = S.refno) " + "\n"
 + "     WHEN MATCHED THEN UPDATE SET " + "\n"
 + " d.tin_# =s.tin_# ,d.line_no_tin2=s.line_no_tin2 ,d.usrid =s.usrid ,d.edate=s.edate ,d.line_no=s.line_no " + "\n"
 + " ,d.rej_q=s.rej_q,d.rtncd=s.rtncd,d.notes=s.notes " + "\n"
 + "     WHEN NOT MATCHED THEN " + "\n"
 + "    INSERT (tin_#,line_no_tin2,Usrid,logno,edate,line_no,rej_q,rtncd,notes,refno) " + "\n"
 + "      values('{0}','{1}','{2}','{3}',sysdate,'{4}',{5},'{6}','{7}','{8}')";
        private static readonly string SELECT_MAX_TRFIN= "select STR_GRN_NO('{0}', '{1}', 'TI') FROM DUAL";
        private static readonly string SELECT_MAX_TRFIN2 = "SELECT (LPAD(NVL(MAX(TO_NUMBER(SUBSTR(LINE#,0,5))),0) +1,5,0) || '.' || '{0}')  FROM STR_TRF_INS2 WHERE  TIN_# ='{0}'";
        private static readonly string SELECT_MAX_TRFIN3 = "SELECT (LPAD(NVL(MAX(TO_NUMBER(SUBSTR(LINE_no,0,3))),0) +1,3,0) || '.' || '{0}')  FROM STR_TRF_INS4 WHERE  LINE_NO_TIN2 ='{0}'";
        private static readonly string SELECT_EMPLOYEE = "SELECT E.ECODE FROM SEC_USERNAME  S INNER JOIN EMP_PERSONAL E ON E.ECODE =S.ECODE WHERE S.USRID ='{0}'";

        private static readonly string SELECT_PREVIOUSTRFIN = "SELECT S.TIN_# FROM STR_TRF_INS1 S WHERE S.refno ='{0}'";
        private static readonly string SELECT_PREVIOUSTRFIN2 = "SELECT K.LINE# FROM STR_TRF_INS2 K WHERE K.REFNO ='{0}'";
        private static readonly string SELECT_PREVIOUSTRFIN3 = "SELECT K.LINE_no FROM STR_TRF_INS4 K WHERE K.REFNO ='{0}'";

        private static readonly string SELECT_PREVIOUSINAPP = "SELECT S.APROV FROM STR_TRF_INS1 S WHERE S.TIN_# ='{0}'";
        private static readonly string SELECT_RESON_TYP = "select CODE# ID,DESCR NAME from com_codefile where type#='TIR'";
        
        #endregion

        #region Variables
        private ConnClass _connClass = null;
        DBHelper _dbHelper = null;

        #endregion

        #region Events
        public DataTable GetReasonsType()
        {
            
            return _dbHelper.DataAdapter(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_RESON_TYP))).Tables[0];
        }
        public object GetRefTRFIN(string refno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(TRFIN), refno));
        }
       
        public string GetMaxTRFIN(string compc, string brnch)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_MAX_TRFIN), compc, brnch)).ToString();
        }
        public string GetMaxTRFIN2( string to)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_MAX_TRFIN2), to)).ToString();
        }
        public string GetMaxTRFIN3(string to)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_MAX_TRFIN3), to)).ToString();
        }
        public object GetEMPLOYECode(string id)
        {

            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_EMPLOYEE), id));
        }
        public object GetTRFIN(string grnno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PREVIOUSTRFIN), grnno));
        }
        public object GetTRFIN2(string lineno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PREVIOUSTRFIN2), lineno));
        }
        public object GetTRFIN3(string lineno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PREVIOUSTRFIN3), lineno));
        }
        public object GetInApproval(string grnno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text,  string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PREVIOUSINAPP), grnno));
        }
        public bool InsertTRFIN01(string compc, string brnch, string usrid, string logno, string outNO, string wdate, string PDATE, string STR_F, string SNT_B, string STR_T, string Snd_c, string SND_B, string Godwn, string NOTES, string APROV, string DMG_CODE, string REFNO,string inno,string
             sentby ,string outdate,string posted)
        {
           

                int rowsEfect = _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(MERGE_TRFIN),
                           compc, brnch, usrid, logno, outNO, wdate, PDATE, STR_F, SNT_B, STR_T, Snd_c, SND_B, Godwn, NOTES, APROV, DMG_CODE, REFNO, inno,sentby,outdate,posted));
                return (rowsEfect > 0) ? true : false;
           
           
        }
        public bool InsertTRFIN02(string tinno, string usrid, string logno, string lineno, string items, string batch, string  expdt, string rec_q, string  bon_q, string costp,
            string tradp, string amont, string notes, string tolno, string oline, string barcode, string out_q, string indlno, string refno)
        {
           

                int rowsEfect = _connClass.Execute_DML( string.Format(Utils.Utilities.GenerateProperTableName(MERGE_TRFIN2),
                         tinno,usrid,logno,lineno,items,batch,expdt,rec_q,bon_q,costp,tradp,amont,notes,tolno,oline,barcode,out_q,indlno,refno));
                return (rowsEfect > 0) ? true : false;
           
           
        }

        public bool InsertTRFIN01WOR(string compc, string brnch, string usrid, string logno, string wdate, string PDATE, string STR_F, string SNT_B, string STR_T, string Snd_c, string SND_B, string Godwn, string NOTES, string APROV, string DMG_CODE, string REFNO,string inno,string posted)
        {
            

                int rowsEfect = _connClass.Execute_DML( string.Format(MERGE_TRFINWOR,
                           compc, brnch, usrid, logno, wdate, PDATE, STR_F, SNT_B, STR_T, Snd_c, SND_B, Godwn, NOTES, APROV, DMG_CODE, REFNO, inno,posted));
                return (rowsEfect > 0) ? true : false;
           

             
        }
        public bool InsertTRFIN02WOR(string tinno, string usrid, string logno, string lineno, string items, string batch, string expdt, string rec_q, string bon_q, string costp,
            string tradp, string amont, string notes, string barcode, string out_q, string indlno, string refno)
        {
           
                int rowsEfect = _connClass.Execute_DML( string.Format(MERGE_TRFINWOR2,
                         tinno, usrid, logno, lineno, items, batch, expdt, rec_q, bon_q, costp, tradp, amont, notes, barcode, indlno, refno));
                return (rowsEfect > 0) ? true : false;
           
            
        }



        public bool InsertTRFIN03(string tinno, string line_no_tin2, string Usrid, string logno, string line_no, string rej_q, string rtncd, string notes, string  refno)
        {
            

                int rowsEfect = _connClass.Execute_DML( string.Format(MERGE_TRFIN03,
                         tinno,line_no_tin2,Usrid,logno,line_no,rej_q,rtncd,notes,refno));
                return (rowsEfect > 0) ? true : false;
           
        }


        #endregion

        #region Constructor
        public TransferDAO()
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
