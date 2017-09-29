using DAL.Provider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Utils
{
    public enum enPolicyType
    {
        BranchWise, UserWise, RctpWise
    }
    public static class PolicyDetails
    {
        public static string CheckIFPORequired ="Device PO Is Required For GRN";
        public static string CheckTollarance = "Device Enable Tolerance For GRN";
        public static string AllowToPunchItemBlindlyGoodsR = "Device Allow Blind Punching For GRN";
        public static string AllowToPunchItemBlindlyIN = "Device Allow Blind Punching For Transfer In";
        public static string CheckIFOutRequired = "Device Tanster Out Is Required For Transfer In";
        public static string CheckINQtyExceed = "Device Transfer In Can Qty Exceed Out Qty";
        public static string CheckIFIndRequired = "Device Indent Is Required For Tanster Out";
        public static string AllowToPunchItemBlindlyOUT = "Device Allow Blind Punching For Transfer OUT";
        public static string CheckOUTQtyExceed = "Device Indent Is Required For Tanster Out";
        public static object[] IsPolicyTrueOrFalse(string PolicyName, enPolicyType typ, string usrBrnch, string userComp, string userID)
        {
            object trueOrFalse = false;

            string Qry = string.Empty;
            try
            {

                switch (typ)
                {
                    case enPolicyType.BranchWise:
                        Qry = string.Format("SELECT ACTIV, DEFVAL, VALTYPE FROM SAL_POS_POLICY WHERE COMPC ='{0}' AND BRNCH ='{1}' AND DESCR ='{2}'", userComp, usrBrnch, PolicyName);
                        break;
                    case enPolicyType.UserWise:
                        Qry = string.Format("SELECT  A.ACTIV,A.VAL AS DEFVAL,B.VALTYPE    FROM SAL_POS_USER_POLICY A INNER JOIN SAL_POS_POLICY B ON A.POLCY=B.POLCY WHERE B.COMPC='{0}'  AND A.POLUSRID ='{2}' AND B.ACTIV='Y' AND B.BRNCH='{1}' AND B.DESCR ='{3}'", userComp, usrBrnch, userID, PolicyName);
                        break;
                    case enPolicyType.RctpWise:
                        Qry = string.Format("SELECT  ACTIV,DEFVAL, VALTYPE  FROM SAL_POS_RCPT_POLCY WHERE COMPC ='{0}' AND BRNCH ='{1}' AND DESCR ='{2}'", userComp, usrBrnch, PolicyName);
                        break;
                    default:
                        break;
                }

                DBHelper my_func = new DBHelper();

                System.Data.DataTable polcyDT = my_func.DataAdapter(CommandType.Text, Qry).Tables[0];
                object[] obj = new object[3];
                if (polcyDT != null && polcyDT.Rows.Count > 0)
                {
                    //isActive
                    obj[0] = !string.IsNullOrWhiteSpace(Convert.ToString(polcyDT.Rows[0]["ACTIV"])) ? (Convert.ToString(polcyDT.Rows[0]["ACTIV"]) == "Y" ? true : false) : false;
                    if (Convert.ToBoolean(obj[0]))
                    {
                        //PolicyVal
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(polcyDT.Rows[0]["DEFVAL"])) && !string.IsNullOrWhiteSpace(Convert.ToString(polcyDT.Rows[0]["VALTYPE"])))
                        {
                            switch (Convert.ToString(polcyDT.Rows[0]["VALTYPE"]))
                            {
                                case "N":
                                    obj[1] = Convert.ToDecimal(polcyDT.Rows[0]["DEFVAL"].ToString());
                                    break;
                                case "B":
                                    obj[1] = Convert.ToBoolean((polcyDT.Rows[0]["DEFVAL"].ToString()) == "Y" ? true : false);
                                    break;
                                case "T":
                                    obj[1] = Convert.ToString(polcyDT.Rows[0]["DEFVAL"].ToString());
                                    break;
                                default:
                                    break;
                            }
                        }
                    }


                }

                return obj;

            }
            catch (Exception exception)
            {
              //  CreateLog(exception.StackTrace);
                return null;
            }
        }
    }
}
