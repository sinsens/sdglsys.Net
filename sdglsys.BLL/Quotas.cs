using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sdglsys.DbHelper
{
    public class Quotas : DbContext
    {
        /// <summary>
        /// 获取最新的配置信息
        /// </summary>
        /// <returns></returns>
        public Entity.TQuota getLast()
        {
            return Db.Queryable<Entity.TQuota>().OrderBy(a => a.Id, SqlSugar.OrderByType.Desc).First();
        }

        /// <summary>
        /// 更新基础配额信息
        /// </summary>
        /// <param name="quota"></param>
        /// <returns></returns>
        public bool Update(Entity.TQuota quota)
        {
            if (quota.Id < 1)
                return Add(quota);
            return Db.Updateable(quota).ExecuteCommand() > 0 ? true : false;
        }

        public bool Add(Entity.TQuota quota)
        {
            return Db.Insertable(quota).ExecuteCommand() > 0 ? true : false;
        }
    }
}