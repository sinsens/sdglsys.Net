using sdglsys.Entity;
using SqlSugar;
using System.Collections.Generic;

namespace sdglsys.DbHelper
{
    public class Buildings : DbContext
    {
        public List<Entity.VBuilding> getAll()
        {
            return Db.Queryable<sdglsys.Entity.TBuilding, Entity.TDorm>((b, d) => new object[] { JoinType.Left, b.Pid == d.Id }).Select((b, d) => new VBuilding { Id = b.Id, Pid = b.Pid, Vid = b.Vid, Nickname = b.Nickname, Note = b.Note, PNickname = d.Nickname, Is_active = b.Is_active }).ToList();
        }

        /// <summary>
        /// 获取已启用的宿舍楼
        /// </summary>
        /// <returns></returns>
        public List<Entity.VBuilding> getAllActive()
        {
            return Db.Queryable<sdglsys.Entity.TBuilding, Entity.TDorm>((b, d) => new object[] { JoinType.Left, b.Pid == d.Id }).
                  Where((b,d)=>b.Is_active==true).Select((b, d) => new VBuilding { Id = b.Id, Pid = b.Pid, Vid = b.Vid, Nickname = b.Nickname, Note = b.Note, PNickname = d.Nickname, Is_active = b.Is_active }).ToList();
        }

        /// <summary>
        /// 获取已启用的宿舍楼
        /// </summary>
        /// <returns></returns>
        public List<Entity.VBuilding> getAllActiveById(int pid)
        {
            return Db.Queryable<sdglsys.Entity.TBuilding, Entity.TDorm>((b, d) => new object[] { JoinType.Left, b.Pid == d.Id }).
                  Where((b, d) => b.Is_active == true && b.Pid == pid).Select((b, d) => new VBuilding { Id = b.Id, Pid = b.Pid, Vid = b.Vid, Nickname = b.Nickname, Note = b.Note, PNickname = d.Nickname, Is_active = b.Is_active }).ToList();
        }

        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.TBuilding findById(int id)
        {
            return BuildingDb.GetById(id);
        }

        /// <summary>
        /// 通过ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            return BuildingDb.DeleteById(id);
        }

        /// <summary>
        /// 更新宿舍楼信息
        /// </summary>
        /// <param name="Building"></param>
        /// <returns></returns>
        public bool Update(Entity.TBuilding Building)
        {
            return BuildingDb.Update(Building);
        }

        /// <summary>
        /// 添加宿舍楼信息
        /// </summary>
        /// <param name="Building"></param>
        /// <returns></returns>
        public bool Add(Entity.TBuilding Building)
        {
            return BuildingDb.Insert(Building);
        }

        /// <summary>
        /// 查找宿舍楼
        /// </summary>
        /// <param name="page">当前页数</param>
        /// <param name="limit">每页数量</param>
        /// <param name="totalCount">当前页结果数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<VBuilding> getByPages(int page, int limit, ref int totalCount, string where=null)
        {
            if (where == null)
                return Db.Queryable<sdglsys.Entity.TBuilding, Entity.TDorm>((b, d) => new object[] { JoinType.Left, b.Pid == d.Id}).
                  Select((b, d) => new VBuilding{ Id=b.Id,Pid=b.Pid,Vid=b.Vid, Nickname = b.Nickname,Note = b.Note,PNickname = d.Nickname, Is_active = b.Is_active }).ToPageList(page, limit, ref totalCount);
            return Db.Queryable<sdglsys.Entity.TBuilding, Entity.TDorm>((b, d) => new object[] { JoinType.Left, b.Pid == d.Id }).Where((b,d)=> b.Vid.Contains(where)||b.Nickname.Contains(where)||b.Note.Contains(where)).OrderBy((b, d) => b.Id, OrderByType.Desc).
                  Select((b, d) => new VBuilding { Id = b.Id, Pid = b.Pid, Vid = b.Vid, Nickname = b.Nickname, Note = b.Note, PNickname = d.Nickname, Is_active = b.Is_active }).ToPageList(page, limit, ref totalCount);
            //return Db.Queryable<Entity.Building>().Where((b) => b.Nickname.Contains(where) || b.Note.Contains(where)).ToPageList(page, limit, ref totalCount);
        }
    }
}
