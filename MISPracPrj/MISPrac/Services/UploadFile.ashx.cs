using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MISPrac.Services
{
    /// <summary>
    /// TableCrudSrv 的摘要说明
    /// </summary>
    public class TableCrudSrv : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            SSPUCore.Controls.Uploadify.RunUploadFileOnServerSideLogic(context);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}