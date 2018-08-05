using System.Collections.Generic;

namespace sdglsys.DbHelper
{
    public class Users : DbContext
    {
        public List<Entity.TUser> getAll()
        {
            return Db.Queryable<Entity.TUser>().ToList();
        }

        /// <summary>
        /// 通过用户ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.TUser FindById(int id)
        {
            return Db.Queryable<Entity.TUser>().Where((u) => u.Id == id).First();
        }

        /// <summary>
        /// 通过login_name查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.TUser findByLoginName(string login_name)
        {
            return Db.Queryable<Entity.TUser>().Where(a => a.Login_name == login_name).First();
        }

        /// <summary>
        /// 通过login_name查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.VUser findVUserByLoginName(string login_name)
        {
            var user = Db.Queryable<Entity.TUser>().Where( u => u.Login_name == login_name).Select(u=>new Entity.VUser {
                 Pid = u.Pid, Id = u.Id, Nickname = u.Nickname,Login_name = u.Login_name, Is_active = u.Is_active, Note=u.Note,
                  Phone = u.Phone, Reg_date = u.Reg_date, Role = u.Role,
            }).First();
            if (user == null)
                return user;
            foreach (var item in Db.Queryable<Entity.TDorm>().ToList())
            {
                if (item.Id==user.Id)
                {
                    user.DormName = item.Nickname;
                };
            }
            user.RoleName = user.Role < 3 ? (user.Role < 2 ? "辅助登记员" : "宿舍管理员") : "系统管理员";
            return user;
        }

        public bool Delete(int id)
        {
            return UserDb.DeleteById(id);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Update(Entity.TUser user)
        {
            return UserDb.Update(user);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Add(Entity.TUser user)
        {
            return UserDb.Insert(user);
        }


        /// <summary>
        /// 查找角色
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Entity.TUser> getByPages(int page, int limit, ref int totalCount, string where=null)
        {
            if (where == null) {
                return Db.Queryable<Entity.TUser>().ToPageList(page, limit, ref totalCount);
            }
            return Db.Queryable<Entity.TUser>().Where(u=>u.Nickname.Contains(where)||
            u.Login_name.Contains(where)||u.Phone.Contains(where)||u.Note.Contains(where)).ToPageList(page, limit, ref totalCount);
        }

        /// <summary>
        /// 查找角色
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="pid">园区ID</param>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Entity.TUser> getByPages(int page, int limit, ref int totalCount,int pid, string where=null)
        {
            if (where == null)
            {
                return Db.Queryable<Entity.TUser>().Where(u => u.Pid == pid).ToPageList(page, limit, ref totalCount);
            }
            return Db.Queryable<Entity.TUser>().Where(u => u.Pid == pid && (u.Nickname.Contains(where) ||
            u.Login_name.Contains(where) || u.Phone.Contains(where) || u.Note.Contains(where))).ToPageList(page, limit, ref totalCount);
        }
    }
}
