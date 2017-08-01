using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30,ErrorMessage = "长度不得超过30个字")]
        [DisplayName("部门名称")]
        public string Name { get; set; }

        public int ParentId { get; set; }

        [MaxLength(1000,ErrorMessage = "不得超过1000字")]
        [DisplayName("备注")]
        public string Note { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
