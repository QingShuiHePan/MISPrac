using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text ;
using System.Web;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Definition;
using SSPUCore;
using SSPUCore.Controls;

namespace DBManager
{
    [Serializable]
    public abstract partial class DBObjBase : ITableDef,IDBOperator
    {
        protected List<JDataTableColumnDefine> _columnsDefine               = null              ;
        private   bool                         _columnDefineInitialized     = false             ;
        private   DBObjBase                    _oldValueUseOnlyBeforeModify = null              ;
        protected List<string>                 _columnsForAllTables         = new List<string>();
        protected DoActionResult _doActionResultInfo = null;
        
        public DBObjBase()
        {
            InitializeColumnsDefine();
            Oninitialize();
        }

        protected virtual bool CancelAdd    { get; set; }
        protected virtual bool CancelModify { get; set; }
        protected virtual bool CancelDelete { get; set; }

        public virtual string MyTableNameInCode
        {
            get
            {
                return this.MyTableNameInDB;
            }
        }
        public virtual string[] _MyForeignIDAndForeignNameColumns
        {
            get { return new string[] { }; }
        }

        protected void Oninitialize()
        {
            Enabled = true;
        }

        private bool InputCreateTimeAuto
        {
            get { return SSPUCore.Configuration.SSPUAppSettings.GetConfig("InputCreateTimeAuto", true); }
        }

        public virtual EnumDefs.TOTableType TableType
        {
            get
            {
                return EnumDefs.TOTableType.View;
                //throw new Exception("This tale type not defined");
            }
        }

        public virtual string[] _MyColumnsArray
        {
            get { throw ( new Exception( "This table not defined _MyColumnsArray property." ) ); }
        }

        protected virtual string[] _ColumnsArrayForAdd
        {
            get { throw ( new Exception( "This table not defined _MyColumnsArray property." ) ); }
        }

        public virtual string[] _ColumnsArrayForCRUD
        {
            get
            {
                List<string> columns = new List<string>(_MyColumnsArray);
                int maxLen = columns.Count;
                for (int i = 0; i <maxLen ; i++)
                {
                    string s = columns[i];
                    if (s.Equals("ID"))
                    {
                        columns.RemoveAt(i);
                        maxLen--;
                        i--;
                    }
                    else if ( s.EndsWith("ID"))
                    {
                        string target = s.Substring(0, s.Length - 2) + "Name";
                        for (int j = i + 1; j < maxLen; j++)
                        {
                            string sj = columns[j];
                            if (sj.EndsWith(target, StringComparison.CurrentCultureIgnoreCase))
                            {
                                columns.RemoveAt(i);
                                maxLen--;
                                i--;
                            }
                        }
                    }
                    
                }
                
                return columns.ToArray();
            }

        }

        protected virtual string[] _ColumnsArrayForModify
        {
            get { throw ( new Exception( "This table not defined _MyColumnsArray property." ) ); }
        }

        private void InitializeColumnsDefine()
        {
            _columnsDefine = new List<JDataTableColumnDefine>();

            JTextBox idInput = new JTextBox();
            idInput.TextType = JTextBoxValidatedType.text;
            idInput.Required = false ;
            idInput.ReadOnly = true ;

            _columnsDefine.Add( new JDataTableColumnDefine( _ID , "" , idInput , ColumnFeatureDefine.IDType ) );

            if ( InputCreateTimeAuto )
            {
                _columnsDefine.Add( new JDataTableColumnDefine( _CreatedTime, ColumnFeatureDefine.NoEditedButVisible ) );    
            }
            
            _columnsDefine.Add( new JDataTableColumnDefine( _LastUpdateTime    , ColumnFeatureDefine.InvisibleAndNotEdited ) );
            _columnsDefine.Add( new JDataTableColumnDefine( _CreatorOperatorID , ColumnFeatureDefine.InvisibleAndNotEdited ) );
            _columnsDefine.Add( new JDataTableColumnDefine( _LastOperatorID    , ColumnFeatureDefine.InvisibleAndNotEdited ) );
            _columnsDefine.Add( new JDataTableColumnDefine( _Comments          , ColumnFeatureDefine.EditedButNotRequried ) );
            _columnsDefine.Add( new JDataTableColumnDefine( _Enabled            , ColumnFeatureDefine.InvisibleAndNotEdited ) );
            //_columnsDefine.Add( new JDataTableColumnDefine( "CreatorOperatorName"                        , ColumnFeatureDefine.NoEditedButVisible));
            //_columnsDefine.Add( new JDataTableColumnDefine( "LastOperatorName"                           , ColumnFeatureDefine.NoEditedButVisible));

            _columnsForAllTables.Add( _ID                );
            _columnsForAllTables.Add( _CreatedTime       );
            _columnsForAllTables.Add( _LastUpdateTime    );
            _columnsForAllTables.Add( _CreatorOperatorID );
            _columnsForAllTables.Add( _LastOperatorID    );
            _columnsForAllTables.Add( _Comments          );
            _columnsForAllTables.Add( _Enabled           );
            //_columnsForAllTables.Add( "CreatorOperatorName" );
            //_columnsForAllTables.Add( "LastOperatorName" );

            this[_CreatedTime].FormatStr
                = this[_LastUpdateTime].FormatStr
                = "yyyy-MM-dd HH:mm:ss";

            this.Comments = string.Empty;
        }

        protected bool IsAdmin
        {
            get { return Utility.Instance.IsAdmin; }
        }

        protected bool IsLogin
        {
            get
            {
                object obj = Utility.Instance.CurrentUser;

                return obj != null;
            }
        }

        
        protected IUserDef CurrentUser
        {
            get
            {
                return Utility.Instance.CurrentUser;
            }
        }

        public JDataTableColumnDefine this[ string columnName ]
        {
            get
            {
                foreach ( JDataTableColumnDefine columnDefine in _columnsDefine )
                {
                    if ( columnDefine.ColumnName == columnName )
                    {
                        return columnDefine;
                    }
                }

                JDataTableColumnDefine columnDef = new JDataTableColumnDefine(columnName);
                _columnsDefine.Add( columnDef );

                return columnDef;
            }
        }

        #region Implements Interface Properties

        public virtual Int32 ID { get; set; }

        public virtual bool Enabled { get; set; }

        public virtual DateTime CreatedTime { get; set; }

        public virtual Int32 CreatorOperatorID { get; set; }

        public virtual DateTime LastUpdateTime { get; set; }

        public virtual Int32 LastOperatorID { get; set; }

        public virtual string Comments { get; set; }

        public static string _ID
        {
            get { return "ID"; }
        }

         public static string _CreatedTime
        {
            get { return "CreatedTime"; }
        }
         public static string _CreatorOperatorID
        {
            get { return "CreatorOperatorID"; }
        }
         public static string _LastOperatorID
        {
            get { return "LastOperatorID"; }
        }
         public static string _LastUpdateTime
        {
            get { return "LastUpdateTime"; }
        }
         public static string _Enabled
        {
            get { return "Enabled"; }
        }

         public static string _Comments
        {
            get { return "Comments"; }
        }

        #endregion

        public virtual string MyTableNameInDB
        {
            get { throw ( new Exception( "This table not defined MyTableNameInDB property." ) ); }
        }

        public DBObjBase OldValueUseOnlyBeforeModify
        {
            get { return _oldValueUseOnlyBeforeModify; }
        }

        protected DBObjBase OldValueUseOnlyBeforeModifyCanSet
        {
            get { return _oldValueUseOnlyBeforeModify; }
            set { _oldValueUseOnlyBeforeModify = value; }
        }
        
        public virtual DataTable ModifyToDB()
        {
            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "ModifyToDBBegin" );
            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "OnPreModifyToDBDBBegin" );
            
            CancelModify = false;
            OnPreEditToDB( EnumDefs.EditType.Modify );
            if ( CancelModify )
            {
                return null;
            }

            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "OnPreModifyToDBDBEnd" );

            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "OnPreReflection" );
            List<SqlParameter> parms = new List<SqlParameter>();
            foreach ( string clmName in _ColumnsArrayForModify )
            {
                PropertyInfo p = this.GetType().GetProperty( clmName );

                parms.Add( new SqlParameter( string.Format( "@{0}", clmName ), p.GetValue( this, null ) ) );
            }

            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "OnReflectionEnd" );

            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "OnPreExcuteProcedure" );

            OnPreExcuteProcedue( EnumDefs.EditType.Modify );

            DataTable result = SSPUSqlHelper.Instance.ExcuteProcedure_GetDataTable(
                                                    string.Format( "Modify_{0}", this.MyTableNameInDB )
                                                    , parms );
            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "OnExcuteProcedueEnd" );
            this.OnEditToDBComplete( EnumDefs.EditType.Modify, result );

            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "ModifyToDBEnd" );
            
            return result;
            
        }

        private void SetDataByUIDefinBeforeWrite2DB()
        {
            List<TO_TableUIDefine> uis = CachingManager.Instance.GetTO_ObjsByCondition<TO_TableUIDefine>(
                Utility.Instance.GetSearchingCondition(TO_TableUIDefine._TableName, this.MyTableNameInDB));
            if (uis != null && uis.Count >= 1)
            {
                foreach (var toTableUiDefine in uis)//读取外表的列优先处理 //todo 可优化效率
                {
                    TableUIBase obj = SSPUConverter.Instance.DeSerialize_FromString<TableUIBase>(toTableUiDefine.UIDefine);
                    if (obj is TableUITextBoxNumberReadOut2Inner)
                    {
                        SetOneUIDefBeforWrite2DB(toTableUiDefine, uis);
                    }

                }
                foreach (var ui in uis)
                {
                    SetOneUIDefBeforWrite2DB(ui, uis);
                }
            }
        }

        private void SetOneUIDefBeforWrite2DB(TO_TableUIDefine ui, List<TO_TableUIDefine> uis)
        {
            TableUIBase obj = SSPUConverter.Instance.DeSerialize_FromString<TableUIBase>(ui.UIDefine);
            if (obj is TableUIFileRender)
            {
                TableUIFileRender fr = (TableUIFileRender) obj;
                //if (fr.RenderType == TableUIFileRenderType.Image)
                {
                    System.Reflection.PropertyInfo[] ps = this.GetType().GetProperties();
                    PropertyInfo setP = null;
                    PropertyInfo sourP = null;
                    foreach (PropertyInfo i in ps)
                    {
                        if (i.Name.Equals(ui.ColumnName))
                        {
                            setP = i;
                        }
                        else if (i.Name.Equals(fr.DataSourceColumnName))
                        {
                            sourP = i;
                        }
                    }

                    if (setP != null && sourP != null)
                    {
                        string fileSource = sourP.GetValue(this,null).ToString();
                        if (!string.IsNullOrEmpty(fileSource))
                        {
                            if (fr.RenderType == TableUIFileRenderType.Image)
                            {
                                string style = "";
                                if (!string.IsNullOrEmpty(fr.ImageWidth))
                                {
                                    if (string.IsNullOrEmpty(fr.ImageHeight))
                                    {
                                        style = string.Format("width:{0}px;height:auto;",fr.ImageWidth);
                                    }
                                    else
                                    {
                                        style = string.Format("width:{0}px;height:{1}px;",fr.ImageWidth,fr.ImageHeight);
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(fr.ImageHeight))
                                    {
                                        style = string.Format("width:auto;height:{0}px;",fr.ImageHeight);
                                    }
                                }

                                string src = string.Format("<img src=\"{0}\" style=\"{1}\" />",fileSource,style);

                                setP.SetValue(this, src, null);
                            }
                            else if (fr.RenderType == TableUIFileRenderType.LinkURL)
                            {
                                string link = string.Format("<a href=\"{0}\">{1}</a>", fileSource, fr.LinkText);
                                setP.SetValue(this, link, null);
                            }
                        }
                    }
                }
                //else if(fr.RenderType == TableUIFileRenderType.LinkURL)
                //{
                    
                //}
            }
            else if (obj is TableUITextBoxNumberReadOut2Inner)
            {
                SetTxbNumReadOut2InnerBeforWrite2DB(ui, obj);
            }
            else if (obj is TableUITextBoxNumberInnerAssociate)
            {
                SetOneInnerTxtNumAssociate(ui, uis);
            }
        }

        private void SetTxbNumReadOut2InnerBeforWrite2DB(TO_TableUIDefine ui, TableUIBase obj)
        {
            TableUITextBoxNumberReadOut2Inner tbNum = (TableUITextBoxNumberReadOut2Inner) obj;
            if (!string.IsNullOrEmpty(tbNum.AssociatTableName) &&
                !tbNum.AssociatTableName.Equals(DBDataTables.EmptyTableName))
            {
                DBObjBase toOutTableObj = Utility.Instance.GetTO_ObjByTableName(tbNum.AssociatTableName);
                Type thisPs = this.GetType();

                string currentAssociatCondition = "";
                PropertyInfo porpertyColumnValue2Set = null;
                foreach (var p in thisPs.GetProperties())
                {
                    if (p.Name.Equals(ui.ColumnName))
                    {
                        porpertyColumnValue2Set = p;
                    }

                    if (p.Name.Equals(tbNum.AssociateCurrentID))
                    {
                        currentAssociatCondition = p.GetValue(this, null).ToString();
                    }
                }

                DataTable dt = CachingManager.Instance.GetDataTable(tbNum.AssociatTableName,
                    Utility.Instance.GetSearchingCondition(tbNum.AssociatTableForeignID,
                        currentAssociatCondition));
                if (dt != null && dt.Rows.Count >= 1)
                {
                    toOutTableObj.Parse(dt.Rows[0]);

                    Type toOutPs = toOutTableObj.GetType();
                    double mutipleValue = double.Parse(tbNum.ChangedRate);
                    object outValueObj = null;
                    foreach (var p in toOutPs.GetProperties())
                    {
                        if (p.Name.Equals(tbNum.AssociatTableColumnName))
                        {
                            outValueObj = p.GetValue(toOutTableObj, null);
                            break;
                        }
                    }

                    if (PropertyIsInt(porpertyColumnValue2Set))
                    {
                        if (porpertyColumnValue2Set != null)
                        {
                            porpertyColumnValue2Set.SetValue(this, Convert.ToInt32(outValueObj) * mutipleValue, null);
                        }
                    }
                    else
                    {
                        if (porpertyColumnValue2Set != null)
                        {
                            porpertyColumnValue2Set.SetValue(this, Convert.ToDouble(outValueObj) * mutipleValue, null);
                        }
                    }
                }
            }
        }

        private void SetOneInnerTxtNumAssociate(TO_TableUIDefine udf, List<TO_TableUIDefine> uis)
        {
            TableUIBase obj = SSPUConverter.Instance.DeSerialize_FromString<TableUIBase>(udf.UIDefine);
            if (obj != null)
            {
                if (obj is TableUITextBoxNumberInnerAssociate)
                {
                    TableUITextBoxNumberInnerAssociate txb = (TableUITextBoxNumberInnerAssociate)obj;
                    //if (IsNotEmptyColumnName(txb.Column1))
                    //{
                    //    //foreach (var toTableUiDefine in uis)
                    //    //{
                    //    //    if (toTableUiDefine.ColumnName.Equals(txb.Column1))
                    //    //    {
                    //    //        SetOneInnerTxtNumAssociate(toTableUiDefine, uis);
                    //    //        break;
                    //    //    }
                    //    //}
                    //}

                    //if (IsNotEmptyColumnName(txb.Column2))
                    //{
                    //    //foreach (var toTableUiDefine in uis)
                    //    //{
                    //    //    if (toTableUiDefine.ColumnName.Equals(txb.Column2))
                    //    //    {
                    //    //        SetOneInnerTxtNumAssociate(toTableUiDefine, uis);
                    //    //        break;
                    //    //    }
                    //    //}
                    //}

                    //if (IsNotEmptyColumnName(txb.Column3))
                    //{
                    //    //foreach (var toTableUiDefine in uis)
                    //    //{
                    //    //    if (toTableUiDefine.ColumnName.Equals(txb.Column3))
                    //    //    {
                    //    //        SetOneInnerTxtNumAssociate(toTableUiDefine, uis);
                    //    //        break;
                    //    //    }
                    //    //}
                    //}

                    double? c1WithC2 = CalculateTwoColumns(txb.Column1, txb.Column2, txb.CalculateType1With2);
                    if (c1WithC2 != null)
                    {
                        PropertyInfo[] ps = this.GetType().GetProperties();

                        if (IsNotEmptyColumnName(txb.CalculateType12With3))
                        {
                            if (IsNotEmptyColumnName(txb.Column3))
                            {
                                double d3 = 0;
                                
                                foreach (var p in ps)
                                {
                                    if (p.Name.Equals(txb.Column3))
                                    {
                                        d3 = Convert.ToDouble(p.GetValue(this, null));
                                        break;
                                    }
                                }

                                if (txb.CalculateType12With3.Equals("乘"))
                                {
                                    c1WithC2 = c1WithC2 * d3;
                                }
                                else if (txb.CalculateType12With3.Equals("除"))
                                {
                                    c1WithC2 = c1WithC2 / d3;
                                }
                                else if (txb.CalculateType12With3.Equals("加"))
                                {
                                    c1WithC2 = c1WithC2 + d3;
                                }
                                else if (txb.CalculateType12With3.Equals("减"))
                                {
                                    c1WithC2 = c1WithC2 - d3;
                                }
                            }
                        }

                        foreach (var p in ps)
                        {
                            if (p.Name.Equals(udf.ColumnName))
                            {
                                if (PropertyIsInt(p))
                                {
                                    p.SetValue(this, Convert.ToInt32(c1WithC2.Value), null);
                                }
                                else if( PropertyIsDouble( p ) )
                                {
                                    p.SetValue(this, c1WithC2.Value, null);
                                }
                                break;
                            }
                        }
                    }
                }
                else if (obj is TableUITextBoxNumberReadOut2Inner)
                {
                    
                    SetTxbNumReadOut2InnerBeforWrite2DB(udf, obj);
                }
            }
        }

        private double? CalculateTwoColumns(string clm1, string clm2, string calculateType)
        {
            double? result = null;
            if (IsNotEmptyColumnName(calculateType))
            {
                if (IsNotEmptyColumnName(clm1) && IsNotEmptyColumnName(clm2))
                {
                    double d1 = 0;
                    double d2 = 0;

                    PropertyInfo[] ps = this.GetType().GetProperties();
                    foreach (var p in ps)
                    {
                        if (p.Name.Equals(clm1))
                        {
                            d1 = double.Parse(p.GetValue(this, null).ToString());
                        }

                        if (p.Name.Equals(clm2))
                        {
                            d2 = double.Parse(p.GetValue(this, null).ToString());
                        }
                    }

                    if (calculateType.Equals("乘"))
                    {
                        result = d1 * d2;
                    }
                    else if (calculateType.Equals("除"))
                    {
                        result = d1 / d2;
                    }
                    else if (calculateType.Equals("加"))
                    {
                        result = d1 + d2;
                    }
                    else if (calculateType.Equals("减"))
                    {
                        result = d1 - d2;
                    }
                }
            }

            return result;
        }

        private bool IsNotEmptyColumnName(string str)
        {
            if (!string.IsNullOrEmpty(str) && !str.Equals(GlobalString.EmptyColumnName))
            {
                return true;
            }

            return false;
        }

        private bool PropertyIsInt(PropertyInfo p )
        {
            if (p != null)
            {
                if (p.PropertyType == typeof(int) || p.PropertyType == typeof(Int32) )
                {
                    return true;
                }
            }
            return false;
        }

        private bool PropertyIsDouble(PropertyInfo p)
        {
            if (p != null)
            {
                if (p.PropertyType == typeof(double) || p.PropertyType == typeof(float) )
                {
                    return true;
                }
            }
            return false;
        }

        private bool PropertyIsNumber(PropertyInfo p)
        {
            return PropertyIsInt(p) || PropertyIsDouble(p);
        }

        public virtual int DeleteToDB()
        {
            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "DeleteToDB-Begin" );
            CancelDelete = false;

            OnPreEditToDB( EnumDefs.EditType.Delete );
            if( CancelDelete )
            {
                return 0;
            }
            OnPreExcuteProcedue(EnumDefs.EditType.Delete);
            int result = SSPUSqlHelper.Instance.Delete_DataInTable( MyTableNameInDB, "ID", ID.ToString() );
            OnEditToDBComplete( EnumDefs.EditType.Delete, null );

            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "DeleteToDB-End" );
            return result ;
        }

        public virtual DataTable AddToDB()
        {
            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "AddToDB" );

            CancelAdd = false;
            this.OnPreEditToDB( EnumDefs.EditType.Add );
            if ( CancelAdd )
            {
                return null;
            }

            List<SqlParameter> parms = new List<SqlParameter>();
            foreach (string clmName in _ColumnsArrayForAdd)
            {
                PropertyInfo p = this.GetType().GetProperty( clmName );

                parms.Add( new SqlParameter( string.Format( "@{0}", clmName ), p.GetValue( this, null ) ) );
            }

            

            OnPreExcuteProcedue(EnumDefs.EditType.Add);

            DataTable result = SSPUSqlHelper.Instance.ExcuteProcedure_GetDataTable( 
                                                    string.Format( "Add_{0}",this.MyTableNameInDB )
                                                    , parms );
            this.OnEditToDBComplete( EnumDefs.EditType.Add, result);
            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "AddToDBEnd" );
            return result;
            //throw ( new Exception( "AddToDB not implement in this class" ) );
        }

        protected virtual void OnPreExcuteProcedue( EnumDefs.EditType editType )
        {
            return;
        }

        public virtual void Parse( DataRow row )
        {
            OnPreParse(row);
            foreach (string clm in this._MyColumnsArray)
            {
                if( row.Table.Columns.Contains(clm) )
                {
                    if (!(row[clm] is DBNull))
                    {
                        this.GetType().GetProperty(clm).SetValue(this, row[clm], null);
                    }
                }
            }
            OnParseComplete(row);
            
        }

        public virtual void ParseExcel( DataRow row )
        {
            OnPreParse( row );
            foreach ( string clm in this._MyColumnsArray )
            {
                if ( row.Table.Columns.Contains( clm ) )
                {
                    if ( !( row[clm] is DBNull ) && !string.IsNullOrEmpty( row[clm].ToString() ) )
                    {
                        object o = null;

                        Type tp = this.GetType().GetProperty(clm).PropertyType;

                        if ( tp == typeof( bool ) )
                        {
                            bool b = false;
                            if ( bool.TryParse( row[clm].ToString(), out b ) )
                            {
                                o = b;
                            }
                            else
                            {
                                o = row[clm].ToString().Equals( ControlUtility.BoolTrueString );
                            }

                        }
                        else if ( tp == typeof( int ) )
                        {
                            o = int.Parse(row[clm].ToString());
                        }
                        else if ( tp == typeof( long ) )
                        {
                            o = long.Parse(row[clm].ToString());
                        }
                        else if ( tp == typeof( double ))
                        {
                            o = double.Parse(row[clm].ToString());
                        }
                        else if( tp == typeof( decimal )  )
                        {
                            o = decimal.Parse(row[clm].ToString());
                        }
                        else if ( tp == typeof( DateTime ) )
                        {
                            o = DateTime.Parse(row[clm].ToString());
                        }
                        else 
                        {
                            o = row[clm];
                        }
                        


                        if( o != null )
                        {
                            this.GetType().GetProperty( clm ).SetValue( this, o, null );
                        }
                        
                    }
                }
            }
            OnParseComplete( row );

        }

        protected void SetOperatorID( EnumDefs.EditType editType )
        {
            //if( !(this is IEditWithoutLogin ))
            //{   
            //    if ( CurrentUser == null )
            //    {
            //        throw new Exception( "未登录，没有权限修改数据库！" );
            //    }
            //    if( editType == EnumDefs.EditType.Add )
            //    {
            //        this.CreatorOperatorID = CurrentUser.ID;    
            //    }
            //    else
            //    {
            //        this.LastOperatorID = CurrentUser.ID;
            //    }
                
            //}

            int operatorID = Utility.Instance.CurrenToUserInfo.ID;
            if (editType == EnumDefs.EditType.Add)
            {
                this.CreatorOperatorID = operatorID;
            }
            else
            {
                this.LastOperatorID = operatorID;
            }


        }

        protected virtual void OnPreEditToDB( EnumDefs.EditType editType )
        {
            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "OnPreEditToDB-Begin" );

            if (this is INotRepeatDef)
            {
                if (editType != EnumDefs.EditType.Delete)
                {
                    INotRepeatDef obj = this as INotRepeatDef;
                    Type          t   = this.GetType();
                    foreach (var clm in obj.NotRepeatColumns)
                    {
                        PropertyInfo pi = t.GetProperty(clm);
                        if (pi.PropertyType == typeof(String))
                        {
                            pi.SetValue(this, pi.GetValue(this, null).ToString().Trim(), null);
                        }
                        else if (pi.PropertyType == typeof(int))
                        {
                            pi.SetValue(this, pi.GetValue(this, null), null);
                        }

                    }


                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    foreach (var clm in obj.NotRepeatColumns)
                    {
                        dic.Add(clm, this.GetType().GetProperty(clm).GetValue(this, null).ToString());
                    }

                    if (CachingManager.Instance.HasRepeatData(this.MyTableNameInDB, dic, this))
                    {
                        throw new Exception("数据库中存在重复数据");
                    }
                }

                
            }

           

            if( editType == EnumDefs.EditType.Add )
            {
                if (InputCreateTimeAuto || this.CreatedTime == DateTime.MinValue)
                {
                    this.CreatedTime = DateTime.Now;
                }
                if (this.CreatedTime.ToString("HH:mm:ss") == "00:00:00")
                {
                    this.CreatedTime =
                        DateTime.Parse(this.CreatedTime.ToString("yyyy/MM/dd ") + DateTime.Now.ToString(" HH:mm:ss"));

                }
                SetDataByUIDefinBeforeWrite2DB();
            }
            else if( editType == EnumDefs.EditType.Modify )
            {
                SetDataByUIDefinBeforeWrite2DB();
                SetMyData( editType);
                
                if ( this.CreatedTime == DateTime.MinValue )
                {
                    this.CreatedTime = DateTime.Now;
                }
            }
            else if( editType == EnumDefs.EditType.Delete )
            {
                SetMyData( editType );
            }

            if (!(this is INoLoginEdited))
            {
                SetOperatorID(editType);
            }
            

            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "OnPreEditToDB-End" );
            return;
        }
        
        private void SetMyData( DataTable dt )
        {
            //deleted
        }

        private void SetMyData( EnumDefs.EditType editType)
        {
            //deleted
        }
        
        public virtual void OnPreParse(Dictionary<string, string> postDic)
        {
            
        }

        public virtual void OnParseComplete(Dictionary<string, string> postDic)
        {

        }

        public virtual void OnPreParse(DataRow row)
        {

        }

        public virtual void OnParseComplete(DataRow row)
        {

        }

        public virtual DBObjBase GetClone()
        {
            DBObjBase obj = System.Activator.CreateInstance(this.GetType()) as DBObjBase;

            foreach (string s in this._MyColumnsArray)
            {
                PropertyInfo p = this.GetType().GetProperty(s);
                p.SetValue( obj, p.GetValue( this, null ), null );
            }

            return obj;
        }

        public virtual void Parse( Dictionary<string, string> postDic )
        {
            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "ParseBegin" );
            OnPreParse( postDic );

            if( postDic.Keys.Contains( "ID" ) && !string.IsNullOrEmpty( postDic["ID"] ) )
            {
                postDic["ID"] = postDic["ID"].Replace(",","");
                if( this is INotEncodeID )
                {
                    this.ID = int.Parse(postDic["ID"]);

                    if(this is IEncodeIDInDataTable )
                    {
                        this.ID = SSPUCore.SSPUConverter.Instance.DecodingNumber( postDic["ID"].ToString() );
                    }

                    if( ID == int.MinValue )
                    {
                        this.ID = int.Parse( postDic["ID"] );    
                    }
                }
                else
                {
                    try
                    {
                        this.ID = SSPUCore.SSPUConverter.Instance.DecodingNumber(postDic["ID"]);
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            this.ID = int.Parse(postDic["ID"]);
                        }
                        catch (Exception exception)
                        {
                            throw;
                        }
                    }
                }

                postDic.Remove("ID");
            }

            if( postDic.Keys.Contains( JDataTable.RealClientIDValue ) )
            {
                if( !( this is INotEncodeID ) )
                {
                    if( !postDic[JDataTable.RealClientIDValue].Trim().Equals( "0" , StringComparison.CurrentCultureIgnoreCase ) )
                    {
                        int realID = SSPUCore.SSPUConverter.Instance.DecodingNumber( postDic[JDataTable.RealClientIDValue] );

                        if( this.ID != realID )
                        {
                            throw new Exception( "数据非法！" );
                        }
                    }
                }

                postDic.Remove( JDataTable.RealClientIDValue );
            }

            ReadOutDataBeforeModify();

            foreach (KeyValuePair<string, string> keyValuePair in postDic)
            {
                if( this._MyColumnsArray.Contains(keyValuePair.Key) )
                {
                    PropertyInfo p = this.GetType().GetProperty( keyValuePair.Key );

                    {
                        if ( p.PropertyType.FullName == "System.Byte[]" )
                        {
                            string propertyName = keyValuePair.Key + "FilePathName";
                            p = this.GetType().GetProperty( propertyName );
                            p.SetValue( this, Convert.ChangeType( keyValuePair.Value, p.PropertyType ), null );
                        }
                        else
                        {
                            Type tp = p.PropertyType;
                            object o = null;
                            if ( tp == typeof( bool ) )
                            {
                                bool b = false;
                                if ( bool.TryParse( keyValuePair.Value, out b ) )
                                {
                                    o = b;
                                }
                                else
                                {
                                    o = keyValuePair.Value.Equals( ControlUtility.BoolTrueString );    
                                }
                                
                            }
                            else if ( tp == typeof( int ) )
                            {
                                if ( string.IsNullOrEmpty( keyValuePair.Value ) )
                                {
                                    o = 0;    
                                }
                                else
                                {
                                    string str = keyValuePair.Value.Replace(",", "");
                                    o = int.Parse( str );
                                }
                                
                            }
                            else if ( tp == typeof( long ) )
                            {
                                if( string.IsNullOrEmpty( keyValuePair.Value ) )
                                {
                                    o = 0;
                                }
                                else
                                {
                                    string str = keyValuePair.Value.Replace( ",", "" );
                                    o = long.Parse( str );
                                }
                            }
                            else if ( tp == typeof( double ) )
                            {
                                if( string.IsNullOrEmpty( keyValuePair.Value ) )
                                {
                                    o = 0.0;
                                }
                                else
                                {
                                    string str = keyValuePair.Value.Replace( ",", "" );
                                    o = double.Parse( str );    
                                }
                                
                            }
                            else if ( tp == typeof( decimal ) )
                            {
                                if( string.IsNullOrEmpty( keyValuePair.Value ) )
                                {
                                    o = new decimal(0.0);
                                }
                                else
                                {
                                    string str = keyValuePair.Value.Replace( ",", "" );
                                    o = decimal.Parse( str );
                                }
                            }
                            else if ( tp == typeof( DateTime ) )
                            {
                                o = DateTime.Parse( keyValuePair.Value );
                            }
                            else
                            {
                                o = keyValuePair.Value;
                            }

                            p.SetValue( this, o , null );
                        }
                    }
                }
                
            }


            if( postDic.Keys.Contains( JDataTable.UsingInWorkflowForDictionParse ) 
                && !string.IsNullOrEmpty( postDic[JDataTable.UsingInWorkflowForDictionParse] ) )
            {
                //deleted
            }

            OnParseComplete( postDic );

            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "ParseEnd" );
        }

        /// <summary>
        /// 此函数仅当用户只能修改一两个字段，其它字段隐藏且保持不变时才有意义
        /// </summary>
        protected void ReadOutDataBeforeModify()
        {
            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "ReadOutDataBeforeModifyBegin" );
            DataTable dt = SSPUSqlHelper.Instance.Get_TopNOfTableByCondition( this.MyTableNameInDB, 0, "ID", this.ID.ToString() ).Tables[0];

            if ( dt != null && dt.Rows.Count >= 1 )
            {
                this.Parse( dt.Rows[0] );
                _oldValueUseOnlyBeforeModify = System.Activator.CreateInstance( this.GetType() ) as DBObjBase ;
                _oldValueUseOnlyBeforeModify.Parse( dt.Rows[0] );
            }

            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "ReadOutDataBeforeModifyEnd" );
        }

        private void ParseSelf( DataTable dt )
        {
            if ( dt == null || dt.Rows.Count <= 0 )
            {
                if ( SSPUCore.Configuration.EnvironmentConfig.IsDevelopment )
                {
                    throw new Exception( string.Format( "表{0}的更新出错！", this.MyTableNameInDB ) );
                }
            }
            else
            {
                this.Parse( dt.Rows[0] );

                foreach ( string clm in this._MyColumnsArray )
                {
                    if ( dt.Columns.Contains( clm ) )
                    {
                        if (  dt.Rows[0][clm] is byte[] )
                        {
                            //to set file data to 0 byte before pushing it into cache
                            dt.Rows[0][clm] = new byte[] {};
                        }
                    }
                }

            }
        }

        protected virtual bool RecordeThisModification2DB
        {
            get { return true; }
        }

        protected virtual string TrimColon( string s )
        {
            return s.Replace( "\'", "" ).Replace( "\"", "" );
        }

        protected virtual DataTable MyData { get; set; }

        private Dictionary<string,string> IDCondition
        {
            get
            {
                Dictionary<string,string> codition = new Dictionary<string, string>();
                codition.Add( "ID", this.ID.ToString() );
                return codition;
            }
        }

        public virtual DataTable GetDetailInfoByKeyAndID( string columnName, string value )
        {
            return CachingManager.Instance.GetDataTable(this.MyTableNameInDB,
                                                        Utility.Instance.GetSearchingCondition(columnName, value));
        }
 
        private void WriteUserOperator2DB(EnumDefs.EditType editType )
        {
            //deleted
        }

        protected virtual void OnEditToDBComplete( EnumDefs.EditType editType, DataTable result)
        {

            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "OnEditToDBComplete-Begin" );

            if( editType == EnumDefs.EditType.Add )
            {
                SetMyData( result );

                ParseSelf( result );

                CachingManager.Instance.AddNewRowIntoCaching( this.MyTableNameInDB, this, result.Rows[0] );

                ChangeAssociatTableValue(editType);

                WriteUserOperator2DB( EnumDefs.EditType.Add );
 
            }
            else if( editType == EnumDefs.EditType.Modify )
            {

                try
                {
                    ParseSelf(result);
                    CachingManager.Instance.ModifyRowInCaching(this.MyTableNameInDB, this, result.Rows[0]);

                    ChangeAssociatTableValue( editType );
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }


                WriteUserOperator2DB( EnumDefs.EditType.Modify );

            }
            else if( editType == EnumDefs.EditType.Delete )
            {
                CachingManager.Instance.DeleteRowIntoCaching( this.MyTableNameInDB, this );

                ChangeAssociatTableValue(editType);

                WriteUserOperator2DB( EnumDefs.EditType.Delete );
            }

            CoreUtil.Trace( "DBObjBase" + this.MyTableNameInDB, "OnEditToDBComplete-End" );
        }

        private void ChangeAssociatTableValue(EnumDefs.EditType editType)
        {
            List<TO_TableUIDefine> uis = CachingManager.Instance.GetTO_ObjsByCondition<TO_TableUIDefine>(
                Utility.Instance.GetSearchingCondition(TO_TableUIDefine._TableName, this.MyTableNameInDB));
            if (uis != null && uis.Count >= 1)
            {
                foreach (var ui in uis)
                {
                    TableUIBase obj = SSPUConverter.Instance.DeSerialize_FromString<TableUIBase>(ui.UIDefine);

                    if (obj is TableUITextBoxNumberWrite2Out && !(obj is TableUITextBoxNumberReadOut2Inner))
                    {
                        Write2OutTableByUIDef(editType, ui,(TableUITextBoxNumberWrite2Out)obj );
                    }
                    else if( obj is TableUITextBoxNumberInnerAssociate)
                    {
                        TableUITextBoxNumberInnerAssociate txb = (TableUITextBoxNumberInnerAssociate)obj;
                        if (txb.WriteOutDef != null)
                        {
                            Write2OutTableByUIDef(editType, ui, txb.WriteOutDef);
                        }
                    }
                    
                }
            }
        }

        private void Write2OutTableByUIDef(EnumDefs.EditType editType, TO_TableUIDefine ui , TableUITextBoxNumberWrite2Out tbNum)
        {
            if (tbNum != null)
            {
                
                if (!string.IsNullOrEmpty(tbNum.AssociatTableName) &&
                    !tbNum.AssociatTableName.Equals(DBDataTables.EmptyTableName))
                {
                    DBObjBase toObj = Utility.Instance.GetTO_ObjByTableName(tbNum.AssociatTableName);
                    bool hasInDB = false;
                    string thisIDValue = string.Empty;
                    Type thisPs = this.GetType();

                    int valueChangedNumberInt = 0;
                    double valueChangedNumberDouble = 0;
                    string currentAssociatID = string.Empty;
                    foreach (var p in thisPs.GetProperties())
                    {
                        if (p.Name.Equals(tbNum.AssociateCurrentID))
                        {
                            thisIDValue = p.GetValue(this, null).ToString();
                        }

                        if (p.Name.Equals(ui.ColumnName))
                        {
                            object changingValueCurrent = p.GetValue(this, null);
                            double changingValueOld = 0;
                            if (editType == EnumDefs.EditType.Modify)
                            {
                                changingValueOld = double.Parse(p.GetValue(_oldValueUseOnlyBeforeModify, null).ToString());
                            }

                            if ((p.PropertyType.Equals(typeof(int)) || (p.PropertyType.Equals(typeof(Int32)))))
                            {
                                valueChangedNumberInt = Convert.ToInt32(changingValueCurrent) - Convert.ToInt32(changingValueOld);
                            }
                            else
                            {
                                valueChangedNumberDouble =
                                     Convert.ToDouble(changingValueCurrent) -Convert.ToDouble(changingValueOld);
                            }

                            if (editType == EnumDefs.EditType.Delete)
                            {
                                valueChangedNumberInt *= -1;
                                valueChangedNumberDouble *= -1;
                            }
                        }

                        if (p.Name.Equals(tbNum.AssociateCurrentID))
                        {
                            currentAssociatID = p.GetValue(this, null).ToString();
                        }
                    }

                    DataTable dt = CachingManager.Instance.GetDataTable(tbNum.AssociatTableName,
                        Utility.Instance.GetSearchingCondition(tbNum.AssociatTableForeignID,
                            thisIDValue));
                    if (dt != null && dt.Rows.Count >= 1)
                    {
                        hasInDB = true;
                        toObj.Parse(dt.Rows[0]);
                    }

                    Type toPs = toObj.GetType();
                    double mutipleValue = double.Parse(tbNum.ChangedRate);
                    foreach (var p in toPs.GetProperties())
                    {
                        if (p.Name.Equals(tbNum.AssociatTableColumnName))
                        {
                            if (valueChangedNumberInt != 0)
                            {
                                int oldValue = Convert.ToInt32(p.GetValue(toObj, null));
                                p.SetValue(toObj, oldValue + valueChangedNumberInt * Convert.ToInt32(mutipleValue), null);
                            }
                            else if (valueChangedNumberDouble != 0)
                            {
                                double oldValue = Convert.ToDouble(p.GetValue(toObj, null));
                                p.SetValue(toObj, oldValue + valueChangedNumberDouble * mutipleValue, null);
                            }
                        }

                        if (p.Name.Equals(tbNum.AssociatTableForeignID))
                        {
                            if (PropertyIsInt(p))
                            {
                                p.SetValue(toObj, int.Parse(currentAssociatID), null);
                            }
                            else if (PropertyIsDouble(p))
                            {
                                p.SetValue(toObj,double.Parse(currentAssociatID), null);
                            }
                            else if( p.PropertyType == typeof(String) )
                            {
                                p.SetValue(toObj, currentAssociatID, null);
                            }

                            
                        }
                    }

                    if (hasInDB)
                    {
                        toObj.ModifyToDB();
                    }
                    else
                    {
                        toObj.AddToDB();
                    }
                }
            }
        }

        private void RecordUserOperator( EnumDefs.EditType editType )
        {
            //deleted
        }

        public virtual DataTable GetDataTableFromDB()
        {
            return SSPUSqlHelper.Instance.Get_TopNOfTable( MyTableNameInDB , 0 ).Tables[0];
        }

        protected virtual void LoadClassFeaturs()
        {
            
        }

        protected virtual void OnPreLoadClassFeaturs()
        {
            
            
        }

        protected virtual void LoadConfigedFeature()
        {
            Dictionary<string, int> clmAuths = AuthorityManager.Instance.CurrentColumnsEditType;
            if (clmAuths != null && clmAuths.Count >= 1)
            {
                foreach (var auth in clmAuths)
                {
                    if ((auth.Value & (int)EnumDefs.FieldAuthority.EditAllowNull) != 0)
                    {
                        this[auth.Key].ColumnType = ColumnFeatureDefine.EditedButNotRequried;
                    }
                    else if ((auth.Value & (int)EnumDefs.FieldAuthority.EditedNotNull) != 0)
                    {
                        this[auth.Key].ColumnType = ColumnFeatureDefine.Null;
                    }
                    else if ((auth.Value & (int)EnumDefs.FieldAuthority.SumAndEdited) != 0)
                    {
                        this[auth.Key].ColumnType = ColumnFeatureDefine.ToSum|ColumnFeatureDefine.EditedButNotRequried;
                    }
                    else if ((auth.Value & (int)EnumDefs.FieldAuthority.Sum) != 0)
                    {
                        this[auth.Key].ColumnType = ColumnFeatureDefine.ToSum ;
                    }
                    else if ((auth.Value & (int)EnumDefs.FieldAuthority.ReadOnly) != 0)
                    {
                        this[auth.Key].ColumnType = ColumnFeatureDefine.NoEditedButVisible;
                    }
                    else if ((auth.Value ==(int)EnumDefs.FieldAuthority.Hidden) )
                    {
                        this[auth.Key].ColumnType = ColumnFeatureDefine.InvisibleAndNotEdited;
                    }
                    else if ((auth.Value == (int)EnumDefs.FieldAuthority.EditableButInvisible))
                    {
                        this[auth.Key].ColumnType = ColumnFeatureDefine.EditableButInvisible;
                    }
                }
            }
        }

        private void SetStyleByInterface()
        {
            if ( this is IColumnsDenyEditAndVisible )
            {
                foreach ( string s in this._MyColumnsArray )
                {
                    if ( !_columnsForAllTables.Contains( s ) )
                    {
                        this[s].ColumnType = ColumnFeatureDefine.NoEditedButVisible;
                    }

                }
            }
            if ( this is IColumnsNotRequired )
            {
                foreach ( string s in this._MyColumnsArray )
                {
                    if ( !_columnsForAllTables.Contains( s ) )
                    {
                        this[s].ColumnType = ColumnFeatureDefine.EditableButInvisible;
                    }

                }
            }
        }
        protected virtual void OnLoadClassFeatursComplete()
        {
            
        }

        public List<JDataTableColumnDefine> ColumnsDefine
        {
            get
            {
                if( !_columnDefineInitialized )
                {
                    _columnDefineInitialized = true;
                    SetStyleByInterface();
                    LoadClassFeaturesFromDBUIDefine();
                    OnPreLoadClassFeaturs() ;
                    LoadClassFeaturs();
                    OnLoadClassFeatursComplete() ;
                    LoadConfigedFeature();
                }
                return _columnsDefine;
            }
        }
        

        private void LoadClassFeaturesFromDBUIDefine()
        {
            List<TO_TableUIDefine> uis = CachingManager.Instance.GetTO_ObjsByCondition<TO_TableUIDefine>(
                Utility.Instance.GetSearchingCondition(TO_TableUIDefine._TableName, this.MyTableNameInDB));
            if (uis != null && uis.Count >= 1)
            {
                foreach (var ui in uis)
                {
                    WebControl ctrl = null;
                    TableUIBase obj = null;
                    obj = SSPUConverter.Instance.DeSerialize_FromString<TableUIBase>(ui.UIDefine);

                    if (obj != null)
                    {
                        if (obj is TableUITextBoxNumberInnerAssociate)
                        {
                            this[ui.ColumnName].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;
                        }else if (obj is TableUITextBoxNumberReadOut2Inner)
                        {
                            this[ui.ColumnName].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;
                        }
                        else if (obj is TableUIJDropDown)
                        {
                            TableUIJDropDown uidef = (TableUIJDropDown)obj;

                            JDropDown dd = new JDropDown();

                            if (!string.IsNullOrEmpty(uidef.ItemsStr))
                            {
                                string[] items = uidef.ItemsStr.Split(new[] {',', '，'});

                                foreach (var item in items)
                                {
                                    dd.Items.Add(item);
                                }
                            }

                            ctrl = dd;

                        }
                        else if (obj is TableUIUploadify)
                        {
                            TableUIUploadify uid = (TableUIUploadify) obj;
                            Uploadify up = new Uploadify();

                            if (!string.IsNullOrEmpty(uid.FileDescription))
                            {
                                up.FileDesc = "File Types:" + uid.FileDescription;
                            }

                            if (!string.IsNullOrEmpty(uid.FileAllowTypes))
                            {
                                up.FileExt = uid.FileAllowTypes;
                            }

                            ctrl = up;
                            this[ui.ColumnName].ColumnType |= ColumnFeatureDefine.EditableButInvisible;
                        }
                        else if (obj is TableUIFileRender)
                        {
                            this[ui.ColumnName].ColumnType |= ColumnFeatureDefine.IsImagePath;
                            //
                        }
                        else if (obj is TableUIInputByTree)
                        {
                            TableUIInputByTree uid = (TableUIInputByTree) obj;
                            
                            InputByTreeItem bt = new InputByTreeItem();

                            bt.AllowMultiple = uid.EnableMultiSelect;

                            if (uid.NodesSourceTableName.Equals(DBDataTables.EmptyTableName))
                            {
                                TreeNodeCollection cnd = new TreeNodeCollection();
                                if (!string.IsNullOrEmpty(uid.NodesSourceText))
                                {
                                    string[] nds = uid.NodesSourceText.Split(new[] {',', '，'});
                                    foreach (var nd in nds)
                                    {
                                        cnd.Add(new JTreeNode(nd,nd,"","",nd));
                                    }

                                    bt.DataNodes = cnd;
                                }
                                

                               
                            }
                            else
                            {
                                TreeNodeCollection cnd = new TreeNodeCollection();
                                DBObjBase sourObj = Utility.Instance.GetTO_ObjByTableName(uid.NodesSourceTableName);
                                if (sourObj != null)
                                {
                                    if (sourObj._MyColumnsArray.Contains("ID") &&
                                        sourObj._MyColumnsArray.Contains("Name"))
                                    {
                                        DataTable dt =
                                            SSPUSqlHelper.Instance.Get_TopNOfTable(uid.NodesSourceTableName, 0).Tables[0];
                                        if (!string.IsNullOrEmpty(uid.TableFilterString))
                                        {
                                            dt.DefaultView.RowFilter = uid.TableFilterString;
                                            dt = dt.DefaultView.ToTable("dt");
                                        }

                                        foreach (DataRow dataRow in dt.Rows)
                                        {
                                            cnd.Add(new JTreeNode(dataRow["Name"].ToString(), dataRow["Name"].ToString(), "", "", dataRow["ID"].ToString()));
                                        }
                                    }

                                    bt.DataNodes = cnd;
                                }
                            }

                            bt.Required = uid.Required;
                            
                            ctrl = bt;
                            
                        }else if (obj is TableUITextBox)
                        {
                            TableUITextBox uidef = (TableUITextBox)obj;

                            JTextBox txb = new JTextBox();
                            txb.Required = uidef.Required;
                            txb.ErrorMessage = uidef.ErrorInfo;

                            switch (uidef.TextBoxType)
                            {
                                case TableUITextBoxType.Double:
                                    {
                                        txb.TextType = JTextBoxValidatedType.DOUBLE;
                                        double min, max;
                                        if (double.TryParse(((TableUITextBoxNumber)uidef).Min, out min))
                                        {
                                            txb.Min = min;
                                        }

                                        if (double.TryParse(((TableUITextBoxNumber)uidef).Max, out max))
                                        {
                                            txb.Max = max;
                                        }

                                    }
                                    break;
                                case TableUITextBoxType.Int:
                                    {
                                        txb.TextType = JTextBoxValidatedType.INT;
                                        int min, max;
                                        if (int.TryParse(((TableUITextBoxNumber)uidef).Min, out min))
                                        {
                                            txb.Min = min;
                                        }

                                        if (int.TryParse(((TableUITextBoxNumber)uidef).Max, out max))
                                        {
                                            txb.Max = max;
                                        }

                                    }
                                    break;
                                case TableUITextBoxType.Normal:

                                    break;
                                case TableUITextBoxType.Regex:
                                    txb.Pattern = uidef.RegexStr.Replace("\\", "\\\\");
                                    break;
                            }

                            ctrl = txb;
                        }
                        

                            this[ui.ColumnName].ControlForEdited = ctrl;
                    }
                }
            }
            
        }
        

        public virtual void DoAction(DoActionInfo acInfo)
        {
            if (_doActionResultInfo == null)
            {
                _doActionResultInfo = new DoActionResult(DoActionType.MODIFY, "");
            }
        }

        public virtual Dictionary<string, string> NameMapping { get; set ; }

        public virtual DoActionResult ActionResultInfo
        {
            get
            {
                return _doActionResultInfo;
            }
        }

    }
}
