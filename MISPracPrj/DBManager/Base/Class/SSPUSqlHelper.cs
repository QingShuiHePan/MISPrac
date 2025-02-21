//#define  _DEBUG
using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Reflection;

namespace DBManager
{
    public partial class SSPUSqlHelper
    {
        #region Instance
        
        private string connectionStr;

        private static SSPUSqlHelper _instance;
        private static object  _mutex = new object();

        private SSPUSqlHelper()
        {


#if _DEBUG
            connectionStr = ConfigurationManager.ConnectionStrings[ "SaleAndInventoryEdu_Server" ].ToString();
            return;
#endif
            connectionStr = ConfigurationManager.ConnectionStrings["SaleAndInventory_Server"].ToString();
          
        }

        public static SSPUSqlHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if( _instance == null )
                        {
                            _instance = new SSPUSqlHelper();
                        }
                    }
                }
                
                return _instance;
            }
        }

        #endregion

        #region Private Methods

        private SqlCommand GetSqlCommand(SqlConnection connection, string procedureName, List<SqlParameter> parms)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlCommand myCommand = new SqlCommand(procedureName, connection);
            myCommand.CommandType = CommandType.StoredProcedure;

            if (parms != null && parms.Count >= 1)
            {
                foreach (SqlParameter parm in parms)
                {
                    myCommand.Parameters.Add(parm);
                }
            }

            return myCommand;
        }

        private int ExcuteProcedure_NoReturnData(string procedureName, List<SqlParameter> parms)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStr))
                {
                     SqlCommand myCommand = GetSqlCommand(connection, procedureName, parms);
                     return myCommand.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw (exc);
            }
            catch
            {
                Exception exc = new Exception("数据库执行错误！");
                throw (exc);
            }

            
        }

        private int ExcuteProcedure_ExecuteScalar(string procedureName, List<SqlParameter> parms)
        {
            try
            {
                using ( SqlConnection connection = new SqlConnection( connectionStr ) )
                {
                    SqlCommand myCommand = GetSqlCommand(connection, procedureName, parms);
                    object obj = myCommand.ExecuteScalar();

                    int result = int.MinValue;
                    if (obj != null)
                    {
                        if (int.TryParse(obj.ToString(), out result))
                        {
                            return result;
                        }
                        else
                        {
                            return int.MinValue;
                        }
                    }

                    return result;
                }
            }
            catch (Exception exc)
            {
                throw (exc);
            }
            catch
            {
                Exception exc = new Exception("数据库执行错误！");
                throw (exc);
            }

        }

        private DataSet ExcuteProcedure_GetDataSet(string procedureName, List<SqlParameter> parms)
        {

            try
            {
                using ( SqlConnection connection = new SqlConnection( connectionStr ) )
                {

                    SqlCommand myCommand = GetSqlCommand( connection, procedureName, parms );

                    SqlDataAdapter DataAdapter = new SqlDataAdapter();
                    DataAdapter.SelectCommand = myCommand;

                    DataSet MyDataSet = new DataSet();
                    DataAdapter.Fill( MyDataSet, "table" );

                    return MyDataSet;
                }

            }
            catch (Exception exc)
            {
                throw (exc);
            }
            catch
            {
                Exception exc = new Exception("数据库执行错误！");
                throw (exc);
            }
            
        }

        public DataTable ExcuteProcedure_GetDataTable(string ProcedureName, List<SqlParameter> parms)
        {
            DataSet ds = ExcuteProcedure_GetDataSet(ProcedureName, parms);

            if (ds != null && ds.Tables.Count >= 1)
            {
                return ds.Tables[0];
            }

            return null;
        }

        #endregion

        public DataSet ExcuteSqlCommand( string commandText )
        {
            SqlConnection connection = new SqlConnection( connectionStr );
            connection.Open();
            DataSet ds = new DataSet();
            using (SqlDataAdapter adapter = new SqlDataAdapter(commandText, connection))
            {
                adapter.Fill(ds);
            }

            return ds;

        }

       
        public DataSet ExcuteGetViewProcedure( string procedureName, string selectColumns , string conditions )
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            SqlParameter selectColumnsParm =  new SqlParameter( "@selectColumns" , SqlDbType.VarChar ); ;
            selectColumnsParm.Value = selectColumns;
            parms.Add( selectColumnsParm );
            SqlParameter conditionsParm =  new SqlParameter( "@conditions" , SqlDbType.VarChar ); ;
            conditionsParm.Value = conditions;
            parms.Add( conditionsParm );
            return ExcuteProcedure_GetDataSet( procedureName , parms );
        }
        

    }
}
