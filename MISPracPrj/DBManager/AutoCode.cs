using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;
using SSPUCore;
using SSPUCore.Controls;
using SSPUCore.Configuration;
//This file is auto generated. Any changes will be recovered. Please do not change it.
namespace DBManager
{
    #region Procedures definition

    public partial class SSPUSqlHelper
    {
        public int Delete_DataInTable(string TableName, string ConditionColumn, string ConditionValue)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            SqlParameter TableNameParm = new SqlParameter("@TableName", SqlDbType.VarChar); ;
            TableNameParm.Value = TableName;
            parms.Add(TableNameParm);
            SqlParameter ConditionColumnParm = new SqlParameter("@ConditionColumn", SqlDbType.VarChar); ;
            ConditionColumnParm.Value = ConditionColumn;
            parms.Add(ConditionColumnParm);
            SqlParameter ConditionValueParm = new SqlParameter("@ConditionValue", SqlDbType.VarChar); ;
            ConditionValueParm.Value = ConditionValue;
            parms.Add(ConditionValueParm);
            return ExcuteProcedure_NoReturnData("Delete_DataInTable", parms);
        }

        public DataSet Get_TopNOfTableWithOption(string TableName, Int32 TopNumber, string Option)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            SqlParameter TableNameParm = new SqlParameter("@TableName", SqlDbType.VarChar); ;
            TableNameParm.Value = TableName;
            parms.Add(TableNameParm);
            SqlParameter TopNumberParm = new SqlParameter("@TopNumber", SqlDbType.Int); ;
            TopNumberParm.Value = TopNumber;
            parms.Add(TopNumberParm);
            SqlParameter OptionParm = new SqlParameter("@Option", SqlDbType.Text); ;
            OptionParm.Value = Option;
            parms.Add(OptionParm);
            DataSet result = null;
            result = ExcuteProcedure_GetDataSet("Get_TopNOfTableWithOption", parms);
            return result;
        }

        public DataSet Get_TopNOfTableByCondition(string TableName, Int32 TopNumber, string ConditionColumn, string ConditionValue)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            SqlParameter TableNameParm = new SqlParameter("@TableName", SqlDbType.VarChar); ;
            TableNameParm.Value = TableName;
            parms.Add(TableNameParm);
            SqlParameter TopNumberParm = new SqlParameter("@TopNumber", SqlDbType.Int); ;
            TopNumberParm.Value = TopNumber;
            parms.Add(TopNumberParm);
            SqlParameter ConditionColumnParm = new SqlParameter("@ConditionColumn", SqlDbType.VarChar); ;
            ConditionColumnParm.Value = ConditionColumn;
            parms.Add(ConditionColumnParm);
            SqlParameter ConditionValueParm = new SqlParameter("@ConditionValue", SqlDbType.VarChar); ;
            ConditionValueParm.Value = ConditionValue;
            parms.Add(ConditionValueParm);
            DataSet result = null;
            result = ExcuteProcedure_GetDataSet("Get_TopNOfTableByCondition", parms);
            return result;
        }

        public DataSet Get_TopNOfTable(string TableName, Int32 TopNumber)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            SqlParameter TableNameParm = new SqlParameter("@TableName", SqlDbType.VarChar); ;
            TableNameParm.Value = TableName;
            parms.Add(TableNameParm);
            SqlParameter TopNumberParm = new SqlParameter("@TopNumber", SqlDbType.Int); ;
            TopNumberParm.Value = TopNumber;
            parms.Add(TopNumberParm);
            DataSet result = null;
            result = ExcuteProcedure_GetDataSet("Get_TopNOfTable", parms);
            return result;
        }

        public DataSet Get_TableAuthorityDistinctTableName()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            DataSet result = null;
            result = ExcuteProcedure_GetDataSet("Get_TableAuthorityDistinctTableName", parms);
            return result;
        }

    }
    #endregion

    #region DataTable object definition

    //class definition of Table_Sample
    [Serializable]
    public partial class TO_Table_Sample : DBObjBase, IDBOperator
    {
        public TO_Table_Sample()
        {
            Name = string.Empty;
            ShowDialogTableLink = string.Empty;
            DoJSACtionLink = string.Empty;
            Photo = string.Empty;
            PhotoForShow = string.Empty;
        }
        public virtual string Name { get; set; }
        public virtual string ShowDialogTableLink { get; set; }
        public virtual string DoJSACtionLink { get; set; }
        public virtual string Photo { get; set; }
        public virtual string PhotoForShow { get; set; }
        public override string MyTableNameInDB { get { return "Table_Sample"; } }
        public override Definition.EnumDefs.TOTableType TableType { get { return Definition.EnumDefs.TOTableType.Table; ; } }
        #region table name and columns
        public static readonly string _Name = "Name";
        public static readonly string _ShowDialogTableLink = "ShowDialogTableLink";
        public static readonly string _DoJSACtionLink = "DoJSACtionLink";
        public static readonly string _Photo = "Photo";
        public static readonly string _PhotoForShow = "PhotoForShow";
        public override string[] _MyColumnsArray
        {
            get
            {
                return new string[] { "ID", "Name", "ShowDialogTableLink", "DoJSACtionLink", "Photo", "PhotoForShow", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID", "LastUpdateTime", "LastOperatorID" };
            }
        }

        public static readonly string _MyTableName = "Table_Sample";
        #endregion
        protected override string[] _ColumnsArrayForAdd
        {
            get
            {
                return new string[] { "Name", "ShowDialogTableLink", "DoJSACtionLink", "Photo", "PhotoForShow", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID" };
            }
        }

        protected override string[] _ColumnsArrayForModify
        {
            get
            {
                return new string[] { "ID", "Name", "ShowDialogTableLink", "DoJSACtionLink", "Photo", "PhotoForShow", "Comments", "Enabled", "CreatedTime", "LastOperatorID" };
            }
        }

        protected override void LoadClassFeaturs()
        {
            base.LoadClassFeaturs();
            this[_PhotoForShow].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;
            this[_ID].CssClass = "__NumberType";
            this[_CreatorOperatorID].CssClass = "__NumberType";
            this[_LastOperatorID].CssClass = "__NumberType";

        }

    }

    //class definition of GlobalNavTree
    [Serializable]
    public partial class TO_GlobalNavTree : DBObjBase, IDBOperator
    {
        public TO_GlobalNavTree()
        {
            Name = string.Empty;
            NavTo = string.Empty;
        }
        public virtual string Name { get; set; }
        public virtual Int32 ParentID { get; set; }
        public virtual string NavTo { get; set; }
        public virtual Int32 OrderIndex { get; set; }
        public override string MyTableNameInDB { get { return "GlobalNavTree"; } }
        public override Definition.EnumDefs.TOTableType TableType { get { return Definition.EnumDefs.TOTableType.Table; ; } }
        #region table name and columns
        public static readonly string _Name = "Name";
        public static readonly string _ParentID = "ParentID";
        public static readonly string _NavTo = "NavTo";
        public static readonly string _OrderIndex = "OrderIndex";
        public override string[] _MyColumnsArray
        {
            get
            {
                return new string[] { "ID", "Name", "ParentID", "NavTo", "OrderIndex", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID", "LastUpdateTime", "LastOperatorID" };
            }
        }

        public static readonly string _MyTableName = "GlobalNavTree";
        #endregion
        protected override string[] _ColumnsArrayForAdd
        {
            get
            {
                return new string[] { "Name", "ParentID", "NavTo", "OrderIndex", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID" };
            }
        }

        protected override string[] _ColumnsArrayForModify
        {
            get
            {
                return new string[] { "ID", "Name", "ParentID", "NavTo", "OrderIndex", "Comments", "Enabled", "CreatedTime", "LastOperatorID" };
            }
        }

        protected override void LoadClassFeaturs()
        {
            base.LoadClassFeaturs();
            this[_ID].CssClass = "__NumberType";
            this[_ParentID].CssClass = "__NumberType";
            this[_OrderIndex].CssClass = "__NumberType";
            this[_CreatorOperatorID].CssClass = "__NumberType";
            this[_LastOperatorID].CssClass = "__NumberType";

        }

    }

    //class definition of UserGroup
    [Serializable]
    public partial class TO_UserGroup : DBObjBase, IDBOperator
    {
        public TO_UserGroup()
        {
            Name = string.Empty;
            InheritFromGroupName = string.Empty;
        }
        public virtual string Name { get; set; }
        public virtual Int32 InheritFromGroupID { get; set; }
        public virtual string InheritFromGroupName { get; set; }
        public override string MyTableNameInDB { get { return "UserGroup"; } }
        public override Definition.EnumDefs.TOTableType TableType { get { return Definition.EnumDefs.TOTableType.Table; ; } }
        #region table name and columns
        public static readonly string _Name = "Name";
        public static readonly string _InheritFromGroupID = "InheritFromGroupID";
        public static readonly string _InheritFromGroupName = "InheritFromGroupName";
        public override string[] _MyColumnsArray
        {
            get
            {
                return new string[] { "ID", "Name", "InheritFromGroupID", "InheritFromGroupName", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID", "LastUpdateTime", "LastOperatorID" };
            }
        }

        public static readonly string _MyTableName = "UserGroup";
        #endregion
        protected override string[] _ColumnsArrayForAdd
        {
            get
            {
                return new string[] { "Name", "InheritFromGroupID", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID" };
            }
        }

        protected override string[] _ColumnsArrayForModify
        {
            get
            {
                return new string[] { "ID", "Name", "InheritFromGroupID", "Comments", "Enabled", "CreatedTime", "LastOperatorID" };
            }
        }

        public override string[] _MyForeignIDAndForeignNameColumns
        {
            get
            {
                return new string[] { "InheritFromGroupID", "InheritFromGroupName" };
            }
        }

        protected override void LoadClassFeaturs()
        {
            base.LoadClassFeaturs();
            this[_InheritFromGroupName].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;

            if (this[_InheritFromGroupID].ControlForEdited == null && ((this[_InheritFromGroupID].ColumnType & ColumnFeatureDefine.NoEditedButVisible) != ColumnFeatureDefine.NoEditedButVisible))
            {
                InputByJDataTable iptTable = new InputByJDataTable();
                string title = SSPULocalization.GetLocalization("UserGroup", "ID");
                iptTable.DialogControl.Title = title;
                ColumnSearchDefine searchCondition = this[_InheritFromGroupID].SearchDefine;
                if (searchCondition != null)
                {
                    searchCondition.DataTableName = string.IsNullOrEmpty(searchCondition.DataTableName) ? TO_UserGroup._MyTableName : searchCondition.DataTableName;
                    searchCondition.Columns = searchCondition.Columns == null ? (new string[] { TO_UserGroup._ID, TO_UserGroup._Name }) : searchCondition.Columns;
                    iptTable.JDataTableControl.DataTableName = searchCondition.DataTableName;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(searchCondition);
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                else
                {
                    iptTable.JDataTableControl.DataTableName = TO_UserGroup._MyTableName;
                    Dictionary<string, string> condition = null;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(TO_UserGroup._MyTableName, condition, new string[] { TO_UserGroup._ID, TO_UserGroup._Name });
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                this[_InheritFromGroupID].ControlForEdited = iptTable;
            }
            this[_InheritFromGroupID].ColumnType |= ColumnFeatureDefine.EditableButInvisible;
            this[_ID].CssClass = "__NumberType";
            this[_InheritFromGroupID].CssClass = "__NumberType";
            this[_CreatorOperatorID].CssClass = "__NumberType";
            this[_LastOperatorID].CssClass = "__NumberType";

        }

    }

    //class definition of UserDef
    [Serializable]
    public partial class TO_UserDef : DBObjBase, IDBOperator
    {
        public TO_UserDef()
        {
            Name = string.Empty;
            Gender = string.Empty;
            AccountID = string.Empty;
            Password = string.Empty;
        }
        public virtual string Name { get; set; }
        public virtual string Gender { get; set; }
        public virtual string AccountID { get; set; }
        public virtual string Password { get; set; }
        public override string MyTableNameInDB { get { return "UserDef"; } }
        public override Definition.EnumDefs.TOTableType TableType { get { return Definition.EnumDefs.TOTableType.Table; ; } }
        #region table name and columns
        public static readonly string _Name = "Name";
        public static readonly string _Gender = "Gender";
        public static readonly string _AccountID = "AccountID";
        public static readonly string _Password = "Password";
        public override string[] _MyColumnsArray
        {
            get
            {
                return new string[] { "ID", "Name", "Gender", "AccountID", "Password", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID", "LastUpdateTime", "LastOperatorID" };
            }
        }

        public static readonly string _MyTableName = "UserDef";
        #endregion
        protected override string[] _ColumnsArrayForAdd
        {
            get
            {
                return new string[] { "Name", "Gender", "AccountID", "Password", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID" };
            }
        }

        protected override string[] _ColumnsArrayForModify
        {
            get
            {
                return new string[] { "ID", "Name", "Gender", "AccountID", "Password", "Comments", "Enabled", "CreatedTime", "LastOperatorID" };
            }
        }

        protected override void LoadClassFeaturs()
        {
            base.LoadClassFeaturs();
            this[_ID].CssClass = "__NumberType";
            this[_CreatorOperatorID].CssClass = "__NumberType";
            this[_LastOperatorID].CssClass = "__NumberType";

        }

    }

    //class definition of TableUIDefine
    [Serializable]
    public partial class TO_TableUIDefine : DBObjBase, IDBOperator
    {
        public TO_TableUIDefine()
        {
            TableName = string.Empty;
            ColumnName = string.Empty;
            UIDefine = string.Empty;
        }
        public virtual string TableName { get; set; }
        public virtual string ColumnName { get; set; }
        public virtual string UIDefine { get; set; }
        public override string MyTableNameInDB { get { return "TableUIDefine"; } }
        public override Definition.EnumDefs.TOTableType TableType { get { return Definition.EnumDefs.TOTableType.Table; ; } }
        #region table name and columns
        public static readonly string _TableName = "TableName";
        public static readonly string _ColumnName = "ColumnName";
        public static readonly string _UIDefine = "UIDefine";
        public override string[] _MyColumnsArray
        {
            get
            {
                return new string[] { "ID", "TableName", "ColumnName", "UIDefine", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID", "LastUpdateTime", "LastOperatorID" };
            }
        }

        public static readonly string _MyTableName = "TableUIDefine";
        #endregion
        protected override string[] _ColumnsArrayForAdd
        {
            get
            {
                return new string[] { "TableName", "ColumnName", "UIDefine", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID" };
            }
        }

        protected override string[] _ColumnsArrayForModify
        {
            get
            {
                return new string[] { "ID", "TableName", "ColumnName", "UIDefine", "Comments", "Enabled", "CreatedTime", "LastOperatorID" };
            }
        }

        protected override void LoadClassFeaturs()
        {
            base.LoadClassFeaturs();
            this[_ID].CssClass = "__NumberType";
            this[_CreatorOperatorID].CssClass = "__NumberType";
            this[_LastOperatorID].CssClass = "__NumberType";

        }

    }

    //class definition of TableColumnAuthorityByUserGroup
    [Serializable]
    public partial class TO_TableColumnAuthorityByUserGroup : DBObjBase, IDBOperator
    {
        public TO_TableColumnAuthorityByUserGroup()
        {
            GlobalNavTreeName = string.Empty;
            UserGroupName = string.Empty;
            TableName = string.Empty;
            TableFieldName = string.Empty;
            FieldEditType = string.Empty;
        }
        public virtual Int32 GlobalNavTreeID { get; set; }
        public virtual string GlobalNavTreeName { get; set; }
        public virtual Int32 UserGroupID { get; set; }
        public virtual string UserGroupName { get; set; }
        public virtual string TableName { get; set; }
        public virtual string TableFieldName { get; set; }
        public virtual string FieldEditType { get; set; }
        public override string MyTableNameInDB { get { return "TableColumnAuthorityByUserGroup"; } }
        public override Definition.EnumDefs.TOTableType TableType { get { return Definition.EnumDefs.TOTableType.Table; ; } }
        #region table name and columns
        public static readonly string _GlobalNavTreeID = "GlobalNavTreeID";
        public static readonly string _GlobalNavTreeName = "GlobalNavTreeName";
        public static readonly string _UserGroupID = "UserGroupID";
        public static readonly string _UserGroupName = "UserGroupName";
        public static readonly string _TableName = "TableName";
        public static readonly string _TableFieldName = "TableFieldName";
        public static readonly string _FieldEditType = "FieldEditType";
        public override string[] _MyColumnsArray
        {
            get
            {
                return new string[] { "ID", "GlobalNavTreeID", "GlobalNavTreeName", "UserGroupID", "UserGroupName", "TableName", "TableFieldName", "FieldEditType", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID", "LastUpdateTime", "LastOperatorID" };
            }
        }

        public static readonly string _MyTableName = "TableColumnAuthorityByUserGroup";
        #endregion
        protected override string[] _ColumnsArrayForModify
        {
            get
            {
                return new string[] { "ID", "GlobalNavTreeID", "UserGroupID", "TableName", "TableFieldName", "FieldEditType", "Comments", "Enabled", "CreatedTime", "LastOperatorID" };
            }
        }

        protected override string[] _ColumnsArrayForAdd
        {
            get
            {
                return new string[] { "GlobalNavTreeID", "UserGroupID", "TableName", "TableFieldName", "FieldEditType", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID" };
            }
        }

        public override string[] _MyForeignIDAndForeignNameColumns
        {
            get
            {
                return new string[] { "UserGroupID", "UserGroupName", "GlobalNavTreeID", "GlobalNavTreeName" };
            }
        }

        protected override void LoadClassFeaturs()
        {
            base.LoadClassFeaturs();
            this[_UserGroupName].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;
            this[_GlobalNavTreeName].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;

            if (this[_UserGroupID].ControlForEdited == null && ((this[_UserGroupID].ColumnType & ColumnFeatureDefine.NoEditedButVisible) != ColumnFeatureDefine.NoEditedButVisible))
            {
                InputByJDataTable iptTable = new InputByJDataTable();
                string title = SSPULocalization.GetLocalization("UserGroup", "ID");
                iptTable.DialogControl.Title = title;
                ColumnSearchDefine searchCondition = this[_UserGroupID].SearchDefine;
                if (searchCondition != null)
                {
                    searchCondition.DataTableName = string.IsNullOrEmpty(searchCondition.DataTableName) ? TO_UserGroup._MyTableName : searchCondition.DataTableName;
                    searchCondition.Columns = searchCondition.Columns == null ? (new string[] { TO_UserGroup._ID, TO_UserGroup._Name }) : searchCondition.Columns;
                    iptTable.JDataTableControl.DataTableName = searchCondition.DataTableName;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(searchCondition);
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                else
                {
                    iptTable.JDataTableControl.DataTableName = TO_UserGroup._MyTableName;
                    Dictionary<string, string> condition = null;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(TO_UserGroup._MyTableName, condition, new string[] { TO_UserGroup._ID, TO_UserGroup._Name });
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                this[_UserGroupID].ControlForEdited = iptTable;
            }
            this[_UserGroupID].ColumnType |= ColumnFeatureDefine.EditableButInvisible;

            if (this[_GlobalNavTreeID].ControlForEdited == null && ((this[_GlobalNavTreeID].ColumnType & ColumnFeatureDefine.NoEditedButVisible) != ColumnFeatureDefine.NoEditedButVisible))
            {
                InputByJDataTable iptTable = new InputByJDataTable();
                string title = SSPULocalization.GetLocalization("GlobalNavTree", "ID");
                iptTable.DialogControl.Title = title;
                ColumnSearchDefine searchCondition = this[_GlobalNavTreeID].SearchDefine;
                if (searchCondition != null)
                {
                    searchCondition.DataTableName = string.IsNullOrEmpty(searchCondition.DataTableName) ? TO_GlobalNavTree._MyTableName : searchCondition.DataTableName;
                    searchCondition.Columns = searchCondition.Columns == null ? (new string[] { TO_GlobalNavTree._ID, TO_GlobalNavTree._Name }) : searchCondition.Columns;
                    iptTable.JDataTableControl.DataTableName = searchCondition.DataTableName;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(searchCondition);
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                else
                {
                    iptTable.JDataTableControl.DataTableName = TO_GlobalNavTree._MyTableName;
                    Dictionary<string, string> condition = null;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(TO_GlobalNavTree._MyTableName, condition, new string[] { TO_GlobalNavTree._ID, TO_GlobalNavTree._Name });
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                this[_GlobalNavTreeID].ControlForEdited = iptTable;
            }
            this[_GlobalNavTreeID].ColumnType |= ColumnFeatureDefine.EditableButInvisible;
            this[_ID].CssClass = "__NumberType";
            this[_GlobalNavTreeID].CssClass = "__NumberType";
            this[_UserGroupID].CssClass = "__NumberType";
            this[_CreatorOperatorID].CssClass = "__NumberType";
            this[_LastOperatorID].CssClass = "__NumberType";

        }

    }

    //class definition of TableAuthorityInfoByUserGroup
    [Serializable]
    public partial class TO_TableAuthorityInfoByUserGroup : DBObjBase, IDBOperator
    {
        public TO_TableAuthorityInfoByUserGroup()
        {
            TableName = string.Empty;
            GlobalNavTreeName = string.Empty;
            UserGroupName = string.Empty;
            EditType = string.Empty;
            NavURL = string.Empty;
            DefDataFilterStr = string.Empty;
        }
        public virtual string TableName { get; set; }
        public virtual Int32 GlobalNavTreeID { get; set; }
        public virtual string GlobalNavTreeName { get; set; }
        public virtual Int32 UserGroupID { get; set; }
        public virtual string UserGroupName { get; set; }
        public virtual string EditType { get; set; }
        public virtual string NavURL { get; set; }
        public virtual string DefDataFilterStr { get; set; }
        public override string MyTableNameInDB { get { return "TableAuthorityInfoByUserGroup"; } }
        public override Definition.EnumDefs.TOTableType TableType { get { return Definition.EnumDefs.TOTableType.Table; ; } }
        #region table name and columns
        public static readonly string _TableName = "TableName";
        public static readonly string _GlobalNavTreeID = "GlobalNavTreeID";
        public static readonly string _GlobalNavTreeName = "GlobalNavTreeName";
        public static readonly string _UserGroupID = "UserGroupID";
        public static readonly string _UserGroupName = "UserGroupName";
        public static readonly string _EditType = "EditType";
        public static readonly string _NavURL = "NavURL";
        public static readonly string _DefDataFilterStr = "DefDataFilterStr";
        public override string[] _MyColumnsArray
        {
            get
            {
                return new string[] { "ID", "TableName", "GlobalNavTreeID", "GlobalNavTreeName", "UserGroupID", "UserGroupName", "EditType", "NavURL", "DefDataFilterStr", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID", "LastUpdateTime", "LastOperatorID" };
            }
        }

        public static readonly string _MyTableName = "TableAuthorityInfoByUserGroup";
        #endregion
        protected override string[] _ColumnsArrayForModify
        {
            get
            {
                return new string[] { "ID", "TableName", "GlobalNavTreeID", "UserGroupID", "EditType", "NavURL", "DefDataFilterStr", "Comments", "Enabled", "CreatedTime", "LastOperatorID" };
            }
        }

        protected override string[] _ColumnsArrayForAdd
        {
            get
            {
                return new string[] { "TableName", "GlobalNavTreeID", "UserGroupID", "EditType", "NavURL", "DefDataFilterStr", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID" };
            }
        }

        public override string[] _MyForeignIDAndForeignNameColumns
        {
            get
            {
                return new string[] { "UserGroupID", "UserGroupName", "GlobalNavTreeID", "GlobalNavTreeName" };
            }
        }

        protected override void LoadClassFeaturs()
        {
            base.LoadClassFeaturs();
            this[_UserGroupName].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;
            this[_GlobalNavTreeName].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;

            if (this[_UserGroupID].ControlForEdited == null && ((this[_UserGroupID].ColumnType & ColumnFeatureDefine.NoEditedButVisible) != ColumnFeatureDefine.NoEditedButVisible))
            {
                InputByJDataTable iptTable = new InputByJDataTable();
                string title = SSPULocalization.GetLocalization("UserGroup", "ID");
                iptTable.DialogControl.Title = title;
                ColumnSearchDefine searchCondition = this[_UserGroupID].SearchDefine;
                if (searchCondition != null)
                {
                    searchCondition.DataTableName = string.IsNullOrEmpty(searchCondition.DataTableName) ? TO_UserGroup._MyTableName : searchCondition.DataTableName;
                    searchCondition.Columns = searchCondition.Columns == null ? (new string[] { TO_UserGroup._ID, TO_UserGroup._Name }) : searchCondition.Columns;
                    iptTable.JDataTableControl.DataTableName = searchCondition.DataTableName;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(searchCondition);
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                else
                {
                    iptTable.JDataTableControl.DataTableName = TO_UserGroup._MyTableName;
                    Dictionary<string, string> condition = null;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(TO_UserGroup._MyTableName, condition, new string[] { TO_UserGroup._ID, TO_UserGroup._Name });
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                this[_UserGroupID].ControlForEdited = iptTable;
            }
            this[_UserGroupID].ColumnType |= ColumnFeatureDefine.EditableButInvisible;

            if (this[_GlobalNavTreeID].ControlForEdited == null && ((this[_GlobalNavTreeID].ColumnType & ColumnFeatureDefine.NoEditedButVisible) != ColumnFeatureDefine.NoEditedButVisible))
            {
                InputByJDataTable iptTable = new InputByJDataTable();
                string title = SSPULocalization.GetLocalization("GlobalNavTree", "ID");
                iptTable.DialogControl.Title = title;
                ColumnSearchDefine searchCondition = this[_GlobalNavTreeID].SearchDefine;
                if (searchCondition != null)
                {
                    searchCondition.DataTableName = string.IsNullOrEmpty(searchCondition.DataTableName) ? TO_GlobalNavTree._MyTableName : searchCondition.DataTableName;
                    searchCondition.Columns = searchCondition.Columns == null ? (new string[] { TO_GlobalNavTree._ID, TO_GlobalNavTree._Name }) : searchCondition.Columns;
                    iptTable.JDataTableControl.DataTableName = searchCondition.DataTableName;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(searchCondition);
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                else
                {
                    iptTable.JDataTableControl.DataTableName = TO_GlobalNavTree._MyTableName;
                    Dictionary<string, string> condition = null;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(TO_GlobalNavTree._MyTableName, condition, new string[] { TO_GlobalNavTree._ID, TO_GlobalNavTree._Name });
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                this[_GlobalNavTreeID].ControlForEdited = iptTable;
            }
            this[_GlobalNavTreeID].ColumnType |= ColumnFeatureDefine.EditableButInvisible;
            this[_ID].CssClass = "__NumberType";
            this[_GlobalNavTreeID].CssClass = "__NumberType";
            this[_UserGroupID].CssClass = "__NumberType";
            this[_CreatorOperatorID].CssClass = "__NumberType";
            this[_LastOperatorID].CssClass = "__NumberType";

        }

    }

    //class definition of DocumentPageAuthorityInfoByUserGroup
    [Serializable]
    public partial class TO_DocumentPageAuthorityInfoByUserGroup : DBObjBase, IDBOperator
    {
        public TO_DocumentPageAuthorityInfoByUserGroup()
        {
            GlobalNavTreeName = string.Empty;
            UserGroupName = string.Empty;
            FileTitle = string.Empty;
            DocPath = string.Empty;
            DocPathForShow = string.Empty;
        }
        public virtual Int32 GlobalNavTreeID { get; set; }
        public virtual string GlobalNavTreeName { get; set; }
        public virtual Int32 UserGroupID { get; set; }
        public virtual string UserGroupName { get; set; }
        public virtual string FileTitle { get; set; }
        public virtual string DocPath { get; set; }
        public virtual string DocPathForShow { get; set; }
        public override string MyTableNameInDB { get { return "DocumentPageAuthorityInfoByUserGroup"; } }
        public override Definition.EnumDefs.TOTableType TableType { get { return Definition.EnumDefs.TOTableType.Table; ; } }
        #region table name and columns
        public static readonly string _GlobalNavTreeID = "GlobalNavTreeID";
        public static readonly string _GlobalNavTreeName = "GlobalNavTreeName";
        public static readonly string _UserGroupID = "UserGroupID";
        public static readonly string _UserGroupName = "UserGroupName";
        public static readonly string _FileTitle = "FileTitle";
        public static readonly string _DocPath = "DocPath";
        public static readonly string _DocPathForShow = "DocPathForShow";
        public override string[] _MyColumnsArray
        {
            get
            {
                return new string[] { "ID", "GlobalNavTreeID", "GlobalNavTreeName", "UserGroupID", "UserGroupName", "FileTitle", "DocPath", "DocPathForShow", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID", "LastUpdateTime", "LastOperatorID" };
            }
        }

        public static readonly string _MyTableName = "DocumentPageAuthorityInfoByUserGroup";
        #endregion
        protected override string[] _ColumnsArrayForModify
        {
            get
            {
                return new string[] { "ID", "GlobalNavTreeID", "UserGroupID", "FileTitle", "DocPath", "DocPathForShow", "Comments", "Enabled", "CreatedTime", "LastOperatorID" };
            }
        }

        protected override string[] _ColumnsArrayForAdd
        {
            get
            {
                return new string[] { "GlobalNavTreeID", "UserGroupID", "FileTitle", "DocPath", "DocPathForShow", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID" };
            }
        }

        public override string[] _MyForeignIDAndForeignNameColumns
        {
            get
            {
                return new string[] { "UserGroupID", "UserGroupName", "GlobalNavTreeID", "GlobalNavTreeName" };
            }
        }

        protected override void LoadClassFeaturs()
        {
            base.LoadClassFeaturs();
            this[_UserGroupName].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;
            this[_GlobalNavTreeName].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;

            if (this[_UserGroupID].ControlForEdited == null && ((this[_UserGroupID].ColumnType & ColumnFeatureDefine.NoEditedButVisible) != ColumnFeatureDefine.NoEditedButVisible))
            {
                InputByJDataTable iptTable = new InputByJDataTable();
                string title = SSPULocalization.GetLocalization("UserGroup", "ID");
                iptTable.DialogControl.Title = title;
                ColumnSearchDefine searchCondition = this[_UserGroupID].SearchDefine;
                if (searchCondition != null)
                {
                    searchCondition.DataTableName = string.IsNullOrEmpty(searchCondition.DataTableName) ? TO_UserGroup._MyTableName : searchCondition.DataTableName;
                    searchCondition.Columns = searchCondition.Columns == null ? (new string[] { TO_UserGroup._ID, TO_UserGroup._Name }) : searchCondition.Columns;
                    iptTable.JDataTableControl.DataTableName = searchCondition.DataTableName;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(searchCondition);
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                else
                {
                    iptTable.JDataTableControl.DataTableName = TO_UserGroup._MyTableName;
                    Dictionary<string, string> condition = null;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(TO_UserGroup._MyTableName, condition, new string[] { TO_UserGroup._ID, TO_UserGroup._Name });
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                this[_UserGroupID].ControlForEdited = iptTable;
            }
            this[_UserGroupID].ColumnType |= ColumnFeatureDefine.EditableButInvisible;

            if (this[_GlobalNavTreeID].ControlForEdited == null && ((this[_GlobalNavTreeID].ColumnType & ColumnFeatureDefine.NoEditedButVisible) != ColumnFeatureDefine.NoEditedButVisible))
            {
                InputByJDataTable iptTable = new InputByJDataTable();
                string title = SSPULocalization.GetLocalization("GlobalNavTree", "ID");
                iptTable.DialogControl.Title = title;
                ColumnSearchDefine searchCondition = this[_GlobalNavTreeID].SearchDefine;
                if (searchCondition != null)
                {
                    searchCondition.DataTableName = string.IsNullOrEmpty(searchCondition.DataTableName) ? TO_GlobalNavTree._MyTableName : searchCondition.DataTableName;
                    searchCondition.Columns = searchCondition.Columns == null ? (new string[] { TO_GlobalNavTree._ID, TO_GlobalNavTree._Name }) : searchCondition.Columns;
                    iptTable.JDataTableControl.DataTableName = searchCondition.DataTableName;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(searchCondition);
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                else
                {
                    iptTable.JDataTableControl.DataTableName = TO_GlobalNavTree._MyTableName;
                    Dictionary<string, string> condition = null;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(TO_GlobalNavTree._MyTableName, condition, new string[] { TO_GlobalNavTree._ID, TO_GlobalNavTree._Name });
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                this[_GlobalNavTreeID].ControlForEdited = iptTable;
            }
            this[_GlobalNavTreeID].ColumnType |= ColumnFeatureDefine.EditableButInvisible;
            this[_DocPathForShow].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;
            this[_ID].CssClass = "__NumberType";
            this[_GlobalNavTreeID].CssClass = "__NumberType";
            this[_UserGroupID].CssClass = "__NumberType";
            this[_CreatorOperatorID].CssClass = "__NumberType";
            this[_LastOperatorID].CssClass = "__NumberType";

        }

    }

    //class definition of DesingedPageAuthorityInfoByUserGroup
    [Serializable]
    public partial class TO_DesingedPageAuthorityInfoByUserGroup : DBObjBase, IDBOperator
    {
        public TO_DesingedPageAuthorityInfoByUserGroup()
        {
            GlobalNavTreeName = string.Empty;
            UserGroupName = string.Empty;
            PagePath = string.Empty;
            EditType = string.Empty;
        }
        public virtual Int32 GlobalNavTreeID { get; set; }
        public virtual string GlobalNavTreeName { get; set; }
        public virtual Int32 UserGroupID { get; set; }
        public virtual string UserGroupName { get; set; }
        public virtual string PagePath { get; set; }
        public virtual Int32 DisplayOrder { get; set; }
        public virtual string EditType { get; set; }
        public override string MyTableNameInDB { get { return "DesingedPageAuthorityInfoByUserGroup"; } }
        public override Definition.EnumDefs.TOTableType TableType { get { return Definition.EnumDefs.TOTableType.Table; ; } }
        #region table name and columns
        public static readonly string _GlobalNavTreeID = "GlobalNavTreeID";
        public static readonly string _GlobalNavTreeName = "GlobalNavTreeName";
        public static readonly string _UserGroupID = "UserGroupID";
        public static readonly string _UserGroupName = "UserGroupName";
        public static readonly string _PagePath = "PagePath";
        public static readonly string _DisplayOrder = "DisplayOrder";
        public static readonly string _EditType = "EditType";
        public override string[] _MyColumnsArray
        {
            get
            {
                return new string[] { "ID", "GlobalNavTreeID", "GlobalNavTreeName", "UserGroupID", "UserGroupName", "PagePath", "DisplayOrder", "EditType", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID", "LastUpdateTime", "LastOperatorID" };
            }
        }

        public static readonly string _MyTableName = "DesingedPageAuthorityInfoByUserGroup";
        #endregion
        protected override string[] _ColumnsArrayForModify
        {
            get
            {
                return new string[] { "ID", "GlobalNavTreeID", "UserGroupID", "PagePath", "DisplayOrder", "EditType", "Comments", "Enabled", "CreatedTime", "LastOperatorID" };
            }
        }

        protected override string[] _ColumnsArrayForAdd
        {
            get
            {
                return new string[] { "GlobalNavTreeID", "UserGroupID", "PagePath", "DisplayOrder", "EditType", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID" };
            }
        }

        public override string[] _MyForeignIDAndForeignNameColumns
        {
            get
            {
                return new string[] { "UserGroupID", "UserGroupName", "GlobalNavTreeID", "GlobalNavTreeName" };
            }
        }

        protected override void LoadClassFeaturs()
        {
            base.LoadClassFeaturs();
            this[_UserGroupName].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;
            this[_GlobalNavTreeName].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;

            if (this[_UserGroupID].ControlForEdited == null && ((this[_UserGroupID].ColumnType & ColumnFeatureDefine.NoEditedButVisible) != ColumnFeatureDefine.NoEditedButVisible))
            {
                InputByJDataTable iptTable = new InputByJDataTable();
                string title = SSPULocalization.GetLocalization("UserGroup", "ID");
                iptTable.DialogControl.Title = title;
                ColumnSearchDefine searchCondition = this[_UserGroupID].SearchDefine;
                if (searchCondition != null)
                {
                    searchCondition.DataTableName = string.IsNullOrEmpty(searchCondition.DataTableName) ? TO_UserGroup._MyTableName : searchCondition.DataTableName;
                    searchCondition.Columns = searchCondition.Columns == null ? (new string[] { TO_UserGroup._ID, TO_UserGroup._Name }) : searchCondition.Columns;
                    iptTable.JDataTableControl.DataTableName = searchCondition.DataTableName;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(searchCondition);
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                else
                {
                    iptTable.JDataTableControl.DataTableName = TO_UserGroup._MyTableName;
                    Dictionary<string, string> condition = null;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(TO_UserGroup._MyTableName, condition, new string[] { TO_UserGroup._ID, TO_UserGroup._Name });
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                this[_UserGroupID].ControlForEdited = iptTable;
            }
            this[_UserGroupID].ColumnType |= ColumnFeatureDefine.EditableButInvisible;

            if (this[_GlobalNavTreeID].ControlForEdited == null && ((this[_GlobalNavTreeID].ColumnType & ColumnFeatureDefine.NoEditedButVisible) != ColumnFeatureDefine.NoEditedButVisible))
            {
                InputByJDataTable iptTable = new InputByJDataTable();
                string title = SSPULocalization.GetLocalization("GlobalNavTree", "ID");
                iptTable.DialogControl.Title = title;
                ColumnSearchDefine searchCondition = this[_GlobalNavTreeID].SearchDefine;
                if (searchCondition != null)
                {
                    searchCondition.DataTableName = string.IsNullOrEmpty(searchCondition.DataTableName) ? TO_GlobalNavTree._MyTableName : searchCondition.DataTableName;
                    searchCondition.Columns = searchCondition.Columns == null ? (new string[] { TO_GlobalNavTree._ID, TO_GlobalNavTree._Name }) : searchCondition.Columns;
                    iptTable.JDataTableControl.DataTableName = searchCondition.DataTableName;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(searchCondition);
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                else
                {
                    iptTable.JDataTableControl.DataTableName = TO_GlobalNavTree._MyTableName;
                    Dictionary<string, string> condition = null;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(TO_GlobalNavTree._MyTableName, condition, new string[] { TO_GlobalNavTree._ID, TO_GlobalNavTree._Name });
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                this[_GlobalNavTreeID].ControlForEdited = iptTable;
            }
            this[_GlobalNavTreeID].ColumnType |= ColumnFeatureDefine.EditableButInvisible;
            this[_ID].CssClass = "__NumberType";
            this[_GlobalNavTreeID].CssClass = "__NumberType";
            this[_UserGroupID].CssClass = "__NumberType";
            this[_DisplayOrder].CssClass = "__NumberType";
            this[_CreatorOperatorID].CssClass = "__NumberType";
            this[_LastOperatorID].CssClass = "__NumberType";

        }

    }

    //class definition of HTMLPageContent
    [Serializable]
    public partial class TO_HTMLPageContent : DBObjBase, IDBOperator
    {
        public TO_HTMLPageContent()
        {
            GlobalNavTreeName = string.Empty;
            Title = string.Empty;
            HTMLContent = string.Empty;
            OtherSettings = string.Empty;
        }
        public virtual Int32 GlobalNavTreeID { get; set; }
        public virtual string GlobalNavTreeName { get; set; }
        public virtual string Title { get; set; }
        public virtual string HTMLContent { get; set; }
        public virtual string OtherSettings { get; set; }
        public override string MyTableNameInDB { get { return "HTMLPageContent"; } }
        public override Definition.EnumDefs.TOTableType TableType { get { return Definition.EnumDefs.TOTableType.Table; ; } }
        #region table name and columns
        public static readonly string _GlobalNavTreeID = "GlobalNavTreeID";
        public static readonly string _GlobalNavTreeName = "GlobalNavTreeName";
        public static readonly string _Title = "Title";
        public static readonly string _HTMLContent = "HTMLContent";
        public static readonly string _OtherSettings = "OtherSettings";
        public override string[] _MyColumnsArray
        {
            get
            {
                return new string[] { "ID", "GlobalNavTreeID", "GlobalNavTreeName", "Title", "HTMLContent", "OtherSettings", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID", "LastUpdateTime", "LastOperatorID" };
            }
        }

        public static readonly string _MyTableName = "HTMLPageContent";
        #endregion
        protected override string[] _ColumnsArrayForModify
        {
            get
            {
                return new string[] { "ID", "GlobalNavTreeID", "Title", "HTMLContent", "OtherSettings", "Comments", "Enabled", "CreatedTime", "LastOperatorID" };
            }
        }

        protected override string[] _ColumnsArrayForAdd
        {
            get
            {
                return new string[] { "GlobalNavTreeID", "Title", "HTMLContent", "OtherSettings", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID" };
            }
        }

        public override string[] _MyForeignIDAndForeignNameColumns
        {
            get
            {
                return new string[] { "GlobalNavTreeID", "GlobalNavTreeName" };
            }
        }

        protected override void LoadClassFeaturs()
        {
            base.LoadClassFeaturs();
            this[_GlobalNavTreeName].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;

            if (this[_GlobalNavTreeID].ControlForEdited == null && ((this[_GlobalNavTreeID].ColumnType & ColumnFeatureDefine.NoEditedButVisible) != ColumnFeatureDefine.NoEditedButVisible))
            {
                InputByJDataTable iptTable = new InputByJDataTable();
                string title = SSPULocalization.GetLocalization("GlobalNavTree", "ID");
                iptTable.DialogControl.Title = title;
                ColumnSearchDefine searchCondition = this[_GlobalNavTreeID].SearchDefine;
                if (searchCondition != null)
                {
                    searchCondition.DataTableName = string.IsNullOrEmpty(searchCondition.DataTableName) ? TO_GlobalNavTree._MyTableName : searchCondition.DataTableName;
                    searchCondition.Columns = searchCondition.Columns == null ? (new string[] { TO_GlobalNavTree._ID, TO_GlobalNavTree._Name }) : searchCondition.Columns;
                    iptTable.JDataTableControl.DataTableName = searchCondition.DataTableName;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(searchCondition);
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                else
                {
                    iptTable.JDataTableControl.DataTableName = TO_GlobalNavTree._MyTableName;
                    Dictionary<string, string> condition = null;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(TO_GlobalNavTree._MyTableName, condition, new string[] { TO_GlobalNavTree._ID, TO_GlobalNavTree._Name });
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                this[_GlobalNavTreeID].ControlForEdited = iptTable;
            }
            this[_GlobalNavTreeID].ColumnType |= ColumnFeatureDefine.EditableButInvisible;
            this[_ID].CssClass = "__NumberType";
            this[_GlobalNavTreeID].CssClass = "__NumberType";
            this[_CreatorOperatorID].CssClass = "__NumberType";
            this[_LastOperatorID].CssClass = "__NumberType";

        }

    }

    //class definition of HTMLPageAuthorityInfoByUserGroup
    [Serializable]
    public partial class TO_HTMLPageAuthorityInfoByUserGroup : DBObjBase, IDBOperator
    {
        public TO_HTMLPageAuthorityInfoByUserGroup()
        {
            GlobalNavTreeName = string.Empty;
            UserGroupName = string.Empty;
            PageType = string.Empty;
            EditType = string.Empty;
        }
        public virtual Int32 GlobalNavTreeID { get; set; }
        public virtual string GlobalNavTreeName { get; set; }
        public virtual Int32 UserGroupID { get; set; }
        public virtual string UserGroupName { get; set; }
        public virtual string PageType { get; set; }
        public virtual string EditType { get; set; }
        public override string MyTableNameInDB { get { return "HTMLPageAuthorityInfoByUserGroup"; } }
        public override Definition.EnumDefs.TOTableType TableType { get { return Definition.EnumDefs.TOTableType.Table; ; } }
        #region table name and columns
        public static readonly string _GlobalNavTreeID = "GlobalNavTreeID";
        public static readonly string _GlobalNavTreeName = "GlobalNavTreeName";
        public static readonly string _UserGroupID = "UserGroupID";
        public static readonly string _UserGroupName = "UserGroupName";
        public static readonly string _PageType = "PageType";
        public static readonly string _EditType = "EditType";
        public override string[] _MyColumnsArray
        {
            get
            {
                return new string[] { "ID", "GlobalNavTreeID", "GlobalNavTreeName", "UserGroupID", "UserGroupName", "PageType", "EditType", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID", "LastUpdateTime", "LastOperatorID" };
            }
        }

        public static readonly string _MyTableName = "HTMLPageAuthorityInfoByUserGroup";
        #endregion
        protected override string[] _ColumnsArrayForModify
        {
            get
            {
                return new string[] { "ID", "GlobalNavTreeID", "UserGroupID", "PageType", "EditType", "Comments", "Enabled", "CreatedTime", "LastOperatorID" };
            }
        }

        protected override string[] _ColumnsArrayForAdd
        {
            get
            {
                return new string[] { "GlobalNavTreeID", "UserGroupID", "PageType", "EditType", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID" };
            }
        }

        public override string[] _MyForeignIDAndForeignNameColumns
        {
            get
            {
                return new string[] { "UserGroupID", "UserGroupName", "GlobalNavTreeID", "GlobalNavTreeName" };
            }
        }

        protected override void LoadClassFeaturs()
        {
            base.LoadClassFeaturs();
            this[_UserGroupName].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;
            this[_GlobalNavTreeName].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;

            if (this[_UserGroupID].ControlForEdited == null && ((this[_UserGroupID].ColumnType & ColumnFeatureDefine.NoEditedButVisible) != ColumnFeatureDefine.NoEditedButVisible))
            {
                InputByJDataTable iptTable = new InputByJDataTable();
                string title = SSPULocalization.GetLocalization("UserGroup", "ID");
                iptTable.DialogControl.Title = title;
                ColumnSearchDefine searchCondition = this[_UserGroupID].SearchDefine;
                if (searchCondition != null)
                {
                    searchCondition.DataTableName = string.IsNullOrEmpty(searchCondition.DataTableName) ? TO_UserGroup._MyTableName : searchCondition.DataTableName;
                    searchCondition.Columns = searchCondition.Columns == null ? (new string[] { TO_UserGroup._ID, TO_UserGroup._Name }) : searchCondition.Columns;
                    iptTable.JDataTableControl.DataTableName = searchCondition.DataTableName;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(searchCondition);
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                else
                {
                    iptTable.JDataTableControl.DataTableName = TO_UserGroup._MyTableName;
                    Dictionary<string, string> condition = null;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(TO_UserGroup._MyTableName, condition, new string[] { TO_UserGroup._ID, TO_UserGroup._Name });
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                this[_UserGroupID].ControlForEdited = iptTable;
            }
            this[_UserGroupID].ColumnType |= ColumnFeatureDefine.EditableButInvisible;

            if (this[_GlobalNavTreeID].ControlForEdited == null && ((this[_GlobalNavTreeID].ColumnType & ColumnFeatureDefine.NoEditedButVisible) != ColumnFeatureDefine.NoEditedButVisible))
            {
                InputByJDataTable iptTable = new InputByJDataTable();
                string title = SSPULocalization.GetLocalization("GlobalNavTree", "ID");
                iptTable.DialogControl.Title = title;
                ColumnSearchDefine searchCondition = this[_GlobalNavTreeID].SearchDefine;
                if (searchCondition != null)
                {
                    searchCondition.DataTableName = string.IsNullOrEmpty(searchCondition.DataTableName) ? TO_GlobalNavTree._MyTableName : searchCondition.DataTableName;
                    searchCondition.Columns = searchCondition.Columns == null ? (new string[] { TO_GlobalNavTree._ID, TO_GlobalNavTree._Name }) : searchCondition.Columns;
                    iptTable.JDataTableControl.DataTableName = searchCondition.DataTableName;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(searchCondition);
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                else
                {
                    iptTable.JDataTableControl.DataTableName = TO_GlobalNavTree._MyTableName;
                    Dictionary<string, string> condition = null;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(TO_GlobalNavTree._MyTableName, condition, new string[] { TO_GlobalNavTree._ID, TO_GlobalNavTree._Name });
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                this[_GlobalNavTreeID].ControlForEdited = iptTable;
            }
            this[_GlobalNavTreeID].ColumnType |= ColumnFeatureDefine.EditableButInvisible;
            this[_ID].CssClass = "__NumberType";
            this[_GlobalNavTreeID].CssClass = "__NumberType";
            this[_UserGroupID].CssClass = "__NumberType";
            this[_CreatorOperatorID].CssClass = "__NumberType";
            this[_LastOperatorID].CssClass = "__NumberType";

        }

    }

    //class definition of VideoPageAuthorityInfoByUserGroup
    [Serializable]
    public partial class TO_VideoPageAuthorityInfoByUserGroup : DBObjBase, IDBOperator
    {
        public TO_VideoPageAuthorityInfoByUserGroup()
        {
            GlobalNavTreeName = string.Empty;
            UserGroupName = string.Empty;
            FileTitle = string.Empty;
            VideoPath = string.Empty;
            VideoPathForShow = string.Empty;
            ThumbnailsPath = string.Empty;
            ThumbnailsPathForShow = string.Empty;
            ExsitThumbnailsPath = string.Empty;
            Highlights = string.Empty;
        }
        public virtual Int32 GlobalNavTreeID { get; set; }
        public virtual string GlobalNavTreeName { get; set; }
        public virtual Int32 UserGroupID { get; set; }
        public virtual string UserGroupName { get; set; }
        public virtual string FileTitle { get; set; }
        public virtual string VideoPath { get; set; }
        public virtual string VideoPathForShow { get; set; }
        public virtual string ThumbnailsPath { get; set; }
        public virtual string ThumbnailsPathForShow { get; set; }
        public virtual string ExsitThumbnailsPath { get; set; }
        public virtual string Highlights { get; set; }
        public override string MyTableNameInDB { get { return "VideoPageAuthorityInfoByUserGroup"; } }
        public override Definition.EnumDefs.TOTableType TableType { get { return Definition.EnumDefs.TOTableType.Table; ; } }
        #region table name and columns
        public static readonly string _GlobalNavTreeID = "GlobalNavTreeID";
        public static readonly string _GlobalNavTreeName = "GlobalNavTreeName";
        public static readonly string _UserGroupID = "UserGroupID";
        public static readonly string _UserGroupName = "UserGroupName";
        public static readonly string _FileTitle = "FileTitle";
        public static readonly string _VideoPath = "VideoPath";
        public static readonly string _VideoPathForShow = "VideoPathForShow";
        public static readonly string _ThumbnailsPath = "ThumbnailsPath";
        public static readonly string _ThumbnailsPathForShow = "ThumbnailsPathForShow";
        public static readonly string _ExsitThumbnailsPath = "ExsitThumbnailsPath";
        public static readonly string _Highlights = "Highlights";
        public override string[] _MyColumnsArray
        {
            get
            {
                return new string[] { "ID", "GlobalNavTreeID", "GlobalNavTreeName", "UserGroupID", "UserGroupName", "FileTitle", "VideoPath", "VideoPathForShow", "Comments", "ThumbnailsPath", "ThumbnailsPathForShow", "ExsitThumbnailsPath", "Highlights", "Enabled", "CreatedTime", "CreatorOperatorID", "LastUpdateTime", "LastOperatorID" };
            }
        }

        public static readonly string _MyTableName = "VideoPageAuthorityInfoByUserGroup";
        #endregion
        protected override string[] _ColumnsArrayForModify
        {
            get
            {
                return new string[] { "ID", "GlobalNavTreeID", "UserGroupID", "FileTitle", "VideoPath", "VideoPathForShow", "Comments", "ThumbnailsPath", "ThumbnailsPathForShow", "ExsitThumbnailsPath", "Highlights", "Enabled", "CreatedTime", "LastOperatorID" };
            }
        }

        protected override string[] _ColumnsArrayForAdd
        {
            get
            {
                return new string[] { "GlobalNavTreeID", "UserGroupID", "FileTitle", "VideoPath", "VideoPathForShow", "Comments", "ThumbnailsPath", "ThumbnailsPathForShow", "ExsitThumbnailsPath", "Highlights", "Enabled", "CreatedTime", "CreatorOperatorID" };
            }
        }

        public override string[] _MyForeignIDAndForeignNameColumns
        {
            get
            {
                return new string[] { "UserGroupID", "UserGroupName", "GlobalNavTreeID", "GlobalNavTreeName" };
            }
        }

        protected override void LoadClassFeaturs()
        {
            base.LoadClassFeaturs();
            this[_UserGroupName].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;
            this[_GlobalNavTreeName].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;

            if (this[_UserGroupID].ControlForEdited == null && ((this[_UserGroupID].ColumnType & ColumnFeatureDefine.NoEditedButVisible) != ColumnFeatureDefine.NoEditedButVisible))
            {
                InputByJDataTable iptTable = new InputByJDataTable();
                string title = SSPULocalization.GetLocalization("UserGroup", "ID");
                iptTable.DialogControl.Title = title;
                ColumnSearchDefine searchCondition = this[_UserGroupID].SearchDefine;
                if (searchCondition != null)
                {
                    searchCondition.DataTableName = string.IsNullOrEmpty(searchCondition.DataTableName) ? TO_UserGroup._MyTableName : searchCondition.DataTableName;
                    searchCondition.Columns = searchCondition.Columns == null ? (new string[] { TO_UserGroup._ID, TO_UserGroup._Name }) : searchCondition.Columns;
                    iptTable.JDataTableControl.DataTableName = searchCondition.DataTableName;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(searchCondition);
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                else
                {
                    iptTable.JDataTableControl.DataTableName = TO_UserGroup._MyTableName;
                    Dictionary<string, string> condition = null;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(TO_UserGroup._MyTableName, condition, new string[] { TO_UserGroup._ID, TO_UserGroup._Name });
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                this[_UserGroupID].ControlForEdited = iptTable;
            }
            this[_UserGroupID].ColumnType |= ColumnFeatureDefine.EditableButInvisible;

            if (this[_GlobalNavTreeID].ControlForEdited == null && ((this[_GlobalNavTreeID].ColumnType & ColumnFeatureDefine.NoEditedButVisible) != ColumnFeatureDefine.NoEditedButVisible))
            {
                InputByJDataTable iptTable = new InputByJDataTable();
                string title = SSPULocalization.GetLocalization("GlobalNavTree", "ID");
                iptTable.DialogControl.Title = title;
                ColumnSearchDefine searchCondition = this[_GlobalNavTreeID].SearchDefine;
                if (searchCondition != null)
                {
                    searchCondition.DataTableName = string.IsNullOrEmpty(searchCondition.DataTableName) ? TO_GlobalNavTree._MyTableName : searchCondition.DataTableName;
                    searchCondition.Columns = searchCondition.Columns == null ? (new string[] { TO_GlobalNavTree._ID, TO_GlobalNavTree._Name }) : searchCondition.Columns;
                    iptTable.JDataTableControl.DataTableName = searchCondition.DataTableName;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(searchCondition);
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                else
                {
                    iptTable.JDataTableControl.DataTableName = TO_GlobalNavTree._MyTableName;
                    Dictionary<string, string> condition = null;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(TO_GlobalNavTree._MyTableName, condition, new string[] { TO_GlobalNavTree._ID, TO_GlobalNavTree._Name });
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                this[_GlobalNavTreeID].ControlForEdited = iptTable;
            }
            this[_GlobalNavTreeID].ColumnType |= ColumnFeatureDefine.EditableButInvisible;
            this[_VideoPathForShow].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;
            this[_ThumbnailsPathForShow].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;
            this[_ID].CssClass = "__NumberType";
            this[_GlobalNavTreeID].CssClass = "__NumberType";
            this[_UserGroupID].CssClass = "__NumberType";
            this[_CreatorOperatorID].CssClass = "__NumberType";
            this[_LastOperatorID].CssClass = "__NumberType";

        }

    }

    //class definition of UserWithGroup
    [Serializable]
    public partial class TO_UserWithGroup : DBObjBase, IDBOperator
    {
        public TO_UserWithGroup()
        {
            UserDefName = string.Empty;
            UserGroupName = string.Empty;
        }
        public virtual Int32 UserDefID { get; set; }
        public virtual string UserDefName { get; set; }
        public virtual Int32 UserGroupID { get; set; }
        public virtual string UserGroupName { get; set; }
        public override string MyTableNameInDB { get { return "UserWithGroup"; } }
        public override Definition.EnumDefs.TOTableType TableType { get { return Definition.EnumDefs.TOTableType.Table; ; } }
        #region table name and columns
        public static readonly string _UserDefID = "UserDefID";
        public static readonly string _UserDefName = "UserDefName";
        public static readonly string _UserGroupID = "UserGroupID";
        public static readonly string _UserGroupName = "UserGroupName";
        public override string[] _MyColumnsArray
        {
            get
            {
                return new string[] { "ID", "UserDefID", "UserDefName", "UserGroupID", "UserGroupName", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID", "LastUpdateTime", "LastOperatorID" };
            }
        }

        public static readonly string _MyTableName = "UserWithGroup";
        #endregion
        protected override string[] _ColumnsArrayForModify
        {
            get
            {
                return new string[] { "ID", "UserDefID", "UserGroupID", "Comments", "Enabled", "CreatedTime", "LastOperatorID" };
            }
        }

        protected override string[] _ColumnsArrayForAdd
        {
            get
            {
                return new string[] { "UserDefID", "UserGroupID", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID" };
            }
        }

        public override string[] _MyForeignIDAndForeignNameColumns
        {
            get
            {
                return new string[] { "UserGroupID", "UserGroupName", "UserDefID", "UserDefName" };
            }
        }

        protected override void LoadClassFeaturs()
        {
            base.LoadClassFeaturs();
            this[_UserGroupName].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;
            this[_UserDefName].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;

            if (this[_UserGroupID].ControlForEdited == null && ((this[_UserGroupID].ColumnType & ColumnFeatureDefine.NoEditedButVisible) != ColumnFeatureDefine.NoEditedButVisible))
            {
                InputByJDataTable iptTable = new InputByJDataTable();
                string title = SSPULocalization.GetLocalization("UserGroup", "ID");
                iptTable.DialogControl.Title = title;
                ColumnSearchDefine searchCondition = this[_UserGroupID].SearchDefine;
                if (searchCondition != null)
                {
                    searchCondition.DataTableName = string.IsNullOrEmpty(searchCondition.DataTableName) ? TO_UserGroup._MyTableName : searchCondition.DataTableName;
                    searchCondition.Columns = searchCondition.Columns == null ? (new string[] { TO_UserGroup._ID, TO_UserGroup._Name }) : searchCondition.Columns;
                    iptTable.JDataTableControl.DataTableName = searchCondition.DataTableName;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(searchCondition);
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                else
                {
                    iptTable.JDataTableControl.DataTableName = TO_UserGroup._MyTableName;
                    Dictionary<string, string> condition = null;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(TO_UserGroup._MyTableName, condition, new string[] { TO_UserGroup._ID, TO_UserGroup._Name });
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                this[_UserGroupID].ControlForEdited = iptTable;
            }
            this[_UserGroupID].ColumnType |= ColumnFeatureDefine.EditableButInvisible;

            if (this[_UserDefID].ControlForEdited == null && ((this[_UserDefID].ColumnType & ColumnFeatureDefine.NoEditedButVisible) != ColumnFeatureDefine.NoEditedButVisible))
            {
                InputByJDataTable iptTable = new InputByJDataTable();
                string title = SSPULocalization.GetLocalization("UserDef", "ID");
                iptTable.DialogControl.Title = title;
                ColumnSearchDefine searchCondition = this[_UserDefID].SearchDefine;
                if (searchCondition != null)
                {
                    searchCondition.DataTableName = string.IsNullOrEmpty(searchCondition.DataTableName) ? TO_UserDef._MyTableName : searchCondition.DataTableName;
                    searchCondition.Columns = searchCondition.Columns == null ? (new string[] { TO_UserDef._ID, TO_UserDef._Name }) : searchCondition.Columns;
                    iptTable.JDataTableControl.DataTableName = searchCondition.DataTableName;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(searchCondition);
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                else
                {
                    iptTable.JDataTableControl.DataTableName = TO_UserDef._MyTableName;
                    Dictionary<string, string> condition = null;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(TO_UserDef._MyTableName, condition, new string[] { TO_UserDef._ID, TO_UserDef._Name });
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                this[_UserDefID].ControlForEdited = iptTable;
            }
            this[_UserDefID].ColumnType |= ColumnFeatureDefine.EditableButInvisible;
            this[_ID].CssClass = "__NumberType";
            this[_UserDefID].CssClass = "__NumberType";
            this[_UserGroupID].CssClass = "__NumberType";
            this[_CreatorOperatorID].CssClass = "__NumberType";
            this[_LastOperatorID].CssClass = "__NumberType";

        }

    }

    //class definition of UserPrivateData
    [Serializable]
    public partial class TO_UserPrivateData : DBObjBase, IDBOperator
    {
        public TO_UserPrivateData()
        {
            UserDefName = string.Empty;
            Theme = string.Empty;
        }
        public virtual Int32 UserDefID { get; set; }
        public virtual string UserDefName { get; set; }
        public virtual string Theme { get; set; }
        public override string MyTableNameInDB { get { return "UserPrivateData"; } }
        public override Definition.EnumDefs.TOTableType TableType { get { return Definition.EnumDefs.TOTableType.Table; ; } }
        #region table name and columns
        public static readonly string _UserDefID = "UserDefID";
        public static readonly string _UserDefName = "UserDefName";
        public static readonly string _Theme = "Theme";
        public override string[] _MyColumnsArray
        {
            get
            {
                return new string[] { "ID", "UserDefID", "UserDefName", "Theme", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID", "LastUpdateTime", "LastOperatorID" };
            }
        }

        public static readonly string _MyTableName = "UserPrivateData";
        #endregion
        protected override string[] _ColumnsArrayForModify
        {
            get
            {
                return new string[] { "ID", "UserDefID", "Theme", "Comments", "Enabled", "CreatedTime", "LastOperatorID" };
            }
        }

        protected override string[] _ColumnsArrayForAdd
        {
            get
            {
                return new string[] { "UserDefID", "Theme", "Comments", "Enabled", "CreatedTime", "CreatorOperatorID" };
            }
        }

        public override string[] _MyForeignIDAndForeignNameColumns
        {
            get
            {
                return new string[] { "UserDefID", "UserDefName" };
            }
        }

        protected override void LoadClassFeaturs()
        {
            base.LoadClassFeaturs();
            this[_UserDefName].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;

            if (this[_UserDefID].ControlForEdited == null && ((this[_UserDefID].ColumnType & ColumnFeatureDefine.NoEditedButVisible) != ColumnFeatureDefine.NoEditedButVisible))
            {
                InputByJDataTable iptTable = new InputByJDataTable();
                string title = SSPULocalization.GetLocalization("UserDef", "ID");
                iptTable.DialogControl.Title = title;
                ColumnSearchDefine searchCondition = this[_UserDefID].SearchDefine;
                if (searchCondition != null)
                {
                    searchCondition.DataTableName = string.IsNullOrEmpty(searchCondition.DataTableName) ? TO_UserDef._MyTableName : searchCondition.DataTableName;
                    searchCondition.Columns = searchCondition.Columns == null ? (new string[] { TO_UserDef._ID, TO_UserDef._Name }) : searchCondition.Columns;
                    iptTable.JDataTableControl.DataTableName = searchCondition.DataTableName;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(searchCondition);
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                else
                {
                    iptTable.JDataTableControl.DataTableName = TO_UserDef._MyTableName;
                    Dictionary<string, string> condition = null;
                    iptTable.JDataTableControl.Data = CachingManager.Instance.GetDataTable(TO_UserDef._MyTableName, condition, new string[] { TO_UserDef._ID, TO_UserDef._Name });
                    iptTable.JDataTableControl.UseSimplestStyle = true;
                }
                this[_UserDefID].ControlForEdited = iptTable;
            }
            this[_UserDefID].ColumnType |= ColumnFeatureDefine.EditableButInvisible;
            this[_ID].CssClass = "__NumberType";
            this[_UserDefID].CssClass = "__NumberType";
            this[_CreatorOperatorID].CssClass = "__NumberType";
            this[_LastOperatorID].CssClass = "__NumberType";

        }

    }

    #endregion

    #region DataView object definition

    #endregion

    #region Tables and columns definition

    public partial class DBDataTables
    {
        public static string[] AllTablesName
        {
            get
            {
                return new string[]{
                                     "Table_Sample","GlobalNavTree","UserGroup","UserDef","TableUIDefine","TableColumnAuthorityByUserGroup","TableAuthorityInfoByUserGroup","DocumentPageAuthorityInfoByUserGroup","DesingedPageAuthorityInfoByUserGroup","HTMLPageContent","HTMLPageAuthorityInfoByUserGroup","VideoPageAuthorityInfoByUserGroup","UserWithGroup","UserPrivateData"
                                   };
            }
        }
        public static readonly string Table_Sample = "Table_Sample";
        public static readonly string GlobalNavTree = "GlobalNavTree";
        public static readonly string UserGroup = "UserGroup";
        public static readonly string UserDef = "UserDef";
        public static readonly string TableUIDefine = "TableUIDefine";
        public static readonly string TableColumnAuthorityByUserGroup = "TableColumnAuthorityByUserGroup";
        public static readonly string TableAuthorityInfoByUserGroup = "TableAuthorityInfoByUserGroup";
        public static readonly string DocumentPageAuthorityInfoByUserGroup = "DocumentPageAuthorityInfoByUserGroup";
        public static readonly string DesingedPageAuthorityInfoByUserGroup = "DesingedPageAuthorityInfoByUserGroup";
        public static readonly string HTMLPageContent = "HTMLPageContent";
        public static readonly string HTMLPageAuthorityInfoByUserGroup = "HTMLPageAuthorityInfoByUserGroup";
        public static readonly string VideoPageAuthorityInfoByUserGroup = "VideoPageAuthorityInfoByUserGroup";
        public static readonly string UserWithGroup = "UserWithGroup";
        public static readonly string UserPrivateData = "UserPrivateData";
    }
    #endregion

}
