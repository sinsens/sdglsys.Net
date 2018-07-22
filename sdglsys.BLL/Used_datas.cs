using sdglsys.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdglsys.DbHelper
{
    /// <summary>
    /// 输出图表需要的数据
    /// </summary>
    public class Used_datas
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string info { get; set; }

        public Data data = new Data();

        /// <summary>
        /// 加入一个列表
        /// </summary>
        /// <param name="used_Data"></param>
        public void Add(List<used_data> used_Data) {
            data.AddRange(used_Data);
        }

        /// <summary>
        /// 加入一个对象
        /// </summary>
        /// <param name="used_Data"></param>
        public void Add(used_data used_Data)
        {
            data.Add(used_Data);
        }

        /// <summary>
        /// 返回Json字符串
        /// </summary>
        /// <returns></returns>
        public string ToJson() {
            return Utils.ToJson(this);
        }
    }

    public class Data {
        /// <summary>
        /// 日期
        /// </summary>
        public List<string> Date { get; set; }
        /// <summary>
        /// 用电量
        /// </summary>
        public List<float> Electric_value { get; set; }
        /// <summary>
        /// 冷水用量
        /// </summary>
        public List<float> Cold_water_value { get; set; }
        /// <summary>
        /// 热水用量
        /// </summary>
        public List<float> Hot_water_value { get; set; }

        public Data() {
            this.Date = new List<string>();
            this.Cold_water_value = new List<float>();
            this.Electric_value = new List<float>();
            this.Hot_water_value = new List<float>();
        }

        public void Add(used_data used_Data) {
            Date.Add(used_Data.Date);
            Electric_value.Add(used_Data.Electric_value);
            Cold_water_value.Add(used_Data.Cold_water_value);
            Hot_water_value.Add(used_Data.Hot_water_value);
        }


        public void AddRange(List<used_data> used_Data)
        {
            foreach (var item in used_Data)
            {
                Add(item);
            }
            
        }
    }

    /// <summary>
    /// 用量数据
    /// </summary>
    public class used_data
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 用电量
        /// </summary>
        public float Electric_value { get; set; }
        /// <summary>
        /// 冷水用量
        /// </summary>
        public float Cold_water_value { get; set; }
        /// <summary>
        /// 热水用量
        /// </summary>
        public float Hot_water_value { get; set; }
    }
}
