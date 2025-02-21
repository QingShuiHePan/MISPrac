using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBManager;
using Definition;
using SSPUCore.Controls;

namespace MISPrac.GroupPage
{
    public partial class ShowTables : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            //if (!this.IsPostBack)
            //{
            //    int nodeId = AuthorityManager.Instance.CurrentNodeID;

            //    EnumDefs.HTMLPageEditType editType = AuthorityManager.Instance.CurrentHTMLPageEditType;

            //    JDataTableEditType edType = AuthorityManager.Instance.CurrentTableEditType;

            //    if (edType == JDataTableEditType.Hidden || edType == JDataTableEditType.None)
            //    {

            //    }

            //    if (editType != EnumDefs.HTMLPageEditType.Hidden)
            //    {
            //        JDataTable1.Visible = false;
            //        PanelShowHTMLPage.Visible = true;

            //        if (editType == EnumDefs.HTMLPageEditType.Write)
            //        {
            //            this.ButtonAddContent.Click += ButtonAddContent_Click;
            //        }

            //        if (!this.IsPostBack)
            //        {

            //            TO_HTMLPageContent cnt = CachingManager.Instance.GetTO_ObjByCondition<TO_HTMLPageContent>(nodeId);

            //            if (editType == EnumDefs.HTMLPageEditType.Write)
            //            {

            //                PanelHTMLEditControl.Visible = PanelHTMLEditJS.Visible = true;
            //                TO_HTMLPageContent oldCnt = CachingManager.Instance.GetTO_ObjByCondition<TO_HTMLPageContent>(
            //                    Utility.Instance.GetSearchingCondition(TO_HTMLPageContent._GlobalNavTreeID,
            //                        AuthorityManager.Instance.CurrentNodeID.ToString()));
            //                if (oldCnt != null)
            //                {
            //                    this.txtarea.Text = oldCnt.HTMLContent;
            //                }
            //            }

            //            if (editType == EnumDefs.HTMLPageEditType.ReadOnly)
            //            {
            //                LabelHTMLContent.Visible = true;

            //                TO_HTMLPageContent oldCnt = CachingManager.Instance.GetTO_ObjByCondition<TO_HTMLPageContent>(
            //                    Utility.Instance.GetSearchingCondition(TO_HTMLPageContent._GlobalNavTreeID,
            //                        AuthorityManager.Instance.CurrentNodeID.ToString()));
            //                if (oldCnt != null)
            //                {
            //                    LabelHTMLContent.Text = oldCnt.HTMLContent;
            //                }
            //            }
            //        }

            //    }
            //    else
            //    {
            //        TO_TableAuthorityInfoByUserGroup tbInfo =
            //            CachingManager.Instance.GetTO_ObjByCondition<TO_TableAuthorityInfoByUserGroup>(
            //                TO_TableAuthorityInfoByUserGroup._GlobalNavTreeID, nodeId.ToString());
            //        if (tbInfo != null)
            //        {
            //            InitializeJDataTable(tbInfo);
            //        }
            //    }
            //}
            //else
            //{
            //    if (AuthorityManager.Instance.CurrentHTMLPageEditType == EnumDefs.HTMLPageEditType.Write)
            //    {
            //        this.ButtonAddContent.Click += ButtonAddContent_Click;
            //    }
            //}
            
        }


        private void ButtonAddContent_Click(object sender, EventArgs e)
        {
            string html = this.txtarea.Text;

            TO_HTMLPageContent oldCnt = CachingManager.Instance.GetTO_ObjByCondition<TO_HTMLPageContent>(
                Utility.Instance.GetSearchingCondition(TO_HTMLPageContent._GlobalNavTreeID,
                    AuthorityManager.Instance.CurrentNodeID.ToString()));
            if (oldCnt != null)
            {
                oldCnt.HTMLContent = html;
                oldCnt.ModifyToDB();
            }
            else
            {
                TO_HTMLPageContent cnt = new TO_HTMLPageContent();
                cnt.GlobalNavTreeID = AuthorityManager.Instance.CurrentNodeID;
                cnt.HTMLContent = html;
                cnt.AddToDB();
            }

            
            
        }

        private void InitializeJDataTable(TO_TableAuthorityInfoByUserGroup tbInfo)
        {
            if (tbInfo != null)
            {
                JDataTableEditType edt = AuthorityManager.Instance.CurrentTableEditType;

                if ((edt & JDataTableEditType.Hidden) != 0)
                {
                    this.JDataTable1.Visible = false;
                }
                else
                {
                    this.JDataTable1.EditType = edt;

                    string searchString = string.Empty;

                    foreach (var instanceCurrentUserGroupID in AuthorityManager.Instance.CurrentUserGroupIDs)
                    {
                        Dictionary<string, string> serchDic =
                            Utility.Instance.GetSearchingCondition(TO_TableAuthorityInfoByUserGroup._TableName,
                                tbInfo.TableName);
                        serchDic.Add(TO_TableAuthorityInfoByUserGroup._GlobalNavTreeID, tbInfo.GlobalNavTreeID.ToString());
                        serchDic.Add(TO_TableAuthorityInfoByUserGroup._UserGroupID, instanceCurrentUserGroupID);
                       
                        

                        DataTable dt =
                            CachingManager.Instance.GetDataTable(TO_TableAuthorityInfoByUserGroup._MyTableName, serchDic);

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
                    }



                    //Dictionary<string, string> dic = new Dictionary<string, string>();
                    //dic.Add(TO_TableAuthorityInfoByUserGroup._UserGroupID, tbInfo.UserGroupID.ToString());
                    //dic.Add(TO_TableAuthorityInfoByUserGroup._GlobalNavTreeID, tbInfo.GlobalNavTreeID.ToString());
                    //dic.Add(TO_TableAuthorityInfoByUserGroup._TableName, tbInfo.MyTableNameInDB);

                    if (!string.IsNullOrEmpty(searchString) && searchString.Length >= 3)
                    {
                        DataTable dt = CachingManager.Instance.GetDataTable(tbInfo.TableName);
                        dt = CachingManager.Instance.GetDataTable(tbInfo.TableName);
                        dt.DefaultView.RowFilter = searchString;
                        JDataTableManager.Instance.InitializeJDataTable(this.JDataTable1,tbInfo.TableName, dt.DefaultView.ToTable("dt"));
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