using DAL.Provider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataAccess
{
   public class StockAuditDAO : IDisposable
    {
        #region Constants
        private static readonly string MERGE_AUD2 = "MERGE INTO str_audit_02 D "+"\n"
 +"USING (select '{0}' AUD_#, '{1}' USRID,   '{2}' LOGNO, sysdate EDATE, '{3}' LINE#,  '{4}' ITEMS,  {5} COM_Q,  {6} PHY_Q, {7} SHT_Q,   {8} EXC_Q,{9} COSTP,{10} TRADP, "+"\n"
 +"              {11} AMONT, '{12}' NOTES,'{13}' BARCODE, '{14}' ITEMS_NAME, '{15}' S2DL#, '{16}' REFNO from dual) S "+"\n"
 +" "+"\n"
 +"ON (d.refno = S.refno) "+"\n"
 +"WHEN MATCHED THEN "+"\n"
 +"  UPDATE "+"\n"
 +"     SET D.AUD_# = S.AUD_#, D.USRID = S.USRID, D.LOGNO = S.LOGNO, "+"\n"
 +"                   D.EDATE = S.EDATE ,D.LINE# = S.LINE#, D.ITEMS = S.ITEMS, "+"\n"
 +"                   D.COM_Q = S.COM_Q ,D.PHY_Q = S.PHY_Q, D.SHT_Q = S.SHT_Q ,"+"\n"
 +"                   D.EXC_Q = S.EXC_Q ,D.COSTP = S.COSTP, D.TRADP = S.TRADP, "+"\n"
 +"                   D.AMONT = S.AMONT, D.NOTES = S.NOTES, D.BARCODE = S.BARCODE, "+"\n"
 +"                   D.ITEMS_NAME = S.ITEMS_NAME, D.S2DL# = S.S2DL# "+"\n"
 +"WHEN NOT MATCHED THEN "+"\n"
 +" "+"\n"
 +"  INSERT "+"\n"
 +"    (AUD_#,     USRID,     LOGNO,     EDATE,     LINE#,     ITEMS,     COM_Q,     PHY_Q,     SHT_Q,     EXC_Q,     COSTP,     TRADP,     AMONT,     NOTES,     BARCODE, "+"\n"
 +"     ITEMS_NAME,     S2DL#,     REFNO) "+"\n"
 +"  values    ('{0}',  '{1}', '{2}',  sysdate, '{3}', '{4}', {5}, {6}, {7}, {8}, {9}, {10}, {11},  '{12}',  '{13}',    '{14}',   '{15}',   '{16}')";
        private static readonly string MERGE_AUD1 = "MERGE INTO str_audit_01 D " + "\n"
 + "USING (Select '{0}' COMPC, '{1}' BRNCH, '{2}' USRID, '{3}' LOGNO, sysdate EDATE, '{4}' AUD_#, to_date('{5}', 'dd-Mon-yyyy') WDATE, to_date('{6}', 'dd-Mon-yyyy') PDATE,    to_date('{7}', 'dd-Mon-yyyy') AUD_S, " + "\n"
 + "              to_date('{8}', 'dd-Mon-yyyy') AUD_E, '{9}' ACTIV,  '{10}' AUDTP, '{11}' AUD_B,  '{12}' WHINC, '{13}' STR_F, '{14}' GODWN,'{15}' REFNO,'{16}' NOTES, '{17}' APROV, " + "\n"
 + "              '{18}' DAMAG, '{19}' DMG_CODE     from dual) S " + "\n"
 + "ON (d.AUD_# = S.AUD_#) " + "\n"
 + "WHEN MATCHED THEN " + "\n"
 + "  UPDATE " + "\n"
 + "     SET D.LOGNO = S.LOGNO ,D.EDATE = S.EDATE  " + "\n";
        //+ "                   D.WDATE = S.Wdate ,D.PDATE = S.Pdate ,D.AUD_S = S.Aud_s, " + "\n"
        //+ "                   D.AUD_E = S.AUD_E, D.ACTIV = S.Activ, D.AUDTP = S.AUDTP, " + "\n"
        //+ "                   D.AUD_B = S.Aud_b ,D.WHINC = S.Whinc ,D.STR_F = S.Str_f, " + "\n"
        //+ "                   D.GODWN = S.Godwn  ,D.NOTES = S.Notes, " + "\n"
        //+ "                   D.APROV = S.Aprov ,D.DAMAG = S.Damag, " + "\n"
        //+ "                   D.DMG_CODE = S.Dmg_Code " + "\n"
        //+ "   " + "\n"
        //+ " " + "\n"
        //+ "WHEN NOT MATCHED THEN " + "\n"
        //+ "  INSERT " + "\n"
        //+ "    (COMPC, BRNCH, USRID,LOGNO, EDATE, AUD_#, WDATE, PDATE, AUD_S, AUD_E, ACTIV, AUDTP, AUD_B,WHINC,STR_F, GODWN,REFNO,NOTES,APROV,DAMAG,   DMG_CODE) " + "\n"
        //+ "  values " + "\n"
        //+ "    ('{0}','{1}','{2}','{3}', sysdate, '{4}',  to_date('{5}', 'dd-Mon-yyyy'),  to_date('{6}', 'dd-Mon-yyyy'), to_date('{7}', 'dd-Mon-yyyy'),to_date('{8}', 'dd-Mon-yyyy'), " + "\n"
        //+ "     '{9}',  '{10}',  '{11}', '{12}','{13}', '{14}',  '{15}', '{16}', '{17}','{18}', '{19}')";
        string INSERT_AUDPRODUCTS = "  INSERT INTO  str_audit_02 " + "\n"
 + "    (AUD_#,     USRID,     LOGNO,     EDATE,     LINE#,     ITEMS,     COM_Q,     PHY_Q,     SHT_Q,     EXC_Q,     COSTP,     TRADP,     AMONT,     NOTES,     BARCODE, " + "\n"
 + "     ITEMS_NAME,     S2DL#,     REFNO) " + "\n"
 + "  values    ('{0}',  '{1}', '{2}',  sysdate, '{3}', '{4}', {5}, {6}, {7}, {8}, {9}, {10}, {11},  '{12}',  '{13}',    '{14}',   '{15}',   '{16}')";

        private static readonly string SELECT_MAX_AUD = "select STR_GRN_NO('{0}', '{1}', 'CS') FROM DUAL";
        private static readonly string SELECT_MAX_AUD2 = "SELECT (LPAD(NVL(MAX(TO_NUMBER(SUBSTR(LINE#,0,5))),0) +1,5,0) || '.' || '{0}')  FROM STR_AUDIT_02 WHERE  AUD_# ='{0}'";
        private static readonly string SELECT_MAX_AUD4 = "SELECT (LPAD(NVL(MAX(TO_NUMBER(SUBSTR(LINE#,0,5))),0) +1,5,0) || '.' || '{0}')  FROM STR_AUDIT_04 WHERE  AUD_# ='{0}'";
        private static readonly string SELECT_EMPLOYEE = "SELECT E.ECODE FROM SEC_USERNAME  S INNER JOIN EMP_PERSONAL E ON E.ECODE =S.ECODE WHERE S.USRID ='{0}'";

        private static readonly string SELECT_PREVIOUSAUD = "SELECT S.AUD_# FROM STR_AUDIT_01 S WHERE S.AUD_# ='{0}'";
        private static readonly string SELECT_PREVIOUSAUD2 = "SELECT K.LINE# FROM STR_AUDIT_02 K WHERE K.REFNO ='{0}'";
        private static readonly string SELECT_PREVIOUSAUD4 = "SELECT K.LINE# FROM STR_AUDIT_04 K WHERE K.REFNO ='{0}'";
        private static readonly string SELECT_PREVIOUSAUDAPP = "SELECT S.APROV FROM STR_AUDIT_01 S WHERE S.AUD_# ='{0}'";
        private static readonly string SELECT_AUDITDOC = " select aud_# DOCUMENTNO , brnch officecode,str_f WAREHOUSECODE  from str_audit_01  WHERE AUD_# ='{0}'";
        #endregion
        private static readonly string MERGEAUDIT4 = @"   MERGE INTO STR_AUDIT_04 D
 USING(select '{0}' AUD_# ,'{1}' USRID,'{2}'  LOGNO , SYSDATE EDATE, '{3}'   LINE# , '{4}' BARCODE  ,  '{5}' ITEMS,{6}         AUD_Q , '{7}'  NOTES , 
        '{8}'   ITEMS_NAME ,     '{9}'      ACT_BARCD ,   '{10}'    AISLESNO ,    '{11}'       BAYNO ,'{12}' REFNO from dual) S
ON(d.refno = S.refno)
 WHEN MATCHED THEN
   UPDATE
      SET D.AUD_# = S.AUD_# , D.USRID = S.USRID, D.LOGNO = S.LOGNO,
                    D.EDATE = S.EDATE, D.LINE#  = S.LINE# , D.BARCODE = S.BARCODE,
                    D.ITEMS = S.ITEMS, D.AUD_Q = S.AUD_Q, D.NOTES = S.NOTES,
                    D.ITEMS_NAME = S.ITEMS_NAME, D.ACT_BARCD = S.ACT_BARCD,
                
                    D.AISLESNO = S.AISLESNO, D.BAYNO = S.BAYNO


 WHEN NOT MATCHED THEN
 INSERT
    ( AUD_# , USRID, LOGNO , EDATE , LINE#  , BARCODE  , ITEMS, AUD_Q , NOTES , ITEMS_NAME , ACT_BARCD , AISLESNO , BAYNO, REFNO )
   values('{0}','{1}','{2}', sysdate,'{3}' , '{4}', '{5}',  {6}, '{7}','{8}',   '{9}',  '{10}',  '{11}',  '{12}') ";

        private static readonly string DELETE_AUDITPRODUCTS = "delete from str_audit_02 where aud_# ='{0}'";

        private static readonly string SELECT_AUDITPRODUCTS = @"select sum(op.aud_q) phy_qty ,op.aud_#,op.act_barcd barcode,op.items from STR_AUDIT_04 op
inner join str_audit_01 om on om.aud_# =op.aud_# 
 where om.aud_# ='{0}'
 group by op.aud_#,op.act_barcd,op.items";
        #region Variables
        private ConnClass _connClass = null;
        DBHelper _dbHelper = null;

        #endregion

        #region Events
        public DataTable AuditClubProducts(string docno)
        {
            return _dbHelper.DataAdapter(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_AUDITPRODUCTS), docno)).Tables[0];
        }
        public int DeleteAuditPorducts(string docno)
        {
            return _dbHelper.ExecuteNonQuery(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(DELETE_AUDITPRODUCTS), docno));
        }
        public string GetMaxAUD(string compc, string brnch)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_MAX_AUD), compc, brnch)).ToString();
        }
        public string GetMaxAUD2( string to)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_MAX_AUD2), to)).ToString();
        }
        public string GetMaxAUD4(string to)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_MAX_AUD4), to)).ToString();
        }
        
        public object GetEMPLOYECode(string id)
        {
            
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_EMPLOYEE), id));
        }
        public object GetAud(string grnno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PREVIOUSAUD), grnno));
        }
        public object GetAud2(string lineno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PREVIOUSAUD2), lineno));
        }
        public object GetAud4(string lineno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PREVIOUSAUD4), lineno));
        }
        public object GetAudApproval(string grnno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PREVIOUSAUDAPP), grnno));
        }

        public DataTable GetAudFromDocNo(string compc,string brnch,string  docno)
        {
            return _dbHelper.DataAdapter(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_AUDITDOC), docno)).Tables[0];
        }
        public bool InsertAudit01(string COMPC, string BRNCH, string USRID, string LOGNO, string AUDno, string WDATE, string PDATE, string AUD_S, string AUD_E,
            string ACTIV, string AUDTP, string AUD_B, string WHINC, string STR_F, string GODWN, string REFNO, string NOTES , string APROV, string DAMAG, string DMG_CODE)
        {
          
             
                int rowsEfect = _connClass.Execute_DML(string.Format(Utils.Utilities.GenerateProperTableName(MERGE_AUD1),
                           COMPC, BRNCH, USRID, LOGNO, AUDno, WDATE, PDATE, AUD_S, AUD_E, ACTIV, AUDTP, AUD_B,WHINC,STR_F, GODWN,REFNO,NOTES,APROV,DAMAG,   DMG_CODE));
                return (rowsEfect > 0) ? true : false;
           

              
           
        }

        // INSERT_AUDPRODUCTS

        public bool InsertAuditProducts(string AUDno, string USRID, string LOGNO, string LINEno, string ITEMS, string COM_Q, string PHY_Q, string SHT_Q, string EXC_Q, string COSTP, string TRADP, string AMONT, string NOTES, string BARCODE,
string ITEMS_NAME, string S2DL, string REFNO)
        {
           

                int rowsEfect = _connClass.Execute_DML(string.Format(Utils.Utilities.GenerateProperTableName(INSERT_AUDPRODUCTS),
                           AUDno, USRID, LOGNO, LINEno, ITEMS, COM_Q, PHY_Q, SHT_Q, EXC_Q, COSTP, TRADP, AMONT, NOTES, BARCODE,
     ITEMS_NAME, S2DL, REFNO));
                return (rowsEfect > 0) ? true : false;
         
          
            
        }
        public bool InsertAudit02(string AUDno, string USRID, string LOGNO, string LINEno, string ITEMS, string COM_Q,  string PHY_Q, string SHT_Q, string EXC_Q, string COSTP, string TRADP, string AMONT, string NOTES, string BARCODE,
     string ITEMS_NAME, string S2DL, string REFNO)
        {
           

                int rowsEfect = _connClass.Execute_DML(string.Format(Utils.Utilities.GenerateProperTableName(MERGE_AUD2),
                           AUDno, USRID, LOGNO, LINEno, ITEMS, COM_Q, PHY_Q, SHT_Q, EXC_Q, COSTP, TRADP, AMONT, NOTES, BARCODE,
     ITEMS_NAME, S2DL, REFNO));
                return (rowsEfect > 0) ? true : false;
           
            

                
        }
        // MERGEAUDIT4
        public bool InsertAudit04(string AUDNO , string USRID, string LOGNO  , string LINENO , string BARCODE  , string ITEMS, string AUD_Q , string NOTES , string  ITEMS_NAME, string ACT_BARCD , string AISLESNO , string BAYNO, string REFNO )
        {
            
                int rowsEfect = _connClass.Execute_DML(string.Format(Utils.Utilities.GenerateProperTableName(MERGEAUDIT4), AUDNO, USRID, LOGNO, LINENO, BARCODE, ITEMS, AUD_Q, NOTES, ITEMS_NAME, ACT_BARCD, AISLESNO, BAYNO, REFNO
                        ));
                return (rowsEfect > 0) ? true : false;
           
          

               
           
        }
        #endregion

        #region Constructor
        public StockAuditDAO()
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
