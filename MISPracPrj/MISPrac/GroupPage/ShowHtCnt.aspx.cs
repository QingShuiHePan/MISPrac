using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBManager;
using Definition;

namespace Web.GroupPage
{
    public partial class ShowHtCnt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            EnumDefs.HTMLPageEditType汉字 editType = AuthorityManager.Instance.CurrentHTMLPageEditType;
            int                         nodeId   = AuthorityManager.Instance.CurrentNodeID;

            if (editType != EnumDefs.HTMLPageEditType汉字.清除权限)
            {
                PanelShowHTMLPage.Visible = true;

                if (editType == EnumDefs.HTMLPageEditType汉字.可写)
                {
                    this.ButtonAddContent.Click += ButtonAddContent_Click;
                    {
                        FileManager1.FileUploadTargetPage = "/Services/Uploadify_FileManager.ashx";

                        FileManager1.TargetPage = "/Services/FileManagerService.aspx";
                        FileManager1.Root       = string.Empty;

                        FileManager1.FileEditTargetPage               = "/Services/FileManagerService.aspx";
                        FileManager1.FileEditTargetPageMethod         = "DataOperator";
                        FileManager1.FileEditTargetPageMethodParmName = "postData";

                    }
                }

                if (!this.IsPostBack)
                {

                    TO_HTMLPageContent cnt = CachingManager.Instance.GetTO_ObjByCondition<TO_HTMLPageContent>(nodeId);

                    if (editType == EnumDefs.HTMLPageEditType汉字.可写)
                    {

                        PanelHTMLEditControl.Visible = PanelHTMLEditJS.Visible = true;
                        TO_HTMLPageContent oldCnt = CachingManager.Instance.GetTO_ObjByCondition<TO_HTMLPageContent>(
                            Utility.Instance.GetSearchingCondition(TO_HTMLPageContent._GlobalNavTreeID,
                                AuthorityManager.Instance.CurrentNodeID.ToString()));
                        if (oldCnt != null)
                        {
                            this.txtarea.Text = oldCnt.HTMLContent;
                        }
                    }

                    if (editType == EnumDefs.HTMLPageEditType汉字.只读)
                    {
                        LabelHTMLContent.Visible = true;

                        TO_HTMLPageContent oldCnt = CachingManager.Instance.GetTO_ObjByCondition<TO_HTMLPageContent>(
                            Utility.Instance.GetSearchingCondition(TO_HTMLPageContent._GlobalNavTreeID,
                                AuthorityManager.Instance.CurrentNodeID.ToString()));
                        if (oldCnt != null)
                        {
                            LabelHTMLContent.Text = oldCnt.HTMLContent;
                        }
                    }
                }

            }
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

                HighLight_EditPageDesignDone.Visible = true;
                HighLight_EditPageDesignDone.Title   = "已修改成功" + DateTime.Now.ToLongTimeString();
            }
            else
            {
                TO_HTMLPageContent cnt = new TO_HTMLPageContent();
                cnt.GlobalNavTreeID = AuthorityManager.Instance.CurrentNodeID;
                cnt.HTMLContent     = html;
                cnt.AddToDB();

                HighLight_EditPageDesignDone.Visible = true;
                HighLight_EditPageDesignDone.Title   = "已添加成功" + DateTime.Now.ToLongTimeString();
            }



        }

    }
}