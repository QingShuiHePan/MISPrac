using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBManager;
using Definition;

namespace MISPrac.Services
{
    public partial class DoLogout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Utility.Instance.DoLogout();
            }
            catch (Exception)
            {
                
            }

            Response.Redirect("~/Default.aspx" + string.Format("?{0}={1}", GlobalString.QueryNavID, AuthorityManager.Instance.CurrentNodeID));
        }
    }
}