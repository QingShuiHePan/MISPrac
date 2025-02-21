using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Definition
{
    [Serializable]
    public class TableUIDef
    {

        public TableUIBase UIInfo { get; set; }

    }

    [Serializable]
    public class TableUIBase
    {
        public bool Required { get; set; }
        public string ErrorInfo { get; set; }
    }

    [Serializable]
    public enum TableUITextBoxType
    {
        Normal,
        Int,
        Double,
        Regex,
    }
    [Serializable]
    public class TableUITextBox:TableUIBase
    {
        public TableUITextBoxType TextBoxType { get; set; }
        public string RegexStr { get; set; }

    }

    [Serializable]
    public class TableUITextBoxNumber : TableUITextBox
    {
        public string Min { get; set; }
        public string Max { get; set; }
    }

    [Serializable]
    public class TableUITextBoxNumberInnerAssociate:TableUITextBoxNumber
    {
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }

        public string CalculateType1With2 { get; set; }

        public string CalculateType12With3 { get; set; }

        public TableUITextBoxNumberWrite2Out WriteOutDef { get; set; }

    }

    [Serializable]
    public class TableUITextBoxNumberReadOut2Inner : TableUITextBoxNumberWrite2Out
    {

    }

    [Serializable]
    public class TableUITextBoxNumberWrite2Out : TableUITextBoxNumber
    {
        public string AssociatTableName { get; set; }
        public string AssociatTableColumnName { get; set; }
        public string AssociatTableForeignID { get; set; }
        public string AssociateCurrentID { get; set; }
        public string ChangedRate { get; set; }
    }

    [Serializable]
    public class TableUIJDropDown : TableUIBase
    {
        public string ItemsStr { get; set; }
    }

    [Serializable]
    public class TableUIUploadify : TableUIBase
    {
        public string FileDescription { get; set; }
        public string FileAllowTypes { get; set; }
    }

    [Serializable]
    public enum TableUIFileRenderType
    {
        Image,
        LinkURL,
    }
    [Serializable]
    public class TableUIFileRender : TableUIBase
    {
        public string DataSourceColumnName { get; set; }

        public TableUIFileRenderType RenderType { get; set; }

        public string ImageWidth { get; set; }
        public string ImageHeight { get; set; }

        public string LinkText {get;set;}
    }

    [Serializable]
    public class TableUIInputByTree : TableUIBase
    {
        public bool EnableMultiSelect { get; set; }
        public string NodesSourceText { get; set; }
        public string NodesSourceTableName { get; set; }

        public string TableFilterString { get; set; }
    }
}