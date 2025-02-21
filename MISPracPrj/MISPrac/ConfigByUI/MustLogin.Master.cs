using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBManager;
using SSPUCore.Controls;



namespace MISPrac
{
    public partial class MustLogin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetLoginControl();

            SetJDataTableLoadExcelHandler();

            if (!Utility.Instance.IsAdmin)
            {
                this.PanelContent.Visible = false;
            }
            else
            {
                this.PanelContent.Visible = true;
                InitializeNavTree();
            }

            if (Utility.Instance.IsLogin)
            {
                Label_PersonInfo.Text = string.Format("用户名：{0}", Utility.Instance.CurrentUser.AccountID);
            }
            else
            {
                Label_PersonInfo.Text = string.Empty;
            }
        }

        private void InitializeNavTree()
        {
            JTreeNode ndUser = new JTreeNode("用户管理", "用户管理", "/Resource/Image/Icons/TreeNode/user.png", "/ConfigByUI/EditUserInfo.aspx", "");
            JTreeNode ndUserGroup = new JTreeNode("用户组管理", "用户组管理", "/Resource/Image/Icons/TreeNode/group.png", "/ConfigByUI/ConfigUserGroup.aspx", "");
            JTreeNode ndGroupWithUser = new JTreeNode("分配用户权限", "分配用户权限", "/Resource/Image/Icons/TreeNode/clients.png", "/ConfigByUI/ConfigUserWithGroup.aspx", "");
            JTreeNode ndNav = new JTreeNode("数据权限管理", "数据权限管理", "/Resource/Image/Icons/TreeNode/database.png", "/ConfigByUI/ConfigNavigateTree.aspx", "");
            JTreeNode ndTableSetting = new JTreeNode("数据表UI管理", "数据表UI管理", "/Resource/Image/Icons/TreeNode/table.png", "/ConfigByUI/ConfigTableUI.aspx", "");
            JTreeNode ndSearch = new JTreeNode("权限查询", "权限查询", "/Resource/Image/Icons/TreeNode/search.png", "/ConfigByUI/ConfigByTableName.aspx", "");
            JTreeNode ndLocalize = new JTreeNode("下载本地化文件", "下载本地化文件", "/Resource/Image/Icons/TreeNode/place.png", "/ConfigByUI/NotLocalizedKeys.aspx", "");
            JTreeNode ndHome = new JTreeNode("返回数据页面", "返回数据页面", "/Resource/Image/Icons/TreeNode/previous.png", "/default.aspx", "");



            if (this.Page is ConfigNavigateTree)
            {
                ndNav.Selected = true;
            }
            else if (this.Page is ConfigUserGroup)
            {
                ndUserGroup.Selected = true;
            }
            else if (this.Page is ConfigUserWithGroup)
            {
                ndGroupWithUser.Selected = true;
            }
            else if (this.Page is EditUserInfo)
            {
                ndUser.Selected = true;
            }
            else if( this.Page is NotLocalizedKeys )
            {
                ndLocalize.Selected = true;
            }else if ( this.Page is ConfigByTableName)
            {
                ndSearch.Selected = true;
            }
            else if (this.Page is ConfigTableUI)
            {
                ndTableSetting.Selected = true;
            }

            JTreeViewNav.Nodes.Add(ndUser);
            JTreeViewNav.Nodes.Add(ndUserGroup);
            JTreeViewNav.Nodes.Add(ndGroupWithUser);
            JTreeViewNav.Nodes.Add(ndTableSetting);
            JTreeViewNav.Nodes.Add(ndNav);
            JTreeViewNav.Nodes.Add(ndSearch);
            JTreeViewNav.Nodes.Add(ndLocalize);
            JTreeViewNav.Nodes.Add(ndHome);
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
                LoginForm.AutoOpen                                    = true;


                this.LoginForm.ClientScriptForAjaxCallSuccess = "$('form').submit();";
                this.LoginForm.ClientScriptForAjaxCallFaile = "$('#LoginForm_ErrorMessage').text(sa[0][1]);$('#LoginForm_ErrorMessage').css('display','block');";

            }
            else
            {
                this.JLinkButton_Logout.LinkURL = "/Services/DoLogout.aspx";
            }



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

    }
}