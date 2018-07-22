using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sdglsys.DbHelper
{
    public class Rates:DbContext
    {
        /// <summary>
        /// 获取最新的费率信息
        /// </summary>
        /// <returns></returns>
        public Entity.TRate getLast() {
            return Db.Queryable<Entity.TRate>().OrderBy(a=>a.Id,SqlSugar.OrderByType.Desc).First();
        }

        public bool Update(Entity.TRate rate) {
            if (rate.Id < 1)
                return Add(rate);
            return Db.Updateable(rate).ExecuteCommand()>0?true:false;
        }

        public bool Add(Entity.TRate rate)
        {
            return Db.Insertable(rate).ExecuteCommand() > 0 ? true : false;
        }
    }
}