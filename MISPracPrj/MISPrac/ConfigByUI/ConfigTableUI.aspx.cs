using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBManager;
using Definition;
using SSPUCore;
using SSPUCore.Configuration;
using SSPUCore.Controls;

namespace MISPrac
{
    public partial class ConfigTableUI : System.Web.UI.Page
    {
        private string selectedTableName;
        private string selectedColumnName;
        private DBObjBase currentDBObj;
        protected void Page_Load(object sender, EventArgs e)
        {
            selectedTableName = HttpContext.Current.Request.QueryString[GlobalString.QueryTableName];
            selectedColumnName = HttpContext.Current.Request.QueryString[GlobalString.QueryTableColumn];

            TO_TableUIDefine uid = null;
            if (!string.IsNullOrEmpty(selectedTableName) )
            {
                currentDBObj = Utility.Instance.GetTO_ObjByTableName(selectedTableName);

                if (!string.IsNullOrEmpty(selectedColumnName))
                {
                    uid = GetUIDObj();

                    if (!IsPostBack)
                    {

                        TableUIBase obj = null;
                        if (uid != null)
                        {
                            obj = SSPUConverter.Instance.DeSerialize_FromString<TableUIBase>(uid.UIDefine);
                        }
                        InitializUIByUIObj(obj);

                    }

                }

            }



            string[] tables = Utility.Instance.AllUserDefinedTables;
            foreach (var s in tables)
            {
                JTreeNode nd = new JTreeNode(s, s, "/Resource/Image/Icons/TreeNode/TableF.png",
                    string.Format("?{0}={1}",GlobalString.QueryTableName,s), "");

                if (s.Equals(selectedTableName))
                {
                    nd.Selected = true;
                }

                JTreeView_TableName.Nodes.Add(nd);
            }

            if (currentDBObj != null)
            {
                
                DBObjBase obj = currentDBObj;

                List<string> excludeColumns = new List<string>();
                excludeColumns.Add(DBObjBase._ID);
                excludeColumns.Add(DBObjBase._CreatedTime);
                excludeColumns.Add(DBObjBase._CreatorOperatorID);
                excludeColumns.Add(DBObjBase._LastOperatorID);
                excludeColumns.Add(DBObjBase._LastUpdateTime);
                if (currentDBObj._MyForeignIDAndForeignNameColumns        != null &&
                    currentDBObj._MyForeignIDAndForeignNameColumns.Length >= 1)
                {
                    foreach (string s in currentDBObj._MyForeignIDAndForeignNameColumns)
                    {
                        excludeColumns.Add(s);
                    }
                }
                PropertyInfo[] ps = obj.GetType().GetProperties();

                foreach (var s in obj._MyColumnsArray)
                {
                    if (!excludeColumns.Contains(s))
                    {
                        if (s.EndsWith("ID", StringComparison.CurrentCultureIgnoreCase))
                        {
                            foreach (var s1 in obj._MyColumnsArray)
                            {
                                if (s1.Equals(s.Substring(0, s.Length - 2) + "Name",
                                    StringComparison.CurrentCultureIgnoreCase))
                                {
                                    continue;
                                }
                            }
                        }else if (s.EndsWith("Name", StringComparison.CurrentCultureIgnoreCase))
                        {
                            foreach (var s1 in obj._MyColumnsArray)
                            {
                                if (s1.Equals(s.Substring(0, s.Length - 4) + "ID",
                                    StringComparison.CurrentCultureIgnoreCase))
                                {
                                    continue;
                                }
                            }
                        }

                        RuntimeTypeHandle tph = Type.GetTypeHandle(obj);
                        bool setUI = true;
                        foreach (var p in ps)
                        {
                            if (p.Name.Equals(s))
                            {
                                if (p.PropertyType.Equals(typeof(DateTime))
                                   || p.PropertyType.Equals(typeof(bool)))
                                {
                                    setUI = false;
                                }
                                break;
                            }
                        }

                        if (setUI)
                        {
                            if (!s.Equals(selectedColumnName))
                            {
                                JDropDown_ShowFileOrLinkDataSource.Items.Add(s);
                            }

                            JTreeNode nd = new JTreeNode(s, s, "/Resource/Image/Icons/TreeNode/trigger.png",
                                string.Format("?{0}={1}&{2}={3}", GlobalString.QueryTableName, selectedTableName, GlobalString.QueryTableColumn,s)
                                , "");

                            if (s.Equals(selectedColumnName))
                            {
                                nd.Selected = true;
                            }

                            JTreeView_Columns.Nodes.Add(nd);
                        }
                        
                    }
                }

                foreach (var p in ps)
                {
                    if (p.Name.Equals(selectedColumnName))
                    {
                        if (p.PropertyType.Equals(typeof(int))
                            || p.PropertyType.Equals(typeof(double)))
                        {
                            JTabControlSelector["菜单型控件"].Visible =
                                JTabControlSelector["文件类型"].Visible =
                                    JTabControlSelector["关联文件显示或下载"].Visible =
                                        JTabControlSelector["数据树类型"].Visible =
                                            false;

                            JTabs tbTxt = GetJTablTextBoxContainer();
                            tbTxt["普通文本"].Visible =
                                tbTxt["正则表达式型"].Visible = false;



                            //JTabs_TextType["普通文本"].Visible =
                            //    JTabs_TextType["正则表达式型"].Visible = false;


                            continue;
                        }
                        break;
                    }
                }


            }
        }

        private void InitializUIByUIObj(TableUIBase obj)
        {
            Panel_ConfigNavTableContainer.Visible = true;

            JDropDown_InputByTreeSourceByTable.Items.Clear();
            JDropDown_AsociatChangeTable.Items.Clear();
            JDropDown_InnerAssociateResultWriteOut_TableName.Items.Clear();
            JDropDown_ReadAssociateOutTableName.Items.Clear();

            JDropDown_InputByTreeSourceByTable.Items.Add(DBDataTables.EmptyTableName);
            JDropDown_AsociatChangeTable.Items.Add(DBDataTables.EmptyTableName);
            JDropDown_InnerAssociateResultWriteOut_TableName.Items.Add(DBDataTables.EmptyTableName);
            string[] tables = Utility.Instance.AllUserDefinedTables;
            foreach (var s in tables)
            {
                JDropDown_InputByTreeSourceByTable.Items.Add(s);
                JDropDown_AsociatChangeTable.Items.Add(s);
                JDropDown_InnerAssociateResultWriteOut_TableName.Items.Add(s);
                JDropDown_ReadAssociateOutTableName.Items.Add(s);
            }

            JDropDown_InputByTreeSourceByTable.SelectedText = DBDataTables.EmptyTableName;

            if (currentDBObj != null)
            {
                Type ps = currentDBObj.GetType();

                foreach (var p in ps.GetProperties())
                {
                    if (p.PropertyType == typeof(int) || p.PropertyType == typeof(double) ||
                        p.PropertyType == typeof(float))
                    {
                        JDropDown_TextBoxNumTableInnerAssociateColumn1.Items.Add(p.Name);
                        JDropDown_TextBoxNumTableInnerAssociateColumn2.Items.Add(p.Name);
                        JDropDown_TextBoxNumTableInnerAssociateColumn3.Items.Add(p.Name);
                    }
                }

                JDropDown_TextBoxNumTableInnerAssociateColumn1.Items.Add(GlobalString.EmptyColumnName);
                JDropDown_TextBoxNumTableInnerAssociateColumn2.Items.Add(GlobalString.EmptyColumnName);
                JDropDown_TextBoxNumTableInnerAssociateColumn3.Items.Add(GlobalString.EmptyColumnName);

                JDropDown_TextBoxNumTableInnerAssociateCalculateType.Items.Add("乘");
                JDropDown_TextBoxNumTableInnerAssociateCalculateType.Items.Add("除");
                JDropDown_TextBoxNumTableInnerAssociateCalculateType.Items.Add("加");
                JDropDown_TextBoxNumTableInnerAssociateCalculateType.Items.Add("减");
                JDropDown_TextBoxNumTableInnerAssociateCalculateType.Items.Add(GlobalString.EmptyColumnName);

                foreach (ListItem item in JDropDown_TextBoxNumTableInnerAssociateCalculateType.Items)
                {
                    JDropDown_TextBoxNumTableInnerAssociateCalculateType2.Items.Add(item.Text);
                }

                JDropDown_TextBoxNumTableInnerAssociateColumn1.SelectedText 
                    = JDropDown_TextBoxNumTableInnerAssociateColumn2.SelectedText
                    = JDropDown_TextBoxNumTableInnerAssociateColumn3.SelectedText
                    = JDropDown_TextBoxNumTableInnerAssociateCalculateType.SelectedText
                    = JDropDown_TextBoxNumTableInnerAssociateCalculateType2.SelectedText
                    = GlobalString.EmptyColumnName;


            }

            
            if (obj != null)
            {
                if (obj is TableUITextBox)
                {
                    TableUITextBox txb = (TableUITextBox) obj;

                    JTabControlSelector["普通文本控件"].Selected = true;

                    JTabs tbTxt = GetJTablTextBoxContainer();
                    switch (txb.TextBoxType)
                    {
                        case TableUITextBoxType.Double:
                            TableUITextBoxNumber tbNum = (TableUITextBoxNumber) txb;
                            tbTxt["数字类型"].Selected = true;
                            JDropDown_TextBoxNumberType.SelectedText = "Double";
                            JTextBox_Min.Text = tbNum.Min;
                            JTextBox_Max.Text = tbNum.Max;

                            InitializeNumberTextbox(tbNum);

                                break;
                        case TableUITextBoxType.Int:
                            TableUITextBoxNumber tbNumInt = (TableUITextBoxNumber)txb;
                            tbTxt["数字类型"].Selected = true;
                            JDropDown_TextBoxNumberType.SelectedText = "Int";
                            JTextBox_Min.Text = tbNumInt.Min;
                            JTextBox_Max.Text = tbNumInt.Max;
                            InitializeNumberTextbox(tbNumInt);
                            break;
                        case TableUITextBoxType.Normal:
                            tbTxt["普通文本"].Selected = true;
                            break;
                        case TableUITextBoxType.Regex:
                            tbTxt["正则表达式型"].Selected = true;
                            JTextBox_Regex.Text = txb.RegexStr;
                            break;
                    }
                }
                else if (obj is TableUIJDropDown)
                {
                    TableUIJDropDown dd = (TableUIJDropDown) obj;

                    JTabControlSelector["菜单型控件"].Selected = true;

                    JTextBox_DropdownItems.Text = dd.ItemsStr;
                }
                else if (obj is TableUIUploadify)
                {
                    JTabControlSelector["文件类型"].Selected = true;

                    TableUIUploadify up = (TableUIUploadify) obj;

                    JTextBox_FileTypesDescription.Text = up.FileDescription;
                    JTextBox_AllowFileTypes.Text = up.FileAllowTypes;

                }
                else if (obj is TableUIFileRender)
                {
                    TableUIFileRender fr = (TableUIFileRender) obj;
                    JTabControlSelector["关联文件显示或下载"].Selected = true;
                    JDropDown_ShowFileOrLinkDataSource.SelectedText = fr.DataSourceColumnName;
                    switch (fr.RenderType)
                    {
                        case TableUIFileRenderType.Image:
                            JDropwn_ShowFileOrLink.SelectedText = "显示为图片";
                            JTextBox_ShowFileAsImageWidth.Text = fr.ImageWidth;
                            JTextBox_ShowFileAsImageHeight.Text = fr.ImageHeight;
                            break;
                        case TableUIFileRenderType.LinkURL:
                            JDropwn_ShowFileOrLink.SelectedText = "显示为下载链接";
                            JTextBox_ShowFileAsDownloadText.Text = fr.LinkText;
                            break;
                    }

                    JTextBox_ShowFileAsDownloadText.Text = fr.LinkText;
                }
                else if (obj is TableUIInputByTree)
                {
                    JTabControlSelector["数据树类型"].Selected = true;
                    TableUIInputByTree bt = (TableUIInputByTree) obj;
                    JCheckBox_DataTreeEnableMultiSelect.Checked = bt.EnableMultiSelect;
                    JTextBox_InputByTreeSourceManule.Text = bt.NodesSourceText;
                    JDropDown_InputByTreeSourceByTable.SelectedText = bt.NodesSourceTableName;
                    JTextBox_InputByTreeSourceByTableDataFilter.Text = bt.TableFilterString;
                }

                JCheckBoxRequired.Checked = obj.Required;
                JTextBox_ErrorInfo.Text = obj.ErrorInfo;
            }
        }

        private void InitializeNumberTextbox(TableUITextBoxNumber tbNum)
        {
            if (tbNum is TableUITextBoxNumberReadOut2Inner)
            {
                TableUITextBoxNumberReadOut2Inner tbIn = (TableUITextBoxNumberReadOut2Inner) tbNum;
                if (!DBDataTables.EmptyTableName.Equals(tbIn.AssociatTableName))
                {
                    
                    JDropDown_ReadAssociateOutTableName.SelectedText = tbIn.AssociatTableName;

                    InitializeReadOut2InnerTab();

                    JDropDown_ReadAssociateOutTableColumn.SelectedText = tbIn.AssociatTableColumnName;
                    JDropDown_ReadAssociateOutTableConditionOutColumn.SelectedText = tbIn.AssociatTableForeignID;
                    JDropDown_ReadAssociateOutTableConditionInnerColumn.SelectedText = tbIn.AssociateCurrentID;
                    JTextBox_ReadAssociateOutTableRate.Text = tbIn.ChangedRate;
                }

                SelectJTabTextNumItem("读取表外数据");
            }else if (tbNum is TableUITextBoxNumberWrite2Out)
            {
                TableUITextBoxNumberWrite2Out tbOut = (TableUITextBoxNumberWrite2Out) tbNum;

                if (!DBDataTables.EmptyTableName.Equals(tbOut.AssociatTableName))
                {
                    
                    JDropDown_AsociatChangeTable.SelectedText = tbOut.AssociatTableName;

                    InitilizeAssociateWriteOutControls();

                    JDropDown_AsociatChangeColumn.SelectedText = tbOut.AssociatTableColumnName;
                    JDropDown_AsociatChangeTableForeinID.SelectedText = tbOut.AssociatTableForeignID;
                    JDrown_CurrentAssociateColumn.SelectedText = tbOut.AssociateCurrentID;
                    JTextbox_AssociatParm.Text = tbOut.ChangedRate;
                }
                SelectJTabTextNumItem("表外关联更新");
            } else if ( tbNum is TableUITextBoxNumberInnerAssociate )
            {
                TableUITextBoxNumberInnerAssociate tbClm = (TableUITextBoxNumberInnerAssociate)tbNum;

                JDropDown_TextBoxNumTableInnerAssociateColumn1.SelectedText = tbClm.Column1;
                JDropDown_TextBoxNumTableInnerAssociateColumn2.SelectedText = tbClm.Column2;
                JDropDown_TextBoxNumTableInnerAssociateColumn3.SelectedText = tbClm.Column3;

                JDropDown_TextBoxNumTableInnerAssociateCalculateType.SelectedText = tbClm.CalculateType1With2;
                JDropDown_TextBoxNumTableInnerAssociateCalculateType2.SelectedText = tbClm.CalculateType12With3;


                if (tbClm.WriteOutDef != null)
                {
                    TableUITextBoxNumberWrite2Out tbOut = tbClm.WriteOutDef;

                    if (!DBDataTables.EmptyTableName.Equals(tbOut.AssociatTableName))
                    {

                        JDropDown_InnerAssociateResultWriteOut_TableName.SelectedText = tbOut.AssociatTableName;

                        InitilizeInner_AssociateWriteOutControls();

                        JDropDown_InnerAssociateResultWriteOut_ColumnName.SelectedText = tbOut.AssociatTableColumnName;
                        JDropDown_InnerAssociateResultWriteOut_ForeinConditionColum.SelectedText = tbOut.AssociatTableForeignID;
                        JDropDown_InnerAssociateResultWriteOut_CurrentConditionColum.SelectedText = tbOut.AssociateCurrentID;
                        JTextbox_InnerAssociateResultWriteOut_Rate.Text = tbOut.ChangedRate;
                    }
                }

                SelectJTabTextNumItem("表内关联更新");

            } else if (tbNum != null)
            {
                SelectJTabTextNumItem("无关联更新");
            }
        }

        private void SelectJTabTextNumItem(string selectedItem)
        {
            foreach (Control control in Panel_JTabForTextboxNum.Controls)
            {
                if (control is JTabs)
                {
                    ((JTabs) control)[selectedItem].Selected = true;
                    break;
                }
            }
        }

        private JTabs GetJTablTextBoxContainer()
        {
            foreach (Control itemContentControl in JTabControlSelector["普通文本控件"].ItemContent.Controls)
            {
                if (itemContentControl.ID != null && itemContentControl.ID.Equals("JTabs_TextType"))
                {
                   return (itemContentControl as JTabs);
                }
            }

            return null;
        }

        protected void JButtonClicked(object sender, EventArgs e)
        {
            if (sender != null)
            {
                JButton btn = (JButton) sender;

                if (!string.IsNullOrEmpty(selectedTableName) && !string.IsNullOrEmpty(selectedColumnName))
                {
                    TableUIDef uidef = new TableUIDef();
                    TableUIBase controlInfo = null;

                    if (btn.Equals(JButton_UseTextBoxNormal))
                    {
                        TableUITextBox txb = new TableUITextBox();
                        txb.TextBoxType = TableUITextBoxType.Normal;
                        controlInfo = txb;
                    }
                    else if (btn.Equals(JButton_UseTextBoxNumberNormal))
                    {
                        TableUITextBoxNumber txb = new TableUITextBoxNumber();
                        ReadTextBoxNumUI2Obj( ref txb);

                        controlInfo = txb;
                    }
                    else if (btn.Equals(JButton_TextBoxNumTableInnerAssociate))
                    {
                        TableUITextBoxNumberInnerAssociate tbIner = new TableUITextBoxNumberInnerAssociate();
                        TableUITextBoxNumber txbBase = (TableUITextBoxNumber) tbIner;
                        ReadTextBoxNumUI2Obj( ref txbBase);

                        tbIner.Column1 = JDropDown_TextBoxNumTableInnerAssociateColumn1.SelectedText;
                        tbIner.Column2 = JDropDown_TextBoxNumTableInnerAssociateColumn2.SelectedText;
                        tbIner.Column3 = JDropDown_TextBoxNumTableInnerAssociateColumn3.SelectedText;
                        tbIner.CalculateType1With2 = JDropDown_TextBoxNumTableInnerAssociateCalculateType.SelectedText;
                        tbIner.CalculateType12With3 = JDropDown_TextBoxNumTableInnerAssociateCalculateType2.SelectedText;

                        if (!JDropDown_InnerAssociateResultWriteOut_TableName.SelectedText.Equals(DBDataTables.EmptyTableName))
                        {
                            TableUITextBoxNumberWrite2Out tbOut = new TableUITextBoxNumberWrite2Out();
                            tbOut.AssociatTableName = JDropDown_InnerAssociateResultWriteOut_TableName.SelectedText;
                            tbOut.AssociatTableColumnName = JDropDown_InnerAssociateResultWriteOut_ColumnName.SelectedText;
                            tbOut.AssociatTableForeignID = JDropDown_InnerAssociateResultWriteOut_ForeinConditionColum.SelectedText;
                            tbOut.AssociateCurrentID = JDropDown_InnerAssociateResultWriteOut_CurrentConditionColum.SelectedText;
                            double parm = 0;
                            if (double.TryParse(JTextbox_InnerAssociateResultWriteOut_Rate.Text, out parm))
                            {
                                tbOut.ChangedRate = JTextbox_InnerAssociateResultWriteOut_Rate.Text;
                            }
                            else
                            {
                                throw new Exception("请输入正确的数字");
                            }

                            tbIner.WriteOutDef = tbOut;
                        }

                        controlInfo = tbIner;
                    }
                    else if (btn.Equals(JButton_TextNumbAssociatTableModify))
                    {
                        TableUITextBoxNumberWrite2Out tbOut = new TableUITextBoxNumberWrite2Out();
                        TableUITextBoxNumber txbBase = (TableUITextBoxNumber) tbOut;
                        ReadTextBoxNumUI2Obj( ref txbBase);
                        if (!JDropDown_AsociatChangeTable.SelectedText.Equals(DBDataTables.EmptyTableName))
                        {
                            tbOut.AssociatTableName = JDropDown_AsociatChangeTable.SelectedText;
                            tbOut.AssociatTableColumnName = JDropDown_AsociatChangeColumn.SelectedText;
                            tbOut.AssociatTableForeignID = JDropDown_AsociatChangeTableForeinID.SelectedText;
                            tbOut.AssociateCurrentID = JDrown_CurrentAssociateColumn.SelectedText;
                            double parm = 0;
                            if (double.TryParse(JTextbox_AssociatParm.Text, out parm))
                            {
                                tbOut.ChangedRate = JTextbox_AssociatParm.Text;
                            }
                            else
                            {
                                throw new Exception("请输入正确的数字");
                            }
                        }
                        controlInfo = tbOut;
                    }
                    else if (btn.Equals(JButton_ReadAssociateOutTable))
                    {
                        TableUITextBoxNumberReadOut2Inner tbOut = new TableUITextBoxNumberReadOut2Inner();
                        TableUITextBoxNumber txbBase = (TableUITextBoxNumber)tbOut;
                        ReadTextBoxNumUI2Obj(ref txbBase);
                        if (!JDropDown_ReadAssociateOutTableName.SelectedText.Equals(DBDataTables.EmptyTableName))
                        {
                            tbOut.AssociatTableName = JDropDown_ReadAssociateOutTableName.SelectedText;
                            tbOut.AssociatTableColumnName = JDropDown_ReadAssociateOutTableColumn.SelectedText;
                            tbOut.AssociatTableForeignID = JDropDown_ReadAssociateOutTableConditionOutColumn.SelectedText;
                            tbOut.AssociateCurrentID = JDropDown_ReadAssociateOutTableConditionInnerColumn.SelectedText;
                            double parm = 0;
                            if (double.TryParse(JTextBox_ReadAssociateOutTableRate.Text, out parm))
                            {
                                tbOut.ChangedRate = JTextBox_ReadAssociateOutTableRate.Text;
                            }
                            else
                            {
                                throw new Exception("请输入正确的数字");
                            }
                        }
                        controlInfo = tbOut;
                    }
                    else if (btn.Equals(JButton_UseTextBoxRegex))
                    {
                        TableUITextBox txb = new TableUITextBox();
                        txb.TextBoxType = TableUITextBoxType.Regex;
                        txb.RegexStr = JTextBox_Regex.Text;
                        controlInfo = txb;
                    }
                    else if (btn.Equals(JButton_UseDropdown))
                    {
                        TableUIJDropDown dropDown = new TableUIJDropDown();
                        dropDown.ItemsStr = JTextBox_DropdownItems.Text;
                        
                        controlInfo = dropDown;
                    }
                    else if (btn.Equals(JButton_UseUploadify))
                    {
                        TableUIUploadify up = new TableUIUploadify();

                        up.FileDescription = JTextBox_FileTypesDescription.Text;
                        up.FileAllowTypes = JTextBox_AllowFileTypes.Text;

                        controlInfo = up;


                    }
                    else if (btn.Equals(JButton_UseShowFileAsImgeOrLink))
                    {
                        TableUIFileRender uiRender = new TableUIFileRender();
                        uiRender.DataSourceColumnName = JDropDown_ShowFileOrLinkDataSource.SelectedText;
                        uiRender.ImageHeight = JTextBox_ShowFileAsImageHeight.Text;
                        uiRender.ImageWidth = JTextBox_ShowFileAsImageWidth.Text;
                        uiRender.LinkText = JTextBox_ShowFileAsDownloadText.Text;
                        if (JDropwn_ShowFileOrLink.SelectedText.Equals("显示为图片"))
                        {
                            uiRender.RenderType = TableUIFileRenderType.Image;

                        }
                        if (JDropwn_ShowFileOrLink.SelectedText.Equals("显示为下载链接"))
                        {
                            uiRender.RenderType = TableUIFileRenderType.LinkURL;

                        }

                        controlInfo = uiRender;

                    }
                    else if (btn.Equals(JButton_UseInputByTreeSource))
                    {
                        TableUIInputByTree dbt = new TableUIInputByTree();

                        dbt.EnableMultiSelect = JCheckBox_DataTreeEnableMultiSelect.Checked;
                        dbt.TableFilterString = JTextBox_InputByTreeSourceManule.Text;
                        dbt.NodesSourceTableName = JDropDown_InputByTreeSourceByTable.SelectedText;
                        dbt.TableFilterString = JTextBox_InputByTreeSourceByTableDataFilter.Text;
                        dbt.NodesSourceText = JTextBox_InputByTreeSourceManule.Text;

                        controlInfo = dbt;
                    }

                    if (controlInfo != null)
                    {
                        controlInfo.ErrorInfo = JTextBox_ErrorInfo.Text;
                        controlInfo.Required = JCheckBoxRequired.Checked;
                    }

                    var uid = GetUIDObj();
                    if (uid == null)
                    {
                        uid = new TO_TableUIDefine();
                        uid.TableName = selectedTableName;
                        uid.ColumnName = selectedColumnName;
                        uid.UIDefine = SSPUConverter.Instance.Serialize_AsString(controlInfo);
                        uid.AddToDB();
                    }
                    else
                    {
                        uid.UIDefine = SSPUConverter.Instance.Serialize_AsString(controlInfo);
                        uid.ModifyToDB();
                    }

                    InitializUIByUIObj(controlInfo);
                }

                
            }
            
        }

        private void ReadTextBoxNumUI2Obj( ref TableUITextBoxNumber txb)
        {
            if (JDropDown_TextBoxNumberType.SelectedText.Equals("Int"))
            {
                txb.TextBoxType = TableUITextBoxType.Int;
            }

            if (JDropDown_TextBoxNumberType.SelectedText.Equals("Double"))
            {
                txb.TextBoxType = TableUITextBoxType.Double;
            }

            if (!string.IsNullOrEmpty(JTextBox_Min.Text))
            {
                txb.Min = JTextBox_Min.Text;
            }

            if (!string.IsNullOrEmpty(JTextBox_Max.Text))
            {
                txb.Max = JTextBox_Max.Text;
            }
        }

        private TO_TableUIDefine GetUIDObj()
        {
            Dictionary<string, string> condition = new Dictionary<string, string>();
            condition.Add(TO_TableUIDefine._TableName, selectedTableName);
            condition.Add(TO_TableUIDefine._ColumnName, selectedColumnName);
            TO_TableUIDefine uid = CachingManager.Instance.GetTO_ObjByCondition<TO_TableUIDefine>(condition);
            return uid;
        }

        protected void JDropDown_AsociatChangeTable_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            InitilizeAssociateWriteOutControls();
            //JTabs tbTxt = GetJTablTextBoxContainer();
            //tbTxt["数字类型"].Selected = true;
            SelectJTabTextNumItem("表外关联更新");
        }

        private void InitilizeAssociateWriteOutControls()
        {
            string selectedTab = JDropDown_AsociatChangeTable.SelectedText;
            if (IsPostBack)
            {
                
                JDropDown_AsociatChangeTable.Items.Clear();
                JDropDown_AsociatChangeTable.Items.Add(DBDataTables.EmptyTableName);
                string[] tables = Utility.Instance.AllUserDefinedTables;
                foreach (var s in tables)
                {
                    JDropDown_AsociatChangeTable.Items.Add(s);
                }
            }

            JDropDown_AsociatChangeTable.SelectedText = selectedTab;

            string tbName = JDropDown_AsociatChangeTable.SelectedText;
            if ( !string.IsNullOrEmpty(tbName) &&  !tbName.Equals(DBDataTables.EmptyTableName))
            {
                Panel_AssociatTableRefresh1.Visible = 
                    Panel_AssociatTableRefresh2.Visible =
                        true;

                DBObjBase obj = Utility.Instance.GetTO_ObjByTableName(tbName);

                
                Type tp = obj.GetType();
                JDropDown_AsociatChangeColumn.Items.Clear();
                JDrown_CurrentAssociateColumn.Items.Clear();
                JDropDown_AsociatChangeTableForeinID.Items.Clear();

                foreach (var s in obj._MyColumnsArray)
                {
                    JDropDown_AsociatChangeTableForeinID.Items.Add(s);
                }

                foreach (var p in tp.GetProperties())
                {
                    if (p.PropertyType == typeof(int) || p.PropertyType == typeof(double) || p.PropertyType == typeof(float))
                    {
                        JDropDown_AsociatChangeColumn.Items.Add(p.Name);
                    }


                }

                if (currentDBObj != null)
                {

                    DBObjBase currenTableObj = currentDBObj;
                    foreach (var s in currenTableObj._MyColumnsArray)
                    {
                        JDrown_CurrentAssociateColumn.Items.Add(s);
                    }
                    //Type ctp = currenTableObj.GetType();
                    //foreach (var p in ctp.GetProperties())
                    //{
                    //    //if (p.PropertyType == typeof(int))
                    //    {
                    //        JDrown_CurrentAssociateColumn.Items.Add(p.Name);
                    //    }
                    //}
                }


            }
        }

        private void InitilizeInner_AssociateWriteOutControls()
        {
            string selectedTab = JDropDown_InnerAssociateResultWriteOut_TableName.SelectedText;
            if (IsPostBack)
            {

                JDropDown_InnerAssociateResultWriteOut_TableName.Items.Clear();
                JDropDown_InnerAssociateResultWriteOut_TableName.Items.Add(DBDataTables.EmptyTableName);
                string[] tables = Utility.Instance.AllUserDefinedTables;
                foreach (var s in tables)
                {
                    JDropDown_InnerAssociateResultWriteOut_TableName.Items.Add(s);
                }
            }

            JDropDown_InnerAssociateResultWriteOut_TableName.SelectedText = selectedTab;

            string tbName = JDropDown_InnerAssociateResultWriteOut_TableName.SelectedText;
            if (!string.IsNullOrEmpty(tbName) && !tbName.Equals(DBDataTables.EmptyTableName))
            {
                Panel_InnerAssociateResultWriteOut_AssociateTable1.Visible =
                    Panel_InnerAssociateResultWriteOut_AssociatTableRefresh2.Visible =
                        true;

                DBObjBase obj = Utility.Instance.GetTO_ObjByTableName(tbName);


                Type tp = obj.GetType();
                JDropDown_InnerAssociateResultWriteOut_ColumnName.Items.Clear();
                JDropDown_InnerAssociateResultWriteOut_CurrentConditionColum.Items.Clear();
                JDropDown_InnerAssociateResultWriteOut_ForeinConditionColum.Items.Clear();

                foreach (var s in obj._MyColumnsArray)
                {
                    JDropDown_InnerAssociateResultWriteOut_ForeinConditionColum.Items.Add(s);
                }

                foreach (var p in tp.GetProperties())
                {
                    if (p.PropertyType == typeof(int) || p.PropertyType == typeof(double) || p.PropertyType == typeof(float))
                    {
                        JDropDown_InnerAssociateResultWriteOut_ColumnName.Items.Add(p.Name);
                    }


                }

                if (currentDBObj != null)
                {

                    DBObjBase currenTableObj = currentDBObj;
                    foreach (var s in currenTableObj._MyColumnsArray)
                    {
                        JDropDown_InnerAssociateResultWriteOut_CurrentConditionColum.Items.Add(s);
                    }
                }


            }
        }

        protected void JDropDown_ReadAssociateOutTableName_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeReadOut2InnerTab();
            //JTabs tbTxt = GetJTablTextBoxContainer();
            //tbTxt["数字类型"].Selected = true;
            SelectJTabTextNumItem("读取表外数据");
        }

        private void InitializeReadOut2InnerTab()
        {
            string selectedTab = JDropDown_ReadAssociateOutTableName.SelectedText;
            if (IsPostBack)
            {
                JDropDown_ReadAssociateOutTableName.Items.Clear();
                JDropDown_ReadAssociateOutTableName.Items.Add(DBDataTables.EmptyTableName);
                string[] tables = Utility.Instance.AllUserDefinedTables;
                foreach (var s in tables)
                {
                    JDropDown_ReadAssociateOutTableName.Items.Add(s);
                }
            }

            JDropDown_ReadAssociateOutTableName.SelectedText = selectedTab;

            string tbName = JDropDown_ReadAssociateOutTableName.SelectedText;
            if (!string.IsNullOrEmpty(tbName) && !tbName.Equals(DBDataTables.EmptyTableName))
            {
                Panel_ReadAssociateOutTableCondition.Visible =
                    Panel_ReadAssociateOutTableCondition2.Visible =
                        true;


                JDropDown_ReadAssociateOutTableColumn.Items.Clear();
                JDropDown_ReadAssociateOutTableConditionOutColumn.Items.Clear();
                JDropDown_ReadAssociateOutTableConditionInnerColumn.Items.Clear();

                DBObjBase obj = Utility.Instance.GetTO_ObjByTableName(tbName);
                Type tp = obj.GetType();
                foreach (var s in obj._MyColumnsArray)
                {
                    JDropDown_ReadAssociateOutTableColumn.Items.Add(s);
                    JDropDown_ReadAssociateOutTableConditionOutColumn.Items.Add(s);
                }

                if (currentDBObj != null)
                {
                    DBObjBase currenTableObj = currentDBObj;
                    foreach (var s in currenTableObj._MyColumnsArray)
                    {
                        JDropDown_ReadAssociateOutTableConditionInnerColumn.Items.Add(s);
                    }
                }
            }
        }

        protected void JDropDown__InnerAssociateResultWriteOut_AsociatChangeTable_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            InitilizeInner_AssociateWriteOutControls();
            //JTabs tbTxt = GetJTablTextBoxContainer();
            //tbTxt["数字类型"].Selected = true;
            SelectJTabTextNumItem("表内关联更新");
        }
    }
}