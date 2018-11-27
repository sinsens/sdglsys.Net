namespace sdglsys.DbHelper
{
    public class Rates : DbContext
    {
        /// <summary>
        /// 获取最新的费率信息
        /// </summary>
        /// <returns></returns>
        public Entity.T_Rate GetLast()
        {
            return Db.Queryable<Entity.T_Rate>().OrderBy(a => a.Rate_id, SqlSugar.OrderByType.Desc).Where(a => a.Rate_model_state).First();
        }

        public bool Update(Entity.T_Rate rate)
        {
            if (rate.Rate_id < 1)
                return Add(rate);
            return Db.Updateable(rate).ExecuteCommand() > 0 ? true : false;
        }

        public bool Add(Entity.T_Rate rate)
        {
            return Db.Insertable(rate).ExecuteCommand() > 0 ? true : false;
        }
    }
}