using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBManager;
using Definition;
using SSPUCore.Controls;

namespace MISPrac
{
    public partial class PersonalSetting : System.Web.UI.Page   
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utility.Instance.IsLogin)
            {
                //Utility.Instance.ClearQueryAndSession(GlobalString.QueryNavID);
                
                ThemeRoller1.SelectedIndexChanged += ThemeRoller1_SelectedIndexChanged;

                JDataTable1.EditType = JDataTableEditType.OnlyModify;
                
                JDataTableManager.Instance.InitializeJDataTable(this.JDataTable1, TO_UserDef._MyTableName,
                    Utility.Instance.GetSearchingCondition(TO_UserDef._ID, Utility.Instance.CurrentUser.ID.ToString()));
            }
            
        }

        private void ThemeRoller1_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            TO_UserPrivateData oldValue = CachingManager.Instance.GetTO_ObjByCondition<TO_UserPrivateData>(
                Utility.Instance.GetSearchingCondition(TO_UserPrivateData._UserDefID,
                    Utility.Instance.CurrentUser.ID.ToString()));

            if (oldValue != null)
            {
                oldValue.Theme = ControlUtility.GetJUIStyleType().ToString();
                oldValue.ModifyToDB();
            }
            else
            {
                TO_UserPrivateData upd = new TO_UserPrivateData();
                upd.UserDefID = Utility.Instance.CurrentUser.ID;
                upd.Theme = ControlUtility.GetJUIStyleType().ToString();
                upd.AddToDB();
            }
        }
    }
}