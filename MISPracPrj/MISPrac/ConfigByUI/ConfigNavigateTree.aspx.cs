using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBManager;
using Definition;
using SSPUCore.Configuration;
using SSPUCore.Controls;

using Image = System.Drawing.Image;

namespace MISPrac
{
    public partial class ConfigNavigateTree : System.Web.UI.Page
    {
        private int SelectedGlobalTreeNodeId = 0;
        private string selectedTableName = "";
        private int selectedUserGroupid = 0;
        private EnumDefs.PageType _selectedPageType;

        //private int SelectedNodePageTypeValue = 0;

        private EnumDefs.PageType SelectedPageType
        {
            get
            {
                return _selectedPageType;
            }
            set
            {
                _selectedPageType = value;
                if (Node_SetPageHTML != null)
                {
                    Node_SetPageHTML.Selected = Node_SetPageDesigned.Selected =
                        Node_SetPageVedio.Selected = Node_SetPageDoc.Selected = false;
                }

                switch (value)
                {
                    case EnumDefs.PageType.None:
                        break;
                    case EnumDefs.PageType.TableData:
                        {
                            if (selectedUserGroupid >= 1)
                            {
                                Panel_SetDataTable.Visible = true;
                            }

                        }
                        break;
                    case EnumDefs.PageType.HtmlPage:
                        if (Node_SetPageHTML != null)
                        {
                            Node_SetPageHTML.Selected = true;

                        }
                        break;
                    case EnumDefs.PageType.DesignedPage:
                        if (Node_SetPageDesigned != null)
                        {
                            Node_SetPageDesigned.Selected = true;

                        }
                        break;
                    case EnumDefs.PageType.ShowDocument:
                        if (Node_SetPageDoc != null)
                        {
                            Node_SetPageDoc.Selected = true;
                        }

                        break;
                    case EnumDefs.PageType.ShowMeida:
                        if (Node_SetPageVedio != null)
                        {
                            Node_SetPageVedio.Selected = true;
                        }
                        break;
                    default:
                        break;
                }

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SelectedPageType = EnumDefs.PageType.None;

            string s = HttpContext.Current.Request.QueryString[GlobalString.QuerySetNode2HTML];

            int selectedNodePageTypeValue = Utility.Instance.GetIDFromQueryString(GlobalString.QuerySetNode2HTML);

            SelectedPageType = selectedNodePageTypeValue > 0 ? (EnumDefs.PageType)selectedNodePageTypeValue : EnumDefs.PageType.None;


            if (SelectedPageType != EnumDefs.PageType.TableData)
            {
                HttpContext.Current.Session[GlobalString.QueryTableName] = null;

            }

            SelectedGlobalTreeNodeId = Utility.Instance.GetIDFromQueryString(GlobalString.QueryNavID);

            RadioButton_HtmlPageEditType.Items.Add(new ListItem(EnumDefs.HTMLPageEditType汉字.只读.ToString()));
            RadioButton_HtmlPageEditType.Items.Add(new ListItem(EnumDefs.HTMLPageEditType汉字.可写.ToString()));


            InitializeGlobalTree();

            InistializeSetNodeHtml();

            SetTableNodes();

            {
                FileManager1.FileUploadTargetPage = "/Services/Uploadify_FileManager.ashx";

                FileManager1.TargetPage = "/Services/FileManagerService.aspx";
                FileManager1.Root = string.Empty;

                FileManager1.FileEditTargetPage = "/Services/FileManagerService.aspx";
                FileManager1.FileEditTargetPageMethod = "DataOperator";
                FileManager1.FileEditTargetPageMethodParmName = "postData";

            }

            {
                if (SelectedGlobalTreeNodeId >= 1 && selectedUserGroupid >= 1)
                {
                    SetPageTypeContentVisible();


                }
            }

            if (selectedUserGroupid >= 1)
            {
                int groupAccordionID = Utility.Instance.GetIDFromQueryString(GlobalString.QueryUserGroupACCORDINID);
                if (groupAccordionID == 2)
                {
                    if (AccordionUserGrops.ItemsCount >= 2)
                    {
                        AccordionUserGrops[1].Selected = true;
                    }

                }
            }
            else
            {
                if (AccordionUserGrops[1].Visible)
                {
                    AccordionUserGrops[1].Selected = true;
                }
                else
                {
                    AccordionUserGrops[0].Selected = true;
                }
            }


        }

        private void SetPageTypeContentVisible()
        {
            Panel_SetDataTable.Visible = Panel_SetHTMLContent.Visible = Panel_SetDesignedPage.Visible = Panel_SetDocumentPage.Visible = Panel_SetVideoPage.Visible = false;

            if (SelectedGlobalTreeNodeId >= 1 && selectedUserGroupid >= 1)
            {
                Dictionary<string, string> condition = Utility.Instance.GetSearchingCondition(TO_DocumentPageAuthorityInfoByUserGroup._GlobalNavTreeID, SelectedGlobalTreeNodeId.ToString());
                condition.Add(TO_DocumentPageAuthorityInfoByUserGroup._UserGroupID, selectedUserGroupid.ToString());

                switch (SelectedPageType)
                {
                    case EnumDefs.PageType.None:
                        break;
                    case EnumDefs.PageType.TableData:
                        {
                            Panel_SetDataTable.Visible = true;
                            InitializeSetTablAuthorJRadios();
                            JDataTableManager.Instance.InitializeJDataTable(this.JDataTable_SetTablColumnAuthority, TO_TableColumnAuthorityByUserGroup._MyTableName, condition);
                        }
                        break;
                    case EnumDefs.PageType.HtmlPage:
                        Panel_SetHTMLContent.Visible = true;
                        break;
                    case EnumDefs.PageType.DesignedPage:
                        {
                            Panel_SetDesignedPage.Visible = true;

                            JDataTableManager.Instance.InitializeJDataTable(this.JDataTable_SetDesignedPage, TO_DesingedPageAuthorityInfoByUserGroup._MyTableName, condition);

                        }
                        break;
                    case EnumDefs.PageType.ShowDocument:
                        Panel_SetDocumentPage.Visible = true;
                        JDataTableManager.Instance.InitializeJDataTable(this.JDataTable_DocumentPageInfo, TO_DocumentPageAuthorityInfoByUserGroup._MyTableName, condition);
                        break;
                    case EnumDefs.PageType.ShowMeida:
                        Panel_SetVideoPage.Visible = true;
                        JDataTableManager.Instance.InitializeJDataTable(JDataTable_VedioPageInfo, TO_VideoPageAuthorityInfoByUserGroup._MyTableName, condition);
                        break;
                    default:
                        break;
                }
            }

        }

        private void InitializeSetTablAuthorJRadios(TO_TableAuthorityInfoByUserGroup oldValue = null)
        {
            if (JRadios_SetTableAuthorPermit.Items.Count == 0)
            {
                JRadios_SetTableAuthorPermit.Items.Add(new ListItem(JDataTableEditType.DenyEdit.ToString()));
                JRadios_SetTableAuthorPermit.Items.Add(new ListItem(JDataTableEditType.OnlyAdd.ToString()));
                JRadios_SetTableAuthorPermit.Items.Add(new ListItem(JDataTableEditType.OnlyModify.ToString()));
                JRadios_SetTableAuthorPermit.Items.Add(new ListItem(JDataTableEditType.OnlyDelete.ToString()));
                JRadios_SetTableAuthorPermit.Items.Add(new ListItem(JDataTableEditType.AddAndModify.ToString()));
                JRadios_SetTableAuthorPermit.Items.Add(new ListItem(JDataTableEditType.ModifyAndDelete.ToString()));
                JRadios_SetTableAuthorPermit.Items.Add(new ListItem(JDataTableEditType.AddAndDelete.ToString()));
                JRadios_SetTableAuthorPermit.Items.Add(new ListItem(JDataTableEditType.FullEdit.ToString()));

            }
            if (oldValue != null)
            {
                if (!string.IsNullOrEmpty(oldValue.EditType.Trim()))
                {
                    JRadios_SetTableAuthorPermit.Items.Add(new ListItem(EnumDefs.HTMLPageEditType汉字.清除权限.ToString()));
                    JRadios_SetTableAuthorPermit.Text = oldValue.EditType;
                }
            }
        }

        private JTreeNode Node_SetPageHTML { get; set; }
        private JTreeNode Node_SetPageDesigned { get; set; }
        private JTreeNode Node_SetPageDoc { get; set; }
        private JTreeNode Node_SetPageVedio { get; set; }


        private void InistializeSetNodeHtml()
        {
            int nodID = (int)EnumDefs.PageType.HtmlPage;

            string urlFormat =
                String.Format("?{0}={1}&{2}={{0}}&{3}={4}",
                    GlobalString.QueryNavID,SelectedGlobalTreeNodeId,
                    GlobalString.QuerySetNode2HTML,GlobalString.QueryUserGroupID,selectedUserGroupid);

            List<JTreeNode> nodes = new List<JTreeNode>();

            Node_SetPageHTML = new JTreeNode("显示为HTML页面", ((int)EnumDefs.PageType.HtmlPage).ToString(), "/Resource/Image/Icons/TreeNode/html.png",
                string.Format(urlFormat, (int)EnumDefs.PageType.HtmlPage), "设置该页面为显示HTML网页");
            nodes.Add(Node_SetPageHTML);

            Node_SetPageDesigned = new JTreeNode("显示已有页面", ((int)EnumDefs.PageType.DesignedPage).ToString(), "/Resource/Image/Icons/TreeNode/asp.png",
                string.Format(urlFormat, (int)EnumDefs.PageType.DesignedPage), "设置该页面为工程内已有页面内容");
            nodes.Add(Node_SetPageDesigned);

            Node_SetPageDoc = new JTreeNode("显示文档内容", ((int)EnumDefs.PageType.ShowDocument).ToString(), "/Resource/Image/Icons/TreeNode/paper.png",
                string.Format(urlFormat, (int)EnumDefs.PageType.ShowDocument), "设置该页面为文档");
            nodes.Add(Node_SetPageDoc);

            Node_SetPageVedio = new JTreeNode("播放多媒体内容", ((int)EnumDefs.PageType.ShowMeida).ToString(), "/Resource/Image/Icons/TreeNode/youtube.png",
                string.Format(urlFormat, (int)EnumDefs.PageType.ShowMeida), "设置该页面为播放多媒体");
            nodes.Add(Node_SetPageVedio);

            bool isShowTableData = true;
            foreach (var nd in nodes)
            {
                int nodeID = 0;
                int.TryParse(nd.NodeID, out nodeID);

                if ((int)SelectedPageType == nodeID)
                {
                    nd.Selected = true;
                    isShowTableData = false;
                }
                JTreeView_SetNode2HTMLPage.Nodes.Add(nd);

            }

            AccodTabType[ShowPageAccodName].Selected = !isShowTableData;

        }

        private string ShowTableAccodName = "显示数据表";
        private string ShowPageAccodName = "显示为页面";

        private void SetTableNodes()
        {

            if (SelectedGlobalTreeNodeId >= 1)
            {
                SetIdOrNameByQueryOrSession();

                EnumDefs.PageType ptype = AuthorityManager.Instance.GetNodeConfigedPageType(SelectedGlobalTreeNodeId);
                bool canAddTableNames = true;

                if (ptype != EnumDefs.PageType.None)
                {
                    SelectedPageType = ptype;

                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(TO_TableColumnAuthorityByUserGroup._GlobalNavTreeID, SelectedGlobalTreeNodeId.ToString());
                    dic.Add(TO_TableColumnAuthorityByUserGroup._UserGroupID, selectedUserGroupid.ToString());

                    this.AccodTabType[ShowTableAccodName].Visible = false;
                    this.AccodTabType[ShowPageAccodName].Visible = true;


                    switch (SelectedPageType)
                    {
                        case EnumDefs.PageType.None:
                            break;
                        case EnumDefs.PageType.TableData:
                            {
                                GetSettedGroupsAdd2JtreeNode(SelectedGlobalTreeNodeId, TO_TableAuthorityInfoByUserGroup._MyTableName);
                                TO_TableAuthorityInfoByUserGroup obj = CachingManager.Instance.GetTO_ObjByCondition<TO_TableAuthorityInfoByUserGroup>(Utility.Instance.GetSearchingCondition(TO_TableAuthorityInfoByUserGroup._GlobalNavTreeID, SelectedGlobalTreeNodeId.ToString()));

                                if (obj != null)
                                {
                                    selectedTableName = obj.TableName;
                                }

                                this.AccodTabType[ShowPageAccodName].Visible = false;
                                this.AccodTabType[ShowTableAccodName].Visible = true;
                                if (SelectedGlobalTreeNodeId >= 1 && selectedUserGroupid >= 1)
                                {
                                    TO_TableAuthorityInfoByUserGroup tau = CachingManager.Instance.GetTO_ObjByCondition<TO_TableAuthorityInfoByUserGroup>(dic);
                                    if (tau != null)
                                    {
                                        this.AccodTabType[ShowTableAccodName].Visible = true;
                                        this.AccodTabType[ShowPageAccodName].Visible = false;

                                        if (!IsPostBack)
                                        {
                                            DataFilter.Text = tau.DefDataFilterStr;
                                            InitializeSetTablAuthorJRadios(tau);
                                        }
                                    }

                                    SetColumnsList();
                                }


                            }
                            break;
                        case EnumDefs.PageType.HtmlPage:
                            {
                                GetSettedGroupsAdd2JtreeNode(SelectedGlobalTreeNodeId, TO_HTMLPageAuthorityInfoByUserGroup._MyTableName);
                                if (SelectedGlobalTreeNodeId >= 1 && selectedUserGroupid >= 1)
                                {
                                    Panel_SetHTMLContent.Visible = true;


                                    TO_HTMLPageAuthorityInfoByUserGroup oldValue = GetAuthorityObj<TO_HTMLPageAuthorityInfoByUserGroup>(SelectedGlobalTreeNodeId, selectedUserGroupid);

                                    if (!IsPostBack)
                                    {

                                        if (oldValue != null)
                                        {
                                            canAddTableNames = false;
                                            InitilizeRadioHtmlPagePermisionAndSetValue(oldValue);
                                        }
                                        else
                                        {
                                            for (int i = 0; i < RadioButton_HtmlPageEditType.Items.Count; i++)
                                            {
                                                RadioButton_HtmlPageEditType.Items[i].Selected = false;
                                            }

                                        }
                                    }


                                }

                            }
                            break;
                        case EnumDefs.PageType.DesignedPage:
                            {
                                GetSettedGroupsAdd2JtreeNode(SelectedGlobalTreeNodeId, TO_DesingedPageAuthorityInfoByUserGroup._MyTableName);

                                if (SelectedGlobalTreeNodeId >= 1 && selectedUserGroupid >= 1)
                                {

                                    this.Panel_SetDesignedPage.Visible = true;
                                    TO_DesingedPageAuthorityInfoByUserGroup oldValue = GetAuthorityObj<TO_DesingedPageAuthorityInfoByUserGroup>(SelectedGlobalTreeNodeId, selectedUserGroupid);

                                    if (oldValue != null)
                                    {
                                        canAddTableNames = false;
                                    }
                                }
                            }
                            break;
                        case EnumDefs.PageType.ShowDocument:
                            {
                                GetSettedGroupsAdd2JtreeNode(SelectedGlobalTreeNodeId, TO_DocumentPageAuthorityInfoByUserGroup._MyTableName);
                                TO_DocumentPageAuthorityInfoByUserGroup oldValue = GetAuthorityObj<TO_DocumentPageAuthorityInfoByUserGroup>(SelectedGlobalTreeNodeId, selectedUserGroupid);

                                if (oldValue != null)
                                {
                                    canAddTableNames = false;
                                }
                            }
                            break;
                        case EnumDefs.PageType.ShowMeida:
                            {
                                GetSettedGroupsAdd2JtreeNode(SelectedGlobalTreeNodeId, TO_VideoPageAuthorityInfoByUserGroup._MyTableName);
                                TO_VideoPageAuthorityInfoByUserGroup oldValue = GetAuthorityObj<TO_VideoPageAuthorityInfoByUserGroup>(SelectedGlobalTreeNodeId, selectedUserGroupid);

                                if (oldValue != null)
                                {
                                    canAddTableNames = false;
                                }
                            }
                            break;
                        default:
                            break;
                    }

                }

                if (canAddTableNames)
                {
                    InitializeTableNames();
                }


                AddAllGroupNodes();

                SaveQueryInfo2Session();
            }
        }

        private void InitializeTableNames()
        {
            string[] tables = Utility.Instance.AllUserDefinedTables;
            string urlFormat = String.Format("?{0}={1}&",GlobalString.QueryNavID,SelectedGlobalTreeNodeId)               +
                               String.Format("{0}={1}&",GlobalString.QuerySetNode2HTML,(int)EnumDefs.PageType.TableData) +
                               String.Format("{0}={{0}}&",GlobalString.QueryTableName) +
                               String.Format("{0}={1}",GlobalString.QueryUserGroupID,selectedUserGroupid);
            foreach (var s in tables)
            {
                List<JTreeNode> nodes = new List<JTreeNode>();
                JTreeNode nd = new JTreeNode(s, s, "/Resource/Image/Icons/TreeNode/TableF.png", string.Format(urlFormat, s), "");

                if (s.Equals(selectedTableName))
                {
                    nd.Selected = true;
                }

                JTreeView_TableName.Nodes.Add(nd);
            }
        }

        private void AddAllGroupNodes()
        {
            if (!string.IsNullOrEmpty(selectedTableName) || SelectedPageType != EnumDefs.PageType.None)
            {

                foreach (var toUserGroup in TO_UserGroup.AllGroups)
                {
                    //string st = setNode2HtmlPage
                    //    ? string.Format("&{0}=1",GlobalString.QuerySetNode2HTML)
                    //    : string.Format("&{0}={1}",GlobalString.QueryTableName,selectedTableName);

                    var nd = GetTreeNodeFromToUserGroup(toUserGroup);

                    JTreeView_UserGroup.Nodes.Add(nd);
                }
            }
        }

        //private void InitializeDesignPageContent(TO_DesingedPageAuthorityInfoByUserGroup oldValue)
        //{
        //    JTextBox_DesignedPageUrl.Text            = oldValue.PagePath;
        //    JRadio_DesignPagePermision.SelectedValue = oldValue.EditType;
        //}

        private void InitilizeRadioHtmlPagePermisionAndSetValue(TO_HTMLPageAuthorityInfoByUserGroup oldValue)
        {
            RadioButton_HtmlPageEditType.Items.Clear();

            if (oldValue == null)
            {
                RadioButton_HtmlPageEditType.Items.Add(new ListItem(EnumDefs.HTMLPageEditType汉字.只读.ToString()));
                RadioButton_HtmlPageEditType.Items.Add(new ListItem(EnumDefs.HTMLPageEditType汉字.可写.ToString()));

            }
            else
            {

                RadioButton_HtmlPageEditType.Items.Add(new ListItem(EnumDefs.HTMLPageEditType汉字.清除权限.ToString()));
                RadioButton_HtmlPageEditType.Items.Add(new ListItem(EnumDefs.HTMLPageEditType汉字.只读.ToString()));
                RadioButton_HtmlPageEditType.Items.Add(new ListItem(EnumDefs.HTMLPageEditType汉字.可写.ToString()));

                if (oldValue.EditTypeEnum == EnumDefs.HTMLPageEditType.Write)
                {
                    RadioButton_HtmlPageEditType.Items[2].Selected = true;
                    RadioButton_HtmlPageEditType.Text = EnumDefs.HTMLPageEditType汉字.可写.ToString();
                }
                else
                {
                    if (oldValue.EditTypeEnum == EnumDefs.HTMLPageEditType.ReadOnly)
                    {
                        RadioButton_HtmlPageEditType.Items[1].Selected = true;
                        RadioButton_HtmlPageEditType.Text = EnumDefs.HTMLPageEditType汉字.只读.ToString();
                    }
                    else
                    {
                        RadioButton_HtmlPageEditType.Items[0].Selected = true;
                    }
                }
            }

            GetSettedGroupsAdd2JtreeNode(SelectedGlobalTreeNodeId, TO_HTMLPageAuthorityInfoByUserGroup._MyTableName);

        }

        private void GetSettedGroupsAdd2JtreeNode(int navNodeID, string tableName)
        {
            int[] settedGroupIDs =
                GetDefinedGroupIDs(
                    Utility.Instance.GetSearchingCondition(
                        TO_HTMLPageAuthorityInfoByUserGroup._GlobalNavTreeID,
                        navNodeID.ToString()), tableName);

            JTreeView_UserGroupDefined.Nodes.Clear();

            foreach (var toUserGroup in TO_UserGroup.AllGroups)
            {
                if (settedGroupIDs.Contains(toUserGroup.ID))
                {
                    var nd = GetTreeNodeFromToUserGroup(toUserGroup, true);
                    if (toUserGroup.ID == selectedUserGroupid)
                    {
                        nd.Selected = true;
                    }
                    JTreeView_UserGroupDefined.Nodes.Add(nd);
                }

                AccordionUserGrops["已定义权限"].Visible = true;
            }
        }

        private JTreeNode GetTreeNodeFromToUserGroup(TO_UserGroup toUserGroup, bool isSetted = false)
        {
            int accordinID = isSetted ? 2 : 1;

            string st = String.Format("&{0}={1}",GlobalString.QuerySetNode2HTML,(int)SelectedPageType);
            if (SelectedPageType == EnumDefs.PageType.TableData)
            {
                st += String.Format("&{0}={1}",GlobalString.QueryTableName,selectedTableName);
            }

            JTreeNode nd = new JTreeNode(toUserGroup.Name, toUserGroup.Name, "/Resource/Image/Icons/TreeNode/group.png",
                string.Format("?{0}={1}", GlobalString.QueryNavID, SelectedGlobalTreeNodeId) +
                st +
                string.Format("&{0}={1}", GlobalString.QueryUserGroupID, toUserGroup.ID)
                + String.Format("&{0}={1}", GlobalString.QueryUserGroupACCORDINID,accordinID)
                , "");

            if (selectedUserGroupid == toUserGroup.ID)
            {
                nd.Selected = true;
            }

            return nd;
        }

        private void SaveQueryInfo2Session()
        {
            HttpContext.Current.Session[GlobalString.QueryNavID] = SelectedGlobalTreeNodeId;
            HttpContext.Current.Session[GlobalString.QueryUserGroupID] = selectedUserGroupid;
            HttpContext.Current.Session[GlobalString.QueryTableName] = selectedTableName;
            //HttpContext.Current.Session[GlobalString.QuerySetNode2HTML] = selectedPageType;
        }

        private void SetIdOrNameByQueryOrSession()
        {
            selectedTableName = HttpContext.Current.Request.QueryString[GlobalString.QueryTableName];

            if (string.IsNullOrEmpty(selectedTableName))
            {
                selectedTableName = AuthorityManager.Instance.CurrentTableName;
            }

            selectedUserGroupid = Utility.Instance.GetIDFromQueryString(GlobalString.QueryUserGroupID);
            if (selectedUserGroupid <= 0)
            {
                //if (HttpContext.Current.Session[GlobalString.QueryUserGroupID] != null)
                //{
                //    selectedUserGroupid = (int) HttpContext.Current.Session[GlobalString.QueryUserGroupID];
                //}
            }
        }

        private void SetColumnsList()
        {
            DBObjBase obj = Utility.Instance.GetTO_ObjByTableName(selectedTableName);

            if (obj != null)
            {
                string columns = "数据表列包括：";
                foreach (var s in obj._MyColumnsArray)
                {
                    columns += " " + s + ", ";
                }

                this.LabelTableColumns.Text = columns;
            }

        }


        #region SetNavTree

        [WebMethod(EnableSession = true)]
        public static string TargetPageMethod_ForCallData(string postData)
        {
            Dictionary<string, string> postDic = ControlUtility.ParseUserInputToDataRow(postData);
            AjaxDataOperatorType operatorType = ControlUtility.GetOperateType(postData);
            DBObjBase node = new TO_GlobalNavTree();

            node.Parse(postDic);
            int nodeId = node.ID;
            node = CachingManager.Instance.SetValueToDBObj<TO_GlobalNavTree>(node.ID);

            if (node == null)
            {
                if (nodeId == 0)
                {
                    node = new TO_GlobalNavTree();
                    node.Parse(postDic);
                }
                else
                {
                    return JDataTable.SetFailedPostbackDataToClient("数据不正确！", null);
                }

            }


            TO_GlobalNavTree riNode = node as TO_GlobalNavTree;

            switch (operatorType)
            {
                case AjaxDataOperatorType.ADD:
                    riNode.ParentID = node.ID;
                    break;
                case AjaxDataOperatorType.MODIFY:

                    break;
                case AjaxDataOperatorType.DELETE:
                    Dictionary<string, string> serachCondition = new Dictionary<string, string>();
                    serachCondition.Add(TO_GlobalNavTree._ParentID, riNode.ID.ToString());
                    DataTable nodesDT = CachingManager.Instance.GetDataTable(riNode.MyTableNameInDB, serachCondition);

                    if (nodesDT != null && nodesDT.Rows.Count >= 1)
                    {
                        return JDataTable.SetFailedPostbackDataToClient("不能删除该节点，因为子节点不为空！", null);
                    }

                    break;
                default:

                    break;
            }

            return JDataTable.TO_ObjectEditDone(riNode);
        }

        private static int DeleteDataByDictionary(string tableName, Dictionary<string, string> dic)
        {
            DataTable dt = CachingManager.Instance.GetDataTable(tableName, dic);
            int count = 0;

            DBObjBase obj = Utility.Instance.GetTO_ObjByTableName(tableName);
            if (dt != null && dt.Rows.Count >= 1)
            {
                if (tableName.Equals(TO_VideoPageAuthorityInfoByUserGroup._MyTableName))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        obj.Parse(row);

                        obj.DeleteToDB();
                        count++;
                    }
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        obj.Parse(row);
                        obj.DeleteToDB();
                        count++;
                    }
                }


            }

            return count;
        }

        [WebMethod(EnableSession = true)]
        public static string JTreeViewTargetMethod(string postData)
        {
            Dictionary<string, string> postDic = ControlUtility.ParseUserInputToDataRow(postData);
            AjaxDataOperatorType operatorType = ControlUtility.GetOperateType(postData);

            TO_GlobalNavTree node = new TO_GlobalNavTree();

            node.Parse(postDic);

            if (operatorType != AjaxDataOperatorType.MOVENODE)
            {
                if (operatorType == AjaxDataOperatorType.DoAction)
                {
                    Dictionary<string, string> dic = Utility.Instance.GetSearchingCondition(TO_HTMLPageContent._GlobalNavTreeID, node.ID.ToString());

                    string[] tables = new string[]
                    {
                        TO_HTMLPageContent._MyTableName,
                        TO_HTMLPageAuthorityInfoByUserGroup._MyTableName,

                        TO_DesingedPageAuthorityInfoByUserGroup._MyTableName,
                        TO_DocumentPageAuthorityInfoByUserGroup._MyTableName,
                        TO_VideoPageAuthorityInfoByUserGroup._MyTableName,

                        TO_TableColumnAuthorityByUserGroup._MyTableName,
                        TO_TableAuthorityInfoByUserGroup._MyTableName,
                    };

                    int count = 0;
                    for (int i = 0; i < tables.Length; i++)
                    {
                        count += DeleteDataByDictionary(tables[i], dic);
                    }


                }
                else
                {
                    node = JDataTableManager.Instance.OnlyDoDBObjBaseDataOperate(node, operatorType);
                }
            }
            else
            {
                int id = node.ID;
                int newParentID = int.Parse(postDic["NEWPARENTID"]);

                node = CachingManager.Instance.SetValueToDBObj<TO_GlobalNavTree>(node.ID);

                if (node.MyAllChildsNodes != null)
                {
                    foreach (var navTree in node.MyAllChildsNodes)
                    {
                        if (navTree.ID == newParentID)
                        {
                            return JDataTable.SetFailedPostbackDataToClient("此操作不合法", null);
                        }
                    }
                }


                node.ParentID = newParentID;
                node.ModifyToDB();
            }

            if (node.NavTo.Trim().Length >= 1)
            {
                node.NavTo = string.Format("?{0}={1},{2}", GlobalString.QueryNavID, node.ID, node.NavTo);
            }
            else
            {
                node.NavTo = string.Format("?{0}={1}", GlobalString.QueryNavID, node.ID);
            }



            return JDataTableManager.Instance.SerializeDataForTreeView(node);
        }

        private void InitializeGlobalTree()
        {
            JTreeNode rootNode;
            DataTable dt = TO_GlobalNavTree.GetAllNodes(out rootNode, SelectedGlobalTreeNodeId, true);

            if (SelectedGlobalTreeNodeId <= 0 || (rootNode != null && SelectedGlobalTreeNodeId == int.Parse(rootNode.Value)))
            {
                this.JTreeView_NavDef.ExpandAll = true;
            }

            this.JTreeView_NavDef.Nodes.Add(rootNode);
            TO_GlobalNavTree to_rif = new TO_GlobalNavTree();
            JTreeView_NavDef.DataTableName = to_rif.MyTableNameInDB;
            JTreeView_NavDef.ColumnsDefine = to_rif.ColumnsDefine;
            JTreeView_NavDef.DataColumnsToInput = dt.Columns;
            JTreeView_NavDef.ContextMenu.SelfDefin1 = new SelfDefContextMenuItem(ContextMenuIcon.Delete, "删除本节点所有配置", true, "是否要删除该节点全部配置？此操作不可恢复！");

            //JTreeView_NavDef.ContextMenu.Items.Add(new ContextMenuItem( ContextMenuIcon.Delete,"删除本节点所有配置", ControlUtility.BuildJSFunction("alert('OK');") ));

            //JTreeView_NavDef.AjaxServerFun = new AjaxServerFunDef("ConfigByUI/ConfigNavigateTree.aspx",
            //    "JTreeViewTargetMethod", "postData");
            JTreeView_NavDef.TargetPage = "ConfigByUI/ConfigNavigateTree.aspx";
            JTreeView_NavDef.TargetPageMethod = "JTreeViewTargetMethod";
            JTreeView_NavDef.TargetPageMethodParmName = "postData";

            JTreeView_NavDef.TargetPage_ForCallData = "ConfigByUI/ConfigNavigateTree.aspx";
            JTreeView_NavDef.TargetPageMethod_ForCallData = "TargetPageMethod_ForCallData";
            JTreeView_NavDef.TargetPageMethodParmName_ForCallData = "postData";

            JTreeView_NavDef.Edit_NodeIDIndex = 0;
            JTreeView_NavDef.Edit_NodeNameIndex = 1;
            JTreeView_NavDef.Edit_NodeIconIndex = 10;
            JTreeView_NavDef.Edit_NodeUrlIndex = 3;
        }


        #endregion

        protected void AddDataFilter(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(TO_TableAuthorityInfoByUserGroup._UserGroupID, selectedUserGroupid.ToString());
                dic.Add(TO_TableAuthorityInfoByUserGroup._GlobalNavTreeID, SelectedGlobalTreeNodeId.ToString());
                dic.Add(TO_TableAuthorityInfoByUserGroup._TableName, selectedTableName);
                TO_TableAuthorityInfoByUserGroup tau =
                    CachingManager.Instance.GetTO_ObjByCondition<TO_TableAuthorityInfoByUserGroup>(dic);

                bool isNew = tau == null;

                if (isNew)
                {
                    tau = new TO_TableAuthorityInfoByUserGroup();
                }

                tau.DefDataFilterStr = DataFilter.Text;
                tau.EditType = JRadios_SetTableAuthorPermit.Text;
                if (tau.EditType == EnumDefs.HTMLPageEditType汉字.清除权限.ToString())
                {


                    Dictionary<string, string> dic2 = new Dictionary<string, string>();
                    dic2.Add(TO_TableColumnAuthorityByUserGroup._GlobalNavTreeID, tau.GlobalNavTreeID.ToString());
                    dic2.Add(TO_TableColumnAuthorityByUserGroup._UserGroupID, tau.UserGroupID.ToString());

                    DeleteDataByDictionary(TO_TableColumnAuthorityByUserGroup._MyTableName, dic2);

                    tau.DeleteToDB();

                    JHighLightContent_EditTabAuthor.Visible = true;
                    JHighLightContent_EditTabAuthor.Title = "已成功删除所有设置信息" + DateTime.Now.ToLongTimeString();
                }
                else
                {

                    if (isNew)
                    {
                        tau.AddToDB();
                        JHighLightContent_EditTabAuthor.Title = "已成功添加到数据库" + DateTime.Now.ToLongTimeString();
                    }
                    else
                    {
                        tau.ModifyToDB();
                        JHighLightContent_EditTabAuthor.Title = "已修改成功" + DateTime.Now.ToLongTimeString();
                    }

                    JHighLightContent_EditTabAuthor.Visible = true;

                }



                InitializeSetTablAuthorJRadios(tau);
            }
            catch (Exception exception)
            {
                JHighLightContent_EditTabAuthor.Visible = true;
                JHighLightContent_EditTabAuthor.ContentType = HeighLightType.ERROR;
                JHighLightContent_EditTabAuthor.Title = exception.Message;
            }
        }

        protected void HtmlSetButtonOnClick(object sender, EventArgs e)
        {
            try
            {

                string editType = RadioButton_HtmlPageEditType.Text;

                if (!string.IsNullOrEmpty(editType))
                {
                    HighLight_EditHTMLTypeDone.Visible = true;
                    TO_HTMLPageAuthorityInfoByUserGroup oldValue = new TO_HTMLPageAuthorityInfoByUserGroup();

                    oldValue = GetAuthorityObj<TO_HTMLPageAuthorityInfoByUserGroup>(SelectedGlobalTreeNodeId, selectedUserGroupid);

                    if (editType == EnumDefs.HTMLPageEditType汉字.清除权限.ToString())
                    {
                        if (oldValue != null)
                        {
                            oldValue.DeleteToDB();
                        }

                        HighLight_EditHTMLTypeDone.Title = "已成功删除权限" + DateTime.Now.ToLongTimeString();

                    }
                    else
                    {
                        if (oldValue != null)
                        {
                            oldValue.EditType = editType;
                            oldValue.ModifyToDB();
                            HighLight_EditHTMLTypeDone.Title = "已成功修改权限" + DateTime.Now.ToLongTimeString();
                        }
                        else
                        {
                            TO_HTMLPageAuthorityInfoByUserGroup hbu = new TO_HTMLPageAuthorityInfoByUserGroup();
                            hbu.GlobalNavTreeID = SelectedGlobalTreeNodeId;
                            hbu.UserGroupID = selectedUserGroupid;
                            hbu.EditType = editType;
                            hbu.AddToDB();
                            HighLight_EditHTMLTypeDone.Title = "已成功添加权限" + DateTime.Now.ToLongTimeString();
                        }
                    }

                    TO_HTMLPageAuthorityInfoByUserGroup newValue = GetAuthorityObj<TO_HTMLPageAuthorityInfoByUserGroup>(SelectedGlobalTreeNodeId, selectedUserGroupid);

                    InitilizeRadioHtmlPagePermisionAndSetValue(newValue);
                }


            }
            catch (Exception exception)
            {
                HighLight_EditHTMLTypeDone.ContentType = HeighLightType.ERROR;

                HighLight_EditHTMLTypeDone.Title = exception.Message + DateTime.Now.ToLongTimeString();
            }
        }

        private T GetAuthorityObj<T>(int navTreeID, int userGroupID)
        {
            Dictionary<string, string> dic = Utility.Instance.GetSearchingCondition(
                TO_HTMLPageAuthorityInfoByUserGroup._GlobalNavTreeID, navTreeID.ToString());
            dic.Add(TO_HTMLPageAuthorityInfoByUserGroup._UserGroupID, userGroupID.ToString());

            return CachingManager.Instance.GetTO_ObjByCondition<T>(dic);

        }

        private int[] GetDefinedGroupIDs(Dictionary<string, string> condition, string tableName)
        {
            DataTable dt = CachingManager.Instance.GetDataTable(tableName, condition);
            List<int> result = new List<int>();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row[TO_HTMLPageAuthorityInfoByUserGroup._UserGroupID] != null)
                    {
                        int t = int.Parse(row[TO_HTMLPageAuthorityInfoByUserGroup._UserGroupID].ToString());
                        result.Add(t);
                    }
                }
            }

            return result.ToArray();
        }
        
    }
}