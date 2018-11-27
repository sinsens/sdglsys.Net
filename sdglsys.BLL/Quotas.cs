namespace sdglsys.DbHelper
{
    public class Quotas : DbContext
    {
        /// <summary>
        /// 获取最新的配置信息
        /// </summary>
        /// <returns></returns>
        public Entity.T_Quota GetLast()
        {
            return Db.Queryable<Entity.T_Quota>().OrderBy(a => a.Quota_id, SqlSugar.OrderByType.Desc).Where(a => a.Quota_model_state).First();
        }

        /// <summary>
        /// 更新基础配额信息
        /// </summary>
        /// <param name="quota"></param>
        /// <returns></returns>
        public bool Update(Entity.T_Quota quota)
        {
            if (quota.Quota_id < 1)
                return Add(quota);
            return Db.Updateable(quota).ExecuteCommand() > 0 ? true : false;
        }

        public bool Add(Entity.T_Quota quota)
        {
            return Db.Insertable(quota).ExecuteCommand() > 0 ? true : false;
        }
    }
}