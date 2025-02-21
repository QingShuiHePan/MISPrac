using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using Definition;
using SSPUCore.Configuration;
using SSPUCore.Controls;

namespace DBManager
{
     public partial class TO_Table_Sample
     {
         protected override void OnPreLoadClassFeaturs()
         {
             base.OnPreLoadClassFeaturs();

             this[_Photo].ControlForEdited = new Uploadify();
             this[_Photo].ColumnType
                 = ColumnFeatureDefine.IsImagePath | ColumnFeatureDefine.EditableButInvisible;

        }
    }

    public partial class TO_UserDef
    {
        protected override void OnPreLoadClassFeaturs()
        {
            base.OnPreLoadClassFeaturs();

            JTextBox txb = new JTextBox();
            txb.TextType = JTextBoxValidatedType.password;
            txb.Pattern = "(?=.*[0-9])(?=.*[a-zA-Z]).{6,36}";
            txb.ErrorMessage = "请输入包含字母数字的密码，最小长度6位";

            this[_Password].ControlForEdited = txb;
            this[_Password].ColumnType = ColumnFeatureDefine.EditableButInvisible;
            
            JDropDown gender = new JDropDown();
            gender.Items.Add("男");
            gender.Items.Add("女");
            this[_Gender].ControlForEdited = gender;

            this[_Enabled].ColumnType = ColumnFeatureDefine.Null;

        }
    }

    public partial class TO_GlobalNavTree
    {
        protected override void OnPreLoadClassFeaturs()
        {
            base.OnPreLoadClassFeaturs();

            JTextBox txb = new JTextBox();
            txb.Disabled = true;

            this[_ParentID].ControlForEdited = txb;
            
            JTextBox txb2 = new JTextBox();
            txb2.InputType                     = ValidateType.text;
            txb2.Pattern                       = @"[0-9]+";
            txb2.ErrorMessage                  = "请输入一个正整数值";
            this[_OrderIndex].ControlForEdited = txb2;

            JTextBox txb3 = new JTextBox();
            txb3.Required = false;
            this[_NavTo].ControlForEdited = txb3;

            //this[_ParentID].ColumnType    |= ColumnFeatureDefine.InvisibleAndNotEdited;
            //this[_Comments].ColumnType |= ColumnFeatureDefine.NoEditedButVisible;
        }
    }

    public partial class TO_HTMLPageAuthorityInfoByUserGroup
    {
        protected override void OnPreLoadClassFeaturs()
        {

            JDropDown dropDown_EditType = new JDropDown();

            foreach (var edType in Enum.GetNames(typeof(EnumDefs.HTMLPageEditType)))
            {
                dropDown_EditType.Items.Add(edType);
            }

            this[_EditType].ControlForEdited = dropDown_EditType;

            this[_GlobalNavTreeID].ColumnType = ColumnFeatureDefine.InvisibleAndNotEdited;
            this[_GlobalNavTreeName].ColumnType = ColumnFeatureDefine.InvisibleAndNotEdited;
            this[_UserGroupID].ColumnType = ColumnFeatureDefine.InvisibleAndNotEdited;
            this[_UserGroupName].ColumnType = ColumnFeatureDefine.InvisibleAndNotEdited;

            JDropDown dropDown_PageType = new JDropDown();
            dropDown_PageType.Items.Add("网页");
            dropDown_PageType.Items.Add("跳转到已有页面");
            dropDown_PageType.Items.Add("浏览PDF文件");
            this[_PageType].ControlForEdited = dropDown_PageType;

            //this[_Navigate2Page].ColumnType = ColumnFeatureDefine.EditedButNotRequried;

            Uploadify upfile = new Uploadify();
            //this[_PDFFile].ControlForEdited = upfile;

            base.OnPreLoadClassFeaturs();
        }
    }

    public partial class TO_TableColumnAuthorityByUserGroup
    {
        protected override void OnPreLoadClassFeaturs()
        {
            this[_GlobalNavTreeID].ColumnType   |= ColumnFeatureDefine.InvisibleAndNotEdited;
            this[_GlobalNavTreeName].ColumnType |= ColumnFeatureDefine.InvisibleAndNotEdited;
            this[_UserGroupID].ColumnType       |= ColumnFeatureDefine.InvisibleAndNotEdited;
            this[_UserGroupName].ColumnType     |= ColumnFeatureDefine.InvisibleAndNotEdited;
            this[_TableName].ColumnType         |= ColumnFeatureDefine.InvisibleAndNotEdited;

            #region Add Dropdown control

            JDropDown dropDown_Fields = new JDropDown();
            string selectedTableName = AuthorityManager.Instance.CurrentTableName;

            if (!string.IsNullOrEmpty(selectedTableName))
            {
                DBObjBase obj = JDataTableManager.Instance.GetToObjByTableName(selectedTableName);
                if (obj != null)
                {
                    foreach (var s in obj._ColumnsArrayForCRUD)
                    {
                        if ( s.Equals(_Comments) || s.Equals(_Enabled) || !_columnsForAllTables.Contains(s))
                        {
                            dropDown_Fields.Items.Add(s);
                        }
                    }

                    dropDown_Fields.Items.Add(_CreatedTime);
                }
            }

            this[_TableFieldName].ControlForEdited = dropDown_Fields;

            #endregion

            JDropDown dropDown_EditType = new JDropDown();

            foreach (var edType in Enum.GetNames(typeof(EnumDefs.FieldAuthority)))
            {
                dropDown_EditType.Items.Add(edType);
            }

            this[_FieldEditType].ControlForEdited = dropDown_EditType;
            

            base.OnPreLoadClassFeaturs();
        }
    }

    public partial class TO_DesingedPageAuthorityInfoByUserGroup
    {
        protected override void OnPreLoadClassFeaturs()
        {
            this[_GlobalNavTreeID].ColumnType   |= ColumnFeatureDefine.InvisibleAndNotEdited;
            this[_GlobalNavTreeName].ColumnType |= ColumnFeatureDefine.InvisibleAndNotEdited;
            this[_UserGroupID].ColumnType       |= ColumnFeatureDefine.InvisibleAndNotEdited;
            this[_UserGroupName].ColumnType     |= ColumnFeatureDefine.InvisibleAndNotEdited;
            //this[_TableName].ColumnType         |= ColumnFeatureDefine.InvisibleAndNotEdited;
            JDropDown dropDown = new JDropDown();
            foreach (var edType in Enum.GetNames(typeof(JDataTableEditType)))
            {
                if (edType != JDataTableEditType.None.ToString())
                {
                    dropDown.Items.Add(edType);
                }
            }

            this[_EditType].ControlForEdited = dropDown;

            base.OnPreLoadClassFeaturs();
        }
    }

    public partial class TO_TableAuthorityInfoByUserGroup
    {
        protected override void OnPreLoadClassFeaturs()
        {
            DBObjBase toObj = Utility.Instance.GetTO_ObjByTableName(AuthorityManager.Instance.CurrentTableName);
            if (toObj != null)
            {

                if (GlobalString.QueryNav2PageNode.Equals(toObj.MyTableNameInDB))
                {
                    this[_TableName].ColumnType = ColumnFeatureDefine.InvisibleAndNotEdited;
                    this[_EditType].ColumnType = ColumnFeatureDefine.InvisibleAndNotEdited;

                }
                else
                {
                    this[_TableName].ColumnType = ColumnFeatureDefine.NoEditedButVisible;

                    JDropDown dropDown_EditType = new JDropDown();

                    if (toObj.TableType == EnumDefs.TOTableType.Table)
                    {
                        foreach (var edType in Enum.GetNames(typeof(JDataTableEditType)))
                        {
                            dropDown_EditType.Items.Add(edType);
                        }
                    }
                    else if( toObj.TableType == EnumDefs.TOTableType.View )
                    {
                        dropDown_EditType.Items.Add(JDataTableEditType.DenyEdit.ToString());
                        //dropDown_EditType.Items.Add(JDataTableEditType.Nav2OtherPage.ToString());
                        dropDown_EditType.Items.Add(JDataTableEditType.Hidden.ToString());
                    }

                    this[_EditType].ControlForEdited = dropDown_EditType;

                    this[_NavURL].ColumnType = ColumnFeatureDefine.EditedButNotRequried;
                }

            }


            base.OnPreLoadClassFeaturs();
        }

       
    }


    public partial class TO_DocumentPageAuthorityInfoByUserGroup
    {
        protected override void OnPreLoadClassFeaturs()
        {
            this[_GlobalNavTreeID].ColumnType   |= ColumnFeatureDefine.InvisibleAndNotEdited;
            this[_GlobalNavTreeName].ColumnType |= ColumnFeatureDefine.InvisibleAndNotEdited;
            this[_UserGroupID].ColumnType       |= ColumnFeatureDefine.InvisibleAndNotEdited;
            this[_UserGroupName].ColumnType     |= ColumnFeatureDefine.InvisibleAndNotEdited;
            this[_DocPath].ColumnType           |= ColumnFeatureDefine.EditableButInvisible;
            this[_Comments].ColumnType          |= ColumnFeatureDefine.EditableButInvisible;
            this[_CreatedTime].ColumnType       |= ColumnFeatureDefine.InvisibleAndNotEdited;


            Uploadify udy = new Uploadify();
            udy.Required                    = false;
            this[_DocPath].ControlForEdited = udy;

            

            base.OnPreLoadClassFeaturs();

        }
    }

    public partial class TO_VideoPageAuthorityInfoByUserGroup
    {
        protected override void OnPreLoadClassFeaturs()
        {
            base.OnPreLoadClassFeaturs();

            this[_GlobalNavTreeID].ColumnType     |= ColumnFeatureDefine.InvisibleAndNotEdited;
            this[_GlobalNavTreeName].ColumnType   |= ColumnFeatureDefine.InvisibleAndNotEdited;
            this[_UserGroupID].ColumnType         |= ColumnFeatureDefine.InvisibleAndNotEdited;
            this[_UserGroupName].ColumnType       |= ColumnFeatureDefine.InvisibleAndNotEdited;
            this[_VideoPath].ColumnType           |= ColumnFeatureDefine.EditableButInvisible;
            this[_ThumbnailsPath].ColumnType      |= ColumnFeatureDefine.EditableButInvisible;
            this[_Comments].ColumnType            |= ColumnFeatureDefine.EditableButInvisible;
            this[_ExsitThumbnailsPath].ColumnType |= ColumnFeatureDefine.EditableButInvisible;
            this[_ExsitThumbnailsPath].ColumnType |= ColumnFeatureDefine.EditedButNotRequried;
            this[_Highlights].ColumnType          |= ColumnFeatureDefine.EditedButNotRequried;


            Uploadify udy = new Uploadify();
            udy.Required                      = false;
            this[_VideoPath].ControlForEdited = udy;
            

            Uploadify udyPic = new Uploadify();
            udyPic.Required                        = false;
            this[_ThumbnailsPath].ControlForEdited = udyPic;
        }
    }


}
