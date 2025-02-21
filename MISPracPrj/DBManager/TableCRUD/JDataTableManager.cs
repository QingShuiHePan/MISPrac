using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Definition;
using SSPUCore;
using SSPUCore.Configuration;
using SSPUCore.Controls;

namespace DBManager
{
    public class JDataTableManager
    {
        #region Instance

        private readonly Assembly _assemblyDbMananger;
        private static readonly object _mutex = new object();
        private static JDataTableManager _instance = null;

        private JDataTableManager()
        {
            _assemblyDbMananger = Assembly.GetAssembly( typeof( DBObjBase ) );
        }

        public static JDataTableManager Instance
        {
            get
            {
                if ( _instance == null )
                {
                    lock ( _mutex )
                    {
                        if ( _instance == null )
                        {
                            _instance = new JDataTableManager();
                        }
                    }
                }

                return _instance;
            }
        }

        #endregion

        #region For JDataTable Operator

        public string ExcuteCommandReturnJDatatableResult( string postData )
        {
            Dictionary<string, string> postDic = ControlUtility.ParseUserInputToDataRow( postData );

            if ( !postDic.Keys.Contains( JDataTable.EditedTableNameForDictionParse ) )
            {
                throw ( new Exception( "Can not ensure the table name!" ) );
            }

            string tableName = postDic[JDataTable.EditedTableNameForDictionParse];

            var obj = GetToObjByTableName(tableName);
            obj.Parse( postDic );

            AjaxDataOperatorType operatorType = ControlUtility.GetOperateType( postData );

            return ExcuteCommandReturnJDatatableResult( operatorType, obj, postDic );
        }

        public DBObjBase GetToObjByTableName(string tableName)
        {
            if (tableName.StartsWith("_"))
            {
                tableName = SSPUConverter.Instance.DecodingString(tableName);
            }

            Type type = _assemblyDbMananger.GetType(string.Format("DBManager.TO_{0}", tableName));

            DBObjBase obj = (DBObjBase) Activator.CreateInstance(type);
            return obj;
        }

        public string ExcuteCommandReturnJDatatableResult( Dictionary<string, string> postDic, AjaxDataOperatorType operatorType )
        {
            if ( !postDic.Keys.Contains( JDataTable.EditedTableNameForDictionParse ) )
            {
                throw ( new Exception( "Can not ensure the table name!" ) );
            }

            string tableName = postDic[JDataTable.EditedTableNameForDictionParse];

            Type type = _assemblyDbMananger.GetType( string.Format( "DBManager.TO_{0}", tableName ) );

            DBObjBase obj = (DBObjBase)Activator.CreateInstance( type );
            obj.Parse( postDic );

            return ExcuteCommandReturnJDatatableResult( operatorType, obj );
        }

        public T OnlyDoDBObjBaseDataOperate<T>( T objT, AjaxDataOperatorType operatorType )
        {
            if ( !( objT is DBObjBase ) )
            {
                return default( T );
            }

            DBObjBase obj = objT as DBObjBase;

            try
            {
                int result = -1;

                switch ( operatorType )
                {
                    case AjaxDataOperatorType.ADD:
                        obj.AddToDB();
                        break;
                    case AjaxDataOperatorType.MODIFY:
                        obj.ModifyToDB();
                        break;

                    case AjaxDataOperatorType.DELETE:
                        result = obj.DeleteToDB();
                        break;
                    default:
                        break;
                }
            }
            catch ( System.Exception ex )
            {
                //if ( SSPUAppSettings.Environment.Equals( "Development" ) || throwErrorOut )
                {
                    throw ex;
                }
                //else
                //{
                //    return (T)( (object)obj );
                //}
                //return JDataTable.SetFailedPostbackDataToClient( "数据库操作出现错误，请联系管理员！", obj.Serialize() );

                //throw ex ;
            }

            return (T)( (object)obj );
        }

        public string ExcuteCommandReturnJDatatableResult( AjaxDataOperatorType operatorType, DBObjBase obj)
        {
            return ExcuteCommandReturnJDatatableResult(operatorType, obj, null);
        }

        public string ExcuteCommandReturnJDatatableResult( AjaxDataOperatorType operatorType, DBObjBase obj, Dictionary<string,string> postDic  )
        {
            try
            {
                switch (operatorType)
                {
                    case AjaxDataOperatorType.ADD:
                    case AjaxDataOperatorType.MODIFY:
                    case AjaxDataOperatorType.DELETE:
                        obj = OnlyDoDBObjBaseDataOperate( obj, operatorType );
                        string s = JDataTable.TO_ObjectEditDone(obj);
                        return s;
                    case AjaxDataOperatorType.ForItemDialogDetail:
                        {
                            if( !Utility.Instance.IsLogin )
                            {
                                return JDataTable.SetFailedPostbackDataToClient("请无权访问该数据", null);
                            }

                            OpenDialogDetailItem item = OpenDialogDetailItem.Parse(postDic);

                            if (item != null)
                            {
                                DataTable dt = obj.GetDetailInfoByKeyAndID(item.SearchColmnName, item.SearchValue);
                                if (dt != null)
                                {
                                    dt.TableName = obj.MyTableNameInDB;
                                }

                                if (item.ShowColumns == null || item.ShowColumns.Length == 0)
                                {
                                    return JDataTable.SetSuccessPostbackDataToDetailTable(dt, obj.ColumnsDefine.ToArray());
                                }
                                else
                                {
                                    DataTable newDt = dt.DefaultView.ToTable(obj.MyTableNameInDB, false, item.ShowColumns);

                                    return JDataTable.SetSuccessPostbackDataToDetailTable(newDt, obj.ColumnsDefine.ToArray());
                                }

                            }
                            else
                            {
                                throw new Exception(EnumDefs.ErrorsDef.AjaxPostBackError.ToString());
                            }
                        }
                        
                        break;
                    case AjaxDataOperatorType.DoAction:
                        {
                            DoActionInfo acInf = DoActionInfo.Parse(postDic);

                            if( acInf == null )
                            {
                                return JDataTable.TO_ObjectEditDone( null );
                            }
                            else
                            {
                                obj.ID = acInf.SenderID;
                                obj = CachingManager.Instance.SetValueToDBObjByIDValue(obj);
                                obj.DoAction( acInf );
                                return JDataTable.ConverAjaxCallBackForDoAction( obj );
                            }
                            
                        }
                        
                }
                
            }
            catch ( System.Exception ex )
            {
                return JDataTable.SetFailedPostbackDataToClient( ex.Message.Replace("\"","").Replace("'","").Replace("\\",""), null );
            }

            return string.Empty;
        }

        //public string SerializeDataForClientJDatatable( DBObjBase obj, AjaxDataOperatorType operatorType )
        //{
        //    if ( obj == null )
        //    {
        //        return JDataTable.SetFailedPostbackDataToClient( "未将数据应用到实例！" , null ) ;
        //    }

        //    if ( operatorType == AjaxDataOperatorType.DELETE )
        //    {
        //        return JDataTable.SetSuccessPostbackDataToClient( obj.SerializeForDeleteOption() );
        //    }
        //    else
        //    {
        //        return JDataTable.SetSuccessPostbackDataToClient( obj.Serialize() );
        //    }
        //}

        public string SerializeDataForTreeView( DBObjBase obj  )
        {
            if ( obj == null )
            {
                return JDataTable.SetFailedPostbackDataToClient( "未将数据应用到实例！", null );
            }

            
            return JDataTable.TO_ObjectEditDone(obj) ;
        }

        public void InitializeJDataTable( JDataTable jdt, string tableName, Dictionary<string ,string > conditions, bool onlySetColumnToJDataTable )
        {
            if ( jdt == null || string.IsNullOrEmpty( tableName ) )
            {
                return;
            }

            jdt.DataTableName = tableName;
            DBObjBase obj = CreateInstanceByTableName( tableName );
            jdt.EncodeIDValue = !(obj is INotEncodeID);
            if ( obj != null )
            {
                jdt.ColumnsDefine = obj.ColumnsDefine;
                jdt.ColumnNameMapping = obj.NameMapping;

                if ( onlySetColumnToJDataTable )
                {
                    DataTable resultdt = new DataTable();
                    DataTable dt = CachingManager.Instance.GetDataTable( tableName );

                    foreach (DataColumn column in dt.Columns)
                    {
                        resultdt.Columns.Add( DataTableOperatorManager.Instance.GetColne( column ) );
                    }
                    jdt.Data = resultdt;
                }
                else
                {
                    if ( conditions == null )
                    {
                        jdt.Data = CachingManager.Instance.GetDataTable( tableName );
                    }
                    else
                    {
                        jdt.Data = CachingManager.Instance.GetDataTable( tableName, conditions );
                    }    
                }

                
            }
        }

        public void InitializeJDataTable( JDataTable jdt, string tableName, Dictionary<string ,string > conditions )
        {
            InitializeJDataTable( jdt, tableName, conditions, false );
        }

        public void InitializeJDataTable( JDataTable jdt, string tableName, DataTable dt )
        {
            if ( jdt == null || string.IsNullOrEmpty( tableName ) )
            {
                return;
            }

            
            DBObjBase obj = CreateInstanceByTableName( tableName );

            if ( obj != null )
            {
                InitializeJDataTable( jdt, obj, dt );
                jdt.DataTableName = tableName;
            }
        }

        public void InitializeJDataTable( JDataTable jdt, DBObjBase obj, DataTable dt )
        {
            if( jdt != null && obj != null )
            {
                jdt.DataTableName = obj.MyTableNameInDB;
                jdt.ColumnsDefine = obj.ColumnsDefine;
                jdt.ColumnNameMapping = obj.NameMapping;
                jdt.EncodeIDValue = !( obj is INotEncodeID );
                jdt.Data = dt;
            }
        }

        public void InitializeJDataTableInWorkflow( JDataTable jdt, string tableName, DataTable dt )
        {
            if ( jdt == null || string.IsNullOrEmpty( tableName ) )
            {
                return;
            }

            jdt.DataTableName = tableName;
            DBObjBase obj = CreateInstanceByTableName( tableName );

            if ( obj != null )
            {
                jdt.ColumnsDefine = obj.ColumnsDefine;
                jdt.ColumnNameMapping = obj.NameMapping;
                jdt.EncodeIDValue = !(obj is INotEncodeID);
                jdt.Data = dt;
            }

            jdt.UseingInWorkflow = true;
        }

        public void InitializeJDataTable( JDataTable jdt, string tableName, bool useCaching )
        {
            if ( useCaching )
            {
                DataTable dt = CachingManager.Instance.GetDataTable( tableName ) ;
                InitializeJDataTable( jdt, tableName, dt );
            }
            else
            {
                InitializeJDataTable( jdt, tableName, new Dictionary<string, string>() );
            }

            
        }

        public void InitializeJDataTable( JDataTable jdt, string tableName )
        {
            InitializeJDataTable( jdt, tableName, true  );
        }

        
        private DBObjBase CreateInstanceByTableName( string tableName )
        {
            Type type = _assemblyDbMananger.GetType( string.Format( "DBManager.TO_{0}", tableName ) );

            return  (DBObjBase)Activator.CreateInstance( type );

        }

        #endregion
    }
}
