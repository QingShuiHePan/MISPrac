using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Definition
{
    public class EnumDefs
    {
        public enum ErrorsDef
        {
            UNKONWN = 0,
            AjaxPostBackError,
        }

        public enum TOTableType
        {
            Table,
            View,
        }

        public enum LogType
        {
            Unknown = 0,
            LogInSuccess,
            LogInFailed,
            ChangeAuthority,
        }

        public enum AssetsTableNameType
        {
            UNKONWN = 0,
            AssetsFixedBorrow,
            AssetsLowValueBorrow,
            AssetsOfficeBorrow,
        }
        
        //NOTE DO NOT CHANGE THE ENUM ORDER 
        public enum EditType
        {
            None = 0,
            Add  = 0x1,
            Modify = 0x2,
            Delete = 0x4,
        }

        public enum HTMLPageEditType
        {
            //None = 0,
            Hidden = 1,
            ReadOnly = 2,
            Write    = 4,
        }

        public enum HTMLPageEditType汉字
        {
            //None = 0,
            清除权限 = 1,
            只读   = 2,
            可写   = 4,
        }


        public enum FieldAuthority
        {
            Hidden   = 0 ,
            ReadOnly = 1 ,
            EditAllowNull  = 2 ,
            EditedNotNull = 4,
            Sum          = 8,
            SumAndEdited = 0x10,
            EditableButInvisible = 0x20,
        }

        public enum ThesisFileType
        {
            None = 0,
            任务书 =1,
            开题报告,
            中期检查,
            论文,
            学生手册,
        }


        public enum PageType
        {
            None         = 0,
            TableData    = 1,
            HtmlPage     = 2,
            DesignedPage = 4,
            ShowDocument = 8,
            ShowMeida    = 16,
        }

    }

}
