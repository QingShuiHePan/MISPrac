
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public interface IUserDef:ITableDef
    {
        string Name { get; set; }
        string Gender { get; set; }
        string AccountID { get; set; }
        string Password { get; set; }
        
        DateTime LoginTime { get; set; }

        bool IsAdministrator { get; }
    }
}
