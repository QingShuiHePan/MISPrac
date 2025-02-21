using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using SSPUCore.Classes;
using SSPUCore.Controls ;

namespace DBManager
{
    public class CachingManager
    {
        private static CachingManager _instance;
        private static Object _mutex = new Object();
        private readonly Assembly _assemblyDbMananger;

        private CachingManager()
        {
            _assemblyDbMananger = Assembly.GetAssembly( typeof( DBObjBase ));
        }

        public static CachingManager Instance
        {
            get
            {
                if ( _instance == null )
                {
                    lock ( _mutex )
                    {
                        if ( _instance == null )
                        {
                            _instance = new CachingManager();
                        }
                    }
                }

                return _instance;
            }
        }
        
        public object GetCache( string key )
        {
            if (HttpContext.Current == null)
            {
                return null;
            }
            return HttpContext.Current.Cache[key];
        }

        public void SetCache( string key, object obj )
        {
            if (HttpContext.Current == null)
            {
                return;
            }

            if ( obj == null )
            {
                RemoveCache( key );
                return;
            }

            HttpContext.Current.Cache.Insert(
                 key
                , obj
                , null
                , DateTime.UtcNow.AddMinutes( int.Parse( ConfigurationManager.AppSettings["CachingTime"] ) )
                , TimeSpan.Zero );
        }

        public void RemoveCache( string key )
        {
            if (HttpContext.Current != null && HttpContext.Current.Cache!= null && HttpContext.Current.Cache.Count >= 1)
            {
                HttpContext.Current.Cache.Remove(key);
            }

        }

        public DataTable GetDataTable( string tableName )
        {
            DataTable dt = GetDataTable( tableName , false ) ;
            if (dt == null)
            {
                dt = GetDataTable(tableName, true);
            }

            return dt;
        }

        public DataTable GetDataTable( string tableName, bool readFromDBDirectly )
        {
            object obj = null;
            if (!readFromDBDirectly)
            {
                obj = GetCache(tableName);
            }

            if (obj == null )
            {
                if (tableName.StartsWith("View_", StringComparison.InvariantCultureIgnoreCase))
                {
                    obj = SSPUSqlHelper.Instance.Get_TopNOfTable(tableName, 0).Tables[0];
                }
                else
                {
                    try
                    {
                        Type type = _assemblyDbMananger.GetType(string.Format("DBManager.TO_{0}", tableName));
                        DBObjBase dbObj = (DBObjBase)Activator.CreateInstance(type);

                        DataTable dt = null;
                        if (dbObj != null)
                        {
                            dt = dbObj.GetDataTableFromDB();
                        }

                        if( !readFromDBDirectly )
                        {
                            SetCache(tableName, dt);
                            obj = GetCache(tableName);
                        }

                        return dt;
                    }
                    catch (Exception)
                    {
                        DataTable dt = null;
                        {
                            dt = SSPUSqlHelper.Instance.Get_TopNOfTable(tableName, 0).Tables[0];
                        }
                        if (!readFromDBDirectly)
                        {
                            SetCache(tableName, dt);
                            obj = GetCache(tableName);
                        }

                    }
                }

                
                
            }

            return obj as DataTable;
        }

        public DataTable GetDataTable( ColumnSearchDefine searchCondition )
        {
            if( searchCondition != null )
            {
                
                if( searchCondition.ListConditions != null )
                {
                    return GetDataTable( searchCondition.DataTableName
                                        , searchCondition.ListConditions
                                        , searchCondition.Columns
                                        , searchCondition.AndLogic
                        );
                }

                return GetDataTable( searchCondition.DataTableName
                                        , searchCondition.Conditions
                                        , searchCondition.Columns
                                        , searchCondition.AndLogic
                        );
            }

            return null;
        }

        public DataTable GetDataTable(string tableName, Dictionary<string, string> conditions, string[] columns)
        {
            return GetDataTable(tableName, conditions, columns, true);
        }

        public DataTable GetDataTable( string  tableName, Dictionary<string ,string > conditions, string[] columns, bool andLogic)
        {
            return GetDataTable( tableName , conditions , columns , andLogic , false );
        }

        public DataTable GetDataTable( string  tableName, Dictionary<string ,string > conditions, string[] columns, bool andLogic, bool ignoreCase )
        {
            DataTable dt = null;

            if (tableName.StartsWith("View_", StringComparison.InvariantCultureIgnoreCase))
            {
                dt = GetDataTable(tableName, true);
            }
            else
            {
                dt = GetDataTable(tableName);
            }

             if ( dt == null  )
             {
                 return dt;
             }

             return DataTableOperatorManager.Instance.GetDataTableByCondition( dt, conditions, columns, andLogic, ignoreCase );
         }

        public DataTable GetDataTable(string tableName, Dictionary<string, string> conditions, string[] columns, bool andLogic, bool ignoreCase, bool columnsIsNotIncluded)
        {
            DataTable dt = null;

            if (tableName.StartsWith("View_", StringComparison.InvariantCultureIgnoreCase))
            {
                dt = GetDataTable(tableName, true);
            }
            else
            {
                dt = GetDataTable(tableName);
            }

            if (dt == null)
            {
                return dt;
            }

            return DataTableOperatorManager.Instance.GetDataTableByCondition(dt, conditions, columns, andLogic, ignoreCase, columnsIsNotIncluded);
        }

        public DataTable GetDataTable( string tableName , Dictionary<string , List<string>> conditions , string[] columns , bool andLogic  )
        {
            return GetDataTable( tableName , conditions , columns , andLogic , false );
        }

        public DataTable GetDataTable(string tableName, Dictionary<string, List<string>> conditions, string[] columns, bool andLogic, bool ignoreCase )
        {
            DataTable dt = null;
            if( tableName.StartsWith("View_", StringComparison.InvariantCultureIgnoreCase) )
            {
                dt = GetDataTable(tableName, true);
            }
            else
            {
                dt = GetDataTable(tableName);
            }
            

            if (dt == null)
            {
                return dt;
            }

            return DataTableOperatorManager.Instance.GetDataTableByCondition( dt , conditions , columns , andLogic , ignoreCase );
        }

        public DataTable GetDataTable( string tableName, Dictionary<string, List<string>> conditions, string[] columns, bool andLogic, bool ignoreCase,bool? columnsNotInclude )
        {
            DataTable dt = null;
            if ( tableName.StartsWith( "View_", StringComparison.InvariantCultureIgnoreCase ) )
            {
                dt = GetDataTable( tableName, true );
            }
            else
            {
                dt = GetDataTable( tableName );
            }


            if ( dt == null )
            {
                return dt;
            }

            return DataTableOperatorManager.Instance.GetDataTableByCondition( dt, conditions, columns, andLogic, ignoreCase, columnsNotInclude.HasValue && columnsNotInclude.Value );
        }

        public DataTable GetDataTable( string  tableName, Dictionary<string ,string > conditions )
        {
            return GetDataTable(tableName, conditions, new string[]{});
        }

        public DBObjBase SetValueToDBObjByIDValue( DBObjBase obj )
        {
            if( obj != null && obj.ID >=1 )
            {
                DataTable dt = CachingManager.Instance.GetDataTable(obj.MyTableNameInDB,
                                                                    Utility.Instance.GetSearchingCondition("ID",obj.ID.ToString()));
                if( dt != null && dt.Rows.Count == 1 )
                {
                    obj.Parse(dt.Rows[0]);
                    return obj;
                }

                return null;
            }

            return null;
        }

        public T SetValueToDBObj<T>( int idValue )
        {


            T objT = Activator.CreateInstance<T>();
            if (!(objT is DBObjBase))
            {
                return default(T);
            }

            DBObjBase obj = objT as DBObjBase;

            Dictionary<string ,string> searchCondition = new Dictionary<string , string>();
            searchCondition.Add("ID", idValue.ToString() );
            DataTable dt = CachingManager.Instance.GetDataTable( obj.MyTableNameInDB, searchCondition ) ;
            if( dt != null && dt.Rows.Count >=1 )
            {
                obj.Parse( dt.Rows[0] );

                return objT;
            }
            else
            {
                obj = null ;
            }

            return default(T);
        }

        public void AddNewRowIntoCaching( string tableName, DBObjBase obj, DataRow row )
        {
            DataTable cachingTable = CachingManager.Instance.GetDataTable( tableName, false );
            if ( cachingTable != null )
            {
                lock ( cachingTable )
                {
                    bool contains = false;
                    foreach ( DataRow cdrow in cachingTable.Rows )
                    {
                        if ( cdrow.Field<int>( "ID" ) == obj.ID )
                        {
                            contains = true;
                            break;
                        }
                    }

                    if ( !contains )
                    {
                        cachingTable.Rows.Add( row.ItemArray );
                    }
                }

            }
        }

        public void ModifyRowInCaching( string tableName, DBObjBase obj, DataRow row )
        {
            DataTable cachingTable = GetDataTable( tableName,false );
            if ( cachingTable != null )
            {
                lock ( cachingTable )
                {
                    foreach ( DataRow cdrow in cachingTable.Rows )
                    {
                        if ( cdrow.Field<int>( "ID" ) == obj.ID )
                        {
                            int max = cachingTable.Columns.Count;
                            for ( int i = 0 ; i < max ; i++ )
                            {
                                cdrow[i] = row[i];
                            }
                            break;
                        }
                    }
                }

            }
        }

        public void DeleteRowIntoCaching( string tableName, DBObjBase obj )
        {
            DataTable cachingTable = GetDataTable( tableName, false );
            if ( cachingTable != null )
            {
                lock ( cachingTable )
                {
                    foreach ( DataRow cdrow in cachingTable.Rows )
                    {
                        if ( cdrow.Field<int>( "ID" ) == obj.ID )
                        {
                            cachingTable.Rows.Remove( cdrow );
                            break;
                        }
                    }
                }

            }
        }

        public bool HasRepeatData(string tableName, Dictionary<string, string> conditions, DBObjBase currentObj)
        {
            DataTable dt = this.GetDataTable(tableName, conditions, null, true);

            if (dt != null && dt.Rows.Count >= 1)
            {
                DBObjBase obj = Utility.Instance.GetTO_ObjByTableName(tableName);
                foreach (DataRow row in dt.Rows)
                {
                    obj.Parse(row);
                    if (obj.ID != currentObj.ID)
                    {
                        return true;
                    }
                }
                return false;
            }

            return false;
        }

        public List<T> GetTO_ObjsEnabled<T>()
        {
            return GetTO_ObjsByCondition<T>(Utility.Instance.GetSearchingCondition(DBObjBase._Enabled, "True"));
        }

        public List<T> GetAllObjects<T>()
        {
            return GetTO_ObjsByCondition<T>();
        }

        public List<T> GetTO_ObjsByCondition<T>( Dictionary<string, string> condition )
        {

            return GetTO_ObjsByCondition<T>( condition, false );

        }

        public T GetTO_ObjByCondition<T>(Dictionary<string, string> condition)
        {

            List<T> result = GetTO_ObjsByCondition<T>(condition, false);

            if (result != null && result.Count >= 1)
            {
                return result[0];
            }

            return default(T);
        }

        public List<T> GetTO_ObjsByCondition<T>(string columnName, string columValue)
        {
            
            List<T> result = GetTO_ObjsByCondition<T>(Utility.Instance.GetSearchingCondition(columnName,columValue));

            return result;
        }

        public T GetTO_ObjByCondition<T>(string columnName, string columValue)
        {

            List<T> result = GetTO_ObjsByCondition<T>(columnName,columValue);

            if (result != null && result.Count >= 1)
            {
                return result[0];
            }

            return default(T);
        }

        public T GetTO_ObjByCondition<T>(int id)
        {
            
            return GetTO_ObjByCondition<T>(Utility.Instance.GetSearchingCondition("ID", id.ToString()));

        }

        public List<T> GetTO_ObjsByCondition<T>( Dictionary<string, string> conditions = null, bool AndLogic=true )
        {

            List<T> result = null;

            T o = Activator.CreateInstance<T>();

            if ( o is DBObjBase )
            {
                DBObjBase obj = (DBObjBase)( (Object)o );

                //DataTable dt = SSPUSqlHelper.Instance.Get_TopNOfTable( obj.MyTableNameInDB, 0 ).Tables[0];
                DataTable dt = null;
                if (conditions != null)
                {
                    dt = this.GetDataTable(obj.MyTableNameInDB, conditions, null, AndLogic);
                    dt = DataTableOperatorManager.Instance.GetDataTableByCondition(dt, conditions, null, AndLogic);
                }
                else
                {
                    dt = this.GetDataTable(obj.MyTableNameInDB);
                }

                

                if (dt != null)
                {
                    result = new List<T>();

                    foreach (DataRow dataRow in dt.Rows)
                    {
                        T tmpT = Activator.CreateInstance<T>();

                        (tmpT as DBObjBase).Parse(dataRow);

                        result.Add(tmpT);
                    }
                }

                
            }


            return result;
        }

        public List<T> GetTO_ObjsByCondition<T>( Dictionary<string, List<string>> conditions, bool AndLogic )
        {
            List<T> result = null;

            T o = Activator.CreateInstance<T>();

            if ( o is DBObjBase )
            {
                DBObjBase obj = (DBObjBase)( (Object)o );

                //DataTable dt = SSPUSqlHelper.Instance.Get_TopNOfTable( obj.MyTableNameInDB, 0 ).Tables[0];
                DataTable dt = this.GetDataTable( obj.MyTableNameInDB, conditions, null, AndLogic );

                dt = DataTableOperatorManager.Instance.GetDataTableByCondition( dt, conditions, null, AndLogic );

                result = new List<T>();

                foreach ( DataRow dataRow in dt.Rows )
                {
                    T tmpT = Activator.CreateInstance<T>();

                    ( tmpT as DBObjBase ).Parse( dataRow );

                    result.Add( tmpT );
                }
            }


            return result;

        }

        private string CourseSelectedInfoKey = "Caching_CourseSelectedInfo__Key__";
        private string CourseSelectedInfoKey316 = "Caching_CourseSelectedInfo__Key__316__";

        
        
    }
}
