using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Definition;
using SSPUCore.Controls;

namespace DBManager
{
    public class AuthorityManager
    {
        private static object _mutex = new object();
        private static AuthorityManager _instance = new AuthorityManager();

        private AuthorityManager()
        {

        }

        public static AuthorityManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new AuthorityManager();
                        }
                    }
                }

                return _instance;
            }
        }

        public int CurrentNodeID
        {
            get
            {
                string[] qid = HttpContext.Current.Request.QueryString.GetValues(GlobalString.QueryNavID);
                if (qid != null && qid.Length >= 1)
                {
                    int id = 0;
                    if (int.TryParse(qid[0], out id))
                    {
                        return id;
                    }

                }

                if (HttpContext.Current.Session[GlobalString.QueryNavID] != null)
                {
                    return (int)HttpContext.Current.Session[GlobalString.QueryNavID];
                }

                return 0;
            }
        }

        public string CurrentTableName
        {
            get
            {
                if (CurrentNodeConfigedTableInfo != null)
                {
                    return CurrentNodeConfigedTableInfo.TableName;
                }

                return (string)HttpContext.Current.Session[GlobalString.QueryTableName];

            }
        }

        public TO_TableAuthorityInfoByUserGroup CurrentNodeConfigedTableInfo
        {
            get
            {
                TO_TableAuthorityInfoByUserGroup tai =
                    CachingManager.Instance.GetTO_ObjByCondition<TO_TableAuthorityInfoByUserGroup>(
                        TO_TableAuthorityInfoByUserGroup._GlobalNavTreeID, CurrentNodeID.ToString());

                return tai;
            }
        }

        public TO_TableAuthorityInfoByUserGroup[] CurrentUserAllAuthoredNodesForTable
        {
            get
            {
                TO_TableAuthorityInfoByUserGroup[] result = null;
                object obj = HttpContext.Current.Session[GlobalString.Session_CurrentUserAllAuthoredNodesForTable];
                if (obj == null)
                {
                    Dictionary<string, List<string>> condition = new Dictionary<string, List<string>>();
                    condition.Add(TO_TableAuthorityInfoByUserGroup._UserGroupID, CurrentUserGroupIDs);

                    List<TO_TableAuthorityInfoByUserGroup> authors =
                        CachingManager.Instance.GetTO_ObjsByCondition<TO_TableAuthorityInfoByUserGroup>(condition, true);
                    if (authors != null && authors.Count >= 1)
                    {
                        int max = authors.Count;
                        for (int i = 0; i < max; i++)
                        {
                            if (authors[i] != null && authors[i].EditTypeEnum == JDataTableEditType.Hidden)
                            {
                                authors.RemoveAt(i);
                                i--;
                                max--;
                            }
                        }

                        result = authors.ToArray();

                        HttpContext.Current.Session[GlobalString.Session_CurrentUserAllAuthoredNodesForTable] = result;
                    }
                }
                else
                {
                    result = obj as TO_TableAuthorityInfoByUserGroup[];
                }

                return result;
            }
        }
        
        public TO_HTMLPageAuthorityInfoByUserGroup[] CurrentUserAllAuthoredNodesForHTMLPage
        {
            get
            {

               
                TO_HTMLPageAuthorityInfoByUserGroup[] result = null;
                object obj = HttpContext.Current.Session[GlobalString.Session_CurrentUserAllAuthoredNodesForHTMLPage];
                if (obj == null)
                {
                    Dictionary<string, List<string>> condition = new Dictionary<string, List<string>>();
                    condition.Add(TO_HTMLPageAuthorityInfoByUserGroup._UserGroupID, CurrentUserGroupIDs);
                    List<TO_HTMLPageAuthorityInfoByUserGroup> authors =
                        CachingManager.Instance.GetTO_ObjsByCondition<TO_HTMLPageAuthorityInfoByUserGroup>(condition, true);
                    if (authors != null && authors.Count >= 1)
                    {
                        int max = authors.Count;
                        for (int i = 0; i < max; i++)
                        {
                            if (authors[i] != null && authors[i].EditTypeEnum == EnumDefs.HTMLPageEditType.Hidden)
                            {
                                authors.RemoveAt(i);
                                i--;
                                max--;
                            }
                        }

                        result = authors.ToArray();
                        HttpContext.Current.Session[GlobalString.Session_CurrentUserAllAuthoredNodesForHTMLPage] = result;
                    }
                }
                else
                {
                    result = obj as TO_HTMLPageAuthorityInfoByUserGroup[];
                }

                return result;
            }
        }

        public TO_DesingedPageAuthorityInfoByUserGroup[] CurrentUserAllAuthoredNodesForDesignedPage
        {
            get
            {
                TO_DesingedPageAuthorityInfoByUserGroup[] result = null;
                object obj = HttpContext.Current.Session[GlobalString.Session_CurrentUserAllAuthoredNodesForDesignedPage];
                if (obj == null)
                {
                    Dictionary<string, List<string>> condition = new Dictionary<string, List<string>>();
                    condition.Add(TO_DesingedPageAuthorityInfoByUserGroup._UserGroupID, CurrentUserGroupIDs);
                    List<TO_DesingedPageAuthorityInfoByUserGroup> authors = CachingManager.Instance.GetTO_ObjsByCondition<TO_DesingedPageAuthorityInfoByUserGroup>(condition, true);
                    if (authors != null && authors.Count >= 1)
                    {
                        int max = authors.Count;
                        for (int i = 0; i < max; i++)
                        {
                            if (authors[i] != null )
                            {
                                if (string.IsNullOrEmpty(authors[i].EditType) || authors[i].EditType == JDataTableEditType.Hidden.ToString())
                                {
                                    authors.RemoveAt(i);
                                    i--;
                                    max--;
                                }
                            }
                        }

                        result = authors.ToArray();
                        HttpContext.Current.Session[GlobalString.Session_CurrentUserAllAuthoredNodesForDesignedPage] = result;
                    }
                }
                else
                {
                    result = obj as TO_DesingedPageAuthorityInfoByUserGroup[];
                }

                return result;
            }
        }

        public TO_DocumentPageAuthorityInfoByUserGroup[] CurrentUserAllAuthoredNodesForDocumentPage
        {
            get
            {
                TO_DocumentPageAuthorityInfoByUserGroup[] result = null;
                object obj = HttpContext.Current.Session[GlobalString.Session_CurrentUserAllAuthoredNodesForDocumentPage];
                if (obj == null)
                {
                    Dictionary<string, List<string>> condition = new Dictionary<string, List<string>>();
                    condition.Add(TO_DocumentPageAuthorityInfoByUserGroup._UserGroupID, CurrentUserGroupIDs);
                    List<TO_DocumentPageAuthorityInfoByUserGroup> authors = CachingManager.Instance.GetTO_ObjsByCondition<TO_DocumentPageAuthorityInfoByUserGroup>(condition, true);
                    if (authors != null && authors.Count >= 1)
                    {
                        int max = authors.Count;
                        for (int i = 0; i < max; i++)
                        {
                            if (authors[i] != null && string.IsNullOrEmpty(authors[i].DocPath))
                            {
                                authors.RemoveAt(i);
                                i--;
                                max--;
                            }
                        }

                        result = authors.ToArray();
                        HttpContext.Current.Session[GlobalString.Session_CurrentUserAllAuthoredNodesForDocumentPage] = result;
                    }
                }
                else
                {
                    result = obj as TO_DocumentPageAuthorityInfoByUserGroup[];
                }

                return result;
            }
        }

        public TO_VideoPageAuthorityInfoByUserGroup[] CurrentUserAllAuthoredNodesForVideoPage
        {
            get
            {
                TO_VideoPageAuthorityInfoByUserGroup[] result = null;
                object obj = HttpContext.Current.Session[GlobalString.Session_CurrentUserAllAuthoredNodesForVideoPage];
                if (obj == null)
                {
                    Dictionary<string, List<string>> condition = new Dictionary<string, List<string>>();
                    condition.Add(TO_VideoPageAuthorityInfoByUserGroup._UserGroupID, CurrentUserGroupIDs);
                    List<TO_VideoPageAuthorityInfoByUserGroup> authors = CachingManager.Instance.GetTO_ObjsByCondition<TO_VideoPageAuthorityInfoByUserGroup>(condition, true);
                    if (authors != null && authors.Count >= 1)
                    {
                        int max = authors.Count;
                        for (int i = 0; i < max; i++)
                        {
                            if (authors[i] != null && string.IsNullOrEmpty(authors[i].VideoPath))
                            {
                                authors.RemoveAt(i);
                                i--;
                                max--;
                            }
                        }

                        result = authors.ToArray();
                        HttpContext.Current.Session[GlobalString.Session_CurrentUserAllAuthoredNodesForVideoPage] = result;
                    }
                }
                else
                {
                    result = obj as TO_VideoPageAuthorityInfoByUserGroup[];
                }

                return result;
            }
        }


        public TO_HTMLPageAuthorityInfoByUserGroup CurrentHtmlPageAuthorityInfo
        {
            get
            {
                //Dictionary<string,List<string>> condition = new Dictionary<string, List<string>>();
                //condition.Add(TO_HTMLPageAuthorityInfoByUserGroup._GlobalNavTreeID, new List<string>(){ CurrentNodeID.ToString() });

                //condition.Add(TO_HTMLPageAuthorityInfoByUserGroup._UserGroupID, CurrentUserGroupIDs);


                //List<TO_HTMLPageAuthorityInfoByUserGroup> pgs =
                //    CachingManager.Instance.GetTO_ObjsByCondition<TO_HTMLPageAuthorityInfoByUserGroup>( condition, true );

                //string nav2Page = string.Empty;
                //string editType = string.Empty;

                EnumDefs.HTMLPageEditType editTypeResult = EnumDefs.HTMLPageEditType.Hidden;
                int nodeID = CurrentNodeID;
                Dictionary<string, List<string>> condition = new Dictionary<string, List<string>>();
                condition.Add(TO_HTMLPageAuthorityInfoByUserGroup._GlobalNavTreeID, new List<string>() { nodeID.ToString() });
                condition.Add(TO_HTMLPageAuthorityInfoByUserGroup._UserGroupID, CurrentUserGroupIDs);

                List<TO_HTMLPageAuthorityInfoByUserGroup> authors =
                    CachingManager.Instance.GetTO_ObjsByCondition<TO_HTMLPageAuthorityInfoByUserGroup>(condition, true);
                if (authors != null && authors.Count >= 1)
                {

                    Int32 authorType = 0;
                    EnumDefs.HTMLPageEditType edit;
                    string nav2Page = string.Empty;
                    string pdfFile = string.Empty;
                    foreach (var author in authors)
                    {
                        if (Enum.TryParse(author.EditType, out edit))
                        {
                            authorType |= (int)edit;
                        }

                        
                    }

                    if ((authorType & (int)EnumDefs.HTMLPageEditType.Write) != 0)
                    {
                        editTypeResult = EnumDefs.HTMLPageEditType.Write;
                    }
                    else if ((authorType & (int)EnumDefs.HTMLPageEditType.ReadOnly) != 0)
                    {
                        editTypeResult = EnumDefs.HTMLPageEditType.ReadOnly;
                    }
                    else if (((authorType & (int)EnumDefs.HTMLPageEditType.Hidden) != 0))
                    {
                        editTypeResult = EnumDefs.HTMLPageEditType.Hidden;
                    }

                    TO_HTMLPageAuthorityInfoByUserGroup result = new TO_HTMLPageAuthorityInfoByUserGroup();
                    //result.Navigate2Page = nav2Page;
                    result.EditType = editTypeResult.ToString();
                    //result.PDFFile = pdfFile;
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        public TO_UserDef CurrentUser
        {
            get
            {
                if (Utility.Instance.IsLogin)
                {
                    return (TO_UserDef)Utility.Instance.CurrentUser;
                }

                return null;
            }
        }

        //public JDataTableEditType CurrentTableAuthority
        //{
        //    get
        //    {
        //        TO_UserWithGroup ug = CachingManager.Instance.GetTO_ObjByCondition<TO_UserWithGroup>( TO_UserWithGroup. );
        //    }
        //}


        public TO_UserGroup[] CurrentUserGroupInfo
        {
            get
            {
                if (Utility.Instance.IsLogin)
                {
                    Dictionary<int, TO_UserGroup> dicResult = null;
                    object obj = HttpContext.Current.Session[GlobalString.Session_CurrentUserGroupInfo];
                    if (obj == null)
                    {
                        dicResult = new Dictionary<int, TO_UserGroup>();

                        var myGroupInfo = CachingManager.Instance.GetTO_ObjsByCondition<TO_UserWithGroup>(
                            Utility.Instance.GetSearchingCondition(TO_UserWithGroup._UserDefID, CurrentUser.ID.ToString()));

                        if (myGroupInfo != null)
                        {
                            foreach (var gp in myGroupInfo)
                            {
                                TO_UserGroup ugp = CachingManager.Instance.GetTO_ObjByCondition<TO_UserGroup>(
                                    Utility.Instance.GetSearchingCondition(TO_UserGroup._ID, gp.UserGroupID.ToString()));

                                if (!dicResult.Keys.Contains(ugp.ID))
                                {
                                    dicResult.Add(ugp.ID, ugp);
                                    FindInheritsGroups(ugp, ref dicResult);
                                }
                            }
                        }

                        TO_UserGroup logInGp = CachingManager.Instance.GetTO_ObjByCondition<TO_UserGroup>(
                            TO_UserGroup._Name, TO_UserGroup.LoginedUser);
                        if (!dicResult.Keys.Contains(logInGp.ID))
                        {
                            dicResult.Add(logInGp.ID, logInGp);
                        }

                        HttpContext.Current.Session[GlobalString.Session_CurrentUserGroupInfo] = dicResult;
                    }
                    else
                    {
                        dicResult = obj as Dictionary<int, TO_UserGroup>;
                    }

                    if (dicResult != null)
                    {
                        return dicResult.Values.ToArray();
                    }
                    else
                    {
                        return null;
                    }
                    
                }

                return new TO_UserGroup[] { CachingManager.Instance.GetTO_ObjByCondition<TO_UserGroup>(TO_UserGroup._Name, TO_UserGroup.NotLoginUser) };
            }
        }

        private void FindInheritsGroups(TO_UserGroup gp, ref Dictionary<int, TO_UserGroup> dicResult)
        {
            if (dicResult == null)
            {
                dicResult = new Dictionary<int, TO_UserGroup>();
            }

            if (gp != null && gp.InheritFromGroupID >= 1)
            {
                TO_UserGroup iherGp = CachingManager.Instance.GetTO_ObjByCondition<TO_UserGroup>(
                       Utility.Instance.GetSearchingCondition(TO_UserGroup._ID, gp.InheritFromGroupID.ToString()));

                if (iherGp != null)
                {
                    if (!dicResult.Keys.Contains(iherGp.ID))
                    {
                        dicResult.Add(iherGp.ID, iherGp);
                    }

                    if (iherGp.InheritFromGroupID >= 1)
                    {
                        FindInheritsGroups(iherGp, ref dicResult);
                    }
                }


            }
        }


        public List<string> CurrentUserGroupNames
        {
            get
            {
                TO_UserGroup[] gps = CurrentUserGroupInfo;
                List<string> result = new List<string>();
                if (gps != null && gps.Length >= 1)
                {
                    foreach (var toUserGroup in gps)
                    {
                        result.Add(toUserGroup.Name);
                    }
                }

                return result;
            }
        }

        public List<string> CurrentUserGroupIDs
        {
            get
            {
                TO_UserGroup[] gps = CurrentUserGroupInfo;
                List<string> result = new List<string>();
                if (gps != null && gps.Length >= 1)
                {
                    foreach (var toUserGroup in gps)
                    {
                        result.Add(toUserGroup.ID.ToString());
                    }
                }

                return result;
            }
        }

        public int[] CurrentUserGroupIDsInt
        {
            get
            {
                TO_UserGroup[] gps    = CurrentUserGroupInfo;
                List<int>   result = new List<int>();
                if (gps != null && gps.Length >= 1)
                {
                    foreach (var toUserGroup in gps)
                    {
                        result.Add(toUserGroup.ID);
                    }
                }

                return result.ToArray();
            }
        }



        //public bool CurrentUrlHasAuthor
        //{
        //    get
        //    {


        //        //if (CurrentUser != null && CurrentUser.IsAdministrator)
        //        //{
        //        //    return true;
        //        //}
        //        //string path = HttpContext.Current.Request.Url.LocalPath;
        //        //path = path.Substring(path.LastIndexOf('/') + 1);
        //        //path = path.Substring(0,path.IndexOf(".aspx")+5);

        //        string path = Utility.Instance.GetPageName(HttpContext.Current.Request.Url.LocalPath);

        //        bool? permit = null;
        //        List<TO_HTMLPageAuthorityInfoByUserGroup> pages = CachingManager.Instance.GetTO_ObjsEnabled<TO_HTMLPageAuthorityInfoByUserGroup>();

        //        foreach (TO_HTMLPageAuthorityInfoByUserGroup auP in pages)
        //        {
        //            if (auP.Navigate2Page.Trim().StartsWith(path))
        //            {
        //                if (permit == null)
        //                {
        //                    permit = false;
        //                }
        //                if (CurrentUserGroupIDsInt.Contains(auP.UserGroupID))
        //                {
        //                    bool allvalueOK = true;
        //                    if (auP.Navigate2Page.Contains('?'))
        //                    {
        //                        NameValueCollection clns = HttpUtility.ParseQueryString(auP.Navigate2Page.Substring(auP.Navigate2Page.IndexOf('?')));

        //                        if (clns != null)
        //                        {

        //                            foreach (var cl in clns)
        //                            {
        //                                string s = cl.ToString();
        //                                if (s.Contains('?'))
        //                                {
        //                                    s = s.Substring(s.IndexOf('?') + 1);
        //                                }

        //                                if (HttpContext.Current.Request.QueryString.AllKeys.Contains(s))
        //                                {
        //                                    if (clns[s] != HttpContext.Current.Request.QueryString[s])
        //                                    {
        //                                        allvalueOK = false;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }

        //                    if (allvalueOK)
        //                    {
        //                        permit = true;
        //                        break;
        //                    }


        //                }
        //            }

        //        }

        //        if (permit == null)
        //        {
        //           permit = true;
        //        }



        //        return permit.Value;
        //    }
        //}

        //public JDataTableEditType CurrentDataPageEditType
        //{
        //    get
        //    {
        //        int rights = GetJDataTableEditTypeByAllRightsInt(TO_TableAuthorityInfoByUserGroup._MyTableName);

        //        return GetJDataTableEditTypeByAllRightsInt(rights);
        //    }
        //}

        public JDataTableEditType GetJDataTableEditTypeByAllRights(string tableName, out DBObjBase[] objs)
        {
           

            Dictionary<string, List<string>> condition = new Dictionary<string, List<string>>();
            condition.Add(TO_TableColumnAuthorityByUserGroup._GlobalNavTreeID, new List<string>() { AuthorityManager.Instance.CurrentNodeID.ToString() });
            condition.Add(TO_TableColumnAuthorityByUserGroup._UserGroupID, CurrentUserGroupIDs);

            DataTable dt     = CachingManager.Instance.GetDataTable(tableName, condition, null, true);
            int       result = 0;
            if (dt == null || dt.Rows.Count == 0)
            {
                objs = null;
                return JDataTableEditType.Hidden;
            }

            List<DBObjBase> outObjs = new List<DBObjBase>();

            JDataTableEditType tempET = JDataTableEditType.None;
            foreach (DataRow row in dt.Rows)
            {
                object obj = row[TO_TableAuthorityInfoByUserGroup._EditType];
                if (obj != null)
                {
                    if (Enum.TryParse(obj.ToString(), out tempET))
                    {
                        result |= (int)tempET;
                    }
                }

                DBObjBase dbObj = Utility.Instance.GetTO_ObjByTableName(tableName);
                dbObj.Parse(row);
                outObjs.Add(dbObj);
            }

            objs = outObjs.ToArray();

            return GetJDataTableEditTypeByAllRightsInt(result);
        }

        private int GetJDataTableEditTypeByAllRightsInt(string tableName)
        {
            Dictionary<string, List<string>> condition = new Dictionary<string, List<string>>();
            condition.Add(TO_TableColumnAuthorityByUserGroup._GlobalNavTreeID, new List<string>() { AuthorityManager.Instance.CurrentNodeID.ToString() });
            condition.Add(TO_TableColumnAuthorityByUserGroup._UserGroupID, CurrentUserGroupIDs);

            DataTable dt     = CachingManager.Instance.GetDataTable(tableName, condition, null, true);
            int       result = 0;
            if (dt == null || dt.Rows.Count == 0)
            {
                return result;
            }

            JDataTableEditType tempET = JDataTableEditType.None;
            foreach (DataRow row in dt.Rows)
            {
                object obj = row[TO_TableAuthorityInfoByUserGroup._EditType];
                if (obj != null)
                {
                    if (Enum.TryParse(obj.ToString(), out tempET))
                    {
                        result |= (int)tempET;
                    }
                }
            }

            return result;
        }

        private JDataTableEditType GetJDataTableEditTypeByAllRightsInt(int tmpI)
        {
            int result = tmpI;

            if (tmpI <= 0)
            {
                return JDataTableEditType.None;
            }

            if ((result & ((int)JDataTableEditType.FullEdit)) == (int)JDataTableEditType.FullEdit)
            {
                return JDataTableEditType.FullEdit;
            }

            if ((result & ((int)JDataTableEditType.AddAndModify)) == (int)JDataTableEditType.AddAndModify)
            {
                return JDataTableEditType.AddAndModify;
            }

            if ((result & ((int)JDataTableEditType.AddAndDelete)) == (int)JDataTableEditType.AddAndDelete)
            {
                return JDataTableEditType.AddAndDelete;
            }

            if ((result & ((int)JDataTableEditType.ModifyAndDelete)) == (int)JDataTableEditType.ModifyAndDelete)
            {
                return JDataTableEditType.ModifyAndDelete;
            }
                
            if ((result & ((int)JDataTableEditType.OnlyDelete)) == (int)JDataTableEditType.OnlyDelete)
            {
                return JDataTableEditType.OnlyDelete;
            }

            if ((result & ((int)JDataTableEditType.OnlyModify)) == (int)JDataTableEditType.OnlyModify)
            {
                return JDataTableEditType.OnlyModify;
            }

            if ((result & ((int)JDataTableEditType.OnlyAdd)) == (int)JDataTableEditType.OnlyAdd)
            {
                return JDataTableEditType.OnlyAdd;
            }


            if ((result & ((int)JDataTableEditType.DenyEdit)) == (int)JDataTableEditType.DenyEdit)
            {
                return JDataTableEditType.DenyEdit;
            }

            return JDataTableEditType.Hidden;
        }


        public EnumDefs.HTMLPageEditType汉字 CurrentHTMLPageEditType
        {
            get
            {
                EnumDefs.HTMLPageEditType result = EnumDefs.HTMLPageEditType.Hidden;
                int nodeID = AuthorityManager.Instance.CurrentNodeID;
                Dictionary<string, List<string>> condition = new Dictionary<string, List<string>>();
                condition.Add(TO_HTMLPageAuthorityInfoByUserGroup._GlobalNavTreeID, new List<string>() { nodeID.ToString() });
                condition.Add(TO_HTMLPageAuthorityInfoByUserGroup._UserGroupID, CurrentUserGroupIDs);

                List<TO_HTMLPageAuthorityInfoByUserGroup> authors =
                    CachingManager.Instance.GetTO_ObjsByCondition<TO_HTMLPageAuthorityInfoByUserGroup>(condition, true);

                Int32                       authorType = 0;
                EnumDefs.HTMLPageEditType汉字 edit;
                foreach (var author in authors)
                {
                    if (Enum.TryParse(author.EditType, out edit))
                    {
                        authorType |= (int)edit;
                    }
                }

                if ((authorType & (int)EnumDefs.HTMLPageEditType汉字.可写) != 0)
                {
                    return EnumDefs.HTMLPageEditType汉字.可写;
                }

                if ((authorType & (int)EnumDefs.HTMLPageEditType汉字.只读) != 0)
                {
                    return EnumDefs.HTMLPageEditType汉字.只读;
                }

                return EnumDefs.HTMLPageEditType汉字.清除权限;

            }
        }
        
        public JDataTableEditType CurrentDesignPageEdityType
        {
            get
            {
                int rights = GetJDataTableEditTypeByAllRightsInt(TO_DesingedPageAuthorityInfoByUserGroup._MyTableName);

                return GetJDataTableEditTypeByAllRightsInt(rights);
            }
        }

        public JDataTableEditType CurrentTableEditType
        {
            get
            {
                int rights = GetJDataTableEditTypeByAllRightsInt(TO_TableAuthorityInfoByUserGroup._MyTableName);

                return GetJDataTableEditTypeByAllRightsInt(rights);

                //JDataTableEditType result = JDataTableEditType.Hidden;

                //int nodeId = AuthorityManager.Instance.CurrentNodeID;

                //Dictionary<string, List<string>> condition = new Dictionary<string, List<string>>();
                //condition.Add(TO_TableAuthorityInfoByUserGroup._GlobalNavTreeID, new List<string>() { nodeId.ToString() });
                //condition.Add(TO_TableAuthorityInfoByUserGroup._UserGroupID, CurrentUserGroupIDs);

                //List<TO_TableAuthorityInfoByUserGroup> authors =
                //    CachingManager.Instance.GetTO_ObjsByCondition<TO_TableAuthorityInfoByUserGroup>(condition, true);

                //List<JDataTableEditType> eds = new List<JDataTableEditType>();
                //Int32 authorValue = 0;
                //foreach (var author in authors)
                //{
                //    JDataTableEditType edt;

                //    if (Enum.TryParse(author.EditType, true, out edt))
                //    {
                //        authorValue |= (int)edt;
                //    }
                //}

                ////if ((authorValue & (int)JDataTableEditType.OnlyAdd) != 0)
                ////{
                ////    if ((authorValue & (int) JDataTableEditType.OnlyModify) != 0)
                ////    {
                ////        if ((authorValue & (int)JDataTableEditType.OnlyDelete) != 0)
                ////        {
                ////            result  = JDataTableEditType.FullEdit;
                ////        }
                ////    }
                ////}


                //if ((authorValue & (int)JDataTableEditType.FullEdit) != 0)
                //{
                //    return (JDataTableEditType)(authorValue & (int)JDataTableEditType.FullEdit);
                //}
                //else if ((authorValue == (int)JDataTableEditType.DenyEdit))
                //{
                //    if (authors.Count == 0)
                //    {
                //        return JDataTableEditType.Hidden;
                //    }

                //    return JDataTableEditType.DenyEdit;
                //}
                //else if ((authorValue & (int)JDataTableEditType.Hidden) != 0)
                //{
                //    return JDataTableEditType.Hidden;
                //}
                //else if ((authorValue & (int)JDataTableEditType.Nav2OtherPage) != 0)
                //{
                //    return JDataTableEditType.Nav2OtherPage;
                //}

                //return JDataTableEditType.Hidden;
            }
        }

        public Dictionary<string, int> CurrentColumnsEditType
        {
            get
            {
                int nodeId = AuthorityManager.Instance.CurrentNodeID;

                Dictionary<string, List<string>> condition = new Dictionary<string, List<string>>();
                condition.Add(TO_TableColumnAuthorityByUserGroup._GlobalNavTreeID, new List<string>() { nodeId.ToString() });
                List<string> myGroups = new List<string>();
                if (!Utility.Instance.IsLogin)
                {
                    myGroups.Add(TO_UserGroup.NotLoginUser);
                }
                else
                {
                    TO_UserGroup[] gps = CurrentUserGroupInfo;
                    foreach (var gp in gps)
                    {
                        myGroups.Add(gp.Name);
                    }
                }
                condition.Add(TO_TableColumnAuthorityByUserGroup._TableName, new List<string>() { AuthorityManager.Instance.CurrentTableName });
                condition.Add(TO_TableColumnAuthorityByUserGroup._UserGroupName, myGroups);

                List<TO_TableColumnAuthorityByUserGroup> authors =
                    CachingManager.Instance.GetTO_ObjsByCondition<TO_TableColumnAuthorityByUserGroup>(condition, true);
                Dictionary<string, int> result = new Dictionary<string, int>();
                EnumDefs.FieldAuthority edType = EnumDefs.FieldAuthority.Hidden;

                foreach (var aut in authors)
                {
                    if (result.ContainsKey(aut.TableFieldName))
                    {
                        if (Enum.TryParse(aut.FieldEditType, out edType))
                        {
                            result[aut.TableFieldName] |= (int)edType;
                        }
                    }
                    else
                    {
                        if (Enum.TryParse(aut.FieldEditType, out edType))
                        {
                            result.Add(aut.TableFieldName, (int)edType);
                        }
                    }
                }

                return result;
            }
        }

        public Dictionary<string, ColumnFeatureDefine> CurrentColumnsEditTypeEnum
        {
            get
            {
                Dictionary<string, int> clmAuths = AuthorityManager.Instance.CurrentColumnsEditType;
                Dictionary<string, ColumnFeatureDefine> result = new Dictionary<string, ColumnFeatureDefine>();
                if (clmAuths != null && clmAuths.Count >= 1)
                {
                    foreach (var auth in clmAuths)
                    {
                        if ((auth.Value & (int)EnumDefs.FieldAuthority.EditAllowNull) != 0)
                        {
                            result.Add(auth.Key, ColumnFeatureDefine.EditedButNotRequried);
                        }
                        else if ((auth.Value & (int)EnumDefs.FieldAuthority.EditedNotNull) != 0)
                        {
                            result.Add(auth.Key, ColumnFeatureDefine.Null);
                        }
                        else if ((auth.Value & (int)EnumDefs.FieldAuthority.SumAndEdited) != 0)
                        {
                            result.Add(auth.Key, ColumnFeatureDefine.ToSum | ColumnFeatureDefine.EditedButNotRequried);
                        }
                        else if ((auth.Value & (int)EnumDefs.FieldAuthority.Sum) != 0)
                        {
                            result.Add(auth.Key, ColumnFeatureDefine.ToSum);
                        }
                        else if ((auth.Value & (int)EnumDefs.FieldAuthority.ReadOnly) != 0)
                        {
                            result.Add(auth.Key, ColumnFeatureDefine.NoEditedButVisible);
                        }
                        else if ((auth.Value == (int)EnumDefs.FieldAuthority.Hidden))
                        {
                            result.Add(auth.Key, ColumnFeatureDefine.InvisibleAndNotEdited);
                        }
                        else if (auth.Value == (int)EnumDefs.FieldAuthority.EditableButInvisible)
                        {
                            result.Add(auth.Key, ColumnFeatureDefine.EditableButInvisible);
                        }
                    }
                }

                return result;
            }
        }

        public EnumDefs.PageType GetNodeConfigedPageType(int navNodeID)
        {
            DBObjBase obj = null;
            return GetNodeConfigedPageType(navNodeID, out obj);
        }

        public EnumDefs.PageType GetNodeConfigedPageType(int navNodeID, out DBObjBase obj)
        {
            Dictionary<string,string > dic = new Dictionary<string,string>();
            dic.Add(TO_TableAuthorityInfoByUserGroup._GlobalNavTreeID,navNodeID.ToString());


            string[] tableNames = new[]
            {
                TO_HTMLPageAuthorityInfoByUserGroup._MyTableName, TO_DesingedPageAuthorityInfoByUserGroup._MyTableName,
                TO_DocumentPageAuthorityInfoByUserGroup._MyTableName, TO_VideoPageAuthorityInfoByUserGroup._MyTableName,
                TO_TableAuthorityInfoByUserGroup._MyTableName
            };

            obj = null;

            string result = string.Empty;
            foreach (string tableName in tableNames)
            {
                DataTable dt = CachingManager.Instance.GetDataTable(tableName, dic);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = tableName;

                    obj = Utility.Instance.GetTO_ObjByTableName(tableName);
                    obj.Parse(dt.Rows[0]);

                    break;
                }
            }

            if (result == TO_HTMLPageAuthorityInfoByUserGroup._MyTableName)
            {
                return EnumDefs.PageType.HtmlPage;
            }
            else if (result == TO_DesingedPageAuthorityInfoByUserGroup._MyTableName)
            {
                return EnumDefs.PageType.DesignedPage;
            }
            else if (result == TO_DocumentPageAuthorityInfoByUserGroup._MyTableName)
            {
                return EnumDefs.PageType.ShowDocument;
            }
            else if (result == TO_VideoPageAuthorityInfoByUserGroup._MyTableName)
            {
                return EnumDefs.PageType.ShowMeida;
            }
            else if (result == TO_TableAuthorityInfoByUserGroup._MyTableName)
            {
                return EnumDefs.PageType.TableData;
            }

            return EnumDefs.PageType.None;
        }

        

    }
}
