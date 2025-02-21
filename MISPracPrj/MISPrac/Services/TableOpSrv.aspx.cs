using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBManager;
using Definition;
using SSPUCore.Controls;

namespace MISPrac.Services
{
    public partial class TableOpSrv : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string JDataTableOperatorMethod(string postData)
        {
            return JDataTableManager.Instance.ExcuteCommandReturnJDatatableResult(postData);
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string JudgeLogin(string postData)
        {
            if (HttpContext.Current.Session["TryLoginTimesNumber"] != null && (int)HttpContext.Current.Session["TryLoginTimesNumber"] >= 3)
            {
                return JDataTable.SetFailedPostbackDataToClient("输入错误次数已经超过三次，请过会儿再试！", null);
            }

            Dictionary<string, string> postDic = ControlUtility.ParseUserInputToDataRow(postData);

            Dictionary<string, string> condition = new Dictionary<string, string>();
            
            condition.Add(TO_UserDef._AccountID, postDic["用户名"].ToUpper());
            condition.Add(TO_UserDef._Password, postDic["密码"]);
            TO_UserDef user = new TO_UserDef();
            DataTable dt = CachingManager.Instance.GetDataTable(user.MyTableNameInDB, condition);



            if (dt != null && dt.Rows.Count >= 1)
            {
                
                user.Parse(dt.Rows[0]);
                if (!user.Enabled)
                {
                    return JDataTable.SetFailedPostbackDataToClient("改用户已被禁用！", null);
                }

                

                Utility.Instance.DoLogin( user );
            }
            else
            {
                if (HttpContext.Current.Session["TryLoginTimesNumber"] == null)
                {
                    HttpContext.Current.Session["TryLoginTimesNumber"] = 1;
                }
                else
                {
                    int times = (int)HttpContext.Current.Session["TryLoginTimesNumber"];
                    times++;
                    HttpContext.Current.Session["TryLoginTimesNumber"] = times;

                    if (times >= 3)
                    {
                        return JDataTable.SetFailedPostbackDataToClient("输入错误次数已经超过三次！", null);
                    }
                }

                return JDataTable.SetFailedPostbackDataToClient("用户名或密码错误！", null);
            }

            return JDataTable.SetSuccessPostbackDataToClient(null);
        }

       
    }
}