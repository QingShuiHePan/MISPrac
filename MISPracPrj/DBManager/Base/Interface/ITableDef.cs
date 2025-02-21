using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public interface ITableDef
    {
        int ID { get; set; }
        DateTime CreatedTime { get; set; }
        int CreatorOperatorID { get; set; }
        int LastOperatorID { get; set; }
        DateTime LastUpdateTime { get; set; }
        
        string Comments { get; set; }
        bool Enabled { get; set; }

        //string _ID { get; }

        //string _CreatedTime { get; }
        //string _CreatorOperatorID
        //{
        //    get;
        //}
        //string _LastOperatorID
        //{
        //    get;
        //}
        //string _LastUpdateTime
        //{
        //    get;
        //}
        //string _Enabled
        //{
        //    get;
        //}

        //string _Comments
        //{
        //    get;
        //}

    }
}
