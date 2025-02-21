using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using SSPUCore.Controls;

namespace DBManager
{
    public class TOClassManager
    {
        private static TOClassManager _instance;
        private static Object _mutex = new Object();


        private TOClassManager()
        {

        }

        public static TOClassManager Instance
        {
            get
            {
                if ( _instance == null )
                {
                    lock ( _mutex )
                    {
                        if ( _instance == null )
                        {
                            _instance = new TOClassManager();
                        }
                    }
                }

                return _instance;
            }
        }


        public T ExcuteToDBWithCachingUpdate<T>( T toObjest, AjaxDataOperatorType operatorType )
        {
            if ( !( toObjest is DBObjBase )  )
            {
                return default( T );
            }

            try
            {
                DataTable dt;
                DataRow row;
                DBObjBase toObj = toObjest as DBObjBase;
                int result = -1;
                switch ( operatorType )
                {
                    case AjaxDataOperatorType.ADD:
                        dt = toObj.AddToDB();
                        row = dt.Rows[0];
                        toObj.Parse( row );

                        //CachingManager.Instance.AddNewRowIntoCaching( toObj.MyTableNameInDB, toObj, row );
                        break;
                    case AjaxDataOperatorType.MODIFY:
                        dt = toObj.ModifyToDB();
                        row = dt.Rows[0];
                        toObj.Parse( row );

                        //CachingManager.Instance.ModifyRowInCaching( toObj.MyTableNameInDB, toObj, row );
                        break;

                    case AjaxDataOperatorType.DELETE:
                        result = toObj.DeleteToDB();
                        //dt = SSPUSqlHelper.Instance.Get_TopNOfTableByCondition( tableName, 0, "ID", toObj.ID.ToString() ).Tables[0];

                        //CachingManager.Instance.DeleteRowIntoCaching( toObj.MyTableNameInDB, toObj );
                        break;

                    default:
                        return default( T );
                }

                return (T)( (object)toObj );

            }
            catch ( System.Exception ex )
            {
                throw ( new Exception( ex.Message ) );
            }
        }

        public List<T> GetTO_ObjsByCondition<T>( Dictionary<string, string> condition )
        {

            return GetTO_ObjsByCondition<T>( condition, false );

        }

        public T GetOneTO_Obj<T>()
        {
            return Activator.CreateInstance<T>();
        }

        public T GetTO_ObjsByCondition<T>( string columnName, string Columnvalue )
        {

            T o = Activator.CreateInstance<T>();

            if ( o is DBObjBase )
            {
                DBObjBase obj = (DBObjBase)( (Object)o );
                DataTable dt = null;

                PropertyInfo p = obj.GetType().GetProperty( columnName );
                if ( p.PropertyType == typeof( string ) )
                {
                    dt = SSPUSqlHelper.Instance.Get_TopNOfTableByCondition( obj.MyTableNameInDB, 1, columnName, string.Format("'{0}'",Columnvalue) ).Tables[0];
                }
                else
                {
                    dt = SSPUSqlHelper.Instance.Get_TopNOfTableByCondition( obj.MyTableNameInDB, 1, columnName, Columnvalue ).Tables[0];
                }
                

                if( dt != null && dt.Rows.Count >=1 )
                {
                    T tmpT = Activator.CreateInstance<T>();

                    ( tmpT as DBObjBase ).Parse( dt.Rows[0] );

                    return tmpT;

                }
            }
            return default(T);
        }

        public List<T> GetTO_ObjsByCondition<T>( Dictionary<string, string> conditions, bool AndLogic )
        {

            List<T> result = null;

            T o = Activator.CreateInstance<T>();

            if ( o is DBObjBase )
            {
                DBObjBase obj = (DBObjBase)( (Object)o );

                DataTable dt = SSPUSqlHelper.Instance.Get_TopNOfTable( obj.MyTableNameInDB, 0 ).Tables[0];

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

        public List<T> GetTO_ObjsByCondition<T>( Dictionary<string, List<string>> conditions, bool AndLogic )
        {
            List<T> result = null;

            T o = Activator.CreateInstance<T>();

            if ( o is DBObjBase )
            {
                DBObjBase obj = (DBObjBase)( (Object)o );

                DataTable dt = SSPUSqlHelper.Instance.Get_TopNOfTable( obj.MyTableNameInDB, 0 ).Tables[0];

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
    }
}
