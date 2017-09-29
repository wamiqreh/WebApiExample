#region Modification History
// Created By:  Mirza Fahad Ali Baig
// Created On:  27th May, 2013
// Description: 
// ****************************** Modifications *********************************

// ******************************************************************************
#endregion
using Microsoft.VisualBasic;
using System.Data;
using System.Data.OleDb;
using System;
using System.Configuration;
using System.Data.OracleClient;
using System.Globalization;
using System.Collections.Generic;

namespace DAL.Provider
{
    public class ConnClass:IDisposable
    {

        public OracleConnection DBConn = new OracleConnection();
        public string ConYes = "X";
        public string CmdYes = "X";
        public DataTable DTable = new DataTable();

        public void Open_Con()
        {
            try
            {
                if (DBConn != null)
                {
                    DBConn.Close();
                    ConYes = "X";
                }

                DBConn.ConnectionString = ConfigurationManager.ConnectionStrings["DBR_CON"].ConnectionString;
                DBConn.Open();
                ConYes = "Y";



            }


            catch (Exception ex)
            {
                ConYes = ex.ToString();
            }

        }
        public void Close_Con()
        {
            try
            {
                if (DBConn != null)
                {
                    DBConn.Close();
                    ConYes = "X";
                }

            }

            catch (Exception ex)
            {
               //Fourgen.POS.Helper.FGLogger.PrintError(ex.StackTrace);
            }
        }

        public int Execute_DML(string Qry)
        {
            //Try
            this.Open_Con();
            OracleCommand ObjCmd = new OracleCommand(Qry, DBConn);

            int rowseffect =   ObjCmd.ExecuteNonQuery();
            CmdYes = "Y";
            this.Close_Con();
            return rowseffect;
            // Catch ex As Exception
            //CmdYes = ex.Message()

            // End Try

        }
        public int Execute_Func(string Qry ,Dictionary<object,object> param)
        {
            int rows = 0;
            try
            {

                this.Open_Con();
                OracleCommand ObjCmd = new OracleCommand(Qry, DBConn);
                ObjCmd.CommandType = CommandType.StoredProcedure;
                foreach (var item in param)
                {
                    ObjCmd.Parameters.Add(item.Key.ToString(), item.Value);
                }
               
                CmdYes = "Y";
                rows =  ObjCmd.ExecuteNonQuery();
               

            }
            catch (Exception ex)
            {
                CmdYes = ex.Message;
                rows = 0;
            }
            finally
            {
                this.Close_Con();
            }
            return rows;
        }



        public void MyReader(string Qry)
        {
            try
            {
                this.Open_Con();
                OracleCommand myCmd = new OracleCommand(Qry, DBConn);
                OracleDataAdapter DA = new OracleDataAdapter(myCmd);
                DA.Fill(DTable);
                CmdYes = "Y";

            }
            catch (Exception Ex)
            {
                CmdYes = Ex.Message;
            }
            finally
            {
                this.Close_Con();
            }

        }
        public DataTable MyDataTable(string Qry)
        {
            this.Open_Con();
            DataTable DT = new DataTable();

            try
            {
                OracleCommand myCmd = new OracleCommand(Qry, DBConn);
                OracleDataAdapter DA = new OracleDataAdapter(myCmd);
                DA.Fill(DT);
                CmdYes = "Y";


                return DT;



            }
            catch (Exception Ex)
            {
                //Fourgen.POS.Helper.FGLogger.PrintError(Ex.StackTrace);
                CmdYes = "N";
            }


            finally
            {
                this.Close_Con();
                DT = null;
            }
            return (DT);
        }

        #region Dispose
        public void Dispose()
        {

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}