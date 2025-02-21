using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBManager;


namespace MISPrac
{
    public partial class NotLocalizedKeys : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utility.Instance.IsAdmin)
            {
                Dictionary<string, List<LocalizedKeysInfo>> subsidiaryLocalized = new Dictionary<string, List<LocalizedKeysInfo>>();
                Dictionary<string, string> globalLoalized = new Dictionary<string, string>();

                Assembly _assemblyDbMananger;
                _assemblyDbMananger = Assembly.GetAssembly(typeof(DBObjBase));



                foreach (string tableName in DBDataTables.AllTablesName)
                {
                    Type type = _assemblyDbMananger.GetType(string.Format("DBManager.TO_{0}", tableName));

                    DBObjBase obj = (DBObjBase)Activator.CreateInstance(type);

                    foreach (string columnName in obj._MyColumnsArray)
                    {
                        string llstr = SSPUCore.Configuration.SSPULocalization.GetLocalization(tableName, columnName, true);

                        if (llstr == columnName)
                        {
                            llstr = SSPUCore.Configuration.SSPULocalization.GetLocalization(string.Empty, columnName);

                            if (llstr != columnName)//在全局被本地化
                            {
                                if (!globalLoalized.Keys.Contains(columnName))
                                {
                                    globalLoalized.Add(columnName, llstr);
                                }
                            }
                            else //全局和表中均未被本地化
                            {
                                AddSubsidiaryKeys(subsidiaryLocalized, tableName, columnName, columnName);
                            }
                        }
                        else //在表中被本地化
                        {
                            AddSubsidiaryKeys(subsidiaryLocalized, tableName, columnName, llstr);

                        }

                    }
                }


                AppendJS = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
                AppendJS = "<SSPULocalization>";
                AppendJS = "  <!--NOTE *****************************************************************************-->";
                AppendJS = "  <!--If the key is specific to an environment, then env=\"Development\" needs to be added -->";
                AppendJS = "  <!--If the key is specific to a subsidiary, then <subsidiary name=\"SSPU\"> needs to be added -->";
                AppendJS = "  <!--For example, -->";
                AppendJS = "  <!--<subsidiary name=\"UK\"> -->";
                AppendJS = "  <!--   <add key=\"SSPUKey\" env=\"Development\" value=\"14#320\" /> -->";
                AppendJS = "  <!--</subsidiary> -->";
                AppendJS = "  <!--********************************************************************************-->";
                AppendJS = "  ";
                AppendJS = "  <!-- values for env are Development,Staging,Production as in MaryKay.Configuration.EnviromentType	-->";
                AppendJS = "  <!-- default value and non-sub specific go here-->";
                AppendJS = "  <!-- value=\\\"[a-z|A-Z|0-9]+-->";
                AppendJS = "";

                foreach (KeyValuePair<string, string> keyValuePair in globalLoalized)
                {
                    AppendJS = string.Format(" <add key=\"{0}\" value=\"{1}\" />", keyValuePair.Key, keyValuePair.Value);
                }

                foreach (KeyValuePair<string, List<LocalizedKeysInfo>> keyValuePair in subsidiaryLocalized)
                {
                    AppendJS = string.Format(" <subsidiary name=\"{0}\"> ", keyValuePair.Key);
                    foreach (LocalizedKeysInfo localizedKeysInfo in keyValuePair.Value)
                    {
                        AppendJS = string.Format("    <add key=\"{0}\" value=\"{1}\"/>", localizedKeysInfo.ColumnName, localizedKeysInfo.LocalizedName);
                    }
                    AppendJS = string.Format(" </subsidiary> ");
                }

                AppendJS = " </SSPULocalization>";
                MemoryStream ms = new MemoryStream();



                //byte[] bts = Encoding.Default.GetBytes(AppendJS);

                byte[] bts = Encoding.UTF8.GetBytes(AppendJS);
                string fileName = "Localization.config";


                Response.ContentType = "Application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName.Replace(" ", ""));
                Response.BinaryWrite(bts);
                Response.Flush();
                this.Response.End();

            }


        }

        private void AddSubsidiaryKeys(Dictionary<string, List<LocalizedKeysInfo>> subsidiaryLocalized, string tableName, string columnName, string llstr)
        {
            if (!subsidiaryLocalized.Keys.Contains(tableName))
            {
                List<LocalizedKeysInfo> litInfo = new List<LocalizedKeysInfo>();
                subsidiaryLocalized.Add(tableName, litInfo);
            }

            subsidiaryLocalized[tableName].Add(new LocalizedKeysInfo(columnName, llstr));

        }
    }
}