using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBManager;
using Definition;
using SSPUCore.Configuration;
using SSPUCore.Controls;

namespace MISPrac
{
    public partial class ConfigByTableName : System.Web.UI.Page
    {
        private int selectedNavNodeId = 0;
        private string selectedTableName = "";
        private int selectedUserGroupid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = SSPUSqlHelper.Instance.Get_TableAuthorityDistinctTableName();

            List<string> configedTables = new List<string>();

            if (ds != null && ds.Tables.Count >= 1)
            {
                DataTable dt = ds.Tables[0];

                foreach (DataRow dataRow in dt.Rows)
                {
                    configedTables.Add(dataRow[0].ToString());
                }


            }

            if (configedTables.Count >= 1)
            {
                selectedTableName = HttpContext.Current.Request.QueryString[GlobalString.QueryTableName];

                foreach (var s in configedTables)
                {
                    JTreeNode nd = new JTreeNode(s, s, "/Resource/Image/Icons/TreeNode/TableF.png",
                        string.Format("?{0}={1}",GlobalString.QueryTableName,s), "");

                    if (s.Equals(selectedTableName))
                    {
                        nd.Selected = true;
                    }

                    JTreeView_TableName.Nodes.Add(nd);
                }
            }
            
            if (!string.IsNullOrEmpty(selectedTableName))
            {
                List<TO_TableAuthorityInfoByUserGroup> taus =
                    CachingManager.Instance.GetTO_ObjsByCondition<TO_TableAuthorityInfoByUserGroup>(
                        Utility.Instance.GetSearchingCondition(TO_TableAuthorityInfoByUserGroup._TableName,
                            selectedTableName));

                if (taus != null)
                {
                    selectedNavNodeId = Utility.Instance.GetIDFromQueryString(GlobalString.QueryNavID);
                    
                    
                    List<int> addedIDs = new List<int>();
                    foreach (var tau in taus)
                    {
                        if (!addedIDs.Contains(tau.GlobalNavTreeID))
                        {
                            JTreeNode nd = new JTreeNode(tau.GlobalNavTreeName, tau.GlobalNavTreeName, "", string.Format("?{0}={1}&{2}={3}",GlobalString.QueryTableName,selectedTableName,GlobalString.QueryNavID,tau.GlobalNavTreeID), "");
                            if (selectedNavNodeId == tau.GlobalNavTreeID)
                            {
                                nd.Selected = true;
                            }
                            JTreeView_NavDef.Nodes.Add(nd);

                            addedIDs.Add(tau.GlobalNavTreeID);
                        }
                        
                           
                    }
                }

                
                if (selectedNavNodeId > 0)
                {
                    

                    selectedUserGroupid = Utility.Instance.GetIDFromQueryString(GlobalString.QueryUserGroupID);
                    


                    Dictionary<string,string > condition = new Dictionary<string, string>();
                    condition.Add(TO_TableAuthorityInfoByUserGroup._TableName,selectedTableName);
                    condition.Add(TO_TableAuthorityInfoByUserGroup._GlobalNavTreeID,selectedNavNodeId.ToString());

                    List<TO_TableAuthorityInfoByUserGroup> groupTaus =
                        CachingManager.Instance.GetTO_ObjsByCondition<TO_TableAuthorityInfoByUserGroup>(condition);

                    foreach (var tau in groupTaus)
                    {
                        JTreeNode nd = new JTreeNode(tau.UserGroupName, tau.UserGroupName, "/Resource/Image/Icons/16px/user.png",
                            string.Format("?{0}={1}",GlobalString.QueryNavID,selectedNavNodeId) +
                            string.Format("&{0}={1}",GlobalString.QueryTableName,selectedTableName) +
                            string.Format("&{0}={1}", GlobalString.QueryUserGroupID, tau.UserGroupID), "");

                        if (selectedUserGroupid == tau.UserGroupID)
                        {
                            nd.Selected = true;
                        }

                        JTreeView_UserGroup.Nodes.Add(nd);
                    }
                }
                
            }

            if (selectedNavNodeId >= 1 && !string.IsNullOrEmpty(selectedTableName) && selectedUserGroupid >= 1)
            {
                SetColumAuthorityTable();

                SetTableAuthority();
            }

            SaveQueryInfo2Session();
        }

        private void SaveQueryInfo2Session()
        {
            HttpContext.Current.Session[GlobalString.QueryNavID] = selectedNavNodeId;
            HttpContext.Current.Session[GlobalString.QueryUserGroupID] = selectedUserGroupid;
            HttpContext.Current.Session[GlobalString.QueryTableName] = selectedTableName;
        }

        private void SetTableAuthority()
        {
            Dictionary<string, string> serchDic =
                Utility.Instance.GetSearchingCondition(TO_TableAuthorityInfoByUserGroup._TableName,
                    selectedTableName);
            serchDic.Add(TO_TableAuthorityInfoByUserGroup._GlobalNavTreeID, selectedNavNodeId.ToString());
            serchDic.Add(TO_TableAuthorityInfoByUserGroup._UserGroupID, selectedUserGroupid.ToString());
            string[] columns = new string[]
            {
                DBObjBase._ID, TO_TableAuthorityInfoByUserGroup._TableName, TO_TableAuthorityInfoByUserGroup._EditType
            };
            DataTable dt =
                CachingManager.Instance.GetDataTable(TO_TableAuthorityInfoByUserGroup._MyTableName, serchDic,
                    columns);



            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(TO_TableAuthorityInfoByUserGroup._UserGroupID, selectedUserGroupid.ToString());
            dic.Add(TO_TableAuthorityInfoByUserGroup._GlobalNavTreeID, selectedNavNodeId.ToString());
            dic.Add(TO_TableAuthorityInfoByUserGroup._TableName, selectedTableName);
            if (CachingManager.Instance.GetTO_ObjByCondition<TO_TableAuthorityInfoByUserGroup>(dic) != null)
            {
                this.JDataTableTable.EditType = JDataTableEditType.ModifyAndDelete;
            }

            JDataTableManager.Instance.InitializeJDataTable(this.JDataTableTable,
                TO_TableAuthorityInfoByUserGroup._MyTableName, dt);
        }

        private void SetColumAuthorityTable()
        {
            Dictionary<string, string> serchDic =
                Utility.Instance.GetSearchingCondition(TO_TableColumnAuthorityByUserGroup._TableName,
                    selectedTableName);
            serchDic.Add(TO_TableColumnAuthorityByUserGroup._GlobalNavTreeID, selectedNavNodeId.ToString());
            serchDic.Add(TO_TableColumnAuthorityByUserGroup._UserGroupID, selectedUserGroupid.ToString());
            string[] columns = new string[]
            {
                DBObjBase._ID, TO_TableColumnAuthorityByUserGroup._TableFieldName,
                TO_TableColumnAuthorityByUserGroup._FieldEditType
            };
            DataTable dt =
                CachingManager.Instance.GetDataTable(TO_TableColumnAuthorityByUserGroup._MyTableName, serchDic,
                    columns);
            JDataTableManager.Instance.InitializeJDataTable(this.JDataTable1,
                TO_TableColumnAuthorityByUserGroup._MyTableName, dt);
        }
    }
}