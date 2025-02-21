using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using SSPUCore.Controls;

namespace MISPrac.Service
{
    /// <summary>
    /// Uploadify_FileManager 的摘要说明
    /// </summary>
    public class Uploadify_FileManager : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest( HttpContext context )
        {
            string fileName = FileManager.MethodCallForUploadify( context );

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