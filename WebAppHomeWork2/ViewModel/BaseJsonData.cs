using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppHomeWork.ViewModel
{
    public class BaseJsonData<T>
    {
        public int draw { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public int iTotalRecords { get; set; }
        /// <summary>
        /// 结果总数
        /// </summary>
        public int iTotalDisplayRecords { get; set; }
        /// <summary>
        /// 分页查询的结果
        /// </summary>
        public List<T> aaData { get; set; }
    }
}