using DAL.Provider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataAccess
{
   public class IndentDAO : IDisposable
    {
        #region Constants
        private static readonly String MERGE_INDT = @"
         MERGE INTO STR_INDENT_1 D 
 USING (SELECT 
       '{0}'  COMPC, '{1}' BRNCH, '{2}'  USRID ,'{3}'  LOGNO,'{4}' IND_#,to_date('{5}', 'dd-Mon-yyyy')  WDATE,'{6}'  ACTIV, '{7}'  APROV,
       '{8}'  REQBY,'{9}'  STR_F,'{10}'  STR_T,'{11}'  RSN_#,'{12}'  REFNO,'{13}'  NOTES,(SELECT MAX(SCODE) FROM COM_DEPTSECT WHERE DEFLT ='Y' AND ROWNUM=1)  DPTSC,(SELECT MAX(DCODE) FROM COM_DPARTMNT WHERE DEFLT ='Y' AND ROWNUM=1)   DPTCD      
,'{14}' POSTED       
from dual) S 
 ON (d.IND_#= S.IND_#) 
 WHEN MATCHED THEN 
   UPDATE 
      SET D.LOGNO=  S.LOGNO,D.EDATE=SYSDATE
      ,D.WDATE=  S.WDATE,D.ACTIV=  S.ACTIV,D.APROV=  S.APROV,
      D.REQBY =  S.REQBY,D.STR_F=  S.STR_F,D.STR_T=  S.STR_T,D.RSN_#= S.RSN_#,D.REFNO=  S.REFNO,D.NOTES=  S.NOTES
      
    WHEN NOT MATCHED THEN
 INSERT (EDATE,COMPC,  BRNCH,   USRID , LOGNO,IND_#,  WDATE,  ACTIV,  APROV,
         REQBY,  STR_F,  STR_T,  RSN_#,  REFNO,  NOTES,  DPTSC,   DPTCD ,POSTED,TTYPE )
         VALUES (  SYSDATE,'{0}'  , '{1}' , '{2}'   ,'{3}'  ,'{4}'  ,to_date('{5}', 'dd-Mon-yyyy')  ,'{6}'  ,'{7}'  ,
       '{8}'  ,'{9}'  ,'{10}'  ,'{11}'  ,'{12}'  ,'{13}'  
       ,(SELECT MAX(SCODE) FROM COM_DEPTSECT WHERE DEFLT ='Y' AND ROWNUM=1) 
        ,(SELECT MAX(DCODE) FROM COM_DPARTMNT WHERE DEFLT ='Y' AND ROWNUM=1),'{14}','HHTMOB' )";
 //       private static readonly string MERGE_INDT = @"
 //      MERGE INTO STR_INDENT_1 D 
 //USING (SELECT 
 //      '{0}'  COMPC, '{1}' BRNCH, '{2}'  USRID ,'{3}'  LOGNO,SYSDATE EDATE,'{4}'  IND_#,to_date('{5}', 'dd-Mon-yyyy')  WDATE,'{6}'  ACTIV,'{7}'  APROV,
 //      '{8}'  REQBY,'{8}'  STR_F,'{9}'  STR_T,'{10}'  RSN_#,'{11}'  REFNO,'{12}'  NOTES,(SELECT MAX(SCODE) FROM COM_DEPTSECT WHERE DEFLT ='Y' AND ROWNUM=1)  DPTSC,(SELECT MAX(DCODE) FROM COM_DPARTMNT WHERE DEFLT ='Y' AND ROWNUM=1)   DPTCD      
 //      from dual) S 
 //ON (d.IND_#= S.IND_#) 
 //WHEN MATCHED THEN 
 //  UPDATE 
 //     SET D.COMPC =  S.COMPC, D.BRNCH = S.BRNCH, D.USRID=  S.USRID ,D.LOGNO=  S.LOGNO,D.EDATE=SYSDATE
 //     ,D.WDATE=  S.WDATE,D.ACTIV=  S.ACTIV,D.APROV=  S.APROV,
 //     D.REQBY =  S.REQBY,D.STR_F=  S.STR_F,D.STR_T=  S.STR_T,D.RSN_#= S.RSN_#,D.REFNO=  S.REFNO,D.NOTES=  S.NOTES

        //   WHEN NOT MATCHED THEN
        //INSERT (COMPC,  BRNCH,   USRID , LOGNO,IND_#,  WDATE,  ACTIV,  APROV,
        //        REQBY,  STR_F,  STR_T,  RSN_#,  REFNO,  NOTES,  DPTSC,   DPTCD  )
        //        VALUES (  '{0}'  , '{1}' , '{2}'   ,'{3}'  ,'{4}'  ,to_date('{5}', 'dd-Mon-yyyy')  ,'{6}'  ,'{7}'  ,
        //      '{8}'  ,'{8}'  ,'{9}'  ,'{10}'  ,'{11}'  ,'{12}'  
        //      ,(SELECT MAX(SCODE) FROM COM_DEPTSECT WHERE DEFLT ='Y' AND ROWNUM=1) 
        //       ,(SELECT MAX(DCODE) FROM COM_DPARTMNT WHERE DEFLT ='Y' AND ROWNUM=1) )  ";




        private static readonly string MERGE_INDT2 = @"MERGE INTO STR_INDENT_2 D 
 USING (SELECT 
      '{0}' IND_#,'{1}' USRID,'{2}' LOGNO,SYSDATE EDATE,'{3}' LINE#,'{4}' ITEMS,
      '{5}' I_H_Q,'{6}' IND_Q,'{7}' IND_R,'{8}' IND_V,to_date('{9}', 'dd-Mon-yyyy') DELDT,'{10}' NOTES,'{11}' BARCODE,'{12}' TRADP  ,'{13}' REFNO  
       from dual) S 
 ON (d.REFNO= S.REFNO) 
 WHEN MATCHED THEN 
   UPDATE 
      SET  D.IND_#= S.IND_#,D.USRID= S.USRID,D.LOGNO= S.LOGNO,D.EDATE= SYSDATE,D.LINE#= S.LINE#,D.ITEMS= S.ITEMS,
      D.I_H_Q= S.I_H_Q,D.IND_Q= S.IND_Q,D.IND_R= S.IND_R,D.IND_V= S.IND_V,D.DELDT= S.DELDT,D.NOTES= S.NOTES,D.BARCODE= S.BARCODE,D.TRADP= S.TRADP   
      
    WHEN NOT MATCHED THEN
 INSERT (EDATE,IND_#, USRID, LOGNO, LINE#, ITEMS,
       I_H_Q, IND_Q, IND_R, IND_V, DELDT, NOTES, BARCODE, TRADP  , REFNO    )
         VALUES ( SYSDATE, '{0}' ,'{1}' ,'{2}' ,'{3}' ,'{4}' ,
      '{5}' ,'{6}' ,'{7}' ,'{8}' ,to_date('{9}', 'dd-Mon-yyyy') ,'{10}' ,'{11}' ,'{12}'   ,'{13}'   )        ";
        private static readonly string SELECT_MAX_INT = "select STR_GRN_NO('{0}', '{1}', 'G') FROM DUAL";
        private static readonly string SELECT_MAX_INT2 = "SELECT (LPAD(NVL(MAX(TO_NUMBER(SUBSTR(LINE#,0,5))),0) +1,5,0) || '.' || '{0}')  FROM str_indent_2 WHERE  IND_# ='{0}'";
       
        private static readonly string SELECT_EMPLOYEE = "SELECT E.ECODE FROM SEC_USERNAME  S INNER JOIN EMP_PERSONAL E ON E.ECODE =S.ECODE WHERE S.USRID ='{0}'";

        private static readonly string SELECT_PREVIOUSINT = "SELECT S.IND_# FROM str_indent_1 S WHERE S.refno ='{0}'";
        private static readonly string SELECT_PREVIOUSINT2 = "SELECT K.LINE# FROM str_indent_2 K WHERE K.REFNO ='{0}'";
       
        private static readonly string SELECT_PREVIOUSINDAPP = "SELECT S.APROV FROM str_indent_1 S WHERE S.IND_# ='{0}'";
        
        #endregion
        
       
        
        #region Variables
        private ConnClass _connClass = null;
        DBHelper _dbHelper = null;

        #endregion

        #region Events
       
        
        public string GetMaxIND(string compc, string brnch)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_MAX_INT), compc, brnch)).ToString();
        }
        public string GetMaxIND2( string to)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_MAX_INT2), to)).ToString();
        }
        
        
        public object GetEMPLOYECode(string id)
        {
            
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_EMPLOYEE), id));
        }
        public object GetIND(string grnno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PREVIOUSINT), grnno));
        }
        public object GetIND2(string lineno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PREVIOUSINT2), lineno));
        }
       
        public object GetINDApproval(string grnno)
        {
            return _dbHelper.ExecuteScalar(CommandType.Text, string.Format(Utils.Utilities.GenerateProperTableName(SELECT_PREVIOUSINDAPP), grnno));
        }

       
        public bool InsertIndent01(string COMPC, string BRNCH, string USRID , string LOGNO, string INDNO, string WDATE, string ACTIV, string APROV,
        string REQBY, string STR_F, string STR_T, string RSNNO, string REFNO, string NOTES,string POSTED)
        {
           

                       int rowsEfect = _connClass.Execute_DML(string.Format(Utils.Utilities.GenerateProperTableName(MERGE_INDT),
                                  COMPC, BRNCH, USRID, LOGNO, INDNO,  WDATE,  ACTIV,  APROV,
                REQBY, STR_F, STR_T, RSNNO,  REFNO,  NOTES,POSTED));
         //       int rowsEfect = _connClass.Execute_DML(string.Format(Utils.Utilities.GenerateProperTableName( COMPC, BRNCH, USRID, LOGNO, INDNO,  WDATE,  ACTIV,  APROV,
         //REQBY, STR_F, STR_T, RSNNO,  REFNO,  NOTES));
                return (rowsEfect > 0) ? true : false;
           
         

             
           
        }

        // INSERT_AUDPRODUCTS

  
        public bool InsertIndent02(string INDNO, string USRID, string LOGNO, string LINENO, string ITEMS,
      string I_H_Q, string IND_Q, string IND_R, string IND_V, string DELDT, string NOTES, string BARCODE, string TRADP, string REFNO)
        {
            

                int rowsEfect = _connClass.Execute_DML(string.Format(Utils.Utilities.GenerateProperTableName(MERGE_INDT2),
                           INDNO, USRID, LOGNO, LINENO, ITEMS,
       I_H_Q, IND_Q, IND_R, IND_V, DELDT, NOTES, BARCODE, TRADP, REFNO));
                return (rowsEfect > 0) ? true : false;
          
           
            
        }
        // MERGEAUDIT4
      
        #endregion

        #region Constructor
        public IndentDAO()
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
