using System;

namespace sdglsys.DbHelper
{
    /// <summary>
    /// 已登录用户信息
    /// </summary>
    public class Token : DbContext
    {
        /// <summary>
        /// 清除过期的Token
        /// </summary>
        public void DistroyExpiredToken()
        {
            Db.Deleteable<sdglsys.Entity.T_Token>().Where(x => x.Token_expired_date <= DateTime.Now).ExecuteCommand();
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="login_Info"></param>
        /// <returns></returns>
        public bool Add(Entity.T_Token token)
        {
            return TokenDb.Insert(token);
        }

        /// <summary>
        /// 通过TokenID删除
        /// </summary>
        /// <param name="tokenId">TokenID</param>
        /// <returns></returns>
        public bool Delete(string tokenId)
        {
            return TokenDb.Delete(u => u.Token_id == tokenId);
        }

        /// <summary>
        /// 通过用户ID删除
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public bool Delete(int userId)
        {
            return TokenDb.Delete(u => u.Token_user_id == userId);
        }

        /// <summary>
        /// 通过用户ID获取已登录信息
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        public Entity.T_Token GetByUserId(int userId)
        {
            return TokenDb.GetSingle(u => u.Token_user_id == userId);
        }

        /// <summary>
        /// 返回相关的Token信息
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public Entity.T_Token GetToken(string tokenId)
        {
            return TokenDb.GetSingle(u => u.Token_id == tokenId);
        }

        /// <summary>
        /// 通过Token查找用户
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public Entity.T_User GetUserById(string tokenId)
        {
            var token = GetToken(tokenId);
            if (token != null && token.Token_expired_date > DateTime.Now)
            {
                return Db.Queryable<Entity.T_User>().Single(u => u.User_model_state && u.User_is_active && u.User_id == token.Token_user_id);
            }
            return null;
        }

        /// <summary>
        /// 更新登录信息
        /// </summary>
        /// <param name="login_Info"></param>
        /// <returns></returns>
        public bool Update(Entity.T_Token token)
        {
            return TokenDb.Update(token);
        }
    }
}