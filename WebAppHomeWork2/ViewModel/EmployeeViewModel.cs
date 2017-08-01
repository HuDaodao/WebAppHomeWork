using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppHomeWork.ViewModel
{
    public class EmployeeViewModel:BaseViewModel
    {
        public int Id { get; set; }

        [DisplayName("姓名")]
        [Required(ErrorMessage = "员工姓名必填")]
        [StringLength(50, ErrorMessage = "不得超过50个字")]
        public string EmployeeName { get; set; }

        [DisplayName("密码")]
        [Required(ErrorMessage = "密码必填")]
        [StringLength(50, ErrorMessage = "不得超过50个字")]
        [MinLength(3, ErrorMessage = "不得少于三个字")]
        public string PassWord { get; set; }

        [DisplayName("年龄")]
        [Range(15, 110, ErrorMessage = "年龄必须在15~110之间")]
        public int Age { get; set; }

        [DisplayName("邮箱")]
        [EmailAddress(ErrorMessage = "请输入有效的邮箱")]
        [MaxLength(100, ErrorMessage = "不得超过100个字")]
        public string Email { get; set; }

        [DisplayName("是否是管理员")]
        public string IsAdmin{ get;set; }

        [MaxLength(30, ErrorMessage = "长度不得超过30个字")]
        [DisplayName("部门名称")]
        public string DepartName { get; set; }

        [DisplayName("部门")]
        public int? DepartmentId { get; set; }
    }
}