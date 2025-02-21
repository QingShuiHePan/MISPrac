using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class TO_UserDef:DBObjBase,IUserDef
    {
        public virtual DateTime LoginTime { get; set; }
    }
}
