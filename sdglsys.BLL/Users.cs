using sdglsys.Entity;
using SqlSugar;
using System.Collections.Generic;

namespace sdglsys.DbHelper
{
    public class Users : DbContext
    {

        /// <summary>
        /// 获取一个系统管理员权限的角色
        /// </summary>
        /// <returns></returns>
        public Entity.T_User GetAdminUser()
        {
            return Db.Queryable<Entity.T_User>().Single(u => u.User_model_state && u.User_is_active && u.User_role == 3);
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="login_name">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public Entity.T_User Login(string login_name, string pwd)
        {
            var user = FindByLoginName(login_name);
            if (user == null)
            {
                return null;
            }
            if (!user.User_is_active)
            {
                return null;
            }
            return (new Utils.Utils().CheckPasswd(pwd, user.User_pwd)) ? user : null;
        }

        /// <summary>
        /// 通过用户ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.T_User FindById(int id)
        {
            return Db.Queryable<Entity.T_User>().Where((u) => u.User_id == id && u.User_model_state).First();
        }

        /// <summary>
        /// 通过login_name查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.T_User FindByLoginName(string login_name)
        {
            return Db.Queryable<Entity.T_User>().Where(a => a.User_model_state && a.User_login_name == login_name).First();
        }

        /// <summary>
        /// 通过login_name查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.VUser findVUserByLoginName(string login_name)
        {
            var user = Db.Queryable<Entity.T_User>().Where(u => u.User_model_state && u.User_login_name == login_name).Select<VUser>().Single();
            if (user == null)
                return user;
            foreach (var item in new Dorms().GetAllActive())
            {
                if (item.Dorm_id == user.User_Id)
                {
                    user.User_Dorm_Nickname = item.Dorm_nickname;
                    break;
                };
            }
            user.User_RoleName = user.User_Role < 3 ? (user.User_Role < 2 ? "辅助登记员" : "宿舍管理员") : "系统管理员";
            return user;
        }

        /// <summary>
        /// 删除系统用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            var user = FindById(id);
            if (user != null)
            {
                user.User_model_state = false;
                return Update(user);
            }
            return false;
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Update(Entity.T_User user)
        {
            return UserDb.Update(user);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Add(Entity.T_User user)
        {
            return UserDb.Insert(user);
        }


        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="pid">园区ID</param>
        /// <returns></returns>
        public List<Entity.VUser> GetByPages(int page, int limit, ref int totalCount, string where = null, int pid = 0)
        {
            var sql = Db.Queryable<Entity.T_User, Entity.T_Dorm>((u, d) => new object[]{
                JoinType.Left,u.User_dorm_id==d.Dorm_id
            }).Where((u, d) => u.User_model_state);
            if (pid != 0)
            {
                sql = sql.Where(x => x.User_dorm_id == pid);
            }
            if (!string.IsNullOrWhiteSpace(where))
            {
                sql = sql.Where(u => u.User_model_state && (u.User_nickname.Contains(where) ||
            u.User_login_name.Contains(where) || u.User_phone.Contains(where) || u.User_note.Contains(where)));
            }
            return sql.Select((u, d) => new Entity.VUser
            {
                User_Dorm_Nickname = d.Dorm_nickname,
                User_Id = u.User_id,
                User_Is_active = u.User_is_active,
                User_Nickname = u.User_nickname,
                User_Note = u.User_note,
                User_Role = u.User_role,
                User_Login_name = u.User_login_name,
                User_Reg_date = u.User_reg_date,
            }).ToPageList(page, limit, ref totalCount);
        }

    }
}
