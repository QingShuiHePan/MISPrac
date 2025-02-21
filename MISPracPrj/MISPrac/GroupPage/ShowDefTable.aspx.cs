using DBManager;
using Definition;
using SSPUCore.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.GroupPage
{
    public partial class ShowDefTable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int nodeId = AuthorityManager.Instance.CurrentNodeID;

            JDataTableEditType edType = AuthorityManager.Instance.CurrentTableEditType;



            if (edType == JDataTableEditType.Hidden || edType == JDataTableEditType.None)
            {
                JDataTable1.Visible = false;
                return;
            }

            this.JDataTable1.EditType = edType;
            this.JDataTable1.ShowDialogItemDetailControl = true;

            TO_TableAuthorityInfoByUserGroup tbInfo = CachingManager.Instance.GetTO_ObjByCondition<TO_TableAuthorityInfoByUserGroup>(
                    TO_TableAuthorityInfoByUserGroup._GlobalNavTreeID, nodeId.ToString());
            if (tbInfo != null)
            {
                InitializeJDataTable(tbInfo);
            }



        }

        private void InitializeJDataTable(TO_TableAuthorityInfoByUserGroup tbInfo)
        {
            if (tbInfo != null)
            {
                
                {
                    

                    string searchString = string.Empty;

                    foreach (var instanceCurrentUserGroupID in AuthorityManager.Instance.CurrentUserGroupIDs)
                    {
                        Dictionary<string, string> serchDic = Utility.Instance.GetSearchingCondition(TO_TableAuthorityInfoByUserGroup._TableName, tbInfo.TableName);
                        serchDic.Add(TO_TableAuthorityInfoByUserGroup._GlobalNavTreeID, tbInfo.GlobalNavTreeID.ToString());
                        serchDic.Add(TO_TableAuthorityInfoByUserGroup._UserGroupID, instanceCurrentUserGroupID);

                        DataTable dt = CachingManager.Instance.GetDataTable(TO_TableAuthorityInfoByUserGroup._MyTableName, serchDic);

                        if (dt.Rows.Count >= 1)
                        {
                            TO_TableAuthorityInfoByUserGroup au = new TO_TableAuthorityInfoByUserGroup();
                            au.Parse(dt.Rows[0]);

                            if (!string.IsNullOrEmpty(au.DefDataFilterStr))
                            {
                                searchString += "(" + au.DefDataFilterStr + ")" + " or ";
                            }
                        }

                    }

                    if (searchString.Length >= 4)
                    {
                        searchString = searchString.Remove(searchString.Length - 4);

                        Regex reg = new Regex(@"\s*My\.ID", RegexOptions.IgnoreCase);

                        searchString = reg.Replace(searchString, String.Format("{0}", AuthorityManager.Instance.CurrentUser.ID));
                    }


                    if (!string.IsNullOrEmpty(searchString) && searchString.Length >= 3)
                    {
                        DataTable dt = CachingManager.Instance.GetDataTable(tbInfo.TableName);
                        dt = CachingManager.Instance.GetDataTable(tbInfo.TableName);
                        searchString = searchString.Replace(@"""", "'");
                        dt.DefaultView.RowFilter = searchString;
                        JDataTableManager.Instance.InitializeJDataTable(this.JDataTable1, tbInfo.TableName, dt.DefaultView.ToTable("dt"));
                    }
                    else
                    {

                        JDataTableManager.Instance.InitializeJDataTable(this.JDataTable1, tbInfo.TableName);
                    }


                }
            }

        }


    }
}