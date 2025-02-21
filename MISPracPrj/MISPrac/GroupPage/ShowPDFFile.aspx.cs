using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBManager;
using Definition;
using MISPrac;
using SSPUCore.Controls;

namespace Web.GroupPage
{
    public partial class ShowPDFFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int selectedID = 0;
            if (HttpContext.Current.Session[GlobalString.QueryNavID] == null)
            {
                selectedID = Utility.Instance.GetIDFromQueryString(GlobalString.QueryNavID);
            }
            else
            {
                selectedID = (int)HttpContext.Current.Session[GlobalString.QueryNavID];
            }
            Dictionary<string, List<string>> condition = new Dictionary<string, List<string>>();
            condition.Add(TO_HTMLPageAuthorityInfoByUserGroup._GlobalNavTreeID, new List<string>() { selectedID.ToString() });
            condition.Add(TO_HTMLPageAuthorityInfoByUserGroup._UserGroupID, AuthorityManager.Instance.CurrentUserGroupIDs);

            List<TO_DocumentPageAuthorityInfoByUserGroup> authors = CachingManager.Instance.GetTO_ObjsByCondition<TO_DocumentPageAuthorityInfoByUserGroup>(condition, true);

            if (authors != null)
            {
                if (authors.Count >= 1)
                {
                    int pdfIndex = Utility.Instance.GetIDFromQueryString(GlobalString.QUERYPDFFILEINDEX);
                    if (pdfIndex < 0)
                    {
                        pdfIndex = 0;
                    }

                    if (authors.Count <= 1)
                    {
                        JRadios1.Visible = false;
                    }

                    if (pdfIndex >= 0 && pdfIndex < authors.Count)
                    {
                        string path = authors[pdfIndex].DocPath;
                        if (!string.IsNullOrEmpty(path))
                        {
                            PDFFilePath   = path;
                            JRadios1.Text = authors[pdfIndex].FileTitle;
                        }
                    }



                    for (int i = 0; i < authors.Count; i++)
                    {
                        string qu = "/GroupPage/ShowPDFFile.aspx?" +
                                    String.Format("{0}={1}" ,GlobalString.QueryNavID,HttpContext.Current.Session[GlobalString.QueryNavID])+
                                    String.Format("&{0}={1}", GlobalString.QUERYPDFFILEINDEX, i);

                        //JMenuDocs1.MenuItems.Add( new MenuItem(authors[i].FileTitle,"","",qu) );
                        JRadios1.Items.Add(new ListItem(authors[i].FileTitle,qu));
                    }
                }
               
            }

            //TO_HTMLPageAuthorityInfoByUserGroup pageInfo = AuthorityManager.Instance.CurrentHtmlPageAuthorityInfo;
            //if (pageInfo != null)
            //{
            //    if (!string.IsNullOrEmpty(pageInfo.PDFFile))
            //    {
            //        PDFFilePath = pageInfo.PDFFile;
            //    }
            //}
        }

        public string PDFFilePath { get; set; }
    }
}