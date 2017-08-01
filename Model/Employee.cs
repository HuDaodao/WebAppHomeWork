using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Model
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("姓名")]
        [Required(ErrorMessage = "员工姓名必填")]
        [StringLength(50, ErrorMessage = "不得超过50个字")]
        public string EmployeeName { get; set; }

        [DisplayName("密码")]
        [Required(ErrorMessage = "密码必填")]
        [StringLength(50, ErrorMessage = "不得超过50个字")]
        [MinLength(3, ErrorMessage="不得少于三个字")]
        public string PassWord { get; set; }

        [DisplayName("年龄")]
        [Range(15, 110, ErrorMessage = "年龄必须在15~110之间")]
        public int Age { get; set; }

        [DisplayName("邮箱")]
        [EmailAddress(ErrorMessage = "请输入有效的邮箱")]
        [MaxLength(100,ErrorMessage = "不得超过100个字")]
        public string Email { get; set; }

        [DisplayName("是否是管理员")]
        public bool IsAdmin { get; set; }

        public virtual int? DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        
    }
}