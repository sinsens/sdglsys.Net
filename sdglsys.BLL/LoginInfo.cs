using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdglsys.DbHelper
{
    /// <summary>
    /// 已登录用户信息
    /// </summary>
    public class LoginInfo : DbContext
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="login_Info"></param>
        /// <returns></returns>
        public bool Add(Entity.TLogin_Info login_Info) {
            return LoginInfoDb.Insert(login_Info);
        }

        /// <summary>
        /// 通过用户ID删除
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        public bool Delete(int uid) {
            return LoginInfoDb.Delete(u => u.Uid == uid);
        }

        /// <summary>
        /// 通过用户名删除
        /// </summary>
        /// <param name="login_name">用户名/登录名</param>
        /// <returns></returns>
        public bool Delete(string login_name) {
            return LoginInfoDb.Delete(u => u.Login_name == login_name);
        }

        /// <summary>
        /// 通过Session ID删除
        /// </summary>
        /// <param name="sid">Session ID</param>
        /// <returns></returns>
        public bool DeleteBySessionId(string sid)
        {
            return LoginInfoDb.Delete(u => u.Session_id == sid);
        }

        /*
        /// <summary>
        /// 通过Session ID获取已登录用户信息
        /// </summary>
        /// <param name="sid">Session ID</param>
        /// <returns></returns>
        public Entity.TUser GetUserBySessionId(string sid) {
            return UserDb.GetSingle(user=>user.Id == LoginInfoDb.GetSingle(u => u.Session_id == sid).Uid);
        }
        */

        /// <summary>
        /// 通过Session ID获取已登录信息
        /// </summary>
        /// <param name="sid">Session ID</param>
        /// <returns></returns>
        public Entity.TLogin_Info GetBySessionId(string sid)
        {
            return LoginInfoDb.GetSingle(u => u.Session_id == sid);
        }

        /// <summary>
        /// 通过用户ID获取已登录信息
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        public Entity.TLogin_Info GetByUserId(int uid)
        {
            return LoginInfoDb.GetSingle(u => u.Uid == uid);
        }

        /// <summary>
        /// 更新登录信息
        /// </summary>
        /// <param name="login_Info"></param>
        /// <returns></returns>
        public bool Update(Entity.TLogin_Info login_Info) {
            return LoginInfoDb.Update(login_Info);
        }
    }
}
