using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using RDSDatabase;

namespace RDSService
{
    public class RDSService
    {
        public RDSService()
        {

        }
        RDSDBConnection rdsConnection = new RDSDBConnection();
        
        //All Business Method here  
        public DataSet SelectList(String SP_NAME)
        {
            try
            {


                return rdsConnection.SP_DataSet_return(SP_NAME);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet SelectList(String SP_NAME, String outputFieldName, SortedDictionary<string, string> sd )
        {
            try
            {
                return rdsConnection.SP_Insert_Table(SP_NAME, outputFieldName, GetSdParameter(sd));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet SelectList(String SP_NAME, SortedDictionary<string, string> sd)
        {
            try
            {
                return rdsConnection.SP_DataTable_return(SP_NAME, GetSdParameter(sd));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method Sorted-Dictionary key values to an array of SqlParameters
        /// </summary>
        public static MySqlParameter[] GetSdParameter(SortedDictionary<string, string> sortedDictionary)
        {
            MySqlParameter[] paramArray = new MySqlParameter[] { };

            foreach (string key in sortedDictionary.Keys)
            {
                AddParameter(ref paramArray, new MySqlParameter(key, sortedDictionary[key]));
            }

            return paramArray;
        }

        ////    AddParameter(ref paramArray, parameter);
        ////}

        public static void AddParameter(ref MySqlParameter[] paramArray, params MySqlParameter[] newParameters)
        {
            MySqlParameter[] newArray = Array.CreateInstance(typeof(MySqlParameter), paramArray.Length + newParameters.Length) as MySqlParameter[];
            paramArray.CopyTo(newArray, 0);
            newParameters.CopyTo(newArray, paramArray.Length);

            paramArray = newArray;
        }


    }

}
