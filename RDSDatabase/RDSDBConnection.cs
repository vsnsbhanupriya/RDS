using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.Configuration;

namespace RDSDatabase
{
    public class RDSDBConnection
    {
        static MySqlConnection mySQLConnection;
         
        public RDSDBConnection()
        {

        }

        public static MySqlConnection DatabaseConnection()
        {
            string connection = WebConfigurationManager.AppSettings["connectionstring"];
            mySQLConnection = new MySqlConnection(connection);
            mySQLConnection.Open();
            return mySQLConnection;
        }

        #region ExecuteNonQuery  
        //for insert / Update and Delete
        //For Insert/Update/Delete  
        public int ExecuteNonQuery_IUD(String Querys)
        {
            int result = 0;
           
            //open connection  
            if (OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor  
                MySqlCommand cmd = new MySqlCommand(Querys, mySQLConnection);
                //Execute command  
                result = cmd.ExecuteNonQuery();
                //close connection  
                CloseConnection();
            }
            return result;
        }
        #endregion

    
        #region Dataset  
        //for select result and  
        //return as Dataset
        //for select result and return as Dataset  
        public DataSet DataSet_return(String Querys)
        {
            DataSet ds = new DataSet();
            //open connection  
            if (OpenConnection() == true)
            {
                //for Select Query   
                MySqlCommand cmdSel = new MySqlCommand(Querys, mySQLConnection);
                MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
                da.Fill(ds);
                //close connection  
                CloseConnection();
            }
            return ds;
        }
        #endregion

        #region DataTable  
        //for select result and  
        //return as DataTable
        //for select result and return as DataTable  
        public DataTable DataTable_return(String Querys)
        {
            DataTable dt = new DataTable();
            //open connection  
            if (OpenConnection() == true)
            {
                //for Select Query   
                MySqlCommand cmdSel = new MySqlCommand(Querys, mySQLConnection);
                MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
                da.Fill(dt);
                //close connection  
                CloseConnection();
            }
            return dt;
        }
        #endregion

        #region Dataset  
        //for Stored Procedure and  
        //return as DataTable
        //for select result and return as DataTable  

        private static void AttachParameters(MySqlCommand command, MySqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (MySqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned  
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) && (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        private static void AssignParameterValues(MySqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                // Do nothing if we get no data  
                return;
            }
            // We must have the same number of values as we pave parameters to put them in  
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }
            // Iterate through the SqlParameters, assigning the values from the corresponding position in the   
            // value array  
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property  
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if (paramInstance.Value == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }
        #endregion





        public DataSet SP_Insert_return(String ProcName, params MySqlParameter[] commandParameters)
        {
            DataSet ds = new DataSet();
            //open connection  
            if (OpenConnection() == true)
            {
                //for Select Query   
                MySqlCommand cmdSel = new MySqlCommand(ProcName, mySQLConnection);
                cmdSel.CommandType = CommandType.StoredProcedure;
                // Assign the provided values to these parameters based on parameter order  
                AssignParameterValues(commandParameters, commandParameters);
                AttachParameters(cmdSel, commandParameters);
                MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
                da.Fill(ds);
                //close connection  
                CloseConnection();
            }
            return ds;
        }

        public DataSet SP_DataTable_return(String ProcName, params MySqlParameter[] commandParameters)
        {
            DataSet ds = new DataSet();
            //open connection  
            if (OpenConnection() == true)
            {
                //for Select Query   
                MySqlCommand cmdSel = new MySqlCommand(ProcName, mySQLConnection);
                cmdSel.CommandType = CommandType.StoredProcedure;
                // Assign the provided values to these parameters based on parameter order  
                AssignParameterValues(commandParameters, commandParameters);
                AttachParameters(cmdSel, commandParameters);
                MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
                da.Fill(ds);
                //close connection  
                CloseConnection();
            }
            return ds;
        }

        public DataSet SP_Insert_Table(String ProcName, string outparam, params MySqlParameter[] commandParameters )
        {
            DataSet ds = new DataSet();
            //open connection  
            if (OpenConnection() == true)
            {
                //for Select Query   
                MySqlCommand cmdSel = new MySqlCommand(ProcName, mySQLConnection);
                cmdSel.CommandType = CommandType.StoredProcedure;
                // Assign the provided values to these parameters based on parameter order  
                AssignParameterValues(commandParameters, commandParameters);
                AttachParameters(cmdSel, commandParameters);
                cmdSel.Parameters.AddWithValue(outparam, MySqlDbType.Int32).Direction = ParameterDirection.Output;
               
                MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
                da.Fill(ds);
                //close connection  
                CloseConnection();
            }
            return ds;
        }

        public DataSet SP_DataSet_return(String ProcName)
        {
            DataSet ds = new DataSet();
            //open connection  
            if (OpenConnection() == true)
            {
                //for Select Query   
                MySqlCommand cmdSel = new MySqlCommand(ProcName, mySQLConnection);
                cmdSel.CommandType = CommandType.StoredProcedure;
                // Assign the provided values to these parameters based on parameter order                
                MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
                da.Fill(ds);
                //close connection  
                CloseConnection();
            }
            return ds;
        }

        private bool OpenConnection()
        {
            try
            {
                string connection = WebConfigurationManager.AppSettings["connectionstring"];
                mySQLConnection = new MySqlConnection(connection);
                mySQLConnection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                mySQLConnection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
               // MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
