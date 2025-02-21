namespace DBManager
{
    public class LocalizedKeysInfo
    {
        public string ColumnName { get; set; }
        public string LocalizedName { get; set; }

        public LocalizedKeysInfo(string columnName, string localizedName)
        {
            this.ColumnName = columnName;
            this.LocalizedName = localizedName;
        }
    }
}