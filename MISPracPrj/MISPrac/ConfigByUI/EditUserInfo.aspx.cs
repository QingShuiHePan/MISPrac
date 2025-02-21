using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBManager;

namespace MISPrac
{
    public partial class EditUserInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JDataTableManager.Instance.InitializeJDataTable(this.JDataTable1,TO_UserDef._MyTableName);
        }
    }
}