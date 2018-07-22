using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdglsys.DesktopUtils
{
    /// <summary>
    /// 数据库连接信息
    /// </summary>
    public static class DBInfo
    {
        public static DbHelper.DbContext DB { get; set; }
    }
}
