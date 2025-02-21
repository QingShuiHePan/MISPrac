using DBManager;
using Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSPUCore.Controls;

namespace Web.GroupPage
{
    public partial class ShowVides : System.Web.UI.Page
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

            condition.Add(TO_VideoPageAuthorityInfoByUserGroup._GlobalNavTreeID, new List<string>() { selectedID.ToString() });
            //condition.Add(TO_VideoPageAuthorityInfoByUserGroup._GlobalNavTreeID, new List<string>() { HttpContext.Current.Session[GlobalString.QueryNavID].ToString() });
            condition.Add(TO_VideoPageAuthorityInfoByUserGroup._UserGroupID, AuthorityManager.Instance.CurrentUserGroupIDs);

            List<TO_VideoPageAuthorityInfoByUserGroup> authors = CachingManager.Instance.GetTO_ObjsByCondition<TO_VideoPageAuthorityInfoByUserGroup>(condition, true);

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

                    for (int i = 0; i < authors.Count; i++)
                    {
                        string qu = "/GroupPage/ShowVides.aspx?" +
                                    String.Format("{0}={1}",GlobalString.QueryNavID,HttpContext.Current.Session[GlobalString.QueryNavID])+
                                    String.Format("&{0}={1}", GlobalString.QUERYPDFFILEINDEX, i);

                        //JRadios1.Items.Add(new MenuItem(authors[i].FileTitle, "", "", qu));
                        JRadios1.Items.Add(new ListItem(authors[i].FileTitle, qu));
                    }

                    if (pdfIndex < authors.Count)
                    {
                        string path = authors[pdfIndex].VideoPath;
                        if (!string.IsNullOrEmpty(path))
                        {
                            this.JArtPlayer1.URLVideo = path;
                            if (authors[pdfIndex].DicHightlights != null && authors[pdfIndex].DicHightlights.Count >= 1)
                            {
                                JArtPlayer1.HighlightContent = authors[pdfIndex].DicHightlights;
                            }

                            JArtPlayer1.ThumbnailInfo = authors[pdfIndex].ThumbnailsObj;
                            JRadios1.Text             = authors[pdfIndex].FileTitle;
                            
                        }
                    }




                }

            }
            else
            {
                JArtPlayer1.Visible = false;
            }
        }
    }
}