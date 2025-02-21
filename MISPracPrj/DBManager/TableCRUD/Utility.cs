//#define _DEBUGUSERLOGINED
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using DBManager;
using Definition;
//using PaddleOCRSharp;
using SSPUCore.Classes;
using SSPUCore.Configuration;
using SSPUCore.Controls;
//using System.Web.Security.FormsAuthentication;


namespace DBManager
{
    public class Utility
    {
        #region Instance

        private                 StringBuilder   _script;
        private static readonly object          _mutex    = new object();
        private static          Utility         _instance = null;
        //private                 PaddleOCREngine _ocrEngine;
        private                 Thread          _threadCheckFile     = null;
        private                 string          _aplicationStartPath = string.Empty;
        //private MD5    _MD5Provider = null;
        private bool _isCheckingFiles = false;
        

        
        private Utility()
        {
            //_MD5Provider = new MD5CryptoServiceProvider();
        }

        public static Utility Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new Utility();
                        }
                    }
                }

                return _instance;
            }
        }

        #endregion

        
        public string AplicationStartPath
        {
            get
            {
                return _aplicationStartPath;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (value != _aplicationStartPath)
                    {
                        _aplicationStartPath = value;
                    }
                    
                }
            }
        }
        

        public bool IsAdmin
        {
            get
            {
                if (this.IsLogin)
                {
                    return CurrentUser.IsAdministrator;
                }

                return false;
            }
        }

        public void ReloadPage()
        {
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.ToString());
        }

        public Dictionary<string, string> GetSearchingCondition(string columnName, string columnValue)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(columnName))
            {
                result.Add(columnName, columnValue);
            }

            return result;
        }
        
        public bool IsLogin
        {
            get { return (CurrentUser != null); }
        }

        public TO_UserDef CurrenToUserInfo
        {
            get
            {
                if (IsLogin)
                {
                    return (TO_UserDef)CurrentUser;
                }

                return new TO_UserDef() { ID = -1, Name = TO_UserGroup.NotLoginUser, AccountID = "未登录用户" };
            }
        }

        public string[] AllUserDefinedTables
        {
            get
            {
                string[] notIncludeTables = SSPUAppSettings.GetConfig("NotEditAuthorityTables").Split(new char[] { ',' });
                List<string> result = new List<string>();

                foreach (var s in DBDataTables.AllTablesName)
                {
                    if (!notIncludeTables.Contains(s))
                    {
                        result.Add(s);
                    }
                }

                return result.ToArray();
            }
        }

        public string GetServerFilePath(string uploadFilePath)
        {
            if (uploadFilePath.Contains("/"))
            {
                return HttpContext.Current.Server.MapPath(uploadFilePath);
            }

            return string.Empty;
        }

        public IUserDef CurrentUser
        {
            get
            {
#if _DEBUGUSERLOGINED
                TO_UserDef user = new TO_UserDef();
                user.Name = "Test";
                user.ID = 1;
                user.AccountID = "Test";
                return user;
#endif

                object obj = HttpContext.Current.Session[GlobalString.Session_UserInfo];
                return (IUserDef)obj;
            }
            set
            {
                if (value == null)
                {
                    DoLogout();
                }
                else
                {
                    HttpContext.Current.Session[GlobalString.Session_UserInfo] = value;
                }
            }
        }


        public bool DoLogout()
        {
            if (IsLogin)
            {
                CachingManager.Instance.RemoveCache(GlobalString.Session_UserInfo + CurrentUser.ID);
                HttpContext.Current.Session.Clear();
            }

            return true;
        }



        public bool RecordLogInfo(EnumDefs.LogType logType, string logName, string logMessage)
        {
            //deleted
            return true;

        }

        public string ToMD5(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            //return BitConverter.ToString( _MD5Provider.ComputeHash( Encoding.Default.GetBytes( str ) ) ).ToLower();

            return MD5Hash(str).ToLower();

            //return HashPasswordForStoringInConfigFile(str, "MD5")?.ToLower();
        }

        public string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        public string GetPageName(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return url;
            }

            if (url.Contains(".aspx"))
            {
                string path = url.Substring(url.LastIndexOf('/') + 1);
                path = path.Substring(0, path.IndexOf(".aspx") + 5);
                return path;
            }

            return string.Empty;
        }

        public string GetAbsPath(string file)
        {
            return AplicationStartPath + file.Replace("/", @"\\");
        }

        public int GetIDFromQueryString(string queryName, string queryString= null)
        {
            Regex reg = new Regex(string.Format(@"{0}\=\d+",queryName));
            
            int result = int.MinValue;
            if (string.IsNullOrEmpty(queryName))
            {
                return result;
            }

            string str = HttpContext.Current.Request.QueryString[queryName];

            if (!string.IsNullOrEmpty(str))
            {
                if (reg.IsMatch(str))
                {
                    Regex regD = new Regex("\\d+");

                    return int.Parse(regD.Match(reg.Match(str).Value).Value);

                }

                if (!string.IsNullOrEmpty(queryString))
                {
                    str = queryString;
                }
            }
            else
            {
                return result;
            }


            if (!int.TryParse(str, out result))
            {
                try
                {
                    result = SSPUCore.SSPUConverter.Instance.DecodingNumber(str);
                }
                catch (Exception)
                {
                    throw new Exception("ID非法！");
                }
            }

            return result;
            //else
            //{
            //    throw new Exception( "ID非法！" );
            //}
        }

        public bool DoLogin( TO_UserDef user )
        {
            object objNavID = HttpContext.Current.Session[GlobalString.QueryNavID];
            HttpContext.Current.Session.Clear();
            if (objNavID != null)
            {
                HttpContext.Current.Session.Add(GlobalString.QueryNavID, objNavID);
            }
            HttpContext.Current.Session.Add(GlobalString.Session_UserInfo, user);

            SetPersonalTheme();

            return true;
        }

        private void SetPersonalTheme()
        {
            TO_UserPrivateData prvData = CachingManager.Instance.GetTO_ObjByCondition<TO_UserPrivateData>(
                Utility.Instance.GetSearchingCondition(TO_UserPrivateData._UserDefID,
                    Utility.Instance.CurrentUser.ID.ToString()));
            if (prvData != null)
            {
                ControlUtility.SetJUIStyleType(prvData.Theme);
            }
        }

        public bool ClearQueryAndSession(string queryName)
        {
            if (HttpContext.Current.Request.QueryString[queryName] != null)
            {
                HttpContext.Current.Request.QueryString[queryName] = null;
            }

            HttpContext.Current.Session[queryName] = null;
            return true;

        }

        public DBObjBase GetTO_ObjByTableName(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return null;
            }
            Assembly assemblyDbMananger;
            assemblyDbMananger = Assembly.GetAssembly(typeof(DBObjBase));
            Type type = assemblyDbMananger.GetType(string.Format("DBManager.TO_{0}", tableName));
            return (DBObjBase)Activator.CreateInstance(type);

        }

        public bool AddNewUserAndSetGroup(string userName, string gender, string accountID, string originalPassword, string userGroupName)
        {
            TO_UserDef user = new TO_UserDef();
            user.Name = userName;
            user.Gender = gender;
            user.AccountID = accountID;
            user.Password = Utility.Instance.ToMD5(originalPassword);

            user.AddToDB();

            TO_UserGroup gp = CachingManager.Instance.GetTO_ObjByCondition<TO_UserGroup>(TO_UserGroup._Name, userGroupName);

            if (gp != null)
            {
                TO_UserWithGroup ngp = new TO_UserWithGroup();
                ngp.UserDefID = user.ID;
                ngp.UserGroupID = gp.ID;

                ngp.AddToDB();

                return true;
            }

            return false;
        }

        public string AppendJS
        {
            get
            {
                if (_script == null)
                {
                    _script = new StringBuilder();
                }

                string s = _script.ToString();
                _script.Remove(0, _script.Length);

                return s;
            }
            set
            {
                if (_script == null)
                {
                    _script = new StringBuilder();
                }

                _script.AppendLine(value);
            }
        }
        
        
    }
}