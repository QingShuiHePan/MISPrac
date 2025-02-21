using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSPUCore.Controls;

namespace MISPrac.Service
{
    public partial class FileManagerService : System.Web.UI.Page
    {
        private static string rootPath = SSPUCore.Configuration.SSPUAppSettings.GetConfig("UploadfileFolder");

        //private static string deleteToPath = SSPUCore.Configuration.SSPUAppSettings.GetConfig("UploadfileFolder");

        protected void Page_Load( object sender, EventArgs e )
        {
            //FileManager.CallDirectoryHandler( this , rootPath );
            FileManager.CallDirectoryHandler(this, SSPUCore.Configuration.SSPUAppSettings.GetConfig("UploadfileFolder"));
        }

        [System.Web.Services.WebMethod( EnableSession = true )]
        public static string DataOperator( string postData )
        {
            string deleteToPath = rootPath.Substring(0, rootPath.LastIndexOf('\\')) + "\\DeleteFiles";
            return FileManager.FileManager_DealWithAjaxData( postData, rootPath, deleteToPath );
        }

        
    }
}