using System.Collections.Generic;

namespace sdglsys.BLL
{
    public class Users : DbContext
    {
        public List<Entity.Users> getAll()
        {
            return Db.Queryable<Entity.Users>().ToList();
        }

        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.Users findById(int id)
        {
            return UserDb.GetById(id);
        }

        /// <summary>
        /// 通过login_name查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.Users findByLoginName(string login_name)
        {
            return Db.Queryable<Entity.Users>().Where(a => a.Login_name == login_name).First();
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
        public bool Update(Entity.Users user)
        {
            return UserDb.Update(user);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Add(Entity.Users user)
        {
            return UserDb.Insert(user);
        }


        /// <summary>
        /// 查找角色
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Entity.Users> getByPages(int pageIndex, int pageSize, ref int totalCount, string where=null)
        {
            if (where == null) {
                return Db.Queryable<Entity.Users>().ToPageList(pageIndex, pageSize, ref totalCount);
            }
            return Db.Queryable<Entity.Users>().Where(u=>u.Nickname.Contains(where)||
            u.Login_name.Contains(where)||u.Phone.Contains(where)||u.Note.Contains(where)).ToPageList(pageIndex, pageSize, ref totalCount);
        }

        /// <summary>
        /// 查找角色
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pid">园区ID</param>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Entity.Users> getByPages(int pageIndex, int pageSize, ref int totalCount,int pid, string where=null)
        {
            if (where == null)
            {
                return Db.Queryable<Entity.Users>().Where(u => u.Pid == pid).ToPageList(pageIndex, pageSize, ref totalCount);
            }
            return Db.Queryable<Entity.Users>().Where(u => u.Pid == pid && (u.Nickname.Contains(where) ||
            u.Login_name.Contains(where) || u.Phone.Contains(where) || u.Note.Contains(where))).ToPageList(pageIndex, pageSize, ref totalCount);
        }
    }
}
