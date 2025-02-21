using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Definition;
using SSPUCore;
using SSPUCore.Classes;
using SSPUCore.Configuration;
using SSPUCore.Controls;


namespace DBManager
{
    public partial class TO_Table_Sample
    {
        protected override void OnPreEditToDB(EnumDefs.EditType editType)
        {
            base.OnPreEditToDB(editType);

            if (string.IsNullOrEmpty(this.Photo))
            {
                this.PhotoForShow = string.Empty;
            }
            else
            {
                this.PhotoForShow = string.Format("<a href={0} target=_blank><img width=auto height=100px src={0} ></img></a>", this.Photo);
            }

        }
    }

    public partial class DBDataTables
    {
        public static string EmptyTableName
        {
            get { return "None"; }
        }
    }

    public partial class TO_GlobalNavTree : IComparable<TO_GlobalNavTree>, INotEncodeID
    {
        public int CompareTo(TO_GlobalNavTree tree)
        {
            return this.ID.CompareTo(tree.ID);
        }

        protected override void OnPreEditToDB(EnumDefs.EditType editType)
        {
            if (editType == EnumDefs.EditType.Delete)
            {
                if (HasChildNode)
                {
                    throw new Exception("请先删除子节点，再删除该节点");
                }
                else
                {

                }
            }

            //this.NavTo = " ";

            base.OnPreEditToDB(editType);
        }

        public bool HasChildNode
        {
            get
            {
                return CachingManager.Instance.GetTO_ObjByCondition<TO_GlobalNavTree>(
                           Utility.Instance.GetSearchingCondition(_ParentID, this.ID.ToString())) != null;
            }

        }

        public string Tooltip
        {
            get
            {
                return this.Comments;
            }
            set
            {
                this.Comments = value;
            }
        }

        private static int _currentNodeID = 0;
        public static DataTable GetAllNodes(out JTreeNode rootNode, int currentNodeID, bool getAllIfIsAdmin = false)
        {
            _currentNodeID = currentNodeID;

            var auoredNodesIDs = AuoredNodesIDs();

            DataTable dt = CachingManager.Instance.GetDataTable(TO_GlobalNavTree._MyTableName);
            List<TO_GlobalNavTree> list_roInfo = new List<TO_GlobalNavTree>();
            foreach (DataRow dataRow in dt.Rows)
            {
                TO_GlobalNavTree toInfo = new TO_GlobalNavTree();
                toInfo.Parse(dataRow);

                if ( getAllIfIsAdmin && Utility.Instance.IsAdmin)
                {
                    list_roInfo.Add(toInfo);
                }
                else
                {
                    if (toInfo.ParentID == 0 || auoredNodesIDs.Contains(toInfo.ID))
                    {
                        list_roInfo.Add(toInfo);
                    }
                }

            }

            list_roInfo.Sort();
            Dictionary<int, JTreeNode> dic_nodes = new Dictionary<int, JTreeNode>();

            JTreeNode notOrderNodes;
            AddNodes(list_roInfo, ref dic_nodes, out notOrderNodes);

            RemoveHiddenNodes(notOrderNodes);

            OrderedChildNodes(out rootNode, notOrderNodes);
            return dt;
        }

        private static List<int> AuoredNodesIDs()
        {
            TO_HTMLPageAuthorityInfoByUserGroup[] nodesAuthored = AuthorityManager.Instance.CurrentUserAllAuthoredNodesForHTMLPage;
            List<int> auoredNodesIDs = new List<int>();

            if (nodesAuthored != null && nodesAuthored.Length >= 1)
            {
                foreach (var nd in nodesAuthored)
                {
                    auoredNodesIDs.Add(nd.GlobalNavTreeID);
                }
            }



            TO_TableAuthorityInfoByUserGroup[] tbgs = AuthorityManager.Instance.CurrentUserAllAuthoredNodesForTable;
            if (tbgs != null && tbgs.Length >= 1)
            {
                foreach (var tbg in tbgs)
                {
                    auoredNodesIDs.Add(tbg.GlobalNavTreeID);
                }
            }

            TO_HTMLPageAuthorityInfoByUserGroup[] hts = AuthorityManager.Instance.CurrentUserAllAuthoredNodesForHTMLPage;
            if (hts != null && hts.Length >= 1)
            {
                foreach (var tbg in hts)
                {
                    auoredNodesIDs.Add(tbg.GlobalNavTreeID);
                }
            }

            TO_DesingedPageAuthorityInfoByUserGroup[] dps = AuthorityManager.Instance.CurrentUserAllAuthoredNodesForDesignedPage;
            if (dps != null && dps.Length >= 1)
            {
                foreach (var tbg in dps)
                {
                    auoredNodesIDs.Add(tbg.GlobalNavTreeID);
                }
            }

            TO_DocumentPageAuthorityInfoByUserGroup[] dcu = AuthorityManager.Instance.CurrentUserAllAuthoredNodesForDocumentPage;
            if (dcu != null && dcu.Length >= 1)
            {
                foreach (var tbg in dcu)
                {
                    auoredNodesIDs.Add(tbg.GlobalNavTreeID);
                }
            }

            TO_VideoPageAuthorityInfoByUserGroup[] vps = AuthorityManager.Instance.CurrentUserAllAuthoredNodesForVideoPage;
            if (vps != null && vps.Length >= 1)
            {
                foreach (var tbg in vps)
                {
                    auoredNodesIDs.Add(tbg.GlobalNavTreeID);
                }
            }

            return auoredNodesIDs;
        }

        private static void RemoveHiddenNodes(JTreeNode allNodes)
        {

            if (allNodes != null && allNodes.ChildNodes.Count >= 1)
            {
                int count = allNodes.ChildNodes.Count;
                for (int i = 0; i < count; i++)
                {
                    if (((JTreeNode)allNodes.ChildNodes[i]).IsHidden)
                    {
                        allNodes.ChildNodes.RemoveAt(i);
                        i--;
                        count--;
                    }
                    else
                    {
                        RemoveHiddenNodes((JTreeNode)allNodes.ChildNodes[i]);
                    }
                }
            }

        }

        private static void OrderedChildNodes(out JTreeNode newRootNode, JTreeNode oldRootNode)
        {
            if (oldRootNode == null)
            {
                newRootNode = null;
                return;
            }

            newRootNode = new JTreeNode(oldRootNode.Text, oldRootNode.Value, oldRootNode.ImageUrl, oldRootNode.NavigateUrl, oldRootNode.ToolTip);
            newRootNode.KeepOpening = false;
            newRootNode.IsHidden = oldRootNode.IsHidden;
            newRootNode.Selected = oldRootNode.Selected;
            newRootNode.Expanded = oldRootNode.Expanded;

            List<JTreeNode> nodes = new List<JTreeNode>();

            foreach (JTreeNode childNode in oldRootNode.ChildNodes)
            {
                nodes.Add(childNode);
            }
            nodes.Sort();

            foreach (var nd in nodes)
            {
                //JTreeNode newNd = new JTreeNode(nd.Text, nd.Value, nd.ImageUrl, nd.NavigateUrl, nd.Target);
                JTreeNode newNd;
                OrderedChildNodes(out newNd, nd);
                newRootNode.ChildNodes.Add(newNd);
            }
        }

        public static bool IsHideNode(int nodeID, TO_UserGroup[] grops)
        {
            if (Utility.Instance.IsAdmin)
            {
                return false;
            }

            Dictionary<string, string> condition = new Dictionary<string, string>();
            condition.Add(TO_TableAuthorityInfoByUserGroup._GlobalNavTreeID, nodeID.ToString());
            condition.Add(TO_TableAuthorityInfoByUserGroup._UserGroupID, string.Empty);
            bool isHidden = true;
            bool tableAutorDefIsNotNull = false;
            bool HTMLPageAutorDefIsNotNull = false;
            foreach (var toUserGroup in grops)
            {
                condition[TO_TableAuthorityInfoByUserGroup._UserGroupID] = toUserGroup.ID.ToString();

                TO_TableAuthorityInfoByUserGroup tau = CachingManager.Instance.GetTO_ObjByCondition<TO_TableAuthorityInfoByUserGroup>(condition);
                if (tau != null)
                {
                    tableAutorDefIsNotNull = true;
                    if (tau.EditTypeEnum != JDataTableEditType.Hidden)
                    {
                        isHidden = false;
                    }
                }
            }

            if (!isHidden)
            {
                return isHidden;
            }

            condition.Clear();
            condition.Add(TO_HTMLPageAuthorityInfoByUserGroup._GlobalNavTreeID, nodeID.ToString());
            condition.Add(TO_HTMLPageAuthorityInfoByUserGroup._UserGroupID, string.Empty);


            foreach (var toUserGroup in grops)
            {
                condition[TO_HTMLPageAuthorityInfoByUserGroup._UserGroupID] = toUserGroup.ID.ToString();
                TO_HTMLPageAuthorityInfoByUserGroup pau =
                    CachingManager.Instance.GetTO_ObjByCondition<TO_HTMLPageAuthorityInfoByUserGroup>(condition);
                if (pau != null)
                {
                    HTMLPageAutorDefIsNotNull = true;
                    if (pau.EditTypeEnum != EnumDefs.HTMLPageEditType.Hidden)
                    {
                        isHidden = false;
                    }
                }
            }

            if (tableAutorDefIsNotNull || HTMLPageAutorDefIsNotNull)
            {
                return isHidden;
            }

            return false;
        }

        /// <summary>
        /// Include all child's child nodes
        /// </summary>
        public TO_GlobalNavTree[] MyAllChildsNodes
        {
            get
            {
                List<TO_GlobalNavTree> result = new List<TO_GlobalNavTree>();
                GetChildNods(this.ID, ref result);

                return result.ToArray();

            }
        }

        private void GetChildNods(int parentID, ref List<TO_GlobalNavTree> result)
        {
            List<TO_GlobalNavTree> temp = CachingManager.Instance.GetTO_ObjsByCondition<TO_GlobalNavTree>(
                Utility.Instance.GetSearchingCondition(TO_GlobalNavTree._ParentID, parentID.ToString()));
            if (temp != null && temp.Count >= 1)
            {
                foreach (var navTree in temp)
                {
                    GetChildNods(navTree.ID, ref result);
                }

                result.AddRange(temp);
            }
        }

        private static void AddNodes(List<TO_GlobalNavTree> list_roInfo, ref Dictionary<int, JTreeNode> dic_nodes, out JTreeNode rootNode)
        {
            rootNode = null;
            foreach (TO_GlobalNavTree dbNodeData in list_roInfo)
            {
                JTreeNode tnode = null;

                tnode = GetNavTreeNode(dbNodeData);

                if (dbNodeData.ParentID == 0)
                {
                    rootNode = tnode;
                }

                if (!dic_nodes.Keys.Contains(dbNodeData.ID))
                {
                    dic_nodes.Add(dbNodeData.ID, tnode);
                }
                else
                {
                    continue;
                }

                if (dic_nodes.Keys.Contains(dbNodeData.ParentID))
                {
                    dic_nodes[dbNodeData.ParentID].ChildNodes.Add(tnode);
                }
                else
                {
                    if (dbNodeData.ParentID >= 1)
                    {
                        _currentNodeID = dbNodeData.ParentID;
                        if (AddParent(list_roInfo, ref dic_nodes))
                        {
                            dic_nodes[dbNodeData.ParentID].ChildNodes.Add(tnode);
                        }

                    }
                }
            }
        }


        private static bool AddParent(List<TO_GlobalNavTree> list_roInfo, ref Dictionary<int, JTreeNode> dic_nodes)
        {
            TO_GlobalNavTree tree = list_roInfo.Find(ListSearchNode);

            if (tree != null)
            {
                JTreeNode tnode = null;

                tnode = GetNavTreeNode(tree);

                if (!dic_nodes.Keys.Contains(tree.ID))
                {
                    dic_nodes.Add(tree.ID, tnode);
                }

                if (dic_nodes.Keys.Contains(tree.ParentID))
                {
                    dic_nodes[tree.ParentID].ChildNodes.Add(tnode);
                }
                else
                {
                    if (tree.ParentID >= 1)
                    {
                        _currentNodeID = tree.ParentID;
                        AddParent(list_roInfo, ref dic_nodes);
                        dic_nodes[tree.ParentID].ChildNodes.Add(tnode);
                    }
                }

                return true;
            }

            return false;

        }

        private static JTreeNode GetNavTreeNode(TO_GlobalNavTree dbNodeData)
        {
            //string imgUrl = dbNodeData.NavTo.Trim().Length >= 5 ? dbNodeData.NavTo : string.Empty;
            //JTreeNode node = new JTreeNode(tree.Name, "", "", $"?{GlobalString.QueryNavID}={tree.ID}", IsHideNode(tree.ID,AuthorityManager.Instance.CurrentUserGroupInfo)?"0":"1");
            JTreeNode node = new JTreeNode(dbNodeData.Name, dbNodeData.ID.ToString(), dbNodeData.NavTo, 
                String.Format("?{0}={1}&{2}=1",GlobalString.QueryNavID,dbNodeData.ID,GlobalString.QueryIsTreeNodeClicked), dbNodeData.Comments.ToString());
            if ( AuthorityManager.Instance.CurrentUser != null && AuthorityManager.Instance.CurrentUser.IsAdministrator)
            {
                node.ToolTip = dbNodeData.OrderIndex.ToString();
            }
            node.OrderIndex  = dbNodeData.OrderIndex;
            node.KeepOpening = false;
            node.IsHidden    = IsHideNode(dbNodeData.ID, AuthorityManager.Instance.CurrentUserGroupInfo);
            if (dbNodeData.ID == AuthorityManager.Instance.CurrentNodeID)
            {
                node.Selected = true;
                node.Expanded = true;
            }

            return node;
        }

        private static bool ListSearchNode(TO_GlobalNavTree node)
        {
            if (node != null && node.ID == _currentNodeID)
            {
                return true;
            }

            return false;
        }


    }

    public partial class TO_DesingedPageAuthorityInfoByUserGroup:IComparable<TO_DesingedPageAuthorityInfoByUserGroup>
    {
        public int CompareTo(TO_DesingedPageAuthorityInfoByUserGroup that)
        {
            if (this.DisplayOrder < that.DisplayOrder) return -1;
            if (this.DisplayOrder == that.DisplayOrder) return 0;
            return 1;
        }

        protected override void OnPreEditToDB(EnumDefs.EditType editType)
        {
            int    selectedNavNodeId   = AuthorityManager.Instance.CurrentNodeID;
            string selectedTableName   = string.Empty;
            int    selectedUserGroupid = 0;

            if (HttpContext.Current.Session[GlobalString.QueryUserGroupID] != null)
            {
                selectedUserGroupid = (int)HttpContext.Current.Session[GlobalString.QueryUserGroupID];
            }

            if (selectedNavNodeId > 0 && selectedUserGroupid > 0)
            {
                this.GlobalNavTreeID = selectedNavNodeId;
                this.UserGroupID     = selectedUserGroupid;
                
            }
            else
            {
                if (editType != EnumDefs.EditType.Delete)
                {
                    throw new Exception("数据不完整，无法完成操作");
                }
            }

            base.OnPreEditToDB(editType);
        }
    }

    public partial class TO_HTMLPageAuthorityInfoByUserGroup
    {
        protected override void OnPreEditToDB(EnumDefs.EditType editType)
        {
            int selectedNavNodeId = AuthorityManager.Instance.CurrentNodeID;
            string selectedTableName = string.Empty;
            int selectedUserGroupid = 0;

            if (HttpContext.Current.Session[GlobalString.QueryUserGroupID] != null)
            {
                selectedUserGroupid = (int)HttpContext.Current.Session[GlobalString.QueryUserGroupID];
            }

            if (selectedNavNodeId > 0 && selectedUserGroupid > 0)
            {
                this.GlobalNavTreeID = selectedNavNodeId;
                this.UserGroupID = selectedUserGroupid;

                if (editType == EnumDefs.EditType.Add)
                {
                    OnlyOnePriorityFilter();
                    OneNodeTypeFilter();
                }
            }
            else
            {
                if (editType != EnumDefs.EditType.Delete)
                {
                    throw new Exception("数据不完整，无法完成操作");
                }
            }

            //if (this.PageType.Equals("已有页面"))
            //{
            //    if (string.IsNullOrEmpty(this.Navigate2Page))
            //    {
            //        throw new Exception("请输入要跳转的页面！");
            //    }
            //}

            base.OnPreEditToDB(editType);
        }

        private void OneNodeTypeFilter()
        {
            TO_TableAuthorityInfoByUserGroup tbu =
                CachingManager.Instance.GetTO_ObjByCondition<TO_TableAuthorityInfoByUserGroup>(
                    Utility.Instance.GetSearchingCondition(TO_TableAuthorityInfoByUserGroup._GlobalNavTreeID,
                        this.GlobalNavTreeID.ToString()));

            if (tbu != null)
            {
                throw new Exception("只能为节点定义一种展示类型");
            }
        }

        private void OnlyOnePriorityFilter()
        {
            Dictionary<string, string> condition = new Dictionary<string, string>();
            condition.Add(TO_HTMLPageAuthorityInfoByUserGroup._GlobalNavTreeID, this.GlobalNavTreeID.ToString());
            condition.Add(TO_HTMLPageAuthorityInfoByUserGroup._UserGroupID, this.UserGroupID.ToString());

            TO_HTMLPageAuthorityInfoByUserGroup oldValue =
                CachingManager.Instance.GetTO_ObjByCondition<TO_HTMLPageAuthorityInfoByUserGroup>(condition);
            if (oldValue != null)
            {
                throw new Exception("操作不合法，只能添加一种权限");
            }
        }

        public EnumDefs.HTMLPageEditType EditTypeEnum
        {
            get
            {
                EnumDefs.HTMLPageEditType result = EnumDefs.HTMLPageEditType.Hidden;
                if (!string.IsNullOrEmpty(this.EditType))
                {
                    if (!Enum.TryParse(EditType, out result))
                    {
                        if (EditType == EnumDefs.HTMLPageEditType汉字.只读.ToString())
                        {
                            result = EnumDefs.HTMLPageEditType.ReadOnly;
                        }
                        else if (EditType == EnumDefs.HTMLPageEditType汉字.可写.ToString())
                        {
                            result = EnumDefs.HTMLPageEditType.Write;
                        }
                        else
                        {
                            result = EnumDefs.HTMLPageEditType.Hidden;
                        }
                    }
                }

                

                return result;
            }
        }
    }

    public partial class TO_UserGroup
    {
        public static string Administrator
        {
            get
            {
                return "Administrator";
            }
        }

        public static string LoginedUser
        {
            get
            {
                return "LoginedUser";
            }
        }

        public static string NotLoginUser
        {
            get
            {
                return "NotLoginUser";
            }
        }

        private static string[] PreDefinedGroup = new[] { Administrator, LoginedUser, NotLoginUser };
        private static int      _notLoginUserId;
        private        bool     _canAddPreDefindGroup;

        public TO_UserGroup(string name)
        {
            this.Name = name;
            this.Comments = string.Empty;
            this.Enabled = true;
        }

        private bool CanAddPreDefindGroup
        {
            get
            {
                return _canAddPreDefindGroup;
            }
            set
            {
                _canAddPreDefindGroup = value;
            }
        }

        public override DataTable GetDataTableFromDB()
        {
            DataTable tb = base.GetDataTableFromDB();

            if (tb == null || tb.Rows.Count == 0)
            {
                AddPreDefinedGroup();
            }

            return base.GetDataTableFromDB();
        }

        private void AddPreDefinedGroup()
        {
            foreach (var s in PreDefinedGroup)
            {
                TO_UserGroup user = new TO_UserGroup(s);
                user.AddToDB();
            }
        }

        private static int NotLoginUserID
        {
            get
            {
                if (_notLoginUserId == 0)
                {
                    TO_UserGroup gp = CachingManager.Instance.GetTO_ObjByCondition<TO_UserGroup>(Utility.Instance.GetSearchingCondition(TO_UserGroup._Name, NotLoginUser));
                    if (gp != null)
                    {
                        _notLoginUserId = gp.ID;
                    }
                }

                return _notLoginUserId;
            }
        }

        protected override void OnPreEditToDB(EnumDefs.EditType editType)
        {
            foreach (var s in PreDefinedGroup)
            {
                if (s.Equals(this.Name))
                {
                    if (!(editType == EnumDefs.EditType.Add && CanAddPreDefindGroup))
                    {
                        throw new Exception("默认用户组不允许编辑");
                    }
                }
            }

            if (this.InheritFromGroupID == NotLoginUserID)
            {
                throw new Exception("不能继承未登录用户");
            }

            base.OnPreEditToDB(editType);
        }

        public static TO_UserGroup[] AllGroups
        {
            get
            {
                List<TO_UserGroup> result = new List<TO_UserGroup>();
                DataTable dt = CachingManager.Instance.GetDataTable(_MyTableName);

                if (dt != null && dt.Rows.Count >= 1)
                {
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        TO_UserGroup gp = new TO_UserGroup();
                        gp.Parse(dataRow);
                        result.Add(gp);
                    }
                }

                foreach (var s in PreDefinedGroup)
                {
                    bool contains = false;
                    foreach (var toUserGroup in result)
                    {
                        if (s.Equals(toUserGroup.Name))
                        {
                            contains = true;
                            break;
                        }
                    }

                    if (!contains)
                    {
                        TO_UserGroup user = new TO_UserGroup(s);
                        user.CanAddPreDefindGroup = true;
                        user.AddToDB();
                        user.CanAddPreDefindGroup = false;
                        result.Add(user);
                    }
                }

                return result.ToArray();
            }
        }
    }

    public partial class TO_TableColumnAuthorityByUserGroup
    {
        protected override void OnPreEditToDB(EnumDefs.EditType editType)
        {
            object objNavID = HttpContext.Current.Session[GlobalString.QueryNavID];
            if (objNavID != null)
            {
                this.GlobalNavTreeID = (int)objNavID;
            }


            object objGroupID = HttpContext.Current.Session[GlobalString.QueryUserGroupID];
            if (objGroupID != null)
            {
                this.UserGroupID = (int)objGroupID;
            }

            if (objNavID == null || objGroupID == null)
            {
                if (editType != EnumDefs.EditType.Delete)
                {
                    throw new Exception("数据不完整，请重新检查节点数据0x5505");
                }
            }

            {
                string selectedTableName = string.Empty;

                Dictionary<string, string> condition = new Dictionary<string, string>();
                condition.Add(TO_TableAuthorityInfoByUserGroup._GlobalNavTreeID, this.GlobalNavTreeID.ToString());
                condition.Add(TO_TableAuthorityInfoByUserGroup._UserGroupID, this.UserGroupID.ToString());
                TO_TableAuthorityInfoByUserGroup tai = CachingManager.Instance.GetTO_ObjByCondition<TO_TableAuthorityInfoByUserGroup>(condition);

                if (tai != null)
                {
                    this.TableName = tai.TableName;
                }
                else
                {
                    if (editType != EnumDefs.EditType.Delete)
                    {
                        throw new Exception("请先添加数据表权限");
                    }
                }

                if (editType == EnumDefs.EditType.Add)
                {
                    OneFileOnlyOneEditTypeCheck();
                }
                
            }

            base.OnPreEditToDB(editType);
        }

        private static void OnlyOneNodeTypeFilter(int selectedNavNodeId)
        {
            TO_TableAuthorityInfoByUserGroup tbu =
                CachingManager.Instance.GetTO_ObjByCondition<TO_TableAuthorityInfoByUserGroup>(
                    Utility.Instance.GetSearchingCondition(
                        TO_TableAuthorityInfoByUserGroup._GlobalNavTreeID,
                        selectedNavNodeId.ToString()));
            if (tbu != null)
            {
                throw new Exception("只能为节点定义一种展示类型");
            }
        }

        private void OneFileOnlyOneEditTypeCheck()
        {

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(_UserGroupID, this.UserGroupID.ToString());
            dic.Add(_GlobalNavTreeID, this.GlobalNavTreeID.ToString());
            dic.Add(_TableName, this.TableName);
            dic.Add(_TableFieldName, this.TableFieldName);
            TO_TableColumnAuthorityByUserGroup tbInfo =
                CachingManager.Instance.GetTO_ObjByCondition<TO_TableColumnAuthorityByUserGroup>(dic);

            if (tbInfo != null)
            {
                throw new Exception("操作不合法，每个字段只能添加一种权限类型！");
            }
        }
    }

    public partial class TO_UserDef : INotRepeatDef
    {
        public string[] NotRepeatColumns
        {
            get
            {
                return new string[] { _AccountID };
            }
        }

        public bool IsAdministrator
        {
            get
            {
                if (AuthorityManager.Instance.CurrentUserGroupInfo != null)
                {
                    foreach (var gpInfo in AuthorityManager.Instance.CurrentUserGroupInfo)
                    {
                        if (gpInfo.Name.Equals(TO_UserGroup.Administrator))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        protected override void OnPreEditToDB(EnumDefs.EditType editType)
        {

            if (editType != EnumDefs.EditType.Delete)
            {
                if (editType == EnumDefs.EditType.Add)
                {
                    if (this.Password.Length < 32)
                    {
                        this.Password = Utility.Instance.ToMD5(this.Password);
                    }

                    this.Password = this.Password.ToLower();
                }

                if (editType == EnumDefs.EditType.Modify && !(this is TO_UserDefChangePassword))
                {
                    this.Password = ((TO_UserDef)OldValueUseOnlyBeforeModify).Password;
                }
            }

            this.AccountID = this.AccountID.ToUpper();

            base.OnPreEditToDB(editType);
        }
    }

    public partial class TO_UserDefChangePassword : TO_UserDef
    {
        protected override void OnPreEditToDB(EnumDefs.EditType editType)
        {
            base.OnPreEditToDB(editType);
        }

        protected override void OnPreLoadClassFeaturs()
        {
            base.OnPreLoadClassFeaturs();

        }
    }

    public partial class TO_TableAuthorityInfoByUserGroup
    {
        protected override void OnPreEditToDB(EnumDefs.EditType editType)
        {
            int selectedNavNodeId = AuthorityManager.Instance.CurrentNodeID;
            string selectedTableName = AuthorityManager.Instance.CurrentTableName;
            int selectedUserGroupid = 0;

            //if (HttpContext.Current.Session[GlobalString.QueryNavID] != null)
            //{
            //    selectedNavNodeId = (int)HttpContext.Current.Session[GlobalString.QueryNavID];
            //}

            if (HttpContext.Current.Session[GlobalString.QueryUserGroupID] != null)
            {
                selectedUserGroupid = (int)HttpContext.Current.Session[GlobalString.QueryUserGroupID];
            }

            //if (HttpContext.Current.Session[GlobalString.QueryTableName] != null)
            //{
            //    selectedTableName = (string)HttpContext.Current.Session[GlobalString.QueryTableName];
            //}


            if (selectedNavNodeId >= 1 && !string.IsNullOrEmpty(selectedTableName) && selectedUserGroupid >= 1)
            {
                this.GlobalNavTreeID = selectedNavNodeId;
                this.UserGroupID = selectedUserGroupid;
                this.TableName = selectedTableName;

                //if (selectedTableName.Equals(GlobalString.QueryNav2PageNode))
                //{
                //    this.EditTypeEnum = JDataTableEditType.Nav2OtherPage;
                //}
                //else
                //{
                //    if (this.EditTypeEnum == JDataTableEditType.Nav2OtherPage)
                //    {
                //        if (string.IsNullOrEmpty(this.NavURL))
                //        {
                //            throw new Exception("数据不完整，请输入导航页面地址");
                //        }
                //    }
                //}


            }
            else
            {
                if (editType != EnumDefs.EditType.Delete)
                {
                    throw new Exception("数据不完整，无法完成操作");
                }
            }

            if (editType == EnumDefs.EditType.Add)
            {
                TO_TableAuthorityInfoByUserGroup tbInfo = CachingManager.Instance.GetTO_ObjByCondition<TO_TableAuthorityInfoByUserGroup>(this.ID);

                if (tbInfo != null)
                {
                    throw new Exception("只能添加一种权限类型");
                }

            }

            base.OnPreEditToDB(editType);
        }


        public JDataTableEditType EditTypeEnum
        {
            get
            {
                JDataTableEditType result = JDataTableEditType.Hidden;
                if (!string.IsNullOrEmpty(this.EditType))
                {
                    Enum.TryParse(EditType, out result);
                }
                return result;
            }
            set { this.EditType = value.ToString(); }
        }

        

    }
    

    public partial class TO_DocumentPageAuthorityInfoByUserGroup
    {
        protected override void OnPreEditToDB(EnumDefs.EditType editType)
        {
            object objNavID = HttpContext.Current.Session[GlobalString.QueryNavID];
            if (objNavID != null)
            {
                this.GlobalNavTreeID = (int)objNavID;
            }
            

            object objGroupID = HttpContext.Current.Session[GlobalString.QueryUserGroupID] ;
            if (objGroupID != null)
            {
                this.UserGroupID = (int)objGroupID;
            }

            if (objNavID == null || objGroupID == null)
            {
                if (editType != EnumDefs.EditType.Delete)
                {
                    throw new Exception("数据不完整，请重新检查节点数据0x5505");
                }
            }

            if (string.IsNullOrEmpty((this.DocPath + this.Comments).Trim()))
            {
                throw new Exception("文档路径不全，请重新检查节点数据0x5506");
            }


            if (string.IsNullOrEmpty(this.DocPath))
            {
                this.DocPath  = this.Comments;
                this.Comments = string.Empty;
            }

            if (!this.DocPath.EndsWith(".pdf", StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("文件类型不匹配，请上传pdf文档");
            }

            string fileName = DocPath.Substring(DocPath.LastIndexOf('/') + 1);

            this.DocPathForShow = String.Format("<a href='{0}'>{1}</a>",this.DocPath,fileName);

            base.OnPreEditToDB(editType);
        }
    }



    public partial class TO_VideoPageAuthorityInfoByUserGroup
    {
        private Regex RegHighlights
        {
            get
            {
                return  new Regex(@"\d+[\:|\：][^;|；]*[;|；]");
            }

        }

        protected override void OnPreEditToDB(EnumDefs.EditType editType)
        {
            if (editType != EnumDefs.EditType.Delete)
            {
                this.Highlights = this.Highlights.Trim();
                if (!string.IsNullOrEmpty(this.Highlights))
                {
                    if (!RegHighlights.IsMatch(this.Highlights))
                    {
                        throw new Exception("内容标签数据不符合标准，格式标准如下：“数字1:标签1;数字2:标签2;”");
                    }

                }
            }

            CheckIDs(editType);

            CheckVideoFile();

            string[] supportTypes = new[] { ".jpg", ".jpeg", ".png" };
            bool     isSupport    = false;

            foreach (var s in supportTypes)
            {
                if (this.ExsitThumbnailsPath.EndsWith(s, StringComparison.CurrentCultureIgnoreCase))
                {
                    isSupport = true;
                    break;
                }
            }

            if (isSupport)
            {
                this.ThumbnailsPath = this.ExsitThumbnailsPath;
            }

            this.ExsitThumbnailsPath = "";

            isSupport                = false;
            foreach (var s in supportTypes)
            {
                if (!this.ThumbnailsPath.EndsWith(s, StringComparison.CurrentCultureIgnoreCase))
                {
                    isSupport = true;
                    break;
                }
            }

            if (!isSupport)
            {
                throw new Exception("视频缩略图文件类型不匹配");
            }

            string fileName = ThumbnailsPath.Substring(ThumbnailsPath.LastIndexOf('/') + 1);

            this.ThumbnailsPathForShow = String.Format("<a href='{0}'>{1}</a>", this.ThumbnailsPath, fileName);



            base.OnPreEditToDB(editType);
        }

        private void CheckIDs(EnumDefs.EditType editType)
        {
            object objNavID = HttpContext.Current.Session[GlobalString.QueryNavID];
            if (objNavID != null)
            {
                this.GlobalNavTreeID = (int)objNavID;
            }


            object objGroupID = HttpContext.Current.Session[GlobalString.QueryUserGroupID];
            if (objGroupID != null)
            {
                this.UserGroupID = (int)objGroupID;
            }

            if (objNavID == null || objGroupID == null)
            {
                if (editType != EnumDefs.EditType.Delete)
                {
                    throw new Exception("数据不完整，请重新检查节点数据0x5505");
                }
            }
        }

        private void CheckVideoFile()
        {
            string[] supportTypes = new[] { ".mp4", ".ogg", ".webm" };

            bool isSupport = false;
            foreach (var s in supportTypes)
            {
                if (this.Comments.EndsWith(s, StringComparison.CurrentCultureIgnoreCase))
                {
                    isSupport = true;
                    break;
                }
            }

            if(isSupport) 
            {
                this.VideoPath = this.Comments;
            }

            this.Comments = string.Empty;

            isSupport    = false;
            foreach (var s in supportTypes)
            {
                if (!this.VideoPath.EndsWith(s, StringComparison.CurrentCultureIgnoreCase))
                {
                    isSupport = true;
                    break;
                }
            }

            if(!isSupport)
            {
                throw new Exception("文件类型不匹配，请上传.mp4,.ogg,.webm格式的视频文件");
            }

            string fileName = VideoPath.Substring(VideoPath.LastIndexOf('/') + 1);

            this.VideoPathForShow = String.Format("<a href='{0}'>{1}</a>", this.VideoPath, fileName);
        }

        public JArtPlayerThumbnails ThumbnailsObj
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ThumbnailsPath))
                {
                    return new JArtPlayerThumbnails(this.ThumbnailsPath, 60, 10);
                }

                return null;
            }
        }

        public Dictionary<int, string> DicHightlights
        {
            get
            {
                Dictionary<int,string> dic = new Dictionary<int,string>();

                if (this.Highlights != null)
                {
                    if (RegHighlights.IsMatch(this.Highlights))
                    {
                        Regex regNum = new Regex(@"^\d+");
                        

                        foreach (Match m in RegHighlights.Matches(this.Highlights))
                        {
                            string orig = m.Value.Trim();
                            string numStr = regNum.Match(m.Value).Value;

                            string content = m.Value.Substring(numStr.Length);

                            dic.Add(int.Parse(numStr),content);

                        }
                    }
                }

                return dic;
            }
        }
    }


    public partial class TO_TableColumnAuthorityByUserGroup
    {

    }


}
