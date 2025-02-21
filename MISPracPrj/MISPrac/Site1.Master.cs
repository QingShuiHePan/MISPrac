using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBManager;
using Definition;
using MISPrac;
using SSPUCore.Configuration;
using SSPUCore.Controls;

namespace MISPrac
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        

        private int selectedNavNodeId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Utility.Instance.AplicationStartPath = HttpContext.Current.Request.PhysicalApplicationPath;
            
            GetOrSetFirstLoginSelectedNode();
            
            SetJDataTableLoadExcelHandler();

            if (!IsPostBack)
            {
                string queryString = string.Format("?{0}={1}", GlobalString.QueryNavID, selectedNavNodeId);
                string rdrStr      = "isrdr";
                int    isrdr       = Utility.Instance.GetIDFromQueryString(rdrStr);
                isrdr = isrdr >= 1 ? 1 : 0;

                if (selectedNavNodeId >= 1 )
                {
                    DBObjBase obj = null;
                    EnumDefs.PageType pType = AuthorityManager.Instance.GetNodeConfigedPageType(selectedNavNodeId, out obj);
                    if (isrdr != 1)
                    {
                        queryString += String.Format("&{0}=1",rdrStr);
                        switch (pType)
                        {
                            case EnumDefs.PageType.None:
                                {
                                    OnNoRights();
                                    return;
                                }
                                break;
                            case EnumDefs.PageType.TableData:
                                {
                                    Response.Redirect("~/GroupPage/ShowDefTable.aspx" + queryString);
                                    return;
                                }
                                break;
                            case EnumDefs.PageType.HtmlPage:
                                {
                                    Response.Redirect("~/GroupPage/ShowHtCnt.aspx" + queryString);
                                    return;
                                }
                                break;
                            case EnumDefs.PageType.DesignedPage:
                                {
                                    DBObjBase[] objs = null;
                                    JDataTableEditType editType = AuthorityManager.Instance.GetJDataTableEditTypeByAllRights( TO_DesingedPageAuthorityInfoByUserGroup._MyTableName, out objs);

                                    if (editType != JDataTableEditType.Hidden && editType != JDataTableEditType.None)
                                    {
                                        if (objs != null)
                                        {
                                            List<TO_DesingedPageAuthorityInfoByUserGroup> lstPs = new List<TO_DesingedPageAuthorityInfoByUserGroup>();

                                            foreach (var o in objs)
                                            {
                                                if (o is TO_DesingedPageAuthorityInfoByUserGroup)
                                                {
                                                    lstPs.Add(o as TO_DesingedPageAuthorityInfoByUserGroup);
                                                }
                                            }


                                            if (lstPs.Count >= 1)
                                            {
                                                int isTreeNodeClicked = Utility.Instance.GetIDFromQueryString(GlobalString.QueryIsTreeNodeClicked);
                                                if (isTreeNodeClicked <= 0) //On页面中点击导航
                                                {
                                                    List<TO_DesingedPageAuthorityInfoByUserGroup> samePageNames =
                                                        new List<TO_DesingedPageAuthorityInfoByUserGroup>();
                                                    foreach (var p in lstPs)
                                                    {
                                                        if (!p.PagePath.StartsWith("/"))
                                                        {
                                                            p.PagePath = "/" + p.PagePath;
                                                        }
                                                        if (p.PagePath.StartsWith(Request.CurrentExecutionFilePath))
                                                        {
                                                            samePageNames.Add(p);
                                                        }
                                                    }

                                                    if (samePageNames.Count >= 1)
                                                    {
                                                        lstPs = samePageNames;
                                                    }
                                                }

                                                lstPs.Sort();


                                                string path = string.Empty;
                                                foreach (var p in lstPs)
                                                {
                                                    if (p.GlobalNavTreeID == selectedNavNodeId)
                                                    {
                                                        if (!string.IsNullOrEmpty(p.PagePath.Trim()))
                                                        {
                                                            path = p.PagePath;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (!string.IsNullOrEmpty(path))
                                                {
                                                    if (Navigate2Page(path, queryString))
                                                    {
                                                        return;
                                                    }
                                                }


                                            }
                                        }
                                    }
                                }
                                break;
                            case EnumDefs.PageType.ShowDocument:    
                                {
                                    int pdfIndex = Utility.Instance.GetIDFromQueryString(GlobalString.QUERYPDFFILEINDEX);
                                    if (pdfIndex >= 1)
                                    {
                                        queryString += String.Format("&{0}={1}",GlobalString.QUERYPDFFILEINDEX,pdfIndex);
                                    }
                                    Response.Redirect("~/GroupPage/ShowPDFFile.aspx" + queryString);
                                    return;

                                }
                                break;
                            case EnumDefs.PageType.ShowMeida:
                                {
                                    int pdfIndex = Utility.Instance.GetIDFromQueryString(GlobalString.QUERYPDFFILEINDEX);
                                    if (pdfIndex >= 1)
                                    {
                                        queryString += String.Format("&{0}={1}", GlobalString.QUERYPDFFILEINDEX, pdfIndex);
                                    }
                                    Response.Redirect("~/GroupPage/ShowVides.aspx" + queryString);
                                    return;

                                }
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else //检查权限
                    {
                        switch (pType)
                        {
                            case EnumDefs.PageType.None:
                            {
                                OnNoRights();
                                return;

                            }
                                break;
                            case EnumDefs.PageType.TableData:
                            
                                break;
                            case EnumDefs.PageType.HtmlPage:
                                break;
                            case EnumDefs.PageType.DesignedPage:
                                {
                                    NameValueCollection      currentQueryString = Request.QueryString;
                                    Dictionary<string, bool> dicAllQueryChecked = new Dictionary<string, bool>();
                                    if (currentQueryString.Count >= 1)
                                    {
                                        foreach (string s in currentQueryString)
                                        {
                                            if (!string.IsNullOrEmpty(s))
                                            {
                                                if (s != GlobalString.QueryNavID && s != rdrStr)
                                                {
                                                    dicAllQueryChecked.Add(s, false);
                                                }
                                            }
                                        }
                                    }

                                    if (dicAllQueryChecked.Keys.Count >= 1)
                                    {
                                        //check Query string values
                                        DBObjBase[] dbObjs = null;
                                        JDataTableEditType editType = AuthorityManager.Instance.GetJDataTableEditTypeByAllRights(TO_DesingedPageAuthorityInfoByUserGroup._MyTableName,out dbObjs);

                                        if (editType != JDataTableEditType.Hidden || editType != JDataTableEditType.None)
                                        {
                                            List<TO_DesingedPageAuthorityInfoByUserGroup> lstPgs = new List<TO_DesingedPageAuthorityInfoByUserGroup>();
                                            if (dbObjs != null)
                                            {
                                                foreach (var o in dbObjs)
                                                {
                                                    TO_DesingedPageAuthorityInfoByUserGroup p = o as TO_DesingedPageAuthorityInfoByUserGroup;
                                                    if (!p.PagePath.StartsWith("/"))
                                                    {
                                                        p.PagePath = "/" + p.PagePath;
                                                    }
                                                    if (p.PagePath.StartsWith(Request.CurrentExecutionFilePath))
                                                    {
                                                        lstPgs.Add(p);
                                                    }
                                                }

                                                string path = string.Empty;
                                                bool ignorParms = false;
                                                foreach (var page in lstPgs)
                                                {
                                                    if (!string.IsNullOrEmpty(page.PagePath.Trim()))
                                                    {
                                                        string query = page.PagePath.Contains("?") ? page.PagePath.Substring(page.PagePath.IndexOf('?') + 1) : string.Empty;
                                                        if (!string.IsNullOrEmpty(query))
                                                        {
                                                            NameValueCollection configedQstr = HttpUtility.ParseQueryString(query);

                                                            if (configedQstr.AllKeys.Contains("Q") &&
                                                                configedQstr["Q"].ToUpper() == "ANY".ToUpper())
                                                            {
                                                                ignorParms = true;
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                string[] keys = dicAllQueryChecked.Keys.ToArray();

                                                                foreach (string key in keys)
                                                                {
                                                                    if (dicAllQueryChecked[key] == false)
                                                                    {
                                                                        if (configedQstr[key] != null)
                                                                        {
                                                                            if (configedQstr[key].ToUpper() == "AnyID".ToUpper() || configedQstr[key] == currentQueryString[key])
                                                                            {
                                                                                dicAllQueryChecked[key] = true;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                if (!ignorParms)
                                                {
                                                    foreach (var key in dicAllQueryChecked.Keys)
                                                    {
                                                        if (dicAllQueryChecked[key] == false) //权限不匹配
                                                        {
                                                            Response.Redirect("~/OnNoRightPage.aspx");
                                                            return;
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                OnNoRights();
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            OnNoRights();
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        if (CheckDesignedPageRight_OnNoQuery())
                                        {
                                            return;
                                        }
                                    }
                                }
                                break;
                            case EnumDefs.PageType.ShowDocument:
                                break;
                            case EnumDefs.PageType.ShowMeida:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
                else
                {
                    ContentPlaceHolder1.Visible = false;
                }
            }

            Initialize();
        }

        private bool CheckDesignedPageRight_OnNoQuery()
        {
            DBObjBase[] dbObjs = null;
            JDataTableEditType editType =AuthorityManager.Instance.GetJDataTableEditTypeByAllRights(TO_DesingedPageAuthorityInfoByUserGroup._MyTableName, out dbObjs);

            if (editType != JDataTableEditType.Hidden || editType != JDataTableEditType.None)
            {
                List<TO_DesingedPageAuthorityInfoByUserGroup> lstPgs = new List<TO_DesingedPageAuthorityInfoByUserGroup>();
                
                foreach (var o in dbObjs)
                {
                    TO_DesingedPageAuthorityInfoByUserGroup p = o as TO_DesingedPageAuthorityInfoByUserGroup;
                    if (!p.PagePath.StartsWith("/"))
                    {
                        p.PagePath = "/" + p.PagePath;
                    }
                    if (p.PagePath.StartsWith(Request.CurrentExecutionFilePath))
                    {
                        lstPgs.Add(p);
                    }
                }
                
                bool hasRight = false;
                string pathCurrent = Server.MapPath(Request.Url.AbsolutePath);

                bool isOk = false;
                foreach (var p in lstPgs)
                {
                    string path = GetNavtoPathFromRoot(p.PagePath);
                    if (path.Contains("?"))
                    {
                        path = path.Substring(0, path.IndexOf('?'));
                    }


                    if (Server.MapPath(path) == pathCurrent)
                    {
                        isOk = true;
                        break;
                    }
                }

                if (!isOk)
                {
                    OnNoRights();
                    return true;
                }

            }
            else
            {
                OnNoRights();
                return true;
            }


           

            return false;
        }

        private void OnNoRights()
        {
            
            if (this.Page is Default)
            {
                if (!Initialized)
                {
                    Initialize();
                }
            }
            else
            {
                if (selectedNavNodeId < DynamicNodeBase)
                {
                    this.ContentPlaceHolder1.Visible = false;
                }
                else
                {
                    if (!Initialized)
                    {
                        Initialize();
                    }
                }
            }
            
            return;
        }

        private void GetOrSetFirstLoginSelectedNode()
        {
            selectedNavNodeId = Utility.Instance.GetIDFromQueryString(GlobalString.QueryNavID);



            if (selectedNavNodeId >= 1)
            {
                HttpContext.Current.Session[GlobalString.QueryNavID] = selectedNavNodeId;
            }
            else
            {
                if (this.Page is Default)
                {
                    string s = SSPUAppSettings.GetConfig("HomePageNavID");
                    if (!string.IsNullOrEmpty(s))
                    {
                        int id = 0;
                        if (int.TryParse(s, out id))
                        {
                            selectedNavNodeId                                    = id;
                            HttpContext.Current.Session[GlobalString.QueryNavID] = selectedNavNodeId;
                        }
                    }
                }
                else
                {
                    if (HttpContext.Current.Session[GlobalString.QueryNavID] != null)
                    {
                        selectedNavNodeId = (int)HttpContext.Current.Session[GlobalString.QueryNavID];
                    }
                }
            }
        }

        private bool Navigate2Page(string configedNavStrs, string dynamicParms)
        {
            var navto = GetNavtoPathFromRoot(configedNavStrs, dynamicParms);

            NameValueCollection orignalQueryNVC = HttpContext.Current.Request.QueryString;
            NameValueCollection resultQuerNVC = HttpUtility.ParseQueryString(navto.Substring(navto.IndexOf('?')));

            if (resultQuerNVC.AllKeys.Contains("Q") && resultQuerNVC["Q"].ToUpper() == "Any".ToUpper())
            {
                foreach (string key in orignalQueryNVC.Keys)
                {
                    if (key != GlobalString.QueryIsTreeNodeClicked)
                    {
                        if (!resultQuerNVC.AllKeys.Contains(key))
                        {
                            resultQuerNVC.Add(key, orignalQueryNVC[key]);
                        }
                        else
                        {
                            resultQuerNVC[key] = orignalQueryNVC[key];
                        }
                    }
                    
                }

                resultQuerNVC.Remove("Q");
                
            }
            else
            {
                bool allValuesOk = true;

                for (int i = 0; i < resultQuerNVC.Keys.Count; i++)
                {
                    string key = resultQuerNVC.Keys[i];

                    if (orignalQueryNVC.AllKeys.Contains(key))
                    {
                        if (resultQuerNVC[key].ToUpper() == "ANYID".ToUpper())
                        {
                            resultQuerNVC[key] = orignalQueryNVC[key];
                        }
                    }
                }

            }

            navto = navto.Substring(0, navto.IndexOf('?')) + "?&" + resultQuerNVC.ToString();


            Response.Redirect(navto);
            return false;
        }

        private string GetNavtoPathFromRoot(string navtoParm, string queryString="")
        {
            string path     = this.Page.AppRelativeVirtualPath;
            string pageName = path.Substring(path.LastIndexOf('/') + 1);

            string qu1 = string.Empty;
            string qu2 = string.IsNullOrEmpty(queryString)?string.Empty: queryString.Replace("?",string.Empty);
            
            string navto = navtoParm;
            

            if (navto.Contains("?"))
            {
                
                
                navto = navto.Substring(0, navto.IndexOf('?'));
                qu1   = navtoParm.Substring(navto.Length +1);
            }

            if (!navto.StartsWith("~"))
            {
                if (navto.StartsWith("/"))
                {
                    navto = "~" + navto;
                }
                else
                {
                    navto = "~/" + navto;
                }
                
            }

            if (!string.IsNullOrEmpty(qu2))
            {
                navto = String.Format("{0}?{1}&{2}",navto,qu1,qu2);
            }
            else
            {
                navto = String.Format("{0}?{1}",navto,qu1);
            }

            
            return navto;
        }
        
        private void SetJDataTableLoadExcelHandler()
        {
            if (Utility.Instance.IsLogin)
            {
                string[] ids = JDataTable.TableIDsInPage;
                if (ids != null && ids.Length >= 1)
                {
                    foreach (var id in ids)
                    {
                        Control ctrl = this.ContentPlaceHolder1.FindControl(id);
                        if (ctrl != null && (ctrl is JDataTable))
                        {
                            ((JDataTable)ctrl).UploadByExcleClicked += JDataTable_UploadByExcleClicked;
                        }
                    }
                }

            }

        }

        void JDataTable_UploadByExcleClicked(object sender, InputByExcelInfo info)
        {
            if (sender is JDataTable)
            {
                JDataTable tbControl = sender as JDataTable;

                DataTable dt = JDataTable.GetDataTableFromExcelFile(info);
                List<UploadDataByExcelErrorInfo> errors = new List<UploadDataByExcelErrorInfo>();

                int i = 1;
                int sucessAdded = 0;
                foreach (DataRow dataRow in dt.Rows)
                {
                    DBObjBase obj = Utility.Instance.GetTO_ObjByTableName(tbControl.DataTableName);
                    i++;
                    obj.ParseExcel(dataRow);
                    try
                    {
                        JDataTableManager.Instance.OnlyDoDBObjBaseDataOperate(obj, AjaxDataOperatorType.ADD);

                        sucessAdded++;
                    }
                    catch (Exception exc)
                    {
                        errors.Add(new UploadDataByExcelErrorInfo(i, dataRow, exc.Message));
                    }

                }

                if (errors.Count == 0)
                {
                    Utility.Instance.ReloadPage();
                }
                else
                {
                    tbControl.UploadDataErrorInfo = new UploadDataResultInfo(errors, sucessAdded);
                }
            }
        }

        private bool Initialized { get; set; }

        private int DynamicNodeBase = 100000000;

        private void Initialize()
        {
            Initialized = true;

            SetLoginControl();

            if (Utility.Instance.IsLogin)
            {
                Label_PersonInfo.Text = string.Format("用户名：{0}", Utility.Instance.CurrentUser.AccountID);
            }
            else
            {
                Label_PersonInfo.Text = string.Empty;
            }

            JTreeNode rootNode;

            TO_GlobalNavTree.GetAllNodes(out rootNode, selectedNavNodeId);



            if (rootNode != null)
            {
                if (!Utility.Instance.IsLogin)
                {
                    Panel1NotLogin.Visible = true;
                    PanelLogin.Visible = false;

                    PanelContentPlaceHolder.Controls.RemoveAt(0);
                    PanelContentPlaceNotLogin.Controls.Add(ContentPlaceHolder1);
                    InitializeAsMenu(rootNode);
                }
                else
                {
                    

                    Panel1NotLogin.Visible = false;
                    PanelLogin.Visible = true;
                    InitializeAsTreeView(rootNode);

                    
                }



            }
            else
            {
                JMenu1.Visible = true;
            }

            if (Utility.Instance.IsAdmin)
            {
                TabContent cnt = new TabContent();
                cnt.TabName = "Admin";
                JTreeViewFancy pesonTree = new JTreeViewFancy() { HideNodeIcon = true };
                JTreeNode configNode = new JTreeNode("权限配置", "权限配置", "", "/ConfigByUI/ConfigNavigateTree.aspx", "");
                if (this.Page is ConfigNavigateTree)
                {
                    configNode.Selected = true;
                    cnt.Selected        = true;
                }

                pesonTree.Nodes.Add(configNode);

                Panel pl = new Panel();
                pl.CssClass = "NavTreeContainer";
                pl.Controls.Add(pesonTree);
                cnt.ItemContent = pl;

                this.Accordion1.TabItem = cnt;
            }
        }
        
        private TreeNode GetNodeByName( string text, TreeNode root )
        {
            foreach (TreeNode nd in root.ChildNodes)
            {
                if (nd.Text == text)
                {
                    return nd;
                }
                else
                {
                    if (nd.ChildNodes != null && nd.ChildNodes.Count > 0)
                    {
                        TreeNode nd2 = GetNodeByName(text, nd);
                        if (nd2 != null)
                        {
                            return nd2; 
                        }
                    }
                }
            }

            return null;
        }

        private void InitializeAsTreeView(JTreeNode rootNode)
        {
            Accordion1[0].Visible = false;


            foreach (TreeNode nd in rootNode.ChildNodes)
            {
                TabContent cnt = new TabContent();
                cnt.ItemContent = new Panel();
                cnt.TabName = nd.Text;
                bool hasSelectedNode = false;
                bool hasAuthority = false;

                JTreeViewFancy tree = new JTreeViewFancy() { HideNodeIcon = true };
                tree.CssClass = "LeftAcordNavTree";


                if (nd.ChildNodes.Count >= 1)
                {

                    foreach (JTreeNode childNode in nd.ChildNodes)
                    {
                        JTreeNode newNode = new JTreeNode(childNode.Text, childNode.Target, childNode.ImageUrl, childNode.NavigateUrl, childNode.ToolTip);
                        newNode.Selected = childNode.Selected;
                        if (newNode.Selected)
                        {
                            hasSelectedNode = true;
                        }

                        newNode.KeepOpening = true;
                        bool isHiddenNode = childNode.IsHidden;

                        AddChildNodes(newNode, childNode.ChildNodes, ref hasSelectedNode, ref isHiddenNode);

                        if (isHiddenNode)
                        {
                            newNode.ChildNodes.Clear();
                        }
                        else
                        {
                            tree.Nodes.Add(newNode);
                        }
                    }


                }
                else
                {
                    JTreeNode itemNode = new JTreeNode(nd.Text, nd.Target, nd.ImageUrl, nd.NavigateUrl, nd.ToolTip);
                    itemNode.Selected = nd.Selected;
                    hasSelectedNode = nd.Selected;
                    tree.Nodes.Add(itemNode);
                }

                Panel pl = new Panel();
                pl.CssClass = "NavTreeContainer";
                pl.Controls.Add(tree);
                cnt.ItemContent = pl;

                if (hasSelectedNode)
                {
                    cnt.Selected = true;
                }

                this.Accordion1.TabItem = cnt;
            }
        }

        private void InitializeAsMenu(JTreeNode rootNode)
        {
            DataTable dtPageAuthorInfos =
                CachingManager.Instance.GetDataTable(TO_HTMLPageAuthorityInfoByUserGroup._MyTableName);



            foreach (TreeNode nd in rootNode.ChildNodes)
            {
                MenuItem item = new MenuItem();
                item.Text = nd.Text;
                item.NavigateUrl = nd.ChildNodes.Count >= 1 ? string.Empty : nd.NavigateUrl;
                bool hasSelectedNode = false;
                bool hasAuthority = false;

                JMenu1.MenuItems.Add(item);
                if (nd.ChildNodes.Count >= 1)
                {
                    foreach (JTreeNode childNode in nd.ChildNodes)
                    {

                        MenuItem subItem = new MenuItem(childNode.Text);
                        subItem.NavigateUrl = childNode.NavigateUrl;

                        subItem.Selected = childNode.Selected;
                        if (subItem.Selected)
                        {
                            hasSelectedNode = true;
                        }

                        bool isHiddenNode = childNode.IsHidden;

                        AddChildMenu(subItem, childNode.ChildNodes, ref hasSelectedNode, ref isHiddenNode);

                        if (isHiddenNode)
                        {
                            subItem.ChildItems.Clear(); ;
                        }
                        else
                        {
                            item.ChildItems.Add(subItem);
                        }
                    }

                }


            }
        }

        private void AddChildMenu(MenuItem rootNode, TreeNodeCollection nodes, ref bool hasSelectedNode, ref bool isHiddenNode)
        {
            foreach (JTreeNode node in nodes)
            {
                MenuItem item = new MenuItem();

                item.Text = node.Text;
                item.NavigateUrl = node.NavigateUrl;

                item.Selected = node.Selected;
                if (item.Selected)
                {
                    hasSelectedNode = true;
                }

                if (!node.IsHidden)
                {
                    isHiddenNode = false;
                }

                bool subIsHidden = node.IsHidden;


                if (node.ChildNodes.Count >= 1)
                {
                    AddChildMenu(item, node.ChildNodes, ref hasSelectedNode, ref subIsHidden);

                    if (subIsHidden)
                    {
                        node.ChildNodes.Clear();
                    }
                }

                if (!subIsHidden)
                {
                    rootNode.ChildItems.Add(item);
                }
            }

        }

        private void AddChildNodes(JTreeNode rootNode, TreeNodeCollection nodes, ref bool hasSelectedNode, ref bool isHiddenNode)
        {
            foreach (JTreeNode node in nodes)
            {
                JTreeNode newNode = new JTreeNode(node.Text, node.Target, node.ImageUrl, node.NavigateUrl, node.ToolTip);
                newNode.Selected = node.Selected;
                if (newNode.Selected)
                {
                    hasSelectedNode = true;
                }

                if (!node.IsHidden)
                {
                    isHiddenNode = false;
                }

                bool subIsHidden = node.IsHidden;

                newNode.KeepOpening = true;



                if (node.ChildNodes.Count >= 1)
                {
                    AddChildNodes(newNode, node.ChildNodes, ref hasSelectedNode, ref subIsHidden);

                    if (subIsHidden)
                    {
                        node.ChildNodes.Clear();
                    }
                }

                if (!subIsHidden)
                {
                    rootNode.ChildNodes.Add(newNode);
                }
            }

        }

        private void SetLoginControl()
        {
            bool showLogin = Utility.Instance.CurrentUser == null;

            this.JLinkButton_Logout.Visible = !showLogin;
            this.JButton_Login.Visible =
                this.LoginForm.Visible = showLogin;


            if (showLogin)
            {
                string js = "$('#LoginForm_ErrorMessage').css('display','none');";
                js = ControlUtility.BuildJSFunction(js);
                js = string.Format("$('#{0}').click({1});", this.JButton_Login.ClientID, js);
                js = ControlUtility.BuildPageLoadedFunctionScript(js);
                this.ScriptManager1.AddJSFunction(js);

                this.LoginForm.DialogControl.AutoOpen = false;
                this.LoginForm.DialogControl.OpenerControl = this.JButton_Login;
                this.LoginForm.DialogControl.CloseAction = JUIEffections.explode;
                this.LoginForm.DialogControl.Title = "请输入用户名和密码：";
                //this.LoginForm.DialogControl.AutoOpen = (bool)Session[GlobalString.Session_IsFirstLogin] || denied;

                DataTable dt = new DataTable();
                dt.Columns.Add("用户名");
                dt.Columns.Add("密码");

                List<JDataTableColumnDefine> columDef = new List<JDataTableColumnDefine>();

                JTextBox textPassword = new JTextBox();
                textPassword.TextType = JTextBoxValidatedType.password;
                textPassword.Required = true;
                columDef.Add(new JDataTableColumnDefine("密码", textPassword));

                this.LoginForm.EditType = SSPUCore.Controls.JDataTableEditType.FullEdit;

                this.LoginForm.ColumnsDefine                          = columDef;
                this.LoginForm.DataColumns                            = dt.Columns;
                //this.LoginForm.AjaxServerFun = new AjaxServerFunDef("Services/TableOpSrv.aspx", "JudgeLogin", "postData");
                this.LoginForm.TargetPage               = "Services/TableOpSrv.aspx";
                this.LoginForm.TargetPageMethod         = "JudgeLogin";
                this.LoginForm.TargetPageMethodParmName = "postData";


                this.LoginForm.ClientScriptForAjaxCallSuccess = "$('form').submit();";
                this.LoginForm.ClientScriptForAjaxCallFaile = "$('#LoginForm_ErrorMessage').text(sa[0][1]);$('#LoginForm_ErrorMessage').css('display','block');";

            }
            else
            {
                this.JLinkButton_Logout.LinkURL = "/Services/DoLogout.aspx";
            }



        }
    }
}